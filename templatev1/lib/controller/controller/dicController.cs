using System.Data;
using LMCIS.controller.Utilities;

namespace LMCIS.controller
{
    public class dicController : abstractController
    {
        private readonly Database _db;

        public dicController(Database database = null)
        {
            _db = database ?? new Database();
        }

        public DataTable getData(string id) //order id
        {
            DataTable dt = new DataTable();
            string sqlCmd = "SELECT x.orderDate, x.orderSerialNumber, y.shippingDate, y.shippingAddress, " +
                            "z.companyAddress, z.province, z.city FROM order_ x, shipping_detail y, customer z, " +
                            "customer_account aa  WHERE x.orderID = @orderID AND x.orderID = y.orderID " +
                            "AND x.customerAccountID = aa.customerAccountID AND aa.customerID = z.customerID";
            dt = _db.ExecuteDataTable(sqlCmd, new System.Collections.Generic.Dictionary<string, object> { { "@orderID", id } });
            return dt;
        }
    }
}
