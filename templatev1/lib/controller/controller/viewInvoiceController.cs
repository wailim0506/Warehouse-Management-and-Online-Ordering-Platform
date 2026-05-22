using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LMCIS.controller.Utilities;

namespace LMCIS.controller
{
    public class viewInvoiceController : abstractController
    {
        private Database _db;

        public viewInvoiceController(Database db = null)
        {
            _db = db ?? new Database();
        }

        public string GetOrderDate(string id) //order id
        {
            return _db.ExecuteDataTable("SELECT orderDate FROM order_ WHERE orderID = @orderID",
                    new Dictionary<string, object> { { "@orderID", id } }).Rows[0][0]
                .ToString();
        }

        public string GetCustomerId(string id)
        {
            string customerAccountID = _db.ExecuteDataTable("SELECT customerAccountID FROM order_ WHERE orderID = @orderID",
                new Dictionary<string, object> { { "@orderID", id } }).Rows[0][0].ToString();
            return _db.ExecuteDataTable(
                    "SELECT customerID FROM customer_account WHERE customerAccountID = @customerAccountID",
                    new Dictionary<string, object> { { "@customerAccountID", customerAccountID } })
                .Rows[0][0]
                .ToString();
        }

        public string GetInvoiceNum(string id)
        {
            return _db.ExecuteDataTable("SELECT invoiceNumber FROM invoice WHERE orderID = @orderID",
                    new Dictionary<string, object> { { "@orderID", id } })
                .Rows[0][0]
                .ToString();
        }

        public string GetCustomerAddress(string id)
        {
            return string.Join(", ",
                _db.ExecuteDataTable(
                        "SELECT companyAddress, province, city FROM customer WHERE customerID = @customerID",
                        new Dictionary<string, object> { { "@customerID", GetCustomerId(id) } })
                    .Rows[0].ItemArray);
        }

        public string GetWarehouseAddress(string id)
        {
            return string.Join(", ",
                _db.ExecuteDataTable(
                        "SELECT warehouseAddress, province, city FROM customer WHERE customerID = @customerID",
                        new Dictionary<string, object> { { "@customerID", GetCustomerId(id) } })
                    .Rows[0].ItemArray);
        }

        public string[] GetOrderedSparePartNumber(string id)
        {
            return _db.ExecuteDataTable("SELECT partNumber FROM order_line WHERE orderID = @orderID",
                    new Dictionary<string, object> { { "@orderID", id } })
                .AsEnumerable()
                .Select(row => row[0].ToString()).ToArray();
        }

        public string GetPartName(string num) //part num
        {
            return _db.ExecuteDataTable("SELECT name FROM spare_part WHERE partNumber = @partNumber",
                    new Dictionary<string, object> { { "@partNumber", num } })
                .Rows[0][0]
                .ToString();
        }

        public int GetQty(string id, string num) //id = order id, num = part num
        {
            return int.Parse(
                _db.ExecuteDataTable(
                        "SELECT quantity FROM order_line WHERE partNumber = @partNumber AND orderID = @orderID",
                        new Dictionary<string, object> { { "@partNumber", num }, { "@orderID", id } })
                    .Rows[0][0].ToString());
        }

        public string GetDeliveryDate(string id)
        {
            return _db.ExecuteDataTable("SELECT shippingDate FROM shipping_detail WHERE orderID = @orderID",
                    new Dictionary<string, object> { { "@orderID", id } })
                .Rows[0][0]
                .ToString().Split(' ')[0];
        }

        public Boolean ConfirmInvoice(string num) //invoice num
        {
            try
            {
                _db.ExecuteNonQueryCommand("UPDATE invoice SET status = @status WHERE invoiceNumber = @num",
                    new Dictionary<string, object> { { "@status", "confirmed" }, { "@num", num } });
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
