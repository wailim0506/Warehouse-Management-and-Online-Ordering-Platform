using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using LMCIS.controller;
using LMCIS.controller.Utilities;
using LMCIS.On_Sale_Product_Manag;
using LMCIS.Online_Ordering_Platform;
using LMCIS.Profile;
using LMCIS.Properties;
using LMCIS.Stock_Manag;
using LMCIS.System_page;
using LMCIS.User_Manag;
using Microsoft.Extensions.Logging;
using LMCIS.Properties;

namespace LMCIS.Order_Management
{
    public partial class customerViewOrder : Form
    {
        readonly dateHandler _dateHandler = new dateHandler();
        AccountController _accountController;
        UIController _uiController;
        viewOrderController _controller;
        private string _uName, _uid;
        string _orderId;
        string _shipDate;
        private Boolean _isLm;

        public customerViewOrder(string orderId)
        {
            InitializeComponent();
            _controller = new viewOrderController();
            this._orderId = orderId;
        }

        public customerViewOrder(string orderId, AccountController accountController,
            UIController uiController)
        {
            InitializeComponent();
            this._orderId = orderId;
            this._accountController = accountController;
            this._uiController = uiController;
            _controller = new viewOrderController();
            _shipDate = "";
            _uid = this._accountController.GetUid();
            _isLm = accountController.GetIsLm();
            lblUid.Text = $"Uid: {_uid}";
        }


        private void customerViewOrder_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            cmbSortOrder.SelectedIndex = 0;
            lblLoc.Text += $" {_orderId}";
            load_data(cmbSortOrder.Text);
            palSelect1.Visible =
                palSelect2.Visible = palSelect3.Visible = palSelect4.Visible = palSelect5.Visible = false;
            HideButton();
            SetIndicator(_uiController.getIndicator("Order Management"));
        }

        public void HideButton()
        {
            dynamic btnFun = _uiController.showFun();
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

        private void SetIndicator(int btnNo)
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

        public void load_data(string sortBy)
        {
            pnlSP.Controls.Clear();
            DataTable dt = _controller.GetOrder(_orderId);
            string[] f = dt.Rows[0][4].ToString().Split(' ');

            //order info
            lblOrderID.Text = _orderId;
            lblOrderSerialNum.Text = $"{dt.Rows[0][3]}";
            lblOrderDate.Text = f[0];
            lblStaffIncharge.Text = $"{_controller.GetStaffName(dt.Rows[0][2].ToString())}";
            lblStaffID.Text = $"{viewOrderController.GetStafftId(dt.Rows[0][2].ToString())}";
            lblStaffContact.Text = $"{_controller.getStaffContact(dt.Rows[0][2].ToString())}";
            lblStatus.Text = $"{dt.Rows[0][6]}";

            // if the order is not in the process of being shipped, it will not be displayed
            if (lblStatus.Text != "Pending" && lblStatus.Text != "Cancelled")
            {
                btnViewDelivery.Visible = true;
                btnViewDelivery.Enabled = true;
                btnViewDelivery.Controls.Clear();
                Log.LogMessage(LogLevel.Information, "[View] CustomerViewOrder ",
                    "load_data: Order is not in the process of being shipped");
            }
            else
            {
                btnViewDelivery.Visible = false;
                btnViewDelivery.Enabled = false;
                btnViewDelivery.Controls.Clear();
            }

            //delivery info
            dt = new DataTable();
            dt = _controller.GetShippingDetail(_orderId);
            int daysUntilShipping = _dateHandler.DayDifference((DateTime)dt.Rows[0]["shippingDate"]);
            string shippingDate = dt.Rows[0][2].ToString();
            string[]
                d = shippingDate
                    .Split(' '); //since the database also store the time follwing the date, split it so that only date will be display
            shippingDate = d[0];
            _shipDate = shippingDate;
            string[] delivermanDetail = _controller.GetDelivermanDetail(_orderId);
            if (lblStatus.Text == "Cancelled")
            {
                lblDelivermanID.Text = "N/A";
                lblDelivermanName.Text = "N/A";
                lblDelivermanContact.Text = "N/A";
                lblShippingDate.Text = "N/A";

            }
            else
            {
                lblDelivermanID.Text = dt.Rows[0][1].ToString();
                lblDelivermanName.Text = $"{delivermanDetail[0]} {delivermanDetail[1]}";
                lblDelivermanContact.Text = delivermanDetail[2];
                lblShippingDate.Text = daysUntilShipping >= 0
                    ? $"Scheduled on {shippingDate}"
                    : $"Delivered on {shippingDate}";

            }

            lblShippingAddress.Text = dt.Rows[0][5].ToString();
            if (lblStatus.Text == "Pending" || lblStatus.Text == "Processing" || lblStatus.Text == "Ready to Ship")
            {
                lblDayUntil.Text = $"{daysUntilShipping} day(s) until shipping";
            }
            else
            {
                lblDayUntil.Text = "N/A";
            }

            //ordered spare part
            dt = new DataTable();
            dt = _controller.getOrderedSparePart(_orderId, sortBy);
            int row = dt.Rows.Count;


            int rowPosition = 8;
            int orderTotalPrice = 0;
            for (int i = 1; i <= row; i++)
            {
                Label lblRowNum = new Label
                {
                    Name = $"lblRowNum{i}", Text = $"{i.ToString()}.",
                    Location = new Point(3, rowPosition), Font = new Font("Times New Roman", 12),
                    TextAlign = ContentAlignment.MiddleCenter, Size = new Size(30, 20)
                };
                Label lblItemNum = new Label
                {
                    Name = $"lblItemNum{i}", Text = $"{_controller.GetItemNum(dt.Rows[i - 1][0].ToString())}",
                    Location = new Point(38, rowPosition), Font = new Font("Times New Roman", 12),
                    Size = new Size(93, 20), TextAlign = ContentAlignment.MiddleCenter
                };
                Label lblPartNum = new Label
                {
                    Name = $"lblPartNum{i}", Text = $"{dt.Rows[i - 1][0]}",
                    Location = new Point(127, rowPosition), Font = new Font("Times New Roman", 12),
                    Size = new Size(97, 20), TextAlign = ContentAlignment.MiddleCenter
                };
                Label lblPartName = new Label
                {
                    Name = $"lblPartName{i}", Text = $"{_controller.GetPartName(dt.Rows[i - 1][0].ToString())}",
                    Location = new Point(230, rowPosition), Font = new Font("Times New Roman", 12),
                    Size = new Size(300, 20), TextAlign = ContentAlignment.MiddleCenter
                };
                Label lblQuantity = new Label
                {
                    Name = $"lblQuantity{i}", Text = $"{dt.Rows[i - 1][2]}",
                    Location = new Point(536, rowPosition), Font = new Font("Times New Roman", 12),
                    Size = new Size(106, 20), TextAlign = ContentAlignment.MiddleCenter
                };
                Label lblUnitPrice = new Label
                {
                    Name = $"lblUnitPrice{i}", Text = $"¥{dt.Rows[i - 1][3]}",
                    Location = new Point(648, rowPosition), Font = new Font("Times New Roman", 12),
                    Size = new Size(144, 20), TextAlign = ContentAlignment.MiddleCenter
                };
                Label lblRowTotalPrice = new Label
                {
                    Name = $"lblRowTotalPrice{i}", Text = $"¥{dt.Rows[i - 1][4]}",
                    Location = new Point(798, rowPosition), Font = new Font("Times New Roman", 12),
                    Size = new Size(114, 20), TextAlign = ContentAlignment.MiddleCenter
                };


                rowPosition += 50;
                orderTotalPrice += (int.Parse(dt.Rows[i - 1][2].ToString()) * int.Parse(dt.Rows[i - 1][3].ToString()));
                lblOrderTotalPrice.Text = $"¥ {orderTotalPrice.ToString()}";

                pnlSP.Controls.Add(lblRowNum);
                pnlSP.Controls.Add(lblItemNum);
                pnlSP.Controls.Add(lblPartNum);
                pnlSP.Controls.Add(lblPartName);
                pnlSP.Controls.Add(lblQuantity);
                pnlSP.Controls.Add(lblUnitPrice);
                pnlSP.Controls.Add(lblRowTotalPrice);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTimeDate.Text = DateTime.Now.ToString("yyyy/MM/dd   HH:mm:ss");
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text == "Cancelled" || lblStatus.Text == "Shipped")
            {
                MessageBox.Show(lblStatus.Text == "Cancelled" ? "Order already cancelled." : "Order already finish.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (lblStatus.Text != "Ready to Ship")
                {
                    Form customerEditOrder = new customerEditOrder(_orderId, _accountController, _uiController);
                    Hide();
                    customerEditOrder.StartPosition = FormStartPosition.Manual;
                    customerEditOrder.Location = Location;
                    customerEditOrder.ShowDialog();
                    Close();
                }
                else
                {
                    MessageBox.Show("Order cannot be edited when it is ready to ship", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text == "Cancelled" || lblStatus.Text == "Shipped")
            {
                MessageBox.Show(
                    lblStatus.Text == "Cancelled" ? "Order already cancelled." : "Order already finish.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (lblStatus.Text != "Ready to Ship")
                {
                    DialogResult dialogResult =
                        MessageBox.Show(
                            $"Are you sure you want to cancel order {_orderId} ?\nYour action cannot be revoked after confirming it.",
                            "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    //add qty back to db
                    //get part num and it's qty in the order
                    Dictionary<string, int> partNumQty = _controller.GetPartNumWithQty(_orderId);
                    //add back now;

                    //no need to add back now as the qty will not be deducted when create order
                    //foreach (KeyValuePair<string, int> q in partNumQty)
                    //{
                    //    controller.addQtyback(q.Key, q.Value, UID);
                    //}

                    if (dialogResult == DialogResult.Yes && _controller.DeleteOrder(_orderId))
                    {
                        MessageBox.Show("Cancel successful.", " Cancel Successful", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        Form customerOrderList =
                            new customerOrderList(_accountController, _uiController);
                        Hide();
                        customerOrderList.StartPosition = FormStartPosition.Manual;
                        customerOrderList.Location = Location;
                        customerOrderList.ShowDialog();
                        Close();
                    }
                    else if (dialogResult == DialogResult.Yes && !_controller.DeleteOrder(_orderId))
                    {
                        MessageBox.Show("Something went wrong.\nPlease contact our staff for help", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show("Order cannot be cancel when it is ready to ship", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            Form customerOrderList = new customerOrderList(_accountController, _uiController);
            Hide();
            customerOrderList.StartPosition = FormStartPosition.Manual;
            customerOrderList.Location = Location;
            customerOrderList.ShowDialog();
            Close();
        }

        private void btnViewInvoice_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text == "Cancelled")
            {
                MessageBox.Show("Order already cancelled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (lblStatus.Text != "Shipped")
                {
                    MessageBox.Show("Invoice can only be view after delivery", "View Invoice",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    Log.LogMessage(LogLevel.Information, "[View] Invoice Management", $"User is going to the view invoice page.");
                    Form customerViewInvoice = new customerViewInvoice(_orderId, _accountController, _uiController);
                    Hide();
                    customerViewInvoice.StartPosition = FormStartPosition.Manual;
                    customerViewInvoice.Location = Location;
                    customerViewInvoice.ShowDialog();
                    Close();
                }
            }
        }


        private void btnReorder_Click(object sender, EventArgs e)
        {
            //get all part num and qty in the order first
            Dictionary<string, int> partNumQty = _controller.GetPartNumWithQty(_orderId);
            //add to cart
            try
            {
                foreach (var k in partNumQty)
                {
                    if (k.Value <= _controller.checkOnSaleQty(k.Key))
                    {
                        _controller.reOrder(_uid, k.Key, k.Value, _isLm);
                    }
                    else
                    {
                        DialogResult dialogResult2 =
                            MessageBox.Show($"We dont have enough quantity for {k.Key}.\nContinue to add other item?",
                                "Re-order", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (dialogResult2 == DialogResult.Yes)
                        {
                            continue;
                        }

                        DialogResult dialogResult3 = MessageBox.Show("Clear item in cart?", "Re-order",
                            MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        if (dialogResult3 == DialogResult.Yes)
                        {
                            List<string> allPartNum = _controller.GetAllPartNumInCart(_uid);
                            List<int> allItemQty = _controller.GetAllItemQtyInCart(_uid);
                            for (int i = 0; i < allPartNum.Count; i++)
                            {
                                _controller.AddQtyBack(allPartNum[i], allItemQty[i], 0, _isLm); //add qty back to db
                            }

                            if (_controller.RemoveAll(_uid)) //remove from cart
                            {
                                MessageBox.Show("All items removed from cart", "Remove All", MessageBoxButtons.OK);
                            }

                            return;
                        }

                        return;
                    }
                }

                DialogResult dialogResult =
                    MessageBox.Show("All available item in this order added to cart.\nProceed to cart to create order?",
                        "Re-order", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    Form cart = new cart(_accountController, _uiController);
                    Hide();
                    cart.StartPosition = FormStartPosition.Manual;
                    cart.Location = Location;
                    cart.ShowDialog();
                    Close();
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Please try again.", "Re-order", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void cmbSortOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_data(cmbSortOrder.Text);
        }

        private void BwMode()
        {
            dynamic value = _uiController.getMode();
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

        private void getPage(string Function)
        {
            Form next = new Home(_accountController, _uiController);
            switch (Function)
            {
                case "Order Management":
                    if (_uid.StartsWith("LMC"))
                    {
                        next = new customerOrderList(_accountController, _uiController);
                    }
                    else if (_accountController.CheckIsDeliverman())
                    {
                        next = new deliverman(_accountController, _uiController);
                    }
                    else
                    {
                        next = new staffOrderList(_accountController, _uiController);
                    }

                    break;
                case "Spare Part":
                    next = new sparePartList(_accountController, _uiController);
                    break;
                case "Cart":
                    next = new cart(_accountController, _uiController);
                    break;
                case "Favourite":
                    next = new favourite(_accountController, _uiController);
                    break;
                case "Give Feedback":
                    next = new giveFeedback(_accountController, _uiController);
                    break;

                case "On-Sale Product Management":
                    next = new OnSaleMain(_accountController, _uiController);
                    break;
                case "Stock Management":
                    next = new StockMgmt(_accountController, _uiController);
                    break;
                case "User Management":
                    next = new SAccManage(_accountController, _uiController);
                    break;
                case "Invoice Management":
                    next = new staffInvoiceList(_accountController, _uiController);
                    break;
            }

            Log.LogMessage(LogLevel.Information, "[View] Order Management", $"User: {_uid} is going to the {Function} page.");
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
            Log.LogMessage(LogLevel.Information, "[View] Order Management", $"User: {_uid} is going to the profile page.");
            proFileController proFileController = new proFileController(_accountController);

            proFileController.setType(_accountController.GetAccountType());

            Form proFile = new proFileMain(_accountController, _uiController, proFileController);
            Hide();
            //Swap the current form to another.
            proFile.StartPosition = FormStartPosition.Manual;
            proFile.Location = Location;
            proFile.ShowDialog();
            Close();
        }

        private void picHome_Click(object sender, EventArgs e)
        {
            Log.LogMessage(LogLevel.Information, "[View] Order Management", $"User: {_uid} is going to the home page.");
            Form home = new Home(_accountController, _uiController);
            Hide();
            //Swap the current form to another.
            home.StartPosition = FormStartPosition.Manual;
            home.Location = Location;
            home.ShowDialog();
            Close();
        }

        private void lblCorpName_Click(object sender, EventArgs e)
        {
            Log.LogMessage(LogLevel.Information, "[View] Order Management", $"User: {_uid} is going to the about page.");
            Form about = new About(_accountController, _uiController);
            Hide();
            //Swap the current form to another.
            about.StartPosition = FormStartPosition.Manual;
            about.Location = Location;
            about.Size = Size;
            about.ShowDialog();
            Close();
        }


        private void btnViewDelivery_Click(object sender, EventArgs e)
        {
            Log.LogMessage(LogLevel.Information, "[View] Order Management", $"User: {_uid} is going to the view delivery page.");
            Form o = new CustomerViewOrderDelivery(_orderId, _accountController, _uiController, _controller);
            Hide();
            o.StartPosition = FormStartPosition.Manual;
            o.Location = Location;
            o.ShowDialog();
            Close();
        }
    }
}
