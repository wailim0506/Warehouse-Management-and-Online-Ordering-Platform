using System.Data;
using LMCIS.controller;
using Xunit;

namespace TestController
{
    public class FavouriteControllerTests
    {
        [Fact]
        public void IsFavourite_ReturnsDatabaseExistenceResultWithBoundValues()
        {
            var db = new RecordingDatabase();
            db.Query = (sql, parameters) => Result(sql.Contains("SELECT * FROM favourite"));
            var controller = new favouriteController(db);

            Assert.True(controller.IsFavourite("P-1", "LMC00001"));
            var query = Assert.Single(db.Queries);
            Assert.Equal("P-1", query.Parameters["@partNumber"]);
            Assert.Equal("LMC00001", query.Parameters["@customerID"]);
        }

        [Fact]
        public void AddAndRemoveFavouriteResolveProductAndBindCustomerAndItem()
        {
            var db = new RecordingDatabase();
            db.Query = (sql, parameters) => sql.Contains("SELECT itemID") ? Product("I-1") : Result(false);
            var controller = new favouriteController(db);

            Assert.True(controller.AddToFavourite("P-1", "LMC00001"));
            Assert.True(controller.RemoveFromFavourite("P-1", "LMC00001"));

            Assert.Equal(2, db.Commands.Count);
            Assert.Equal("LMC00001", db.Commands[0].Parameters["@cid"]);
            Assert.Equal("I-1", db.Commands[0].Parameters["@id"]);
            Assert.Equal("LMC00001", db.Commands[1].Parameters["@cid"]);
            Assert.Equal("I-1", db.Commands[1].Parameters["@id"]);
        }

        [Fact]
        public void AddOrRemoveMissingProductReturnsFalseWithoutIndexingEmptyResult()
        {
            var db = new RecordingDatabase();
            db.Query = (sql, parameters) => new DataTable();
            var controller = new favouriteController(db);

            Assert.False(controller.AddToFavourite("missing", "LMC00001"));
            Assert.False(controller.RemoveFromFavourite("missing", "LMC00001"));
            Assert.Empty(db.Commands);
        }

        private static DataTable Product(string itemId)
        {
            var table = new DataTable();
            table.Columns.Add("itemID");
            table.Rows.Add(itemId);
            return table;
        }

        private static DataTable Result(bool exists)
        {
            var table = new DataTable();
            table.Columns.Add("itemID");
            if (exists) table.Rows.Add("I-1");
            return table;
        }
    }
}
