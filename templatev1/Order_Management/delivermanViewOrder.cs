using System;
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
    public partial class delivermanViewOrder : Form
    {
        readonly dateHandler dateHandler = new dateHandler();
        AccountController accountController;
        UIController UIController;
        viewOrderController controller;
        private string uName, UID;
        string orderID;
        string shipDate;

        public delivermanViewOrder(string orderID)
        {
            InitializeComponent();
            controller = new viewOrderController();
            this.orderID = orderID;
        }

        public delivermanViewOrder(string orderID, AccountController accountController,
            UIController UIController)
        {
            InitializeComponent();
            this.orderID = orderID;
            this.accountController = accountController;
            this.UIController = UIController;
            controller = new viewOrderController();
            shipDate = "";
            UID = this.accountController.GetUid();
            lblUid.Text = $"Uid: {UID}";
        }

        private void delivermanViewOrder_Load(object sender, EventArgs e)
        {
            timer1.Enabled = true;
            lblLoc.Text += $" {orderID}";
            load_data();
            palSelect1.Visible =
                palSelect2.Visible = palSelect3.Visible = palSelect4.Visible = palSelect5.Visible = false;
            hideButton();
            setIndicator(UIController.getIndicator("Order Management"));
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

        public void load_data()
        {
            pnlSP.Controls.Clear();
            DataTable dt = controller.GetOrder(orderID);
            string[] f = dt.Rows[0][4].ToString().Split(' ');

            //order info
            lblOrderID.Text = orderID;
            lblOrderSerialNum.Text = $"{dt.Rows[0][3]}";
            lblOrderDate.Text = f[0];
            lblStaffIncharge.Text = $"{controller.GetStaffName(dt.Rows[0][2].ToString())}";
            lblStaffID.Text = $"{viewOrderController.GetStafftId(dt.Rows[0][2].ToString())}";
            lblStaffContact.Text = $"{controller.getStaffContact(dt.Rows[0][2].ToString())}";
            lblStatus.Text = $"{dt.Rows[0][6]}";

            if ($"{dt.Rows[0][6]}" == "Shipped") //if status is shipped, hide the job finish button
            {
                btnJobFinished.Visible = false;
                btnReturn.Location = new Point(1037, 820);
            }

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

                lblExpressNum.Text = "N/A";
            }
            else
            {
                lblDelivermanID.Text = dt.Rows[0][1].ToString();
                lblDelivermanName.Text = $"{delivermanDetail[0]} {delivermanDetail[1]}";
                lblDelivermanContact.Text = delivermanDetail[2];
                lblShippingDate.Text = daysUntilShipping >= 0
                    ? $"Scheduled on {shippingDate}"
                    : $"Delivered on {shippingDate}";

                lblExpressNum.Text = dt.Rows[0][4].ToString();
            }

            lblShippingAddress.Text = dt.Rows[0][5].ToString();
            if (lblStatus.Text == "Pending" || lblStatus.Text == "Processing")
            {
                lblDayUntil.Text = $"{daysUntilShipping} day(s) until shipping";
            }
            else
            {
                lblDayUntil.Text = "N/A";
            }

            //ordered spare part
            dt = new DataTable();
            dt = controller.getOrderedSparePart(orderID, "None");
            int row = dt.Rows.Count;


            int rowPosition = 6;
            for (int i = 1; i <= row; i++)
            {
                Label lblRowNum = new Label
                {
                    Name = $"lblRowNum{i}",
                    Text = $"{i.ToString()}.",
                    Location = new Point(3, rowPosition),
                    Font = new Font("Times New Roman", 12),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Size = new Size(33, 20)
                };
                Label lblItemNum = new Label
                {
                    Name = $"lblItemNum{i}",
                    Text = $"{controller.GetItemNum(dt.Rows[i - 1][0].ToString())}",
                    Location = new Point(38, rowPosition),
                    Font = new Font("Times New Roman", 12),
                    Size = new Size(109, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                Label lblPartNum = new Label
                {
                    Name = $"lblPartNum{i}",
                    Text = $"{dt.Rows[i - 1][0]}",
                    Location = new Point(153, rowPosition),
                    Font = new Font("Times New Roman", 12),
                    Size = new Size(128, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                Label lblPartName = new Label
                {
                    Name = $"lblPartName{i}",
                    Text = $"{controller.GetPartName(dt.Rows[i - 1][0].ToString())}",
                    Location = new Point(287, rowPosition),
                    Font = new Font("Times New Roman", 12),
                    Size = new Size(508, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                };
                Label lblQuantity = new Label
                {
                    Name = $"lblQuantity{i}",
                    Text = $"{dt.Rows[i - 1][2]}",
                    Location = new Point(801, rowPosition),
                    Font = new Font("Times New Roman", 12),
                    Size = new Size(116, 20),
                    TextAlign = ContentAlignment.MiddleCenter
                };


                rowPosition += 50;

                pnlSP.Controls.Add(lblRowNum);
                pnlSP.Controls.Add(lblItemNum);
                pnlSP.Controls.Add(lblPartNum);
                pnlSP.Controls.Add(lblPartName);
                pnlSP.Controls.Add(lblQuantity);
            }
        }


        private void btnReturn_Click(object sender, EventArgs e)
        {
            Form deliverman =
                new deliverman(accountController, UIController);
            Hide();
            deliverman.StartPosition = FormStartPosition.Manual;
            deliverman.Location = Location;
            deliverman.ShowDialog();
            Close();
        }

        private void btnJobFinished_Click(object sender, EventArgs e)
        {
            if (dateHandler.DayDifference(orderID) <= 0)
            {
                DialogResult dialogResult = MessageBox.Show("Are you sure this order is delivered?", "Job Finished",
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (dialogResult == DialogResult.Yes) //confirmed shipped
                {
                    if (controller.DelivermanJobFinished(orderID))
                    {
                        MessageBox.Show("Order status changed.", "Job Finished");
                        Form d =
                            new delivermanViewOrder(orderID, accountController, UIController);
                        Hide();
                        d.StartPosition = FormStartPosition.Manual;
                        d.Location = Location;
                        d.ShowDialog();
                        Close();
                    }
                    else
                    {
                        MessageBox.Show("Please try again.", "Job Finished", MessageBoxButtons.OK,
                            MessageBoxIcon.Error);
                    }
                }
            }
            else
            {
                MessageBox.Show("Today is not shipping date.", "Job Finished", MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTimeDate.Text = DateTime.Now.ToString("yyyy/MM/dd   HH:mm:ss");
        }

        private void btnRelayEdit_Click_1(object sender, EventArgs e)
        {
            Form d = new DeliverymanEditOrderRelay(orderID, accountController, UIController);
            Hide();
            d.StartPosition = FormStartPosition.Manual;
            d.Location = Location;
            d.ShowDialog();
            Close();
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
