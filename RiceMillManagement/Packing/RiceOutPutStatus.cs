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
using System.Drawing.Printing;
using System.Windows.Controls;

namespace RiceMillManagement.Packing
{
    public partial class RiceOutPutStatus : Form
    {
        private string login;
        public RiceOutPutStatus(string login)
        {
            InitializeComponent();Database dbs=new Database();
            dbs.db();
            this.login = login;
            Officeinfo();
            number();
            grid();
            paddysupplier();
        }
        void paddysupplier()
        {
            try
            {
                For_combo.Items.Clear();
                string qry = "select * from PaddySupplier_tbl";
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
            string opnoqry = "select top 1 serialno from RiceStatusSerialNo_tbl order by serialno desc";
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

        private void Close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        string grade = "";
        string name = "";
        private void Productionid_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                string qry1 = "select * from ProductionEntry_tbl where  productionid='" + Productionid_txt.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                SqlDataReader rdr1 = cmd1.ExecuteReader();
                if (rdr1.Read())
                {
                    For_combo.Text = rdr1["stockfor"].ToString();
                    grade = rdr1["grade"].ToString();
                    name = rdr1["name"].ToString();
                    Rice_txt.Focus();
                }
            }
        }
        void grid()
        {
            string qry = "select id as Id,sno as SNo,stockfor as StockFor,productionid as Production,grade as Grade,rice as Rice,blackrice as Black,brokenrice as Broken,husk as Husk,date as Date,LTRIM(RIGHT(convert(varchar,time,100),7)) as Time from RiceOutput_tbl order by id desc";
            SqlDataAdapter adb = new SqlDataAdapter(qry, Database.con);
            DataTable dt = new DataTable();
            adb.Fill(dt);

            dataGridView1.DataSource = dt;
            dataGridView1.Columns[9].DefaultCellStyle.Format = "dd/MM/yyyy";
            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[1].Width = 40;
            dataGridView1.Columns[2].Width = 100;
            int count = dataGridView1.Rows.Count;
            Total_txt.Text = count.ToString();


            //Process production

            string qry1 = "select orderno as SNo,stockfor as Stockfor,productionid as Productionid,date as Date,LTRIM(RIGHT(convert(varchar,time,100),7)) as Time from ProductionEntry_tbl where status='RiceOutput' order by id desc";
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
            if (SNo_txt.Text == "" || Productionid_txt.Text == "" || Rice_txt.Text == "" || Blackrice_txt.Text == "")
            {
                MessageBox.Show("Kindly Fill Primary Information !!!", messagename, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                string qry1 = "select * from RiceOutput_tbl where  productionid='" + Productionid_txt.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                SqlDataReader rdr1 = cmd1.ExecuteReader();
                if (rdr1.Read())
                {
                    MessageBox.Show("This Production Id Already Inserted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    string memberid = "";
                    string opnoqry = "select top 1 serialno from RiceStatusSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
                    SqlDataReader opnordr = opnocmd.ExecuteReader();
                    if (opnordr.Read())
                    {
                        string no = opnordr["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;
                        memberid = serial.ToString();
                    }




                    if (Rice_txt.Text == "")

                    {
                        Rice_txt.Text = "0";
                    }
                    if (Blackrice_txt.Text == "")
                    {
                        Blackrice_txt.Text = "0";
                    }
                    if (BrokenRice_txt.Text == "")
                    {
                        BrokenRice_txt.Text = "0";
                    }
                    if (Husk_txt.Text == "")
                    {
                        Husk_txt.Text = "0";
                    }
                    
                    string qry211 = "insert into RiceOutput_tbl values (" + memberid + ",N'" + Productionid_txt.Text + "',N'" + For_combo.Text + "','"+grade+"','"+name+"'," + Rice_txt.Text + "," + Blackrice_txt.Text + "," + BrokenRice_txt.Text + "," + Husk_txt.Text + ",'" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + DateTime.Today.ToString("yyyy-MM-dd hh:mm:ss tt") + "','Process')";
                    SqlCommand cmd211 = new SqlCommand(qry211, Database.con);
                    cmd211.ExecuteNonQuery();
                    
                    string qry212 = "insert into ProductionStatus_tbl values ('" + memberid + "',N'" + Productionid_txt.Text + "','RiceOutput'," + Rice_txt.Text + ",0,N'" + For_combo.Text+ "','Packing','" + DateTime.Today.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "')";
                    SqlCommand cmd212 = new SqlCommand(qry212, Database.con);
                    cmd212.ExecuteNonQuery();
                    
                    string qry213 = "update StockManagement_tbl set output=" + Rice_txt.Text + " where productionid='"+Productionid_txt.Text+"'";
                    SqlCommand cmd213 = new SqlCommand(qry213, Database.con);
                    cmd213.ExecuteNonQuery();
                    
                    string qry214 = "update ProductionEntry_tbl set status='Packing' where productionid='" + Productionid_txt.Text + "'";
                    SqlCommand cmd214 = new SqlCommand(qry214, Database.con);
                    cmd214.ExecuteNonQuery();

                    string qry217= "insert into CreditDebit_tbl values ('" + memberid + "','RiceOutput','Rice',0,0," + Totalweight_txt.Text + ",0,N'" + For_combo.Text + "','" + name + "','" + grade + "','" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "')";
                    SqlCommand cmd217= new SqlCommand(qry217, Database.con);
                    cmd217.ExecuteNonQuery();

                    string qry215 = "select * from  Overallstock_tbl";
                    SqlCommand cmd215 = new SqlCommand(qry215, Database.con);
                    SqlDataReader rdr215 = cmd215.ExecuteReader();
                    if (rdr215.Read())
                    {
                        string paddy = rdr215["rice"].ToString();
                        decimal finalpaddy = Convert.ToDecimal(paddy) + Convert.ToDecimal(Totalweight_txt.Text);

                        string qry216 = "update Overallstock_tbl set rice=" + finalpaddy + "";
                        SqlCommand cmd216 = new SqlCommand(qry216, Database.con);
                        cmd216.ExecuteNonQuery();

                    }

                    string qry2 = "insert into RiceStatusSerialNo_tbl values(" + opno2 + ")";
                    SqlCommand cmd2 = new SqlCommand(qry2, Database.con);
                    cmd2.ExecuteNonQuery();

                  /*  string opnoqry1 = "select top 1 serialno from IncomeSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd1 = new SqlCommand(opnoqry1, Database.con);
                    SqlDataReader opnordr1 = opnocmd1.ExecuteReader();
                    if (opnordr1.Read())
                    {
                        string no = opnordr1["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;

                    }
                    
                    string get1 = memberid + " - " + Productionid_txt.Text;
                    
                 string qry31 = "insert into Income_tbl values(" + opno2 + ",'RiceOutput','" + DateTime.Today + "',N'" + get1 + "'," + Totalweight_txt.Text + ")";
                    SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                    cmd31.ExecuteNonQuery();

                    string qry21 = "insert into IncomeSerialNo_tbl values(" + opno2 + ")";
                    SqlCommand cmd21 = new SqlCommand(qry21, Database.con);
                    cmd21.ExecuteNonQuery();*/
                 
                    string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','RiceOutput','ProductionId:" + Productionid_txt.Text + "-"+For_combo.Text+"_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                    cmd3.ExecuteNonQuery();

                    MessageBox.Show("Output Id " + SNo_txt.Text + " Saved Successfully...", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    number();
                    grid();

                    Productionid_txt.Text = "";
                    For_combo.SelectedIndex = -1;
                  //comboBox1.SelectedIndex = -1;
                  //Grade_combo.SelectedIndex = -1;
                    Rice_txt.Text = "";
                    Blackrice_txt.Text = "";
                    BrokenRice_txt.Text = "";
                    Husk_txt.Text = "";
                    Totalweight_txt.Text = "";

                    Productionid_txt.Focus();

                }
            }
            /*  }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message,messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);

              }*/
        }

        private void RiceOutPutStatus_Load(object sender, EventArgs e)
        {
            Productionid_txt.Focus();
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
                    Productionid_txt.Text = productionid;

                    string qry = "select * from ProductionEntry_tbl where productionid='" + productionid + "'";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {

                        Totalweight_txt.Text = rdr["total"].ToString();
                        For_combo.Text = rdr["stockfor"].ToString();
                        grade = rdr["grade"].ToString();
                        
                    }
                }
            }
        }

        private void Perc_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            { 
                decimal ricevalue=Convert.ToDecimal(Totalweight_txt.Text)*((Convert.ToDecimal(Perc_txt.Text)/100));
                Rice_txt.Text = ricevalue.ToString("0.000");
                Blackrice_txt.Focus();
            }
        }
    }
}
