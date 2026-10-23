using System;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using LMCIS.controller;
using LMCIS.controller.Utilities;
using LMCIS.On_Sale_Product_Manag;
using LMCIS.Online_Ordering_Platform;
using LMCIS.Order_Management;
using LMCIS.Profile;
using LMCIS.Stock_Manag;
using LMCIS.System_page;
using LMCIS.Utilities;
using Microsoft.Extensions.Logging;

namespace LMCIS.User_Manag
{
    public partial class ScreateAccount : Form
    {
        private string uName, UID;
        Bitmap IMG;
        bool IMGUploaded;
        dynamic value;
        proFileController proFileController;
        AccountController accountController;
        UIController UIController;
        UserController UserController;

        public ScreateAccount(AccountController accountController, UIController UIController,
            UserController userController)
        {
            InitializeComponent();
            OwnedImage.TrackDisposal(picUserIMG, () => IMG);
            palSelect1.Visible =
                palSelect2.Visible = palSelect3.Visible = palSelect4.Visible = palSelect5.Visible = false;
            this.accountController = accountController;
            this.UIController = UIController;
            this.UserController = userController;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Initialization();
            Log.LogMessage(LogLevel.Information, "[View] User Modify", $"User: {UID} is loaded the form.");
        }

        private void Initialization()
        {
            setIndicator(UIController.getIndicator("User Management"));
            timer1.Enabled = true;
            UID = accountController.GetUid();
            uName = accountController.GetName();
            lblUid.Text = "UID: " + UID;
            lblFormUID.Text = "LMS" + UserController.getLMSID().ToString("D5");
            lblCreateDate.Text = DateTime.Now.ToString("yyyy/MM/dd");
            tbPass.PasswordChar = tbConfirmPass.PasswordChar = '*';
            cmbDept.Items.AddRange(UserController.GetDept().ToArray());


            //For determine which button needs to be shown.
            dynamic btnFun = UIController.showFun();
            btnFunction1.Visible = btnFun.btn1show;
            btnFunction1.Text = btnFun.btn1value;
            btnFunction2.Visible = btnFun.btn2show;
            btnFunction2.Text = btnFun.btn2value;
            btnFunction3.Visible = btnFun.btn3show;
            btnFunction3.Text = btnFun.btn3value;
            btnFunction4.Visible = btnFun.btn4show;
            btnFunction4.Text = btnFun.btn4value;
            btnFunction5.Visible = btnFun.btn5show;
            btnFunction5.Text = btnFun.btn5value;
        }

        private void setIndicator(int btnNo)
        {
            switch (btnNo)
            {
                case 1:
                    palSelect1.Visible = true;
                    break;
                case 2:
                    palSelect2.Visible = true;
                    break;
                case 3:
                    palSelect3.Visible = true;
                    break;
                case 4:
                    palSelect4.Visible = true;
                    break;
                case 5:
                    palSelect5.Visible = true;
                    break;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Log.LogMessage(LogLevel.Information, "[View] User Modify", $"User: {UID} is going to the user management page.");
            Form UserMgmt = new SAccManage(accountController, UIController);
            Hide();
            UserMgmt.StartPosition = FormStartPosition.Manual;
            UserMgmt.Location = Location;
            UserMgmt.Size = Size;
            UserMgmt.ShowDialog();
            Close();
        }

        private void btnFunction1_Click(object sender, EventArgs e)
        {
            getPage(btnFunction1.Text);
        }

        private void btnFunction2_Click(object sender, EventArgs e)
        {
            getPage(btnFunction2.Text);
        }

        private void btnFunction3_Click(object sender, EventArgs e)
        {
            getPage(btnFunction3.Text);
        }

        private void btnFunction4_Click(object sender, EventArgs e)
        {
            getPage(btnFunction4.Text);
        }

        private void btnFunction5_Click(object sender, EventArgs e)
        {
            getPage(btnFunction5.Text);
        }

        //Determine next page.
        private void getPage(string Function)
        {
            Form next = new Home(accountController, UIController);
            switch (Function)
            {
                case "Order Management":
                    if (UID.StartsWith("LMC"))
                    {
                        next = new customerOrderList(accountController, UIController);
                    }
                    else if (accountController.CheckIsDeliverman())
                    {
                        next = new deliverman(accountController, UIController);
                    }
                    else
                    {
                        next = new staffOrderList(accountController, UIController);
                    }

                    break;
                case "Spare Part":
                    next = new sparePartList(accountController, UIController);
                    break;
                case "Cart":
                    next = new cart(accountController, UIController);
                    break;
                case "Favourite":
                    next = new favourite(accountController, UIController);
                    break;
                case "Give Feedback":
                    next = new giveFeedback(accountController, UIController);
                    break;

                case "On-Sale Product Management":
                    next = new OnSaleMain(accountController, UIController);
                    break;
                case "Stock Management":
                    next = new StockMgmt(accountController, UIController);
                    break;
                case "User Management":
                    next = new SAccManage(accountController, UIController);
                    break;
                case "Invoice Management":
                    next = new staffInvoiceList(accountController, UIController);
                    break;
            }

            Log.LogMessage(LogLevel.Information, "[View] User Modify", $"User: {UID} is going to the {Function} page.");
            Hide();
            next.StartPosition = FormStartPosition.Manual;
            next.Location = Location;
            next.Size = Size;
            next.ShowDialog();
            Close();
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            if (checkInfo()) //Pass to controller and create account
            {
                setValue(); //If passed set the value in to dynameic.
                if (UserController.create(value))
                {
                    var nextAccc = MessageBox.Show(
                        "Create account success! The new account is LMS" +
                        (UserController.getLMSID() - 1).ToString("D5") +
                        ".\nDo you want to create the next account?", "System message", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Information);

                    if (nextAccc == DialogResult.Yes)
                    {
                        Form ScreateAccount = new ScreateAccount(accountController, UIController, UserController);
                        Hide();
                        //Swap the current form to another.
                        ScreateAccount.StartPosition = FormStartPosition.Manual;
                        ScreateAccount.Location = Location;
                        ScreateAccount.Size = Size;
                        ScreateAccount.ShowDialog();
                        Close();
                    }
                    else
                    {
                        Form SAccManage = new SAccManage(accountController, UIController);
                        Hide();
                        //Swap the current form to another.
                        SAccManage.StartPosition = FormStartPosition.Manual;
                        SAccManage.Location = Location;
                        SAccManage.Size = Size;
                        SAccManage.ShowDialog();
                        Close();
                    }
                }
                else
                {
                    MessageBox.Show("System Error! Please Contact The Help Desk.", "System error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                }
            }
        }

        private bool checkInfo()
        {
            //Clean previous error message.
            lblfNameMsg.Text = lblLNameMsg.Text = lblSexMsg.Text = lblJobMsg.Text = lblDateMsg.Text =
                lblPhoneMsg.Text = lblEmailMsg.Text = lblDeptMsg.Text = lblPwdMsg.Text = "";

            //Check firstName
            if (string.IsNullOrEmpty(tbFirstName.Text))
            {
                lblfNameMsg.Text = "Please enter firstName.";
                tbFirstName.Select();
                return false;
            }
            else if (tbFirstName.Text.Length < 2 || tbFirstName.Text.Length > 20)
            {
                lblfNameMsg.Text = "FirstName too short or too long, minimum 2 maximum 20.";
                tbFirstName.Select();
                return false;
            }

            //Check lastName
            if (string.IsNullOrEmpty(tbLastName.Text))
            {
                lblLNameMsg.Text = "Please enter lastName.";
                tbLastName.Select();
                return false;
            }
            else if (tbLastName.Text.Length < 2 || tbLastName.Text.Length > 20)
            {
                lblLNameMsg.Text = "LastName too short or too long, minimum 2 maximum 20.";
                tbLastName.Select();
                return false;
            }

            //Check gender
            if (cmbGender.SelectedItem == null)
            {
                lblSexMsg.Text = "Please select a gender.";
                cmbGender.Select();
                return false;
            }

            //Check jobtitle
            if (cmbJobTitle.SelectedItem == null)
            {
                lblJobMsg.Text = "Please select a gender.";
                cmbJobTitle.Select();
                return false;
            }

            //Check deptment
            if (cmbDept.SelectedItem == null)
            {
                lblDeptMsg.Text = "Please select a gender.";
                cmbDept.Select();
                return false;
            }

            //Check date of birth, MUST today > selected value > age 17.
            if (dtpDateOfBirth.Value.Date == DateTime.Now.Date)
            {
                lblDateMsg.Text = "Please select the date";
                return false;
            }
            else if ((dtpDateOfBirth.Value.Date > DateTime.Now.Date ||
                      dtpDateOfBirth.Value.Date > new DateTime(2007, 1, 1)))
            {
                lblDateMsg.Text = "Please select a valid date.";
                return false;
            }

            //Check phone number
            if (string.IsNullOrEmpty(tbPhone.Text))
            {
                lblPhoneMsg.Text = "Please enter the phone number.";
                tbPhone.Select();
                return false;
            }
            else if (!Regex.Match(tbPhone.Text, @"^([0-9]{11})$").Success &&
                     !Regex.Match(tbPhone.Text, @"^([0-9]{8})$").Success)
            {
                lblPhoneMsg.Text = "Please enter the correct format.";
                tbPhone.Select();
                return false;
            }
            else if (!UserController.CheckEmailPhone(tbPhone.Text))
            {
                lblPhoneMsg.Text = "The phone number has already registered an account.";
                tbPhone.Select();
                return false;
            }

            //Check email address
            if (string.IsNullOrEmpty(tbEmail.Text))
            {
                lblEmailMsg.Text = "Please enter the email address.";
                tbEmail.Select();
                return false;
            }
            else if (!IsValidEmail(tbEmail.Text) || tbEmail.Text.Length > 30)
            {
                lblEmailMsg.Text = "Please enter the correct format.";
                tbEmail.Select();
                return false;
            }
            else if (!UserController.CheckEmailPhone(tbEmail.Text))
            {
                lblEmailMsg.Text = "The email address has already registered an account.";
                tbEmail.Select();
                return false;
            }

            //Check whether the password and confirm the password match.
            if (string.IsNullOrEmpty(tbPass.Text))
            {
                lblPwdMsg.Text = "Please enter the password.";
                tbPass.Select();
                return false;
            }
            else if (tbPass.Text.Length < 10 || tbPass.Text.Length > 50)
            {
                lblPwdMsg.Text = "Password too short or too long, minimum 10 maximum 50.";
                tbPass.Select();
                return false;
            }
            else if (!tbPass.Text.Equals(tbConfirmPass.Text))
            {
                lblPwdMsg.Text = "Confirm password does NOT match.";
                tbConfirmPass.Select();
                return false;
            }

            return true;
        }

        //Check the email address.
        private bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                Log.LogMessage(LogLevel.Error, "[View] User Modify", $"User: {UID} entered an invalid email : {email}.");
                return false;
            }

            try
            {
                Log.LogMessage(LogLevel.Information, "[View] User Modify", $"User: {UID} entered an valid email : {email}.");
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                Log.LogMessage(LogLevel.Warning, "[View] User Modify", $"User: {UID} entered an invalid email : {email}.");
                return false;
            }
        }

        private void setValue()
        {
            value = new ExpandoObject();
            value.fName = tbFirstName.Text;
            value.lName = tbLastName.Text;
            value.joinDate = DateTime.Now.ToString("yyyy/MM/dd");

            if (cmbGender.SelectedIndex == 0)
                value.gender = "M";
            else
                value.gender = "F";

            if (IMGUploaded)
                value.IMG = "''";
            else
                value.IMG = "NULL";

            value.dateOfBirth = dtpDateOfBirth.Value.ToString("yyyy/MM/dd");
            value.jobTitle = cmbJobTitle.SelectedItem.ToString();
            value.dept = "LMD" + (cmbDept.SelectedIndex + 1).ToString("D2");
            value.phone = tbPhone.Text;
            value.email = tbEmail.Text;
            value.pwd = tbConfirmPass.Text;
        }

        private void btnProFile_Click(object sender, EventArgs e)
        {
            Log.LogMessage(LogLevel.Information, "[View] User Modify", $"User: {UID} is going to the profile page.");
            proFileController = new proFileController(accountController);

            proFileController.setType(accountController.GetAccountType());

            Form proFile = new proFileMain(accountController, UIController, proFileController);
            Hide();
            //Swap the current form to another.
            proFile.StartPosition = FormStartPosition.Manual;
            proFile.Location = Location;
            proFile.Size = Size;
            proFile.ShowDialog();
            Close();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            Log.LogMessage(LogLevel.Information, "[View] User Modify", $"User: {UID} is logging out.");
            Form login = new Login();
            Hide();
            //Swap the current form to another.
            login.StartPosition = FormStartPosition.Manual;
            login.Location = Location;
            login.Size = Size;
            login.ShowDialog();
            Close();
        }

        private void lblCorpName_Click(object sender, EventArgs e)
        {
            Log.LogMessage(LogLevel.Information, "[View] User Modify", $"User: {UID} is going to the about page.");
            Form about = new About(accountController, UIController);
            Hide();
            //Swap the current form to another.
            about.StartPosition = FormStartPosition.Manual;
            about.Location = Location;
            about.Size = Size;
            about.ShowDialog();
            Close();
        }

        private void picHome_Click(object sender, EventArgs e)
        {
            Log.LogMessage(LogLevel.Information, "[View] User Modify", $"User: {UID} is going to the home page.");
            Form home = new Home(accountController, UIController);
            Hide();
            //Swap the current form to another.
            home.StartPosition = FormStartPosition.Manual;
            home.Location = Location;
            home.Size = Size;
            home.ShowDialog();
            Close();
        }

        private void cmbDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            cmbJobTitle.SelectedIndex = -1; //clear the selected value when the province has change.
            cmbJobTitle.Items.Clear(); //clear the value when the selected province has change.
            cmbJobTitle.Items.AddRange(UserController.GetJob(cmbDept.Text)
                .ToArray()); //change city list base on current selected province.
        }

        //For upload and remove IMG function.
        private void btnUploadIMG_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp); *.PNG|*.jpg; *.jpeg; *.gif; *.bmp; *.PNG";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        if (new FileInfo(ofd.FileName).Length > 1000000) //File can't larger than 1MB
                        {
                            MessageBox.Show("File too large! Maximum 1MB.", "System message", MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                            //MessageBox.Show("File too large! Maximum 1MB.");
                            IMGUploaded = false;
                            Log.LogMessage(LogLevel.Warning, "[View] User Modify", $"User: {UID} uploaded an image larger than 1MB.");
                        }
                        else
                        {
                            OwnedImage.Replace(picUserIMG, ref IMG, OwnedImage.LoadDetached(ofd.FileName));
                            btnUploadIMG.Visible = false;
                            btnRemoveIMG.Visible = true;
                            Log.LogMessage(LogLevel.Information, "[View] User Modify", $"User: {UID} uploaded an image.");
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("Illegal operation, please retry.", "System error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                Log.LogMessage(LogLevel.Critical, "[View] User Modify", $"User: {UID} tried to upload an image, but failed.");
                IMGUploaded = false;
            }
        }

        private void btnRemoveIMG_Click(object sender, EventArgs e)
        {
            OwnedImage.Replace(picUserIMG, ref IMG, null);
            btnUploadIMG.Visible = true;
            btnRemoveIMG.Visible = false;
            Log.LogMessage(LogLevel.Information, "[View] User Modify", $"User: {UID} removed the image.");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTimeDate.Text = DateTime.Now.ToString("yyyy/MM/dd   HH:mm:ss");
        }
    }
}
