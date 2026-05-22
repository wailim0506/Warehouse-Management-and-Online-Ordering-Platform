using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.Linq;
using LMCIS.controller.Utilities;
using Microsoft.Extensions.Logging;

namespace LMCIS.controller
{
    public class OnSaleProductController : abstractController
    {
        AccountController accountController;
        private readonly Database db;
        private string sqlStr, ToModifyItemID;
        private DataTable dt;
        private Database _db;

        public OnSaleProductController(AccountController accountController, Database db = null)
        {
            this.accountController = accountController;
            this.db = db ?? new Database();
        }

        private DataTable ExecuteSqlQuery(string sqlQuery) => db.ExecuteDataTable(sqlQuery);
        
        public string GetItemID()
        {
            string itemID = "LMP";

            sqlStr = "SELECT * FROM product";
            dt = db.ExecuteDataTable(sqlStr);
            itemID += (dt.Rows.Count + 1).ToString("D5");

            return itemID;
        }

        public List<string> GetSparePart()
        {
            var sqlStr = "SELECT partNumber FROM spare_part";
            dt = db.ExecuteDataTable(sqlStr);
            Log.LogMessage(LogLevel.Debug, "OnSaleProduct Controller", $"GetSparePart was executed.");
            return dt.AsEnumerable().Select(row => row["partNumber"].ToString()).ToList();
        }


        //Return all records to the data grid view from database.
        public DataTable GetProduct()
        {
            sqlStr = "SELECT itemID, type, onSaleQty, LM_onSaleQty, price, status, onShelvesDate " +
                     "FROM product P, category C WHERE P.category = C.categoryID";
            return ExecuteSqlQuery(sqlStr);
        }

        public int GetTotalProductQty()
        {
            dt = new DataTable();

            sqlStr = $"SELECT * FROM product";

            dt = db.ExecuteDataTable(sqlStr);

            return dt.Rows.Count;
        }


        //Return the detail of the selected product.
        public dynamic GetProductInfo(string itemID)
        {
            dt = new DataTable();

            sqlStr =
                "SELECT P.itemID, P.partNumber, P.onSaleQty, P.LM_onSaleQty, P.price, P.description, SUPP.country, " +
                "P.lastModified, P.status, P.onShelvesDate, C.type, SP.name, SP.quantity, SUPP.name AS suppName, SUPP.supplierID " +
                "FROM product P, category C, spare_part SP, supplier SUPP " +
                "WHERE P.category = C.categoryID AND P.partNumber = SP.partNumber " +
                "AND SP.supplierID = SUPP.supplierID AND itemID = @itemID";

            dt = db.ExecuteDataTable(sqlStr, new Dictionary<string, object> { { "@itemID", itemID } });

            dynamic ProductInfo = new ExpandoObject();
            ProductInfo.itemID = dt.Rows[0]["itemID"].ToString();
            ProductInfo.partNumber = dt.Rows[0]["partNumber"].ToString();
            ProductInfo.onSaleQty = dt.Rows[0]["onSaleQty"].ToString();
            ProductInfo.LM_onSaleQty = dt.Rows[0]["LM_onSaleQty"].ToString();
            ProductInfo.price = dt.Rows[0]["price"].ToString();
            ProductInfo.description = dt.Rows[0]["description"].ToString();
            ProductInfo.lastModified = dt.Rows[0]["lastModified"].ToString();
            ProductInfo.status = dt.Rows[0]["status"].ToString();
            ProductInfo.onShelvesDate = (DateTime)dt.Rows[0]["onShelvesDate"];
            ProductInfo.name = dt.Rows[0]["name"].ToString();
            ProductInfo.suppName = dt.Rows[0]["suppName"].ToString();
            ProductInfo.supplierID = dt.Rows[0]["supplierID"].ToString();
            ProductInfo.type = dt.Rows[0]["type"].ToString();
            ProductInfo.quantity = dt.Rows[0]["quantity"].ToString();
            ProductInfo.country = dt.Rows[0]["country"].ToString();

            return ProductInfo;
        }

        public DataTable SearchProduct(string ItemID)
        {
            sqlStr = "SELECT itemID, type, onSaleQty, LM_onSaleQty, price, status, onShelvesDate " +
                     "FROM product P, category C WHERE P.category = C.categoryID AND itemID = @itemID";
            return db.ExecuteDataTable(sqlStr, new Dictionary<string, object> { { "@itemID", ItemID } });
        }

        public void RemoveFromShelve(string ItemID)
        {
            sqlStr =
                "UPDATE product SET status = 'Disable', lastModified = @lastModified WHERE itemID = @itemID";

            db.ExecuteNonQueryCommand(sqlStr, new Dictionary<string, object>
            {
                { "@lastModified", accountController.GetUid() },
                { "@itemID", ItemID }
            });
        }

        public void SetToModityItemID(string ItemID)
        {
            ToModifyItemID = ItemID;
        }

        public dynamic GetModifyItemInfo()
        {
            return GetProductInfo(ToModifyItemID);
        }

        public bool ModifyItemInfo(dynamic ItemInfo)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@itemID", ToModifyItemID },
                    { "@onSaleQty", ItemInfo.onSaleQty },
                    { "@LM_onSaleQty", ItemInfo.LM_onSaleQty },
                    { "@description", ItemInfo.description },
                    { "@status", ItemInfo.status }
                };

                sqlStr =
                    "UPDATE product SET onSaleQty = @onSaleQty, LM_onSaleQty = @LM_onSaleQty" +
                    ", description = @description, status = @status WHERE itemID = @itemID";

                db.ExecuteNonQueryCommand(sqlStr, parameters);

                return true;
            }
            catch (Exception e)
            {
                Log.LogMessage(LogLevel.Error, "OnSaleProduct Controller", $"Error modifying item info: {e.Message}");
                return false; //Something went wrong.
            }
        }

        private string GetCategoryID(string Category)
        {
            dt = new DataTable();

            sqlStr = "SELECT categoryID FROM category WHERE type = @categoryType";

            dt = db.ExecuteDataTable(sqlStr, new Dictionary<string, object> { { "@categoryType", Category } });

            return dt.Rows[0]["categoryID"].ToString();
        }

        public bool CreateNewItem(dynamic newItemInfo)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@itemID", newItemInfo.itemID },
                    { "@categoryID", GetCategoryID(newItemInfo.category) },
                    { "@partNumber", newItemInfo.partNumber },
                    { "@onSaleQty", newItemInfo.OnSaleQty },
                    { "@LM_onSaleQty", newItemInfo.LM_OnSaleQty },
                    { "@description", newItemInfo.description },
                    { "@price", newItemInfo.price },
                    { "@lastModified", accountController.GetUid() },
                    { "@status", newItemInfo.Status },
                    { "@onShelvesDate", DateTime.Now.ToString("yyyy-MM-dd") },
                };

                sqlStr =
                    "INSERT INTO product VALUES(@itemID, @categoryID, @partNumber, @onSaleQty, " +
                    "@LM_onSaleQty, @description, @price, NULL, @lastModified, @status, @onShelvesDate)";

                db.ExecuteNonQueryCommand(sqlStr, parameters);

                return true;
            }
            catch (Exception e)
            {
                Log.LogMessage(LogLevel.Error, "OnSaleProduct controller", $"Error add new item: {e.Message}");
                return false; //Something went wrong.
            }
        }

        public int GetTotalDiscountQty()
        {
            dt = new DataTable();

            sqlStr = $"SELECT * FROM discount";

            dt = db.ExecuteDataTable(sqlStr);

            return dt.Rows.Count;
        }

        //Return all records to the data grid view from database.
        public DataTable GetDiscount()
        {
            sqlStr = "SELECT discountID, percentage, discountRange, createDate AS postDate, endDate FROM discount";
            return ExecuteSqlQuery(sqlStr);
        }

        public dynamic GetDiscountInfo(string discountID)
        {
            dt = new DataTable();

            sqlStr = "SELECT * FROM discount WHERE discountID = @discountID";

            dt = db.ExecuteDataTable(sqlStr, new Dictionary<string, object> { { "@discountID", discountID } });

            dynamic DiscountInfo = new ExpandoObject();
            DiscountInfo.discountID = dt.Rows[0]["discountID"].ToString();
            DiscountInfo.percentage = dt.Rows[0]["percentage"].ToString();
            DiscountInfo.discountRange = dt.Rows[0]["discountRange"].ToString();
            DiscountInfo.endDate = (DateTime)dt.Rows[0]["endDate"];
            DiscountInfo.createDate = (DateTime)dt.Rows[0]["createDate"];

            return DiscountInfo;
        }

        public string GetDiscountID()
        {
            string discountID = "DISC";

            sqlStr = "SELECT * FROM discount";
            dt = db.ExecuteDataTable(sqlStr);
            discountID += (dt.Rows.Count + 1).ToString("D4");

            return discountID;
        }

        public bool CreateDiscount(dynamic newDiscountInfo)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@discountID", newDiscountInfo.discountID },
                    { "@percentage", newDiscountInfo.percentage },
                    { "@discountRange", newDiscountInfo.range },
                    { "@endDate", newDiscountInfo.endDate },
                    { "@createDate", newDiscountInfo.postDate }
                };

                sqlStr =
                    "INSERT INTO discount VALUES(@discountID, @percentage, @discountRange, @endDate, @createDate)";

                db.ExecuteNonQueryCommand(sqlStr, parameters);

                return true;
            }
            catch (Exception e)
            {
                Log.LogMessage(LogLevel.Error, "OnSaleProduct controller", $"Error add new discount: {e.Message}");
                return false; //Something went wrong.
            }
        }

        public bool ModifyDiscount(dynamic modifyDiscountInfo)
        {
            try
            {
                var parameters = new Dictionary<string, object>
                {
                    { "@discountID", modifyDiscountInfo.discountID },
                    { "@percentage", modifyDiscountInfo.percentage },
                    { "@discountRange", modifyDiscountInfo.range },
                    { "@endDate", modifyDiscountInfo.endDate }
                };

                sqlStr =
                    "UPDATE discount SET percentage = @percentage, discountRange = @discountRange, endDate = @endDate " +
                    "WHERE discountID = @discountID";

                db.ExecuteNonQueryCommand(sqlStr, parameters);

                return true;
            }
            catch (Exception e)
            {
                Log.LogMessage(LogLevel.Error, "OnSaleProduct controller", $"Error modify discount: {e.Message}");
                return false; //Something went wrong.
            }
        }
    }
}
