namespace LMCIS.System_page
{
    partial class LogHis
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LogHis));
            this.palNav = new System.Windows.Forms.Panel();
            this.picBWMode = new System.Windows.Forms.PictureBox();
            this.btnProFile = new System.Windows.Forms.Button();
            this.btnLogOut = new System.Windows.Forms.Button();
            this.picHome = new System.Windows.Forms.PictureBox();
            this.lblCorpName = new System.Windows.Forms.Label();
            this.btnFunction5 = new System.Windows.Forms.Button();
            this.btnFunction4 = new System.Windows.Forms.Button();
            this.btnFunction3 = new System.Windows.Forms.Button();
            this.btnFunction2 = new System.Windows.Forms.Button();
            this.btnFunction1 = new System.Windows.Forms.Button();
            this.palTime = new System.Windows.Forms.Panel();
            this.lblUid = new System.Windows.Forms.Label();
            this.lblTimeDate = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.palLoc = new System.Windows.Forms.Panel();
            this.lblLoc = new System.Windows.Forms.Label();
            this.lblDate = new System.Windows.Forms.Label();
            this.dgvLog = new System.Windows.Forms.DataGridView();
            this.palNav.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBWMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHome)).BeginInit();
            this.palTime.SuspendLayout();
            this.palLoc.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).BeginInit();
            this.SuspendLayout();
            // 
            // palNav
            // 
            this.palNav.BackColor = global::LMCIS.Properties.Settings.Default.navColor;
            this.palNav.Controls.Add(this.picBWMode);
            this.palNav.Controls.Add(this.btnProFile);
            this.palNav.Controls.Add(this.btnLogOut);
            this.palNav.Controls.Add(this.picHome);
            this.palNav.Controls.Add(this.lblCorpName);
            this.palNav.Controls.Add(this.btnFunction5);
            this.palNav.Controls.Add(this.btnFunction4);
            this.palNav.Controls.Add(this.btnFunction3);
            this.palNav.Controls.Add(this.btnFunction2);
            this.palNav.Controls.Add(this.btnFunction1);
            this.palNav.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::LMCIS.Properties.Settings.Default, "navColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.palNav.Dock = System.Windows.Forms.DockStyle.Left;
            this.palNav.Location = new System.Drawing.Point(0, 0);
            this.palNav.Margin = new System.Windows.Forms.Padding(2);
            this.palNav.Name = "palNav";
            this.palNav.Size = new System.Drawing.Size(198, 941);
            this.palNav.TabIndex = 0;
            // 
            // picBWMode
            // 
            this.picBWMode.Image = global::LMCIS.Properties.Resources.LB;
            this.picBWMode.Location = new System.Drawing.Point(143, 24);
            this.picBWMode.Name = "picBWMode";
            this.picBWMode.Size = new System.Drawing.Size(49, 46);
            this.picBWMode.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picBWMode.TabIndex = 22;
            this.picBWMode.TabStop = false;
            this.picBWMode.Click += new System.EventHandler(this.picBWMode_Click);
            // 
            // btnProFile
            // 
            this.btnProFile.BackColor = global::LMCIS.Properties.Settings.Default.profileColor;
            this.btnProFile.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::LMCIS.Properties.Settings.Default, "profileColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnProFile.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::LMCIS.Properties.Settings.Default, "textColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnProFile.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnProFile.FlatAppearance.BorderSize = 0;
            this.btnProFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProFile.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProFile.ForeColor = global::LMCIS.Properties.Settings.Default.textColor;
            this.btnProFile.Location = new System.Drawing.Point(0, 794);
            this.btnProFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnProFile.Name = "btnProFile";
            this.btnProFile.Size = new System.Drawing.Size(198, 34);
            this.btnProFile.TabIndex = 20;
            this.btnProFile.Text = "ProFile";
            this.btnProFile.UseVisualStyleBackColor = false;
            this.btnProFile.Click += new System.EventHandler(this.btnProFile_Click);
            // 
            // btnLogOut
            // 
            this.btnLogOut.BackColor = global::LMCIS.Properties.Settings.Default.logoutColor;
            this.btnLogOut.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::LMCIS.Properties.Settings.Default, "logoutColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnLogOut.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::LMCIS.Properties.Settings.Default, "textColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnLogOut.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnLogOut.FlatAppearance.BorderSize = 0;
            this.btnLogOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogOut.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnLogOut.ForeColor = global::LMCIS.Properties.Settings.Default.textColor;
            this.btnLogOut.Location = new System.Drawing.Point(0, 836);
            this.btnLogOut.Margin = new System.Windows.Forms.Padding(4);
            this.btnLogOut.Name = "btnLogOut";
            this.btnLogOut.Size = new System.Drawing.Size(198, 34);
            this.btnLogOut.TabIndex = 19;
            this.btnLogOut.Text = "Log Out";
            this.btnLogOut.UseVisualStyleBackColor = false;
            this.btnLogOut.Click += new System.EventHandler(this.btnLogOut_Click);
            // 
            // picHome
            // 
            this.picHome.Image = global::LMCIS.Properties.Resources.home;
            this.picHome.Location = new System.Drawing.Point(13, 13);
            this.picHome.Margin = new System.Windows.Forms.Padding(4);
            this.picHome.Name = "picHome";
            this.picHome.Size = new System.Drawing.Size(57, 56);
            this.picHome.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picHome.TabIndex = 18;
            this.picHome.TabStop = false;
            this.picHome.Click += new System.EventHandler(this.picHome_Click);
            // 
            // lblCorpName
            // 
            this.lblCorpName.Font = new System.Drawing.Font("Times New Roman", 15F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCorpName.ForeColor = System.Drawing.Color.Red;
            this.lblCorpName.Location = new System.Drawing.Point(1, 892);
            this.lblCorpName.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblCorpName.Name = "lblCorpName";
            this.lblCorpName.Size = new System.Drawing.Size(163, 49);
            this.lblCorpName.TabIndex = 10;
            this.lblCorpName.Text = "AutoNexus";
            this.lblCorpName.Click += new System.EventHandler(this.lblCorpName_Click);
            // 
            // btnFunction5
            // 
            this.btnFunction5.BackColor = global::LMCIS.Properties.Settings.Default.navBarColor;
            this.btnFunction5.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::LMCIS.Properties.Settings.Default, "navBarColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnFunction5.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::LMCIS.Properties.Settings.Default, "textColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnFunction5.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(213)))), ((int)(((byte)(184)))));
            this.btnFunction5.FlatAppearance.BorderSize = 0;
            this.btnFunction5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFunction5.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFunction5.ForeColor = global::LMCIS.Properties.Settings.Default.textColor;
            this.btnFunction5.Location = new System.Drawing.Point(0, 371);
            this.btnFunction5.Margin = new System.Windows.Forms.Padding(4);
            this.btnFunction5.Name = "btnFunction5";
            this.btnFunction5.Size = new System.Drawing.Size(198, 55);
            this.btnFunction5.TabIndex = 117;
            this.btnFunction5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFunction5.UseVisualStyleBackColor = false;
            this.btnFunction5.Click += new System.EventHandler(this.btnFunction5_Click);
            // 
            // btnFunction4
            // 
            this.btnFunction4.BackColor = global::LMCIS.Properties.Settings.Default.navBarColor;
            this.btnFunction4.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::LMCIS.Properties.Settings.Default, "navBarColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnFunction4.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::LMCIS.Properties.Settings.Default, "textColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnFunction4.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(213)))), ((int)(((byte)(184)))));
            this.btnFunction4.FlatAppearance.BorderSize = 0;
            this.btnFunction4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFunction4.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFunction4.ForeColor = global::LMCIS.Properties.Settings.Default.textColor;
            this.btnFunction4.Location = new System.Drawing.Point(0, 296);
            this.btnFunction4.Margin = new System.Windows.Forms.Padding(4);
            this.btnFunction4.Name = "btnFunction4";
            this.btnFunction4.Size = new System.Drawing.Size(198, 55);
            this.btnFunction4.TabIndex = 116;
            this.btnFunction4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFunction4.UseVisualStyleBackColor = false;
            this.btnFunction4.Click += new System.EventHandler(this.btnFunction4_Click);
            // 
            // btnFunction3
            // 
            this.btnFunction3.BackColor = global::LMCIS.Properties.Settings.Default.navBarColor;
            this.btnFunction3.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::LMCIS.Properties.Settings.Default, "navBarColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnFunction3.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::LMCIS.Properties.Settings.Default, "textColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnFunction3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(213)))), ((int)(((byte)(184)))));
            this.btnFunction3.FlatAppearance.BorderSize = 0;
            this.btnFunction3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFunction3.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFunction3.ForeColor = global::LMCIS.Properties.Settings.Default.textColor;
            this.btnFunction3.Location = new System.Drawing.Point(0, 223);
            this.btnFunction3.Margin = new System.Windows.Forms.Padding(4);
            this.btnFunction3.Name = "btnFunction3";
            this.btnFunction3.Size = new System.Drawing.Size(198, 55);
            this.btnFunction3.TabIndex = 115;
            this.btnFunction3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFunction3.UseVisualStyleBackColor = false;
            this.btnFunction3.Click += new System.EventHandler(this.btnFunction3_Click);
            // 
            // btnFunction2
            // 
            this.btnFunction2.BackColor = global::LMCIS.Properties.Settings.Default.navBarColor;
            this.btnFunction2.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::LMCIS.Properties.Settings.Default, "navBarColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnFunction2.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::LMCIS.Properties.Settings.Default, "textColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnFunction2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(213)))), ((int)(((byte)(184)))));
            this.btnFunction2.FlatAppearance.BorderSize = 0;
            this.btnFunction2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFunction2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFunction2.ForeColor = global::LMCIS.Properties.Settings.Default.textColor;
            this.btnFunction2.Location = new System.Drawing.Point(0, 150);
            this.btnFunction2.Margin = new System.Windows.Forms.Padding(4);
            this.btnFunction2.Name = "btnFunction2";
            this.btnFunction2.Size = new System.Drawing.Size(198, 55);
            this.btnFunction2.TabIndex = 114;
            this.btnFunction2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFunction2.UseVisualStyleBackColor = false;
            this.btnFunction2.Click += new System.EventHandler(this.btnFunction2_Click);
            // 
            // btnFunction1
            // 
            this.btnFunction1.BackColor = global::LMCIS.Properties.Settings.Default.navBarColor;
            this.btnFunction1.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::LMCIS.Properties.Settings.Default, "navBarColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnFunction1.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::LMCIS.Properties.Settings.Default, "textColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnFunction1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(213)))), ((int)(((byte)(184)))));
            this.btnFunction1.FlatAppearance.BorderSize = 0;
            this.btnFunction1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFunction1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFunction1.ForeColor = global::LMCIS.Properties.Settings.Default.textColor;
            this.btnFunction1.Location = new System.Drawing.Point(0, 77);
            this.btnFunction1.Margin = new System.Windows.Forms.Padding(4);
            this.btnFunction1.Name = "btnFunction1";
            this.btnFunction1.Size = new System.Drawing.Size(198, 55);
            this.btnFunction1.TabIndex = 113;
            this.btnFunction1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFunction1.UseVisualStyleBackColor = false;
            this.btnFunction1.Click += new System.EventHandler(this.btnFunction1_Click);
            // 
            // palTime
            // 
            this.palTime.BackColor = global::LMCIS.Properties.Settings.Default.timeColor;
            this.palTime.Controls.Add(this.lblUid);
            this.palTime.Controls.Add(this.lblTimeDate);
            this.palTime.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::LMCIS.Properties.Settings.Default, "timeColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.palTime.Dock = System.Windows.Forms.DockStyle.Top;
            this.palTime.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.palTime.Location = new System.Drawing.Point(198, 0);
            this.palTime.Margin = new System.Windows.Forms.Padding(2);
            this.palTime.Name = "palTime";
            this.palTime.Size = new System.Drawing.Size(972, 40);
            this.palTime.TabIndex = 1;
            // 
            // lblUid
            // 
            this.lblUid.AutoSize = true;
            this.lblUid.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUid.Location = new System.Drawing.Point(814, 9);
            this.lblUid.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUid.Name = "lblUid";
            this.lblUid.Size = new System.Drawing.Size(43, 22);
            this.lblUid.TabIndex = 1;
            this.lblUid.Text = "UID";
            // 
            // lblTimeDate
            // 
            this.lblTimeDate.AutoSize = true;
            this.lblTimeDate.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::LMCIS.Properties.Settings.Default, "textColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblTimeDate.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTimeDate.ForeColor = global::LMCIS.Properties.Settings.Default.textColor;
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
            // palLoc
            // 
            this.palLoc.BackColor = global::LMCIS.Properties.Settings.Default.locTbColor;
            this.palLoc.Controls.Add(this.lblLoc);
            this.palLoc.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::LMCIS.Properties.Settings.Default, "locTbColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.palLoc.Dock = System.Windows.Forms.DockStyle.Top;
            this.palLoc.Location = new System.Drawing.Point(198, 40);
            this.palLoc.Name = "palLoc";
            this.palLoc.Size = new System.Drawing.Size(972, 38);
            this.palLoc.TabIndex = 2;
            // 
            // lblLoc
            // 
            this.lblLoc.AutoSize = true;
            this.lblLoc.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::LMCIS.Properties.Settings.Default, "textColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblLoc.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoc.ForeColor = global::LMCIS.Properties.Settings.Default.textColor;
            this.lblLoc.Location = new System.Drawing.Point(6, 9);
            this.lblLoc.Name = "lblLoc";
            this.lblLoc.Size = new System.Drawing.Size(241, 22);
            this.lblLoc.TabIndex = 0;
            this.lblLoc.Text = "Home -> View Login History";
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::LMCIS.Properties.Settings.Default, "textColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblDate.Font = new System.Drawing.Font("Times New Roman", 15F);
            this.lblDate.ForeColor = global::LMCIS.Properties.Settings.Default.textColor;
            this.lblDate.Location = new System.Drawing.Point(285, 110);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(87, 22);
            this.lblDate.TabIndex = 111;
            this.lblDate.Text = "DateTime";
            // 
            // dgvLog
            // 
            this.dgvLog.AllowUserToAddRows = false;
            this.dgvLog.AllowUserToDeleteRows = false;
            this.dgvLog.AllowUserToResizeColumns = false;
            this.dgvLog.AllowUserToResizeRows = false;
            this.dgvLog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvLog.BackgroundColor = global::LMCIS.Properties.Settings.Default.btnColor;
            this.dgvLog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvLog.ColumnHeadersVisible = false;
            this.dgvLog.DataBindings.Add(new System.Windows.Forms.Binding("GridColor", global::LMCIS.Properties.Settings.Default, "btnColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dgvLog.DataBindings.Add(new System.Windows.Forms.Binding("BackgroundColor", global::LMCIS.Properties.Settings.Default, "btnColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.dgvLog.GridColor = global::LMCIS.Properties.Settings.Default.btnColor;
            this.dgvLog.Location = new System.Drawing.Point(289, 135);
            this.dgvLog.Name = "dgvLog";
            this.dgvLog.ReadOnly = true;
            this.dgvLog.RowHeadersVisible = false;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dgvLog.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.dgvLog.RowTemplate.Height = 23;
            this.dgvLog.Size = new System.Drawing.Size(790, 735);
            this.dgvLog.TabIndex = 113;
            // 
            // LogHis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = global::LMCIS.Properties.Settings.Default.bgColor;
            this.ClientSize = new System.Drawing.Size(1170, 941);
            this.Controls.Add(this.dgvLog);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.palLoc);
            this.Controls.Add(this.palTime);
            this.Controls.Add(this.palNav);
            this.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::LMCIS.Properties.Settings.Default, "bgColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.Color.Black;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "LogHis";
            this.Text = "AutoNexus Business Management System";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.palNav.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBWMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHome)).EndInit();
            this.palTime.ResumeLayout(false);
            this.palTime.PerformLayout();
            this.palLoc.ResumeLayout(false);
            this.palLoc.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvLog)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel palNav;
        private System.Windows.Forms.Panel palTime;
        private System.Windows.Forms.Button btnLogOut;
        private System.Windows.Forms.PictureBox picHome;
        private System.Windows.Forms.Label lblCorpName;
        private System.Windows.Forms.Label lblUid;
        private System.Windows.Forms.Label lblTimeDate;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel palLoc;
        private System.Windows.Forms.Label lblLoc;
        private System.Windows.Forms.Button btnProFile;
        private System.Windows.Forms.PictureBox picBWMode;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Button btnFunction5;
        private System.Windows.Forms.Button btnFunction4;
        private System.Windows.Forms.Button btnFunction3;
        private System.Windows.Forms.Button btnFunction2;
        private System.Windows.Forms.Button btnFunction1;
        private System.Windows.Forms.DataGridView dgvLog;
    }
}

