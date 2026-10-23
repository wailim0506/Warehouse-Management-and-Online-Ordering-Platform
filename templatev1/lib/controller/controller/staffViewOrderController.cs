using System;
using System.Collections.Generic;
using System.Data;
using LMCIS.controller.Utilities;
using Microsoft.Extensions.Logging;

namespace LMCIS.controller
{
    public class staffViewOrderController : abstractController
    {
        string sqlCmd;

        public staffViewOrderController(IDatabase database = null)
        {
            sqlCmd = "";
            _db = database ?? new Database();
        }

        public DataTable getOrder(string id) //orderID
        {
            string sqlCmd = "SELECT * FROM order_ WHERE orderID = @orderID";
            DataTable dt = new DataTable();
            dt = _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object> { { "@orderID", id } });
            return dt;
        }

        public string getStafftID(string id) //staff account id
        {
            AccountController ac = new AccountController();
            DataTable dt = ac.GetStaffDetail(id);
            return dt.Rows[0][0].ToString();
        }

        public string getStaffName(string id) //staff account id
        {
            AccountController ac = new AccountController();
            DataTable dt = ac.GetStaffDetail(id);
            return $"{dt.Rows[0][2].ToString()} {dt.Rows[0][3].ToString()}";
        }

        public string getStaffContact(string id)
        {
            AccountController ac = new AccountController();
            DataTable dt = ac.GetStaffDetail(id);
            return dt.Rows[0][6].ToString();
        }

        public DataTable getOrderedSparePart(string id, string sortBy) //orderID
        {
            switch (sortBy)
            {
                case "None":
                    sqlCmd = "SELECT *,(quantity*orderUnitPrice)FROM order_line WHERE orderID = @orderID";
                    break;
                case "Quantity(Ascending)":
                    sqlCmd =
                        "SELECT *,(quantity*orderUnitPrice) FROM order_line WHERE orderID = @orderID ORDER BY quantity";
                    break;
                case "Quantity(Descending)":
                    sqlCmd =
                        "SELECT *,(quantity*orderUnitPrice) FROM order_line WHERE orderID = @orderID ORDER BY quantity DESC";
                    break;
                case "Total Price(Ascending)":
                    sqlCmd =
                        "SELECT *,(quantity*orderUnitPrice) FROM order_line WHERE orderID = @orderID ORDER BY (quantity*orderUnitPrice)";
                    break;
                case "Total Price(Descending)":
                    sqlCmd =
                        "SELECT *,(quantity*orderUnitPrice) FROM order_line WHERE orderID = @orderID ORDER BY (quantity*orderUnitPrice) DESC";
                    break;
            }

            DataTable dt = new DataTable();
            dt = _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object> { { "@orderID", id } });
            return dt;
        }


        public string getItemNum(string id) //part number
        {
            string query = "SELECT itemID FROM product WHERE partNumber = @partNumber";
            DataTable dt = new DataTable();
            dt = _db.ExecuteDataTable(query, new Dictionary<string, object> { { "@partNumber", id } });
            return dt.Rows[0][0].ToString();
        }

        public string getPartName(string id)
        {
            DataTable dt = new DataTable();
            string query = "SELECT name FROM spare_part WHERE partNumber = @partNumber";
            dt = _db.ExecuteDataTable(query, new Dictionary<string, object> { { "@partNumber", id } });
            return dt.Rows[0][0].ToString();
        }

        public DataTable GetShippingDetail(string id) //orderID
        {
            //orderID
            DataTable dt = new DataTable();
            string query = "SELECT * FROM shipping_detail WHERE orderID = @orderID";
            dt = _db.ExecuteDataTable(query, new Dictionary<string, object> { { "@orderID", id } });
            return dt;
        }

        public string[] GetDelivermanDetail(string id) //orderID
        {
            string delivermanID = ExecuteSqlQueryAndReturnFirstRow(
                "SELECT delivermanID FROM shipping_detail WHERE orderID = @orderID",
                new Dictionary<string, object> { { "@orderID", id } });

            //get deliverman name and contact from staff table
            DataTable dt = _db.ExecuteDataTable(
                "SELECT firstName, lastName, phoneNumber FROM staff WHERE delivermanID = @delivermanID",
                new Dictionary<string, object> { { "@delivermanID", delivermanID } });

            return new string[] { dt.Rows[0][0].ToString(), dt.Rows[0][1].ToString(), dt.Rows[0][2].ToString() };
        }

        public Dictionary<string, int> GetPartNumWithQty(string id) //order id
        {
            DataTable dt = new DataTable();
            string sqlCmd = "SELECT partNumber, quantity FROM order_line WHERE orderID = @orderID";
            dt = _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object> { { "@orderID", id } });

            Dictionary<string, int> partNumQty = new Dictionary<string, int>();

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                partNumQty.Add($"{dt.Rows[i][0]}", int.Parse(dt.Rows[i][1].ToString()));
            }

            return partNumQty;
        }

        public void addQtyback(string partNum, int qtyInOrder, string id) //order id
        {
            editOrderController c = new editOrderController();
            c.AddQtyBackToSparePart(partNum, id, qtyInOrder);
        }

        public bool DeleteOrder(string id) //order id
        {
            try
            {
                _db.ExecuteNonQueryCommand("DELETE FROM invoice WHERE orderID = @id",
                    new Dictionary<string, object> { { "@id", id } });
            }
            catch (Exception ex)
            {
                Log.LogMessage(LogLevel.Error, "order controller", $"Error deleting order from invoice: {ex.Message}");
                return false;
            }

            try
            {
                _db.ExecuteNonQueryCommand("DELETE FROM feedback WHERE orderID = @id",
                    new Dictionary<string, object> { { "@id", id } });
            }
            catch (Exception ex)
            {
                Log.LogMessage(LogLevel.Error, "order controller", $"Error deleting order from feedback: {ex.Message}");
                return false;
            }

            try
            {
                _db.ExecuteNonQueryCommand(
                    "UPDATE shipping_detail SET remark = 'Cancelled' WHERE orderID = @id",
                    new Dictionary<string, object> { { "@id", id } });
            }
            catch (Exception ex)
            {
                Log.LogMessage(LogLevel.Error, "order controller", $"Error updating shipping detail: {ex.Message}");
                return false;
            }

            try
            {
                _db.ExecuteNonQueryCommand("DELETE FROM instruction WHERE orderID = @id",
                    new Dictionary<string, object> { { "@id", id } });
            }
            catch (Exception ex)
            {
                Log.LogMessage(LogLevel.Error, "order controller",
                    $"Error deleting order from instruction: {ex.Message}");
                return false;
            }

            try
            {
                _db.ExecuteNonQueryCommand("UPDATE order_ SET status = @status WHERE orderID = @id",
                    new Dictionary<string, object> { { "@status", "Cancelled" }, { "@id", id } });
            }
            catch (Exception ex)
            {
                Log.LogMessage(LogLevel.Error, "order controller", $"Error updating order status: {ex.Message}");
                return false;
            }

            return true;
        }

        private string ExecuteSqlQueryAndReturnFirstRow(string query, Dictionary<string, object> parameters = null)
        {
            DataTable dt = _db.ExecuteDataTable(query, parameters);
            return dt.Rows[0][0].ToString();
        }

        //Chenge order line status whether is 
        private bool CheckStatus(string orderID)
        {
            DataTable dt = _db.ExecuteDataTable(
                "SELECT status FROM order_ WHERE orderID = @orderID FOR UPDATE",
                new Dictionary<string, object> { { "@orderID", orderID } });

            return dt.Rows.Count == 1 && dt.Rows[0]["status"].ToString().Equals("Processing", StringComparison.OrdinalIgnoreCase);
        }

        //Storeman change order status to ready to ship.
        public bool ReadyToShipANDDeductQty(string orderID)
        {
            try
            {
                bool succeeded = false;
                _db.ExecuteInTransaction(() =>
                {
                    if (!CheckStatus(orderID)) return;

                    DataTable lines = _db.ExecuteDataTable(
                        "SELECT OL.partNumber, OL.quantity, SP.quantity AS availableQuantity " +
                        "FROM order_line OL LEFT JOIN spare_part SP ON SP.partNumber = OL.partNumber " +
                        "WHERE OL.orderID = @orderID ORDER BY OL.partNumber FOR UPDATE",
                        new Dictionary<string, object> { { "@orderID", orderID } });
                    if (lines.Rows.Count == 0) return;

                    foreach (DataRow line in lines.Rows)
                    {
                        if (line.IsNull("availableQuantity")) return;
                        int quantity = Convert.ToInt32(line["quantity"]);
                        int available = Convert.ToInt32(line["availableQuantity"]);
                        if (quantity <= 0 || available < quantity) return;
                    }

                    foreach (DataRow line in lines.Rows)
                    {
                        int quantity = Convert.ToInt32(line["quantity"]);
                        _db.ExecuteNonQueryCommand(
                            "UPDATE spare_part SET quantity = quantity - @quantity " +
                            "WHERE partNumber = @partNumber AND quantity >= @quantity",
                            new Dictionary<string, object>
                            {
                                { "@quantity", quantity },
                                { "@partNumber", line["partNumber"].ToString() }
                            });
                    }

                    _db.ExecuteNonQueryCommand(
                        "UPDATE order_ SET status = 'Ready to Ship' WHERE orderID = @orderID AND status = 'Processing'",
                        new Dictionary<string, object> { { "@orderID", orderID } });
                    succeeded = true;
                });

                return succeeded;
            }
            catch (Exception ex)
            {
                Log.LogMessage(LogLevel.Error, "staff view order controller",
                    $"Error changing order status: {ex.Message}");
                return false;
            }
        }


        public bool confirmOrder(string orderID)
        {
            try
            {
                _db.ExecuteNonQueryCommand(
                    "UPDATE order_ SET status = @status WHERE orderID = @id",
                    new Dictionary<string, object> { { "@status", "Processing" }, { "@id", orderID } });
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }
    }
}
