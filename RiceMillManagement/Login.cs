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
using RiceMillManagement.Properties;
using System.Web.UI.WebControls;

namespace RiceMillManagement
{
    public partial class Login : Form
    {
        public Login()
        {
            InitializeComponent();Database dbs = new Database();
            dbs.db();
        }
        string loginname = "";
        private void Login_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (UserName_txt.Text != "" && Password_txt.Text != "" && Roll_combo.Text != "")
                {
                    string qry = "select * from Pwd_tbl where type='" + Roll_combo.Text + "' and username='" + UserName_txt.Text + "' and pwd='" + Password_txt.Text + "'";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {

                     
                            loginname = UserName_txt.Text;
                        

                        if (Roll_combo.Text == "Admin")
                        {
                            string qry1 = "update Pwd_tbl set login='" + DateTime.Now + "' where role='" + Roll_combo.Text + "' and username='" + UserName_txt.Text + "' and pwd='" + Password_txt.Text + "'";
                            SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                            cmd1.ExecuteNonQuery();

                            string qry3 = "insert into LogReport_tbl values('" + UserName_txt.Text + "','" + DateTime.Now + "','LoginForm','Login','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                            SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                            cmd3.ExecuteNonQuery();


                            string login = loginname;

                            // Form1 FM = new Form1(login);
                            // FM.Show();

                            Form1 FM = new Form1(login);
                            FM.Show();
                            this.Hide();

                        }
                        else
                        {
                            string qry1 = "update Pwd_tbl set login='" + DateTime.Now + "' where type='" + Roll_combo.Text + "' and username='" + UserName_txt.Text + "' and pwd='" + Password_txt.Text + "'";
                            SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                            cmd1.ExecuteNonQuery();

                            string login = loginname;
                          // MainMenu FM = new MainMenu(login);
                         //   FM.Show();
                            this.Hide();

                            string qry3 = "insert into LogReport_tbl values('" + UserName_txt.Text + "','" + DateTime.Now + "','LoginForm','Login','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                            SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                            cmd3.ExecuteNonQuery();

                        }
                       
                    }
                    else
                    {

                        if (Roll_combo.Text == "Super Admin" && UserName_txt.Text == "Super Admin" && Password_txt.Text == "#SuperFastAdmin3")
                        {
                            Cursor.Current = Cursors.WaitCursor;

                           // Setting.SuperAdminModification FM = new Setting.SuperAdminModification();
                          //  FM.Show();


                            string qry3 = "insert into LogReport_tbl values('" + UserName_txt.Text + "','" + Convert.ToDateTime(DateTime.Now).ToString("yyyy-MM-dd") + "','Login')";
                            SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                            cmd3.ExecuteNonQuery();

                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Username Or Password Incorrect Please Check the login credentials..", "Warning..!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            UserName_txt.Text = "";
                            Password_txt.Text = "";
                            UserName_txt.Focus();
                        }



                    }
                }
                else
                {
                    MessageBox.Show("Please Fill the login credentials..", "Warning..!!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("exception :" + ex);
            }
        }

        private void Cancel_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox2.Checked == true)
            {
                Password_txt.PasswordChar = '\0';
                Password_txt.PasswordChar = '\0';
            }
            else
            {

                Password_txt.PasswordChar = '*';
                Password_txt.PasswordChar = '*';
            }

        }
    }
}
