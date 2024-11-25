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
using System.Xml.Linq;

namespace RiceMillManagement.Packing
{
    public partial class PackingEntry : Form
    {
        private string login;
        public PackingEntry(string login)
        {
            InitializeComponent();Database dbs= new Database();
            dbs.db();
            this.login = login;
            Officeinfo();
            number();
            grid();
            //   paddysupplier();
            overallstock();
            Packedby_combo.Text = "Packing Team";
        }
        void paddysupplier()
        {
            try
            {
                Packedby_combo.Items.Clear();
                string qry = "select * from PaddySupplier_tbl";
                SqlCommand cmd = new SqlCommand(qry, Database.con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Packedby_combo.Items.Add(rdr["name"]);

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
            string opnoqry = "select top 1 serialno from PackingSerialNo_tbl order by serialno desc";
            SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
            SqlDataReader opnordr = opnocmd.ExecuteReader();
            if (opnordr.Read())
            {
                string no = opnordr["serialno"].ToString();
                int opno1 = Convert.ToInt32(no);

                serial = opno1 + 1;

                SNo_txt.Text = serial.ToString();
                Productionid_txt.Text = serial.ToString();

            }
        }
        void overallstock()
        {
            string qry213 = "select * from  Overallstock_tbl";
            SqlCommand cmd213 = new SqlCommand(qry213, Database.con);
            SqlDataReader rdr213 = cmd213.ExecuteReader();
            if (rdr213.Read())
            {
                string rice = rdr213["totalrice"].ToString();
                Totalweight_txt.Text = rice;
                Weight_lbl.Text = rice;
              /*  decimal finalpaddy = Convert.ToDecimal(Totalweight_txt.Text) + Convert.ToDecimal(paddy);

                string qry214 = "update Overallstock_tbl set paddy=" + finalpaddy + "";
                SqlCommand cmd214 = new SqlCommand(qry214, Database.con);
                cmd214.ExecuteNonQuery();*/

            }
        }
        private void Close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        string grade = "";
        string name = "";
        string stockfor = "";
        private void Machine_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string qry1 = "select * from ProductionEntry_tbl where  productionid='" + Productionid_txt.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                SqlDataReader rdr1 = cmd1.ExecuteReader();
                if (rdr1.Read())
                {
                    stockfor = rdr1["stockfor"].ToString();
                    grade = rdr1["grade"].ToString();
                    name = rdr1["name"].ToString();
                    Type_txt.Focus();

                }
            }
        }
        void grid()
        {
            string qry = "select id as Id,sno as SNo,packedby as StockFor,productionid as Production,grade as Grade,type as Type,bag as Bag,date as Date,LTRIM(RIGHT(convert(varchar,time,100),7)) as Time from PackingEntry_tbl order by id desc";
            SqlDataAdapter adb = new SqlDataAdapter(qry, Database.con);
            DataTable dt = new DataTable();
            adb.Fill(dt);

            dataGridView1.DataSource = dt;
            dataGridView1.Columns[7].DefaultCellStyle.Format = "dd/MM/yyyy";
            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[1].Width = 40;
            dataGridView1.Columns[2].Width = 100;
            int count = dataGridView1.Rows.Count;
            Total_txt.Text = count.ToString();

            //Process production

            string qry1 = "select orderno as SNo,stockfor as Stockfor,productionid as Productionid,date as Date,LTRIM(RIGHT(convert(varchar,time,100),7)) as Time from ProductionEntry_tbl where status='Packing' order by id desc";
            SqlDataAdapter adb1 = new SqlDataAdapter(qry1, Database.con);
            DataTable dt1 = new DataTable();
            adb1.Fill(dt1);
            dataGridView2.DataSource = dt1;

        }
        int opno2;
        int sno;
        private void Add_btn_Click(object sender, EventArgs e)
        {
            //  try
            //   {
            if (SNo_txt.Text == "" || Productionid_txt.Text == "" || Type_txt.Text == "" || Noofbag_txt.Text == "")
            {
                MessageBox.Show("Kindly Fill Primary Information !!!", messagename, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                string qry1 = "select * from PackingEntry_tbl where  sno=" + SNo_txt.Text + "";
                SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                SqlDataReader rdr1 = cmd1.ExecuteReader();
                if (rdr1.Read())
                {
                    MessageBox.Show("This Production Id Already Inserted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    string memberid = "";
                    string opnoqry = "select top 1 serialno from PackingSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
                    SqlDataReader opnordr = opnocmd.ExecuteReader();
                    if (opnordr.Read())
                    {
                        string no = opnordr["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        sno = opno1 + 1;
                        memberid = sno.ToString();
                    }
                    
                    if (Noofbag_txt.Text == "")

                    {
                        Noofbag_txt.Text = "0";
                    }
                  
                    string qry211 = "insert into PackingEntry_tbl values (" + memberid + ",N'" + Productionid_txt.Text + "',N'" + stockfor + "',N'" + Packedby_combo.Text + "',"+PackingRice_txt.Text+",'" + grade + "','" + name + "','" + Type_txt.Text + "'," + Noofbag_txt.Text + ",'" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + DateTime.Today.ToString("yyyy-MM-dd hh:mm:ss tt") + "','Process',"+Totalweight_txt.Text+")";
                    SqlCommand cmd211 = new SqlCommand(qry211, Database.con);
                    cmd211.ExecuteNonQuery();
                    
                    string qry212 = "insert into ProductionStatus_tbl values ('" + memberid + "',N'" + Productionid_txt.Text + "','Packing',"+Totalweight_txt.Text+","+Noofbag_txt.Text+",N'" +stockfor + "','ReadyforDelivery','" + DateTime.Today.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "')";
                    SqlCommand cmd212 = new SqlCommand(qry212, Database.con);
                    cmd212.ExecuteNonQuery();
                    
                    string qry214 = "update ProductionEntry_tbl set status='ReadyforDelivery' where productionid='" + Productionid_txt.Text + "'";
                    SqlCommand cmd214 = new SqlCommand(qry214, Database.con);
                    cmd214.ExecuteNonQuery();
                    
                    /*  string qry213 = "update StockManagement_tbl set output=" + Rice_txt.Text + " where productionid='" + Productionid_txt.Text + "'";
                      SqlCommand cmd213 = new SqlCommand(qry213, Database.con);
                      cmd213.ExecuteNonQuery();
                    */
                    
                    string qry21 = "insert into PackingSerialNo_tbl values(" + sno + ")";
                    SqlCommand cmd21 = new SqlCommand(qry21, Database.con);
                    cmd21.ExecuteNonQuery();

                    string opnoqry1 = "select top 1 serialno from IncomeSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd1 = new SqlCommand(opnoqry1, Database.con);
                    SqlDataReader opnordr1 = opnocmd1.ExecuteReader();
                    if (opnordr1.Read())
                    {
                        string no = opnordr1["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;

                    }
                    
                    string get1 = memberid + " - " + Productionid_txt.Text;

                    /*   string qry31 = "insert into Income_tbl values(" + opno2 + ",'RiceOutput','" + DateTime.Today + "',N'" + get1 + "'," + Totalweight_txt.Text + ")";
                       SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                       cmd31.ExecuteNonQuery();*/

                    string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','PackingEntry','ProductionId:" + Productionid_txt.Text + "-"+stockfor+"_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                    cmd3.ExecuteNonQuery();
                    
                    MessageBox.Show("Packing Id " + SNo_txt.Text + " Saved Successfully...", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    number();
                    grid();
                    
                    Productionid_txt.Text = "";
                    Packedby_combo.SelectedIndex = -1;
                    //  comboBox1.SelectedIndex = -1;
                    //  Grade_combo.SelectedIndex = -1;
                    Noofbag_txt.Text = "";
                    Type_txt.Text = "";
                    PackingRice_txt.Text = "";
                  //  Husk_txt.Text = "";

                    Productionid_txt.Focus();

                }
            }
            /*  }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message,messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);

              }*/
        }

        private void Clear_btn_Click(object sender, EventArgs e)
        {
            Productionid_txt.Text = "";
            Packedby_combo.SelectedIndex = -1;
            //  comboBox1.SelectedIndex = -1;
            //  Grade_combo.SelectedIndex = -1;
            Noofbag_txt.Text = "";
            Type_txt.Text = "";
            PackingRice_txt.Text = "";
            //  Husk_txt.Text = "";

            Productionid_txt.Focus();
        }

        private void PackingEntry_Load(object sender, EventArgs e)
        {
            PackingRice_txt.Focus();
        }
        string productionid = "";
        string orderid = "";
       // string stockfor = "";
        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView2.Rows)
            {
                if (dr.Cells[0].Selected == true)
                {

                    productionid = dr.Cells[2].Value.ToString();
                    orderid = dr.Cells[0].Value.ToString();
                    Productionid_txt.Text = productionid;

                    string qry = "select * from RiceOutput_tbl where productionid='" + productionid + "'";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {

                        Totalweight_txt.Text = rdr["rice"].ToString();
                        stockfor= rdr["stockfor"].ToString();
                        grade= rdr["grade"].ToString();
                        
                    }
                }
            }
        }

        private void delete_btn_Click(object sender, EventArgs e)
        {

        }
    }
}
