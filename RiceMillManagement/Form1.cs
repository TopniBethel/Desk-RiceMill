using FontAwesome.Sharp;
using RiceMillManagement.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RiceMillManagement
{
    public partial class Form1 : Form
    {
        private IconButton currentBtn;
        private IconButton currentSubBtn;
        private IconButton currentSubSubBtn;
        private Panel leftBorderBtn;
        private string login;
        public Form1(string login)
        {
            InitializeComponent();
            this.login = login;
            leftBorderBtn = new Panel();
            // leftBorderBtn.Size = new Size(7, 60);
            leftBorderBtn.Size = new Size(0, 0);
            panelMenu.Controls.Add(leftBorderBtn);
            panelMenu.AutoSize = true;
            timer1.Start();
            CompanyDetails();
            shopinfo();
            Get();
            
        }
        void CompanyDetails()
        {
            string qry = "select * from CompanyDetails_tbl";
            SqlCommand cmd = new SqlCommand(qry, Database.con);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                TechPartner_txt.Text = rdr["companyname"].ToString();
                CompanyAddress_txt.Text = rdr["address"].ToString();
                companyContact_txt.Text = rdr["contact"].ToString();
                CompanyMail_txt.Text = rdr["mailid"].ToString();
                CompanyWebsite_txt.Text = rdr["website"].ToString();


            }

        }

        string shopname;
        string gstno;
        string dlno;
        string address;
        string place;
        string shopcontact;

        void shopinfo()
        {
            string qry = "select * from ShopInfo_tbl ";
            SqlCommand cmd = new SqlCommand(qry, Database.con);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {

                shopname = rdr["shopname"].ToString();
                ShopName_txt.Text = rdr["shopname"].ToString();
                gstno = rdr["gstno"].ToString();
                GST_txt.Text = rdr["gstno"].ToString();
                dlno = rdr["dlno"].ToString();
                DlNo_txt.Text = rdr["dlno"].ToString();
                address = rdr["address"].ToString();
                Addres_txt.Text = rdr["address"].ToString();
                shopcontact = rdr["contact"].ToString();
                Contact_txt.Text = rdr["contact"].ToString();
                place = rdr["Place"].ToString();
                Place_txt.Text = rdr["Place"].ToString();

                /*  if (rdr["logo"] != DBNull.Value)
                  {
                      photo_aray = (byte[])rdr["logo"];
                      if (photo_aray.Length > 0)
                      {
                          ms = new MemoryStream(photo_aray);

                          iconPictureBox1.Image = Image.FromStream(ms);
                          iconPictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                      }
                      else
                      {
                          iconPictureBox1.SizeMode = iconPictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                      }

                  }

                  */
            }
        }
        void Get()
        {

            //today sales

        /*    string qry10 = "select sum(amount) from PurchaseRound_tbl where dt = '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd") + "' ";
            SqlCommand cmd10 = new SqlCommand(qry10, Database.con);
            SqlDataReader rdr10 = cmd10.ExecuteReader();
            if (rdr10.Read())
            {
                string purchase = rdr10[0].ToString();
                if (purchase == "")
                {
                    purchase = "0.00";
                }
                Todaypurchase_lbl.Text = Convert.ToDecimal(purchase).ToString("0.00");
            }

            string qrys = "select sum(finalamount) from SalesRound_tbl where dt = '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd") + "' ";
            SqlCommand cmds = new SqlCommand(qrys, Database.con);
            SqlDataReader rdrs = cmds.ExecuteReader();
            if (rdrs.Read())
            {

                string sales = rdrs[0].ToString();
                if (sales == "")
                {
                    sales = "0.00";
                }

                Todaysales_lbl.Text = Convert.ToDecimal(sales).ToString("0.00");

            }
        */

            //Income 
            string qry11 = "select sum(amount) from Income_tbl where date = '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd") + "' ";
            SqlCommand cmd11 = new SqlCommand(qry11, Database.con);
            SqlDataReader rdr11 = cmd11.ExecuteReader();
            if (rdr11.Read())
            {
                string inomce = rdr11[0].ToString();
                if (inomce == "")
                {
                    inomce = "0.00";
                }
                Income_lbl.Text = Convert.ToDecimal(inomce).ToString("0.00");

            }

            //Expense
            string qry12 = "select sum(amount) from Expense_tbl where date = '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd") + "' ";
            SqlCommand cmd12 = new SqlCommand(qry12, Database.con);
            SqlDataReader rdr12 = cmd12.ExecuteReader();
            if (rdr12.Read())
            {
                string expense = rdr12[0].ToString();
                if (expense == "")
                {
                    expense = "0.00";
                }
                Expense_lbl.Text = Convert.ToDecimal(expense).ToString("0.00");

            }
            //Stcok Received
            /*    string qry13 = "select sum(total) from StockEntry_tbl where date = '" + Convert.ToDateTime(DateTime.Now).ToString("yyyy/MM/dd") + "' ";
                SqlCommand cmd13 = new SqlCommand(qry13, Database.con);
                SqlDataReader rdr13 = cmd13.ExecuteReader();
                if (rdr13.Read())
                {
                    string stockreceived = rdr13[0].ToString();
                    if (stockreceived == "")
                    {
                        stockreceived = "0.00";
                    }
                    TodayStock_lbl.Text = Convert.ToDecimal(stockreceived).ToString("0.00");

                }*/
            string paddycredit = "";
            string qry215 = "select sum(paddycredit) from  creditdebit_tbl where mode='StockReceived'";
            SqlCommand cmd215 = new SqlCommand(qry215, Database.con);
            SqlDataReader rdr215 = cmd215.ExecuteReader();
            if (rdr215.Read())
            {
                paddycredit = rdr215[0].ToString();
                TodayStock_lbl.Text = paddycredit;
            }
            else
            {
                TodayStock_lbl.Text = "0.00";
            }


            string paddydebit = "";
            string qry216 = "select sum(paddydebit) from  creditdebit_tbl where mode='CreateProduction'";
            SqlCommand cmd216 = new SqlCommand(qry216, Database.con);
            SqlDataReader rdr216 = cmd216.ExecuteReader();
            if (rdr216.Read())
            {
                paddydebit = rdr216[0].ToString();
                Totalproduction_lbl.Text = paddydebit.ToString();

            }
            else
            {
                Totalproduction_lbl.Text = "0.00";
            }

            // Delivery weight
            string ricedelivery = "";
            string qry210 = "select sum(ricedebit) from  creditdebit_tbl where mode='Delivery'";
            SqlCommand cmd210 = new SqlCommand(qry210, Database.con);
            SqlDataReader rdr210 = cmd210.ExecuteReader();
            if (rdr210.Read())
            {
                ricedelivery = rdr210[0].ToString();
                Delivery_lbl.Text = ricedelivery;

            }
            else
            {
                Delivery_lbl.Text = "0.00";
            }

            string riceoutput = "";
            string qry212 = "select sum(ricecredit) from  creditdebit_tbl where mode='RiceOutput'";
            SqlCommand cmd212 = new SqlCommand(qry212, Database.con);
            SqlDataReader rdr212 = cmd212.ExecuteReader();
            if (rdr212.Read())
            {
                riceoutput = rdr212[0].ToString();
                Riceoutput_lbl.Text = riceoutput;
                
            }
            else
            {
                Riceoutput_lbl.Text = "0.00";
            }
            //Production Status
            string qry2 = "select * from ProductionEntry_tbl";
            SqlCommand cmd2 = new SqlCommand(qry2, Database.con);
            SqlDataReader rdr2 = cmd2.ExecuteReader();
            while (rdr2.Read())
            {
                string stockfor = rdr2["stockfor"].ToString();
                string productionid = rdr2["productionid"].ToString();
                string status = rdr2["status"].ToString();
           
                    int n = dataGridView1.Rows.Add();
                    dataGridView1.Rows[n].Cells[0].Value = productionid;
                    dataGridView1.Rows[n].Cells[1].Value = stockfor;
                    dataGridView1.Rows[n].Cells[2].Value = status;
                 
            }

        }
        private void InfoPic_pic_Click(object sender, EventArgs e)
        {

            Setting_pnl.Visible = false;
            Notifications_pnl.Visible = false;
            Help_pnl.Visible = false;
            if (InfoPanel_pnl.Visible == false)
            {
                InfoPanel_pnl.Visible = true;
            }
            else
            {
                InfoPanel_pnl.Visible = false;
            }
        }

        private void Setting_pic_Click(object sender, EventArgs e)
        {
            InfoPanel_pnl.Visible = false;
            Notifications_pnl.Visible = false;
            Help_pnl.Visible = false;
            if (Setting_pnl.Visible == false)
            {
                Setting_pnl.Visible = true;
            }
            else
            {
                Setting_pnl.Visible = false;
            }
        }

        private void help_pic_Click(object sender, EventArgs e)
        {
            InfoPanel_pnl.Visible = false;
            Setting_pnl.Visible = false;
            Notifications_pnl.Visible = false;
            if (Help_pnl.Visible == false)
            {
                Help_pnl.Visible = true;
            }
            else
            {
                Help_pnl.Visible = false;
            }
        }

        private void Notification_pic_Click(object sender, EventArgs e)
        {
            InfoPanel_pnl.Visible = false;
            Setting_pnl.Visible = false;
            Help_pnl.Visible = false;

            if (Notifications_pnl.Visible == false)
            {
                Notifications_pnl.Visible = true;
            }
            else
            {
                Notifications_pnl.Visible = false;
            }
        }

        private void Refresh_pic_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;

          /*  ExpiryUpdate();
            Trail();
            MemberExpiry();
            memberbalance();
          

            Reset();
            Open();
         */

            InfoPanel_pnl.Visible = false;
            Help_pnl.Visible = false;
            Notifications_pnl.Visible = false;
            Setting_pnl.Visible = false;
        }
        void FormClose()
        {
            /* string system = Environment.MachineName.ToString();
             if (system == "DESKTOP-1RQ99DD")
             {
                 string backqry = "Backup database FFSGym to disk='D:/BUYP/Software/Dropbox/Databack/" + DateTime.Now.ToString("dd -MM-yyyy HH-mm") + ".bak'";
                 SqlCommand cmd = new SqlCommand(backqry, Database.con);
                 cmd.CommandTimeout = 200;
                 cmd.ExecuteNonQuery();

                 delete();
             }*/
            for (int i = Application.OpenForms.Count - 1; i >= 0; i--)
            {
                if (Application.OpenForms[i].Name != "MainForm")
                    Application.OpenForms[i].Close();

            }

        }
        private void Yesclose_btn_Click(object sender, EventArgs e)
        {
            FormClose();
        }

        private void Noclose_btn_Click(object sender, EventArgs e)
        {
            Closing_pnl.Visible = false;
        }


        private struct RGBColors
        {
            public static Color color1 = Color.FromArgb(0, 151, 220);
            public static Color color2 = Color.FromArgb(0, 151, 220);
            public static Color color3 = Color.FromArgb(153, 138, 114);
            public static Color color4 = Color.FromArgb(95, 77, 221);
            public static Color color5 = Color.FromArgb(249, 88, 155);
            public static Color color6 = Color.FromArgb(24, 161, 251);
        }

        // Methods

        private void ActivateButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DisableButton();



                // button
                currentBtn = (IconButton)senderBtn;
                currentBtn.BackColor = Color.FromArgb(0, 120, 175);
                // currentBtn.BackColor = Color.FromArgb(37, 36, 81);
                // currentBtn.ForeColor = color;
                /*   currentBtn.TextAlign = ContentAlignment.MiddleCenter;
                   // currentBtn.IconColor = color;
                   currentBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                   currentBtn.ImageAlign = ContentAlignment.MiddleRight;
                */
                //left border button

                //  leftBorderBtn.BackColor = color;
                /*   leftBorderBtn.Location = new Point(0, currentBtn.Location.Y);
                   leftBorderBtn.Visible = true;
                   leftBorderBtn.BringToFront();
                */
                // Icon Current Child Form

                iconCurrentChildForm.IconChar = currentBtn.IconChar;
                //  iconCurrentChildForm.IconColor = color;

            }

        }
        private void DisableButton()
        {
            if (currentBtn != null)
            {
                Setting_pnl.Visible = false;
                Notifications_pnl.Visible = false;
                InfoPanel_pnl.Visible = false;
                Help_pnl.Visible = false;

                // currentBtn.BackColor = Color.FromArgb(253,176,255);

                currentBtn.BackColor = Color.FromArgb(0, 151, 220);
                iconCurrentChildForm.IconChar = IconChar.Home;
                // currentBtn.ForeColor = Color.Gainsboro;
                /*   currentBtn.TextAlign = ContentAlignment.MiddleLeft;
                   //  currentBtn.IconColor = Color.Gainsboro;
                   currentBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                   currentBtn.ImageAlign = ContentAlignment.MiddleLeft;
                */
            }

        }

        private void ActivateSubButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DisableSubButton();

                // DisableSubSubButton();
                // hidesubsubmenu();

                // button

                currentSubBtn = (IconButton)senderBtn;
                //  currentSubBtn.ForeColor = Color.FromArgb(255, 194, 15);
                //  currentSubBtn.IconColor = Color.FromArgb(255, 194, 15);
                // currentSubBtn.BackColor = Color.FromArgb(0, 120, 175);
                // currentBtn.ForeColor = color;
                currentSubBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentSubBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentSubBtn.ImageAlign = ContentAlignment.MiddleRight;

                //  Icon Current Child Form

                iconCurrentChildForm.IconChar = currentBtn.IconChar;
                // iconCurrentChildForm.IconColor = color;

                //    Search_txt.Text = currentBtn.Text +" / "+ currentSubBtn.Text;
                // iconCurrentChildForm.IconColor = color;

            }
        }

        private void DisableSubButton()
        {
            if (currentSubBtn != null)
            {
                Setting_pnl.Visible = false;
                Notifications_pnl.Visible = false;
                InfoPanel_pnl.Visible = false;
                Help_pnl.Visible = false;

                //  hidesubsubmenu();
                //  DisableSubSubButtonSpare();



                currentSubBtn.ForeColor = Color.White;
                currentSubBtn.IconColor = Color.White;
                //  currentSubBtn.BackColor = Color.FromArgb(0, 151, 220);
                currentSubBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentSubBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentSubBtn.ImageAlign = ContentAlignment.MiddleLeft;


                //  Icon Current Child Form

                iconCurrentChildForm.IconChar = IconChar.Home;
                // iconCurrentChildForm.IconColor = color;

                // Search_txt.Text = "Home";
                // iconCurrentChildForm.IconColor = color;

            }

        }

        private void ActivateSubSubButton(object senderBtn, Color color)
        {
            if (senderBtn != null)
            {
                DisableSubSubButton();
                //    allbutton();

                currentSubSubBtn = (IconButton)senderBtn;

                currentSubSubBtn.TextAlign = ContentAlignment.MiddleCenter;
                currentSubSubBtn.IconChar = FontAwesome.Sharp.IconChar.AngleDown;
                // currentBtn.IconColor = color;
                currentSubSubBtn.TextImageRelation = TextImageRelation.TextBeforeImage;
                currentSubSubBtn.ImageAlign = ContentAlignment.MiddleRight;

                //  Icon Current Child Form

                iconCurrentChildForm.IconChar = currentBtn.IconChar;
                // iconCurrentChildForm.IconColor = color;

                //   Search_txt.Text = currentBtn.Text + " / " + currentSubSubBtn.Text;
                // iconCurrentChildForm.IconColor = color;
            }
            else
            {
                DisableSubSubButton();
            }



        }
        private void DisableSubSubButton()
        {
            if (currentSubSubBtn != null)
            {
                DisableSubButton();

                currentSubSubBtn.TextAlign = ContentAlignment.MiddleLeft;
                currentSubSubBtn.IconChar = FontAwesome.Sharp.IconChar.Sliders;
                // currentBtn.IconColor = color;
                currentSubSubBtn.TextImageRelation = TextImageRelation.ImageBeforeText;
                currentSubSubBtn.ImageAlign = ContentAlignment.MiddleLeft;

                //  Icon Current Child Form

                iconCurrentChildForm.IconChar = currentBtn.IconChar;
                // iconCurrentChildForm.IconColor = color;

                //    Search_txt.Text = currentBtn.Text;
                // iconCurrentChildForm.IconColor = color;

            }

        }

        private void hidesubmenu()
        {

            if (FileMenu.Visible == true)
                FileMenu.Visible = false;
            if (StockMenu.Visible == true)
                StockMenu.Visible = false;
            if (SettingMenu.Visible == true)
                SettingMenu.Visible = false;
            if (PurchaseMenu.Visible == true)
                PurchaseMenu.Visible = false;

            if (SalesMenu.Visible == true)
                SalesMenu.Visible = false;
            if (DaysheetMenu.Visible == true)
                DaysheetMenu.Visible = false;
            if (ReportMenu.Visible == true)
                ReportMenu.Visible = false;
            if (SupplierMenu.Visible == true)
                SupplierMenu.Visible = false;
            if (ProductionMenu.Visible == true)
                ProductionMenu.Visible = false;
            if (DeliveryMenu.Visible == true)
                DeliveryMenu.Visible = false;
            if (ReportMenu.Visible == true)
                ReportMenu.Visible = false;

            /*   if (HealthReporMenu.Visible == true)
                   HealthReporMenu.Visible = false;*/

            if (Packing_Menu.Visible == true)
            {
                Packing_Menu.Visible = false;
            }
        }

        private void showsubmenu(Panel submenu)
        {
            if (submenu.Visible == false)
            {
                hidesubmenu();
                submenu.Visible = true;
            }
            else
            {

                submenu.Visible = false;
            }
        }

        private void hidesubsubmenu()
        {


            if (SubSuppliersettingsubmenu.Visible == true)
                SubSuppliersettingsubmenu.Visible = false;


            if (SubStocksettingsubmenu.Visible == true)
                SubStocksettingsubmenu.Visible = false;
            if (SubMembersettingsubmenu.Visible == true)
                SubMembersettingsubmenu.Visible = false;

            if (SubPlansettingsubmenu.Visible == true)
                SubPlansettingsubmenu.Visible = false;

            if (StockSubMenu.Visible == true)
                StockSubMenu.Visible = false;

            if (ProductionSubMenu.Visible == true)
                ProductionSubMenu.Visible = false;
            if (PackingSubMenu.Visible == true)
                PackingSubMenu.Visible = false;

            if (Deliverysubmenu.Visible == true)
                Deliverysubmenu.Visible = false;

            if (Othersssubmenu.Visible == true)
                Othersssubmenu.Visible = false;

            if (Daysheetsubmenu.Visible == true)
                Daysheetsubmenu.Visible = false;

        }

        private void showsubsubmenu(Panel subsubmenu)
        {
            if (subsubmenu.Visible == false)
            {

                hidesubsubmenu();
                subsubmenu.Visible = true;
            }
            else
            {
                subsubmenu.Visible = false;

            }
        }

        private Form activeForm = null;
        private void openChildForm(Form childForm)
        {
            if (activeForm != null)

                activeForm.Close();
            activeForm = childForm;
            childForm.TopLevel = false;
            childForm.FormBorderStyle = FormBorderStyle.None;
            childForm.Dock = DockStyle.Fill;
            Desktop.Controls.Add(childForm);
            Desktop.Tag = childForm;
            childForm.BringToFront();
            childForm.Show();
            //   lblTitleChildForm.Text = childForm.Text;
            //    Search_txt.Text = childForm.Text;

        }

        private void Reset()
        {
            Setting_pnl.Visible = false;
            Notifications_pnl.Visible = false;
            InfoPanel_pnl.Visible = false;
            Help_pnl.Visible = false;

            DisableButton();
            hidesubmenu();
            leftBorderBtn.Visible = false;

            Search_txt.Text = "Home";

            if (activeForm != null)
                activeForm.Close();

         
        }


        private void allbutton()
        {
            if (activeForm != null)
                activeForm.Close();
        }


        int File = 0;
        int setting = 0;
        int Supplier = 0;
        int Stocks = 0;
        int Production = 0;
        int Packing = 0;
        int DaySheet = 0;
        int purchase = 0;
        int Sales = 0;
        int others = 0;
      
     
        int Delivery = 0;
        int email = 0;
        int Setting = 0;
        int Report = 0;
       
        int Graphs = 0;
        int health = 0;
        int consultant = 0;
        int Guest = 0;
        int Training = 0;
        void Open()
        {
            File = 0;
            Stocks = 0;
            Production = 0;
            purchase = 0;
            Sales = 0;
            others = 0;
            setting = 0;
            Packing = 0;
            Delivery = 0;
            email = 0;
            Setting = 0;
            Report = 0;
            DaySheet = 0;
            Graphs = 0;
            Supplier = 0;
            health = 0;
            consultant = 0;
            Guest = 0;
            Training = 0;
        }
        private void Setting_btn_Click(object sender, EventArgs e)
        {
            setting += 1;

            if (setting == 2)
            {
                Reset();
                Open();

            }
            else
            {

                allbutton();
                SettingMenu.AutoSize = true;
                showsubmenu(SettingMenu);

                ActivateButton(sender, RGBColors.color1);
                Search_txt.Text = "Setting";
            }
        }

        private void EmployeeSetting_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Setting.EmployeeSetting(login));
        }

        private void Password_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Setting.PasswordSetting(login));
        }

        private void Daysheet_btn_Click(object sender, EventArgs e)
        {
            DaySheet += 1;

            if (DaySheet == 2)
            {
                Reset();
                Open();

            }
            else
            {
                allbutton();
                DaysheetMenu.AutoSize = true;
                showsubmenu(DaysheetMenu);

                ActivateButton(sender, RGBColors.color1);
                Search_txt.Text = "DaySheet";
            }
        }

        private void IncomeEntry_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new DaySheet.IncomeEntry(login));
        }

        private void ExpenseEntry_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new DaySheet.ExpenseEntry(login));
        }

        private void Dayreport_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new DaySheet.DaysheetReport(login));
        }

        private void Daysheetsetting_Click(object sender, EventArgs e)
        {
            SettingMenu.AutoSize = true;
            Daysheetsubmenu.AutoSize = true;
            showsubsubmenu(Daysheetsubmenu);
        }

        private void PrinterDetails_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Setting.IncomeSetting(login));
        }

        private void ExpenseCategory_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Setting.ExpenseSetting(login));
        }

        private void Supplier_btn_Click(object sender, EventArgs e)
        {
            Supplier += 1;

            if (Supplier == 2)
            {
                Reset();
                Open();

            }
            else
            {
                allbutton();
                SupplierMenu.AutoSize = true;
                showsubmenu(SupplierMenu);

                ActivateButton(sender, RGBColors.color1);
                Search_txt.Text = "Supplier";
            }
        }

        private void iconButton3_Click(object sender, EventArgs e)
        {
            SettingMenu.AutoSize = true;
            SubSuppliersettingsubmenu.AutoSize = true;
            showsubsubmenu(SubSuppliersettingsubmenu);
        }

        private void Statesetting_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Setting.StateSetting(login));
        }

        private void PaddySupplier_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Supplier.PaddySupplier(login));
        }

        private void PaymentType_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Setting.Payment(login));
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Supplier.PaddyReceiver(login));
        }

        private void iconButton8_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Setting.Workwages(login));
        }

        private void StockEntry_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Stock.StockReceivedEntry(login));
        }

        private void Stock_btn_Click(object sender, EventArgs e)
        {
            Stocks += 1;

            if (Stocks == 2)
            {
                Reset();
                Open();

            }
            else
            {
                allbutton();
                StockMenu.AutoSize = true;
                showsubmenu(StockMenu);

                ActivateButton(sender, RGBColors.color1);
                Search_txt.Text = "Stock";
            }
        }

        private void Production_btn_Click(object sender, EventArgs e)
        {
            Production += 1;

            if (Production == 2)
            {
                Reset();
                Open();

            }
            else
            {
                allbutton();
                ProductionMenu.AutoSize = true;
                showsubmenu(ProductionMenu);

                ActivateButton(sender, RGBColors.color1);
                Search_txt.Text = "Production";
            }
        }

        private void OrderSalesEntry_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Production.CreateProduction(login));
        }

        private void Grade_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Stock.GradeSetting(login));
        }

        private void SupplierSettingMenu_Click(object sender, EventArgs e)
        {
            SettingMenu.AutoSize = true;
            SubStocksettingsubmenu.AutoSize = true;
            showsubsubmenu(SubStocksettingsubmenu);
        }

        private void CreateProductionEntry_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Production.CreateProduction(login));
        }

        private void Minimize_btn_Click(object sender, EventArgs e)
        {
            try
            {

                Setting_pnl.Visible = false;

                this.WindowState = FormWindowState.Minimized;

                if (this.WindowState == FormWindowState.Minimized)
                {
                    foreach (Form frm in Application.OpenForms)
                    {
                        frm.WindowState = FormWindowState.Minimized;
                    }
                }
                else if (this.WindowState == FormWindowState.Normal)
                {
                    foreach (Form frm in Application.OpenForms)
                    {
                        frm.WindowState = FormWindowState.Normal;
                    }

                }

            }
            catch
            {

            }
        }

        private void LogOut_btn_Click(object sender, EventArgs e)
        {
            Setting_pnl.Visible = false;
            Closing_pnl.Visible = true;
            Yesclose_btn.Focus();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Date_lbl.Text = DateTime.Now.ToString("dd MMMM yyyy");
            Time_lbl.Text = DateTime.Now.ToString("hh:mm:ss tt");
        }

        private void Packing_btn_Click(object sender, EventArgs e)
        {
            Packing += 1;

            if (Packing == 2)
            {
                Reset();
                Open();

            }
            else
            {
                allbutton();
                Packing_Menu.AutoSize = true;
                showsubmenu(Packing_Menu);

                ActivateButton(sender, RGBColors.color1);
                Search_txt.Text = "Packing & Loading";
            }
        }

        private void Report_btn_Click(object sender, EventArgs e)
        {
            Report += 1;

            if (Report == 2)
            {
                Reset();
                Open();

            }
            else
            {
                allbutton();
                ReportMenu.AutoSize = true;
                showsubmenu(ReportMenu);

                ActivateButton(sender, RGBColors.color1);
                Search_txt.Text = "Report";
            }
        }

        private void StockReceived_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Report.StockReceived(login));
        }

        private void ProductionDetailReport_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Report.ProductionReport(login));
        }

        private void StockReportMenu_Click(object sender, EventArgs e)
        {
            ReportMenu.AutoSize = true;
            StockSubMenu.AutoSize = true;
            showsubsubmenu(StockSubMenu);
        }

        private void ProductionReportMenu_btn_Click(object sender, EventArgs e)
        {
            ReportMenu.AutoSize = true;
            ProductionSubMenu.AutoSize = true;
            showsubsubmenu(ProductionSubMenu);
        }

        private void Riceoutput_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Packing.RiceOutPutStatus(login));
        }

        private void Productionstatus_btn_Click(object sender, EventArgs e)
        {

            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Production.ProductionStatus(login));
        }

        private void FactoryStatus_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Production.FactoryStatusEntry(login));
        }

        private void Packing_btn4_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Packing.PackingEntry(login));
        }

        private void Delivery_btn_Click(object sender, EventArgs e)
        {
            Delivery += 1;

            if (Delivery == 2)
            {
                Reset();
                Open();

            }
            else
            {
                allbutton();
                DeliveryMenu.AutoSize = true;
                showsubmenu(DeliveryMenu);

                ActivateButton(sender, RGBColors.color1);
                Search_txt.Text = "Delivery";
            }
        }

        private void iconButton1_Click(object sender, EventArgs e)
        {
            purchase += 1;

            if (purchase == 2)
            {
                Reset();
                Open();

            }
            else
            {

                allbutton();
                PurchaseMenu.AutoSize = true;
                showsubmenu(PurchaseMenu);

                ActivateButton(sender, RGBColors.color1);
                Search_txt.Text = "Purchase";

            }
        }

        private void AddDelivery_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Delivery.RiceDelivery(login));
        }

        private void PackingReport_btn_Click(object sender, EventArgs e)
        {
            ReportMenu.AutoSize = true;
            PackingSubMenu.AutoSize = true;
            showsubsubmenu(PackingSubMenu);
        }

        private void PackingDetails_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Report.PackingReport(login));
        }

        private void RiceOutputReport_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Report.RiceOutput(login));
        }

        private void DeliveryReport_btn_Click(object sender, EventArgs e)
        {
            ReportMenu.AutoSize = true;
            Deliverysubmenu.AutoSize = true;
            showsubsubmenu(Deliverysubmenu);
            
        }

        private void DeliveryDetails_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Report.DeliveryReport(login));
        }

        private void guna2GradientButton7_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Purchase.NewPurchase(login));
        }

        private void PaddySelling_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Purchase.PaymentReport(login));
        }

        private void RiceSupplier_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Supplier.RiceSupplier(login));
        }

        private void PurchaseReport_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Report.PurchaseReport(login));

        }

        private void PurchaserBalReport_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Report.SellingReport(login));
        }

        private void NewReceipt_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Delivery.DeliveryReceipt(login));
            
        }

        private void SalesReportMenu_Click(object sender, EventArgs e)
        {
            /* others += 1;

             if (others == 2)
             {
                 Reset();
                 Open();

             }
             else
             {
                 allbutton();
                 Othersssubmenu.AutoSize = true;
                 showsubmenu(Othersssubmenu);

                 ActivateButton(sender, RGBColors.color1);
                 Search_txt.Text = "Others";

             }*/

            ReportMenu.AutoSize = true;
            Othersssubmenu.AutoSize = true;
            showsubsubmenu(Othersssubmenu);
        }

        private void CreditDebitReport_btn_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Report.CreditDebit(login));
        }

        private void Overallstock_tbl_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            ActivateSubButton(sender, RGBColors.color1);
            openChildForm(new Report.OverallStock(login));
        }

        private void iconButton5_Click(object sender, EventArgs e)
        {

        }

        private void iconButton6_Click(object sender, EventArgs e)
        {

        }

        private void guna2GradientButton4_Click(object sender, EventArgs e)
        {

        }
    }
}
