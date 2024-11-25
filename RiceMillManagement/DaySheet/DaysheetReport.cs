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
    public partial class DaysheetReport : Form
    {
        private string login;
        public DaysheetReport(string login)
        {
            InitializeComponent();
            this.login = login;
        }

        private void Search_btn_Click(object sender, EventArgs e)
        {
            try
            {

                string qry1 = "select serialno as Sno,category as Category,date as Date,description as Description,amount as Amount from Income_tbl where  date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' order by id desc";
                SqlDataAdapter adb1 = new SqlDataAdapter(qry1, Database.con);
                DataTable dt1 = new DataTable();
                adb1.Fill(dt1);
                dataGridView1.DataSource = dt1;

                this.dataGridView1.Columns[4].DefaultCellStyle.Format = "0.00";
                this.dataGridView1.Columns[2].DefaultCellStyle.Format = "dd/MM/yyyy";
                string income = "";
                string qry2 = "select sum(amount) from Income_tbl where date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'";
                SqlCommand cmd2 = new SqlCommand(qry2, Database.con);
                SqlDataReader rdr2 = cmd2.ExecuteReader();
                if (rdr2.Read())
                {
                    income = rdr2[0].ToString();
                    if (income == "")
                    {
                        income = "0";
                    }
                    Income_txt.Text =Convert.ToDecimal(income).ToString("0.00");
                }

                string qry3 = "select serialno as Sno,category as Category,date as Date,description as Description,amount as Amount from Expense_tbl where  date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "' order by id desc";
                SqlDataAdapter adb3 = new SqlDataAdapter(qry3, Database.con);
                DataTable dt3 = new DataTable();
                adb3.Fill(dt3);
                dataGridView2.DataSource = dt3;
                this.dataGridView2.Columns[4].DefaultCellStyle.Format = "0.00";
                this.dataGridView2.Columns[2].DefaultCellStyle.Format = "dd/MM/yyyy";
                string outgo = "";
                string qry4 = "select sum(amount) from Expense_tbl where date BETWEEN '" + dateTimePicker1.Text + "' and '" + dateTimePicker2.Text + "'";
                SqlCommand cmd4 = new SqlCommand(qry4, Database.con);
                SqlDataReader rdr4 = cmd4.ExecuteReader();
                if (rdr4.Read())
                {
                    outgo = rdr4[0].ToString();
                    if (outgo == "")
                    {
                        outgo = "0";
                    }
                    Expence_txt.Text = Convert.ToDecimal(outgo).ToString("0.00");
                }


                Double res = Convert.ToDouble(income) - Convert.ToDouble(outgo);
                Prof_txt.Text = res.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception :" + ex);
            }
        }

        private void Close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
