using System;
using System.Collections.Generic;
using System.Data;
using LMCIS.controller.Utilities;
using Microsoft.Extensions.Logging;

namespace LMCIS.controller
{
    public class cartController : abstractController
    {
        string sqlCmd;
        private readonly Database _db;

        public cartController(Database database = null)
        {
            sqlCmd = "";
            _db = database ?? new Database();
        }

        public DataTable getCartItem(string id)
        {
            string cartID = getCartID(id);
            string sqlCmd =
                "SELECT * from product_in_cart x, product y, spare_part z where x.cartID = @cartID AND x.itemID = y.itemID AND y.partNumber = z.partNumber";
            return _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object> { { "@cartID", cartID } });
        }

        public List<string> getAllPartNumInCart(string id) //customer id  //for remove all item
        {
            string cartID = getCartID(id);
            string sqlCmd = "SELECT itemID FROM product_in_cart WHERE cartID = @cartID";
            DataTable dt = _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object> { { "@cartID", cartID } });

            List<string> itemId = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                itemId.Add(dt.Rows[i][0].ToString());
            }

            List<string> partNum = new List<string>();
            foreach (var t in itemId)
            {
                dt = new DataTable();
                sqlCmd = "SELECT partNumber FROM product WHERE itemID = @itemID";
                dt = _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object> { { "@itemID", t } });
                partNum.Add(dt.Rows[0][0].ToString());
            }

            return partNum;
        }

        public List<int> getAllItemQtyInCart(string id) //for remove all item
        {
            string cartID = getCartID(id);
            string sqlCmd = "SELECT quantity FROM product_in_cart WHERE cartID = @cartID";
            DataTable dt = _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object> { { "@cartID", cartID } });

            List<int> itemQty = new List<int>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                itemQty.Add(int.Parse(dt.Rows[i][0].ToString()));
            }

            return itemQty;
        }


        public Boolean RemovePart(string num, string id) //partNum, customer id
        {
            string cartID = getCartID(id);
            string sqlCmd = "SELECT itemID FROM product WHERE partNumber = @partNumber";
            DataTable dt = _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object> { { "@partNumber", num } });
            string itemID = dt.Rows[0][0].ToString();

            sqlCmd = "DELETE FROM product_in_cart WHERE itemID = @id AND cartID = @cartID";
            var parameters = new Dictionary<string, object>
            {
                { "@id", itemID },
                { "@cartID", cartID }
            };

            try
            {
                _db.ExecuteNonQueryCommand(sqlCmd, parameters);
            }
            catch (Exception ex)
            {
                Log.LogMessage(LogLevel.Error, "CartController",
                    $"Failed to remove part Error: {ex.Message}");
                throw new Exception("Failed to remove part", ex);
            }

            return true;
        }

        public Boolean removeAll(string id) //customer id
        {
            string cartID = getCartID(id);
            string sqlCmd = "DELETE FROM product_in_cart WHERE cartID = @cartID";
            var parameters = new Dictionary<string, object>
            {
                { "@cartID", cartID }
            };

            try
            {
                _db.ExecuteNonQueryCommand(sqlCmd, parameters);
            }
            catch (Exception ex)
            {
                Log.LogMessage(LogLevel.Error, "CartController",
                    $"Failed to clear customer cart Error: {ex.Message}");
                throw new Exception("Failed to remove all items", ex);
            }

            return true;
        }

        public string getCartID(string id) //customer id
        {
            string sqlCmd = "SELECT customerAccountID FROM customer_account WHERE customerID = @customerID";
            string customerAccointID = _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object> { { "@customerID", id } }).Rows[0][0].ToString();

            sqlCmd = "SELECT cartID FROM cart WHERE customerAccountID = @customerAccountID";
            return _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object> { { "@customerAccountID", customerAccointID } }).Rows[0][0].ToString();
        }

        public int GetCurrentQtyInCart(string num, string id) //part num, customer id
        {
            string cartID = getCartID(id);
            string itemID = _db.ExecuteDataTable("SELECT itemID FROM product WHERE partNumber = @partNumber",
                new Dictionary<string, object> { { "@partNumber", num } }).Rows[0][0].ToString();
            return int.Parse(_db.ExecuteDataTable("SELECT quantity FROM product_in_cart WHERE itemID = @itemID AND cartID = @cartID",
                new Dictionary<string, object> { { "@itemID", itemID }, { "@cartID", cartID } }).Rows[0][0].ToString());
        }

        //add qty back to db for product table and spare_part table
        public Boolean addQtyBack(string num, int currentCartQty, int desiredQty, Boolean isLM) //part num 
        {
            //get the qty in db first

            int qtyInProduct = getOnSaleQtyInDb(num, isLM);
            int qtyInSpare_Part = GetSpareQtyInDb(num);

            //add db qty with cart qty
            qtyInProduct += currentCartQty;
            qtyInSpare_Part += currentCartQty;

            try
            {
                if ((qtyInProduct - desiredQty < 0) ||
                    (qtyInSpare_Part - desiredQty <
                     0)) //check if the desired qty is larger than availabe qty after add cart qty to db
                {
                    Log.LogMessage(LogLevel.Error, "CartController",
                        $"Failed to edit database qty Error: desiredQty: {desiredQty}, qtyInProduct: {qtyInProduct}, qtyInSpare_Part: {qtyInSpare_Part}");
                    throw new notEnoughException();
                }
            }
            catch (notEnoughException e)
            {
                throw e;
            }


            //Add to db
            sqlCmd = !isLM
                ? $"UPDATE product SET onSaleQty = @qty WHERE partNumber = @num"
                : $"UPDATE product SET LM_onSaleQty = @qty WHERE partNumber = @num";

            try
            {
                _db.ExecuteNonQueryCommand(sqlCmd, new Dictionary<string, object>
                {
                    { "@qty", qtyInProduct },
                    { "@num", num }
                });
            }
            catch (Exception ex)
            {
                return false;
            }

            //Since no deduction in spare part table when add to cat, no need to add back to spare part table
            //sqlCmd = $"UPDATE spare_part SET quantity = @qty WHERE partNumber = @num";
            //try
            //{
            //    using (MySqlConnection connection = new MySqlConnection(connString))
            //    {
            //        connection.Open();

            //        using (MySqlCommand command = new MySqlCommand(sqlCmd, connection))
            //        {
            //            command.Parameters.AddWithValue("@qty", qtyInSpare_Part);
            //            command.Parameters.AddWithValue("@num", num);

            //            command.ExecuteNonQuery();
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}
            //finally
            //{
            //    conn.Close();
            //}

            return true;
        }

        public Boolean
            EditDbQty(string num, int newQty, Boolean isLM, Boolean editOrder, int currentQtyInOrder) //part num 
        {
            //get the qty in db first
            int qtyInProduct = getOnSaleQtyInDb(num, isLM);
            int qtyInSpare_Part = GetSpareQtyInDb(num);

            //deduct db qty with cart qty
            qtyInProduct -= newQty;


            //edit to db
            string column = isLM ? "LM_onSaleQty" : "onSaleQty";
            sqlCmd = isLM
                ? "UPDATE product SET LM_onSaleQty = @qty WHERE partNumber = @num"
                : "UPDATE product SET onSaleQty = @qty WHERE partNumber = @num";

            try
            {
                _db.ExecuteNonQueryCommand(sqlCmd, new Dictionary<string, object>
                {
                    { "@qty", qtyInProduct },
                    { "@num", num }
                });
            }
            catch (Exception ex)
            {
                Log.LogMessage(LogLevel.Error, "CartController", $"Failed to edit db qty Error: {ex.Message}");
                return false;
            }

            //no need to do it now as no deductin in qty when create order
            //if (editOrder != true) return true;
            //{
            //    //Since no deduction in spare part table when add to cat, no need to deduct in spare part table, only deduct when editing order
            //    qtyInSpare_Part += currentQtyInOrder;
            //    qtyInSpare_Part -= newQty;
            //    sqlCmd = $"UPDATE spare_part SET quantity = @qty WHERE partNumber = @num";
            //    var parameters = new Dictionary<string, object>
            //    {
            //        { "@qty", qtyInSpare_Part },
            //        { "@num", num }
            //    };

            //    try
            //    {
            //        _db.ExecuteNonQueryCommand(sqlCmd, parameters);
            //    }
            //    catch (Exception ex)
            //    {
            //        Log.LogMessage(LogLevel.Error, "CartController", $"Failed to edit db qty Error: {ex.Message}");
            //        return false;
            //    }
            //}


            return true;
        }

        public Boolean editCartQty(string num, string id, int newQty) //part num, customer id, new qty
        {
            string cartID = getCartID(id);
            string itemID = _db.ExecuteDataTable("SELECT itemID FROM product WHERE partNumber = @partNumber",
                    new Dictionary<string, object> { { "@partNumber", num } })
                .Rows[0][0].ToString();

            var parameters = new Dictionary<string, object>
            {
                { "@qty", newQty },
                { "@num", itemID },
                { "@cartID", cartID }
            };

            try
            {
                _db.ExecuteNonQueryCommand(
                    "UPDATE product_in_cart SET quantity = @qty WHERE itemID = @num AND cartID = @cartID", parameters);
            }
            catch (Exception ex)
            {
                Log.LogMessage(LogLevel.Error, "CartController", $"Failed to edit cart qty Error: {ex.Message}");
                return false;
            }

            return true;
        }

        public int getOnSaleQtyInDb(string num, Boolean isLM) //part num
        {
            string column = isLM ? "LM_onSaleQty" : "onSaleQty";
            string sqlCmd = isLM
                ? "SELECT LM_onSaleQty FROM product WHERE partNumber = @partNumber"
                : "SELECT onSaleQty FROM product WHERE partNumber = @partNumber";
            int qtyInProduct = int.Parse(_db.ExecuteDataTable(sqlCmd, new Dictionary<string, object> { { "@partNumber", num } }).Rows[0][0].ToString());
            return qtyInProduct;
        }

        public int GetSpareQtyInDb(string num)
        {
            int qtyInSpare_Part = int.Parse(_db.ExecuteDataTable(
                "SELECT quantity FROM spare_part WHERE partNumber = @partNumber",
                new Dictionary<string, object> { { "@partNumber", num } }).Rows[0][0].ToString());
            return qtyInSpare_Part;
        }

        public Boolean DeductQtyInSparePart(string id) //customer id
        {
            List<string> partNum = getAllPartNumInCart(id); //customer id
            List<int> qtyInCart = getAllItemQtyInCart(id); //customer id
            int qtyInSparePart;
            for (int i = 0; i < partNum.Count; i++)
            {
                qtyInSparePart = GetSpareQtyInDb(partNum[i]);
                qtyInSparePart -= qtyInCart[i]; //deduct the cart qty
                sqlCmd = $"UPDATE spare_part SET quantity = @qty WHERE partNumber = @num";
                try
                {
                    _db.ExecuteNonQueryCommand(sqlCmd, new Dictionary<string, object>
                    {
                        { "@qty", qtyInSparePart },
                        { "@num", partNum[i] }
                    });
                }
                catch (Exception ex)
                {
                    Log.LogMessage(LogLevel.Error, "CartController",
                        $"Failed to deduct qty in spare part Error: {ex.Message}");
                    return false;
                }
            }

            return true;
        }

        public Boolean CreateOrder(string id, string shippingDate, string shippingAddress) //customerID
        {
            //get the customer account id first
            string customerAccountID =
                _db.ExecuteDataTable(
                    "SELECT customerAccountID FROM customer_account WHERE customerID = @customerID",
                    new Dictionary<string, object> { { "@customerID", id } }).Rows[0][0].ToString();
            //generate an order id
            string orderID = OrderIdGenerator();
            //generate order serial num
            string orderSerialNum = OrderSerialNumGenerator();
            //assign an order processing clerk to this order
            string staffAccountID = OrderProcessingClerkAssignment();
            //get date today
            string orderDate = DateTime.Now.ToString("yyyy-MM-dd");

            //create order
            var parameters = new Dictionary<string, object>
            {
                { "@orderID", orderID },
                { "@CAID", customerAccountID },
                { "@SAID", staffAccountID },
                { "@OSN", orderSerialNum },
                { "@date", orderDate },
                { "@status", "Pending" }
            };
            try
            {
                _db.ExecuteNonQueryCommand(
                    "INSERT INTO order_ (orderID, customerAccountID, staffAccountID, " +
                    "orderSerialNumber,orderDate, status) " +
                    "VALUES(@orderID,@CAID,@SAID,@OSN,@date,@status)", parameters);
            }
            catch (Exception ex)
            {
                Log.LogMessage(LogLevel.Error, "CartController",
                    $"Failed to create order Error: {ex.Message}");
                return false;
            }

            //insert to order line, create invoice
            if (CreateOrderLineRow(id, orderID) &&
                createInvoice(customerAccountID, orderID) &&
                createShippingDetail(orderID, shippingDate, shippingAddress))
            {
                //no need to deduct qty in spare part now, shuold be deduct by storeman
                //deduct qty in spare part table
                //return DeductQtyInSparePart(id);
                return true;
            }
            else
            {
                return false;
            }
        }

        public Boolean CreateOrderLineRow(string cid, string id) //customer id, order id
        {
            //get item in cart
            DataTable dt = getCartItem(cid);
            //insert to db
            for (int i = 0; i < dt.Rows.Count; i++)
            {
            string sqlCmd = $"INSERT INTO order_line VALUES(@partNum,@orderID,@qty,@price)";
                var parameters = new Dictionary<string, object>
                {
                    { "@partNum", dt.Rows[i][6].ToString() },
                    { "@orderID", id },
                    { "@qty", dt.Rows[i][2].ToString() },
                    { "@price", dt.Rows[i][10].ToString() }
                };

                try
                {
                    _db.ExecuteNonQueryCommand(sqlCmd, parameters);
                }
                catch (Exception ex)
                {
                    Log.LogMessage(LogLevel.Error, "CartController",
                        $"Failed to create order line Error: {ex.Message}");
                    return false;
                }
            }

            return true;
        }

        public Boolean createInvoice(string caid, string id) //customer account id, order id
        {
            //count how many inovice in db first
            string sqlCmd = "SELECT COUNT(*) FROM invoice";
            string result = _db.ExecuteScalarCommand(sqlCmd);
            int numOfInvoice = result != null ? Convert.ToInt32(result) : 0;

            numOfInvoice++; //invoice number of the order
            string invoiceNum = $"IN{numOfInvoice:D5}";

            sqlCmd =
                $"INSERT INTO invoice (customerAccountID, orderID, invoiceNumber) VALUES(@CAID,@orderID,@invoiceNum)";
            var parameters = new Dictionary<string, object>
            {
                { "@CAID", caid },
                { "@orderID", id },
                { "@invoiceNum", invoiceNum }
            };
            try
            {
                _db.ExecuteNonQueryCommand(sqlCmd, parameters);
            }
            catch (Exception ex)
            {
                Log.LogMessage(LogLevel.Error, "CartController",
                    $"Failed to create invoice Error: {ex.Message}");
                throw new Exception("Failed to create invoice", ex);
            }

            return true;
        }

        public Boolean createShippingDetail(string id, string shippingDate, string shippingAddress)
        {
            string deliverman = DelivermanAssignment();
            string sqlCmd =
                "INSERT INTO shipping_detail (orderID, delivermanID, shippingDate, shippingAddress) VALUES(@orderID,@deliverman,@date,@shippingAddress)";
            var parameters = new Dictionary<string, object>
            {
                { "@orderID", id },
                { "@deliverman", deliverman },
                { "@date", shippingDate },
                { "@shippingAddress", shippingAddress }
            };

            try
            {
                _db.ExecuteNonQueryCommand(sqlCmd, parameters);
            }
            catch (Exception ex)
            {
                Log.LogMessage(LogLevel.Error, "CartController",
                    $"Failed to create shipping detail Error: {ex.Message}");
                throw new Exception("Failed to create shipping detail", ex);
            }

            return true;
        }

        public void ClearCustomerCartAfterCreateOrder(string id) //customer id
        {
            string cartID = getCartID(id);
            string sqlCmd = "DELETE FROM product_in_cart WHERE cartID = @cartID";
            var parameters = new Dictionary<string, object> { { "@cartID", cartID } };

            try
            {
                _db.ExecuteNonQueryCommand(sqlCmd, parameters);
            }
            catch (Exception ex)
            {
                Log.LogMessage(LogLevel.Error, "CartController",
                    $"Failed to clear customer cart Error: {ex.Message}");
                throw new Exception("Failed to clear customer cart", ex);
            }
        }


        public string OrderIdGenerator()
        {
            string yearMonth = DateTime.Now.ToString("yyMM");
            string sqlCmd = "SELECT COUNT(orderID) FROM order_ WHERE orderID LIKE @orderPrefix";

            //see how many orders in this year month
            int orderInYearMonth = Int32.Parse(_db.ExecuteScalarCommand(sqlCmd, new Dictionary<string, object> { { "@orderPrefix", "OD" + yearMonth + "%" } }).ToString()) + 1; //+1 for order id
            return $"OD{yearMonth}{orderInYearMonth:D4}";
        }

        public string OrderSerialNumGenerator()
        {
            string yearMonth = DateTime.Now.ToString("yyMM");
            string sqlCmd = "SELECT COUNT(orderSerialNumber) FROM order_ WHERE orderSerialNumber LIKE @serialPrefix";
            int orderInYearMonth = Convert.ToInt32(_db.ExecuteScalarCommand(sqlCmd, new Dictionary<string, object> { { "@serialPrefix", "SN" + yearMonth + "%" } })) + 1; //+1 for order id

            //see how many order in this year month
            return $"SN{yearMonth}{orderInYearMonth:D4}";
        }

        public string OrderProcessingClerkAssignment()
        {
            //get num of opc first
            string sqlCmd = "SELECT staffID FROM staff WHERE jobTitle = 'order processing clerk'";
            DataTable dt = _db.ExecuteDataTable(sqlCmd);
            int numOfOpc = dt.Rows.Count;


            //random assignment
            Random random = new Random();
            int num = random.Next(0, numOfOpc);
            string staffIDAssigned = dt.Rows[num][0].ToString(); //db dont have enough data now
            //string staffIDAssigned = "LMS00002";

            //get the staff account id
            sqlCmd = "SELECT staffAccountID FROM staff_account WHERE staffID = @staffID";
            dt = _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object> { { "@staffID", staffIDAssigned } });
            return dt.Rows[0][0].ToString();
        }

        public string DelivermanAssignment()
        {
            //get num of opc first
            string sqlCmd = "SELECT delivermanID FROM deliverman";
            DataTable dt = _db.ExecuteDataTable(sqlCmd);
            int numOfDeliverman = dt.Rows.Count;

            //random assignment
            Random random = new Random();
            int num = random.Next(0, numOfDeliverman);
            return dt.Rows[num][0].ToString();
        }

        public DataTable GetCustomerAddress(string id) //customer id
        {
            string sqlCmd = "SELECT warehouseAddress, province, city FROM customer WHERE customerID = @customerID";
            return _db.ExecuteDataTable(sqlCmd, new Dictionary<string, object> { { "@customerID", id } });
        }
    }

    class notEnoughException : Exception
    {
        public notEnoughException() : base("Not enough items in the cart.")
        {
        }
    }
}
