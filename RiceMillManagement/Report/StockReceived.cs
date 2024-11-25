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
    public partial class StockReceived : Form
    {
        private string login;
        public StockReceived(string login)
        {
            InitializeComponent();Database dbs=new Database();
            dbs.db();
            this.login = login;
        }

        private void Search_btn_Click(object sender, EventArgs e)
        {
            string qry = "select id as Id,orderno as SNo,stockfrom as StockFrom,stockfor as StockFor,grade as Grade,bag as Bag,name as Name,total as Total,date as Date from StockEntry_tbl where date between '" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "' and '" + Convert.ToDateTime(dateTimePicker2.Text).ToString("yyyy-MM-dd") + "' order by id desc";
            SqlDataAdapter adb = new SqlDataAdapter(qry, Database.con);
            DataTable dt = new DataTable();
            adb.Fill(dt);
            
            dataGridView1.DataSource = dt;
            
            dataGridView1.Columns[8].DefaultCellStyle.Format = "dd/MM/yyyy";
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
