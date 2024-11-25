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

namespace RiceMillManagement.Production
{
    public partial class ProductionStatus : Form
    {
        private string login;
        public ProductionStatus(string login)
        {
            InitializeComponent();Database dbs=new Database();
            dbs.db();
            this.login = login;
            grid();
        }
        void grid()
        {
            string qry1 = "select orderno as SNo,stockfor as Stockfor,productionid as Productionid,date as Date from ProductionEntry_tbl where overallstatus='Process' order by id desc";
            SqlDataAdapter adb1 = new SqlDataAdapter(qry1, Database.con);
            DataTable dt1 = new DataTable();
            adb1.Fill(dt1);
            dataGridView2.DataSource = dt1;

        }
        private void Close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Search_btn_Click(object sender, EventArgs e)
        {
            string qry = "select id as Id,productionid as Production,weight as Weight,productionfor as ProductionFor,status as Status,date as Date,LTRIM(RIGHT(convert(varchar,time,100),7)) as Time from ProductionStatus_tbl where productionid='"+ProductionId_txt.Text+"' order by id desc";
            SqlDataAdapter adb = new SqlDataAdapter(qry, Database.con);
            DataTable dt = new DataTable();
            adb.Fill(dt);

            dataGridView1.DataSource = dt;
            dataGridView1.Columns[5].DefaultCellStyle.Format = "dd/MM/yyyy";
            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[1].Width = 150;
            dataGridView1.Columns[2].Width = 100;
            
            
        }
        string orderid="";
        string productionid = "";

        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView2.Rows)
            {
                if (dr.Cells[0].Selected == true)
                {

                    productionid = dr.Cells[2].Value.ToString();
                    orderid = dr.Cells[0].Value.ToString();
                    ProductionId_txt.Text = productionid;

                    string qry = "select id as Id,productionid as Production,weight as Weight,productionfor as ProductionFor,status as Status,date as Date,LTRIM(RIGHT(convert(varchar,time,100),7)) as Time from ProductionStatus_tbl where productionid='" + ProductionId_txt.Text + "' order by id asc";
                    SqlDataAdapter adb = new SqlDataAdapter(qry, Database.con);
                    DataTable dt = new DataTable();
                    adb.Fill(dt);

                    dataGridView1.DataSource = dt;
                    dataGridView1.Columns[5].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dataGridView1.Columns[0].Width = 40;
                    dataGridView1.Columns[1].Width = 150;
                    dataGridView1.Columns[2].Width = 100;

                }
            }
        }
    }
}
