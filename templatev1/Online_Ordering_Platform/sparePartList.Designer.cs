
namespace LMCIS.Online_Ordering_Platform
{
    partial class sparePartList
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(sparePartList));
            this.tbKW = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblResultNum = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.pnlSP = new System.Windows.Forms.Panel();
            this.cmbCategory = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbSorting = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timer2 = new System.Windows.Forms.Timer(this.components);
            this.btnClear = new System.Windows.Forms.Button();
            this.palLoc = new System.Windows.Forms.Panel();
            this.lblLoc = new System.Windows.Forms.Label();
            this.palDate = new System.Windows.Forms.Panel();
            this.lblTimeDate = new System.Windows.Forms.Label();
            this.lblUid = new System.Windows.Forms.Label();
            this.palNav = new System.Windows.Forms.Panel();
            this.palSelect5 = new System.Windows.Forms.Panel();
            this.palSelect4 = new System.Windows.Forms.Panel();
            this.btnFunction5 = new System.Windows.Forms.Button();
            this.palSelect2 = new System.Windows.Forms.Panel();
            this.palSelect1 = new System.Windows.Forms.Panel();
            this.picBWMode = new System.Windows.Forms.PictureBox();
            this.btnProFile = new System.Windows.Forms.Button();
            this.palSelect3 = new System.Windows.Forms.Panel();
            this.picHome = new System.Windows.Forms.PictureBox();
            this.lblCorpName = new System.Windows.Forms.Label();
            this.btnFunction4 = new System.Windows.Forms.Button();
            this.btnFunction3 = new System.Windows.Forms.Button();
            this.btnFunction2 = new System.Windows.Forms.Button();
            this.btnFunction1 = new System.Windows.Forms.Button();
            this.btnLogOut = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.palLoc.SuspendLayout();
            this.palDate.SuspendLayout();
            this.palNav.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picBWMode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHome)).BeginInit();
            this.SuspendLayout();
            // 
            // tbKW
            // 
            this.tbKW.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.tbKW.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.CustomSource;
            this.tbKW.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tbKW.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.tbKW.Location = new System.Drawing.Point(481, 100);
            this.tbKW.Name = "tbKW";
            this.tbKW.Size = new System.Drawing.Size(463, 29);
            this.tbKW.TabIndex = 76;
            this.tbKW.TextChanged += new System.EventHandler(this.tbKW_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(245, 153);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(86, 21);
            this.label1.TabIndex = 77;
            this.label1.Text = "Result(s) :";
            // 
            // lblResultNum
            // 
            this.lblResultNum.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblResultNum.Location = new System.Drawing.Point(205, 151);
            this.lblResultNum.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.lblResultNum.Name = "lblResultNum";
            this.lblResultNum.Size = new System.Drawing.Size(43, 22);
            this.lblResultNum.TabIndex = 78;
            this.lblResultNum.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.pnlSP);
            this.groupBox1.Location = new System.Drawing.Point(208, 176);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(950, 687);
            this.groupBox1.TabIndex = 79;
            this.groupBox1.TabStop = false;
            // 
            // pnlSP
            // 
            this.pnlSP.AutoScroll = true;
            this.pnlSP.Location = new System.Drawing.Point(2, 7);
            this.pnlSP.Name = "pnlSP";
            this.pnlSP.Size = new System.Drawing.Size(943, 675);
            this.pnlSP.TabIndex = 0;
            // 
            // cmbCategory
            // 
            this.cmbCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCategory.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbCategory.FormattingEnabled = true;
            this.cmbCategory.Items.AddRange(new object[] {
            "All",
            "A",
            "B",
            "C",
            "D"});
            this.cmbCategory.Location = new System.Drawing.Point(311, 100);
            this.cmbCategory.Name = "cmbCategory";
            this.cmbCategory.Size = new System.Drawing.Size(48, 27);
            this.cmbCategory.TabIndex = 80;
            this.cmbCategory.SelectedIndexChanged += new System.EventHandler(this.cmbCategory_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(221, 100);
            this.label2.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 22);
            this.label2.TabIndex = 81;
            this.label2.Text = "Category: ";
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(388, 102);
            this.label3.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 22);
            this.label3.TabIndex = 82;
            this.label3.Text = "Keyword:";
            // 
            // cmbSorting
            // 
            this.cmbSorting.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSorting.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbSorting.FormattingEnabled = true;
            this.cmbSorting.Items.AddRange(new object[] {
            "Category",
            "Price (Ascending)",
            "Price (Descending)"});
            this.cmbSorting.Location = new System.Drawing.Point(1002, 157);
            this.cmbSorting.Name = "cmbSorting";
            this.cmbSorting.Size = new System.Drawing.Size(151, 25);
            this.cmbSorting.TabIndex = 83;
            this.cmbSorting.SelectedIndexChanged += new System.EventHandler(this.cmbSorting_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Times New Roman", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(937, 157);
            this.label4.Margin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 22);
            this.label4.TabIndex = 84;
            this.label4.Text = "Sort By:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // btnClear
            // 
            this.btnClear.Font = new System.Drawing.Font("Times New Roman", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnClear.Location = new System.Drawing.Point(950, 100);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(81, 27);
            this.btnClear.TabIndex = 85;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // palLoc
            // 
            this.palLoc.BackColor = System.Drawing.SystemColors.ControlLight;
            this.palLoc.Controls.Add(this.lblLoc);
            this.palLoc.Dock = System.Windows.Forms.DockStyle.Top;
            this.palLoc.Location = new System.Drawing.Point(198, 40);
            this.palLoc.Name = "palLoc";
            this.palLoc.Size = new System.Drawing.Size(972, 38);
            this.palLoc.TabIndex = 102;
            // 
            // lblLoc
            // 
            this.lblLoc.AutoSize = true;
            this.lblLoc.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoc.Location = new System.Drawing.Point(6, 9);
            this.lblLoc.Name = "lblLoc";
            this.lblLoc.Size = new System.Drawing.Size(93, 22);
            this.lblLoc.TabIndex = 0;
            this.lblLoc.Text = "Spare Part";
            // 
            // palDate
            // 
            this.palDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(204)))), ((int)(((byte)(204)))), ((int)(((byte)(204)))));
            this.palDate.Controls.Add(this.lblTimeDate);
            this.palDate.Controls.Add(this.lblUid);
            this.palDate.Dock = System.Windows.Forms.DockStyle.Top;
            this.palDate.Font = new System.Drawing.Font("Times New Roman", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.palDate.Location = new System.Drawing.Point(198, 0);
            this.palDate.Margin = new System.Windows.Forms.Padding(2);
            this.palDate.Name = "palDate";
            this.palDate.Size = new System.Drawing.Size(972, 40);
            this.palDate.TabIndex = 101;
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
            // lblUid
            // 
            this.lblUid.AutoSize = true;
            this.lblUid.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUid.Location = new System.Drawing.Point(814, 9);
            this.lblUid.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblUid.Name = "lblUid";
            this.lblUid.Size = new System.Drawing.Size(134, 22);
            this.lblUid.TabIndex = 1;
            this.lblUid.Text = "Uid: LMXXXX";
            // 
            // palNav
            // 
            this.palNav.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
            this.palNav.Controls.Add(this.palSelect5);
            this.palNav.Controls.Add(this.palSelect4);
            this.palNav.Controls.Add(this.btnFunction5);
            this.palNav.Controls.Add(this.palSelect2);
            this.palNav.Controls.Add(this.palSelect1);
            this.palNav.Controls.Add(this.picBWMode);
            this.palNav.Controls.Add(this.btnProFile);
            this.palNav.Controls.Add(this.palSelect3);
            this.palNav.Controls.Add(this.picHome);
            this.palNav.Controls.Add(this.lblCorpName);
            this.palNav.Controls.Add(this.btnFunction4);
            this.palNav.Controls.Add(this.btnFunction3);
            this.palNav.Controls.Add(this.btnFunction2);
            this.palNav.Controls.Add(this.btnFunction1);
            this.palNav.Controls.Add(this.btnLogOut);
            this.palNav.Dock = System.Windows.Forms.DockStyle.Left;
            this.palNav.Location = new System.Drawing.Point(0, 0);
            this.palNav.Margin = new System.Windows.Forms.Padding(2);
            this.palNav.Name = "palNav";
            this.palNav.Size = new System.Drawing.Size(198, 941);
            this.palNav.TabIndex = 100;
            // 
            // palSelect5
            // 
            this.palSelect5.BackColor = System.Drawing.Color.Red;
            this.palSelect5.Location = new System.Drawing.Point(0, 371);
            this.palSelect5.Name = "palSelect5";
            this.palSelect5.Size = new System.Drawing.Size(10, 55);
            this.palSelect5.TabIndex = 26;
            // 
            // palSelect4
            // 
            this.palSelect4.BackColor = System.Drawing.Color.Red;
            this.palSelect4.Location = new System.Drawing.Point(0, 296);
            this.palSelect4.Name = "palSelect4";
            this.palSelect4.Size = new System.Drawing.Size(10, 55);
            this.palSelect4.TabIndex = 4;
            // 
            // btnFunction5
            // 
            this.btnFunction5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(213)))), ((int)(((byte)(184)))));
            this.btnFunction5.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(213)))), ((int)(((byte)(184)))));
            this.btnFunction5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFunction5.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFunction5.Location = new System.Drawing.Point(0, 371);
            this.btnFunction5.Margin = new System.Windows.Forms.Padding(4);
            this.btnFunction5.Name = "btnFunction5";
            this.btnFunction5.Size = new System.Drawing.Size(198, 55);
            this.btnFunction5.TabIndex = 25;
            this.btnFunction5.Text = "Give Feedback";
            this.btnFunction5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFunction5.UseVisualStyleBackColor = false;
            this.btnFunction5.Click += new System.EventHandler(this.btnFunction5_Click);
            // 
            // palSelect2
            // 
            this.palSelect2.BackColor = System.Drawing.Color.Red;
            this.palSelect2.Location = new System.Drawing.Point(0, 150);
            this.palSelect2.Name = "palSelect2";
            this.palSelect2.Size = new System.Drawing.Size(10, 55);
            this.palSelect2.TabIndex = 4;
            // 
            // palSelect1
            // 
            this.palSelect1.BackColor = System.Drawing.Color.Red;
            this.palSelect1.Location = new System.Drawing.Point(0, 77);
            this.palSelect1.Name = "palSelect1";
            this.palSelect1.Size = new System.Drawing.Size(10, 55);
            this.palSelect1.TabIndex = 4;
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
            // 
            // btnProFile
            // 
            this.btnProFile.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnProFile.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.btnProFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProFile.Font = new System.Drawing.Font("Times New Roman", 15F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnProFile.Location = new System.Drawing.Point(0, 794);
            this.btnProFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnProFile.Name = "btnProFile";
            this.btnProFile.Size = new System.Drawing.Size(198, 34);
            this.btnProFile.TabIndex = 20;
            this.btnProFile.Text = "ProFile";
            this.btnProFile.UseVisualStyleBackColor = false;
            this.btnProFile.Click += new System.EventHandler(this.btnProFile_Click);
            // 
            // palSelect3
            // 
            this.palSelect3.BackColor = System.Drawing.Color.Red;
            this.palSelect3.Location = new System.Drawing.Point(0, 223);
            this.palSelect3.Name = "palSelect3";
            this.palSelect3.Size = new System.Drawing.Size(10, 55);
            this.palSelect3.TabIndex = 3;
            // 
            // picHome
            // 
            this.picHome.BackColor = System.Drawing.SystemColors.GradientActiveCaption;
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
            this.lblCorpName.Size = new System.Drawing.Size(163, 52);
            this.lblCorpName.TabIndex = 10;
            this.lblCorpName.Text = "AutoNexus";
            this.lblCorpName.Click += new System.EventHandler(this.lblCorpName_Click);
            // 
            // btnFunction4
            // 
            this.btnFunction4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(213)))), ((int)(((byte)(184)))));
            this.btnFunction4.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(213)))), ((int)(((byte)(184)))));
            this.btnFunction4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFunction4.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFunction4.Location = new System.Drawing.Point(0, 296);
            this.btnFunction4.Margin = new System.Windows.Forms.Padding(4);
            this.btnFunction4.Name = "btnFunction4";
            this.btnFunction4.Size = new System.Drawing.Size(198, 55);
            this.btnFunction4.TabIndex = 14;
            this.btnFunction4.Text = "Favourite";
            this.btnFunction4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFunction4.UseVisualStyleBackColor = false;
            this.btnFunction4.Click += new System.EventHandler(this.btnFunction4_Click);
            // 
            // btnFunction3
            // 
            this.btnFunction3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(213)))), ((int)(((byte)(184)))));
            this.btnFunction3.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(213)))), ((int)(((byte)(184)))));
            this.btnFunction3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFunction3.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFunction3.Location = new System.Drawing.Point(0, 223);
            this.btnFunction3.Margin = new System.Windows.Forms.Padding(4);
            this.btnFunction3.Name = "btnFunction3";
            this.btnFunction3.Size = new System.Drawing.Size(198, 55);
            this.btnFunction3.TabIndex = 13;
            this.btnFunction3.Text = "Cart";
            this.btnFunction3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFunction3.UseVisualStyleBackColor = false;
            this.btnFunction3.Click += new System.EventHandler(this.btnFunction3_Click);
            // 
            // btnFunction2
            // 
            this.btnFunction2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(213)))), ((int)(((byte)(184)))));
            this.btnFunction2.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(213)))), ((int)(((byte)(184)))));
            this.btnFunction2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFunction2.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFunction2.Location = new System.Drawing.Point(0, 150);
            this.btnFunction2.Margin = new System.Windows.Forms.Padding(4);
            this.btnFunction2.Name = "btnFunction2";
            this.btnFunction2.Size = new System.Drawing.Size(198, 55);
            this.btnFunction2.TabIndex = 12;
            this.btnFunction2.Text = "Spare Part";
            this.btnFunction2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFunction2.UseVisualStyleBackColor = false;
            this.btnFunction2.Click += new System.EventHandler(this.btnFunction2_Click);
            // 
            // btnFunction1
            // 
            this.btnFunction1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(213)))), ((int)(((byte)(184)))));
            this.btnFunction1.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(59)))), ((int)(((byte)(213)))), ((int)(((byte)(184)))));
            this.btnFunction1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFunction1.Font = new System.Drawing.Font("Times New Roman", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnFunction1.Location = new System.Drawing.Point(0, 77);
            this.btnFunction1.Margin = new System.Windows.Forms.Padding(4);
            this.btnFunction1.Name = "btnFunction1";
            this.btnFunction1.Size = new System.Drawing.Size(198, 55);
            this.btnFunction1.TabIndex = 11;
            this.btnFunction1.Text = "Order Management";
            this.btnFunction1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.btnFunction1.UseVisualStyleBackColor = false;
            this.btnFunction1.Click += new System.EventHandler(this.btnFunction1_Click);
            // 
            // btnLogOut
            // 
            this.btnLogOut.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
            this.btnLogOut.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(192)))));
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
            // sparePartList
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1170, 941);
            this.Controls.Add(this.palLoc);
            this.Controls.Add(this.palDate);
            this.Controls.Add(this.palNav);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbSorting);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbCategory);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblResultNum);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbKW);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "sparePartList";
            this.Text = "AutoNexus Business Management System";
            this.Load += new System.EventHandler(this.sparePartList_Load);
            this.groupBox1.ResumeLayout(false);
            this.palLoc.ResumeLayout(false);
            this.palLoc.PerformLayout();
            this.palDate.ResumeLayout(false);
            this.palDate.PerformLayout();
            this.palNav.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.picBWMode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picHome)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.TextBox tbKW;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblResultNum;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Panel pnlSP;
        private System.Windows.Forms.ComboBox cmbCategory;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbSorting;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Timer timer2;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Panel palLoc;
        private System.Windows.Forms.Label lblLoc;
        private System.Windows.Forms.Panel palDate;
        private System.Windows.Forms.Label lblTimeDate;
        private System.Windows.Forms.Label lblUid;
        private System.Windows.Forms.Panel palNav;
        private System.Windows.Forms.Panel palSelect5;
        private System.Windows.Forms.Panel palSelect4;
        private System.Windows.Forms.Button btnFunction5;
        private System.Windows.Forms.Panel palSelect2;
        private System.Windows.Forms.Panel palSelect1;
        private System.Windows.Forms.PictureBox picBWMode;
        private System.Windows.Forms.Button btnProFile;
        private System.Windows.Forms.Panel palSelect3;
        private System.Windows.Forms.Button btnLogOut;
        private System.Windows.Forms.PictureBox picHome;
        private System.Windows.Forms.Label lblCorpName;
        private System.Windows.Forms.Button btnFunction4;
        private System.Windows.Forms.Button btnFunction3;
        private System.Windows.Forms.Button btnFunction2;
        private System.Windows.Forms.Button btnFunction1;
    }
}