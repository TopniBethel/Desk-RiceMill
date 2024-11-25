using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Windows.Markup;
using static System.Net.Mime.MediaTypeNames;
using System.Xml.Linq;

namespace RiceMillManagement.Delivery
{
    public partial class RiceDelivery : Form
    {
        private string login;
        public RiceDelivery(string login)
        {
            InitializeComponent();Database dbs=new Database();
            dbs.db();
            this.login = login;
            Officeinfo();
            number();
            grid();
            paddyreceiver();
            Grade();
        }
        string messagename;
        void Officeinfo()
        {
            string qry = "select * from OfficeInfo_tbl ";
            SqlCommand cmd = new SqlCommand(qry, Database.con);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                messagename = rdr["shortname"].ToString();

            }

        }
        int serial;
        void number()
        {
            OrderNo_txt.Text = "";
            string opnoqry = "select top 1 serialno from DeliverySerialNo_tbl order by serialno desc";
            SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
            SqlDataReader opnordr = opnocmd.ExecuteReader();
            if (opnordr.Read())
            {
                string no = opnordr["serialno"].ToString();
                int opno1 = Convert.ToInt32(no);

                serial = opno1 + 1;

                OrderNo_txt.Text = serial.ToString();

            }
        }
        void paddyreceiver()
        {
            try
            {
                For_combo.Items.Clear();
                string qry = "select * from PaddyReceiver_tbl";
                SqlCommand cmd = new SqlCommand(qry, Database.con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    For_combo.Items.Add(rdr["name"]);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex);
            }
            finally
            {
                ///
            }
        }

        void Grade()
        {
            try
            {
                Grade_combo.Items.Clear();
                string qry = "select * from GradeSetting_tbl";
                SqlCommand cmd = new SqlCommand(qry, Database.con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Grade_combo.Items.Add(rdr["name"]);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex);
            }
            finally
            {
                ///
            }
        }
        private void Close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Gram_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {

                if (Ton_txt.Text == "")

                {
                    Ton_txt.Text = "0";
                }
                if (KiloGram_txt.Text == "")
                {
                    KiloGram_txt.Text = "0";
                }
                if (Gram_txt.Text == "")
                {
                    Gram_txt.Text = "0";
                }
                if (Totalweight_txt.Text == "")
                {
                    Totalweight_txt.Text = "0";
                }

                decimal totalweight = ((Convert.ToDecimal(Ton_txt.Text) * 1000) + Convert.ToDecimal(KiloGram_txt.Text));
                string weight = totalweight.ToString() + "." + Gram_txt.Text;
                Totalweight_txt.Text = weight.ToString();
            }
        }
        void grid()
        {
            string qry = "select id as Id,orderno as SNo,stockfor as StockFor,grade as Grade,bag as Bag,name as Name,total as Total,date as Date from DeliveryEntry_tbl  order by id desc";
            SqlDataAdapter adb = new SqlDataAdapter(qry, Database.con);
            DataTable dt = new DataTable();
            adb.Fill(dt);

            dataGridView1.DataSource = dt;
            dataGridView1.Columns[7].DefaultCellStyle.Format = "dd/MM/yyyy";
            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[1].Width = 40;
            dataGridView1.Columns[2].Width = 200;
            int count = dataGridView1.Rows.Count;
            Total_txt.Text = count.ToString();

          //  string qry1 = "select orderno as SNo,stockfor as Stockfor,productionid as Productionid,date as Date,LTRIM(RIGHT(convert(varchar,time,100),7)) as Time from ProductionEntry_tbl where status='ReadyforDelivery' order by id desc";
            string qry1 = "select sno as SNo,stockfor as Stockfor,productionid as Productionid,date as Date,LTRIM(RIGHT(convert(varchar,time,100),7)) as Time from packingentry_tbl where status='Process' order by id desc";
            SqlDataAdapter adb1 = new SqlDataAdapter(qry1, Database.con);
            DataTable dt1 = new DataTable();
            adb1.Fill(dt1);
            dataGridView2.DataSource = dt1;
            
        }
        int opno2;
        private void Add_btn_Click(object sender, EventArgs e)
        {
            //  try
            //   {
            if (OrderNo_txt.Text == "" || Name_txt.Text == "" || Totalweight_txt.Text == "" || Grade_combo.Text == "")
            {
                MessageBox.Show("Kindly Fill Primary Information !!!", messagename, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                string qry1 = "select * from DeliveryEntry_tbl where  orderno=" + OrderNo_txt.Text + "";
                SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                SqlDataReader rdr1 = cmd1.ExecuteReader();
                if (rdr1.Read())
                {
                    MessageBox.Show("This Delivery Already Inserted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    string memberid = "";
                    string opnoqry = "select top 1 serialno from DeliverySerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
                    SqlDataReader opnordr = opnocmd.ExecuteReader();
                    if (opnordr.Read())
                    {
                        string no = opnordr["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;
                        memberid = serial.ToString();

                    }

                    if (Ton_txt.Text == "")

                    {
                        Ton_txt.Text = "0";
                    }
                    if (KiloGram_txt.Text == "")
                    {
                        KiloGram_txt.Text = "0";
                    }
                    if (Gram_txt.Text == "")
                    {
                        Gram_txt.Text = "0";
                    }
                    if (Totalweight_txt.Text == "")
                    {
                        Totalweight_txt.Text = "0";
                    }

                    string qry211 = "insert into DeliveryEntry_tbl values (" + memberid + ",N'" + For_combo.Text + "','" + DriverName_txt.Text + "','" + Vehicle_txt.Text + "','"+ProductionId_txt.Text+"','" + Grade_combo.Text + "'," + Noofbag_txt.Text + ",N'" + Name_txt.Text + "'," + Ton_txt.Text + "," + KiloGram_txt.Text + "," + Gram_txt.Text + "," + Totalweight_txt.Text + ",'" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "','Process',"+Deliverycharge_txt.Text+","+Loadingcharge_txt.Text+")";
                    SqlCommand cmd211 = new SqlCommand(qry211, Database.con);
                    cmd211.ExecuteNonQuery();


                    string qry212 = "insert into ProductionStatus_tbl values ('" + memberid + "',N'" + ProductionId_txt.Text + "','Delivery'," + Totalweight_txt.Text + ","+Noofbag_txt.Text+",N'" + For_combo.Text + "','Closed','" + DateTime.Today.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "')";
                    SqlCommand cmd212 = new SqlCommand(qry212, Database.con);
                    cmd212.ExecuteNonQuery();

                    string qry213 = "update StockManagement_tbl set output=" + Totalweight_txt.Text + " where productionid='" + ProductionId_txt.Text + "'";
                    SqlCommand cmd213 = new SqlCommand(qry213, Database.con);
                    cmd213.ExecuteNonQuery();

                    string qry214 = "update ProductionEntry_tbl set status='Delivered' where productionid='" + ProductionId_txt.Text + "'";
                    SqlCommand cmd214 = new SqlCommand(qry214, Database.con);
                    cmd214.ExecuteNonQuery();

                    string qry216 = "update packingentry_tbl set status='Delivered' where productionid='" + ProductionId_txt.Text + "'";
                    SqlCommand cmd216 = new SqlCommand(qry216, Database.con);
                    cmd216.ExecuteNonQuery();

                    string qry215 = "select * from  Overallstock_tbl";
                    SqlCommand cmd215 = new SqlCommand(qry215, Database.con);
                    SqlDataReader rdr215 = cmd215.ExecuteReader();
                    if (rdr215.Read())
                    {
                        string rice = rdr215["totalrice"].ToString();
                        decimal finalpaddy = Convert.ToDecimal(rice) - Convert.ToDecimal(Totalweight_txt.Text);

                        string qry217 = "update Overallstock_tbl set totalrice=" + finalpaddy + "";
                        SqlCommand cmd217 = new SqlCommand(qry217, Database.con);
                        cmd217.ExecuteNonQuery();
                    }

                    string qry218 = "insert into CreditDebit_tbl values ('" + memberid + "','Delivery','Rice',0,0,0," + Totalweight_txt.Text + ",N'" + For_combo.Text + "','" + Name_txt.Text + "','" + Grade_combo.Text + "','" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "')";
                    SqlCommand cmd218= new SqlCommand(qry218, Database.con);
                    cmd218.ExecuteNonQuery();

                    string qry2 = "insert into DeliverySerialNo_tbl values(" + opno2 + ")";
                    SqlCommand cmd2 = new SqlCommand(qry2, Database.con);
                    cmd2.ExecuteNonQuery();

                   /* string opnoqry1 = "select top 1 serialno from IncomeSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd1 = new SqlCommand(opnoqry1, Database.con);
                    SqlDataReader opnordr1 = opnocmd1.ExecuteReader();
                    if (opnordr1.Read())
                    {
                        string no = opnordr1["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;

                    }

                    string get1 = memberid + " - " + Name_txt.Text;

                    string qry31 = "insert into Income_tbl values(" + opno2 + ",'Delivery','" + DateTime.Today + "',N'" + get1 + "'," + Totalweight_txt.Text + ")";
                    SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                    cmd31.ExecuteNonQuery();

                    string qry21 = "insert into IncomeSerialNo_tbl values(" + opno2 + ")";
                    SqlCommand cmd21 = new SqlCommand(qry21, Database.con);
                    cmd21.ExecuteNonQuery();
                   */
                   
                    
                    string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','Delivery','Delivery:" + Name_txt.Text + "-"+ProductionId_txt.Text+"_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                    cmd3.ExecuteNonQuery();

                 //   printPreviewDialog1.Document = printDocument1;
                 //   printPreviewDialog1.ShowDialog();

                    MessageBox.Show("Delivery Id " + OrderNo_txt.Text + " Saved Successfully...", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    number();
                    grid();


                    Name_txt.Text = "";
                  //  From_combo.SelectedIndex = -1;
                    For_combo.SelectedIndex = -1;
                    Grade_combo.SelectedIndex = -1;
                    DriverName_txt.Text = "";
                    Vehicle_txt.Text = "";
                    Noofbag_txt.Text = "";
                    Name_txt.Text = "";
                    Ton_txt.Text = "";
                    KiloGram_txt.Text = "";
                    Gram_txt.Text = "";
                    Totalweight_txt.Text = "";
                    ProductionId_txt.Text = "";
                    Deliverycharge_txt.Text = "";
                    Loadingcharge_txt.Text = "";
                    ProductionId_txt.Focus();

                }
            }
            /*  }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message,messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);

              }*/
        }
        string productionid = "";
        string orderid = "";
        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView2.Rows)
            {
                if (dr.Cells[0].Selected == true)
                {
                    productionid = dr.Cells[2].Value.ToString();
                    orderid = dr.Cells[0].Value.ToString();
                    ProductionId_txt.Text = productionid;
                    
                    string qry = "select * from PackingEntry_tbl where productionid='" + productionid + "'";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        Totalweight_txt.Text = rdr["weight"].ToString();
                        For_combo.Text = rdr["stockfor"].ToString();
                        Grade_combo.Text = rdr["grade"].ToString();
                        Noofbag_txt.Text = rdr["bag"].ToString();
                        Name_txt.Text = rdr["name"].ToString();
                     
                        decimal ton=Convert.ToDecimal(Totalweight_txt.Text)/1000;
                        Ton_txt.Text = ton.ToString("0");
                        
                        string stringStart = Totalweight_txt.Text.Substring(0, Totalweight_txt.Text.IndexOf("."));
                        string lastCharacters = stringStart.Substring(stringStart.Length - 3);
                       
                        KiloGram_txt.Text = lastCharacters.ToString();

                        string str = Totalweight_txt.Text;
                        str = str.Substring(str.LastIndexOf(".") + 1);
                        Gram_txt.Text=str;
                        
                    }
                }
            }
        }
    }
}
