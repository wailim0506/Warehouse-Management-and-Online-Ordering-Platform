using System;
using System.Collections.Generic;
using System.Data;
using LMCIS.controller.Utilities;

namespace LMCIS.controller
{
    public class editOrderController : abstractController
    {
        string sqlCmd;
        viewOrderController c;
        private readonly Database _db;

        public editOrderController(Database database = null)
        {
            sqlCmd = "";
            _db = database ?? new Database();
            c = new viewOrderController(_db);
        }

        // getter
        public DataTable GetOrderedSparePart(string orderID, string sortBy) => c.getOrderedSparePart(orderID, sortBy);
        
        //part Number
        public string GetItemNum(string id) => c.GetItemNum(id);
        
        //part Number
        public string GetPartName(string id) => c.GetPartName(id);
        
        public DataTable GetOrder(string id) => c.GetOrder(id);
        
        public string GetStaffName(string id) => c.GetStaffName(id);
        
        public string GetStafftId(string id) => viewOrderController.GetStafftId(id);
        
        public string GetStaffContact(string id) => c.getStaffContact(id);        // Database operation
        //orderID
        public DataTable GetShippingDetail(string id) => c.GetShippingDetail(id);
        
        //orderID
        public string[] GetDelivermanDetail(string id)=> c.GetDelivermanDetail(id);
        
        // Database operation
        //part num //order id
        public int GetPartQtyInOrder(string num, string id) => int.Parse(
            _db.ExecuteDataTable(
                    "SELECT quantity FROM order_line WHERE partNumber = @partNumber AND orderID = @orderID",
                    new Dictionary<string, object> { { "@partNumber", num }, { "@orderID", id } })
                .Rows[0][0].ToString());
        
        public Boolean AddQtyBack(string num, int currentOrderQty, int desiredQty,
            Boolean isLM)
        {
            cartController cc = new cartController();
            try
            {
                cc.addQtyBack(num, currentOrderQty, desiredQty, isLM);
                return true;
            }
            catch (notEnoughException e)
            {
                throw e;
            }
        }

        public Boolean
            AddQtyBackToSparePart(string num, string id, int currentOrderQty) //part num, order id

        {
            int partQtyInSparePart = GetSpareQtyInDb(num); //current qty in db

            partQtyInSparePart += currentOrderQty;
            try
            {
                _db.ExecuteNonQueryCommand(
                    "UPDATE spare_part SET quantity = @qty WHERE partNumber = @num",
                    new Dictionary<string, object> { { "@qty", partQtyInSparePart }, { "@num", num } });
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public int GetSpareQtyInDb(string num)
        {
            int qtyInSpare_Part = int.Parse(_db.ExecuteDataTable(
                "SELECT quantity FROM spare_part WHERE partNumber = @partNumber",
                new Dictionary<string, object> { { "@partNumber", num } }).Rows[0][0].ToString());
            return qtyInSpare_Part;
        }

        public int getPartQtyInOrder(string id, string num) //order id, part num
        {
            sqlCmd = "SELECT quantity from order_line WHERE partNumber = @partNumber AND orderID = @orderID";
            return int.Parse(_db.ExecuteDataTable(sqlCmd, new Dictionary<string, object>
            {
                { "@partNumber", num },
                { "@orderID", id }
            }).Rows[0][0].ToString());
        }

        public Boolean EditDbQty(string num, int desiredQty, Boolean isLM, string id) //order id
        {
            cartController cc = new cartController();
            if (cc.EditDbQty(num, desiredQty, isLM, true, getPartQtyInOrder(id, num)))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        //id = order id, num = part num, qty = new qty wanted
        public Boolean EditOrderLineQuantity(string id, string num, string qty)
        {
            try
            {
                _db.ExecuteNonQueryCommand(
                    "UPDATE order_line SET quantity = @qty WHERE partNumber = @num AND orderID = @id",
                    new Dictionary<string, object> { { "@qty", qty }, { "@num", num }, { "@id", id } });
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public Boolean DeleteSparePart(string id, string num) //id = order id, num = part num
        {
            try
            {
                _db.ExecuteNonQueryCommand("DELETE FROM order_line WHERE partNumber = @num AND orderID = @id",
                    new Dictionary<string, object> { { "@num", num }, { "@id", id } });
            }
            catch (Exception ex)
            {
                return false;
            }

            return true;
        }

        public void DeleteOrder(string id) //order id
        {
            c.DeleteOrder(id);
        }
    }
}