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

namespace RiceMillManagement.DaySheet
{
    public partial class ExpenseEntry : Form
    {
        private string login;
        public ExpenseEntry(string login)
        {
            InitializeComponent();
            this.login = login;
            label2.Text = login.ToString();
            Officeinfo();
            number();
            category();
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
            StaffNo_txt.Text = "";
            string opnoqry = "select top 1 serialno from ExpenseSerialNo_tbl order by serialno desc";
            SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
            SqlDataReader opnordr = opnocmd.ExecuteReader();
            if (opnordr.Read())
            {

                string no = opnordr["serialno"].ToString();
                int opno1 = Convert.ToInt32(no);

                serial = opno1 + 1;

                StaffNo_txt.Text = serial.ToString();

            }
        }
        void category()
        {
            try
            {
                Category_combo.Items.Clear();
                string qry = "select * from ExpenseSetting_tbl";
                SqlCommand cmd = new SqlCommand(qry, Database.con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Category_combo.Items.Add(rdr["name"]);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex);
            }
            finally
            {
                //
            }
        }
        void grid()
        {
            try
            {
                string qry = "select id as Id,serialno as Serial,category as Category,date as Date,description as Description,amount as Amount from Expense_tbl where date='"+DateTime.Today.ToString("yyyy-MM-dd")+"' order by id desc";
                SqlDataAdapter adb = new SqlDataAdapter(qry, Database.con);
                DataTable dt = new DataTable();
                adb.Fill(dt);
                dataGridView1.DataSource = dt;
                this.dataGridView1.Columns[3].DefaultCellStyle.Format = "dd/MM/yyyy";
                this.dataGridView1.Columns[5].DefaultCellStyle.Format = "0.00";

                string qry4 = "select sum(amount) from Expense_tbl ";
                SqlCommand cmd4 = new SqlCommand(qry4, Database.con);
                SqlDataReader rdr4 = cmd4.ExecuteReader();
                if (rdr4.Read())
                {
                    string amount = rdr4[0].ToString();
                    TotalIncome_txt.Text =Convert.ToDecimal(amount).ToString("0.00");
                }
            }
            catch
            {
                MessageBox.Show("Please Follow Proper Method");
            }
        }
        int opno2;

        private void Add_btn_Click(object sender, EventArgs e)
        {
            if (Description_tbl.Text == "" || Amount_txt.Text == "" || Category_combo.Text == "" || Amount_txt.Text == "")
            {
                MessageBox.Show("Please Follow Proper Method..", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                string incomeid = "";
                string opnoqry = "select top 1 serialno from ExpenseSerialNo_tbl order by serialno desc";
                SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
                SqlDataReader opnordr = opnocmd.ExecuteReader();
                if (opnordr.Read())
                {
                    string no = opnordr["serialno"].ToString();
                    int opno1 = Convert.ToInt32(no);
                    opno2 = opno1 + 1;
                    incomeid = serial.ToString();
                }

                string qry3 = "insert into Expense_tbl values(" + incomeid + ",'" + Category_combo.Text + "','" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + Description_tbl.Text + "'," + Amount_txt.Text + ")";
                SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                cmd3.ExecuteNonQuery();

                string qry2 = "insert into ExpenseSerialNo_tbl values(" + opno2 + ")";
                SqlCommand cmd2 = new SqlCommand(qry2, Database.con);
                cmd2.ExecuteNonQuery();

                string qry31 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','Expense','Expense:" + Category_combo.Text + "_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                cmd31.ExecuteNonQuery();

                string qry4 = "select sum(amount) from Expense_tbl where date='" + DateTime.Today + "'";
                SqlCommand cmd4 = new SqlCommand(qry4, Database.con);
                SqlDataReader rdr4 = cmd4.ExecuteReader();
                if (rdr4.Read())
                {
                    string amount = rdr4[0].ToString();
                    TotalIncome_txt.Text = amount;
                }
                MessageBox.Show("Successfully Insert", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);
                Description_tbl.Text = "";
                Amount_txt.Text = "";
                Category_combo.SelectedIndex = -1;
                Category_combo.Focus();
                number();
                grid();
            }
        }
        string getid = "";
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                if (dr.Cells[0].Selected == true)
                {

                    getid = dr.Cells[0].Value.ToString();

                    string qry = "select * from Expense_tbl where id='" + getid + "'";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        Category_combo.Text = rdr["category"].ToString();
                        dateTimePicker1.Text = rdr["date"].ToString();
                        Description_tbl.Text = rdr["description"].ToString();
                        Amount_txt.Text = rdr["amount"].ToString();

                        Add_btn.Enabled = false;
                        Update_btn.Enabled = true;


                    }

                }
            }
        }

        private void delete_btn_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want delete this Item?", messagename, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                if (getid != "")
                {
                    string qry = "delete from Expense_tbl where id=" + getid + "";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    cmd.ExecuteNonQuery();


                    //Moment Report;

                    string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','Expense','Expense:" + getid + "_Delete','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                    cmd3.ExecuteNonQuery();

                    MessageBox.Show("Record Deleted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Description_tbl.Text = "";
                    Amount_txt.Text = "";
                    Category_combo.SelectedIndex = -1;
                    Category_combo.Focus();
                    Add_btn.Enabled = true;
                    Update_btn.Enabled = false;


                    getid = "";
                    grid();

                }
                else
                {
                    MessageBox.Show("Please Select Item Name");
                }
            }
        }

        private void Update_btn_Click(object sender, EventArgs e)
        {
            try
            {

                if (Description_tbl.Text == "" || Amount_txt.Text == "" || Category_combo.Text == "" || Amount_txt.Text == "")
                {
                    MessageBox.Show("Please Follow Proper Method..", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Category_combo.Focus();
                }
                else
                {

                    string qry3 = "update Expense_tbl set category='" + Category_combo.Text + "',description='" + Description_tbl.Text + "',amount=" + Amount_txt.Text + ",date='" + dateTimePicker1.Text + "' where id='" + getid + "'";
                    SqlCommand cmd1 = new SqlCommand(qry3, Database.con);
                    cmd1.ExecuteNonQuery();

                    //Moment Report;
                    string qry31 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','Expense','Expense:" + getid + "_Update','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                    cmd31.ExecuteNonQuery();

                    MessageBox.Show("Successfully Updated.", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Description_tbl.Text = "";
                    Amount_txt.Text = "";
                    Category_combo.SelectedIndex = -1;
                    Category_combo.Focus();
                    Add_btn.Enabled = true;
                    Update_btn.Enabled = false;


                    getid = "";
                    grid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Please Follow Proper Method..", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                //  MessageBox.Show("Exception" + ex);
                Description_tbl.Text = "";
                Amount_txt.Text = "";
                Category_combo.SelectedIndex = -1;
                Category_combo.Focus();
                Add_btn.Enabled = true;
                Update_btn.Enabled = false;

                getid = "";
                grid();
            }
        }

        private void Clear_btn_Click(object sender, EventArgs e)
        {
            Description_tbl.Text = "";
            Amount_txt.Text = "";
            Category_combo.SelectedIndex = -1;
            Category_combo.Focus();
            Add_btn.Enabled = true;
            Update_btn.Enabled = true;
        }

        private void Close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void adddata_btn_Click(object sender, EventArgs e)
        {
            Setting.ExpenseSetting ES = new Setting.ExpenseSetting(login);
            ES.Show();
        }

        private void Refreshdata_btn_Click(object sender, EventArgs e)
        {
            category();
            Category_combo.Focus();
        }

        private void ExpenseEntry_Load(object sender, EventArgs e)
        {
            Category_combo.Focus();
        }
    }
}
