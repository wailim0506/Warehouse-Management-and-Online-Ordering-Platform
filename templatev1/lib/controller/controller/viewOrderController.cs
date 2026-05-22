using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using LMCIS.controller.Utilities;
using Microsoft.Extensions.Logging;

namespace LMCIS.controller
{
    public class viewOrderController : abstractController
    {
        string _sqlCmd;
        Database _db;

        public viewOrderController(Database database = null)
        {
            _sqlCmd = "";
            _db = database ?? new Database();
        }

        public DataTable GetOrder(string id) //orderID
        {
            string sqlCmd = "SELECT * FROM order_ WHERE orderID = @orderID";
            DataTable dt = new DataTable();
            dt = _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object> { { "@orderID", id } });
            return dt;
        }

        public static string GetStafftId(string id) //staff account id
        {
            AccountController ac = new AccountController();
            DataTable dt = ac.GetStaffDetail(id);
            return dt.Rows[0][0].ToString();
        }

        public string GetStaffName(string id) //staff account id
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
                    _sqlCmd = "SELECT *,(quantity*orderUnitPrice)FROM order_line WHERE orderID = @orderID";
                    break;
                case "Quantity(Ascending)":
                    _sqlCmd =
                        "SELECT *,(quantity*orderUnitPrice) FROM order_line WHERE orderID = @orderID ORDER BY quantity";
                    break;
                case "Quantity(Descending)":
                    _sqlCmd =
                        "SELECT *,(quantity*orderUnitPrice) FROM order_line WHERE orderID = @orderID ORDER BY quantity DESC";
                    break;
                case "Total Price(Ascending)":
                    _sqlCmd =
                        "SELECT *,(quantity*orderUnitPrice) FROM order_line WHERE orderID = @orderID ORDER BY (quantity*orderUnitPrice)";
                    break;
                case "Total Price(Descending)":
                    _sqlCmd =
                        "SELECT *,(quantity*orderUnitPrice) FROM order_line WHERE orderID = @orderID ORDER BY (quantity*orderUnitPrice) DESC";
                    break;
            }

            DataTable dt = new DataTable();
            dt = _db.ExecuteDataTable(_sqlCmd, new Dictionary<string, object> { { "@orderID", id } });
            return dt;
        }


        public string GetItemNum(string id) //part number
        {
            string query = "SELECT itemID FROM product WHERE partNumber = @partNumber";
            DataTable dt = new DataTable();
            dt = _db.ExecuteDataTable(query, new Dictionary<string, object> { { "@partNumber", id } });
            return dt.Rows[0][0].ToString();
        }

        public string GetPartName(string id)
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

        private string ExecuteSqlQueryAndReturnFirstRow(string query, Dictionary<string, object> parameters = null)
        {
            DataTable dt = _db.ExecuteDataTable(query, parameters);
            return dt.Rows[0][0].ToString();
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

        public async Task<bool> AddBackToSparePartQty(string num, int qtyInOrder)
        {
            //get the qty in db first
            int qtyInSpare_Part = GetSpareQtyInDb(num);

            //add db qty with cart qty
            qtyInSpare_Part += qtyInOrder;

            try
            {
                _db.ExecuteNonQueryCommand("UPDATE spare_part SET quantity = @qty WHERE partNumber = @num",
                    new Dictionary<string, object> { { "@qty", qtyInSpare_Part }, { "@num", num } });
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public int GetSpareQtyInDb(string num)
        {
            DataTable dt = new DataTable();
            _sqlCmd = "SELECT quantity FROM spare_part WHERE partNumber = @partNumber";
            dt = _db.ExecuteDataTable(_sqlCmd, new Dictionary<string, object> { { "@partNumber", num } });
            int qtyInSpare_Part = int.Parse(dt.Rows[0][0].ToString());
            return qtyInSpare_Part;
        }

        public Boolean reOrder(string id, string partNum, int qty, Boolean isLM) //customer id, part number, quantity
        {
            spareListController c = new spareListController();
            return c.addCart(id, partNum, qty, isLM);
        }

        public int checkOnSaleQty(string partNum)
        {
            DataTable dt = new DataTable();
            _sqlCmd = "SELECT OnSaleQty FROM product WHERE partNumber = @partNumber";
            dt = _db.ExecuteDataTable(_sqlCmd, new Dictionary<string, object> { { "@partNumber", partNum } });
            return int.Parse(dt.Rows[0][0].ToString());
        }

        public bool RemoveAll(string id) //customer id
        {
            string cartID = GetCartId(id);
            try
            {
                _db.ExecuteNonQueryCommand("DELETE FROM product_in_cart WHERE cartID = @cartID",
                    new Dictionary<string, object> { { "@cartID", cartID } });
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public string GetCartId(string id) //customer id
        {
            DataTable dt = new DataTable();
            _sqlCmd = "SELECT customerAccountID FROM customer_account WHERE customerID = @customerID";
            dt = _db.ExecuteDataTable(_sqlCmd, new Dictionary<string, object> { { "@customerID", id } });
            string customerAccointID = dt.Rows[0][0].ToString();

            dt = new DataTable();
            _sqlCmd = "SELECT cartID FROM cart WHERE customerAccountID = @customerAccountID";
            dt = _db.ExecuteDataTable(_sqlCmd, new Dictionary<string, object> { { "@customerAccountID", customerAccointID } });
            return dt.Rows[0][0].ToString();
        }

        public List<string> GetAllPartNumInCart(string id) //customer id  //for remove all item
        {
            string cartID = GetCartId(id);
            DataTable dt = new DataTable();
            _sqlCmd = "SELECT itemID FROM product_in_cart WHERE cartID = @cartID";
            dt = _db.ExecuteDataTable(_sqlCmd, new Dictionary<string, object> { { "@cartID", cartID } });
            List<string> itemId = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                itemId.Add(dt.Rows[i][0].ToString());
            }

            List<string> partNum = new List<string>();
            foreach (var t in itemId)
            {
                dt = new DataTable();
                _sqlCmd = "SELECT partNumber FROM product WHERE itemID = @itemID";
                dt = _db.ExecuteDataTable(_sqlCmd, new Dictionary<string, object> { { "@itemID", t } });
                partNum.Add(dt.Rows[0][0].ToString());
            }

            return partNum;
        }

        public List<int> GetAllItemQtyInCart(string id) //for remove all item
        {
            string cartID = GetCartId(id);
            DataTable dt = new DataTable();
            _sqlCmd = "SELECT quantity FROM product_in_cart WHERE cartID = @cartID";
            dt = _db.ExecuteDataTable(_sqlCmd, new Dictionary<string, object> { { "@cartID", cartID } });
            List<int> itemQty = new List<int>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                itemQty.Add(int.Parse(dt.Rows[i][0].ToString()));
            }

            return itemQty;
        }

        public Boolean AddQtyBack(string num, int currentCartQty, int desiredQty, Boolean isLM)
        {
            cartController c = new cartController();
            return c.addQtyBack(num, currentCartQty, desiredQty, isLM);
        }


        //for deliverman
        public bool DelivermanJobFinished(string id) //order id
        {
            try
            {
                _db.ExecuteNonQueryCommand(
                    "UPDATE order_ SET status = @status WHERE orderID = @id",
                    new Dictionary<string, object> { { "@status", "Shipped" }, { "@id", id } });
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public string GetStatus(string orderId)
        {
            DataTable dt = _db.ExecuteDataTable("SELECT status FROM order_ WHERE orderID = @id",
                new Dictionary<string, object> { { "@id", orderId } });
            return dt.Rows[0][0].ToString();
        }
    }
}
