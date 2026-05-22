using System.Collections.Generic;
using System.Data;
using LMCIS.controller.Utilities;

namespace LMCIS.controller
{
    public class delivermanOrderListController : abstractController
    {
        private readonly Database _db;

        public delivermanOrderListController(Database database = null)
        {
            _db = database ?? new Database();
        }

        public DataTable getAllOrder(string id, string sortBy) //staff id
        {
            string delivermanID = getDelivermanID(id);
            string status = "Ready to Ship";
            string sqlCmd = sortBy == "Nearest Dates"
                ? "SELECT x.*, y.status FROM shipping_detail x, order_ y WHERE x.delivermanID = @delivermanID AND x.orderID = y.orderID AND y.status = @status"
                : "SELECT x.*, y.status FROM shipping_detail x, order_ y WHERE x.delivermanID = @delivermanID AND x.orderID = y.orderID AND y.status = @status ORDER BY shippingDate DESC";

            return _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object>
            {
                { "@delivermanID", delivermanID },
                { "@status", status }
            });
        }

        public DataTable GetAllFinishedOrder(string id, string sortBy) //staff id
        {
            string delivermanID = getDelivermanID(id);
            string status = "Shipped";
            string sqlCmd = sortBy == "Nearest Dates"
                ? "SELECT x.*, y.status FROM shipping_detail x, order_ y WHERE x.delivermanID = @delivermanID AND x.orderID = y.orderID AND y.status = @status ORDER BY shippingDate DESC"
                : "SELECT x.*, y.status FROM shipping_detail x, order_ y WHERE x.delivermanID = @delivermanID AND x.orderID = y.orderID AND y.status = @status";
            return _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object>
            {
                { "@delivermanID", delivermanID },
                { "@status", status }
            });
        }

        public string getDelivermanID(string id) //staff id
        {
            string sqlCmd = "SELECT delivermanID FROM staff WHERE staffID = @staffID";
            return _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object>
            {
                { "@staffID", id }
            }).Rows[0][0].ToString();
        }
    }
}
