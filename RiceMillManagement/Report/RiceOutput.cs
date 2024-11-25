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
    public partial class RiceOutput : Form
    {
        private string login;
        public RiceOutput(string login)
        {
            InitializeComponent();Database dbs=new Database();
            dbs.db();
            this.login = login;
        }

        private void Search_btn_Click(object sender, EventArgs e)
        {
            string qry = "select id as Id,sno as SNo,stockfor as StockFor,productionid as Production,grade as Grade,rice as Rice,blackrice as Black,brokenrice as Broken,husk as Husk,date as Date,LTRIM(RIGHT(convert(varchar,time,100),7)) as Time from RiceOutput_tbl where date between '\" + Convert.ToDateTime(dateTimePicker1.Text).ToString(\"yyyy-MM-dd\") + \"' and '\" + Convert.ToDateTime(dateTimePicker2.Text).ToString(\"yyyy-MM-dd\") + \"' order by id desc";
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
            
        }
    }
}
