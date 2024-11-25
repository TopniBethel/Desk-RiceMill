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
    public partial class RiceSupplier : Form
    {
        private string login;
        public RiceSupplier(string login)
        {
            InitializeComponent();Database dbs=new Database();
            dbs.db();
            this.login = login;
            Officeinfo();
            number();

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
            string opnoqry = "select top 1 serialno from RiceSupplierSerialNo_tbl order by serialno desc";
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

        private void State_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            Code_txt.Text = "";
            string qry = "select * from StateSetting_tbl where name='" + State_combo.Text + "'";
            SqlCommand cmd = new SqlCommand(qry, Database.con);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                Code_txt.Text = rdr["shortname"].ToString();

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
                adb = new SqlDataAdapter("select id as id,serialno as SNo,name as BusinessName,address as Address,contact as Contact,city as City,state as State,statecode as Code,cusname as Name from RiceSupplier_tbl order by name", Database.con);
                // adb = new SqlDataAdapter("select * from ItemSetting_tbl", con);
                ds = new System.Data.DataSet();
                adb.Fill(ds, "ItemDetails");
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].ReadOnly = false;
                dataGridView1.Columns[2].ReadOnly = false;
                dataGridView1.Columns[3].ReadOnly = false;

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
                    string qry = "select * from RiceSupplier_tbl where name='" + Name_txt.Text + "'";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        MessageBox.Show("Business Name Already Inserted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else
                    {
                        string serialno = "";
                        string opnoqry = "select top 1 serialno from RiceSupplierSerialNo_tbl order by serialno desc";
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

                        string qry1 = "insert into RiceSupplier_tbl values(" + serialno + ",'" + Name_txt.Text + "','" + Address_txt.Text + "','" + Contact_txt.Text + "','" + Gst_txt.Text + "',0,'" + City_txt.Text + "','" + State_combo.Text + "','" + Code_txt.Text + "','" + Purchasername_txt.Text + "')";
                        SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                        cmd1.ExecuteNonQuery();
                        
                        string qry2 = "insert into RiceSupplierSerialNo_tbl values(" + serialno + ")";
                        SqlCommand cmd2 = new SqlCommand(qry2, Database.con);
                        cmd2.ExecuteNonQuery();
                        
                        string qry31 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','RiceSupplier','RiceSupplier:" + Purchasername_txt.Text + "_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                        SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                        cmd31.ExecuteNonQuery();
                        
                        Name_txt.Text = "";
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

        private void Update_btn_Click(object sender, EventArgs e)
        {
            try
            {
                cmdbl = new SqlCommandBuilder(adb);
                adb.Update(ds, "ItemDetails");


                string qry31 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','RiceSupplier','RiceSupplier_Update','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                cmd31.ExecuteNonQuery();

                MessageBox.Show("Infromation Updated..", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                    string qry = "delete from RiceSupplier_tbl where id=" + getid + "";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    cmd.ExecuteNonQuery();


                    string qry31 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','RiceSupplier','RiceSupplier:" + getname + "_Delete','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
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

        private void Close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
