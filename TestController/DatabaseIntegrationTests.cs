using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LMCIS.controller;
using LMCIS.controller.Utilities;
using MySql.Data.MySqlClient;
using Xunit;

namespace TestController
{
    public sealed class DatabaseFactAttribute : FactAttribute
    {
        public DatabaseFactAttribute()
        {
            if (string.IsNullOrWhiteSpace(Environment.GetEnvironmentVariable("WAREHOUSE_TEST_CONNECTION_STRING")))
                Skip = "Set WAREHOUSE_TEST_CONNECTION_STRING to opt into isolated MySQL/MariaDB integration tests.";
        }
    }

    [Trait("Category", "DatabaseIntegration")]
    public class DatabaseIntegrationTests
    {
        [DatabaseFact]
        public void TransactionsCommitAcrossEveryCommandPath()
        {
            using (var fixture = new TestDatabase())
            using (var prepared = fixture.Database.CreateCommand("INSERT INTO probe VALUES (@value)",
                new Dictionary<string, object> { { "@value", 1 } }))
            {
                var db = fixture.Database;
                db.ExecuteInTransaction(() =>
                {
                    db.ExecuteNonQuery(prepared);
                    Assert.NotNull(prepared.Transaction);
                    db.ExecuteCommand("INSERT INTO probe VALUES (2)", null, command =>
                    {
                        Assert.NotNull(command.Transaction);
                        return command.ExecuteNonQuery();
                    });
                    db.ExecuteNonQueryCommand("INSERT INTO probe VALUES (3)", null);
                    db.ExecuteScalar("INSERT INTO probe VALUES (4)");
                    Assert.Equal(4L, Convert.ToInt64(db.ExecuteScalarCommand("SELECT COUNT(*) FROM probe", null)));
                    Assert.Equal("4", db.ExecuteScalarCommand("SELECT COUNT(*) FROM probe"));
                    Assert.Equal(4, db.ExecuteDataTable("SELECT * FROM probe").Rows.Count);
                    Assert.Single(db.ExecuteDataTable("SELECT * FROM probe WHERE value = @value",
                        new Dictionary<string, object> { { "@value", 1 } }).Rows);
                    using (var reader = db.ExecuteReaderCommand("SELECT value FROM probe ORDER BY value", null))
                    {
                        var count = 0;
                        while (reader.Read()) count++;
                        Assert.Equal(4, count);
                    }
                });
                Assert.Equal(4, fixture.ObserveCount());
            }
        }

        [DatabaseFact]
        public void CallbackFailureRollsBackAllWritesAndAllowsAnotherTransaction()
        {
            using (var fixture = new TestDatabase())
            {
                var db = fixture.Database;
                var failure = new InvalidOperationException("Injected operation failure");
                Assert.Same(failure, Assert.Throws<InvalidOperationException>(() => db.ExecuteInTransaction(() =>
                {
                    db.ExecuteNonQueryCommand("INSERT INTO probe VALUES (1)", null);
                    using (var command = db.CreateCommand("INSERT INTO probe VALUES (2)", null))
                        db.ExecuteNonQuery(command);
                    db.ExecuteScalar("INSERT INTO probe VALUES (3)");
                    throw failure;
                })));
                Assert.Equal(0, fixture.ObserveCount());

                db.ExecuteInTransaction(() => db.ExecuteNonQueryCommand("INSERT INTO probe VALUES (4)", null));
                Assert.Equal(1, fixture.ObserveCount());
            }
        }

        [DatabaseFact]
        public void NestedTransactionIsRejectedAndRollsBackTheOuterOperation()
        {
            using (var fixture = new TestDatabase())
            {
                var db = fixture.Database;
                Assert.Throws<ArgumentNullException>(() => db.ExecuteInTransaction(null));
                Assert.Throws<InvalidOperationException>(() => db.ExecuteInTransaction(() =>
                {
                    db.ExecuteNonQueryCommand("INSERT INTO probe VALUES (1)", null);
                    db.ExecuteInTransaction(() => { });
                }));
                Assert.Equal(0, fixture.ObserveCount());
            }
        }

        [DatabaseFact]
        public void ConcurrentShippingRequestsDeductStockOnlyOnce()
        {
            using (var fixture = new TestDatabase())
            using (var other = new Database(fixture.ConnectionString))
            using (var gate = new Barrier(2))
            {
                fixture.Execute("INSERT INTO order_ VALUES ('OD00000001', 'Processing'); " +
                    "INSERT INTO spare_part VALUES ('A00001', 20); " +
                    "INSERT INTO order_line VALUES ('A00001', 'OD00000001', 4, 100)");
                var first = Task.Run(() => { gate.SignalAndWait(); return new staffViewOrderController(fixture.Database).ReadyToShipANDDeductQty("OD00000001"); });
                var second = Task.Run(() => { gate.SignalAndWait(); return new staffViewOrderController(other).ReadyToShipANDDeductQty("OD00000001"); });
                Task.WaitAll(first, second);
                Assert.NotEqual(first.Result, second.Result);
                Assert.Equal(16, fixture.IntValue("SELECT quantity FROM spare_part WHERE partNumber = 'A00001'"));
                Assert.Equal("Ready to Ship", fixture.Database.ExecuteScalarCommand("SELECT status FROM order_"));
            }
        }

        [DatabaseFact]
        public void ShippingWriteFailureRollsBackEarlierStockDeductions()
        {
            using (var fixture = new TestDatabase())
            {
                fixture.Execute("INSERT INTO order_ VALUES ('OD00000001', 'Processing'); " +
                    "INSERT INTO spare_part VALUES ('A00001', 20), ('A00002', 20); " +
                    "INSERT INTO order_line VALUES ('A00001', 'OD00000001', 4, 100), ('A00002', 'OD00000001', 5, 100)");
                fixture.Execute("CREATE TRIGGER fail_stock_update BEFORE UPDATE ON spare_part FOR EACH ROW " +
                    "BEGIN IF NEW.partNumber = 'A00002' THEN SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Injected stock failure'; END IF; END");

                Assert.False(new staffViewOrderController(fixture.Database).ReadyToShipANDDeductQty("OD00000001"));

                Assert.Equal(40, fixture.IntValue("SELECT SUM(quantity) FROM spare_part"));
                Assert.Equal("Processing", fixture.Database.ExecuteScalarCommand("SELECT status FROM order_"));
            }
        }

        [DatabaseFact]
        public void RepeatedRestockUsesTheStoredRequestOnlyOnce()
        {
            using (var fixture = new TestDatabase())
            {
                fixture.Execute("INSERT INTO spare_part VALUES ('A00001', 10); " +
                    "INSERT INTO reorder_request VALUES ('RE00001', 'A00001', 7, 'processing')");
                var controller = new stockController(null, fixture.Database);
                controller.AcceptReorderANDRestock("A00001", "7", "RE00001");
                controller.AcceptReorderANDRestock("A00001", "7", "RE00001");
                Assert.Equal(17, fixture.IntValue("SELECT quantity FROM spare_part"));
                Assert.Equal("finished", fixture.Database.ExecuteScalarCommand("SELECT status FROM reorder_request"));
            }
        }

        [DatabaseFact]
        public void RestockWriteFailureRollsBackTheStockIncrease()
        {
            using (var fixture = new TestDatabase())
            {
                fixture.Execute("INSERT INTO spare_part VALUES ('A00001', 10); " +
                    "INSERT INTO reorder_request VALUES ('RE00001', 'A00001', 7, 'processing')");
                fixture.Execute("CREATE TRIGGER fail_reorder_update BEFORE UPDATE ON reorder_request FOR EACH ROW " +
                    "SIGNAL SQLSTATE '45000' SET MESSAGE_TEXT = 'Injected request failure'");

                Assert.Throws<InvalidOperationException>(() =>
                    new stockController(null, fixture.Database).AcceptReorderANDRestock("A00001", "7", "RE00001"));

                Assert.Equal(10, fixture.IntValue("SELECT quantity FROM spare_part"));
                Assert.Equal("processing", fixture.Database.ExecuteScalarCommand("SELECT status FROM reorder_request"));
            }
        }

        [DatabaseFact]
        public void ConcurrentOrderAdditionsCannotOversellProductInventory()
        {
            using (var fixture = new TestDatabase())
            using (var other = new Database(fixture.ConnectionString))
            using (var gate = new Barrier(2))
            {
                fixture.Execute("INSERT INTO order_ VALUES ('OD00000001', 'Pending'), ('OD00000002', 'Pending'); " +
                    "INSERT INTO product VALUES ('A00001', 3, 3)");
                var first = Task.Run(() => { gate.SignalAndWait(); return new addPartToOrderController(fixture.Database).addToOrder("OD00000001", "A00001", 2, 100, false); });
                var second = Task.Run(() => { gate.SignalAndWait(); return new addPartToOrderController(other).addToOrder("OD00000002", "A00001", 2, 100, false); });
                Task.WaitAll(first, second);
                Assert.NotEqual(first.Result, second.Result);
                Assert.Equal(1, fixture.IntValue("SELECT onSaleQty FROM product"));
                Assert.Equal(2, fixture.IntValue("SELECT SUM(quantity) FROM order_line"));
            }
        }

        // Each test creates and removes its own schema. The supplied database is never selected or modified.
        private sealed class TestDatabase : IDisposable
        {
            private readonly string name = "warehouse_test_" + Guid.NewGuid().ToString("N");
            private readonly MySqlConnection admin;
            public Database Database { get; private set; }
            public string ConnectionString { get; private set; }

            public TestDatabase()
            {
                var options = new MySqlConnectionStringBuilder(Environment.GetEnvironmentVariable("WAREHOUSE_TEST_CONNECTION_STRING"))
                {
                    Database = string.Empty,
                    ConnectionTimeout = 5,
                    DefaultCommandTimeout = 10,
                    Pooling = false
                };
                admin = new MySqlConnection(options.ConnectionString);
                admin.Open();
                try
                {
                    using (var command = admin.CreateCommand())
                    {
                        command.CommandText = "CREATE DATABASE `" + name + "`";
                        command.ExecuteNonQuery();
                    }
                    options.Database = name;
                    ConnectionString = options.ConnectionString;
                    Database = new Database(ConnectionString);
                    // The columns and InnoDB keys used by the inventory operations.
                    Execute("CREATE TABLE probe (value INT NOT NULL) ENGINE=InnoDB; " +
                        "CREATE TABLE order_ (orderID CHAR(10) PRIMARY KEY, status VARCHAR(20) NOT NULL) ENGINE=InnoDB; " +
                        "CREATE TABLE order_line (partNumber CHAR(6), orderID CHAR(10), quantity INT NOT NULL, orderUnitPrice INT NOT NULL, PRIMARY KEY (partNumber, orderID)) ENGINE=InnoDB; " +
                        "CREATE TABLE spare_part (partNumber CHAR(6) PRIMARY KEY, quantity INT NOT NULL) ENGINE=InnoDB; " +
                        "CREATE TABLE product (partNumber CHAR(6) PRIMARY KEY, onSaleQty INT NOT NULL, LM_onSaleQty INT NOT NULL) ENGINE=InnoDB; " +
                        "CREATE TABLE reorder_request (reorderID CHAR(7) PRIMARY KEY, partNumber CHAR(6), quantity INT NOT NULL, status VARCHAR(20) NOT NULL) ENGINE=InnoDB");
                }
                catch
                {
                    Dispose();
                    throw;
                }
            }

            public void Execute(string sql) => Database.ExecuteNonQueryCommand(sql, null);
            public int IntValue(string sql) => Convert.ToInt32(Database.ExecuteScalar(sql));
            public int ObserveCount()
            {
                using (var observer = new Database(ConnectionString))
                    return Convert.ToInt32(observer.ExecuteScalar("SELECT COUNT(*) FROM probe"));
            }

            public void Dispose()
            {
                Database?.Dispose();
                try
                {
                    using (var command = admin.CreateCommand())
                    {
                        command.CommandText = "DROP DATABASE IF EXISTS `" + name + "`";
                        command.ExecuteNonQuery();
                    }
                }
                finally { admin.Dispose(); }
            }
        }
    }
}
