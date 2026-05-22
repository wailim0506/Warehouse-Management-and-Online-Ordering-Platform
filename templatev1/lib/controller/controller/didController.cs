using System;
using System.Data;
using LMCIS.controller.Utilities;

namespace LMCIS.controller
{
    public class didController : abstractController
    {
        private readonly Database _db;

        public didController(Database database = null)
        {
            _db = database ?? new Database();
        }

        public DataTable getData(string id, string partNum) //order id
        {
            DataTable dt;
            String sqlCmd = "SELECT x.orderDate, x.orderSerialNumber,y.quantity, z.name, z.categoryID, aa.type, " +
                            "ab.customerID,ac.delivermanID, ad.lastName, ad.firstName FROM order_ x, order_line y, " +
                            "spare_part z, category aa, customer ab, deliverman ac, staff ad, shipping_detail ae, " +
                            "customer_account af WHERE x.orderID = @orderID AND y.partNumber = @partNumber AND x.orderID = y.orderID " +
                            "AND y.partNumber = z.partNumber AND aa.categoryID = z.categoryID AND ae.delivermanID = ad.delivermanID " +
                            "AND ae.orderID = x.orderID AND af.customerAccountID = x.customerAccountID AND ab.customerID = af.customerID";
            dt = _db.ExecuteDataTable(sqlCmd, new System.Collections.Generic.Dictionary<string, object>
            {
                { "@orderID", id },
                { "@partNumber", partNum }
            });
            return dt;
        }
    }
}
