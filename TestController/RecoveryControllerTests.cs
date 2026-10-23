using System.Data;
using LMCIS.controller;
using LMCIS.controller.Utilities;
using Xunit;

namespace TestController
{
    public class RecoveryControllerTests
    {
        [Fact]
        public void ValidateUserDetails_IsPureAndRejectsEmptyOrMalformedValues()
        {
            var validator = new Validator();
            Assert.True(validator.IsValidEmail("person@example.com"));
            Assert.False(validator.IsValidEmail(null));
            Assert.False(validator.IsValidEmail(""));
            Assert.False(validator.IsValidPhoneNumber(null));
            Assert.False(validator.IsValidName(""));

            var db = new RecordingDatabase();
            var recovery = new RecoveryController(new FakeAccountController(db), db);
            Assert.True(recovery.ValidateUserDetails("LMC00001", "person@example.com", "12345678901"));
            Assert.Empty(db.Queries);
        }

        [Fact]
        public void FindUser_UsesUserDetailsAsBoundParameters()
        {
            var db = new RecordingDatabase();
            db.Query = (sql, parameters) => OneRow("customerID", "LMC00001");
            var recovery = new RecoveryController(new FakeAccountController(db), db);

            Assert.True(recovery.FindUser("LMC00001", "a' OR 1=1 --@example.com", "12345678901"));
            var query = Assert.Single(db.Queries);
            Assert.Contains("@id", query.Parameters.Keys);
            Assert.Equal("LMC00001", query.Parameters["@id"]);
            Assert.Equal("a' OR 1=1 --@example.com", query.Parameters["@email"]);
            Assert.DoesNotContain("a' OR 1=1", query.Sql);
        }

        [Fact]
        public void FindUser_UnsupportedOrEmptyIdDoesNotQueryDatabase()
        {
            var db = new RecordingDatabase();
            var recovery = new RecoveryController(new FakeAccountController(db), db);
            Assert.False(recovery.FindUser("", "x@example.com", "12345678901"));
            Assert.False(recovery.FindUser("BAD00001", "x@example.com", "12345678901"));
            Assert.Empty(db.Queries);
        }

        [Fact]
        public void ChangePassword_UsesBoundValuesAndCustomerTable()
        {
            var db = new RecordingDatabase();
            db.Query = (sql, parameters) => OneRow("customerID", "LMC00001");
            var recovery = new RecoveryController(new FakeAccountController(db), db);
            Assert.True(recovery.FindUser("LMC00001", "person@example.com", "12345678901"));
            Assert.True(recovery.ChangePassword("NewSecurePassword1!"));

            var update = Assert.Single(db.Commands);
            Assert.Contains("UPDATE customer_account", update.Sql);
            Assert.Equal("LMC00001", update.Parameters["@id"]);
            Assert.NotEqual("NewSecurePassword1!", update.Parameters["@password"]);
        }

        [Fact]
        public void ChangePassword_IsRejectedAfterFailedOrUnsupportedLookup()
        {
            var db = new RecordingDatabase();
            db.Query = (sql, parameters) => new DataTable();
            var recovery = new RecoveryController(new FakeAccountController(db), db);

            Assert.False(recovery.FindUser("LMC00001", "wrong@example.com", "12345678901"));
            Assert.False(recovery.ChangePassword("NewSecurePassword1!"));
            Assert.False(recovery.FindUser("BAD00001", "person@example.com", "12345678901"));
            Assert.False(recovery.ChangePassword("NewSecurePassword1!"));
            Assert.Empty(db.Commands);
        }

        [Fact]
        public void FailedLookupAfterSuccessfulLookupRevokesPreviousAuthorization()
        {
            var db = new RecordingDatabase();
            var userExists = true;
            db.Query = (sql, parameters) => userExists ? OneRow("customerID", "LMC00001") : new DataTable();
            var recovery = new RecoveryController(new FakeAccountController(db), db);

            Assert.True(recovery.FindUser("LMC00001", "person@example.com", "12345678901"));
            userExists = false;
            Assert.False(recovery.FindUser("LMC00001", "wrong@example.com", "12345678901"));
            Assert.False(recovery.ChangePassword("NewSecurePassword1!"));
            Assert.Empty(db.Commands);
        }

        [Fact]
        public void LoggedInAccountControllerCanChangeItsOwnPasswordWithoutRecoveryLookup()
        {
            var db = new RecordingDatabase();
            var account = new FakeAccountController(db) { IsLogin = true };
            var recovery = new RecoveryController(account, db);

            Assert.True(recovery.ChangePassword("NewSecurePassword1!"));
            var update = Assert.Single(db.Commands);
            Assert.Equal("LMC00001", update.Parameters["@id"]);
        }

        private static DataTable OneRow(string column, string value)
        {
            var table = new DataTable();
            table.Columns.Add(column);
            table.Rows.Add(value);
            return table;
        }
    }

    internal sealed class FakeAccountController : AccountController
    {
        public FakeAccountController(RecordingDatabase database) : base(database) { }
        public override string GetUid() => "LMC00001";
    }
}
