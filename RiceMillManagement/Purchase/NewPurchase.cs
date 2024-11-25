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
using System.Windows.Controls;

namespace RiceMillManagement.Purchase
{
    public partial class NewPurchase : Form
    {
        private string login;
        public NewPurchase(string login)
        {
            InitializeComponent();Database dbs=new Database();
            dbs.db();
            this.login = login;
            Officeinfo();
            number();
            paddysupplier();
            Grade();
            grid();
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
            string opnoqry = "select top 1 serialno from PurchaseSerialNo_tbl order by serialno desc";
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
                string qry = "select * from RiceSupplier_tbl";
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
        private void Close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        void grid()
        {
            string qry = "select id as Id,sno as SNo,stockfor as StockFor,grade as Grade,name as Rice,weight as Weight,date as Date,LTRIM(RIGHT(convert(varchar,time,100),7)) as Time from PurchaseEntry_tbl order by id desc";
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

                string qry1 = "select * from PurchaseEntry_tbl where  sno=" + SNo_txt.Text + "";
                SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                SqlDataReader rdr1 = cmd1.ExecuteReader();
                if (rdr1.Read())
                {
                    MessageBox.Show("This Purchase Id Already Inserted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    string memberid = "";
                    string opnoqry = "select top 1 serialno from PurchaseSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
                    SqlDataReader opnordr = opnocmd.ExecuteReader();
                    if (opnordr.Read())
                    {
                        string no = opnordr["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;
                        memberid = serial.ToString();
                    }

                    string qry211 = "insert into PurchaseEntry_tbl values (" + memberid + ",N'" + From_combo.Text + "',N'" + Grade_combo.Text + "','" + Name_txt.Text + "',"+Rate_txt.Text+"," + Weight_txt.Text + ","+Totalamount_txt.Text+",'" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "','Process')";
                    SqlCommand cmd211 = new SqlCommand(qry211, Database.con);
                    cmd211.ExecuteNonQuery();

                    string qry215 = "select * from  Overallstock_tbl";
                    SqlCommand cmd215 = new SqlCommand(qry215, Database.con);
                    SqlDataReader rdr215 = cmd215.ExecuteReader();
                    if (rdr215.Read())
                    {
                        string rice = rdr215["totalrice"].ToString();
                        decimal finalpaddy = Convert.ToDecimal(rice) + Convert.ToDecimal(Weight_txt.Text);

                        string qry217 = "update Overallstock_tbl set totalrice=" + finalpaddy + "";
                        SqlCommand cmd217 = new SqlCommand(qry217, Database.con);
                        cmd217.ExecuteNonQuery();
                        
                    }
                    string qry218 = "insert into CreditDebit_tbl values ('" + memberid + "','RicePurchase','Rice',0,0," + Weight_txt.Text + ",0,N'" + From_combo.Text + "','" + Name_txt.Text + "','" + Grade_combo.Text + "','" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "')";
                    SqlCommand cmd218 = new SqlCommand(qry218, Database.con);
                    cmd218.ExecuteNonQuery();

                    string qry2 = "insert into PurchaseSerialNo_tbl values(" + opno2 + ")";
                    SqlCommand cmd2 = new SqlCommand(qry2, Database.con);
                    cmd2.ExecuteNonQuery();

                    string opnoqry1 = "select top 1 serialno from ExpenseSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd1 = new SqlCommand(opnoqry1, Database.con);
                    SqlDataReader opnordr1 = opnocmd1.ExecuteReader();
                    if (opnordr1.Read())
                    {
                        string no = opnordr1["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;

                    }

                    string get1 = memberid + " - " + SNo_txt.Text;

                       string qry31 = "insert into Expense_tbl values(" + opno2 + ",'RicePurchase','" + DateTime.Today + "',N'" + get1 + "'," + Totalamount_txt.Text + ")";
                       SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                       cmd31.ExecuteNonQuery();

                    string qry21 = "insert into ExpenseSerialNo_tbl values(" + opno2 + ")";
                    SqlCommand cmd21 = new SqlCommand(qry21, Database.con);
                    cmd21.ExecuteNonQuery();

                    string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','PurchaseEntry','PurchaseId:" + SNo_txt.Text + "-"+From_combo.Text+"_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
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
            if (e.KeyCode == Keys.Enter)
            {
                if (Rate_txt.Text == "")
                {
                    Rate_txt.Text = "0";
                }
                if (Weight_txt.Text == "")
                {
                    Weight_txt.Text = "0";
                }

                decimal total = Convert.ToDecimal(Weight_txt.Text) * Convert.ToDecimal(Rate_txt.Text);
                Totalamount_txt.Text = total.ToString();
            }
        }

        private void From_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string qry = "select * from RiceSupplier_tbl where name='" + From_combo.Text + "'";
            SqlCommand cmd = new SqlCommand(qry, Database.con);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                Name_txt.Text = rdr["cusname"].ToString();

            }
            Rate_txt.Focus();
        }
        string getid = "";
        string getname = "";
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                if (dr.Cells[0].Selected == true)
                {
                    // Id_txt.Text = dr.Cells[0].Value.ToString();
                    getid = dr.Cells[0].Value.ToString();
                    getname = dr.Cells[1].Value.ToString();

                    string qry = "select * from PurchaseEntry_tbl where id='" + getid + "'";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        From_combo.Text = rdr["stockfor"].ToString();
                        Grade_combo.Text = rdr["grade"].ToString();
                        Name_txt.Text = rdr["name"].ToString();
                        Rate_txt.Text = rdr["rate"].ToString();
                        Weight_txt.Text = rdr["weight"].ToString();
                        Totalamount_txt.Text = rdr["total"].ToString();
                        dateTimePicker1.Text = rdr["date"].ToString();
                       
                        Add_btn.Enabled = false;
                        Update_btn.Enabled = true;

                    }
                }
            }
        }

        private void Update_btn_Click(object sender, EventArgs e)
        {

        }

        private void delete_btn_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you wish to Delete the selected Status?", messagename, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                //To Find Status Alraedy Is USe
                if (getid != "")
                {
                    string qry = "delete from PurchaseEntry_tbl where id=" + getid + "";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    cmd.ExecuteNonQuery();


                    string qry31 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','PurchaseEntry','PurchaseEntry:" + getname + "_Delete','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                    cmd31.ExecuteNonQuery();

                    MessageBox.Show("Record Deleted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    grid();
                }
                else
                {
                    MessageBox.Show("\r\nPlease select any one Row to Delete Purchaser Details\r\n", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else if (dialogResult == DialogResult.No)
            {

            }
        }
    }
}
