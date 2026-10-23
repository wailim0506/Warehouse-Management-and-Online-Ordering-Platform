using System;
using System.Data;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using LMCIS.controller;
using LMCIS.controller.Utilities;
using LMCIS.Online_Ordering_Platform;
using MySql.Data.MySqlClient;
using Xunit;

namespace TestController
{
    public class CatalogPerformanceTests
    {
        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(25)]
        public void CatalogQueryReturnsCardFieldsInOneJoinedRead(int cardCount)
        {
            var expected = CreateCatalogRows(cardCount);
            var database = new RecordingDatabase { Query = (sql, parameters) => expected };
            var controller = new spareListController(database);

            var rows = controller.getSpareWhenTextChange("All", "", "");

            Assert.Single(database.Queries);
            Assert.True(database.Queries[0].Sql.IndexOf("JOIN product", StringComparison.OrdinalIgnoreCase) >= 0);
            Assert.True(database.Queries[0].Sql.IndexOf("JOIN category", StringComparison.OrdinalIgnoreCase) >= 0);
            Assert.Equal(cardCount, rows.Rows.Count);
            if (cardCount > 0)
            {
                Assert.Equal("B", rows.Rows[0][2]);
                Assert.Equal("Brake pad 1", rows.Rows[0][3]);
                Assert.Equal("Brakes", rows.Rows[0]["categoryName"]);
                Assert.Equal("125.50", rows.Rows[0]["price"]);
            }
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(25)]
        public void RefreshRendersJoinedFieldsAndDisposesPreviousCards(int cardCount)
        {
            RunOnSta(() =>
            {
                var database = new RecordingDatabase();
                using (var form = new sparePartList(new spareListController(database)))
                {
                    var rows = CreateCatalogRows(cardCount);
                    form.load_part(rows);
                    var catalogPanel = form.Controls.Find("pnlSP", true).OfType<Panel>().Single();
                    var previousCards = GetGroupBoxes(catalogPanel).ToArray();
                    var sharedImage = cardCount > 0 ? GetPictureBoxes(previousCards[0]).Single().Image : null;

                    Assert.Equal(cardCount, GetGroupBoxes(catalogPanel).Count());
                    if (cardCount > 0)
                    {
                        Assert.Contains(GetLabels(form).Select(label => label.Text), text => text == "B - Brakes");
                        Assert.Contains(GetLabels(form).Select(label => label.Text), text => text == "125.50");
                        var card = GetGroupBoxes(catalogPanel).First();
                        var textControls = card.Controls.Cast<Control>().Where(control => !(control is PictureBox)).ToArray();
                        Assert.All(textControls, control => Assert.Same(textControls[0].Font, control.Font));
                    }

                    form.load_part(rows);
                    if (cardCount > 0)
                        Assert.Same(sharedImage, GetPictureBoxes(GetGroupBoxes(catalogPanel).First()).Single().Image);

                    form.load_part(rows.Clone());
                    Assert.Empty(GetGroupBoxes(catalogPanel));
                    if (sharedImage != null)
                        Assert.True(sharedImage.Width > 0);

                    Assert.All(previousCards, card => Assert.True(card.IsDisposed));
                    Assert.Empty(database.Queries);
                    var fontField = typeof(sparePartList).GetField("catalogFont",
                        System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                    form.Dispose();
                    Assert.Null(fontField.GetValue(form));
                    if (sharedImage != null)
                        Assert.True(sharedImage.Width > 0);
                }
            });
        }

        [DatabaseFact]
        [Trait("Category", "DatabaseIntegration")]
        public void JoinedCatalogReadWorksAgainstMariaDb()
        {
            using (var fixture = new CatalogDatabase())
            {
                fixture.Database.ExecuteNonQueryCommand("INSERT INTO category VALUES ('B', 'Brakes')", null);
                fixture.Database.ExecuteNonQueryCommand("INSERT INTO spare_part " +
                    "(partNumber, supplierID, categoryID, name, reorderLevel, dangerLevel, quantity, status, lastModified) " +
                    "VALUES ('P00001', 'SIDIN00001', 'B', 'Brake pad', 100, 50, 5, 'Enable', 'LMS00003')", null);
                fixture.Database.ExecuteNonQueryCommand("INSERT INTO product VALUES " +
                    "('LMP00001', 'B', 'P00001', 5, 5, 'Brake pad details', 125, NULL, 'LMS00003', 'Enable', '2023-02-15')", null);

                var rows = new spareListController(fixture.Database).getSpareWhenTextChange("All", "", "");

                Assert.Single(rows.Rows);
                Assert.Equal("Brakes", rows.Rows[0]["categoryName"]);
                Assert.Equal(125m, Convert.ToDecimal(rows.Rows[0]["price"]));
            }
        }

        [Fact]
        public void InitialLoadRunsOneFilteredReadDespiteComboSelectionEvents()
        {
            RunOnSta(() =>
            {
                var database = new RecordingDatabase { Query = (sql, parameters) => CreateCatalogRows(0) };
                using (var form = new sparePartList(new spareListController(database)))
                {
                    typeof(sparePartList).GetField("UIController", System.Reflection.BindingFlags.Instance |
                        System.Reflection.BindingFlags.NonPublic).SetValue(form, new UIController(null, database));
                    typeof(sparePartList).GetMethod("sparePartList_Load", System.Reflection.BindingFlags.Instance |
                        System.Reflection.BindingFlags.NonPublic).Invoke(form, new object[] { form, EventArgs.Empty });

                    Assert.Single(database.Queries);
                    Assert.Contains("INNER JOIN product", database.Queries[0].Sql);
                }
            });
        }

        private static DataTable CreateCatalogRows(int count)
        {
            var rows = new DataTable();
            rows.Columns.Add("partNumber");
            rows.Columns.Add("supplierID");
            rows.Columns.Add("categoryID");
            rows.Columns.Add("name");
            rows.Columns.Add("reorderLevel");
            rows.Columns.Add("dangerLevel");
            rows.Columns.Add("quantity");
            rows.Columns.Add("status");
            rows.Columns.Add("lastModified");
            rows.Columns.Add("price");
            rows.Columns.Add("categoryName");
            for (var index = 1; index <= count; index++)
                rows.Rows.Add("A" + index.ToString("D5"), "SIDIN00001", "B", "Brake pad " + index,
                    "100", "50", "5", "Enable", "LMS00003", "125.50", "Brakes");
            return rows;
        }

        private sealed class CatalogDatabase : IDisposable
        {
            private readonly string name = "catalog_test_" + Guid.NewGuid().ToString("N");
            private readonly MySqlConnection admin;
            public Database Database { get; private set; }

            public CatalogDatabase()
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
                    Database = new Database(options.ConnectionString);
                    Database.ExecuteNonQueryCommand("CREATE TABLE category (categoryID CHAR(1) PRIMARY KEY, type VARCHAR(30) NOT NULL); " +
                        "CREATE TABLE spare_part (partNumber CHAR(6) PRIMARY KEY, supplierID CHAR(10) NOT NULL, categoryID CHAR(1) NOT NULL, name VARCHAR(50) NOT NULL, reorderLevel INT NOT NULL, dangerLevel INT NOT NULL, quantity INT NOT NULL, status VARCHAR(7) NOT NULL, lastModified CHAR(8) NOT NULL); " +
                        "CREATE TABLE product (itemID CHAR(8) NOT NULL, category CHAR(1) NOT NULL, partNumber CHAR(6) PRIMARY KEY, onSaleQty INT NOT NULL, LM_onSaleQty INT NOT NULL, description VARCHAR(500) NOT NULL, price INT NOT NULL, imageID CHAR(8) NULL, lastModified CHAR(8) NOT NULL, status VARCHAR(7) NOT NULL, onShelvesDate DATE NOT NULL)", null);
                }
                catch
                {
                    Dispose();
                    throw;
                }
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

        private static void RunOnSta(Action action)
        {
            Exception failure = null;
            var thread = new Thread(() =>
            {
                try { action(); }
                catch (Exception exception) { failure = exception; }
            });
            thread.IsBackground = true;
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            Assert.True(thread.Join(TimeSpan.FromSeconds(15)), "STA catalog test did not finish within 15 seconds.");
            if (failure != null) throw failure;
        }

        private static System.Collections.Generic.IEnumerable<Label> GetLabels(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                if (child is Label label) yield return label;
                foreach (var nestedLabel in GetLabels(child)) yield return nestedLabel;
            }
        }

        private static System.Collections.Generic.IEnumerable<GroupBox> GetGroupBoxes(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                if (child is GroupBox groupBox) yield return groupBox;
                foreach (var nestedGroupBox in GetGroupBoxes(child)) yield return nestedGroupBox;
            }
        }

        private static System.Collections.Generic.IEnumerable<PictureBox> GetPictureBoxes(Control parent)
        {
            foreach (Control child in parent.Controls)
            {
                if (child is PictureBox pictureBox) yield return pictureBox;
                foreach (var nestedPictureBox in GetPictureBoxes(child)) yield return nestedPictureBox;
            }
        }

    }
}
