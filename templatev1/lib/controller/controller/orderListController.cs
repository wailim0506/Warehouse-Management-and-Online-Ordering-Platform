using System.Collections.Generic;
using System.Data;
using LMCIS.controller.Utilities;

namespace LMCIS.controller
{
    public class orderListController : abstractController
    {
        string sqlCmd;
        private Database _db;
        private AccountController _ac;

        public orderListController(Database db = null)
        {
            _db = db ?? new Database();
            _ac = new AccountController();
            sqlCmd = "";
        }

        public string GetCustomerAccountId(string id) //id = customerID
        {
            return _db.ExecuteDataTable(
                    "SELECT customerAccountID FROM customer_account WHERE customerID = @customerID",
                    new Dictionary<string, object> { { "@customerID", id } })
                .Rows[0][0].ToString();
        }

        public int CountOrder(string id, string sortBy, string kw)
        {
            string customerAccountID = GetCustomerAccountId(id);
            string sqlCmd = sortBy == "All"
                ? "SELECT COUNT(*) FROM order_ WHERE customerAccountID = @customerAccountID AND orderID LIKE @orderIDLike"
                : "SELECT COUNT(*) FROM order_ WHERE customerAccountID = @customerAccountID AND status = @status AND orderID LIKE @orderIDLike";

            var parameters = new Dictionary<string, object>
            {
                { "@customerAccountID", customerAccountID },
                { "@orderIDLike", "%" + kw + "%" }
            };
            if (sortBy != "All")
            {
                parameters.Add("@status", sortBy);
            }

            return int.Parse(_db.ExecuteDataTable(sqlCmd, parameters).Rows[0][0].ToString());
        }

        public DataTable GetOrder(string id, string sortBy, string kw)
        {
            string customerAccountID = GetCustomerAccountId(id);
            string sqlCmd = sortBy == "All"
                ? "SELECT * FROM order_ WHERE customerAccountID = @customerAccountID AND orderID LIKE @orderIDLike"
                : "SELECT * FROM order_ WHERE customerAccountID = @customerAccountID AND status = @status AND orderID LIKE @orderIDLike";

            var parameters = new Dictionary<string, object>
            {
                { "@customerAccountID", customerAccountID },
                { "@orderIDLike", "%" + kw + "%" }
            };
            if (sortBy != "All")
            {
                parameters.Add("@status", sortBy);
            }

            return _db.ExecuteDataTable(sqlCmd, parameters);
        }

        public string GetStafftId(string id) //staff account id
        {
            DataTable dt = _ac.GetStaffDetail(id);
            return dt.Rows[0][0].ToString();
        }

        public string GetStaffName(string id) //staff account id
        {
            DataTable dt = _ac.GetStaffDetail(id);
            return $"{dt.Rows[0][2].ToString()} {dt.Rows[0][3].ToString()}";
        }

        public string GetStaffContact(string id)
        {
            DataTable dt = _ac.GetStaffDetail(id);
            return dt.Rows[0][6].ToString();
        }
    }
}
