using System;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Windows.Forms;
using LMCIS.controller;
using LMCIS.Online_Ordering_Platform;
using LMCIS.Order_Management;
using LMCIS.Profile;
using LMCIS.Properties;
using LMCIS.Stock_Manag;
using LMCIS.System_page;
using LMCIS.Utilities;
using LMCIS.User_Manag;

namespace LMCIS.On_Sale_Product_Manag
{
    public partial class OnSaleAdd : Form
    {
        private string uName, UID;
        dynamic sparePartInfo, newItemInfo;
        Bitmap IMG;
        bool IMGUploaded;
        AccountController accountController;
        OnSaleProductController onSaleProductController;
        UIController UIController;
        stockController stockController;


        public OnSaleAdd(AccountController accountController, UIController UIController,
            OnSaleProductController onSaleProductController)
        {
            InitializeComponent();
            OwnedImage.TrackDisposal(picProductIMG, () => IMG);
            this.accountController = accountController;
            this.UIController = UIController;
            this.onSaleProductController = onSaleProductController;
            stockController = new stockController(accountController);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTimeDate.Text = DateTime.Now.ToString("yyyy/MM/dd   HH:mm:ss");
            lblOnShelveDate.Text = DateTime.Now.ToString("dd-MM-yy");
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Initialization();
        }

        private void Initialization()
        {
            timer1.Enabled = true;

            UID = accountController.GetUid();
            uName = accountController.GetName();
            lblUid.Text = "UID: " + UID;
            setIndicator(UIController.getIndicator("Stock Management"));
            chkStatus.Checked = true;
            cmbPartNumber.Items.AddRange(onSaleProductController.GetSparePart().ToArray());
            lblItemID.Text = onSaleProductController.GetItemID();

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


            //For icon color
            if (LMCIS.Properties.Settings.Default.BWmode == true)
            {
                picBWMode.Image = Resources.LBWhite;
                picHome.Image = Resources.homeWhite;
            }
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

        //Determine next page.
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

            Hide();
            next.StartPosition = FormStartPosition.Manual;
            next.Location = Location;
            next.Size = Size;
            next.ShowDialog();
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
            proFile.Size = Size;
            proFile.ShowDialog();
            Close();
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


        private void cmbPartNumber_SelectedValueChanged(object sender, EventArgs e)
        {
            lblStoreInfoMsg.Text = "";
            sparePartInfo =
                stockController.GetPartInfo(cmbPartNumber.SelectedItem.ToString());
            lblPartNo.Text = sparePartInfo.partNumber;
            lblSuppilerID.Text = sparePartInfo.supplierID;
            lblCountry.Text = sparePartInfo.country;
            lblSuppiler.Text = sparePartInfo.Sname;
            lblCat.Text = sparePartInfo.type;
            lblNoOfStock.Text = sparePartInfo.quantity;
            lblPartName.Text = sparePartInfo.SPname;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            getPage("On-Sale Product Management");
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (checkInfo()) //Pass to controller and create account
            {
                setValue(); //If passed set the value in to dynameic.
                if (onSaleProductController.CreateNewItem(newItemInfo))
                {
                    MessageBox.Show(
                        $"Create new item success! New itemID is {lblItemID.Text}"
                        , "System message", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    getPage("On-Sale Product Management");
                }
                else
                {
                    MessageBox.Show("System Error! Please Contact The Help Desk.", "System error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
                    getPage("On-Sale Product Management");
                }
            }
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
                        }
                        else
                        {
                            OwnedImage.Replace(picProductIMG, ref IMG, OwnedImage.LoadDetached(ofd.FileName));
                            btnUploadIMG.Visible = false;
                            btnRemoveIMG.Visible = true;
                        }
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
            OwnedImage.Replace(picProductIMG, ref IMG, null);
            btnUploadIMG.Visible = true;
            btnRemoveIMG.Visible = false;
        }


        //Check the inputted data.
        private bool checkInfo()
        {
            //Clean previous error message.
            lblDescMsg.Text = lblPartNoMsg.Text = lblPriceMsg.Text
                = lblQtyForLMMsg.Text = lblQtyForSaleMsg.Text = "";

            //Check part number.
            if (cmbPartNumber.SelectedItem == null)
            {
                lblPartNoMsg.Text = "Please select a part number.";
                cmbPartNumber.Select();
                return false;
            }


            //Check price.
            int PQty;
            if (!int.TryParse(tbPrice.Text.ToString(), out PQty) || PQty <= 0 || PQty > 99999)
            {
                lblPriceMsg.Text = "minimum 1 maximum 99999.";
                tbPrice.Select();
                return false;
            }

            //Check quantity for sale.
            int SQty;
            if (!int.TryParse(tbQtyForSale.Text.ToString(), out SQty) || SQty <= 0
                                                                      || SQty > 99999 ||
                                                                      SQty > int.Parse(sparePartInfo.quantity))
            {
                lblQtyForSaleMsg.Text = "Can NOT larger than stock quantity AND minimum 1 maximum 99999.";
                tbQtyForSale.Select();
                return false;
            }

            //Check quantity for LM.
            int LMty;
            if (!int.TryParse(tbQtyForLM.Text.ToString(), out LMty) || LMty <= 0
                                                                    || LMty > 99999 ||
                                                                    LMty > int.Parse(sparePartInfo.quantity))
            {
                lblQtyForLMMsg.Text = "Can NOT larger than stock quantity AND minimum 1 maximum 99999.";
                tbQtyForLM.Select();
                return false;
            }

            //The sun of quantity for sale and quantity for LM cannot larger than the quantity of stock.
            if (int.Parse(tbQtyForLM.Text) + int.Parse(tbQtyForSale.Text) > int.Parse(sparePartInfo.quantity))
            {
                lblQtyForLMMsg.Text = lblQtyForSaleMsg.Text
                    = "The sun of quantity for sale AND quantity for LM CANNOT larger than quantity of the stock.";
                return false;
            }

            //Check description
            if (tbDescription.Text.Length < 2 || tbDescription.Text.Length > 500)
            {
                lblDescMsg.Text = "Description too short or too long, minimum 2 maximum 500.";
                tbDescription.Select();
                return false;
            }

            return true;
        }

        private void setValue()
        {
            newItemInfo = new ExpandoObject();
            newItemInfo.itemID = lblItemID.Text;
            newItemInfo.partNumber = cmbPartNumber.SelectedItem.ToString();
            newItemInfo.category = lblCat.Text;
            newItemInfo.OnSaleQty = tbQtyForSale.Text;
            newItemInfo.LM_OnSaleQty = tbQtyForLM.Text;
            newItemInfo.description = tbDescription.Text;
            newItemInfo.price = tbPrice.Text;
            newItemInfo.Status = chkStatus.Checked ? "Enable" : "Disable";
        }
    }
}
