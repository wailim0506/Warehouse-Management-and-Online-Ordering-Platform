using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using LMCIS.controller;
using LMCIS.controller.Utilities;
using MySql.Data.MySqlClient;
using Xunit;

namespace TestController
{
    public class InventoryTransactionTests
    {
        [Fact]
        public void ReadyToShip_DeductsOnceAndMovesOrderStateInsideTransaction()
        {
            var database = new InventoryDatabase { OrderStatus = "Processing", SpareQty = 20, OrderQty = 4 };
            var controller = new staffViewOrderController(database);

            Assert.True(controller.ReadyToShipANDDeductQty("OD00000001"));
            Assert.False(controller.ReadyToShipANDDeductQty("OD00000001"));

            Assert.Equal(16, database.SpareQty);
            Assert.Equal("Ready to Ship", database.OrderStatus);
            Assert.Equal(2, database.TransactionCount);
            Assert.Contains(database.Commands, command => command.Sql.Contains("FROM order_ WHERE orderID = @orderID FOR UPDATE"));
            Assert.All(database.Commands.Where(command => command.Sql.StartsWith("UPDATE")),
                command => Assert.True(command.InTransaction));
        }

        [Fact]
        public void ReadyToShip_DoesNotPartiallyDeductWhenAnyPartIsInsufficient()
        {
            var database = new InventoryDatabase { OrderStatus = "Processing", SpareQty = 2, OrderQty = 4 };
            var controller = new staffViewOrderController(database);

            Assert.False(controller.ReadyToShipANDDeductQty("OD00000001"));

            Assert.Equal(2, database.SpareQty);
            Assert.Equal("Processing", database.OrderStatus);
            Assert.DoesNotContain(database.Commands, command => command.Sql.StartsWith("UPDATE"));
        }

        [Fact]
        public void ReadyToShip_RejectsOrdersWithoutLines()
        {
            var database = new InventoryDatabase { OrderStatus = "Processing", HasOrderLines = false };
            var controller = new staffViewOrderController(database);

            Assert.False(controller.ReadyToShipANDDeductQty("OD00000001"));

            Assert.Equal("Processing", database.OrderStatus);
            Assert.DoesNotContain(database.Commands, command => command.Sql.StartsWith("UPDATE"));
        }

        [Fact]
        public void ReadyToShip_RejectsOrderLineWithoutMatchingSparePart()
        {
            var database = new InventoryDatabase { OrderStatus = "Processing", MissingSparePart = true, OrderQty = 1 };
            var controller = new staffViewOrderController(database);

            Assert.False(controller.ReadyToShipANDDeductQty("OD00000001"));

            Assert.Equal("Processing", database.OrderStatus);
            Assert.DoesNotContain(database.Commands, command => command.Sql.StartsWith("UPDATE"));
        }

        [Fact]
        public void AcceptReorder_RestsOnceAndOnlyForAProcessingRequest()
        {
            var database = new InventoryDatabase { ReorderStatus = "processing", ReorderQty = 7, SpareQty = 10 };
            var controller = new stockController(null, database);

            controller.AcceptReorderANDRestock("A00001", "7", "RE00001");
            controller.AcceptReorderANDRestock("A00001", "7", "RE00001");

            Assert.Equal(17, database.SpareQty);
            Assert.Equal("finished", database.ReorderStatus);
            Assert.Equal(2, database.TransactionCount);
            Assert.Contains(database.Commands, command => command.Sql.Contains("FROM reorder_request") && command.Sql.Contains("FOR UPDATE"));
            Assert.All(database.Commands.Where(command => command.Sql.StartsWith("UPDATE")),
                command => Assert.True(command.InTransaction));
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void CreateReorderRequest_RejectsNonPositiveQuantity(int quantity)
        {
            var database = new InventoryDatabase();
            var controller = new stockController(null, database);

            Assert.Throws<ArgumentOutOfRangeException>(() => controller.CreateReorderRequest("A00001", quantity));
            Assert.Empty(database.Commands);
        }

        [Fact]
        public void AddToOrder_RejectsNonPositiveOrUnavailableQuantityWithoutWriting()
        {
            var database = new InventoryDatabase { OnSaleQty = 3 };
            var controller = new addPartToOrderController(database);

            Assert.False(controller.addToOrder("OD00000001", "A00001", 0, 100, false));
            Assert.False(controller.addToOrder("OD00000001", "A00001", 4, 100, false));

            Assert.Equal(3, database.OnSaleQty);
            Assert.Equal(0, database.OrderLineQty);
            Assert.DoesNotContain(database.Commands, command => command.Sql.StartsWith("INSERT") || command.Sql.StartsWith("UPDATE"));
        }

        [Theory]
        [InlineData("Ready to Ship")]
        [InlineData("Shipped")]
        [InlineData("Cancelled")]
        public void AddToOrder_RejectsOrdersOutsideEditableStates(string status)
        {
            var database = new InventoryDatabase { OrderStatus = status, OnSaleQty = 8 };
            var controller = new addPartToOrderController(database);

            Assert.False(controller.addToOrder("OD00000001", "A00001", 1, 100, false));

            Assert.Equal(8, database.OnSaleQty);
            Assert.Equal(0, database.OrderLineQty);
            Assert.Single(database.Commands);
            Assert.Contains("FROM order_", database.Commands[0].Sql);
            Assert.Contains("FOR UPDATE", database.Commands[0].Sql);
        }

        [Fact]
        public void AddToOrder_RejectsMissingOrder()
        {
            var database = new InventoryDatabase { OrderExists = false, OnSaleQty = 8 };
            var controller = new addPartToOrderController(database);

            Assert.False(controller.addToOrder("OD00000001", "A00001", 1, 100, false));

            Assert.Equal(8, database.OnSaleQty);
            Assert.Equal(0, database.OrderLineQty);
            Assert.Single(database.Commands);
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void DeductOnSalesQty_UsesGuardedRelativeUpdateForEachInventoryColumn(bool isLm)
        {
            var database = new InventoryDatabase { OnSaleQty = 8, LMOnSaleQty = 10 };
            var controller = new addPartToOrderController(database);

            Assert.True(controller.deductOnSalesQty("A00001", 3, isLm));

            Assert.Equal(isLm ? 8 : 5, database.OnSaleQty);
            Assert.Equal(isLm ? 7 : 10, database.LMOnSaleQty);
            Assert.Single(database.Commands);
            Assert.Contains(isLm ? "LM_onSaleQty = LM_onSaleQty - @qty" : "onSaleQty = onSaleQty - @qty", database.Commands[0].Sql);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        public void DeductOnSalesQty_RejectsNonPositiveQuantityWithoutQuery(int quantity)
        {
            var database = new InventoryDatabase { OnSaleQty = 8 };
            var controller = new addPartToOrderController(database);

            Assert.False(controller.deductOnSalesQty("A00001", quantity, false));
            Assert.Empty(database.Commands);
        }

        [Fact]
        public void DeductOnSalesQty_DoesNotOversell()
        {
            var database = new InventoryDatabase { OnSaleQty = 2 };
            var controller = new addPartToOrderController(database);

            Assert.False(controller.deductOnSalesQty("A00001", 3, false));

            Assert.Equal(2, database.OnSaleQty);
            Assert.Contains("onSaleQty >= @qty", database.Commands.Single().Sql);
        }

        [Fact]
        public void AddToOrder_RollsBackOrderAndSaleInventoryWhenSecondWriteFails()
        {
            var database = new InventoryDatabase { OnSaleQty = 8, OrderLineQty = 2, FailOnProductUpdate = true };
            var controller = new addPartToOrderController(database);

            Assert.False(controller.addToOrder("OD00000001", "A00001", 3, 100, false));

            Assert.Equal(8, database.OnSaleQty);
            Assert.Equal(2, database.OrderLineQty);
            Assert.Equal(1, database.TransactionCount);
        }

        [Fact]
        public void InvoiceList_RejectsUnsupportedSortKeysBeforeQuerying()
        {
            var database = new InventoryDatabase();
            var controller = new staffInvoiceListController(database);

            Assert.Throws<ArgumentException>(() => controller.getData("S0000001", "invoiceNumber; DROP TABLE invoice", "All", false));
            Assert.Empty(database.Commands);
        }
    }

    internal sealed class InventoryDatabase : IDatabase
    {
        internal sealed class RecordedCommand
        {
            public string Sql;
            public Dictionary<string, object> Parameters;
            public bool InTransaction;
        }

        public readonly List<RecordedCommand> Commands = new List<RecordedCommand>();
        public string OrderStatus = "Processing";
        public int OrderQty = 0;
        public int SpareQty = 0;
        public int OnSaleQty = 0;
        public int LMOnSaleQty = 0;
        public int OrderLineQty = 0;
        public string ReorderStatus = "processing";
        public int ReorderQty = 0;
        public int TransactionCount;
        public bool FailOnProductUpdate;
        public bool HasOrderLines = true;
        public bool MissingSparePart;
        public bool OrderExists = true;
        private bool inTransaction;

        public void ExecuteInTransaction(Action operation)
        {
            TransactionCount++;
            int oldSpare = SpareQty;
            int oldSale = OnSaleQty;
            int oldLmSale = LMOnSaleQty;
            int oldLine = OrderLineQty;
            string oldOrderStatus = OrderStatus;
            string oldReorderStatus = ReorderStatus;
            bool previous = inTransaction;
            inTransaction = true;
            try
            {
                operation();
            }
            catch
            {
                SpareQty = oldSpare;
                OnSaleQty = oldSale;
                LMOnSaleQty = oldLmSale;
                OrderLineQty = oldLine;
                OrderStatus = oldOrderStatus;
                ReorderStatus = oldReorderStatus;
                throw;
            }
            finally
            {
                inTransaction = previous;
            }
        }

        public DataTable ExecuteDataTable(string sqlQuery, Dictionary<string, object> queryParameters)
        {
            Record(sqlQuery, queryParameters);
            var table = new DataTable();
            if (sqlQuery.Contains("SELECT status FROM order_"))
            {
                table.Columns.Add("status");
                if (OrderExists) table.Rows.Add(OrderStatus);
            }
            else if (sqlQuery.Contains("FROM order_line") && sqlQuery.Contains("spare_part"))
            {
                table.Columns.Add("partNumber");
                table.Columns.Add("quantity", typeof(int));
                table.Columns.Add("availableQuantity", typeof(int));
                if (HasOrderLines)
                    table.Rows.Add("A00001", OrderQty, MissingSparePart ? DBNull.Value : (object)SpareQty);
            }
            else if (sqlQuery.Contains("FROM reorder_request"))
            {
                table.Columns.Add("status");
                table.Columns.Add("quantity", typeof(int));
                table.Columns.Add("partNumber");
                if (ReorderStatus != null) table.Rows.Add(ReorderStatus, ReorderQty, "A00001");
            }
            else if (sqlQuery.Contains("FROM spare_part") && sqlQuery.Contains("FOR UPDATE"))
            {
                table.Columns.Add("partNumber");
                table.Rows.Add("A00001");
            }
            else if (sqlQuery.Contains("FROM product"))
            {
                table.Columns.Add(sqlQuery.Contains("LM_onSaleQty") ? "LM_onSaleQty" : "onSaleQty", typeof(int));
                table.Rows.Add(sqlQuery.Contains("LM_onSaleQty") ? LMOnSaleQty : OnSaleQty);
            }
            else if (sqlQuery.Contains("FROM order_line"))
            {
                table.Columns.Add("quantity", typeof(int));
                if (OrderLineQty > 0) table.Rows.Add(OrderLineQty);
            }
            return table;
        }

        public DataTable ExecuteDataTable(string sqlQuery) => ExecuteDataTable(sqlQuery, null);

        public void ExecuteNonQueryCommand(string sqlQuery, Dictionary<string, object> queryParameters)
        {
            Record(sqlQuery, queryParameters);
            if (sqlQuery.Contains("UPDATE order_line"))
                OrderLineQty = Convert.ToInt32(queryParameters["@qty"]);
            else if (sqlQuery.Contains("INSERT INTO order_line"))
                OrderLineQty = Convert.ToInt32(queryParameters["@qty"]);
            else if (sqlQuery.Contains("UPDATE product"))
            {
                if (FailOnProductUpdate) throw new InvalidOperationException("Injected product update failure.");
                OnSaleQty = Convert.ToInt32(queryParameters["@qty"]);
            }
            else if (sqlQuery.Contains("UPDATE spare_part SET quantity = quantity +"))
                SpareQty += Convert.ToInt32(queryParameters["@reorderQty"]);
            else if (sqlQuery.Contains("UPDATE spare_part SET quantity = quantity -"))
                SpareQty -= OrderQty;
            else if (sqlQuery.Contains("UPDATE order_ SET status"))
                OrderStatus = "Ready to Ship";
            else if (sqlQuery.Contains("UPDATE reorder_request SET status"))
                ReorderStatus = "finished";
        }

        private void Record(string sql, Dictionary<string, object> parameters)
        {
            Commands.Add(new RecordedCommand
            {
                Sql = sql,
                Parameters = parameters == null ? null : new Dictionary<string, object>(parameters),
                InTransaction = inTransaction
            });
        }

        public object ExecuteScalarCommand(string sqlQuery, Dictionary<string, object> queryParameters) => throw new NotSupportedException();
        public MySqlDataReader ExecuteReaderCommand(string sqlQuery, Dictionary<string, object> queryParameters) => throw new NotSupportedException();
        public MySqlCommand CreateCommand(string query, Dictionary<string, object> parameters) => throw new NotSupportedException();
        public void ExecuteNonQuery(MySqlCommand command) => throw new NotSupportedException();
        public object ExecuteScalar(string sqlCmd) => throw new NotSupportedException();
        public string ExecuteScalarCommand(string sqlQuery) => throw new NotSupportedException();
        public object ExecuteCommand(string sqlQuery, Dictionary<string, object> queryParameters, Func<MySqlCommand, object> execute)
        {
            Record(sqlQuery, queryParameters);
            if (sqlQuery.Contains("UPDATE product"))
            {
                if (FailOnProductUpdate) throw new InvalidOperationException("Injected product update failure.");
                int quantity = Convert.ToInt32(queryParameters["@qty"]);
                if (sqlQuery.Contains("LM_onSaleQty"))
                {
                    if (LMOnSaleQty < quantity) return 0;
                    LMOnSaleQty -= quantity;
                }
                else
                {
                    if (OnSaleQty < quantity) return 0;
                    OnSaleQty -= quantity;
                }
                return 1;
            }
            throw new NotSupportedException("Unexpected ExecuteCommand SQL: " + sqlQuery);
        }
        public void Dispose() { }
    }
}
