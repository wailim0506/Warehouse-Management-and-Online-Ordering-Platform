using System.Collections.Generic;
using System.Data;
using LMCIS.controller.Utilities;

namespace LMCIS.controller
{
    public class didListController : abstractController
    {
        private readonly Database _db;

        public didListController(Database database = null)
        {
            _db = database ?? new Database();
        }

        public DataTable getData(string category, string sortBy, string id) //id = order id 
        {
            const string baseQuery =
                "SELECT x.partNumber, y.name, x.quantity, y.categoryID FROM order_line x, spare_part y WHERE x.partNumber = y.partNumber AND x.orderID = @orderID";
            string sqlCmd = baseQuery;
            var parameters = new Dictionary<string, object>
            {
                { "@orderID", id }
            };
            if (category != "All")
            {
                sqlCmd += " AND y.categoryID = @categoryID";
                parameters.Add("@categoryID", category);
            }

            switch (sortBy)
            {
                case "Part Number (Ascending)":
                    sqlCmd += " ORDER BY x.partNumber";
                    break;
                case "Part Number (Descending)":
                    sqlCmd += " ORDER BY x.partNumber DESC";
                    break;
                case "Quantity (Ascending)":
                    sqlCmd += " ORDER BY x.quantity";
                    break;
                case "Quantity (Descending)":
                    sqlCmd += " ORDER BY x.quantity DESC";
                    break;
                default: //sort by = none
                    break;
            }


            return _db.ExecuteDataTable(sqlCmd, parameters);
        }
    }
}
