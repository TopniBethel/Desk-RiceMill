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
using System.Xml.Linq;
using System.Windows.Controls;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace RiceMillManagement.Production
{
    public partial class FactoryStatusEntry : Form
    {
        private string login;
        public FactoryStatusEntry(string login)
        {
            InitializeComponent();Database dbs=new Database();
            dbs.db();
            this.login = login;
            Officeinfo();
            number();
            grid();
            employee();
            paddysupplier();
        }
        void paddysupplier()
        {
            try
            {
                For_combo.Items.Clear();
                string qry = "select * from PaddySupplier_tbl";
                SqlCommand cmd = new SqlCommand(qry, Database.con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    For_combo.Items.Add(rdr["name"]);

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
            SNo_txt.Text = "";
            string opnoqry = "select top 1 serialno from FactoryStatusSerialNo_tbl order by serialno desc";
            SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
            SqlDataReader opnordr = opnocmd.ExecuteReader();
            if (opnordr.Read())
            {
                string no = opnordr["serialno"].ToString();
                int opno1 = Convert.ToInt32(no);

                serial = opno1 + 1;

                SNo_txt.Text = serial.ToString();

            }
        }
        void employee()
        {
            try
            {
                Boiling_combo.Items.Clear();
                Drying_combo.Items.Clear();
                Beating_Combo.Items.Clear();
                Helper_combo.Items.Clear();
                string qry = "select * from EmployeeEntry_tbl";
                SqlCommand cmd = new SqlCommand(qry, Database.con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Boiling_combo.Items.Add(rdr["name"]);
                    Drying_combo.Items.Add(rdr["name"]);
                    Beating_Combo.Items.Add(rdr["name"]);
                    Helper_combo.Items.Add(rdr["name"]);

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
        private void FactoryStatusEntry_Load(object sender, EventArgs e)
        {
            ProductionId_txt.Focus();
        }
        void grid()
        {
            string qry = "select sno as SNo,type as Type,date as Date,LTRIM(RIGHT(convert(varchar,time,100),7)) as Time from FactoryStatus_tbl where productionid='" + ProductionId_txt.Text + "' order by id desc";
            SqlDataAdapter adb = new SqlDataAdapter(qry, Database.con);
            DataTable dt = new DataTable();
            adb.Fill(dt);
            
            dataGridView1.DataSource = dt;
            dataGridView1.Columns[2].DefaultCellStyle.Format = "dd/MM/yyyy";
            dataGridView1.Columns[0].Width = 90;
            dataGridView1.Columns[2].Width = 90;
            dataGridView1.Columns[1].Width = 90;
            dataGridView1.Columns[3].Width = 90;
            int count = dataGridView1.Rows.Count;
            //  Total_txt.Text = count.ToString();


            //Process production

            string qry1 = "select orderno as SNo,stockfor as Stockfor,productionid as Productionid,date as Date,LTRIM(RIGHT(convert(varchar,time,100),7)) as Time from ProductionEntry_tbl where status='Process' order by id desc";
            SqlDataAdapter adb1 = new SqlDataAdapter(qry1, Database.con);
            DataTable dt1 = new DataTable();
            adb1.Fill(dt1);
            dataGridView2.DataSource = dt1;

        }
        int opno2;
        private void Loading_btn_Click(object sender, EventArgs e)
        {
            if (SNo_txt.Text == "" || ProductionId_txt.Text == "" || Loading_combo.Text == "")
            {
                MessageBox.Show("Kindly Fill Primary Information !!!", messagename, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                string qry1 = "select * from FactoryStatus_tbl where  productionid='" + ProductionId_txt.Text + "' and type='Loading'";
                SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                SqlDataReader rdr1 = cmd1.ExecuteReader();
                if (rdr1.Read())
                {
                    MessageBox.Show("This Production Id Already Inserted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    string memberid = "";
                    string opnoqry = "select top 1 serialno from FactoryStatusSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
                    SqlDataReader opnordr = opnocmd.ExecuteReader();
                    if (opnordr.Read())
                    {
                        string no = opnordr["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;
                        memberid = serial.ToString();
                    }



                    string qry211 = "insert into FactoryStatus_tbl values (" + memberid + ",N'" + ProductionId_txt.Text + "',N'" + stockfor + "','" + grade + "','" + name + "','Loading',N'ஏற்றுதல்','" + Loading_combo.Text + "','','" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "','Process')";
                    SqlCommand cmd211 = new SqlCommand(qry211, Database.con);
                    cmd211.ExecuteNonQuery();

                    string qry212 = "insert into ProductionStatus_tbl values ('" + memberid + "',N'" + ProductionId_txt.Text + "','Factory',"+Totalweight_txt.Text+",0,N'" + stockfor + "','Process','" + DateTime.Today.ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "')";
                    SqlCommand cmd212 = new SqlCommand(qry212, Database.con);
                    cmd212.ExecuteNonQuery();

                   
                    /*  string qry212 = "insert into MembershipPlan_tbl values ('" + memberid + "'," + rfid_txt.Text + ",N'" + Name_txt.Text + "','" + Gender_combo.Text + "','" + contact_txt.Text + "',N'" + Coach_combo.Text + "',N'" + Shift_combo.Text + "',N'" + Plantype_combo.Text + "'," + Amount_txt.Text + ",'" + Payment_combo.Text + "'," + Advanceamount_txt.Text + "," + Balance_txt.Text + "," + Days_txt.Text + ",'" + DateTime.Today.ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(dateTimePicker3.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(Starttime.Text).ToString("yyyy-MM-dd hh:mm:ss tt") + "','" + Convert.ToDateTime(endtime.Text).ToString("yyyy-MM-dd hh:mm:ss tt") + "',N'" + Status_combo.Text + "'," + planamount + "," + DisPerc_txt.Text + "," + DisAmt_txt.Text + ",'MemberPlan'," + opno2 + ")";
                      SqlCommand cmd212 = new SqlCommand(qry212, Database.con);
                      cmd212.ExecuteNonQuery();*/


                    string qry2 = "insert into FactoryStatusSerialNo_tbl values(" + opno2 + ")";
                    SqlCommand cmd2 = new SqlCommand(qry2, Database.con);
                    cmd2.ExecuteNonQuery();

                    string opnoqry1 = "select top 1 serialno from IncomeSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd1 = new SqlCommand(opnoqry1, Database.con);
                    SqlDataReader opnordr1 = opnocmd1.ExecuteReader();
                    if (opnordr1.Read())
                    {
                        string no = opnordr1["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;

                    }

                    string get1 = memberid + " - " + ProductionId_txt.Text;

                    /*   string qry31 = "insert into Income_tbl values(" + opno2 + ",'RiceOutput','" + DateTime.Today + "',N'" + get1 + "'," + Totalweight_txt.Text + ")";
                       SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                       cmd31.ExecuteNonQuery();*/

                    string qry21 = "insert into IncomeSerialNo_tbl values(" + opno2 + ")";
                    SqlCommand cmd21 = new SqlCommand(qry21, Database.con);
                    cmd21.ExecuteNonQuery();


                    string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','FactoryLoading','ProductionId:" + ProductionId_txt.Text + "_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                    cmd3.ExecuteNonQuery();



                    MessageBox.Show("Factory Loading Id " + SNo_txt.Text + " Saved Successfully...", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    number();
                    grid();


                    ProductionId_txt.Text = "";
                    Loading_combo.SelectedIndex = -1;
                    Totalweight_txt.Text = "";


                    ProductionId_txt.Focus();

                }
            }
            /*  }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message,messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);

              }*/
        }
        string grade = "";
        string name = "";
        string stockfor = "";
        private void ProductionId_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string qry1 = "select * from ProductionEntry_tbl where  productionid='" + ProductionId_txt.Text + "'";
                SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                SqlDataReader rdr1 = cmd1.ExecuteReader();
                if (rdr1.Read())
                {
                    stockfor = rdr1["stockfor"].ToString();
                    grade = rdr1["grade"].ToString();
                    name = rdr1["name"].ToString();
                    Loading_combo.Focus();
                }
                else
                {
                    MessageBox.Show("Production Id Not Available");
                }
            }
        }

        private void Close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Soaking_btn_Click(object sender, EventArgs e)
        {
            if (SNo_txt.Text == "" || ProductionId_txt.Text == "" || Soaking_combo.Text == "")
            {
                MessageBox.Show("Kindly Fill Primary Information !!!", messagename, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                string qry1 = "select * from FactoryStatus_tbl where  productionid='" + ProductionId_txt.Text + "' and type='Soaking'";
                SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                SqlDataReader rdr1 = cmd1.ExecuteReader();
                if (rdr1.Read())
                {
                    MessageBox.Show("This Production Id Already Inserted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    string memberid = "";
                    string opnoqry = "select top 1 serialno from FactoryStatusSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
                    SqlDataReader opnordr = opnocmd.ExecuteReader();
                    if (opnordr.Read())
                    {
                        string no = opnordr["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;
                        memberid = serial.ToString();
                    }



                    string qry211 = "insert into FactoryStatus_tbl values (" + memberid + ",N'" + ProductionId_txt.Text + "',N'" + stockfor + "','" + grade + "','" + name + "','Soaking',N'நனைய வைத்தல் ','" + Soaking_combo.Text + "','','" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "','Process')";
                    SqlCommand cmd211 = new SqlCommand(qry211, Database.con);
                    cmd211.ExecuteNonQuery();


                    /*  string qry212 = "insert into MembershipPlan_tbl values ('" + memberid + "'," + rfid_txt.Text + ",N'" + Name_txt.Text + "','" + Gender_combo.Text + "','" + contact_txt.Text + "',N'" + Coach_combo.Text + "',N'" + Shift_combo.Text + "',N'" + Plantype_combo.Text + "'," + Amount_txt.Text + ",'" + Payment_combo.Text + "'," + Advanceamount_txt.Text + "," + Balance_txt.Text + "," + Days_txt.Text + ",'" + DateTime.Today.ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(dateTimePicker3.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(Starttime.Text).ToString("yyyy-MM-dd hh:mm:ss tt") + "','" + Convert.ToDateTime(endtime.Text).ToString("yyyy-MM-dd hh:mm:ss tt") + "',N'" + Status_combo.Text + "'," + planamount + "," + DisPerc_txt.Text + "," + DisAmt_txt.Text + ",'MemberPlan'," + opno2 + ")";
                      SqlCommand cmd212 = new SqlCommand(qry212, Database.con);
                      cmd212.ExecuteNonQuery();*/


                    string qry2 = "insert into FactoryStatusSerialNo_tbl values(" + opno2 + ")";
                    SqlCommand cmd2 = new SqlCommand(qry2, Database.con);
                    cmd2.ExecuteNonQuery();

                    string opnoqry1 = "select top 1 serialno from IncomeSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd1 = new SqlCommand(opnoqry1, Database.con);
                    SqlDataReader opnordr1 = opnocmd1.ExecuteReader();
                    if (opnordr1.Read())
                    {
                        string no = opnordr1["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;

                    }

                    string get1 = memberid + " - " + ProductionId_txt.Text;

                    /*   string qry31 = "insert into Income_tbl values(" + opno2 + ",'RiceOutput','" + DateTime.Today + "',N'" + get1 + "'," + Totalweight_txt.Text + ")";
                       SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                       cmd31.ExecuteNonQuery();*/

                    string qry21 = "insert into IncomeSerialNo_tbl values(" + opno2 + ")";
                    SqlCommand cmd21 = new SqlCommand(qry21, Database.con);
                    cmd21.ExecuteNonQuery();


                    string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','FactorySoaking','ProductionId:" + ProductionId_txt.Text + "_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                    cmd3.ExecuteNonQuery();



                    MessageBox.Show("Factory Soaking Id " + SNo_txt.Text + " Saved Successfully...", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    number();
                    grid();


                    ProductionId_txt.Text = "";
                    Soaking_combo.SelectedIndex = -1;
                    Totalweight_txt.Text = "";

                    ProductionId_txt.Focus();

                }
            }
            /*  }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message,messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);

              }*/
        }

        private void Boiling_btn_Click(object sender, EventArgs e)
        {
            if (SNo_txt.Text == "" || ProductionId_txt.Text == "" || Boiling_combo.Text == "")
            {
                MessageBox.Show("Kindly Fill Primary Information !!!", messagename, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                string qry1 = "select * from FactoryStatus_tbl where  productionid='" + ProductionId_txt.Text + "' and type='Boiling'";
                SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                SqlDataReader rdr1 = cmd1.ExecuteReader();
                if (rdr1.Read())
                {
                    MessageBox.Show("This Production Id Already Inserted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    string memberid = "";
                    string opnoqry = "select top 1 serialno from FactoryStatusSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
                    SqlDataReader opnordr = opnocmd.ExecuteReader();
                    if (opnordr.Read())
                    {
                        string no = opnordr["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;
                        memberid = serial.ToString();
                    }



                    string qry211 = "insert into FactoryStatus_tbl values (" + memberid + ",N'" + ProductionId_txt.Text + "',N'" + stockfor + "','" + grade + "','" + name + "','Boiling',N'அவித்தல்','" + Boiling_combo.Text + "','','" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "','Process')";
                    SqlCommand cmd211 = new SqlCommand(qry211, Database.con);
                    cmd211.ExecuteNonQuery();


                    /*  string qry212 = "insert into MembershipPlan_tbl values ('" + memberid + "'," + rfid_txt.Text + ",N'" + Name_txt.Text + "','" + Gender_combo.Text + "','" + contact_txt.Text + "',N'" + Coach_combo.Text + "',N'" + Shift_combo.Text + "',N'" + Plantype_combo.Text + "'," + Amount_txt.Text + ",'" + Payment_combo.Text + "'," + Advanceamount_txt.Text + "," + Balance_txt.Text + "," + Days_txt.Text + ",'" + DateTime.Today.ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(dateTimePicker3.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(Starttime.Text).ToString("yyyy-MM-dd hh:mm:ss tt") + "','" + Convert.ToDateTime(endtime.Text).ToString("yyyy-MM-dd hh:mm:ss tt") + "',N'" + Status_combo.Text + "'," + planamount + "," + DisPerc_txt.Text + "," + DisAmt_txt.Text + ",'MemberPlan'," + opno2 + ")";
                      SqlCommand cmd212 = new SqlCommand(qry212, Database.con);
                      cmd212.ExecuteNonQuery();*/


                    string qry2 = "insert into FactoryStatusSerialNo_tbl values(" + opno2 + ")";
                    SqlCommand cmd2 = new SqlCommand(qry2, Database.con);
                    cmd2.ExecuteNonQuery();

                    string opnoqry1 = "select top 1 serialno from IncomeSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd1 = new SqlCommand(opnoqry1, Database.con);
                    SqlDataReader opnordr1 = opnocmd1.ExecuteReader();
                    if (opnordr1.Read())
                    {
                        string no = opnordr1["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;

                    }

                    string get1 = memberid + " - " + ProductionId_txt.Text;

                    /*   string qry31 = "insert into Income_tbl values(" + opno2 + ",'RiceOutput','" + DateTime.Today + "',N'" + get1 + "'," + Totalweight_txt.Text + ")";
                       SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                       cmd31.ExecuteNonQuery();*/

                    string qry21 = "insert into IncomeSerialNo_tbl values(" + opno2 + ")";
                    SqlCommand cmd21 = new SqlCommand(qry21, Database.con);
                    cmd21.ExecuteNonQuery();


                    string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','FactoryBoiling','ProductionId:" + ProductionId_txt.Text + "_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                    cmd3.ExecuteNonQuery();



                    MessageBox.Show("Factory Boiling Id " + SNo_txt.Text + " Saved Successfully...", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    number();
                    grid();


                    ProductionId_txt.Text = "";
                    Boiling_combo.SelectedIndex = -1;
                    For_combo.SelectedIndex = -1;
                    Totalweight_txt.Text = "";

                    ProductionId_txt.Focus();

                }
            }
            /*  }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message,messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);

              }*/
        }

        private void Drying_btn_Click(object sender, EventArgs e)
        {
            if (SNo_txt.Text == "" || ProductionId_txt.Text == "" || Drying_combo.Text == "")
            {
                MessageBox.Show("Kindly Fill Primary Information !!!", messagename, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                string qry1 = "select * from FactoryStatus_tbl where  productionid='" + ProductionId_txt.Text + "' and type='Drying'";
                SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                SqlDataReader rdr1 = cmd1.ExecuteReader();
                if (rdr1.Read())
                {
                    MessageBox.Show("This Production Id Already Inserted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    string memberid = "";
                    string opnoqry = "select top 1 serialno from FactoryStatusSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
                    SqlDataReader opnordr = opnocmd.ExecuteReader();
                    if (opnordr.Read())
                    {
                        string no = opnordr["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;
                        memberid = serial.ToString();
                    }



                    string qry211 = "insert into FactoryStatus_tbl values (" + memberid + ",N'" + ProductionId_txt.Text + "',N'" + stockfor + "','" + grade + "','" + name + "','Drying',N'காயவைத்தல்','" + Drying_combo.Text + "','','" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "','Process')";
                    SqlCommand cmd211 = new SqlCommand(qry211, Database.con);
                    cmd211.ExecuteNonQuery();


                    /*  string qry212 = "insert into MembershipPlan_tbl values ('" + memberid + "'," + rfid_txt.Text + ",N'" + Name_txt.Text + "','" + Gender_combo.Text + "','" + contact_txt.Text + "',N'" + Coach_combo.Text + "',N'" + Shift_combo.Text + "',N'" + Plantype_combo.Text + "'," + Amount_txt.Text + ",'" + Payment_combo.Text + "'," + Advanceamount_txt.Text + "," + Balance_txt.Text + "," + Days_txt.Text + ",'" + DateTime.Today.ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(dateTimePicker3.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(Starttime.Text).ToString("yyyy-MM-dd hh:mm:ss tt") + "','" + Convert.ToDateTime(endtime.Text).ToString("yyyy-MM-dd hh:mm:ss tt") + "',N'" + Status_combo.Text + "'," + planamount + "," + DisPerc_txt.Text + "," + DisAmt_txt.Text + ",'MemberPlan'," + opno2 + ")";
                      SqlCommand cmd212 = new SqlCommand(qry212, Database.con);
                      cmd212.ExecuteNonQuery();*/


                    string qry2 = "insert into FactoryStatusSerialNo_tbl values(" + opno2 + ")";
                    SqlCommand cmd2 = new SqlCommand(qry2, Database.con);
                    cmd2.ExecuteNonQuery();

                    string opnoqry1 = "select top 1 serialno from IncomeSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd1 = new SqlCommand(opnoqry1, Database.con);
                    SqlDataReader opnordr1 = opnocmd1.ExecuteReader();
                    if (opnordr1.Read())
                    {
                        string no = opnordr1["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;

                    }

                    string get1 = memberid + " - " + ProductionId_txt.Text;

                    /*   string qry31 = "insert into Income_tbl values(" + opno2 + ",'RiceOutput','" + DateTime.Today + "',N'" + get1 + "'," + Totalweight_txt.Text + ")";
                       SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                       cmd31.ExecuteNonQuery();*/

                    string qry21 = "insert into IncomeSerialNo_tbl values(" + opno2 + ")";
                    SqlCommand cmd21 = new SqlCommand(qry21, Database.con);
                    cmd21.ExecuteNonQuery();


                    string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','FactoryDrying','ProductionId:" + ProductionId_txt.Text + "_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                    cmd3.ExecuteNonQuery();



                    MessageBox.Show("Factory Drying Id " + SNo_txt.Text + " Saved Successfully...", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    number();
                    grid();


                    ProductionId_txt.Text = "";
                    Drying_combo.SelectedIndex = -1;
                    For_combo.SelectedIndex = -1;
                    Totalweight_txt.Text = "";

                    ProductionId_txt.Focus();

                }
            }
            /*  }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message,messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);

              }*/
        
    }

        private void Beating_btn_Click(object sender, EventArgs e)
        {
            if (SNo_txt.Text == "" || ProductionId_txt.Text == "" || Beating_Combo.Text == "")
            {
                MessageBox.Show("Kindly Fill Primary Information !!!", messagename, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                string qry1 = "select * from FactoryStatus_tbl where  productionid='" + ProductionId_txt.Text + "' and type='Beating'";
                SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                SqlDataReader rdr1 = cmd1.ExecuteReader();
                if (rdr1.Read())
                {
                    MessageBox.Show("This Production Id Already Inserted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    string memberid = "";
                    string opnoqry = "select top 1 serialno from FactoryStatusSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
                    SqlDataReader opnordr = opnocmd.ExecuteReader();
                    if (opnordr.Read())
                    {
                        string no = opnordr["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;
                        memberid = serial.ToString();
                    }



                    string qry211 = "insert into FactoryStatus_tbl values (" + memberid + ",N'" + ProductionId_txt.Text + "',N'" + stockfor + "','" + grade + "','" + name + "','Beating',N'அரவை','" + Beating_Combo.Text + "','"+Helper_combo.Text+"','" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "','Process')";
                    SqlCommand cmd211 = new SqlCommand(qry211, Database.con);
                    cmd211.ExecuteNonQuery();

                    string qry213 = "update ProductionEntry_tbl set status='RiceOutput' where productionid='" + ProductionId_txt.Text + "'";
                    SqlCommand cmd213 = new SqlCommand(qry213, Database.con);
                    cmd213.ExecuteNonQuery();

                    /*  string qry212 = "insert into MembershipPlan_tbl values ('" + memberid + "'," + rfid_txt.Text + ",N'" + Name_txt.Text + "','" + Gender_combo.Text + "','" + contact_txt.Text + "',N'" + Coach_combo.Text + "',N'" + Shift_combo.Text + "',N'" + Plantype_combo.Text + "'," + Amount_txt.Text + ",'" + Payment_combo.Text + "'," + Advanceamount_txt.Text + "," + Balance_txt.Text + "," + Days_txt.Text + ",'" + DateTime.Today.ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(dateTimePicker3.Text).ToString("yyyy-MM-dd") + "','" + Convert.ToDateTime(Starttime.Text).ToString("yyyy-MM-dd hh:mm:ss tt") + "','" + Convert.ToDateTime(endtime.Text).ToString("yyyy-MM-dd hh:mm:ss tt") + "',N'" + Status_combo.Text + "'," + planamount + "," + DisPerc_txt.Text + "," + DisAmt_txt.Text + ",'MemberPlan'," + opno2 + ")";
                      SqlCommand cmd212 = new SqlCommand(qry212, Database.con);
                      cmd212.ExecuteNonQuery();*/


                    string qry2 = "insert into FactoryStatusSerialNo_tbl values(" + opno2 + ")";
                    SqlCommand cmd2 = new SqlCommand(qry2, Database.con);
                    cmd2.ExecuteNonQuery();

                    string opnoqry1 = "select top 1 serialno from IncomeSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd1 = new SqlCommand(opnoqry1, Database.con);
                    SqlDataReader opnordr1 = opnocmd1.ExecuteReader();
                    if (opnordr1.Read())
                    {
                        string no = opnordr1["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;

                    }

                    string get1 = memberid + " - " + ProductionId_txt.Text;

                    /*   string qry31 = "insert into Income_tbl values(" + opno2 + ",'RiceOutput','" + DateTime.Today + "',N'" + get1 + "'," + Totalweight_txt.Text + ")";
                       SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                       cmd31.ExecuteNonQuery();*/

                    string qry21 = "insert into IncomeSerialNo_tbl values(" + opno2 + ")";
                    SqlCommand cmd21 = new SqlCommand(qry21, Database.con);
                    cmd21.ExecuteNonQuery();


                    string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','FactoryBeating','ProductionId:" + ProductionId_txt.Text + "_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                    cmd3.ExecuteNonQuery();



                    MessageBox.Show("Factory Beating Id " + SNo_txt.Text + " Saved Successfully...", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    number();
                    grid();


                    ProductionId_txt.Text = "";
                    Beating_Combo.SelectedIndex = -1;
                    For_combo.SelectedIndex = -1;
                    Totalweight_txt.Text = "";

                    ProductionId_txt.Focus();

                }
            }
            /*  }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message,messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);

              }*/
        }

        private void ProductionId_txt_TextChanged(object sender, EventArgs e)
        {

        }
        string orderid = "";
        string productionid = "";
        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView2.Rows)
            {
                if (dr.Cells[0].Selected == true)
                {
                    productionid = dr.Cells[2].Value.ToString();
                    orderid = dr.Cells[0].Value.ToString();
                    stockfor = dr.Cells[1].Value.ToString();
                    ProductionId_txt.Text=productionid;
                    For_combo.Text= stockfor;

                    string qry2 = "select * from ProductionEntry_tbl where productionid='" + ProductionId_txt.Text + "'";
                    SqlCommand cmd2 = new SqlCommand(qry2, Database.con);
                    SqlDataReader rdr2 = cmd2.ExecuteReader();
                    if (rdr2.Read())
                    {
                        Totalweight_txt.Text = rdr2["total"].ToString();
                       
                    }
                        string qry = "select sno as SNo,type as Type,date as Date,LTRIM(RIGHT(convert(varchar,time,100),7)) as Time from FactoryStatus_tbl where productionid='" + ProductionId_txt.Text + "' order by id desc";
                    SqlDataAdapter adb = new SqlDataAdapter(qry, Database.con);
                    DataTable dt = new DataTable();
                    adb.Fill(dt);

                    dataGridView1.DataSource = dt;
                    dataGridView1.Columns[2].DefaultCellStyle.Format = "dd/MM/yyyy";
                    dataGridView1.Columns[0].Width = 90;
                    dataGridView1.Columns[2].Width = 90;
                    dataGridView1.Columns[1].Width = 90;
                    dataGridView1.Columns[3].Width = 90;
                    int count = dataGridView1.Rows.Count;

                    Loading_btn.Visible= true;
                    Soaking_btn.Visible= true;
                    Boiling_btn.Visible= true;
                    Drying_btn.Visible= true;
                    Beating_btn.Visible= true;


                    string qry1 = "select * from FactoryStatus_tbl where productionid='" + ProductionId_txt.Text + "'";
                    SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                    SqlDataReader rdr1 = cmd1.ExecuteReader();
                    while(rdr1.Read())
                    {
                       string status = rdr1[6].ToString();

                        if(status== "Loading")
                        {
                            Loading_btn.Visible=false;
                        }
                       

                        if (status == "Soaking")
                        {
                            Soaking_btn.Visible = false;
                        }
                      

                        if (status == "Boiling")
                        {
                            Boiling_btn.Visible = false;
                        }
                       

                        if (status == "Drying")
                        {
                            Drying_btn.Visible = false;
                        }
                      

                        if (status == "Beating")
                        {
                            Beating_btn.Visible = false;
                        }
                       
                    }

                }
            }
        }
    }
}
