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
using System.Windows.Controls;
using System.Windows.Media;

namespace RiceMillManagement.Production
{
    public partial class CreateProduction : Form
    {
        private string login;
        public CreateProduction(string login)
        {
            InitializeComponent();Database dbs=new Database();
            dbs.db();
            this.login = login;
            Officeinfo();
            number();
            Grade();
            paddysupplier();
            grid();
            Employee();
            
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
        int monthload;
        void number()
        {
            OrderNo_txt.Text = "";
            string opnoqry = "select top 1 serialno from ProductionSerialNo_tbl order by serialno desc";
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
                ProductionFor_combo.Items.Clear();
                string qry = "select * from PaddySupplier_tbl";
                SqlCommand cmd = new SqlCommand(qry, Database.con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    ProductionFor_combo.Items.Add(rdr["name"]);

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

      void Employee()
        {
            try
            {
                comboBox1.Items.Clear();
                string qry = "select * from EmployeeEntry_tbl";
                SqlCommand cmd = new SqlCommand(qry, Database.con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    comboBox1.Items.Add(rdr["name"]);

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

            string qry = "select id as Id,orderno as SNo,stockfor as StockFor,productionid as Production,grade as Grade,bag as Bag,name as Name,total as Total,date as Date from ProductionEntry_tbl order by id desc";
            SqlDataAdapter adb = new SqlDataAdapter(qry, Database.con);
            DataTable dt = new DataTable();
            adb.Fill(dt);

            dataGridView1.DataSource = dt;
            dataGridView1.Columns[8].DefaultCellStyle.Format = "dd/MM/yyyy";
            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[1].Width = 40;
            dataGridView1.Columns[2].Width = 200;
            int count = dataGridView1.Rows.Count;
            Total_txt.Text = count.ToString();

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

                string qry1 = "select * from ProductionEntry_tbl where  orderno=" + OrderNo_txt.Text + "";
                SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                SqlDataReader rdr1 = cmd1.ExecuteReader();
                if (rdr1.Read())
                {
                    MessageBox.Show("This Production Id Already Inserted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    string memberid = "";
                    string opnoqry = "select top 1 serialno from ProductionSerialNo_tbl order by serialno desc";
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

                    string qry211 = "insert into ProductionEntry_tbl values (" + memberid + ",N'" + ProductionFor_combo.Text + "',N'" + Code_txt.Text + "','" + Load_txt.Text + "','" + comboBox1.Text + "','" + Grade_combo.Text + "'," + Noofbags_txt.Text + ",'"+ProductionId_txt.Text+"',N'" + Name_txt.Text + "'," + Ton_txt.Text + "," + KiloGram_txt.Text + "," + Gram_txt.Text + "," + Totalweight_txt.Text + ",'" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "','Process','Process')";
                    SqlCommand cmd211 = new SqlCommand(qry211, Database.con);
                    cmd211.ExecuteNonQuery();

                    string qry212 = "insert into ProductionStatus_tbl values ('" + memberid + "',N'" + ProductionId_txt.Text + "','Production'," + Totalweight_txt.Text + "," + Noofbags_txt.Text + ",N'" + ProductionFor_combo.Text + "','Generated','" + DateTime.Today.ToString("yyyy-MM-dd") + "','" +DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "')";
                    SqlCommand cmd212 = new SqlCommand(qry212, Database.con);
                    cmd212.ExecuteNonQuery();

                    string qry213 = "insert into StockManagement_tbl values (" + memberid + ",N'" + ProductionId_txt.Text + "'," + Totalweight_txt.Text + ",0,0,0)";
                    SqlCommand cmd213= new SqlCommand(qry213, Database.con);
                    cmd213.ExecuteNonQuery();
                    
                    string qry214 = "insert into CreditDebit_tbl values ('" + memberid + "','CreateProduction','Paddy',0," + Totalweight_txt.Text + ",0,0,N'" + ProductionFor_combo.Text + "','" + Name_txt.Text + "','" + Grade_combo.Text + "','" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "')";
                    SqlCommand cmd214 = new SqlCommand(qry214, Database.con);
                    cmd214.ExecuteNonQuery();
                    
                    string qry215 = "select * from  Overallstock_tbl";
                    SqlCommand cmd215 = new SqlCommand(qry215, Database.con);
                    SqlDataReader rdr215 = cmd215.ExecuteReader();
                    if (rdr215.Read())
                    {
                        
                        string paddy = rdr215["paddy"].ToString();
                        decimal finalpaddy = Convert.ToDecimal(paddy)- Convert.ToDecimal(Totalweight_txt.Text);

                        string qry216= "update Overallstock_tbl set paddy=" + finalpaddy + "";
                        SqlCommand cmd216 = new SqlCommand(qry216, Database.con);
                        cmd216.ExecuteNonQuery();
                        
                    }
                    
                    string qry2 = "insert into ProductionSerialNo_tbl values(" + opno2 + ")";
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

                    string get1 = memberid + " - " + Name_txt.Text;

                    string qry31 = "insert into Income_tbl values(" + opno2 + ",'StockReceived','" + DateTime.Today + "',N'" + get1 + "'," + Totalweight_txt.Text + ")";
                    SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                    cmd31.ExecuteNonQuery();

                    string qry21 = "insert into IncomeSerialNo_tbl values(" + opno2 + ")";
                    SqlCommand cmd21 = new SqlCommand(qry21, Database.con);
                    cmd21.ExecuteNonQuery();
                  */
                    string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','CreateProduction','Production:" + ProductionId_txt.Text + "-"+ProductionFor_combo.Text+"_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                    cmd3.ExecuteNonQuery();
                    
                  //  printPreviewDialog1.Document = printDocument1;
                  //  printPreviewDialog1.ShowDialog();

                    MessageBox.Show("Production Id " + OrderNo_txt.Text + " Saved Successfully...", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    number();
                     grid();


                    Name_txt.Text = "";
                    ProductionFor_combo.SelectedIndex = -1;
                    comboBox1.SelectedIndex = -1;
                    Grade_combo.SelectedIndex = -1;
                    Load_txt.Text = "";
                    Code_txt.Text = "";
                    Noofbags_txt.Text = "";
                    Name_txt.Text = "";
                    Ton_txt.Text = "";
                    KiloGram_txt.Text = "";
                    Gram_txt.Text = "";
                    Totalweight_txt.Text = "";
                    ProductionId_txt.Text = "";
                    ProductionFor_combo.Focus();

                }
            }
            /*  }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message,messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);

              }*/
        }

        private void ProductionFor_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string qry = "select * from PaddySupplier_tbl where name='" + ProductionFor_combo.Text + "'";
            SqlCommand cmd = new SqlCommand(qry, Database.con);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                Code_txt.Text= rdr["code"].ToString();
               
            }
            Grade_combo.Focus();
            
            string qry1 = "select count(*) from ProductionEntry_tbl where stockfor='" + ProductionFor_combo.Text + "' and MONTH(date) = MONTH(GetDate())  AND YEAR(date) = YEAR(GetDate())";
            SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
            SqlDataReader rdr1 = cmd1.ExecuteReader();
            if (rdr1.Read())
            {
                string load = rdr1[0].ToString();
                int load1 = Convert.ToInt32(load);
                
                monthload = load1 + 1;
                
                Load_txt.Text = "L"+monthload.ToString();
                
            }
            
        }

        private void Get_btn_Click(object sender, EventArgs e)
        {
            string todaydate = "";
            string month = "";
            DateTime date = Convert.ToDateTime(dateTimePicker1.Text);
            todaydate = date.ToString("dd");
            month = date.ToString("MM");
            string getdate = todaydate + month;
            string get = Code_txt.Text +Load_txt.Text +getdate+ Grade_combo.Text;
            ProductionId_txt.Text = get.ToString();
            comboBox1.Focus();
        }

        private void Grade_combo_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {

            }
        }
        string getid = "";
        string orderid = "";
        string stockfor = "";
        string productionid = "";
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                if (dr.Cells[0].Selected == true)
                {
                    
                    getid = dr.Cells[0].Value.ToString();
                    orderid = dr.Cells[1].Value.ToString();
                    stockfor = dr.Cells[2].Value.ToString();
                    productionid = dr.Cells[3].Value.ToString();
                    
                    string qry = "select * from ProductionEntry_tbl where id='" + getid + "'";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        Name_txt.Text = rdr["name"].ToString();
                        Code_txt.Text = rdr["code"].ToString();
                        ProductionFor_combo.Text = rdr["stockfor"].ToString();
                        Load_txt.Text = rdr["loadname"].ToString();
                        ProductionId_txt.Text = rdr["productionid"].ToString();
                        Grade_combo.Text = rdr["grade"].ToString();
                        comboBox1.Text = rdr["createdby"].ToString();
                        Ton_txt.Text = rdr["ton"].ToString();
                        KiloGram_txt.Text = rdr["kilogram"].ToString();
                        Gram_txt.Text = rdr["gram"].ToString();
                        Totalweight_txt.Text = rdr["total"].ToString();
                        dateTimePicker1.Text = rdr["date"].ToString();
                        Noofbags_txt.Text = rdr["bag"].ToString();


                        Add_btn.Enabled = false;
                        Update_btn.Enabled = true;
                        delete_btn.Enabled = true;

                    }

                }
            }
        }

        private void Update_btn_Click(object sender, EventArgs e)
        {
          //  try
          //  {

                if (OrderNo_txt.Text == "" || Name_txt.Text == "" || Totalweight_txt.Text == "" || Grade_combo.Text == "")
                {
                    MessageBox.Show("Kindly Fill Primary Information !!!", messagename, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string qry3 = "update ProductionEntry_tbl set ton=" + Ton_txt.Text + ",kilogram=" + KiloGram_txt.Text + ",gram=" + Gram_txt.Text + ",total=" + Totalweight_txt.Text + ",bag=" + Noofbags_txt.Text + " where  id=" + getid + "";
                    SqlCommand cmd1 = new SqlCommand(qry3, Database.con);
                    cmd1.ExecuteNonQuery();
                    
                    //Moment Report;
                    string qry31 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','CreateProduction','Production:" + productionid + "-"+stockfor+"_Update','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                    cmd31.ExecuteNonQuery();
                    
                    MessageBox.Show("Successfully Updated.", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    Name_txt.Text = "";
                    ProductionFor_combo.SelectedIndex = -1;
                    comboBox1.SelectedIndex = -1;
                    Grade_combo.SelectedIndex = -1;
                    Load_txt.Text = "";
                    Code_txt.Text = "";
                    Noofbags_txt.Text = "";
                    Name_txt.Text = "";
                    Ton_txt.Text = "";
                    KiloGram_txt.Text = "";
                    Gram_txt.Text = "";
                    Totalweight_txt.Text = "";
                    ProductionId_txt.Text = "";
                    ProductionFor_combo.Focus();
                    Add_btn.Enabled = true;
                    Update_btn.Enabled = false;
                    delete_btn.Enabled = false;


                    getid = "";
                    grid();
                }
          /*  }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);

               
                Name_txt.Text = "";
                ProductionFor_combo.SelectedIndex = -1;
                comboBox1.SelectedIndex = -1;
                Grade_combo.SelectedIndex = -1;
                Load_txt.Text = "";
                Code_txt.Text = "";
                Noofbags_txt.Text = "";
                Name_txt.Text = "";
                Ton_txt.Text = "";
                KiloGram_txt.Text = "";
                Gram_txt.Text = "";
                Totalweight_txt.Text = "";
                ProductionId_txt.Text = "";
                ProductionFor_combo.Focus();

                Add_btn.Enabled = true;
                Update_btn.Enabled = false;

                getid = "";
                grid();
            }*/
        }

        private void delete_btn_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want delete this Item?", messagename, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                if (getid != "")
                {

                    string qry = "delete from ProductionEntry_tbl where id='" + getid + "'";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    cmd.ExecuteNonQuery();

                    /*  string qry1 = "delete from MembershipPlan_tbl where memberid='" + memberid + "'";
                      SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                      cmd1.ExecuteNonQuery();
                    

                    string get1 = orderid + " - " + Name_txt.Text;
                    string qry11 = "select * from Income_tbl where  description=N'" + get1 + "' and category='StockReceived'";
                    SqlCommand cmd11 = new SqlCommand(qry11, Database.con);
                    SqlDataReader rdr11 = cmd11.ExecuteReader();
                    if (rdr11.Read())
                    {

                        string id = rdr11["id"].ToString();

                        string qry31 = "delete from Income_tbl where description='" + get1 + "' and category='StockReceived' and id=" + id + "";
                        SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                        cmd31.ExecuteNonQuery();
                        
                    }
                    */
                    //Moment Report;

                    string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','CreateProduction','Production:" + productionid + "-"+stockfor+"_Delete','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                    cmd3.ExecuteNonQuery();
                    
                    MessageBox.Show("Record Deleted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Name_txt.Text = "";
                    ProductionFor_combo.SelectedIndex = -1;
                    comboBox1.SelectedIndex = -1;
                    Grade_combo.SelectedIndex = -1;
                    Load_txt.Text = "";
                    Code_txt.Text = "";
                    Noofbags_txt.Text = "";
                    Name_txt.Text = "";
                    Ton_txt.Text = "";
                    KiloGram_txt.Text = "";
                    Gram_txt.Text = "";
                    Totalweight_txt.Text = "";
                    ProductionId_txt.Text = "";
                    ProductionFor_combo.Focus();
                    Add_btn.Enabled = true;
                    Update_btn.Enabled = false;
                    delete_btn.Enabled = false;
                    
                    getid = "";
                    grid();

                }
                else
                {
                    MessageBox.Show("\r\nPlease select any one Row to Delete Member Details\r\n", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Clear_btn_Click(object sender, EventArgs e)
        {
            Name_txt.Text = "";
            ProductionFor_combo.SelectedIndex = -1;
            comboBox1.SelectedIndex = -1;
            Grade_combo.SelectedIndex = -1;
            Load_txt.Text = "";
            Code_txt.Text = "";
            Noofbags_txt.Text = "";
            Name_txt.Text = "";
            Ton_txt.Text = "";
            KiloGram_txt.Text = "";
            Gram_txt.Text = "";
            Totalweight_txt.Text = "";
            ProductionId_txt.Text = "";
            ProductionFor_combo.Focus();
            Add_btn.Enabled = true;
            Update_btn.Enabled = false;
            delete_btn.Enabled = false;
        }
    }
}
