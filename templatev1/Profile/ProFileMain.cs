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
using LMCIS.Properties;
using LMCIS.Stock_Manag;
using LMCIS.System_page;
using LMCIS.User_Manag;
using Microsoft.Extensions.Logging;

namespace LMCIS.Profile
{
    public partial class proFileMain : Form
    {
        private string uName, UID;
        Bitmap IMG;
        dynamic placeholder, update;
        bool IMGUploaded;

        AccountController accountController;
        UIController UIController;
        proFileController proFileController;


        public proFileMain()
        {
            InitializeComponent();
        }

        public proFileMain(AccountController accountController, UIController UIController,
            proFileController proFileController)
        {
            InitializeComponent();

            this.accountController = accountController;
            this.UIController = UIController;
            this.proFileController = proFileController;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Initialization();
            Log.LogMessage(LogLevel.Information, "[View] Profile", $"User: {UID} is loaded the form.");
            tbOldPass.PasswordChar = tbPass.PasswordChar = tbConfirmPass.PasswordChar = '*';
        }

        private void Initialization()
        {
            timer1.Enabled = true;
            UID = accountController.GetUid();
            uName = accountController.GetName();
            lblUid.Text = "UID: " + UID;
            lblUserUID.Text = UID;
            placeholder = proFileController.getUserInfo();

            //Show user information
            dynamic info = proFileController.getUserInfo();
            lblAccType.Text = info.accountType;
            lblJobTitle.Text = info.jobTitle;
            lblDept.Text = info.dept;
            lblEmail.Text = info.email;
            tbFirstName.Text = info.fName;
            tbLastName.Text = info.lName;
            cmbGender.Text = info.sex;
            tbPhone.Text = info.phone;
            dtpDateOfBirth.Value = DateTime.ParseExact((info.dateOfBirth).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null);
            lblCreateDate.Text = (info.createDate).ToString("yyyy/MM/dd");
            chkNGDateOfBirth.Checked = info.NGDateOfBirth;
            lblCorpAddress.Text = info.caddress;
            cmbPayment.Text = info.payment;
            tbCorp.Text = info.corp;
            lblWareAddress.Text = info.waddress;


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


            //For swap the form betwee staff and customer
            dynamic show = UIController.proFile();
            palSatffInfo.Visible = show.group1;
            palSCMode.Visible = btnDelete.Visible = show.group2;

            //For icon color
            if (Settings.Default.BWmode)
            {
                picBWMode.Image = Resources.LBWhite;
                picHome.Image = Resources.homeWhite;
            }

            if (proFileController.CheckUserAvatar(UID))
            {
                string avatarPath = proFileController.GetUserAvatar(UID);
                if (avatarPath != null)
                {
                    picUserIMG.Image = new Bitmap(avatarPath);
                    IMGUploaded = true;
                }
                else
                {
                    IMGUploaded = false;
                    //btnRemoveIMG.Visible = false;
                }
            }
            else
            {
                IMGUploaded = false;
                btnRemoveIMG.Visible = false;
            }
        }

        //To determine and show the next page.
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

            Hide();
            next.StartPosition = FormStartPosition.Manual;
            next.Location = Location;
            next.Size = Size;
            next.ShowDialog();
            Close();
        }

        private void btnProFile_Click(object sender, EventArgs e)
        {
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

        private void lblCorpName_Click(object sender, EventArgs e)
        {
            Form about = new About(accountController, UIController);
            Hide();
            //Swap the current form to another.
            about.StartPosition = FormStartPosition.Manual;
            about.Location = Location;
            about.Size = Size;
            about.ShowDialog();
            Close();
        }

        private void picBWMode_Click(object sender, EventArgs e)
        {
            UIController.setMode(Settings.Default.BWmode);
            BWMode();
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            Form login = new Login();
            Hide();
            //Swap the current form to another.
            login.StartPosition = FormStartPosition.Manual;
            login.Location = Location;
            login.Size = Size;
            login.ShowDialog();
            Close();
        }

        private void picHome_Click(object sender, EventArgs e)
        {
            Form home = new Home(accountController, UIController);
            Hide();
            //Swap the current form to another.
            home.StartPosition = FormStartPosition.Manual;
            home.Location = Location;
            home.Size = Size;
            home.ShowDialog();
            Close();
        }

        //For Dark Color function
        private void BWMode()
        {
            dynamic value = UIController.getMode();
            Settings.Default.textColor = ColorTranslator.FromHtml(value.textColor);
            Settings.Default.bgColor = ColorTranslator.FromHtml(value.bgColor);
            Settings.Default.navBarColor = ColorTranslator.FromHtml(value.navBarColor);
            Settings.Default.navColor = ColorTranslator.FromHtml(value.navColor);
            Settings.Default.timeColor = ColorTranslator.FromHtml(value.timeColor);
            Settings.Default.locTbColor = ColorTranslator.FromHtml(value.locTbColor);
            Settings.Default.logoutColor = ColorTranslator.FromHtml(value.logoutColor);
            Settings.Default.profileColor = ColorTranslator.FromHtml(value.profileColor);
            Settings.Default.btnColor = ColorTranslator.FromHtml(value.btnColor);
            Settings.Default.BWmode = value.BWmode;
            if (Settings.Default.BWmode)
            {
                picBWMode.Image = Resources.LBWhite;
                picHome.Image = Resources.homeWhite;
            }
            else
            {
                picBWMode.Image = Resources.LB;
                picHome.Image = Resources.home;
            }
        }

        private void btnManagAddress_Click(object sender, EventArgs e)
        {
            Form AddressMgmt = new AddressMgmt(accountController, UIController, proFileController);
            Hide();
            //Swap the current form to another.
            AddressMgmt.StartPosition = FormStartPosition.Manual;
            AddressMgmt.Location = Location;
            AddressMgmt.Size = Size;
            AddressMgmt.ShowDialog();
            Close();
        }

        //The placeholder for textbox.
        private void tbFirstName_Enter(object sender, EventArgs e)
        {
            if (tbFirstName.Text == placeholder.fName)
                tbFirstName.Text = "";
        }

        private void tbFirstName_Leave(object sender, EventArgs e)
        {
            if (tbFirstName.Text == "")
                tbFirstName.Text = placeholder.fName;
        }

        private void tbLastName_Enter(object sender, EventArgs e)
        {
            if (tbLastName.Text == placeholder.lName)
                tbLastName.Text = "";
        }

        private void tbLastName_Leave(object sender, EventArgs e)
        {
            if (tbLastName.Text == "")
                tbLastName.Text = placeholder.lName;
        }

        private void tbPhone_Enter(object sender, EventArgs e)
        {
            if (tbPhone.Text == placeholder.phone)
                tbPhone.Text = "";
        }

        private void tbPhone_Leave(object sender, EventArgs e)
        {
            if (tbPhone.Text == "")
                tbPhone.Text = placeholder.phone;
        }

        private void tbCorp_Enter(object sender, EventArgs e)
        {
            if (tbCorp.Text == placeholder.corp)
                tbCorp.Text = "";
        }

        private void tbCorp_Leave(object sender, EventArgs e)
        {
            if (tbCorp.Text == "")
                tbCorp.Text = placeholder.corp;
        }

        private bool checkInfo()
        {
            lblfNameMsg.Text = lbllNameMsg.Text = lblDateMsg.Text = lblContactMsg.Text = lblPhoneMsg.Text = "";
            update = new ExpandoObject();

            //Check and update firstName if have change.
            if (tbFirstName.Text != placeholder.fName)
            {
                if (tbFirstName.Text.Length < 2 || tbFirstName.Text.Length > 20)
                {
                    lblfNameMsg.Text = "FirstName too short or too long, minimum 2 maximum 20.";
                    tbFirstName.Select();
                    return false;
                }

                update.fName = tbFirstName.Text;
            }
            else
                update.fName = placeholder.fName;

            //Check and update lastName if have change.
            if (tbLastName.Text != placeholder.lName)
            {
                if (tbLastName.Text.Length < 2 || tbLastName.Text.Length > 20)
                {
                    lbllNameMsg.Text = "LastName too short or too long, minimum 2 maximum 20.";
                    tbLastName.Select();
                    return false;
                }

                update.lName = tbLastName.Text;
            }
            else
                update.lName = placeholder.lName;

            //Check and update gender if have change.
            update.sex = cmbGender.SelectedIndex == 0 ? "M" : "F";

            //Check and update date of birth if have change.
            if (dtpDateOfBirth.Value !=
                DateTime.ParseExact((placeholder.dateOfBirth).ToString("dd/MM/yyyy"), "dd/MM/yyyy", null) ||
                !chkNGDateOfBirth.Checked)
            {
                if (!chkNGDateOfBirth.Checked && (dtpDateOfBirth.Value.Date > DateTime.Now.Date ||
                                                  dtpDateOfBirth.Value.Date > new DateTime(2007, 1, 1)))
                {
                    lblDateMsg.Text = UID.StartsWith("LMS")
                        ? "Please select a valid date or click NOT provided."
                        : "Please select a valid date or click NOT provided.";
                    return false;
                }

                update.DFB = dtpDateOfBirth.Value.ToString("yyyy-MM-dd");
            }
            else if (chkNGDateOfBirth.Checked)
                update.DFB = DBNull.Value;
            else
                update.DFB = dtpDateOfBirth.Value.ToString("yyyy-MM-dd");

            //Check and update phone if have change.
            if (tbPhone.Text != placeholder.phone)
            {
                if (!Regex.Match(tbPhone.Text, @"^([0-9]{11})$").Success &&
                    !Regex.Match(tbPhone.Text, @"^([0-9]{8})$").Success)
                {
                    lblPhoneMsg.Text = "Please enter the correct phone format.";
                    tbPhone.Select();
                    return false;
                }

                if (!proFileController.CheckEmailPhone(tbPhone.Text))
                {
                    lblPhoneMsg.Text = "The phone number has already registered an account.";
                    tbPhone.Select();
                    return false;
                }

                update.phone = tbPhone.Text;
            }
            else
                update.phone = placeholder.phone;


            if (placeholder.accountType.Equals("Customer"))
            {
                update.pay = cmbPayment.Text; //Check and update payment if have change.

                if (tbCorp.Text != placeholder.corp) //Check and update company name if have change.
                {
                    if (tbCorp.Text.Length > 30)
                    {
                        lblContactMsg.Text = "Company name too long, maximum 30.";
                        tbCorp.Select();
                        return false;
                    }

                    update.corp = tbCorp.Text;
                }
                else
                    update.corp = placeholder.corp;
            }

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Form home = new Home(accountController, UIController);
            Hide();
            //Swap the current form to another.
            home.StartPosition = FormStartPosition.Manual;
            home.Location = Location;
            home.Size = Size;
            home.ShowDialog();
            Close();
        }

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (checkInfo())
                if (proFileController.ModifyUserInfo(update))
                {
                    MessageBox.Show("Modify successful!", "System message", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    btnProFile_Click(this, e); //Refresh the profile page.
                }
                else //Something wrong from the controller.
                    MessageBox.Show("System Error! Please Contact The Help Desk.", "System error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
        }

        //For upload and remove IMG function.
        private void btnUploadIMG_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp); *.PNG|*.jpg; *.jpeg; *.gif; *.bmp; *.PNG";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if (new FileInfo(ofd.FileName).Length > 1000000) //File can't larger than 1MB
                    {
                        MessageBox.Show("File too large! Maximum 1MB.", "System message", MessageBoxButtons.OK,
                            MessageBoxIcon.Warning);
                        //MessageBox.Show("File too large! Maximum 1MB.");
                        IMGUploaded = false;
                    }
                    else
                    {
                        IMG = new Bitmap(Image.FromFile(ofd.FileName));
                        btnUploadIMG.Visible = false;
                        btnRemoveIMG.Visible = true;
                        picUserIMG.Image = IMG;
                        IMGUploaded = proFileController.UploadUserAvatar(ofd.FileName, UID);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Illegal operation, please retry.", "System error", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
                IMGUploaded = false;
            }
        }

        private void btnRemoveIMG_Click(object sender, EventArgs e)
        {
            IMG = null;
            btnUploadIMG.Visible = true;
            btnRemoveIMG.Visible = false;
            picUserIMG.Image = IMG;
            proFileController.DeleteUserAvatar(UID);
        }

        //For change password function.
        private void btnChangePwd_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbOldPass.Text))
            {
                if (accountController.MatchPwd(tbOldPass.Text))
                {
                    if (checkPwd())
                    {
                        RecoveryController recoveryController =
                            new RecoveryController(accountController);
                        if (recoveryController.ChangePassword(tbConfirmPass.Text))
                        {
                            MessageBox.Show("Password changed successful!\nThe system will redirect to the login page.",
                                "System message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            btnLogOut_Click(this, e); //Redirect to login page.
                        }
                        else
                        {
                            lblPwdMsg.Text = "Unable to verify the account. Please log in again.";
                        }
                    }
                }
                else
                    lblPwdMsg.Text = "Incorrect old password.";
            }
            else
                lblPwdMsg.Text = "Please enter old password.";
        }

        private bool checkPwd()
        {
            if (string.IsNullOrEmpty(tbPass.Text)) //Check whether the password and confirm the password match.
            {
                lblPwdMsg.Text = "Please enter the new password.";
                return false;
            }

            if (tbPass.Text.Length < 10 || tbPass.Text.Length > 50)
            {
                lblPwdMsg.Text = "New password too short or too long, minimum 10 maximum 50.";
                return false;
            }

            if (!tbPass.Text.Equals(tbConfirmPass.Text))
            {
                lblPwdMsg.Text = "Confirm password does NOT match.";
                return false;
            }

            return true;
        }


        //Delete the account, only available for the customer account.
        private void btnDelete_Click(object sender, EventArgs e)
        {
            var result =
                MessageBox.Show(
                    "Are you sure to delete your account?\nThis operation also deletes all sub-accounts belonging to this account.\nClick Yes to continue.",
                    "System message", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result == DialogResult.Yes)
            {
                if (accountController.DelAccount())
                {
                    MessageBox.Show("Account deleted successful.\nThe system will redirect to the login page.",
                        "System message", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    btnLogOut_Click(this, e); //Redirect to login page.
                }
                else
                {
                    MessageBox.Show("System Error! Please Contact The Help Desk.", "System error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    btnLogOut_Click(this, e); //Redirect to login page.
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTimeDate.Text = DateTime.Now.ToString("yyyy/MM/dd   HH:mm:ss");
        }
    }
}
