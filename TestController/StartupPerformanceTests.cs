using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using LMCIS.controller;
using LMCIS.controller.Utilities;
using LMCIS.System_page;
using Xunit;

namespace TestController
{
    public class StartupPerformanceTests
    {
        [Fact]
        public void DateFormattingDoesNotOpenADatabaseConnection()
        {
            var formatter = new dateHandler();
            Assert.False(string.IsNullOrEmpty(formatter.SystemDate));
            Assert.False(string.IsNullOrEmpty(formatter.GetSystemDateTime()));
        }

        [Fact]
        public void LoginScreenCanLoadWithoutADatabaseConnection()
        {
            OnStaThread(() =>
            {
                using (var form = new LoadableLogin())
                    form.LoadForTest();
            });
        }

        [Fact]
        public void AuthenticationDoesNotRunOnTheUiThread()
        {
            OnStaThread(() =>
            {
                int uiThread = Thread.CurrentThread.ManagedThreadId;
                int databaseThread = 0;
                var db = new RecordingDatabase
                {
                    Query = (sql, parameters) =>
                    {
                        databaseThread = Thread.CurrentThread.ManagedThreadId;
                        return new DataTable();
                    }
                };
                var account = new AccountController(db);
                using (var form = new Login(account, new UIController(account, db)))
                {
                    PrepareLogin(form);
                    InvokeLogin(form);
                    PumpUntil(() => databaseThread != 0 && Find<Button>(form, "btnLogin").Enabled);
                    Assert.NotEqual(uiThread, databaseThread);
                }
            });
        }

        [Fact]
        public void PendingAuthenticationRejectsRepeatedSubmissionsAndRestoresControls()
        {
            OnStaThread(() =>
            {
                using (var release = new ManualResetEventSlim())
                using (var entered = new ManualResetEventSlim())
                {
                    var db = new RecordingDatabase
                    {
                        Query = (sql, parameters) =>
                        {
                            entered.Set();
                            if (!release.Wait(TimeSpan.FromSeconds(5)))
                                throw new TimeoutException("Authentication was not released by the UI.");
                            return new DataTable();
                        }
                    };
                    var account = new AccountController(db);
                    using (var form = new Login(account, new UIController(account, db)))
                    {
                        try
                        {
                            PrepareLogin(form);
                            InvokeLogin(form);
                            Assert.True(entered.Wait(TimeSpan.FromSeconds(2)));
                            Assert.False(Find<Button>(form, "btnLogin").Enabled);
                            Assert.False(Find<TextBox>(form, "tbUsername").Enabled);
                            InvokeLogin(form);
                            Assert.Single(db.Queries);
                        }
                        finally { release.Set(); }
                        PumpUntil(() => Find<Button>(form, "btnLogin").Enabled);
                        Assert.True(Find<TextBox>(form, "tbUsername").Enabled);
                        Assert.Equal("Invalid username or password.", Find<Label>(form, "lblPasswordMsg").Text);
                    }
                }
            });
        }

        [Theory]
        [InlineData("", "password123", "lblUsernameMsg")]
        [InlineData("LMC00001", "", "lblPasswordMsg")]
        public void MissingCredentialsDoNotOpenAConnection(string username, string password, string message)
        {
            OnStaThread(() =>
            {
                int attempts = 0;
                using (var form = new Login(() => { attempts++; throw new InvalidOperationException(); }))
                {
                    PrepareLogin(form);
                    Find<TextBox>(form, "tbUsername").Text = username;
                    Find<TextBox>(form, "tbPassword").Text = password;
                    InvokeLogin(form);
                    Assert.Equal(0, attempts);
                    Assert.True(Find<Button>(form, "btnLogin").Enabled);
                    Assert.NotEmpty(Find<Label>(form, message).Text);
                }
            });
        }

        [Fact]
        public void FailedConnectionRestoresControlsAndAllowsRetry()
        {
            OnStaThread(() =>
            {
                int attempts = 0;
                var db = new RecordingDatabase { Query = (sql, parameters) => new DataTable() };
                using (var form = new Login(() =>
                {
                    if (Interlocked.Increment(ref attempts) == 1) throw new InvalidOperationException("Unavailable");
                    return db;
                }))
                {
                    PrepareLogin(form);
                    InvokeLogin(form);
                    PumpUntil(() => Find<Button>(form, "btnLogin").Enabled);
                    Assert.Equal("Unable to sign in. Please try again.", Find<Label>(form, "lblPasswordMsg").Text);
                    Assert.True(Find<TextBox>(form, "tbUsername").Enabled);
                    InvokeLogin(form);
                    PumpUntil(() => Find<Button>(form, "btnLogin").Enabled);
                    Assert.Equal("Invalid username or password.", Find<Label>(form, "lblPasswordMsg").Text);
                    Assert.Equal(2, attempts);
                    Assert.Equal(0, db.DisposeCount);
                }
                Assert.Equal(1, db.DisposeCount);
            });
        }

        [Fact]
        public void DisposingLoginWaitsForActiveAuthenticationBeforeReleasingItsConnection()
        {
            OnStaThread(() =>
            {
                using (var entered = new ManualResetEventSlim())
                using (var release = new ManualResetEventSlim())
                {
                    var db = new RecordingDatabase();
                    db.Query = (sql, parameters) =>
                    {
                        entered.Set();
                        if (!release.Wait(TimeSpan.FromSeconds(5))) throw new TimeoutException();
                        Assert.Equal(0, db.DisposeCount);
                        return new DataTable();
                    };
                    using (var form = new Login(() => db))
                    {
                        try
                        {
                            PrepareLogin(form);
                            InvokeLogin(form);
                            Assert.True(entered.Wait(TimeSpan.FromSeconds(2)));
                            form.Dispose();
                            Assert.Equal(0, db.DisposeCount);
                        }
                        finally { release.Set(); }
                        PumpUntil(() => db.DisposeCount == 1);
                    }
                    Assert.Equal(1, db.DisposeCount);
                }
            });
        }

        [Theory]
        [InlineData("en-US", -2)]
        [InlineData("en-GB", 0)]
        [InlineData("zh-TW", 1)]
        [InlineData("th-TH", 30)]
        [InlineData("ar-SA", -30)]
        public void ShippingDateCalculationUsesTypedDatesAcrossCultures(string culture, int days)
        {
            var previousCulture = CultureInfo.CurrentCulture;
            try
            {
                CultureInfo.CurrentCulture = CultureInfo.GetCultureInfo(culture);
                var dates = new DataTable();
                dates.Columns.Add("shippingDate", typeof(DateTime));
                dates.Rows.Add(DateTime.Today.AddDays(days).AddHours(18));
                var db = new RecordingDatabase { Query = (sql, parameters) => dates };
                var handler = new dateHandler(db);
                Assert.Equal(days, handler.DayDifference("order-value"));
                var query = Assert.Single(db.Queries);
                Assert.DoesNotContain("order-value", query.Sql);
                Assert.Equal("order-value", query.Parameters["@orderID"]);
                Assert.Equal(0, db.DisposeCount);
                Assert.Equal(days, handler.DayDifference((DateTime)dates.Rows[0][0]));
                Assert.Single(db.Queries);
            }
            finally { CultureInfo.CurrentCulture = previousCulture; }
        }

        [Theory]
        [InlineData(false)]
        [InlineData(true)]
        public void MissingShippingDatesHaveAClearError(bool nullDate)
        {
            var dates = new DataTable();
            dates.Columns.Add("shippingDate", typeof(DateTime));
            if (nullDate) dates.Rows.Add(DBNull.Value);
            var db = new RecordingDatabase { Query = (sql, parameters) => dates };
            var handler = new dateHandler(db);
            Assert.Throws<InvalidOperationException>(() => handler.DayDifference("order-value"));
            Assert.Equal(0, db.DisposeCount);
        }

        private static T Find<T>(Control form, string name) where T : Control
        {
            return (T)Assert.Single(form.Controls.Find(name, true));
        }

        private static void PrepareLogin(Login form)
        {
            form.CreateControl();
            Find<TextBox>(form, "tbUsername").Text = "LMC00001";
            Find<TextBox>(form, "tbPassword").Text = "password123";
        }

        private static void InvokeLogin(Login form)
        {
            typeof(Login).GetMethod("btnLogin_Click", BindingFlags.NonPublic | BindingFlags.Instance)
                .Invoke(form, new object[] { form, EventArgs.Empty });
        }

        private static void PumpUntil(Func<bool> condition)
        {
            var timeout = Stopwatch.StartNew();
            while (!condition() && timeout.Elapsed < TimeSpan.FromSeconds(5))
            {
                Application.DoEvents();
                Thread.Yield();
            }
            Assert.True(condition(), "The UI operation did not complete.");
        }

        internal static void OnStaThread(Action action)
        {
            Exception failure = null;
            var thread = new Thread(() =>
            {
                try { action(); }
                catch (Exception error) { failure = error; }
            }) { IsBackground = true };
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start();
            Assert.True(thread.Join(TimeSpan.FromSeconds(15)), "The STA operation did not complete.");
            if (failure != null) throw new Exception("The STA operation failed.", failure);
        }

        private sealed class LoadableLogin : Login
        {
            public void LoadForTest() { OnLoad(EventArgs.Empty); }
        }
    }
}
