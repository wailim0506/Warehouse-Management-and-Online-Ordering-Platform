using System;
using System.Drawing;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using LMCIS.controller;
using LMCIS.controller.Utilities;
using Microsoft.Extensions.Logging;

namespace LMCIS.System_page
{
    public partial class Login : Form
    {
        //For install date in about page.
        public static string sysInsDate = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");

        private bool IsLogin;

        AccountController accountController;
        UIController UIController;
        RecoveryController recoveryController;
        private readonly Func<IDatabase> createDatabase;
        private IDatabase sessionDatabase;
        private Task<bool> loginTask;
        private bool loginPending;
        private bool closed;

        public Login() : this(null, null, () => new Database())
        {
        }

        public Login(AccountController accountController, UIController uiController)
            : this(accountController, uiController, () => new Database())
        {
        }

        internal Login(Func<IDatabase> createDatabase) : this(null, null, createDatabase)
        {
        }

        private Login(AccountController accountController, UIController uiController, Func<IDatabase> createDatabase)
        {
            if ((accountController == null) != (uiController == null))
                throw new ArgumentException("Both authentication controllers must be supplied together.");
            this.createDatabase = createDatabase ?? throw new ArgumentNullException(nameof(createDatabase));
            InitializeComponent();
            this.accountController = accountController;
            UIController = uiController;
            Disposed += (sender, args) => ReleaseDatabaseAfterLogin();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            IsLogin = false;
            timer1.Interval = 1000;
            timer1.Enabled = true;
            tbPassword.PasswordChar = '*';
            rememberMe();
            grpDevTools.Visible = Configuration.IsDevMode();
            if (Configuration.IsDevMode())
                Log.LogMessage(LogLevel.Information, "[View] Login", "Development mode is enabled.");
            Log.LogMessage(LogLevel.Information, "[View] Login", "Guest user is Loaded the login page.");
        }

        //Login the system.
        private async void btnLogin_Click(object sender, EventArgs e)
        {
            if (loginPending || closed || IsDisposed || Disposing) return;

            //Clean the error message in the label.
            lblUsernameMsg.Text = "";
            lblPasswordMsg.Text = "";

            if (string.IsNullOrEmpty(tbUsername.Text)) //username and password have not been entered.
            {
                lblUsernameMsg.Text = "Please enter your UserID.";
                tbUsername.Select();
                return;
            }
            else if (string.IsNullOrEmpty(tbPassword.Text))
            {
                lblPasswordMsg.Text = "Please enter your password.";
                tbPassword.Select();
                return;
            }

            // Capture UI values before the worker starts. Only this attempt uses the
            // controllers' shared connection until authentication finishes.
            string username = tbUsername.Text;
            string password = tbPassword.Text;
            loginPending = true;
            SetLoginControlsEnabled(false);
            try
            {
                loginTask = Task.Run(() => Authenticate(username, password));
                bool authenticated = await loginTask;
                if (closed || IsDisposed || Disposing) return;

                if (!authenticated)
                {
                    tbPassword.Clear();
                    lblPasswordMsg.Text = "Invalid username or password.";
                    tbUsername.Select();
                    return;
                }

                IsLogin = true;
                rememberMe();
                Log.LogMessage(LogLevel.Information, "[View] Login", "Guest user is logging in.");
                using (var home = new Home(accountController, UIController))
                {
                    Hide();
                    home.StartPosition = FormStartPosition.Manual;
                    home.Location = Location;
                    home.Size = Size;
                    home.ShowDialog();
                }
                Close();
            }
            catch (Exception error)
            {
                Log.LogException(error, "[View] Login");
                if (!closed && !IsDisposed && !Disposing)
                {
                    lblPasswordMsg.Text = "Unable to sign in. Please try again.";
                    Show();
                }
            }
            finally
            {
                loginPending = false;
                if (!closed && !IsDisposed && !Disposing) SetLoginControlsEnabled(true);
            }
        }

        private bool Authenticate(string username, string password)
        {
            var elapsed = Stopwatch.StartNew();
            try
            {
                if (accountController == null)
                {
                    sessionDatabase = createDatabase();
                    accountController = new AccountController(sessionDatabase);
                    UIController = new UIController(accountController, sessionDatabase);
                }
                bool authenticated = accountController.Login(username, password, UIController);
                if (authenticated)
                    accountController.SetLog(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                return authenticated;
            }
            finally
            {
                Log.LogMessage(LogLevel.Information, "Performance", $"Authentication completed in {elapsed.ElapsedMilliseconds} ms.");
            }
        }

        private void SetLoginControlsEnabled(bool enabled)
        {
            tbUsername.Enabled = tbPassword.Enabled = chkRememberMe.Enabled = enabled;
            btnLogin.Enabled = btnForgetPassword.Enabled = btnCreateCustomerAcc.Enabled = enabled;
            grpDevTools.Enabled = enabled;
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            closed = true;
            base.OnFormClosed(e);
        }

        private void ReleaseDatabaseAfterLogin()
        {
            var pending = loginTask;
            if (pending != null && !pending.IsCompleted)
            {
                pending.ContinueWith(completed =>
                {
                    // A disposed window may no longer receive its await continuation.
                    if (completed.IsFaulted) Log.LogException(completed.Exception, "[View] Login");
                    Interlocked.Exchange(ref sessionDatabase, null)?.Dispose();
                }, CancellationToken.None, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
            }
            else
            {
                Interlocked.Exchange(ref sessionDatabase, null)?.Dispose();
            }
        }

        //For remember me function.
        private void rememberMe()
        {
            if (IsLogin)
            {
                if (chkRememberMe.Checked == true) //Store to local
                {
                    LMCIS.Properties.Settings.Default.Usesrname = tbUsername.Text;
                    LMCIS.Properties.Settings.Default.Password = tbPassword.Text;
                    LMCIS.Properties.Settings.Default.Save();
                }
                else
                    LMCIS.Properties.Settings.Default.Reset(); //Clean local data
            }
            else //Read from local
            {
                if (!string.IsNullOrEmpty(LMCIS.Properties.Settings.Default.Usesrname))
                {
                    chkRememberMe.CheckState = CheckState.Checked;
                    tbUsername.Text = LMCIS.Properties.Settings.Default.Usesrname;
                    tbPassword.Text = LMCIS.Properties.Settings.Default.Password;
                    btnLogin.Select();
                }
            }
        }

        //Forget password.
        private void btnForgetPassword_Click(object sender, EventArgs e)
        {
            recoveryController = new RecoveryController();
            Log.LogMessage(LogLevel.Information, "[View] Login", $"Guest user is going to the password recovery page.");
            Form PasswordRecovery = new PasswordRe(recoveryController);
            Hide();
            //Swap the current form to another.
            PasswordRecovery.StartPosition = FormStartPosition.Manual;
            PasswordRecovery.Location = Location;
            PasswordRecovery.Size = Size;
            PasswordRecovery.ShowDialog();
            Close();
        }

        //Create a customer account.
        private void btnCreateCustomerAcc_Click(object sender, EventArgs e)
        {
            recoveryController = new RecoveryController();

            Log.LogMessage(LogLevel.Information, "[View] Login", $"Guest user is going to the create customer account page.");
            Form CreateCustoemrAcc = new CreateCustomerAcc(recoveryController);
            Hide();
            //Swap the current form to another.
            CreateCustoemrAcc.StartPosition = FormStartPosition.Manual;
            CreateCustoemrAcc.Location = Location;
            CreateCustoemrAcc.Size = Size;
            CreateCustoemrAcc.ShowDialog();
            Close();
        }


        //Development Tools
        private void btnTest1_Click(object sender, EventArgs e)
        {
            tbUsername.Text = "LMC00001";
            tbPassword.Text = "password123";
        }

        private void btnTest2_Click(object sender, EventArgs e)
        {
            tbUsername.Text = "LMS00001";
            tbPassword.Text = "password123";
        }

        private void btnTest3_Click(object sender, EventArgs e)
        {
            tbUsername.Text = "LMS00006";
            tbPassword.Text = "password123";
        }

        private void btnTest4_Click_1(object sender, EventArgs e)
        {
            tbUsername.Text = "LMS00003";
            tbPassword.Text = "password123";
        }

        private void btnTest5_Click_1(object sender, EventArgs e)
        {
            tbUsername.Text = "LMS00004";
            tbPassword.Text = "password123";
        }

        private void btnTest6_Click_1(object sender, EventArgs e)
        {
            tbUsername.Text = "LMS00009";
            tbPassword.Text = "password123";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            tbUsername.Text = "LMC00002";
            tbPassword.Text = "password123";
        }

        private void tbPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.Handled = true;
                e.SuppressKeyPress = true;
                btnLogin_Click(this, new EventArgs());
            }
        }

        //Set installed date for the about page.
        public static string getInsDate()
        {
            return sysInsDate;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTimeDate.Text = DateTime.Now.ToString("yyyy/MM/dd   HH:mm:ss");
        }

        private void btnText7_Click(object sender, EventArgs e)
        {
            Log.LogMessage(LogLevel.Information, "[View] Login", "Developer tool is loaded.");
            //Redirect to test tools
            Form DevTool = new DevTool();
            //Swap the current form to another.
            DevTool.StartPosition = FormStartPosition.Manual;
            DevTool.Location = new Point(100, 100);
            DevTool.Size = Size;
            DevTool.ShowDialog();
        }
        

        private void button2_Click(object sender, EventArgs e)
        {
            tbUsername.Text = "LMS00012";
            tbPassword.Text = "password123";
        }
    }
}
