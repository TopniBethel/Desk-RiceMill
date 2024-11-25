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

namespace RiceMillManagement.DaySheet
{
    public partial class IncomeEntry : Form
    {
        private string login;
        public IncomeEntry(string login)
        {
            InitializeComponent(); Database dbs = new Database();
            dbs.db();
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
            string opnoqry = "select top 1 serialno from IncomeSerialNo_tbl order by serialno desc";
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
                string qry = "select * from IncomeSetting_tbl";
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
                string qry = "select id as Id,serialno as Serial,category as Category,date as Date,description as Description,amount as Amount from Income_tbl where date='" + DateTime.Today + "' order by id desc";
                SqlDataAdapter adb = new SqlDataAdapter(qry, Database.con);
                DataTable dt = new DataTable();
                adb.Fill(dt);
                dataGridView1.DataSource = dt;
                this.dataGridView1.Columns[3].DefaultCellStyle.Format = "dd/MM/yyyy";

                string qry4 = "select sum(amount) from Income_tbl where date='" + DateTime.Today + "'";
                SqlCommand cmd4 = new SqlCommand(qry4, Database.con);
                SqlDataReader rdr4 = cmd4.ExecuteReader();
                if (rdr4.Read())
                {
                    string amount = rdr4[0].ToString();
                    TotalIncome_txt.Text = amount;
                }
            }
            catch
            {
                MessageBox.Show("Please Follow Proper Method");
            }
        }

        private void Close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        int opno2;
        private void Add_btn_Click(object sender, EventArgs e)
        {
            if (Description_tbl.Text == "" || Amount_txt.Text == "" || Category_combo.Text == "")
            {
                MessageBox.Show("Please Follow Proper Method..", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {

                string incomeid = "";
                string opnoqry = "select top 1 serialno from IncomeSerialNo_tbl order by serialno desc";
                SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
                SqlDataReader opnordr = opnocmd.ExecuteReader();
                if (opnordr.Read())
                {
                    string no = opnordr["serialno"].ToString();
                    int opno1 = Convert.ToInt32(no);
                    opno2 = opno1 + 1;
                    incomeid = serial.ToString();
                }

                string qry3 = "insert into Income_tbl values(" + incomeid + ",'" + Category_combo.Text + "','" + DateTime.Today + "','" + Description_tbl.Text + "'," + Amount_txt.Text + ")";
                SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                cmd3.ExecuteNonQuery();

                string qry2 = "insert into IncomeSerialNo_tbl values(" + opno2 + ")";
                SqlCommand cmd2 = new SqlCommand(qry2, Database.con);
                cmd2.ExecuteNonQuery();

                string qry31 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','Income','Income:" + Category_combo.Text + "_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                cmd31.ExecuteNonQuery();

                string qry4 = "select sum(amount) from Income_tbl where date='" + DateTime.Today + "'";
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

        private void IncomeEntry_Load(object sender, EventArgs e)
        {
            Category_combo.Focus();
        }
    }
}
