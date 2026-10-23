using System;
using System.Collections.Generic;
using System.Data;
using LMCIS.controller.Utilities;

namespace LMCIS.controller
{
    public class addPartToOrderController : abstractController
    {
        public addPartToOrderController(IDatabase database = null)
        {
            _db = database ?? new Database();
        }

        public DataTable GetPartDetail(string partNum)
        {
            string sqlCmd =
                "SELECT w.name, w.partNumber, w.categoryID, x.type, y.name, y.country, z.price, z.onSaleQty, z.LM_onSaleQty " +
                "FROM spare_part w, category x, supplier y, product z " +
                "WHERE w.partNumber = z.partNumber AND w.categoryID = x.categoryID AND w.supplierID = y.supplierID AND w.partNumber = @partNum";
            return _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object>
            {
                { "@partNum", partNum }
            });
        }

        public DataTable GetEditableOrderId(string id) //customer id
        {
            string customerAccountID = GetCustomerAccountId(id);
            string sqlCmd =
                "SELECT orderID FROM order_ WHERE customerAccountID = @customerAccountID AND (status = 'Pending' OR status = 'Processing')";
            return _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object>
            {
                { "@customerAccountID", customerAccountID }
            });
        }

        public string GetShippingDate(string id) //order id
        {
            string sqlCmd = "SELECT shippingDate FROM shipping_detail WHERE orderID = @orderID";
            return _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object>
            {
                { "@orderID", id }
            }).Rows[0][0].ToString();
        }

        public string GetOrderStatus(string id) //order id
        {
            string sqlCmd = "SELECT status FROM order_ WHERE orderID = @orderID";
            return _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object>
            {
                { "@orderID", id }
            }).Rows[0][0].ToString();
        }

        public DataTable GetShippingDetail(string id) //orderID
        {
            //orderID
            string sqlCmd = "SELECT * FROM shipping_detail WHERE orderID = @orderID";
            return _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object>
            {
                { "@orderID", id }
            });
        }

        public string GetCustomerAccountId(string id) //id = customerID
        {
            string sqlCmd = "SELECT customerAccountID FROM customer_account WHERE customerID = @customerID";
            return _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object>
            {
                { "@customerID", id }
            }).Rows[0][0].ToString();
        }

        public int getNumberOfPartAlreadyInOrder(string orderId, string partNum)
        {
            string sqlCmd =
                "SELECT quantity FROM order_line WHERE partNumber = @partNum AND orderID = @orderID";
            try
            {
                return int.Parse(_db.ExecuteDataTable(sqlCmd, new Dictionary<string, object>
                {
                    { "@partNum", partNum },
                    { "@orderID", orderId }
                }).Rows[0][0].ToString());
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool addToOrder(string orderId, string partNum, int qty, int price, bool isLM)
        {
            if (qty <= 0) return false;

            try
            {
                bool added = false;
                _db.ExecuteInTransaction(() =>
                {
                    DataTable order = _db.ExecuteDataTable(
                        "SELECT status FROM order_ WHERE orderID = @orderID FOR UPDATE",
                        new Dictionary<string, object> { { "@orderID", orderId } });
                    if (order.Rows.Count != 1 ||
                        (!string.Equals(order.Rows[0]["status"].ToString(), "Pending", StringComparison.OrdinalIgnoreCase) &&
                         !string.Equals(order.Rows[0]["status"].ToString(), "Processing", StringComparison.OrdinalIgnoreCase)))
                        return;

                    string inventoryColumn = isLM ? "LM_onSaleQty" : "onSaleQty";
                    DataTable inventory = _db.ExecuteDataTable(
                        $"SELECT {inventoryColumn} FROM product WHERE partNumber = @partNum FOR UPDATE",
                        new Dictionary<string, object> { { "@partNum", partNum } });
                    if (inventory.Rows.Count != 1 || Convert.ToInt32(inventory.Rows[0][0]) < qty) return;

                    DataTable existingLine = _db.ExecuteDataTable(
                        "SELECT quantity FROM order_line WHERE partNumber = @partNum AND orderID = @orderID FOR UPDATE",
                        new Dictionary<string, object>
                        {
                            { "@partNum", partNum },
                            { "@orderID", orderId }
                        });
                    if (existingLine.Rows.Count > 1)
                        throw new InvalidOperationException("An order contains duplicate lines for this part.");

                    if (existingLine.Rows.Count == 1)
                    {
                        int newQuantity = checked(Convert.ToInt32(existingLine.Rows[0][0]) + qty);
                        _db.ExecuteNonQueryCommand(
                            "UPDATE order_line SET quantity = @qty WHERE partNumber = @partNum AND orderID = @orderID",
                            new Dictionary<string, object>
                            {
                                { "@qty", newQuantity },
                                { "@partNum", partNum },
                                { "@orderID", orderId }
                            });
                    }
                    else
                    {
                        _db.ExecuteNonQueryCommand(
                            "INSERT INTO order_line VALUES (@partNum, @orderID, @qty, @orderUnitPrice)",
                            new Dictionary<string, object>
                            {
                                { "@partNum", partNum },
                                { "@orderID", orderId },
                                { "@qty", qty },
                                { "@orderUnitPrice", price }
                            });
                    }

                    string updateInventory =
                        $"UPDATE product SET {inventoryColumn} = {inventoryColumn} - @qty " +
                        $"WHERE partNumber = @partNum AND {inventoryColumn} >= @qty";
                    object affectedRows = _db.ExecuteCommand(updateInventory,
                        new Dictionary<string, object> { { "@qty", qty }, { "@partNum", partNum } },
                        command => command.ExecuteNonQuery());
                    if (Convert.ToInt32(affectedRows) != 1)
                        throw new InvalidOperationException("On-sale inventory changed while adding the order line.");

                    added = true;
                });

                return added;
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "addPartToOrderController");
                return false;
            }
        }

        public int getOnSaleQty(string partNum, bool isLM)
        {
            string sqlCmd = "";
            if (!isLM)
            {
                sqlCmd = "SELECT onSaleQty FROM product WHERE partNumber = @partNum";
            }
            else
            {
                sqlCmd = "SELECT LM_onSaleQty FROM product WHERE partNumber = @partNum";
            }

            try
            {
                return int.Parse(_db.ExecuteDataTable(sqlCmd, new Dictionary<string, object>
                {
                    { "@partNum", partNum }
                }).Rows[0][0].ToString());
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public bool deductOnSalesQty(string partNum, int qty, bool isLM)
        {
            if (qty <= 0) return false;
            try
            {
                string inventoryColumn = isLM ? "LM_onSaleQty" : "onSaleQty";
                string sqlCmd =
                    $"UPDATE product SET {inventoryColumn} = {inventoryColumn} - @qty " +
                    $"WHERE partNumber = @partNum AND {inventoryColumn} >= @qty";
                object affectedRows = _db.ExecuteCommand(sqlCmd,
                    new Dictionary<string, object> { { "@qty", qty }, { "@partNum", partNum } },
                    command => command.ExecuteNonQuery());
                return Convert.ToInt32(affectedRows) == 1;
            }
            catch (Exception ex)
            {
                Log.LogException(ex, "addPartToOrderController");
                return false;
            }
        }

        public DataTable getSparePartList()
        {
            string sqlCmd =
                "SELECT * FROM spare_part WHERE status = 'Enable'";
            return _db.ExecuteDataTable(sqlCmd);
        }
    }
}
