using System;
using System.Collections.Generic;
using System.Data;
using LMCIS.controller.Utilities;

namespace LMCIS.controller
{
    public class addPartToOrderController : abstractController
    {
        private readonly Database _db;

        public addPartToOrderController(Database database = null)
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
            int qtyAlreadyInOrder = getNumberOfPartAlreadyInOrder(orderId, partNum);
            if (qtyAlreadyInOrder != 0) //already in order
            {
                int newQty = qtyAlreadyInOrder + qty;
                try
                {
                    string sqlCmd =
                        "UPDATE order_line SET quantity = @qty WHERE partNumber = @partNum AND orderID = @orderID";
                    Dictionary<string, object> parameters = new Dictionary<string, object>
                    {
                        { "@qty", newQty },
                        { "@partNum", partNum },
                        { "@orderID", orderId }
                    };
                    _db.ExecuteNonQueryCommand(sqlCmd, parameters);
                    if (deductOnSalesQty(partNum, qty, isLM))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Log.LogException(ex, "addPartToOrderController");
                    return false;
                }
            }
            else //not already in order
            {
                try
                {
                    string sqlCmd = "INSERT INTO order_line VALUES (@partNum, @orderID, @qty, @orderUnitPrice)";
                    Dictionary<string, object> parameters = new Dictionary<string, object>
                    {
                        { "@partNum", partNum },
                        { "@orderID", orderId },
                        { "@qty", qty },
                        { "@orderUnitPrice", price }
                    };
                    _db.ExecuteNonQueryCommand(sqlCmd, parameters);
                    if (deductOnSalesQty(partNum, qty, isLM))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Log.LogException(ex, "addPartToOrderController");
                    return false;
                }
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
            int onSaleQty = getOnSaleQty(partNum, isLM);
            int newOnSaleQty = onSaleQty - qty;


            try
            {
                string sqlCmd = "";
                if (!isLM)
                {
                    sqlCmd = "UPDATE product SET onSaleQty = @qty WHERE partNumber = @partNum";
                }
                else
                {
                    sqlCmd = "UPDATE product SET LM_onSaleQty = @qty WHERE partNumber = @partNum";
                }

                Dictionary<string, object> parameters = new Dictionary<string, object>
                {
                    { "@qty", newOnSaleQty },
                    { "@partNum", partNum }
                };
                _db.ExecuteNonQueryCommand(sqlCmd, parameters);
                return true;
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
