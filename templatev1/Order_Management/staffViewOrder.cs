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
using LMCIS.Stock_Manag;
using LMCIS.System_page;
using LMCIS.User_Manag;

namespace LMCIS.Order_Management
{
    public partial class staffViewOrder : Form
    {
        readonly dateHandler dateHandler = new dateHandler();
        AccountController accountController;
        UIController UIController;
        staffViewOrderController controller;
        private string uName, UID;
        string orderID;
        string shipDate;
        bool isLMOrder;
        bool isManager;
        bool isStoreman;

        public staffViewOrder(string orderID)
        {
            InitializeComponent();
            controller = new staffViewOrderController();
            this.orderID = orderID;
        }

        public staffViewOrder(string orderID, AccountController accountController,
            UIController UIController)
        {
            InitializeComponent();
            this.orderID = orderID;
            this.accountController = accountController;
            this.UIController = UIController;
            controller = new staffViewOrderController();
            shipDate = "";
            UID = this.accountController.GetUid();
            lblUid.Text = $"Uid: {UID}";
            isManager = accountController.CheckIsManager();
            isStoreman = accountController.CheckIsStoreman();
        }

        private void clerkViewOrder_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            cmbSortOrder.SelectedIndex = 0;
            lblLoc.Text += $" {orderID}";
            load_data(cmbSortOrder.Text);
            palSelect1.Visible =
                palSelect2.Visible = palSelect3.Visible = palSelect4.Visible = palSelect5.Visible = false;
            hideButton();
            setIndicator(UIController.getIndicator("Order Management"));

            if (!isStoreman)
            {
                btnReadyToShip.Visible = false;
                btnDelete.Location = new Point(499, 895);
            }
        }

        public void load_data(string sortBy)
        {
            pnlSP.Controls.Clear();
            DataTable dt = controller.getOrder(orderID);
            string[] f = dt.Rows[0][4].ToString().Split(' ');

            //order info
            lblOrderID.Text = orderID;
            lblOrderSerialNum.Text = $"{dt.Rows[0][3]}";
            lblOrderDate.Text = f[0];
            lblStaffIncharge.Text = $"{controller.getStaffName(dt.Rows[0][2].ToString())}";
            lblStaffID.Text = $"{controller.getStafftID(dt.Rows[0][2].ToString())}";
            lblStaffContact.Text = $"{controller.getStaffContact(dt.Rows[0][2].ToString())}";
            lblStatus.Text = $"{dt.Rows[0][6]}";

            //delivery info
            dt = new DataTable();
            dt = controller.GetShippingDetail(orderID);
            int daysUntilShipping = dateHandler.DayDifference((DateTime)dt.Rows[0]["shippingDate"]);
            string shippingDate = dt.Rows[0][2].ToString();
            string[]
                d = shippingDate
                    .Split(' '); //since the database also store the time follwing the date, split it so that only date will be display
            shippingDate = d[0];
            shipDate = shippingDate;
            string[] delivermanDetail = controller.GetDelivermanDetail(orderID);
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
            dt = controller.getOrderedSparePart(orderID, sortBy);
            int row = dt.Rows.Count;


            int rowPosition = 8;
            int orderTotalPrice = 0;
            for (int i = 1; i <= row; i++)
            {
                Label lblRowNum = new Label
                {
                    Name = $"lblRowNum{i}",
                    Text = $"{i.ToString()}.",
                    Location = new Point(3, rowPosition),
                    Font = new Font("Times New Roman", 12),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(30, 20)
                };
                Label lblItemNum = new Label
                {
                    Name = $"lblItemNum{i}",
                    Text = $"{controller.getItemNum(dt.Rows[i - 1][0].ToString())}",
                    Location = new Point(38, rowPosition),
                    Font = new Font("Times New Roman", 12),
                    Size = new Size(83, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                Label lblPartNum = new Label
                {
                    Name = $"lblPartNum{i}",
                    Text = $"{dt.Rows[i - 1][0]}",
                    Location = new Point(127, rowPosition),
                    Font = new Font("Times New Roman", 12),
                    Size = new Size(97, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                Label lblPartName = new Label
                {
                    Name = $"lblPartName{i}",
                    Text = $"{controller.getPartName(dt.Rows[i - 1][0].ToString())}",
                    Location = new Point(230, rowPosition),
                    Font = new Font("Times New Roman", 12),
                    Size = new Size(300, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                Label lblQuantity = new Label
                {
                    Name = $"lblQuantity{i}",
                    Text = $"{dt.Rows[i - 1][2]}",
                    Location = new Point(536, rowPosition),
                    Font = new Font("Times New Roman", 12),
                    Size = new Size(106, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                Label lblUnitPrice = new Label
                {
                    Name = $"lblUnitPrice{i}",
                    Text = $"¥{dt.Rows[i - 1][3]}",
                    Location = new Point(648, rowPosition),
                    Font = new Font("Times New Roman", 12),
                    Size = new Size(144, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                Label lblRowTotalPrice = new Label
                {
                    Name = $"lblRowTotalPrice{i}",
                    Text = $"¥{dt.Rows[i - 1][4]}",
                    Location = new Point(798, rowPosition),
                    Font = new Font("Times New Roman", 12),
                    Size = new Size(114, 20),
                    TextAlign = ContentAlignment.MiddleCenter
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


        private void cmbSortOrder_SelectedIndexChanged(object sender, EventArgs e)
        {
            load_data(cmbSortOrder.Text);
        }

        private void btnReturn_Click(object sender, EventArgs e)
        {
            Form o = new staffOrderList(accountController, UIController);
            Hide();
            o.StartPosition = FormStartPosition.Manual;
            o.Location = Location;
            o.ShowDialog();
            Close();
            return;
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
                    MessageBox.Show("Invoice can only be view after 1 day of delivery", "View Invoice",
                        MessageBoxButtons.OK, MessageBoxIcon.Stop);
                }
                else
                {
                    Form o = new staffViewInvoice(orderID, accountController, UIController, false);
                    Hide();
                    o.StartPosition = FormStartPosition.Manual;
                    o.Location = Location;
                    o.ShowDialog();
                    Close();
                    return;
                }
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTimeDate.Text = DateTime.Now.ToString("yyyy/MM/dd   HH:mm:ss");
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
                            $"Are you sure you want to cancel order {orderID} ?\nYour action cannot be revoked after confirming it.",
                            "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    //add qty back to db
                    //get part num and it's qty in the order
                    Dictionary<string, int> partNumQty = controller.GetPartNumWithQty(orderID);
                    //add back now;
                    //no need now
                    //foreach (KeyValuePair<string, int> q in partNumQty)
                    //{
                    //    controller.addQtyback(q.Key, q.Value, UID);
                    //}

                    if (dialogResult == DialogResult.Yes && controller.DeleteOrder(orderID))
                    {
                        MessageBox.Show("Cancel successful.", " Cancel Successful", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        Form customerOrderList =
                            new staffOrderList(accountController, UIController);
                        Hide();
                        customerOrderList.StartPosition = FormStartPosition.Manual;
                        customerOrderList.Location = Location;
                        customerOrderList.ShowDialog();
                        Close();
                    }
                    else if (dialogResult == DialogResult.Yes && !controller.DeleteOrder(orderID))
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

        private void btnDIC_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text == "Cancelled")
            {
                MessageBox.Show(
                    lblStatus.Text == "Cancelled" ? "Order already cancelled." : "Order already finish.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Form o = new DIC(orderID, accountController, UIController);
                Hide();
                o.StartPosition = FormStartPosition.Manual;
                o.Location = Location;
                o.ShowDialog();
                Close();
            }
        }

        private void btnDID_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text == "Cancelled")
            {
                MessageBox.Show(
                    lblStatus.Text == "Cancelled" ? "Order already cancelled." : "Order already finish.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                Form o = new DID_List(orderID, accountController, UIController);
                Hide();
                o.StartPosition = FormStartPosition.Manual;
                o.Location = Location;
                o.ShowDialog();
                Close();
                return;
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
                    Form o =
                        new staffEditOrder(orderID, accountController, UIController);
                    Hide();
                    o.StartPosition = FormStartPosition.Manual;
                    o.Location = Location;
                    o.ShowDialog();
                    Close();
                    return;
                }
                else
                {
                    MessageBox.Show("Order cannot be edited when it is ready to ship", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void btnReadyToShip_Click(object sender, EventArgs e)
        {
            if (controller.ReadyToShipANDDeductQty(orderID))
            {
                MessageBox.Show("The status has now changed to Ready to ship.", "System message", MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
            }
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (lblStatus.Text == "Pending")
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure confirming this order?", "Confirm Order",
                    MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    if (controller.confirmOrder(orderID))
                    {
                        MessageBox.Show("Order confirmed", "Confirm Order", MessageBoxButtons.OK);
                        Form home = new staffViewOrder(orderID, accountController, UIController);
                        Hide();
                        home.StartPosition = FormStartPosition.Manual;
                        home.Location = Location;
                        home.ShowDialog();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Please try again", "Confirm Order", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
                else if (dialogResult == DialogResult.No)
                {
                }
            }
            else
            {
                MessageBox.Show("Order already confirm", "Confirm Order", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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
