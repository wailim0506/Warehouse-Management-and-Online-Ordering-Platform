namespace LMCIS.System_page
{
    partial class PasswordRe
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PasswordRe));
            this.panel2 = new System.Windows.Forms.Panel();
            this.lblTimeDate = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.lblFormTitle = new System.Windows.Forms.Label();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPhone = new System.Windows.Forms.Label();
            this.grpFindAccound = new System.Windows.Forms.GroupBox();
            this.lblFinfMsg = new System.Windows.Forms.Label();
            this.btnFind = new System.Windows.Forms.Button();
            this.tbUserID = new System.Windows.Forms.TextBox();
            this.lblUserID = new System.Windows.Forms.Label();
            this.tbPhone = new System.Windows.Forms.TextBox();
            this.tbEmail = new System.Windows.Forms.TextBox();
            this.grpChangePass = new System.Windows.Forms.GroupBox();
            this.lblChangePassMsg = new System.Windows.Forms.Label();
            this.btnChangePass = new System.Windows.Forms.Button();
            this.lblPass = new System.Windows.Forms.Label();
            this.tbConfirmPass = new System.Windows.Forms.TextBox();
            this.lblConfirmPass = new System.Windows.Forms.Label();
            this.tbPassword = new System.Windows.Forms.TextBox();
            this.btnToLogin = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.grpFindAccound.SuspendLayout();
            this.grpChangePass.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.panel2.Controls.Add(this.lblTimeDate);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.panel2.Location = new System.Drawing.Point(0, 0);
            this.panel2.Margin = new System.Windows.Forms.Padding(2);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(1170, 40);
            this.panel2.TabIndex = 1;
            // 
            // lblTimeDate
            // 
            this.lblTimeDate.AutoSize = true;
            this.lblTimeDate.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeDate.Location = new System.Drawing.Point(6, 9);
            this.lblTimeDate.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblTimeDate.Name = "lblTimeDate";
            this.lblTimeDate.Size = new System.Drawing.Size(57, 22);
            this.lblTimeDate.TabIndex = 0;
            this.lblTimeDate.Text = "TIME";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ControlLight;
            this.panel3.Controls.Add(this.lblFormTitle);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel3.Location = new System.Drawing.Point(0, 40);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(1170, 38);
            this.panel3.TabIndex = 2;
            // 
            // lblFormTitle
            // 
            this.lblFormTitle.AutoSize = true;
            this.lblFormTitle.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblFormTitle.Location = new System.Drawing.Point(6, 9);
            this.lblFormTitle.Name = "lblFormTitle";
            this.lblFormTitle.Size = new System.Drawing.Size(169, 22);
            this.lblFormTitle.TabIndex = 0;
            this.lblFormTitle.Text = "Password Recovery";
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Font = new System.Drawing.Font("Times New Roman", 13F);
            this.lblEmail.Location = new System.Drawing.Point(34, 97);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(120, 20);
            this.lblEmail.TabIndex = 3;
            this.lblEmail.Text = "Registed Email:";
            // 
            // lblPhone
            // 
            this.lblPhone.AutoSize = true;
            this.lblPhone.Font = new System.Drawing.Font("Times New Roman", 13F);
            this.lblPhone.Location = new System.Drawing.Point(34, 130);
            this.lblPhone.Name = "lblPhone";
            this.lblPhone.Size = new System.Drawing.Size(59, 20);
            this.lblPhone.TabIndex = 4;
            this.lblPhone.Text = "Phone:";
            // 
            // grpFindAccound
            // 
            this.grpFindAccound.Controls.Add(this.lblFinfMsg);
            this.grpFindAccound.Controls.Add(this.btnFind);
            this.grpFindAccound.Controls.Add(this.tbUserID);
            this.grpFindAccound.Controls.Add(this.lblUserID);
            this.grpFindAccound.Controls.Add(this.tbPhone);
            this.grpFindAccound.Controls.Add(this.tbEmail);
            this.grpFindAccound.Controls.Add(this.lblEmail);
            this.grpFindAccound.Controls.Add(this.lblPhone);
            this.grpFindAccound.Font = new System.Drawing.Font("Times New Roman", 15F);
            this.grpFindAccound.Location = new System.Drawing.Point(101, 150);
            this.grpFindAccound.Name = "grpFindAccound";
            this.grpFindAccound.Size = new System.Drawing.Size(555, 181);
            this.grpFindAccound.TabIndex = 5;
            this.grpFindAccound.TabStop = false;
            this.grpFindAccound.Text = "Find Accound";
            // 
            // lblFinfMsg
            // 
            this.lblFinfMsg.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.lblFinfMsg.ForeColor = System.Drawing.Color.Red;
            this.lblFinfMsg.Location = new System.Drawing.Point(34, 35);
            this.lblFinfMsg.Name = "lblFinfMsg";
            this.lblFinfMsg.Size = new System.Drawing.Size(515, 19);
            this.lblFinfMsg.TabIndex = 110;
            // 
            // btnFind
            // 
            this.btnFind.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFind.Location = new System.Drawing.Point(397, 61);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(132, 93);
            this.btnFind.TabIndex = 4;
            this.btnFind.Text = "Find";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // tbUserID
            // 
            this.tbUserID.Font = new System.Drawing.Font("Times New Roman", 13F);
            this.tbUserID.Location = new System.Drawing.Point(179, 61);
            this.tbUserID.Name = "tbUserID";
            this.tbUserID.Size = new System.Drawing.Size(177, 27);
            this.tbUserID.TabIndex = 1;
            // 
            // lblUserID
            // 
            this.lblUserID.AutoSize = true;
            this.lblUserID.Font = new System.Drawing.Font("Times New Roman", 13F);
            this.lblUserID.Location = new System.Drawing.Point(34, 64);
            this.lblUserID.Name = "lblUserID";
            this.lblUserID.Size = new System.Drawing.Size(139, 20);
            this.lblUserID.TabIndex = 107;
            this.lblUserID.Text = "UserID (Require):";
            // 
            // tbPhone
            // 
            this.tbPhone.Font = new System.Drawing.Font("Times New Roman", 13F);
            this.tbPhone.Location = new System.Drawing.Point(179, 127);
            this.tbPhone.Name = "tbPhone";
            this.tbPhone.Size = new System.Drawing.Size(177, 27);
            this.tbPhone.TabIndex = 3;
            // 
            // tbEmail
            // 
            this.tbEmail.Font = new System.Drawing.Font("Times New Roman", 13F);
            this.tbEmail.Location = new System.Drawing.Point(179, 94);
            this.tbEmail.Name = "tbEmail";
            this.tbEmail.Size = new System.Drawing.Size(177, 27);
            this.tbEmail.TabIndex = 2;
            // 
            // grpChangePass
            // 
            this.grpChangePass.Controls.Add(this.lblChangePassMsg);
            this.grpChangePass.Controls.Add(this.btnChangePass);
            this.grpChangePass.Controls.Add(this.lblPass);
            this.grpChangePass.Controls.Add(this.tbConfirmPass);
            this.grpChangePass.Controls.Add(this.lblConfirmPass);
            this.grpChangePass.Controls.Add(this.tbPassword);
            this.grpChangePass.Font = new System.Drawing.Font("Times New Roman", 15F);
            this.grpChangePass.Location = new System.Drawing.Point(101, 371);
            this.grpChangePass.Name = "grpChangePass";
            this.grpChangePass.Size = new System.Drawing.Size(555, 168);
            this.grpChangePass.TabIndex = 6;
            this.grpChangePass.TabStop = false;
            this.grpChangePass.Text = "Change Password:";
            // 
            // lblChangePassMsg
            // 
            this.lblChangePassMsg.Font = new System.Drawing.Font("Times New Roman", 12F);
            this.lblChangePassMsg.ForeColor = System.Drawing.Color.Red;
            this.lblChangePassMsg.Location = new System.Drawing.Point(34, 35);
            this.lblChangePassMsg.Name = "lblChangePassMsg";
            this.lblChangePassMsg.Size = new System.Drawing.Size(495, 19);
            this.lblChangePassMsg.TabIndex = 111;
            // 
            // btnChangePass
            // 
            this.btnChangePass.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnChangePass.Font = new System.Drawing.Font("Times New Roman", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnChangePass.Location = new System.Drawing.Point(392, 55);
            this.btnChangePass.Name = "btnChangePass";
            this.btnChangePass.Size = new System.Drawing.Size(144, 84);
            this.btnChangePass.TabIndex = 7;
            this.btnChangePass.Text = "Change Password";
            this.btnChangePass.UseVisualStyleBackColor = false;
            this.btnChangePass.Click += new System.EventHandler(this.btnChangePass_Click);
            // 
            // lblPass
            // 
            this.lblPass.AutoSize = true;
            this.lblPass.Font = new System.Drawing.Font("Times New Roman", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPass.Location = new System.Drawing.Point(34, 68);
            this.lblPass.Name = "lblPass";
            this.lblPass.Size = new System.Drawing.Size(79, 19);
            this.lblPass.TabIndex = 101;
            this.lblPass.Text = "Password:";
            // 
            // tbConfirmPass
            // 
            this.tbConfirmPass.Font = new System.Drawing.Font("Times New Roman", 13F);
            this.tbConfirmPass.Location = new System.Drawing.Point(188, 101);
            this.tbConfirmPass.Name = "tbConfirmPass";
            this.tbConfirmPass.Size = new System.Drawing.Size(177, 27);
            this.tbConfirmPass.TabIndex = 6;
            // 
            // lblConfirmPass
            // 
            this.lblConfirmPass.AutoSize = true;
            this.lblConfirmPass.Font = new System.Drawing.Font("Times New Roman", 12.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblConfirmPass.Location = new System.Drawing.Point(34, 104);
            this.lblConfirmPass.Name = "lblConfirmPass";
            this.lblConfirmPass.Size = new System.Drawing.Size(139, 19);
            this.lblConfirmPass.TabIndex = 100;
            this.lblConfirmPass.Text = "Confirm password:";
            // 
            // tbPassword
            // 
            this.tbPassword.Font = new System.Drawing.Font("Times New Roman", 13F);
            this.tbPassword.Location = new System.Drawing.Point(188, 65);
            this.tbPassword.Name = "tbPassword";
            this.tbPassword.Size = new System.Drawing.Size(177, 27);
            this.tbPassword.TabIndex = 5;
            // 
            // btnToLogin
            // 
            this.btnToLogin.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnToLogin.Location = new System.Drawing.Point(101, 584);
            this.btnToLogin.Name = "btnToLogin";
            this.btnToLogin.Size = new System.Drawing.Size(217, 37);
            this.btnToLogin.TabIndex = 8;
            this.btnToLogin.Text = " Back to the login page";
            this.btnToLogin.UseVisualStyleBackColor = true;
            this.btnToLogin.Click += new System.EventHandler(this.btnToLogin_Click);
            // 
            // PasswordRe
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 941);
            this.Controls.Add(this.btnToLogin);
            this.Controls.Add(this.grpChangePass);
            this.Controls.Add(this.grpFindAccound);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "PasswordRe";
            this.Text = "AutoNexus Business Management System";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.grpFindAccound.ResumeLayout(false);
            this.grpFindAccound.PerformLayout();
            this.grpChangePass.ResumeLayout(false);
            this.grpChangePass.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lblTimeDate;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblFormTitle;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblPhone;
        private System.Windows.Forms.GroupBox grpFindAccound;
        private System.Windows.Forms.GroupBox grpChangePass;
        private System.Windows.Forms.Label lblFinfMsg;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.TextBox tbUserID;
        private System.Windows.Forms.Label lblUserID;
        private System.Windows.Forms.TextBox tbPhone;
        private System.Windows.Forms.TextBox tbEmail;
        private System.Windows.Forms.Label lblChangePassMsg;
        private System.Windows.Forms.Button btnChangePass;
        private System.Windows.Forms.Label lblPass;
        private System.Windows.Forms.TextBox tbConfirmPass;
        private System.Windows.Forms.Label lblConfirmPass;
        private System.Windows.Forms.TextBox tbPassword;
        private System.Windows.Forms.Button btnToLogin;
    }
}

