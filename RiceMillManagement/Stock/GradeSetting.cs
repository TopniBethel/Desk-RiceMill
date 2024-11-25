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

namespace RiceMillManagement.Stock
{
    public partial class GradeSetting : Form
    {
        private string login;
        public GradeSetting(string login)
        {
            InitializeComponent(); Database dbs = new Database();
            dbs.db();
            this.login = login;
            Officeinfo();
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

        SqlDataAdapter adb;
        DataSet ds;
        SqlCommandBuilder cmdbl;
        void grid()
        {
            try
            {
                ///////Notes/////  Database Table id shuld set primary key
                adb = new SqlDataAdapter("select id as Id,name as Name,reference as Reference from GradeSetting_tbl order by id asc", Database.con);
                // adb = new SqlDataAdapter("select * from ItemSetting_tbl", con);
                ds = new System.Data.DataSet();
                adb.Fill(ds, "ItemDetails");
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].ReadOnly = false;
                
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception" + ex);
            }
        }
        private void Close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Save_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (Name_txt.Text == "")
                {
                    MessageBox.Show("Please Enter Name...", messagename, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {

                    string qry1 = "select * from GradeSetting_tbl where name='" + Name_txt.Text + "'";
                    SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                    SqlDataReader rdr1 = cmd1.ExecuteReader();
                    if (rdr1.Read())
                    {
                        MessageBox.Show("This Name Already Inserted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string qry = "insert into GradeSetting_tbl values('" + Name_txt.Text + "','"+Reference_txt.Text+"')";
                        SqlCommand cmd = new SqlCommand(qry, Database.con);
                        cmd.ExecuteNonQuery();

                        string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','GradeSetting','Grade:" + Name_txt.Text + "_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                        cmd3.ExecuteNonQuery();

                        Name_txt.Text = "";

                        MessageBox.Show("Successfully Save", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        grid();
                        Name_txt.Focus();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        string getid = "";
        private void Delete_btn_Click(object sender, EventArgs e)
        {
            try
            {
                //To Find Status Alraedy Is USe
                if (getid == "")
                {
                    MessageBox.Show("\r\nPlease select any one Row to Delete Grade Details\r\n", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Do you wish to Delete the selected Status?", messagename, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        //To Find Status Alraedy Is USe

                        string qry = "delete from GradeSetting_tbl where id=" + getid + "";
                        SqlCommand cmd = new SqlCommand(qry, Database.con);
                        cmd.ExecuteNonQuery();

                        string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','GradeSetting','Grade:" + getid + "_Delete','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                        cmd3.ExecuteNonQuery();

                        MessageBox.Show("Record Deleted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                        grid();

                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        ///
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Update_btn_Click(object sender, EventArgs e)
        {
            try
            {
                //  cmdbl = new SqlCommandBuilder(adb);
                //    adb.Update(ds, "ItemDetails");

                string qry = "update GradeSetting_tbl set name='" + Name_txt.Text + "',reference='" + Reference_txt.Text + "' where id="+getid+"";
                SqlCommand cmd = new SqlCommand(qry, Database.con);
                cmd.ExecuteNonQuery();

                MessageBox.Show("Infromation Updated..", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','GradeSetting','Grade Update','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                cmd3.ExecuteNonQuery();

                
                grid();

                Name_txt.Text = "";
                Reference_txt.Text = "";
                Name_txt.Text = "";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void GradeSetting_Load(object sender, EventArgs e)
        {
            Name_txt.Focus();
        }

        private void Name_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                Reference_txt.Focus();
            }
        }

        private void Reference_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                Save_btn.Focus();
            }
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                if (dr.Cells[0].Selected == true)
                {
                    // Id_txt.Text = dr.Cells[0].Value.ToString();
                    getid = dr.Cells[0].Value.ToString();
                   
                    string qry = "select * from GradeSetting_tbl where id='" + getid + "'";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        Name_txt.Text = rdr["name"].ToString();
                        Reference_txt.Text = rdr["reference"].ToString();
                
                        Save_btn.Enabled = false;
                        Update_btn.Enabled = true;


                    }
                }
            }
        }
    }
}
