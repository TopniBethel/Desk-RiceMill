﻿using System;
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
    public partial class Payment : Form
    {
        private string login;
        public Payment(string login)
        {
            InitializeComponent(); Database dbs = new Database();
            dbs.db();
            this.login = login;
            Officeinfo();
            grid();
            this.ActiveControl = Name_txt;
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
                adb = new SqlDataAdapter("select id as Id,name as Name from PaymentSetting_tbl order by id asc", Database.con);
                // adb = new SqlDataAdapter("select * from ItemSetting_tbl", con);
                ds = new System.Data.DataSet();
                adb.Fill(ds, "ItemDetails");
                dataGridView1.DataSource = ds.Tables[0];
                dataGridView1.Columns[0].ReadOnly = true;
                dataGridView1.Columns[1].ReadOnly = false;

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                    string qry1 = "select * from PaymentSetting_tbl where name='" + Name_txt.Text + "'";
                    SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                    SqlDataReader rdr1 = cmd1.ExecuteReader();
                    if (rdr1.Read())
                    {
                        MessageBox.Show("This Name Already Inserted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        string qry = "insert into PaymentSetting_tbl values('" + Name_txt.Text + "')";
                        SqlCommand cmd = new SqlCommand(qry, Database.con);
                        cmd.ExecuteNonQuery();

                        string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','PaymentSetting','Payment:" + Name_txt.Text + "_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
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
                    MessageBox.Show("\r\nPlease select any one Row to Delete Payment Details\r\n", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    DialogResult dialogResult = MessageBox.Show("Do you wish to Delete the selected Status?", messagename, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (dialogResult == DialogResult.Yes)
                    {
                        //To Find Status Alraedy Is USe

                        string qry = "delete from PaymentSetting_tbl where id=" + getid + "";
                        SqlCommand cmd = new SqlCommand(qry, Database.con);
                        cmd.ExecuteNonQuery();

                        string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','PaymentSetting','Payment:" + getid + "-"+getname+"_Delete','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
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
                cmdbl = new SqlCommandBuilder(adb);
                adb.Update(ds, "ItemDetails");
                MessageBox.Show("Infromation Updated..", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','PaymentSetting','Payment Update','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                cmd3.ExecuteNonQuery();
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message, messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        string getname = "";
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                if (dr.Cells[0].Selected == true)
                {
                    // Id_txt.Text = dr.Cells[0].Value.ToString();
                    getid = dr.Cells[0].Value.ToString();
                    getname = dr.Cells[1].Value.ToString();


                }
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

        private void Payment_Load(object sender, EventArgs e)
        {
            this.ActiveControl = Name_txt;
            Name_txt.Focus();
        }

        private void Close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Name_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                Save_btn.Focus();

            }
        }
    }
}
