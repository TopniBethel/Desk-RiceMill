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

namespace RiceMillManagement.Supplier
{
    public partial class PaddySupplier : Form
    {
        private string login;
        public PaddySupplier(string login)
        {
            InitializeComponent();Database dbs=new Database();
            dbs.db();
            this.login = login;
            Officeinfo();
            number();
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
        int serial;
        void number()
        {
            StaffNo_txt.Text = "";
            string opnoqry = "select top 1 serialno from PaddySupplierSerialNo_tbl order by serialno desc";
            SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
            SqlDataReader opnordr = opnocmd.ExecuteReader();
            if (opnordr.Read())
            {

                string no = opnordr["serialno"].ToString();
                int opno1 = Convert.ToInt32(no);

                serial = opno1 + 1;

                StaffNo_txt.Text = serial.ToString();

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
                adb = new SqlDataAdapter("select id as id,serialno as SNo,name as BusinessName,code as Code,address as Address,contact as Contact,city as City,state as State,statecode as StateCode,cusname as Name from PaddySupplier_tbl order by name", Database.con);
                // adb = new SqlDataAdapter("select * from ItemSetting_tbl", con);
                ds = new System.Data.DataSet();
                adb.Fill(ds, "ItemDetails");
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[4].ReadOnly = false;
                dataGridView1.Columns[5].ReadOnly = false;
                dataGridView1.Columns[6].ReadOnly = false;

                dataGridView1.Columns[0].Visible = false;
                
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

                if (Name_txt.Text == "" || Contact_txt.Text == "")
                {
                    MessageBox.Show("Kindly Fill all Information !!!", messagename, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string qry = "select * from PaddySupplier_tbl where name='" + Name_txt.Text + "'";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        MessageBox.Show("Business Name Already Inserted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else
                    {

                        string serialno = "";
                        string opnoqry = "select top 1 serialno from PaddySupplierSerialNo_tbl order by serialno desc";
                        SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
                        SqlDataReader opnordr = opnocmd.ExecuteReader();
                        if (opnordr.Read())
                        {
                            string no = opnordr["serialno"].ToString();
                            int opno1 = Convert.ToInt32(no);
                           int opno2 = opno1 + 1;
                            serialno = serial.ToString();
                        }

                        if (Code_txt.Text == "")
                        {
                            Code_txt.Text = "0";
                        }
                        string qry1 = "insert into PaddySupplier_tbl values(" + serialno+",'" + Name_txt.Text + "','"+Suppliercode_txt.Text+"','" + Address_txt.Text + "','" + Contact_txt.Text + "','" + Gst_txt.Text + "',0,'" + City_txt.Text + "','" + State_combo.Text + "','" + Code_txt.Text + "','" + Purchasername_txt.Text + "')";
                        SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                        cmd1.ExecuteNonQuery();

                        string qry2 = "insert into PaddySupplierSerialNo_tbl values(" + serialno + ")";
                        SqlCommand cmd2 = new SqlCommand(qry2, Database.con);
                        cmd2.ExecuteNonQuery();

                        string qry31 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','PaddySupplier','PaddySupplier:" + Purchasername_txt.Text + "_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                        cmd31.ExecuteNonQuery();

                        Name_txt.Text = "";
                        Suppliercode_txt.Text = "";
                        Purchasername_txt.Text = "";
                        Address_txt.Text = "";
                        Contact_txt.Text = "";
                        Gst_txt.Text = "";
                        City_txt.Text = "";
                        Code_txt.Text = "";
                        State_combo.SelectedIndex = -1;

                        MessageBox.Show("Successfully Save", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);
                        grid();
                        number();
                        Name_txt.Text = "";

                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        string getid = "";
        string getname = "";
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                if (dr.Cells[0].Selected == true)
                {
                    // Id_txt.Text = dr.Cells[0].Value.ToString();
                    getid = dr.Cells[0].Value.ToString();
                    getname = dr.Cells[8].Value.ToString();
                    string qry = "select * from PaddySupplier_tbl where id='" + getid + "'";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        Name_txt.Text = rdr["name"].ToString();
                        Purchasername_txt.Text = rdr["cusname"].ToString();
                        Suppliercode_txt.Text = rdr["code"].ToString();
                        Address_txt.Text = rdr["address"].ToString();
                        Contact_txt.Text = rdr["contact"].ToString();
                        Gst_txt.Text = rdr["gst"].ToString();
                        City_txt.Text = rdr["city"].ToString();
                        State_combo.Text = rdr["state"].ToString();
                        Code_txt.Text = rdr["statecode"].ToString();

                        Save_btn.Enabled = false;
                        Update_btn.Enabled = true;


                    }
                }
            }
        }

        private void Delete_btn_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you wish to Delete the selected Status?", messagename, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                //To Find Status Alraedy Is USe
                if (getid != "")
                {
                    string qry = "delete from PaddySupplier_tbl where id=" + getid + "";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    cmd.ExecuteNonQuery();


                    string qry31 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','PaddySupplier','PaddySupplier:" + getname + "_Delete','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                    cmd31.ExecuteNonQuery();

                    MessageBox.Show("Record Deleted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    grid();
                }
                else
                {
                    MessageBox.Show("\r\nPlease select any one Row to Delete Purchaser Details\r\n", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            }
            else if (dialogResult == DialogResult.No)
            {

            }
        }

        private void Update_btn_Click(object sender, EventArgs e)
        {
            try
            {
                // cmdbl = new SqlCommandBuilder(adb);
                // adb.Update(ds, "ItemDetails");
                string qry1 = "update PaddySupplier_tbl set name='" + Name_txt.Text + "',code='" + Suppliercode_txt.Text + "',address='" + Address_txt.Text + "',contact='" + Contact_txt.Text + "',gst='" + Gst_txt.Text + "',city='" + City_txt.Text + "',state='" + State_combo.Text + "',statecode='" + Code_txt.Text + "',cusname='" + Purchasername_txt.Text + "' where id="+getid+"";
                SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                cmd1.ExecuteNonQuery();

                string qry31 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','PaddySupplier','PaddySupplier_Update','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                cmd31.ExecuteNonQuery();

                MessageBox.Show("Infromation Updated..", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                grid();


                Name_txt.Text = "";
                Suppliercode_txt.Text = "";
                Purchasername_txt.Text = "";
                Address_txt.Text = "";
                Contact_txt.Text = "";
                Gst_txt.Text = "";
                City_txt.Text = "";
                Code_txt.Text = "";
                State_combo.SelectedIndex = -1;
                Name_txt.Text = "";

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void State_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //  State_combo.SelectedIndex = -1;
            Code_txt.Text = "";
            string qry = "select * from StateSetting_tbl where name='" + State_combo.Text + "'";
            SqlCommand cmd = new SqlCommand(qry, Database.con);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                Code_txt.Text = rdr["shortname"].ToString();

            }
        }

        private void Close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void PaddySupplier_Load(object sender, EventArgs e)
        {
            Name_txt.Focus();
        }

        private void Name_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                Purchasername_txt.Focus();
            }
        }

        private void Purchasername_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Suppliercode_txt.Focus();
            }
        }

        private void Address_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Contact_txt.Focus();
            }
        }

        private void Contact_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Gst_txt.Focus();
            }
        }

        private void Gst_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                City_txt.Focus();
                
            }
        }

        private void City_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                State_combo.Focus();
            }
        }

        private void Suppliercode_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                Address_txt.Focus();
                
            }
        }
    }
}
