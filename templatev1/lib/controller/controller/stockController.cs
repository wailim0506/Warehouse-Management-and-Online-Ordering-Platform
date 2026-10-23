using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using LMCIS.controller.Utilities;
using Microsoft.Extensions.Logging;

namespace LMCIS.controller
{
    public class stockController : abstractController
    {
        AccountController accountController;
        private readonly IDatabase db;
        private string sqlStr, ToModifySpareID, ToModifySupplierID;
        private DataTable dt;
        public stockController(AccountController accountController, IDatabase db = null)
        {
            _db = db ?? new Database();
            this.accountController = accountController;
            this.db = _db;
        }

        //Return all records to the data grid view from database.
        public DataTable GetPart()
        {
            sqlStr = "SELECT partNumber, supplierID, categoryID, name, reorderLevel, dangerLevel, quantity, status " +
                     "FROM spare_part WHERE quantity <= dangerLevel " +
                     "UNION ALL SELECT partNumber, supplierID, categoryID, name, reorderLevel, dangerLevel, quantity, status " +
                     "FROM spare_part WHERE quantity <= reorderLevel AND quantity > dangerLevel " + //Move the spare to the top which meets the danger level.
                     "UNION ALL SELECT partNumber, supplierID, categoryID, name, reorderLevel, dangerLevel, quantity, status " +
                     "FROM spare_part WHERE quantity > dangerLevel AND quantity > reorderLevel"; //Move the spare to the top which meets the reorder level.
            return ExecuteSqlQuery(sqlStr);
        }

        //Search function.
        public DataTable SearchPart(string PartID)
        {
            sqlStr = "SELECT * FROM spare_part WHERE partNumber = @partNumber";
            return _db.ExecuteDataTable(sqlStr, new Dictionary<string, object> { { "@partNumber", PartID } });
        }

        public DataTable AdvancedSearch(dynamic partValues)
        {
            string condition = "";
            var parameters = new Dictionary<string, object>();

            //Set the condition of advenced search.
            condition += string.IsNullOrEmpty(partValues.partName)
                ? ""
                : " AND SP.name LIKE @partName ";
            if (!string.IsNullOrEmpty(partValues.partName)) parameters["@partName"] = "%" + partValues.partName + "%";
            condition += string.IsNullOrEmpty(partValues.supplier) ? "" : " AND S.name = @supplierName ";
            if (!string.IsNullOrEmpty(partValues.supplier)) parameters["@supplierName"] = partValues.supplier;
            condition += string.IsNullOrEmpty(partValues.category) ? "" : " AND C.type = @categoryType ";
            if (!string.IsNullOrEmpty(partValues.category)) parameters["@categoryType"] = partValues.category;
            condition += string.IsNullOrEmpty(partValues.country) ? "" : " AND S.country = @country ";
            if (!string.IsNullOrEmpty(partValues.country)) parameters["@country"] = partValues.country;

            sqlStr =
                "SELECT SP.partNumber, SP.supplierID, SP.categoryID, SP.name, SP.reorderLevel, SP.dangerLevel, SP.quantity, SP.status " +
                "FROM spare_part SP, supplier S, category C " +
                "WHERE SP.supplierID = S.supplierID AND SP.categoryID = C.categoryID " +
                condition;
            return _db.ExecuteDataTable(sqlStr, parameters.Count == 0 ? null : parameters);
        }


        public bool CheckOutOfStock(string PartID)
        {
            dt = new DataTable();

            dt = _db.ExecuteDataTable("SELECT * FROM spare_part WHERE quantity = 0 AND partNumber = @partNumber",
                new Dictionary<string, object> { { "@partNumber", PartID } });

            return dt.Rows.Count == 1;
        }

        public bool CheckReorderLevel(string PartID)
        {
            dt = new DataTable();

            dt = _db.ExecuteDataTable("SELECT * FROM spare_part WHERE quantity < reorderLevel AND partNumber = @partNumber",
                new Dictionary<string, object> { { "@partNumber", PartID } });

            return dt.Rows.Count == 1;
        }

        //Return the detail of the selected spare part.
        public dynamic GetPartInfo(string PartID)
        {
            dt = new DataTable();

            sqlStr =
                "SELECT SP.status, SP.partNumber, SP.supplierID, SP.name AS SPname, SP.reorderLevel, SP.dangerLevel, SP.quantity, " +
                "SP.lastModified, S.name AS Sname, S.phone, S.address, S.country, C.type FROM spare_part SP, supplier S, category C " +
                "WHERE SP.partNumber = @partNumber AND SP.supplierID = S.supplierID AND SP.categoryID = C.categoryID";
            dt = _db.ExecuteDataTable(sqlStr, new Dictionary<string, object> { { "@partNumber", PartID } });

            dynamic StockInfo = new ExpandoObject();
            StockInfo.partNumber = dt.Rows[0]["partNumber"].ToString();
            StockInfo.supplierID = dt.Rows[0]["supplierID"].ToString();
            StockInfo.SPname = dt.Rows[0]["SPname"].ToString();
            StockInfo.reorderLevel = dt.Rows[0]["reorderLevel"].ToString();
            StockInfo.dangerLevel = dt.Rows[0]["dangerLevel"].ToString();
            StockInfo.quantity = dt.Rows[0]["quantity"].ToString();
            StockInfo.Sname = dt.Rows[0]["Sname"].ToString();
            StockInfo.phone = dt.Rows[0]["phone"].ToString();
            StockInfo.address = dt.Rows[0]["address"].ToString();
            StockInfo.country = dt.Rows[0]["country"].ToString();
            StockInfo.type = dt.Rows[0]["type"].ToString();
            StockInfo.status = dt.Rows[0]["status"].ToString();
            StockInfo.lastModified = dt.Rows[0]["lastModified"].ToString();

            return StockInfo;
        }

        private dynamic GetOnSaleInfo(string PartID)
        {
            dt = new DataTable();

            dt = _db.ExecuteDataTable("SELECT * FROM product WHERE partNumber = @partNumber",
                new Dictionary<string, object> { { "@partNumber", PartID } });

            dynamic OnSaleInfo = new ExpandoObject();
            OnSaleInfo.itemID = dt.Rows[0]["itemID"].ToString();
            OnSaleInfo.onSaleQty = dt.Rows[0]["onSaleQty"].ToString();
            OnSaleInfo.LM_onSaleQty = dt.Rows[0]["LM_onSaleQty"].ToString();
            OnSaleInfo.description = dt.Rows[0]["description"].ToString();
            OnSaleInfo.price = dt.Rows[0]["price"].ToString();
            OnSaleInfo.status = dt.Rows[0]["status"].ToString();

            return OnSaleInfo;
        }


        public List<string> GetCategory()
        {
            var sqlStr = "SELECT type FROM category";
            dt = db.ExecuteDataTable(sqlStr);
            Log.LogMessage(LogLevel.Debug, "Stock Controller", $"GetCategory was executed.");
            return dt.AsEnumerable().Select(row => row["type"].ToString()).ToList();
        }

        public List<string> GetCountry()
        {
            var sqlStr = "SELECT country FROM countries";
            dt = db.ExecuteDataTable(sqlStr);
            Log.LogMessage(LogLevel.Debug, "Stock Controller", $"GetCountry was executed.");
            return dt.AsEnumerable().Select(row => row["country"].ToString()).ToList();
        }

        public List<string> GetSupplier()
        {
            var sqlStr = "SELECT name FROM supplier WHERE status = 'Enable'";
            dt = db.ExecuteDataTable(sqlStr);
            Log.LogMessage(LogLevel.Debug, "Stock Controller", $"GetSupplier was executed.");
            return dt.AsEnumerable().Select(row => row["name"].ToString()).ToList();
        }

        public List<string> GetAllSupplier()
        {
            var sqlStr = "SELECT name FROM supplier";
            dt = db.ExecuteDataTable(sqlStr);
            Log.LogMessage(LogLevel.Debug, "Stock Controller", $"GetSupplier was executed.");
            return dt.AsEnumerable().Select(row => row["name"].ToString()).ToList();
        }


        public DataTable GetReorder()
        {
            sqlStr = "SELECT * FROM reorder_request ORDER BY reorderID DESC";
            return ExecuteSqlQuery(sqlStr);
        }

        private string GetReorderID()
        {
            string reorderID = "RE";

            sqlStr = "SELECT * FROM reorder_request";
            dt = db.ExecuteDataTable(sqlStr);
            reorderID += (dt.Rows.Count + 1).ToString("D5");

            return reorderID;
        }

        public void CreateReorderRequest(string PartID, int reorderQty)
        {
            if (reorderQty <= 0) throw new ArgumentOutOfRangeException(nameof(reorderQty), "Reorder quantity must be greater than zero.");

            var reorderParams = new Dictionary<string, object>
            {
                { "@ReorderID", GetReorderID() },
                { "@UID", accountController.GetUid() },
                { "@PartID", PartID },
                { "@Date", DateTime.Now.ToString("yyyy/MM/dd") },
                { "@Qty", reorderQty },
                { "@Status", "processing" }
            };

            sqlStr = "INSERT INTO reorder_request VALUES(@ReorderID, @PartID, @UID, @Date, @Qty, @Status)";
            _db.ExecuteNonQueryCommand(sqlStr, reorderParams);
        }

        public void CancelReorder(string reorderID)
        {
            _db.ExecuteNonQueryCommand("UPDATE reorder_request SET status = 'cancelled' WHERE reorderID = @reorderID",
                new Dictionary<string, object> { { "@reorderID", reorderID } });
        }

        public void AcceptReorderANDRestock(string PartID, string reorderQty, string reorderID)
        {
            _db.ExecuteInTransaction(() =>
            {
                DataTable reorder = _db.ExecuteDataTable(
                    "SELECT partNumber, quantity, status FROM reorder_request WHERE reorderID = @reorderID FOR UPDATE",
                    new Dictionary<string, object> { { "@reorderID", reorderID } });
                if (reorder.Rows.Count != 1 ||
                    !string.Equals(reorder.Rows[0]["status"].ToString(), "processing", StringComparison.OrdinalIgnoreCase) ||
                    !string.Equals(reorder.Rows[0]["partNumber"].ToString(), PartID, StringComparison.OrdinalIgnoreCase))
                    return;

                int quantity = Convert.ToInt32(reorder.Rows[0]["quantity"]);
                if (quantity <= 0) return;

                DataTable sparePart = _db.ExecuteDataTable(
                    "SELECT partNumber FROM spare_part WHERE partNumber = @partNumber FOR UPDATE",
                    new Dictionary<string, object> { { "@partNumber", PartID } });
                if (sparePart.Rows.Count != 1) return;

                _db.ExecuteNonQueryCommand(
                    "UPDATE spare_part SET quantity = quantity + @reorderQty WHERE partNumber = @partNumber",
                    new Dictionary<string, object> { { "@reorderQty", quantity }, { "@partNumber", PartID } });
                _db.ExecuteNonQueryCommand(
                    "UPDATE reorder_request SET status = 'finished' WHERE reorderID = @reorderID AND status = 'processing'",
                    new Dictionary<string, object> { { "@reorderID", reorderID } });
            });
        }

        public void SetModifyPartID(string PartID)
        {
            this.ToModifySpareID = PartID;
        }

        public dynamic GetModifyPartInfo()
        {
            return GetPartInfo(ToModifySpareID);
        }

        public bool ModifyStockInfo(dynamic StockInfo)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@Name", StockInfo.SPname },
                    { "@RLevel", StockInfo.reorderLevel },
                    { "@DLevel", StockInfo.dangerLevel },
                    { "@Supplier", GetSupplierID(StockInfo.Sname) },
                    { "@Cat", GetCategoryID(StockInfo.category) },
                    { "@Qty", StockInfo.quantity },
                    { "@Status", StockInfo.status },
                    { "@partNumber", ToModifySpareID },
                    { "@UID", accountController.GetUid() }
                };

                sqlStr =
                    "UPDATE spare_part SET supplierID = @Supplier, categoryID = @Cat, name = @Name, reorderLevel = @RLevel" +
                    ", dangerLevel = @DLevel, quantity = @Qty, status = @Status, lastModified = @UID WHERE partNumber = @partNumber";

                _db.ExecuteNonQueryCommand(sqlStr, parameters);

                if (StockInfo.status
                    .Equals("Disable")) //Also update the status in the product table if status is disable.
                {
                    sqlStr =
                        "UPDATE product SET status = @Status WHERE partNumber = @partNumber";

                    _db.ExecuteNonQueryCommand(sqlStr, parameters);
                }

                return true;
            }
            catch (Exception e)
            {
                Log.LogMessage(LogLevel.Error, "stock controller", $"Error modifying stock info: {e.Message}");
                return false; //Something went wrong.
            }
        }

        private string GetSupplierID(string suppilerName)
        {
            dt = new DataTable();

            dt = _db.ExecuteDataTable("SELECT supplierID FROM supplier WHERE name = @supplierName",
                new Dictionary<string, object> { { "@supplierName", suppilerName } });

            return dt.Rows[0]["supplierID"].ToString();
        }

        private string GetCategoryID(string Category)
        {
            dt = new DataTable();

            dt = _db.ExecuteDataTable("SELECT categoryID FROM category WHERE type = @categoryType",
                new Dictionary<string, object> { { "@categoryType", Category } });

            return dt.Rows[0]["categoryID"].ToString();
        }

        public dynamic GetOnSalePartInfo()
        {
            return GetOnSaleInfo(ToModifySpareID);
        }

        public int GetTotalSpareQty()
        {
            dt = new DataTable();

            dt = _db.ExecuteDataTable("SELECT * FROM spare_part");

            return dt.Rows.Count;
        }

        public int GetTotalSupplierQty()
        {
            dt = new DataTable();

            dt = _db.ExecuteDataTable("SELECT * FROM supplier");

            return dt.Rows.Count;
        }

        public string GenPartNumber(string category)
        {
            string CategoryID = GetCategoryID(category); //Convert category to categoryID.

            dt = new DataTable();

            dt = _db.ExecuteDataTable("SELECT * FROM spare_part WHERE categoryID = @categoryID",
                new Dictionary<string, object> { { "@categoryID", CategoryID } });

            int No = dt.Rows.Count + 1; //Get the no. of part.

            return CategoryID + No.ToString("D5"); //Convert in to spare part ID format.
        }


        private DataTable ExecuteSqlQuery(string sqlQuery)
        {
            return db.ExecuteDataTable(sqlQuery);
        }

        public bool CreateNewParts(dynamic newPartsInfo)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@SPNumber", newPartsInfo.SPNumber },
                    { "@CategoryID", GetCategoryID(newPartsInfo.Category) },
                    { "@SPname", newPartsInfo.SPname },
                    { "@SuppID", GetSupplierID(newPartsInfo.Supp) },
                    { "@RLevel", newPartsInfo.RLevel },
                    { "@DLevel", newPartsInfo.DLevel },
                    { "@Qty", newPartsInfo.Qty },
                    { "@Status", newPartsInfo.Status },
                    { "@UID", accountController.GetUid() }
                };

                sqlStr =
                    "INSERT INTO spare_part VALUES(@SPNumber, @SuppID, @CategoryID, @SPname, @RLevel, @DLevel, @Qty, @Status, @UID)";

                _db.ExecuteNonQueryCommand(sqlStr, parameters);

                return true;
            }
            catch (Exception e)
            {
                Log.LogMessage(LogLevel.Error, "stock controller", $"Error modifying stock info: {e.Message}");
                return false; //Something went wrong.
            }
        }

        public DataTable GetSupplierList()
        {
            sqlStr = "SELECT * FROM supplier";
            return ExecuteSqlQuery(sqlStr);
        }

        //Return the detail of the selected Supplier.
        public dynamic GetSupplierInfo(string SupplierID)
        {
            dt = new DataTable();

            dt = _db.ExecuteDataTable("SELECT * FROM supplier WHERE supplierID = @supplierID",
                new Dictionary<string, object> { { "@supplierID", SupplierID } });

            dynamic SupplierInfo = new ExpandoObject();
            SupplierInfo.supplierID = dt.Rows[0]["supplierID"].ToString();
            SupplierInfo.name = dt.Rows[0]["name"].ToString();
            SupplierInfo.phone = dt.Rows[0]["phone"].ToString();
            SupplierInfo.address = dt.Rows[0]["address"].ToString();
            SupplierInfo.country = dt.Rows[0]["country"].ToString();
            SupplierInfo.status = dt.Rows[0]["status"].ToString();

            return SupplierInfo;
        }

        public void SetModifySupplierID(string SupplierID)
        {
            ToModifySupplierID = SupplierID;
        }

        public dynamic GetModifySupplierInfo()
        {
            return GetSupplierInfo(ToModifySupplierID);
        }

        public DataTable SearchSupplier(string SupplierID)
        {
            return _db.ExecuteDataTable("SELECT * FROM supplier WHERE supplierID = @supplierID",
                new Dictionary<string, object> { { "@supplierID", SupplierID } });
        }

        public bool ModifySupplierInfo(dynamic SupplierInfo)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@Name", SupplierInfo.name },
                    { "@Phone", SupplierInfo.phone },
                    { "@Address", SupplierInfo.address },
                    { "@Status", SupplierInfo.status },
                    { "@SupplierID", ToModifySupplierID }
                };

                sqlStr =
                    "UPDATE supplier SET name = @Name, phone = @Phone, address = " +
                    "@Address, status = @Status WHERE supplierID = @SupplierID";

                _db.ExecuteNonQueryCommand(sqlStr, parameters);

                if (SupplierInfo.status.Equals("Disable")) //Also update the status in the product
                {
                    //and spare part table if status is disable.
                    sqlStr =
                        "UPDATE spare_part SP, product P SET SP.status = @Status, P.status = @Status WHERE SP.supplierID = @SupplierID AND SP.partNumber = P.partNumber";

                    _db.ExecuteNonQueryCommand(sqlStr, parameters);
                }

                return true;
            }
            catch (Exception e)
            {
                Log.LogMessage(LogLevel.Error, "stock controller", $"Error modifying stock info: {e.Message}");
                return false; //Something went wrong.
            }
        }

        private string GetCountryAbbreviation(string country)
        {
            dt = new DataTable();

            dt = _db.ExecuteDataTable("SELECT abbreviation FROM countries WHERE country = @country",
                new Dictionary<string, object> { { "@country", country } });

            return dt.Rows[0]["abbreviation"].ToString();
        }


        public string GenSupplierNumber(string country)
        {
            string CountryAbbreviation = GetCountryAbbreviation(country); //Convert category to categoryID.

            dt = new DataTable();

            dt = _db.ExecuteDataTable("SELECT * FROM supplier WHERE supplierID LIKE @supplierIdLike",
                new Dictionary<string, object> { { "@supplierIdLike", "%" + CountryAbbreviation + "%" } });

            int No = dt.Rows.Count + 1; //Get the no. of part.

            return "SID" + CountryAbbreviation + No.ToString("D5"); //Convert in to spare part ID format.
        }

        public bool CreateNewSupplier(dynamic newSupplierInfo)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@SupplierID", newSupplierInfo.SID },
                    { "@name", newSupplierInfo.Name },
                    { "@phone", newSupplierInfo.Phone },
                    { "@address", newSupplierInfo.Address },
                    { "@country", newSupplierInfo.Country },
                    { "@status", newSupplierInfo.Status }
                };

                sqlStr =
                    "INSERT INTO supplier VALUES(@SupplierID, @name, @phone, @address, @country, @status)";

                _db.ExecuteNonQueryCommand(sqlStr, parameters);

                return true;
            }
            catch (Exception e)
            {
                Log.LogMessage(LogLevel.Error, "stock controller", $"Error modifying stock info: {e.Message}");
                return false; //Something went wrong.
            }
        }
    }
}
