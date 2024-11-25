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

namespace RiceMillManagement.Report
{
    public partial class CreditDebit : Form
    {
        private string login;
        public CreditDebit(string login)
        {
            InitializeComponent();Database dbs = new Database();
            dbs.db();
            this.login = login;
        }

        private void Search_btn_Click(object sender, EventArgs e)
        {
            string qry = "select id as Id,serialno as SNo,mode as Mode,type as Type,paddycredit as PaddyCredit,paddydebit,ricecredit as RiceCredit,ricedebit as RiceDebit,cusname as StockFor,name as Name,grade as Grade,date as Date,LTRIM(RIGHT(convert(varchar,time,100),7)) as Time from CreditDebit_tbl where date between '" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(dateTimePicker2.Text).ToString("yyyy-MM-dd") + "' order by id desc";
            SqlDataAdapter adb = new SqlDataAdapter(qry, Database.con);
            DataTable dt = new DataTable();
            adb.Fill(dt);

            dataGridView1.DataSource = dt;
            
            dataGridView1.Columns[11].DefaultCellStyle.Format = "dd/MM/yyyy";
            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[1].Width = 40;
            dataGridView1.Columns[2].Width = 200;
            int count = dataGridView1.Rows.Count;
            TotalSales_txt.Text = count.ToString();
            
        }

        private void Close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
