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

namespace RiceMillManagement.Setting
{
    public partial class EmployeeSetting : Form
    {
        private string login;
        public EmployeeSetting(string login)
        {
            InitializeComponent();
            this.login = login;
            Officeinfo();
            number();
            Jobtype();
            grid();
            Status_combo.Text = "Active";
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
            string opnoqry = "select top 1 serialno from EmployeeSerialNo_tbl order by serialno desc";
            SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
            SqlDataReader opnordr = opnocmd.ExecuteReader();
            if (opnordr.Read())
            {

                string no = opnordr["serialno"].ToString();
                int opno1 = Convert.ToInt32(no);

                serial = opno1 + 1;

                StaffNo_txt.Text = "S" + serial.ToString();

            }
        }
        void Jobtype()
        {
            try
            {
                Jobtype_combo.Items.Clear();
                string qry = "select * from JobtypeSetting_tbl";
                SqlCommand cmd = new SqlCommand(qry, Database.con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Jobtype_combo.Items.Add(rdr["name"]);

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Exception:" + ex);
            }
            finally
            {
                ///
            }
        }
      
        void grid()
        {
            string qry = "select id as Id,staffid as Staffid,name as Name,gender as Gender,contact as Contact,jobtype as JobType,joindate as JoinDate,date as Date from EmployeeEntry_tbl where status='Active' order by id asc";
            SqlDataAdapter adb = new SqlDataAdapter(qry, Database.con);
            DataTable dt = new DataTable();
            adb.Fill(dt);

            dataGridView1.DataSource = dt;
            dataGridView1.Columns[6].DefaultCellStyle.Format = "dd/MM/yyyy";
            dataGridView1.Columns[7].DefaultCellStyle.Format = "dd/MM/yyyy";

        }
        int opno2;
        private void Add_btn_Click(object sender, EventArgs e)
        {
            try
            {
                if (StaffNo_txt.Text == "" || Name_txt.Text == "" || contact_txt.Text == "" || Gender_combo.Text == "")
                {
                    MessageBox.Show("Kindly Fill all Information !!!", messagename, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string qry1 = "select * from EmployeeEntry_tbl where  name=N'" + Name_txt.Text + "'";
                    SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                    SqlDataReader rdr1 = cmd1.ExecuteReader();
                    if (rdr1.Read())
                    {
                        MessageBox.Show("This Name Already Inserted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {

                        string memberid = "";
                        string opnoqry = "select top 1 serialno from EmployeeSerialNo_tbl order by serialno desc";
                        SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
                        SqlDataReader opnordr = opnocmd.ExecuteReader();
                        if (opnordr.Read())
                        {
                            string no = opnordr["serialno"].ToString();
                            int opno1 = Convert.ToInt32(no);
                            opno2 = opno1 + 1;
                            memberid = "S" + serial.ToString();
                        }
                      
                        string qry = "INSERT into EmployeeEntry_tbl values('" + memberid + "',N'" + Name_txt.Text + "','" + Gender_combo.Text + "','" + contact_txt.Text + "','" + Emergencycontact.Text + "','" + email_txt.Text + "',N'" + Addres_txt.Text + "','" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "',N'" + Jobtype_combo.Text + "',N'" + Status_combo.Text + "','" + DateTime.Today.ToString("yyyy-MM-dd") + "')";
                        SqlCommand cmd = new SqlCommand(qry, Database.con);
                        cmd.ExecuteNonQuery();

                        string qry2 = "insert into EmployeeSerialNo_tbl values(" + opno2 + ")";
                        SqlCommand cmd2 = new SqlCommand(qry2, Database.con);
                        cmd2.ExecuteNonQuery();


                        string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','EmployeeEntry','Employee:" + Name_txt.Text + "_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                        cmd3.ExecuteNonQuery();

                        MessageBox.Show("Employee Id " + memberid + " Saved Successfully...", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        number();
                        grid();

                        Name_txt.Text = "";

                        contact_txt.Text = "";
                        Emergencycontact.Text = "";
                        email_txt.Text = "";
                        Addres_txt.Text = "";
                      
                        Gender_combo.SelectedIndex = -1;
                        Jobtype_combo.SelectedIndex = -1;
                      //  Shift_combo.SelectedIndex = -1;
                        Status_combo.SelectedIndex = -1;
                        Status_combo.Text = "Active";
                        Name_txt.Focus();

                    }

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private void Clear_btn_Click(object sender, EventArgs e)
        {
            Name_txt.Text = "";

            contact_txt.Text = "";
            Emergencycontact.Text = "";
            email_txt.Text = "";
            Addres_txt.Text = "";
          
            Gender_combo.SelectedIndex = -1;
            Jobtype_combo.SelectedIndex = -1;
           // Shift_combo.SelectedIndex = -1;
            Status_combo.SelectedIndex = -1;
            Name_txt.Focus();
            Add_btn.Enabled = true;
            Update_btn.Enabled = true;
            Status_combo.Text = "Active";
            getid = "";
        }
        string getid = "";

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                if (dr.Cells[0].Selected == true)
                {

                    getid = dr.Cells[0].Value.ToString();

                    string qry = "select * from EmployeeEntry_tbl where id='" + getid + "'";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        Name_txt.Text = rdr["name"].ToString();
                        Gender_combo.Text = rdr["gender"].ToString();
                     
                        contact_txt.Text = rdr["contact"].ToString();
                        Emergencycontact.Text = rdr["altcontact"].ToString();
                        dateTimePicker1.Text = rdr["joindate"].ToString();
                        email_txt.Text = rdr["email"].ToString();
                        Addres_txt.Text = rdr["address"].ToString();
                        Jobtype_combo.Text = rdr["jobtype"].ToString();
                      
                        Status_combo.Text = rdr["status"].ToString();


                        Add_btn.Enabled = false;
                        Update_btn.Enabled = true;
                        delete_btn.Enabled = true;


                    }

                }
            }
        }

        private void delete_btn_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want delete this Employee Details?", messagename, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                if (getid != "")
                {
                    string qry = "delete from EmployeeEntry_tbl where id='" + getid + "'";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    cmd.ExecuteNonQuery();

                    //Moment Report;
                    
                    string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','EmployeeEntry','Employee:" + getid + "-" + Name_txt.Text + "_Delete','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                    cmd3.ExecuteNonQuery();

                    MessageBox.Show("Record Deleted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Name_txt.Text = "";

                    contact_txt.Text = "";
                    Emergencycontact.Text = "";
                    email_txt.Text = "";
                    Addres_txt.Text = "";
                    
                    Gender_combo.SelectedIndex = -1;
                    Jobtype_combo.SelectedIndex = -1;
                   // Shift_combo.SelectedIndex = -1;
                    Status_combo.SelectedIndex = -1;
                    Name_txt.Focus();
                    Add_btn.Enabled = true;
                    Update_btn.Enabled = false;
                    delete_btn.Enabled = false;


                    getid = "";
                    grid();

                }
                else
                {
                    MessageBox.Show("\r\nPlease select any one Row to Delete Staff Details\r\n", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Update_btn_Click(object sender, EventArgs e)
        {
            try
            {

                if (StaffNo_txt.Text == "" || Name_txt.Text == "" || contact_txt.Text == "" || Jobtype_combo.Text == "" || Gender_combo.Text == "")
                {
                    MessageBox.Show("Please Follow Proper Method..", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Name_txt.Focus();
                }
                else
                {

                    string qry3 = "update EmployeeEntry_tbl set name='" + Name_txt.Text + "',contact='" + contact_txt.Text + "',altcontact='" + Emergencycontact.Text + "',email='" + email_txt.Text + "',address='" + Addres_txt.Text + "',jobtype='" + Jobtype_combo.Text + "',status='" + Status_combo.Text + "' where id='" + getid + "'";
                    SqlCommand cmd1 = new SqlCommand(qry3, Database.con);
                    cmd1.ExecuteNonQuery();

                    //Moment Report;
                    string qry31 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','EmployeeEntry','Employee:" + Name_txt.Text + "_Update','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                    cmd31.ExecuteNonQuery();

                    MessageBox.Show("Employee Details Successfully Updated.", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Name_txt.Text = "";

                    contact_txt.Text = "";
                    Emergencycontact.Text = "";
                    email_txt.Text = "";
                    Addres_txt.Text = "";
                 
                    Gender_combo.SelectedIndex = -1;
                    Jobtype_combo.SelectedIndex = -1;
                  //  Shift_combo.SelectedIndex = -1;
                    Status_combo.SelectedIndex = -1;
                    Name_txt.Focus();
                    Add_btn.Enabled = true;
                    Update_btn.Enabled = false;
                    delete_btn.Enabled = false;

                    getid = "";
                    grid();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                Name_txt.Text = "";

                contact_txt.Text = "";
                Emergencycontact.Text = "";
                email_txt.Text = "";
                Addres_txt.Text = "";
               
                Gender_combo.SelectedIndex = -1;
                Jobtype_combo.SelectedIndex = -1;
              //  Shift_combo.SelectedIndex = -1;
                Status_combo.SelectedIndex = -1;
                Name_txt.Focus();
                Add_btn.Enabled = true;
                Update_btn.Enabled = false;


                getid = "";
                grid();
            }
        }

        private void Name_txt_TextChanged(object sender, EventArgs e)
        {
            try
            {
                char[] v = Name_txt.Text.ToCharArray();
                string s = v[0].ToString().ToUpper();
                for (int b = 1; b < v.Length; b++)
                    s += v[b].ToString().ToLower();
                Name_txt.Text = s;
                Name_txt.Select(Name_txt.Text.Length, 0);
            }
            catch
            {

            }
        }

        private void contact_txt_KeyPress(object sender, KeyPressEventArgs e)
        {

        }

        private void Name_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Gender_combo.Focus();
            }
        }

        private void Gender_combo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                contact_txt.Focus();
            }
        }

        private void contact_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Emergencycontact.Focus();
            }
        }

        private void Emergencycontact_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                email_txt.Focus();
            }
        }

        private void email_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Addres_txt.Focus();
            }
        }

        private void Addres_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                dateTimePicker1.Focus();
            }
        }

        private void dateTimePicker1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Jobtype_combo.Focus();
            }
        }

        private void Jobtype_combo_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Add_btn.Focus();
            }
        }

        private void Close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void EmployeeSetting_Load(object sender, EventArgs e)
        {
            Name_txt.Focus();
        }
    }
}
