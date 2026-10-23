using System;
using System.Collections.Generic;
using System.Data;
using LMCIS.controller;
using Xunit;

namespace TestController
{
    public class AccountControllerTests
    {
        private const string UserId = "LMC00001";
        private const string Password = "secret";

        [Fact]
        public void Login_ActiveAccount_SetsIdentityAndUsesBoundUserId()
        {
            var db = new RecordingDatabase();
            db.Query = (sql, parameters) =>
            {
                if (sql.Contains("SELECT password, status")) return AccountRow(BCrypt.Net.BCrypt.HashPassword(Password), "active");
                if (sql.Contains("SELECT isLM")) return Table("isLM", "N");
                if (sql.Contains("SELECT customerAccountID")) return UserRow("CA1", "Ada", "Lovelace");
                return new DataTable();
            };
            var controller = new AccountController(db);
            var ui = new FakeUIController(controller, db);

            Assert.True(controller.Login(UserId, Password, ui));
            Assert.True(controller.IsLogin);
            Assert.Equal(UserId, controller.GetUid());
            Assert.Equal("Lovelace Ada", controller.GetName());
            Assert.Contains(db.Queries, q => q.Parameters.ContainsKey("@uid") && (string)q.Parameters["@uid"] == UserId);
        }

        [Fact]
        public void Login_WrongPassword_ReturnsFalseWithoutChangingLoginState()
        {
            var controller = NewController("active", Password, out var db);
            Assert.False(controller.Login(UserId, "wrong", new FakeUIController(controller, db)));
            Assert.False(controller.IsLogin);
        }

        [Fact]
        public void Login_MissingOrDisabledAccount_ReturnsFalse()
        {
            var missingDb = new RecordingDatabase();
            var missing = new AccountController(missingDb);
            Assert.False(missing.Login(UserId, Password, new FakeUIController(missing, missingDb)));

            var disabled = NewController("disable", Password, out var disabledDb);
            Assert.False(disabled.Login(UserId, Password, new FakeUIController(disabled, disabledDb)));
            Assert.False(disabled.IsLogin);
        }

        [Fact]
        public void Login_FailedAttemptAfterSuccessClearsLoginState()
        {
            var controller = NewController("active", Password, out var db);
            var ui = new FakeUIController(controller, db);
            Assert.True(controller.Login(UserId, Password, ui));
            Assert.False(controller.Login(UserId, "wrong", ui));
            Assert.False(controller.IsLogin);
        }

        [Fact]
        public void Login_PermissionSetupFailureClearsPartialAuthenticationState()
        {
            var controller = NewController("active", Password, out var db);

            Assert.False(controller.Login(UserId, Password, new ThrowingUIController(controller, db)));
            Assert.False(controller.IsLogin);
            Assert.Null(controller.GetUid());
        }

        [Fact]
        public void DelAccount_DisablesCurrentCustomerAccount()
        {
            var db = new RecordingDatabase();
            db.Query = (sql, parameters) => sql.Contains("SELECT password, status")
                ? AccountRow(BCrypt.Net.BCrypt.HashPassword(Password), "active")
                : sql.Contains("SELECT isLM") ? Table("isLM", "N")
                : sql.Contains("SELECT customerAccountID") ? UserRow("CA1", "Ada", "Lovelace")
                : new DataTable();
            var controller = new AccountController(db);
            Assert.True(controller.Login(UserId, Password, new FakeUIController(controller, db)));

            Assert.True(controller.DelAccount());
            var update = Assert.Single(db.Commands, c => c.Sql.Contains("UPDATE customer_account"));
            Assert.Equal("CA1", update.Parameters["@accountID"]);
        }

        private static AccountController NewController(string status, string password, out RecordingDatabase db)
        {
            db = new RecordingDatabase();
            db.Query = (sql, parameters) => sql.Contains("SELECT password, status")
                ? AccountRow(BCrypt.Net.BCrypt.HashPassword(password), status)
                : sql.Contains("SELECT isLM") ? Table("isLM", "N")
                : sql.Contains("SELECT customerAccountID") ? UserRow("CA1", "Ada", "Lovelace")
                : new DataTable();
            return new AccountController(db);
        }

        private static DataTable AccountRow(string hash, string status)
        {
            var table = new DataTable();
            table.Columns.Add("password");
            table.Columns.Add("status");
            table.Rows.Add(hash, status);
            return table;
        }

        private static DataTable UserRow(string accountId, string first, string last)
        {
            var table = new DataTable();
            table.Columns.Add("accountID");
            table.Columns.Add("firstName");
            table.Columns.Add("lastName");
            table.Rows.Add(accountId, first, last);
            return table;
        }

        private static DataTable Table(string column, string value)
        {
            var table = new DataTable();
            table.Columns.Add(column);
            table.Rows.Add(value);
            return table;
        }
    }

    internal sealed class FakeUIController : UIController
    {
        public FakeUIController(AccountController accountController, RecordingDatabase database) : base(accountController, database) { }
        public override void SetPermission(string userId) { }
    }

    internal sealed class ThrowingUIController : UIController
    {
        public ThrowingUIController(AccountController accountController, RecordingDatabase database)
            : base(accountController, database) { }

        public override void SetPermission(string userId)
        {
            throw new InvalidOperationException("Injected permission lookup failure.");
        }
    }
}
