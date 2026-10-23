using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Data;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using Microsoft.Extensions.Logging;
using LMCIS.controller;
using LMCIS.On_Sale_Product_Manag;
using LMCIS.Order_Management;
using LMCIS.Profile;
using LMCIS.Properties;
using LMCIS.Stock_Manag;
using LMCIS.System_page;
using LMCIS.User_Manag;
using LMCIS.Properties;

namespace LMCIS.Online_Ordering_Platform
{
    public partial class sparePartList : Form
    {
        private string uName, UID;
        private Boolean isLM;
        AccountController accountController;
        UIController UIController;
        spareListController controller;
        private bool isInitializingCatalog;
        private Font catalogFont;
        private static readonly object CatalogImageSync = new object();
        private static readonly Dictionary<string, Image> CatalogImages = new Dictionary<string, Image>(StringComparer.Ordinal);

        public sparePartList()
        {
            InitializeComponent();
            initializeCatalogFontLifetime();
            controller = new spareListController();
            lblUid.Text = $"Uid: {UID}";
        }

        public sparePartList(spareListController controller)
        {
            InitializeComponent();
            initializeCatalogFontLifetime();
            this.controller = controller ?? throw new ArgumentNullException(nameof(controller));
            lblUid.Text = $"Uid: {UID}";
        }

        public sparePartList(AccountController accountController, UIController UIController)
        {
            InitializeComponent();
            initializeCatalogFontLifetime();
            this.accountController = accountController;
            this.UIController = UIController;
            isLM = accountController.GetIsLm();
            controller = new spareListController();
            UID = accountController.GetUid();
            lblUid.Text = $"Uid: {UID}";
        }


        private void sparePartList_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            isInitializingCatalog = true;
            try
            {
                cmbCategory.SelectedIndex = 0;
                cmbSorting.SelectedIndex = 0;
            }
            finally
            {
                isInitializingCatalog = false;
            }
            load_part(controller.getSpareWhenTextChange(cmbCategory.Text, tbKW.Text, cmbSorting.Text));
            palSelect1.Visible =
                palSelect2.Visible = palSelect3.Visible = palSelect4.Visible = palSelect5.Visible = false;
            hideButton();
            setIndicator(UIController.getIndicator("Spare Part"));
        }


        public void load_part(DataTable dt)
        {
            var elapsed = Stopwatch.StartNew();
            pnlSP.SuspendLayout();
            try
            {
                while (pnlSP.Controls.Count > 0)
                {
                    Control oldCard = pnlSP.Controls[0];
                    pnlSP.Controls.RemoveAt(0);
                    oldCard.Dispose();
                }
                renderPartCards(dt);
            }
            finally
            {
                pnlSP.ResumeLayout(true);
            }
            elapsed.Stop();
            LMCIS.controller.Utilities.Log.LogMessage(LogLevel.Information, "Performance",
                $"Catalog rendered {dt.Rows.Count} rows in {elapsed.ElapsedMilliseconds} ms.");
        }

        private void renderPartCards(DataTable dt)
        {
            int grpBoxNeeded = countSparePart(dt);
            int columnPerRow = 3;
            int rowNeed = divideRoundUp(grpBoxNeeded, columnPerRow);
            int firstColumnXPosition = 23;
            int secondColumnXPosition = 328;
            int thirdColumnXPosition = 636;
            int yPosition = 11; //first row y position  
            int currentGrpBox = 0;

            List<string> partName = new List<string>();
            for (int i = 0; i < rowNeed; i++)
            {
                for (int k = 0; k < columnPerRow; k++)
                {
                    if (currentGrpBox < grpBoxNeeded)
                    {
                        GroupBox grpSpareBox;
                        if (k == 0)
                        {
                            grpSpareBox = new GroupBox
                            {
                                Name = $"grpBox{currentGrpBox}", Size = new Size(281, 383),
                                Location = new Point(firstColumnXPosition, yPosition)
                            };
                        }
                        else if (k == 1)
                        {
                            grpSpareBox = new GroupBox
                            {
                                Size = new Size(281, 383),
                                Location = new Point(secondColumnXPosition, yPosition)
                            };
                        }
                        else
                        {
                            grpSpareBox = new GroupBox
                            {
                                Size = new Size(281, 383),
                                Location = new Point(thirdColumnXPosition, yPosition)
                            };
                        }

                        PictureBox picPartImage = new PictureBox
                        {
                            Size = new Size(275, 186), Location = new Point(3, 16),
                            SizeMode = PictureBoxSizeMode.Zoom, Image = imageString($"{dt.Rows[currentGrpBox][0]}")
                        };
                        Label lblCategoryLabel = new Label
                        {
                            Text = "Category :", AutoSize = true, Location = new Point(6, 207),
                            Font = getCatalogFont()
                        };
                        Label lblPartNumLabel = new Label
                        {
                            Text = "Part Number :", AutoSize = true, Location = new Point(6, 237),
                            Font = getCatalogFont()
                        };
                        Label lblNameLabel = new Label
                        {
                            Text = "Name :", AutoSize = true, Location = new Point(6, 267),
                            Font = getCatalogFont()
                        };
                        Label lblPriceLabel = new Label
                        {
                            Text = "Price :¥", AutoSize = true, Location = new Point(6, 297),
                            Font = getCatalogFont()
                        };

                        Label lblCategory = new Label
                        {
                            Text = $"{dt.Rows[currentGrpBox][2]} - {dt.Rows[currentGrpBox]["categoryName"]}",
                            AutoSize = false, Font = getCatalogFont(),
                            Location = new Point(83, 208), Size = new Size(174, 20)
                        };
                        Label lblPartNum = new Label
                        {
                            Name = $"lblPartNum{currentGrpBox}", Text = $"{dt.Rows[currentGrpBox][0]}",
                            AutoSize = false, Font = getCatalogFont(),
                            Location = new Point(108, 238), Size = new Size(171, 20)
                        };
                        Label lblName = new Label
                        {
                            Text = $"{dt.Rows[currentGrpBox][3]}", AutoSize = false,
                            Font = getCatalogFont(), Location = new Point(62, 268),
                            Size = new Size(218, 20)
                        };
                        Label lblPrice = new Label
                        {
                            Text = $"{dt.Rows[currentGrpBox]["price"]}", AutoSize = false,
                            Font = getCatalogFont(), Location = new Point(64, 297),
                            Size = new Size(213, 20)
                        };
                        Button btnView = new Button
                        {
                            Name = $"btnView{currentGrpBox}", Text = "View", Font = getCatalogFont(),
                            Cursor = Cursors.Hand, Location = new Point(3, 319),
                            Size = new Size(272, 30)
                        };
                        Button btnAddCart = new Button
                        {
                            Name = $"btnAddCart{currentGrpBox}", Text = "Add Cart",
                            Font = getCatalogFont(), Cursor = Cursors.Hand,
                            Location = new Point(3, 350), Size = new Size(205, 30)
                        };
                        TextBox tbQty = new TextBox
                        {
                            Text = "1", Name = $"tbQty{currentGrpBox}", BorderStyle = BorderStyle.FixedSingle,
                            Font = getCatalogFont(), Location = new Point(214, 352),
                            MaxLength = 4, Size = new Size(61, 26),
                            TextAlign = HorizontalAlignment.Center
                        };
                        btnView.Click += viewPart;
                        btnAddCart.Click += addCart;
                        tbQty.KeyPress += qtyBox_KeyPress;

                        grpSpareBox.Controls.Add(picPartImage);
                        grpSpareBox.Controls.Add(lblCategoryLabel);
                        grpSpareBox.Controls.Add(lblPartNumLabel);
                        grpSpareBox.Controls.Add(lblNameLabel);
                        grpSpareBox.Controls.Add(lblPriceLabel);
                        grpSpareBox.Controls.Add(lblCategory);
                        grpSpareBox.Controls.Add(lblPartNum);
                        grpSpareBox.Controls.Add(lblName);
                        grpSpareBox.Controls.Add(lblPrice);
                        grpSpareBox.Controls.Add(btnView);
                        grpSpareBox.Controls.Add(btnAddCart);
                        grpSpareBox.Controls.Add(tbQty);

                        pnlSP.Controls.Add(grpSpareBox);
                        ++currentGrpBox;

                        partName.Add(lblName.Text);
                    }
                }

                yPosition += 388;
            }

            lblResultNum.Text = grpBoxNeeded.ToString();

            AutoCompleteStringCollection autoCompleteCollection = new AutoCompleteStringCollection();
            autoCompleteCollection.AddRange(partName.ToArray());
            tbKW.AutoCompleteCustomSource = autoCompleteCollection;
        }

        private void initializeCatalogFontLifetime()
        {
            Disposed += (sender, args) =>
            {
                catalogFont?.Dispose();
                catalogFont = null;
            };
        }

        private Font getCatalogFont()
        {
            if (catalogFont == null)
                catalogFont = new Font("Times New Roman", 12);
            return catalogFont;
        }

        public int countSparePart(DataTable dt)
        {
            return dt.Rows.Count;
        }

        public int divideRoundUp(int upperNum, int lowerNum)
        {
            return (upperNum + lowerNum - 1) / lowerNum;
        }

        private void viewPart(object sender, EventArgs e)
        {
            string partNum = "";

            if (sender is Button clickedButton)
            {
                string buttonName = clickedButton.Name;
                int index = getViewIndex(buttonName);
                if (index != -1)
                {
                    if (clickedButton.Parent is GroupBox parentGroupBox)
                    {
                        foreach (Control control in parentGroupBox.Controls)
                        {
                            if (control.Name == $"lblPartNum{index}")
                            {
                                Form viewSparePart = new viewSparePart(control.Text, accountController,
                                    UIController);
                                Hide();
                                viewSparePart.StartPosition = FormStartPosition.Manual;
                                viewSparePart.Location = Location;
                                viewSparePart.ShowDialog();
                                Close();
                            }
                        }
                    }
                }
            }
        }

        private void addCart(object sender, EventArgs e)
        {
            string partNum = "";
            int qty = 0;
            if (sender is Button clickedButton)
            {
                string buttonName = clickedButton.Name;
                int index = getAddCartIndex(buttonName);
                if (index != -1)
                {
                    if (clickedButton.Parent is GroupBox parentGroupBox)
                    {
                        foreach (Control control in parentGroupBox.Controls)
                        {
                            if (control.Name == $"lblPartNum{index}")
                            {
                                partNum = control.Text;
                            }

                            if (control.Name == $"tbQty{index}")
                            {
                                qty += int.Parse(control.Text);
                                control.Text = "1";
                            }
                        }
                    }
                }
            }

            if (qty <= 0)
            {
                MessageBox.Show("The quantity input is not valid.\nPlease adjust the quantity input", "Add Cart",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (qty <= controller.getOnSaleQty(partNum, isLM))
            {
                if (controller.addCart(UID, partNum, qty, isLM))
                {
                    MessageBox.Show("Added to cart", "Add Cart", MessageBoxButtons.OK);
                }
                else
                {
                    MessageBox.Show("/*Please try aga*/in", "Add Cart", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show(
                    $"The quantity input exceed the on sale quantity({controller.getOnSaleQty(partNum, isLM)}).\nPlease adjust the quantity input",
                    "Add Cart", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void qtyBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private int getViewIndex(string btnName)
        {
            int i = 0;
            while (true)
            {
                if (btnName == $"btnView{i}")
                {
                    return i;
                }

                i++;
            }

            int x = -1;
            return x;
        }

        private int getAddCartIndex(string btnName)
        {
            int i = 0;
            while (true)
            {
                if (btnName == $"btnAddCart{i}")
                {
                    return i;
                }

                i++;
            }

            int x = -1;
            return x;
        }

        private void tbKW_TextChanged(object sender, EventArgs e)
        {
            if (isInitializingCatalog) return;
            load_part(controller.getSpareWhenTextChange(cmbCategory.Text, tbKW.Text,
                cmbSorting.Text));
        }

        private void cmbSorting_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitializingCatalog) return;
            load_part(controller.getSpareWhenTextChange(cmbCategory.Text, tbKW.Text,
                cmbSorting.Text));
        }

        private void cmbCategory_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (isInitializingCatalog) return;
            load_part(controller.getSpareWhenTextChange(cmbCategory.Text, tbKW.Text,
                cmbSorting.Text));
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTimeDate.Text = DateTime.Now.ToString("yyyy/MM/dd   HH:mm:ss");
        }


        private void btnClear_Click(object sender, EventArgs e)
        {
            tbKW.Text = "";
        }

        private Image imageString(string imageName)
        {
            lock (CatalogImageSync)
            {
                if (CatalogImages.TryGetValue(imageName, out var cachedImage))
                    return cachedImage;

                PropertyInfo property =
                    typeof(Resources).GetProperty(imageName, BindingFlags.NonPublic | BindingFlags.Static);
                var image = property?.GetValue(null, null) as Image;
                if (image != null)
                    CatalogImages.Add(imageName, image);
                return image;
            }
        }

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

        public void hideButton()
        {
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

        private void btnFunction1_Click(object sender, EventArgs e)
        {
            getPage(btnFunction1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            getPage(btnFunction5.Text);
        }

        private void btnFunction4_Click(object sender, EventArgs e)
        {
            getPage(btnFunction4.Text);
        }

        private void btnFunction3_Click(object sender, EventArgs e)
        {
            getPage(btnFunction3.Text);
        }

        private void btnFunction2_Click(object sender, EventArgs e)
        {
            getPage(btnFunction2.Text);
        }

        private void btnFunction5_Click(object sender, EventArgs e)
        {
            getPage(btnFunction5.Text);
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            Form o = new Login();
            Hide();
            o.StartPosition = FormStartPosition.Manual;
            o.Location = Location;
            o.ShowDialog();
            Close();
        }

        private void btnProFile_Click(object sender, EventArgs e)
        {
            proFileController proFileController = new proFileController(accountController);

            proFileController.setType(accountController.GetAccountType());

            Form proFile = new proFileMain(accountController, UIController, proFileController);
            Hide();
            //Swap the current form to another.
            proFile.StartPosition = FormStartPosition.Manual;
            proFile.Location = Location;
            proFile.ShowDialog();
            Close();
        }

        private void picHome_Click(object sender, EventArgs e)
        {
            Form home = new Home(accountController, UIController);
            Hide();
            //Swap the current form to another.
            home.StartPosition = FormStartPosition.Manual;
            home.Location = Location;
            home.ShowDialog();
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


    }
}
