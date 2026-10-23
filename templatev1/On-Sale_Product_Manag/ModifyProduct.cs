using System;
using System.Drawing;
using System.Dynamic;
using System.IO;
using System.Windows.Forms;
using LMCIS.controller;
using LMCIS.Online_Ordering_Platform;
using LMCIS.Order_Management;
using LMCIS.Profile;
using LMCIS.Stock_Manag;
using LMCIS.System_page;
using LMCIS.Utilities;
using LMCIS.User_Manag;

namespace LMCIS.On_Sale_Product_Manag
{
    public partial class OnSaleModify : Form
    {
        private string uName, UID;
        private int index;
        Bitmap IMG;
        bool IMGUploaded;
        dynamic placeholder, update;
        AccountController accountController;
        UIController UIController;
        OnSaleProductController onSaleProductController;


        public OnSaleModify(AccountController accountController, UIController UIController,
            OnSaleProductController onSaleProductController)
        {
            InitializeComponent();
            OwnedImage.TrackDisposal(picProductIMG, () => IMG);
            palSelect1.Visible =
                palSelect2.Visible = palSelect3.Visible = palSelect4.Visible = palSelect5.Visible = false;
            this.accountController = accountController;
            this.UIController = UIController;
            this.onSaleProductController = onSaleProductController;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Initialization();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTimeDate.Text = DateTime.Now.ToString("yyyy/MM/dd   HH:mm:ss");
        }

        private void Initialization()
        {
            setIndicator(UIController.getIndicator("On-Sale Product Management"));
            timer1.Enabled = true;
            UID = accountController.GetUid();
            uName = accountController.GetName();
            lblUid.Text = "UID: " + UID;

            //For item info.
            placeholder = onSaleProductController.GetModifyItemInfo();
            lblItemID.Text = placeholder.itemID;
            lblItemName.Text = placeholder.name;
            lblOnShelveDate.Text = placeholder.onShelvesDate.ToString("yyyy/MM/dd");
            lblLastMod.Text = placeholder.lastModified;
            lblQty.Text = placeholder.quantity;
            lblPartNo.Text = placeholder.partNumber;
            lblSuppilerID.Text = placeholder.supplierID;
            lblCountry.Text = placeholder.country;
            lblSuppiler.Text = placeholder.suppName;
            lblPCat.Text = placeholder.type;
            tbPrice.Text = placeholder.price;
            tbQtyForSale.Text = placeholder.onSaleQty;
            tbQtyForLM.Text = placeholder.LM_onSaleQty;
            tbDescription.Text = placeholder.description;
            chkStatus.Checked = placeholder.status.Equals("Enable") ? true : false;

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

        private void tbPrice_Enter(object sender, EventArgs e)
        {
            if (tbPrice.Text == placeholder.price)
                tbPrice.Text = "";
        }

        private void tbPrice_Leave(object sender, EventArgs e)
        {
            if (tbPrice.Text == "")
                tbPrice.Text = placeholder.price;
        }

        private void tbQtyForSale_Enter(object sender, EventArgs e)
        {
            if (tbQtyForSale.Text == placeholder.onSaleQty)
                tbQtyForSale.Text = "";
        }

        private void tbQtyForSale_Leave(object sender, EventArgs e)
        {
            if (tbQtyForSale.Text == "")
                tbQtyForSale.Text = placeholder.onSaleQty;
        }

        private void tbQtyForLM_Enter(object sender, EventArgs e)
        {
            if (tbQtyForLM.Text == placeholder.LM_onSaleQty)
                tbQtyForLM.Text = "";
        }

        private void tbQtyForLM_Leave(object sender, EventArgs e)
        {
            if (tbQtyForLM.Text == "")
                tbQtyForLM.Text = placeholder.LM_onSaleQty;
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

        private void btnModify_Click(object sender, EventArgs e)
        {
            if (checkInfo())
            {
                if (onSaleProductController.ModifyItemInfo(update))
                {
                    MessageBox.Show("Modify successful!", "System message", MessageBoxButtons.OK,
                        MessageBoxIcon.Information);
                    getPage("On-Sale Product Management");
                }
                else //Something wrong from the controller.
                    MessageBox.Show("System Error! Please Contact The Help Desk.", "System error", MessageBoxButtons.OK,
                        MessageBoxIcon.Warning);
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


        private bool checkInfo()
        {
            lblDescMsg.Text = lblPriceMsg.Text = lblSaleForLMMsg.Text = lblSaleQtyMsg.Text = "";
            update = new ExpandoObject();

            //Check and update item price if have change.
            if (tbPrice.Text != placeholder.price)
            {
                int Qty;
                if (!int.TryParse(tbPrice.Text.ToString(), out Qty) || Qty <= 0 || Qty > 99999)
                {
                    lblPriceMsg.Text = "minimum 1 maximum 99999.";
                    tbPrice.Select();
                    return false;
                }

                update.price = tbPrice.Text;
            }
            else
                update.price = placeholder.price;

            //Check and update quantity for sale if have change.
            if (tbQtyForSale.Text != placeholder.onSaleQty)
            {
                int Qty;
                if (!int.TryParse(tbQtyForSale.Text.ToString(), out Qty) || Qty <= 0 ||
                    Qty > int.Parse(placeholder.quantity))
                {
                    lblSaleQtyMsg.Text = "Can NOT larger than stock quantity AND minimum 1 maximum 99999.";
                    tbQtyForSale.Select();
                    return false;
                }

                update.onSaleQty = tbQtyForSale.Text;
            }
            else
                update.onSaleQty = placeholder.onSaleQty;

            //Check and update quantity for LM if have change.
            if (tbQtyForLM.Text != placeholder.LM_onSaleQty)
            {
                int Qty;
                if (!int.TryParse(tbQtyForLM.Text.ToString(), out Qty) || Qty <= 0 ||
                    Qty > int.Parse(placeholder.quantity))
                {
                    lblSaleForLMMsg.Text = "Can NOT lower than danger level AND minimum 1 maximum 99999.";
                    tbQtyForLM.Select();
                    return false;
                }

                update.LM_onSaleQty = tbQtyForLM.Text;
            }
            else
                update.LM_onSaleQty = placeholder.LM_onSaleQty;

            //The sun of quantity for sale and quantity for LM cannot larger than the quantity of stock.
            if (int.Parse(tbQtyForLM.Text) + int.Parse(tbQtyForSale.Text) > int.Parse(placeholder.quantity))
            {
                lblSaleForLMMsg.Text = lblSaleQtyMsg.Text
                    = "The sun of quantity for sale AND quantity for LM CANNOT larger than quantity of the stock.";
                return false;
            }


            //Check and update status
            update.status = chkStatus.Checked == true ? "Enable" : "Disable";

            //Check and update description if have change.
            if (tbDescription.Text != placeholder.description)
            {
                if (tbDescription.Text.Length < 2 || tbDescription.Text.Length > 500)
                {
                    lblDescMsg.Text = "Description too short or too long, minimum 2 maximum 500.";
                    tbDescription.Select();
                    return false;
                }

                update.description = tbDescription.Text;
            }
            else
                update.description = placeholder.description;

            return true;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            getPage("On-Sale Product Management");
        }
    }
}
