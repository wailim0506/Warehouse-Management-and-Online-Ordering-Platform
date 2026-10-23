using System;
using System.Windows.Forms;
using LMCIS.controller;
using LMCIS.controller.Utilities;
using Microsoft.Extensions.Logging;

namespace LMCIS.System_page
{
    public partial class PasswordRe : Form
    {
        RecoveryController recoveryController;
        Boolean userFound;
        string UID, email, phone;

        public PasswordRe()
        {
            InitializeComponent();
            userFound = false;
        }

        public PasswordRe(RecoveryController recoveryController)
        {
            InitializeComponent();
            this.recoveryController = recoveryController;
            userFound = false;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Log.LogMessage(LogLevel.Information, "[View] Password Recovery", $"Guest user is loaded the form.");
            timer1.Enabled = true;
            tbPassword.PasswordChar = tbConfirmPass.PasswordChar = '*';
        }

        private void btnToLogin_Click(object sender, EventArgs e)
        {
            Form Login = new Login();
            Hide();
            //Swap the current form to another.
            Login.StartPosition = FormStartPosition.Manual;
            Login.Location = Location;
            Login.Size = Size;
            Login.ShowDialog();
            Close();
        }

        private void btnChangePass_Click(object sender, EventArgs e)
        {
            //Clean the content in the label.
            lblChangePassMsg.Text = "";

            //Show the error if the user hasn't found the account yet.
            if (!userFound)
            {
                lblChangePassMsg.Text = "Please find the account first.";
                tbUserID.Select();
            }
            else
            {
                if (tbPassword.Text.Length < 10 || tbPassword.Text.Length > 50 ||
                    tbPassword.Text.Equals("")) //Too short.
                {
                    Log.LogMessage(Microsoft.Extensions.Logging.LogLevel.Debug, "PasswordRe",
                        $"btnChangePass_Click method User id: {UID} Password too short or too long, minimum 10 maximum 50.");
                    lblChangePassMsg.Text = "Password too short or too long, minimum 10 maximum 50.";
                    tbPassword.Select();
                }
                else
                {
                    if (tbPassword.Text.Equals(tbConfirmPass.Text)) //Confirm password matched.
                    {
                        if (!recoveryController.ChangePassword(tbConfirmPass.Text))
                        {
                            userFound = false;
                            lblChangePassMsg.Text = "Please find the account first.";
                            return;
                        }
                        MessageBox.Show("Password changed! The system will redirect to the login page.",
                            "System message");
                        Form login = new Login();
                        Hide();
                        //Swap the current form to another.
                        login.StartPosition = FormStartPosition.Manual;
                        login.Location = Location;
                        login.Size = Size;
                        login.ShowDialog();
                        Close();
                    }
                    else //NOT match.
                    {
                        lblChangePassMsg.Text = "Passwords do NOT match.";
                        tbConfirmPass.Select();
                        Log.LogMessage(LogLevel.Warning, "[View] Password Recovery", $"Guest user entered a different password.");
                    }
                }
            }
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            //Clean the content in the label.
            lblFinfMsg.Text = "";
            lblChangePassMsg.Text = "";
            userFound = false;
            int count = 0;
            UID = phone = email = "";

            //Counting the inputted information.
            if (!string.IsNullOrEmpty(tbUserID.Text))
            {
                count += 2;
                UID = tbUserID.Text;
            }

            if (!string.IsNullOrEmpty(tbPhone.Text))
            {
                count++;
                phone = tbPhone.Text;
            }

            if (!string.IsNullOrEmpty(tbEmail.Text))
            {
                count++;
                email = tbEmail.Text;
            }

            //Show the error if the user entered less than any two types of information.
            if (count < 3)
            {
                lblFinfMsg.Text = "Please at least enter any two types of information.";
                tbUserID.Select();
                userFound = false;
            }
            else
            {
                if (recoveryController.FindUser(UID, email, phone)) //User found
                {
                    lblFinfMsg.Text = "User found!";
                    tbPassword.Select();
                    userFound = true;
                }
                else //User NOT found
                {
                    lblFinfMsg.Text = "User NOT found, please retry.";
                    tbUserID.Select();
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTimeDate.Text = DateTime.Now.ToString("yyyy/MM/dd   HH:mm:ss");
        }
    }
}
