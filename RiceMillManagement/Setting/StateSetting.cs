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

namespace RiceMillManagement.Setting
{
    public partial class StateSetting : Form
    {
        private string login;
        public StateSetting(string login)
        {
            InitializeComponent(); Database dbs = new Database();
            dbs.db();
            this.login = login;
            grid();
            Officeinfo();
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

        SqlDataAdapter adb;
        DataSet ds;
        SqlCommandBuilder cmdbl;
        void grid()
        {
            try
            {
                ///////Notes/////  Database Table id shuld set primary key
                adb = new SqlDataAdapter("select id as Id,name as Name,code as Code,shortname as ShortName from StateSetting_tbl order by id asc", Database.con);
                // adb = new SqlDataAdapter("select * from ItemSetting_tbl", con);
                ds = new System.Data.DataSet();
                adb.Fill(ds, "ItemDetails");
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].ReadOnly = true;
                //dataGridView1.Columns[1].ReadOnly = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception" + ex);
            }
        }

        private void Save_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (State_combo.Text == "")
                {
                    MessageBox.Show("Please Enter Name...", messagename, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {

                    string qry1 = "select * from StateSetting_tbl where name='" + State_combo.Text + "'";
                    SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                    SqlDataReader rdr1 = cmd1.ExecuteReader();
                    if (rdr1.Read())
                    {
                        MessageBox.Show("This Name Already Inserted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string qry = "insert into StateSetting_tbl values('" + State_combo.Text + "','" + Code_txt.Text + "','')";
                        SqlCommand cmd = new SqlCommand(qry, Database.con);
                        cmd.ExecuteNonQuery();

                        string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','StateSetting','State:" + State_combo.Text + "_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                        cmd3.ExecuteNonQuery();

                        State_combo.SelectedIndex = -1;
                        Code_txt.Text = "";
                        MessageBox.Show("Successfully Save", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        grid();
                        State_combo.Focus();

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Delete_btn_Click(object sender, EventArgs e)
        {

        }

        private void StateSetting_Load(object sender, EventArgs e)
        {
            State_combo.Focus();
        }
    }
}
