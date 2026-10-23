using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using LMCIS.controller.Utilities;
using Microsoft.Extensions.Logging;

namespace LMCIS.controller
{
    public class spareListController : abstractController
    {
        private readonly IDatabase db;

        public spareListController(IDatabase database = null)
        {
            db = database ?? new Database();
        }

        private DataTable ExecuteQuery(string sqlCmd) => db.ExecuteDataTable(sqlCmd);
        private DataTable ExecuteQuery(string sqlCmd, Dictionary<string, object> parameters) => db.ExecuteDataTable(sqlCmd, parameters);

        public DataTable getAllPart() => getSpareWhenTextChange("All", "", "");

        public DataTable getSpareWhenTextChange(string category, string kw, string sorting)
        {
            string sqlCmd =
                "SELECT x.*, y.price AS price, c.type AS categoryName " +
                "FROM spare_part x " +
                "INNER JOIN product y ON x.partNumber = y.partNumber " +
                "INNER JOIN category c ON x.categoryID = c.categoryID " +
                "WHERE x.status = 'Enable'";
            var parameters = new Dictionary<string, object>();
            if (category != "All")
            {
                sqlCmd += " AND x.categoryID = @categoryID";
                parameters["@categoryID"] = category;
            }

            if (kw != "")
            {
                sqlCmd += " AND x.name LIKE @nameLike";
                parameters["@nameLike"] = "%" + kw + "%";
            }
            switch (sorting)
            {
                case "Category":
                    sqlCmd += " ORDER BY x.categoryID, x.partNumber";
                    break;
                case "Price (Ascending)":
                    sqlCmd += " ORDER BY y.price";
                    break;
                case "Price (Descending)":
                    sqlCmd += " ORDER BY y.price DESC";
                    break;
                default:
                    sqlCmd += "";
                    break;
            }

            var elapsed = Stopwatch.StartNew();
            var rows = ExecuteQuery(sqlCmd, parameters);
            elapsed.Stop();
            Log.LogMessage(LogLevel.Information, "Performance",
                $"Catalog query returned {rows.Rows.Count} rows in {elapsed.ElapsedMilliseconds} ms.");
            return rows;
        }

        public string getCategoryName(string id) =>
            ExecuteQuery("SELECT type FROM category WHERE categoryID = @categoryID",
                new Dictionary<string, object> { { "@categoryID", id } }).Rows[0][0].ToString();

        public string getPrice(string num) =>
            ExecuteQuery("SELECT price FROM product WHERE partNumber = @partNumber",
                new Dictionary<string, object> { { "@partNumber", num } }).Rows[0][0].ToString();

        public int getOnSaleQty(string num, Boolean isLM)
        {
            string sqlCmd = isLM
                ? "SELECT LM_onSaleQty FROM product WHERE partNumber = @partNumber"
                : "SELECT onSaleQty FROM product WHERE partNumber = @partNumber";
            return int.Parse(ExecuteQuery(sqlCmd, new Dictionary<string, object> { { "@partNumber", num } }).Rows[0][0].ToString());
        }

        public Boolean addCart(string id, string num, int qty, Boolean isLM) =>
            new viewSparePartController().AddToCart(id, num, qty, isLM);

        public List<string> getAllPartName() => ExecuteQuery("SELECT name FROM spare_part").AsEnumerable()
            .Select(row => row[0].ToString()).ToList();
    }
}
