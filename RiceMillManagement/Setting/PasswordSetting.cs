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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;
using System.Windows.Controls;

namespace RiceMillManagement.Setting
{
    public partial class PasswordSetting : Form
    {
        private string login;
        public PasswordSetting(string login)
        {
            InitializeComponent(); Database dbs = new Database();
            dbs.db();
            this.login = login;
            usercombo();
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
        void usercombo()
        {
            try
            {
                //con.Open();
                UUserName_txt.Items.Clear();
                string qry = "select*from Pwd_tbl where username!='ADMIN'";
                SqlCommand cmd = new SqlCommand(qry, Database.con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    UUserName_txt.Items.Add(rdr["username"]);
                }

                //con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception :" + ex);
            }
            finally
            {
                //con.Close();
            }
        }
        void grid()
        {
            string qry = "select id as Id,role as Role,username as Username,pwd as Password,login from Pwd_tbl where role='User' order by id asc";
            SqlDataAdapter adb = new SqlDataAdapter(qry, Database.con);
            DataTable dt = new DataTable();
            adb.Fill(dt);
            dataGridView1.DataSource = dt;

        }

        private void AddAdd_btn_Click(object sender, EventArgs e)
        {
            if (AddUserName_txt.Text != "" && AddPassword_txt.Text != "")
            {
                string qry1 = "select * from Pwd_tbl where username='" + AddUserName_txt.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                SqlDataReader rdr1 = cmd1.ExecuteReader();
                if (rdr1.Read())
                {
                    MessageBox.Show("This name Already Exist...", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    AddUserName_txt.Text = "";
                }
                else
                {
                    string qry = "insert into Pwd_tbl values('User','" + AddUserName_txt.Text + "','" + AddPassword_txt.Text + "','" + DateTime.Now + "','" + Type_combo.Text + "','Custom')";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    cmd.ExecuteNonQuery();

                    string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','PasswordSetting','User:" + AddUserName_txt.Text + "_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                    cmd3.ExecuteNonQuery();

                    MessageBox.Show("Successfully Inserted..", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    AddUserName_txt.Text = "";
                    AddPassword_txt.Text = "";
                    Type_combo.Text = "";
                    Type_combo.Focus();
                    grid();
                    usercombo();
                }
            }
            else
            {
                MessageBox.Show("Please Follow Proper Method..");
            }
        }

        private void UUpdate_btn_Click(object sender, EventArgs e)
        {
            if (UUserName_txt.Text != "" && UNewPass_txt.Text != "")
            {
                string qry = "update Pwd_tbl set pwd='" + UNewPass_txt.Text + "' where username='" + UUserName_txt.Text + "' and role='User'";
                SqlCommand cmd = new SqlCommand(qry, Database.con);
                cmd.ExecuteNonQuery();

                string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','PasswordSetting','User:"+UUserName_txt.Text+"-" + UNewPass_txt.Text + "_Update','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                cmd3.ExecuteNonQuery();

                MessageBox.Show("Password Successfully Updated..");
                UNewPass_txt.Text = "";
                UUserName_txt.Text = "";
            }
            else
            {
                MessageBox.Show("Please Follow Proper Method..");
            }
        }

        private void Userclose_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        string getid = "";
        string username = "";
        private void AddDelete_btn_Click(object sender, EventArgs e)
        {
            if (getid == "")
            {
                MessageBox.Show("Please Select GridCells", messagename, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                DialogResult dialogResult = MessageBox.Show("Do you wish to Delete the selected Status?", messagename, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (dialogResult == DialogResult.Yes)
                {
                    //To Find Status Alraedy Is USe

                    string qry = "delete from  Pwd_tbl where id=" + getid + "";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    cmd.ExecuteNonQuery();

                    string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','PasswordSetting','AdminPassword:" + username + "_Delete','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                    cmd3.ExecuteNonQuery();

                    MessageBox.Show("Record Deleted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    grid();
                    dataGridView1.Focus();

                }
                else if (dialogResult == DialogResult.No)
                {
                    ///
                }
            }
        }

        private void iconButton2_Click(object sender, EventArgs e)
        {
            AdminOld_txt.PasswordChar = '*';
            AdminNew_txt.PasswordChar = '*';
            iconButton2.Visible = false;
            iconButton1.Visible = true;
        }

        private void iconButton6_Click(object sender, EventArgs e)
        {
            AdminNew_txt.PasswordChar = '*';
            AdminNew_txt.PasswordChar = '*';
            iconButton6.Visible = false;
            iconButton5.Visible = true;
        }

        private void AdminUpdate_btn_Click(object sender, EventArgs e)
        {
            if (AdminOld_txt.Text != "" && AdminNew_txt.Text != "")
            {
                string qry = "select * from Pwd_tbl where role='Admin'";
                SqlCommand cmd = new SqlCommand(qry, Database.con);
                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    string get = rdr[3].ToString();
                    if (get == AdminOld_txt.Text)
                    {
                        string qry1 = "update Pwd_tbl set pwd='" + AdminNew_txt.Text + "' where role='Admin'";
                        SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                        cmd1.ExecuteNonQuery();
                        
                        string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','PasswordSetting','AdminPassword:" + AdminNew_txt.Text + "_Update','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                        cmd3.ExecuteNonQuery();

                        MessageBox.Show("Infromation Updated..", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        AdminNew_txt.Text = "";
                        AdminUpdate_btn.Text = "";
                        
                    }
                    else
                    {
                        MessageBox.Show("Old Password Is Wrong..", "Alert", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        AdminOld_txt.Text = "";
                        AdminOld_txt.Focus();
                    }
                }

            }
            else
            {
                MessageBox.Show("Password Successfully Updated..");
            }
        }

        private void AdminClose_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                if (dr.Cells[0].Selected == true)
                {
                    // Id_txt.Text = dr.Cells[0].Value.ToString();
                    getid = dr.Cells[0].Value.ToString();
                    username = dr.Cells[1].Value.ToString();

                }
            }
        }

        private void guna2TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (guna2TabControl1.SelectedIndex == 4)
            {
                // Set focus on text box you need
                AdminOld_txt.Focus();
            }

            if (guna2TabControl1.SelectedIndex == 6)
            {
                // Set focus on text box you need
                Type_combo.Focus();
            }

            if (guna2TabControl1.SelectedIndex == 5)
            {
                // Set focus on text box you need
                UUserName_txt.Focus();
            }

        }

        private void PasswordSetting_Load(object sender, EventArgs e)
        {
            this.ActiveControl = AdminOld_txt;
            AdminOld_txt.Focus();
        }
    }
}
