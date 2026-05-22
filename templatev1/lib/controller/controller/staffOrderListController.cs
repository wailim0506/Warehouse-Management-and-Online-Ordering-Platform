using System.Collections.Generic;
using System.Data;
using LMCIS.controller.Utilities;

namespace LMCIS.controller
{
    public class staffOrderListController : abstractController
    {
        private readonly Database _db;

        public staffOrderListController(Database database = null)
        {
            _db = database ?? new Database();
        }

        public DataTable getOrder(string id, string status, string sortBy, bool isManager, string kw,
            bool isStoreman) //id = staff id
        {
            string sqlCmd = "";
            var parameters = new Dictionary<string, object>
            {
                { "@orderIdLike", "%" + kw + "%" }
            };
            var sortByOptions = new Dictionary<string, string>
            {
                { "Id", "x.orderID" },
                { "IdDESC", "x.orderID DESC" },
                { "Date", "x.orderDate DESC" },
                { "DateDESC", "x.orderDate" },
                { "DDate", "z.shippingDate" },
                { "DDateDESC", "z.shippingDate DESC" },
                { "cId", "y.customerID" },
                { "cIdDESC", "y.customerID DESC" }
            };

            if (!sortByOptions.ContainsKey(sortBy)) return new DataTable();
            sqlCmd = "SELECT x.orderID, x.orderDate, y.customerID, z.shippingDate, x.status FROM order_ x, " +
                     "customer_account y, shipping_detail z";

            if (!isManager)
            {
                if (!isStoreman)
                {
                    sqlCmd +=
                        ", staff_account aa WHERE x.orderID = z.orderID AND y.customerAccountID = x.customerAccountID AND x.staffAccountID = aa.staffAccountID AND aa.staffID = @staffID";
                    parameters.Add("@staffID", id);
                }
                else
                {
                    sqlCmd +=
                        ", staff_account aa WHERE x.orderID = z.orderID AND y.customerAccountID = x.customerAccountID AND x.staffAccountID = aa.staffAccountID AND x.status = 'Processing'";
                }
            }
            else
            {
                sqlCmd += " WHERE x.orderID = z.orderID AND y.customerAccountID = x.customerAccountID";
            }

            //sqlCmd += !isManager
            //    ? $", staff_account aa WHERE x.orderID = z.orderID AND y.customerAccountID = x.customerAccountID AND x.staffAccountID = aa.staffAccountID AND aa.staffID = \'{id}\'"
            //    : " WHERE x.orderID = z.orderID AND y.customerAccountID = x.customerAccountID";


            if (status != "All")
            {
                sqlCmd += " AND x.status = @status";
                parameters.Add("@status", status);
            }

            sqlCmd += " AND x.orderID LIKE @orderIdLike";

            sqlCmd += $" ORDER BY {sortByOptions[sortBy]}";

            return _db.ExecuteDataTable(sqlCmd, parameters);
        }
    }
}
