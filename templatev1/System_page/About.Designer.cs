namespace LMCIS.System_page
{
    partial class About
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(About));
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
            this.lblTitSysName = new System.Windows.Forms.Label();
            this.lblTitTotalOpTime = new System.Windows.Forms.Label();
            this.lblTieInsDate = new System.Windows.Forms.Label();
            this.lblTitVer = new System.Windows.Forms.Label();
            this.lblInsDate = new System.Windows.Forms.Label();
            this.lblTotalOpTime = new System.Windows.Forms.Label();
            this.lblTitPlatform = new System.Windows.Forms.Label();
            this.lblPlatform = new System.Windows.Forms.Label();
            this.lblSysName = new System.Windows.Forms.Label();
            this.lblVer = new System.Windows.Forms.Label();
            this.palNav.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBWMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHome)).BeginInit();
            this.palTime.SuspendLayout();
            this.palLoc.SuspendLayout();
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
            this.btnLogOut.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnLogOut.FlatAppearance.BorderSize = 0;
            this.btnLogOut.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnLogOut.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
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
            this.btnFunction5.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(213)))), ((int)(((byte)(184)))));
            this.btnFunction5.FlatAppearance.BorderSize = 0;
            this.btnFunction5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFunction5.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFunction5.Location = new System.Drawing.Point(0, 371);
            this.btnFunction5.Margin = new System.Windows.Forms.Padding(4);
            this.btnFunction5.Name = "btnFunction5";
            this.btnFunction5.Size = new System.Drawing.Size(198, 55);
            this.btnFunction5.TabIndex = 15;
            this.btnFunction5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFunction5.UseVisualStyleBackColor = false;
            this.btnFunction5.Click += new System.EventHandler(this.btnFunction5_Click);
            // 
            // btnFunction4
            // 
            this.btnFunction4.BackColor = global::LMCIS.Properties.Settings.Default.navBarColor;
            this.btnFunction4.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::LMCIS.Properties.Settings.Default, "navBarColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnFunction4.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(213)))), ((int)(((byte)(184)))));
            this.btnFunction4.FlatAppearance.BorderSize = 0;
            this.btnFunction4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFunction4.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFunction4.Location = new System.Drawing.Point(0, 296);
            this.btnFunction4.Margin = new System.Windows.Forms.Padding(4);
            this.btnFunction4.Name = "btnFunction4";
            this.btnFunction4.Size = new System.Drawing.Size(198, 55);
            this.btnFunction4.TabIndex = 14;
            this.btnFunction4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFunction4.UseVisualStyleBackColor = false;
            this.btnFunction4.Click += new System.EventHandler(this.btnFunction4_Click);
            // 
            // btnFunction3
            // 
            this.btnFunction3.BackColor = global::LMCIS.Properties.Settings.Default.navBarColor;
            this.btnFunction3.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::LMCIS.Properties.Settings.Default, "navBarColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnFunction3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(213)))), ((int)(((byte)(184)))));
            this.btnFunction3.FlatAppearance.BorderSize = 0;
            this.btnFunction3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFunction3.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFunction3.Location = new System.Drawing.Point(0, 223);
            this.btnFunction3.Margin = new System.Windows.Forms.Padding(4);
            this.btnFunction3.Name = "btnFunction3";
            this.btnFunction3.Size = new System.Drawing.Size(198, 55);
            this.btnFunction3.TabIndex = 13;
            this.btnFunction3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFunction3.UseVisualStyleBackColor = false;
            this.btnFunction3.Click += new System.EventHandler(this.btnFunction3_Click);
            // 
            // btnFunction2
            // 
            this.btnFunction2.BackColor = global::LMCIS.Properties.Settings.Default.navBarColor;
            this.btnFunction2.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::LMCIS.Properties.Settings.Default, "navBarColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnFunction2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(213)))), ((int)(((byte)(184)))));
            this.btnFunction2.FlatAppearance.BorderSize = 0;
            this.btnFunction2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFunction2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFunction2.Location = new System.Drawing.Point(0, 150);
            this.btnFunction2.Margin = new System.Windows.Forms.Padding(4);
            this.btnFunction2.Name = "btnFunction2";
            this.btnFunction2.Size = new System.Drawing.Size(198, 55);
            this.btnFunction2.TabIndex = 12;
            this.btnFunction2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFunction2.UseVisualStyleBackColor = false;
            this.btnFunction2.Click += new System.EventHandler(this.btnFunction2_Click);
            // 
            // btnFunction1
            // 
            this.btnFunction1.BackColor = global::LMCIS.Properties.Settings.Default.navBarColor;
            this.btnFunction1.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::LMCIS.Properties.Settings.Default, "navBarColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.btnFunction1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(213)))), ((int)(((byte)(184)))));
            this.btnFunction1.FlatAppearance.BorderSize = 0;
            this.btnFunction1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFunction1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFunction1.Location = new System.Drawing.Point(0, 77);
            this.btnFunction1.Margin = new System.Windows.Forms.Padding(4);
            this.btnFunction1.Name = "btnFunction1";
            this.btnFunction1.Size = new System.Drawing.Size(198, 55);
            this.btnFunction1.TabIndex = 11;
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
            this.lblLoc.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoc.Location = new System.Drawing.Point(6, 9);
            this.lblLoc.Name = "lblLoc";
            this.lblLoc.Size = new System.Drawing.Size(144, 22);
            this.lblLoc.TabIndex = 0;
            this.lblLoc.Text = "About the system";
            // 
            // lblTitSysName
            // 
            this.lblTitSysName.AutoSize = true;
            this.lblTitSysName.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitSysName.Location = new System.Drawing.Point(233, 110);
            this.lblTitSysName.Name = "lblTitSysName";
            this.lblTitSysName.Size = new System.Drawing.Size(123, 22);
            this.lblTitSysName.TabIndex = 3;
            this.lblTitSysName.Text = "System Name:";
            // 
            // lblTitTotalOpTime
            // 
            this.lblTitTotalOpTime.AutoSize = true;
            this.lblTitTotalOpTime.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitTotalOpTime.Location = new System.Drawing.Point(233, 203);
            this.lblTitTotalOpTime.Name = "lblTitTotalOpTime";
            this.lblTitTotalOpTime.Size = new System.Drawing.Size(186, 22);
            this.lblTitTotalOpTime.TabIndex = 6;
            this.lblTitTotalOpTime.Text = "Total Operation Time:";
            // 
            // lblTieInsDate
            // 
            this.lblTieInsDate.AutoSize = true;
            this.lblTieInsDate.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTieInsDate.Location = new System.Drawing.Point(233, 172);
            this.lblTieInsDate.Name = "lblTieInsDate";
            this.lblTieInsDate.Size = new System.Drawing.Size(107, 22);
            this.lblTieInsDate.TabIndex = 7;
            this.lblTieInsDate.Text = "Install Date:";
            // 
            // lblTitVer
            // 
            this.lblTitVer.AutoSize = true;
            this.lblTitVer.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitVer.Location = new System.Drawing.Point(233, 141);
            this.lblTitVer.Name = "lblTitVer";
            this.lblTitVer.Size = new System.Drawing.Size(137, 22);
            this.lblTitVer.TabIndex = 8;
            this.lblTitVer.Text = "System Version:";
            // 
            // lblInsDate
            // 
            this.lblInsDate.BackColor = global::LMCIS.Properties.Settings.Default.locTbColor;
            this.lblInsDate.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::LMCIS.Properties.Settings.Default, "locTbColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblInsDate.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblInsDate.Location = new System.Drawing.Point(434, 170);
            this.lblInsDate.Name = "lblInsDate";
            this.lblInsDate.Size = new System.Drawing.Size(353, 26);
            this.lblInsDate.TabIndex = 30;
            this.lblInsDate.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTotalOpTime
            // 
            this.lblTotalOpTime.BackColor = global::LMCIS.Properties.Settings.Default.locTbColor;
            this.lblTotalOpTime.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::LMCIS.Properties.Settings.Default, "locTbColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblTotalOpTime.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTotalOpTime.Location = new System.Drawing.Point(434, 201);
            this.lblTotalOpTime.Name = "lblTotalOpTime";
            this.lblTotalOpTime.Size = new System.Drawing.Size(353, 26);
            this.lblTotalOpTime.TabIndex = 32;
            this.lblTotalOpTime.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblTitPlatform
            // 
            this.lblTitPlatform.AutoSize = true;
            this.lblTitPlatform.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblTitPlatform.Location = new System.Drawing.Point(233, 234);
            this.lblTitPlatform.Name = "lblTitPlatform";
            this.lblTitPlatform.Size = new System.Drawing.Size(84, 22);
            this.lblTitPlatform.TabIndex = 34;
            this.lblTitPlatform.Text = "Platform:";
            // 
            // lblPlatform
            // 
            this.lblPlatform.BackColor = global::LMCIS.Properties.Settings.Default.locTbColor;
            this.lblPlatform.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::LMCIS.Properties.Settings.Default, "locTbColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblPlatform.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPlatform.Location = new System.Drawing.Point(434, 232);
            this.lblPlatform.Name = "lblPlatform";
            this.lblPlatform.Size = new System.Drawing.Size(353, 26);
            this.lblPlatform.TabIndex = 35;
            this.lblPlatform.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblSysName
            // 
            this.lblSysName.BackColor = global::LMCIS.Properties.Settings.Default.locTbColor;
            this.lblSysName.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::LMCIS.Properties.Settings.Default, "locTbColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblSysName.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSysName.Location = new System.Drawing.Point(434, 108);
            this.lblSysName.Name = "lblSysName";
            this.lblSysName.Size = new System.Drawing.Size(353, 26);
            this.lblSysName.TabIndex = 33;
            this.lblSysName.Text = "AutoNexus Business Management System";
            this.lblSysName.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // lblVer
            // 
            this.lblVer.BackColor = global::LMCIS.Properties.Settings.Default.locTbColor;
            this.lblVer.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::LMCIS.Properties.Settings.Default, "locTbColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.lblVer.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblVer.Location = new System.Drawing.Point(434, 139);
            this.lblVer.Name = "lblVer";
            this.lblVer.Size = new System.Drawing.Size(353, 26);
            this.lblVer.TabIndex = 31;
            this.lblVer.Text = "Ver 1.1.0";
            this.lblVer.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // About
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = global::LMCIS.Properties.Settings.Default.bgColor;
            this.ClientSize = new System.Drawing.Size(1170, 941);
            this.Controls.Add(this.lblPlatform);
            this.Controls.Add(this.lblTitPlatform);
            this.Controls.Add(this.lblSysName);
            this.Controls.Add(this.lblTotalOpTime);
            this.Controls.Add(this.lblInsDate);
            this.Controls.Add(this.lblVer);
            this.Controls.Add(this.lblTitVer);
            this.Controls.Add(this.lblTieInsDate);
            this.Controls.Add(this.lblTitTotalOpTime);
            this.Controls.Add(this.lblTitSysName);
            this.Controls.Add(this.palLoc);
            this.Controls.Add(this.palTime);
            this.Controls.Add(this.palNav);
            this.DataBindings.Add(new System.Windows.Forms.Binding("ForeColor", global::LMCIS.Properties.Settings.Default, "textColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.DataBindings.Add(new System.Windows.Forms.Binding("BackColor", global::LMCIS.Properties.Settings.Default, "bgColor", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = global::LMCIS.Properties.Settings.Default.textColor;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "About";
            this.Text = "AutoNexus Business Management System";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.palNav.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBWMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHome)).EndInit();
            this.palTime.ResumeLayout(false);
            this.palTime.PerformLayout();
            this.palLoc.ResumeLayout(false);
            this.palLoc.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Panel palNav;
        private System.Windows.Forms.Panel palTime;
        private System.Windows.Forms.Button btnLogOut;
        private System.Windows.Forms.PictureBox picHome;
        private System.Windows.Forms.Button btnFunction5;
        private System.Windows.Forms.Button btnFunction4;
        private System.Windows.Forms.Button btnFunction3;
        private System.Windows.Forms.Button btnFunction2;
        private System.Windows.Forms.Label lblCorpName;
        private System.Windows.Forms.Label lblUid;
        private System.Windows.Forms.Label lblTimeDate;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel palLoc;
        private System.Windows.Forms.Label lblLoc;
        private System.Windows.Forms.Button btnProFile;
        private System.Windows.Forms.PictureBox picBWMode;
        private System.Windows.Forms.Label lblTitSysName;
        private System.Windows.Forms.Label lblTitTotalOpTime;
        private System.Windows.Forms.Label lblTieInsDate;
        private System.Windows.Forms.Label lblTitVer;
        private System.Windows.Forms.Label lblInsDate;
        private System.Windows.Forms.Label lblVer;
        private System.Windows.Forms.Label lblTotalOpTime;
        private System.Windows.Forms.Label lblTitPlatform;
        private System.Windows.Forms.Label lblPlatform;
        private System.Windows.Forms.Label lblSysName;
        private System.Windows.Forms.Button btnFunction1;
    }
}

