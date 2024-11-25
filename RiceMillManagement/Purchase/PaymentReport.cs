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
using System.Net.NetworkInformation;

namespace RiceMillManagement.Purchase
{
    public partial class PaymentReport : Form
    {
        private string login;
        public PaymentReport(string login)
        {
            InitializeComponent();Database dbs=new Database();
            dbs.db();
            this.login = login;
            Officeinfo();
            number();
            Grade();
            paddysupplier();
            grid();
            paddystock();
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
            SNo_txt.Text = "";
            string opnoqry = "select top 1 serialno from PaddySellingSerialNo_tbl order by serialno desc";
            SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
            SqlDataReader opnordr = opnocmd.ExecuteReader();
            if (opnordr.Read())
            {
                string no = opnordr["serialno"].ToString();
                int opno1 = Convert.ToInt32(no);

                serial = opno1 + 1;

                SNo_txt.Text = serial.ToString();

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
        void paddysupplier()
        {
            try
            {
                From_combo.Items.Clear();
                string qry = "select * from PaddyReceiver_tbl";
                SqlCommand cmd = new SqlCommand(qry, Database.con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    From_combo.Items.Add(rdr["name"]);
                    
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
        void paddystock()
        {
            string paddycredit = "";
            string qry215 = "select sum(paddycredit) from  creditdebit_tbl where mode='StockReceived'";
            SqlCommand cmd215 = new SqlCommand(qry215, Database.con);
            SqlDataReader rdr215 = cmd215.ExecuteReader();
            if (rdr215.Read())
            {
                paddycredit = rdr215[0].ToString();
               
            }
            
            string paddydebit = "";
            string qry216 = "select sum(paddydebit) from  creditdebit_tbl";
            SqlCommand cmd216 = new SqlCommand(qry216, Database.con);
            SqlDataReader rdr216 = cmd216.ExecuteReader();
            if (rdr216.Read())
            {
                paddydebit = rdr216[0].ToString();

            }

            if(paddycredit=="")
            {
                paddycredit = "0";
            }
            if(paddydebit=="")
            {
                paddydebit = "0";
            }
            decimal finalpaddy = Convert.ToDecimal(paddycredit) - Convert.ToDecimal(paddydebit);

            string qry217 = "update Overallstock_tbl set paddy=" + finalpaddy + "";
            SqlCommand cmd217 = new SqlCommand(qry217, Database.con);
            cmd217.ExecuteNonQuery();

            //Paddy stock

            string qry213 = "select * from  Overallstock_tbl";
            SqlCommand cmd213 = new SqlCommand(qry213, Database.con);
            SqlDataReader rdr213 = cmd213.ExecuteReader();
            if (rdr213.Read())
            {
                string paddy = rdr213["paddy"].ToString();
             
                Weight_lbl.Text = paddy;
             
            }
        }
        void grid()
        {
            string qry = "select id as Id,sno as SNo,stockfor as StockFor,grade as Grade,name as Rice,weight as Weight,date as Date,LTRIM(RIGHT(convert(varchar,time,100),7)) as Time from PaddySellingEntry_tbl order by id desc";
            SqlDataAdapter adb = new SqlDataAdapter(qry, Database.con);
            DataTable dt = new DataTable();
            adb.Fill(dt);

            dataGridView1.DataSource = dt;
            dataGridView1.Columns[6].DefaultCellStyle.Format = "dd/MM/yyyy";
            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[1].Width = 40;
            dataGridView1.Columns[2].Width = 100;
            int count = dataGridView1.Rows.Count;
            Total_txt.Text = count.ToString();
        }
        int opno2;
        private void Add_btn_Click(object sender, EventArgs e)
        {
            //  try
            //   {
            if (SNo_txt.Text == "" || Grade_combo.Text == "" || Weight_txt.Text == "" || From_combo.Text == "")
            {
                MessageBox.Show("Kindly Fill Primary Information !!!", messagename, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                string qry1 = "select * from PaddySellingEntry_tbl where  sno=" + SNo_txt.Text + "";
                SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                SqlDataReader rdr1 = cmd1.ExecuteReader();
                if (rdr1.Read())
                {
                    MessageBox.Show("This Purchase Id Already Inserted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string memberid = "";
                    string opnoqry = "select top 1 serialno from PaddySellingSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
                    SqlDataReader opnordr = opnocmd.ExecuteReader();
                    if (opnordr.Read())
                    {
                        string no = opnordr["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;
                        memberid = serial.ToString();
                    }
                    
                    string qry211 = "insert into PaddySellingEntry_tbl values (" + memberid + ",N'" + From_combo.Text + "',N'" + Grade_combo.Text + "','" + Name_txt.Text + "',"+Rate_txt.Text+"," + Weight_txt.Text + ","+Total_txt.Text+",'" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "','Process')";
                    SqlCommand cmd211 = new SqlCommand(qry211, Database.con);
                    cmd211.ExecuteNonQuery();

                    string qry212 = "insert into CreditDebit_tbl values ('" + memberid + "','PaddySelling','Paddy',0," + Weight_txt.Text + ",0,0,N'" + From_combo.Text + "','" + Name_txt.Text + "','" + Grade_combo.Text + "','" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "')";
                    SqlCommand cmd212 = new SqlCommand(qry212, Database.con);
                    cmd212.ExecuteNonQuery();

                    string qry213 = "select * from  Overallstock_tbl";
                    SqlCommand cmd213 = new SqlCommand(qry213, Database.con);
                    SqlDataReader rdr213 = cmd213.ExecuteReader();
                    if (rdr213.Read())
                    {
                        string paddy = rdr213["paddy"].ToString();
                        decimal finalpaddy = Convert.ToDecimal(paddy) - Convert.ToDecimal(Weight_txt.Text);
                        
                        string qry214 = "update Overallstock_tbl set paddy=" + finalpaddy + "";
                        SqlCommand cmd214 = new SqlCommand(qry214, Database.con);
                        cmd214.ExecuteNonQuery();

                    }

                    string qry2 = "insert into PaddySellingSerialNo_tbl values(" + opno2 + ")";
                    SqlCommand cmd2 = new SqlCommand(qry2, Database.con);
                    cmd2.ExecuteNonQuery();

                    string opnoqry1 = "select top 1 serialno from IncomeSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd1 = new SqlCommand(opnoqry1, Database.con);
                    SqlDataReader opnordr1 = opnocmd1.ExecuteReader();
                    if (opnordr1.Read())
                    {
                        string no = opnordr1["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;

                    }
                    
                    string get1 = memberid + " - " + SNo_txt.Text;

                       string qry31 = "insert into Income_tbl values(" + opno2 + ",'PaddySelling','" + DateTime.Today + "',N'" + get1 + "'," + Totalamount_txt.Text + ")";
                       SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                       cmd31.ExecuteNonQuery();

                    string qry21 = "insert into IncomeSerialNo_tbl values(" + opno2 + ")";
                    SqlCommand cmd21 = new SqlCommand(qry21, Database.con);
                    cmd21.ExecuteNonQuery();


                    string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','PaddySelling','SellingId:" + SNo_txt.Text + "-"+From_combo.Text+"_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                    cmd3.ExecuteNonQuery();



                    MessageBox.Show("Purchase Id " + SNo_txt.Text + " Saved Successfully...", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    number();
                    grid();


                    Weight_txt.Text = "";
                    From_combo.SelectedIndex = -1;
                    //  comboBox1.SelectedIndex = -1;
                    Grade_combo.SelectedIndex = -1;
                    Name_txt.Text = "";
                    Rate_txt.Text = "";
                    Totalamount_txt.Text = "";
                    From_combo.Focus();

                }
            }
            /*  }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message,messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);

              }*/
        }

        private void Close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Clear_btn_Click(object sender, EventArgs e)
        {
            Weight_txt.Text = "";
            From_combo.SelectedIndex = -1;
            //  comboBox1.SelectedIndex = -1;
            Grade_combo.SelectedIndex = -1;
            Name_txt.Text = "";
            Rate_txt.Text = "";
            Totalamount_txt.Text = "";
            From_combo.Focus();
        }

        private void Weight_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                if(Rate_txt.Text=="")
                {
                    Rate_txt.Text = "0";
                }
                if(Weight_txt.Text=="")
                {
                    Weight_txt.Text = "0";
                }

                decimal total=Convert.ToDecimal(Weight_txt.Text)*Convert.ToDecimal(Rate_txt.Text);
                Totalamount_txt.Text = total.ToString();
                
            }
        }

        private void From_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string qry = "select * from PaddyReceiver_tbl where name='"+From_combo.Text+"'";
            SqlCommand cmd = new SqlCommand(qry, Database.con);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                Name_txt.Text=rdr["cusname"].ToString();

            }
        }
    }
}
