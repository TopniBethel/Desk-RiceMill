using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Forms;

namespace RiceMillManagement.Stock
{
    public partial class StockReceivedEntry : Form
    {
        private string login;
        public StockReceivedEntry(string login)
        {
            InitializeComponent();Database dbs=new Database();
            dbs.db();
            this.login = login;
            Officeinfo();
            number();
            Grade();
            paddysupplier();
            paddyreceiver();
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
            OrderNo_txt.Text = "";
            string opnoqry = "select top 1 serialno from StockSerialNo_tbl order by serialno desc";
            SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
            SqlDataReader opnordr = opnocmd.ExecuteReader();
            if (opnordr.Read())
            {
                string no = opnordr["serialno"].ToString();
                int opno1 = Convert.ToInt32(no);

                serial = opno1 + 1;

                OrderNo_txt.Text =serial.ToString();

            }
        }

        void Grade()
        {
            try
            {
                Grade_combo.Items.Clear();
                string qry = "select * from GradeSetting_tbl";
                SqlCommand cmd = new SqlCommand(qry, Database.con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Grade_combo.Items.Add(rdr["name"]);

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
        void paddysupplier()
        {
            try
            {
                From_combo.Items.Clear();
                string qry = "select * from PaddySupplier_tbl";
                SqlCommand cmd = new SqlCommand(qry, Database.con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    From_combo.Items.Add(rdr["name"]);

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

        void paddyreceiver()
        {
            try
            {
                For_combo.Items.Clear();
                string qry = "select * from PaddySupplier_tbl";
                SqlCommand cmd = new SqlCommand(qry, Database.con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    For_combo.Items.Add(rdr["cusname"]);

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
        private void Close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void StockReceivedEntry_Load(object sender, EventArgs e)
        {
            From_combo.Focus();
        }

        void grid()
        {

            string qry = "select id as Id,orderno as SNo,stockfrom as StockFrom,stockfor as StockFor,grade as Grade,bag as Bag,name as Name,total as Total,date as Date from StockEntry_tbl  order by id desc";
            SqlDataAdapter adb = new SqlDataAdapter(qry, Database.con);
            DataTable dt = new DataTable();
            adb.Fill(dt);

            dataGridView1.DataSource = dt;
            dataGridView1.Columns[8].DefaultCellStyle.Format = "dd/MM/yyyy";
            dataGridView1.Columns[0].Width = 40;
            dataGridView1.Columns[1].Width = 40;
            dataGridView1.Columns[2].Width = 200;
            int count = dataGridView1.Rows.Count;
            Total_txt.Text = count.ToString();

        }
        int opno2;
        private void Add_btn_Click(object sender, EventArgs e)
        {
            //  try
            //   {
            if (OrderNo_txt.Text == "" || Name_txt.Text == "" || Totalweight_txt.Text == "" || Grade_combo.Text == "" )
            {
                MessageBox.Show("Kindly Fill Primary Information !!!", messagename, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                string qry1 = "select * from StockEntry_tbl where  orderno=" + OrderNo_txt.Text + "";
                SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                SqlDataReader rdr1 = cmd1.ExecuteReader();
                if (rdr1.Read())
                {
                    MessageBox.Show("This Stock Already Inserted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    string memberid = "";
                    string opnoqry = "select top 1 serialno from StockSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
                    SqlDataReader opnordr = opnocmd.ExecuteReader();
                    if (opnordr.Read())
                    {
                        string no = opnordr["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;
                        memberid =serial.ToString();
                    }


                 

                    if (Ton_txt.Text == "")

                    {
                        Ton_txt.Text = "0";
                    }
                    if (KiloGram_txt.Text == "")
                    {
                        KiloGram_txt.Text = "0";
                    }
                    if (Gram_txt.Text == "")
                    {
                        Gram_txt.Text = "0";
                    }
                    if (Totalweight_txt.Text == "")
                    {
                        Totalweight_txt.Text = "0";
                    }
                  
                        string qry211 = "insert into StockEntry_tbl values (" + memberid + ",N'" + From_combo.Text + "',N'" + For_combo.Text + "','" + MemoNo_txt.Text + "','" + Vehicle_txt.Text + "','" + Grade_combo.Text + "'," + Noofbag_txt.Text + ",N'" + Name_txt.Text + "'," + Ton_txt.Text + "," + KiloGram_txt.Text + "," + Gram_txt.Text + ","+Totalweight_txt.Text+",'" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" + DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "','Process')";
                        SqlCommand cmd211 = new SqlCommand(qry211, Database.con);
                        cmd211.ExecuteNonQuery();
                    
                    string qry212 = "insert into CreditDebit_tbl values ('" + memberid + "','StockReceived','Paddy'," + Totalweight_txt.Text + ",0,0,0,N'" + From_combo.Text + "','"+Name_txt.Text+"','"+Grade_combo.Text+"','" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" +DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "')";
                    SqlCommand cmd212 = new SqlCommand(qry212, Database.con);
                    cmd212.ExecuteNonQuery();

                    string qry213 = "select * from  Overallstock_tbl";
                    SqlCommand cmd213 = new SqlCommand(qry213, Database.con);
                    SqlDataReader rdr213=cmd213.ExecuteReader();
                   if(rdr213.Read())
                    {
                        string paddy = rdr213["paddy"].ToString();
                        decimal finalpaddy = Convert.ToDecimal(Totalweight_txt.Text) + Convert.ToDecimal(paddy);

                        string qry214 = "update Overallstock_tbl set paddy=" + finalpaddy + "";
                        SqlCommand cmd214 = new SqlCommand(qry214,Database.con);
                        cmd214.ExecuteNonQuery();

                    }
                   
                    string qry2 = "insert into StockSerialNo_tbl values(" + opno2 + ")";
                    SqlCommand cmd2 = new SqlCommand(qry2, Database.con);
                    cmd2.ExecuteNonQuery();
                    
                  /*  string opnoqry1 = "select top 1 serialno from IncomeSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd1 = new SqlCommand(opnoqry1, Database.con);
                    SqlDataReader opnordr1 = opnocmd1.ExecuteReader();
                    if (opnordr1.Read())
                    {
                        string no = opnordr1["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;

                    }

                    string get1 = memberid + " - " + Name_txt.Text;

                    string qry31 = "insert into Income_tbl values(" + opno2 + ",'StockReceived','" + DateTime.Today + "',N'" + get1 + "'," + Totalweight_txt.Text + ")";
                    SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                    cmd31.ExecuteNonQuery();

                    string qry21 = "insert into IncomeSerialNo_tbl values(" + opno2 + ")";
                    SqlCommand cmd21 = new SqlCommand(qry21, Database.con);
                    cmd21.ExecuteNonQuery();
                  */

                    string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','StockReceived','Stock:" + Name_txt.Text + "_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                    cmd3.ExecuteNonQuery();

                  //  printPreviewDialog1.Document = printDocument1;
                 //   printPreviewDialog1.ShowDialog();

                    MessageBox.Show("Stock Id " + OrderNo_txt.Text + " Saved Successfully...", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    number();
                    grid();

                 
                    Name_txt.Text = "";
                    From_combo.SelectedIndex = -1;
                    For_combo.SelectedIndex = -1;
                    Grade_combo.SelectedIndex = -1;
                    MemoNo_txt.Text = "";
                    Vehicle_txt.Text = "";
                    Noofbag_txt.Text = "";
                    Name_txt.Text = "";
                    Ton_txt.Text = "";
                    KiloGram_txt.Text = "";
                    Gram_txt.Text = "";
                    Totalweight_txt.Text = "";
                    From_combo.Focus();

                }
            }
            /*  }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message,messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);

              }*/

        }

        private void Gram_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {

                if (Ton_txt.Text == "")

                {
                    Ton_txt.Text = "0";
                }
                if (KiloGram_txt.Text == "")
                {
                    KiloGram_txt.Text = "0";
                }
                if (Gram_txt.Text == "")
                {
                    Gram_txt.Text = "0";
                }
                if (Totalweight_txt.Text == "")
                {
                    Totalweight_txt.Text = "0";
                }

                decimal totalweight=((Convert.ToDecimal(Ton_txt.Text)*1000)+Convert.ToDecimal(KiloGram_txt.Text));
                string weight=totalweight.ToString()+"."+ Gram_txt.Text;
                Totalweight_txt.Text=weight.ToString();
            }
        }

        private void Clear_btn_Click(object sender, EventArgs e)
        {
            Name_txt.Text = "";
            From_combo.SelectedIndex = -1;
            For_combo.SelectedIndex = -1;
            Grade_combo.SelectedIndex = -1;
            MemoNo_txt.Text = "";
            Vehicle_txt.Text = "";
            Noofbag_txt.Text = "";
            Name_txt.Text = "";
            Ton_txt.Text = "";
            KiloGram_txt.Text = "";
            Gram_txt.Text = "";
            Totalweight_txt.Text = "";
            From_combo.Focus();
            Add_btn.Enabled= true;
            Update_btn.Enabled= false;
            delete_btn.Enabled= false;
        }
        string getid = "";
        string orderid = "";
        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView1.Rows)
            {
                if (dr.Cells[0].Selected == true)
                {

                    getid = dr.Cells[0].Value.ToString();
                    orderid = dr.Cells[1].Value.ToString();

                    string qry = "select * from StockEntry_tbl where id='" + getid + "'";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    SqlDataReader rdr = cmd.ExecuteReader();
                    if (rdr.Read())
                    {
                        Name_txt.Text = rdr["name"].ToString();
                        From_combo.Text = rdr["stockfrom"].ToString();
                        For_combo.Text = rdr["stockfor"].ToString();
                        MemoNo_txt.Text = rdr["memono"].ToString();
                        Vehicle_txt.Text = rdr["vehicleno"].ToString();
                        Grade_combo.Text = rdr["grade"].ToString();
                        Noofbag_txt.Text = rdr["bag"].ToString();
                        Ton_txt.Text = rdr["ton"].ToString();
                        KiloGram_txt.Text = rdr["kilogram"].ToString();
                        Gram_txt.Text = rdr["gram"].ToString();
                        Totalweight_txt.Text = rdr["total"].ToString();
                        dateTimePicker1.Text = rdr["date"].ToString();

                      
                        Add_btn.Enabled = false;
                        Update_btn.Enabled = true;

                    }

                }
            }
        }

        private void delete_btn_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Do you want delete this Item?", messagename, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                if (getid != "")
                {

                    string qry = "delete from StockEntry_tbl where id='" + getid + "'";
                    SqlCommand cmd = new SqlCommand(qry, Database.con);
                    cmd.ExecuteNonQuery();

                  /*  string qry1 = "delete from MembershipPlan_tbl where memberid='" + memberid + "'";
                    SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                    cmd1.ExecuteNonQuery();
                  */

                    string get1 = orderid + " - " + Name_txt.Text;
                    string qry11 = "select * from Income_tbl where  description=N'" + get1 + "' and category='StockReceived'";
                    SqlCommand cmd11 = new SqlCommand(qry11, Database.con);
                    SqlDataReader rdr11 = cmd11.ExecuteReader();
                    if (rdr11.Read())
                    {

                        string id = rdr11["id"].ToString();

                        string qry31 = "delete from Income_tbl where description='" + get1 + "' and category='StockReceived' and id=" + id + "";
                        SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                        cmd31.ExecuteNonQuery();


                    }

                    //Moment Report;

                    string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','StockReceived','Stock:" + get1 + "_Delete','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                    cmd3.ExecuteNonQuery();

                    MessageBox.Show("Record Deleted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Name_txt.Text = "";
                    From_combo.SelectedIndex = -1;
                    For_combo.SelectedIndex = -1;
                    Grade_combo.SelectedIndex = -1;
                    MemoNo_txt.Text = "";
                    Vehicle_txt.Text = "";
                    Noofbag_txt.Text = "";
                    Name_txt.Text = "";
                    Ton_txt.Text = "";
                    KiloGram_txt.Text = "";
                    Gram_txt.Text = "";
                    Totalweight_txt.Text = "";
                    From_combo.Focus();
                    Add_btn.Enabled = true;
                    Update_btn.Enabled = false;
                    delete_btn.Enabled = false;

                    getid = "";
                    grid();

                }
                else
                {
                    MessageBox.Show("\r\nPlease select any one Row to Delete Member Details\r\n", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void Update_btn_Click(object sender, EventArgs e)
        {
            try
            {

                if (OrderNo_txt.Text == "" || Name_txt.Text == "" || Totalweight_txt.Text == "" || Grade_combo.Text == "")
                {
                    MessageBox.Show("Kindly Fill Primary Information !!!", messagename, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    

                    string qry3 = "update StockEntry_tbl set ton=" + Ton_txt.Text + ",kilogram=" + KiloGram_txt.Text + ",gram=" + Gram_txt.Text + ",total=" + Totalweight_txt.Text + ",memono='" + MemoNo_txt.Text + "',vehicleno='" + Vehicle_txt.Text + "',bag="+Noofbag_txt.Text+" where  id='" + getid + "'";
                    SqlCommand cmd1 = new SqlCommand(qry3, Database.con);
                    cmd1.ExecuteNonQuery();

                   
                    //Moment Report;
                    string qry31 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','StockReceived','Stock:" + Name_txt.Text + "_Update','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                    cmd31.ExecuteNonQuery();

                    MessageBox.Show("Successfully Updated.", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    Name_txt.Text = "";
                    From_combo.SelectedIndex = -1;
                    For_combo.SelectedIndex = -1;
                    Grade_combo.SelectedIndex = -1;
                    MemoNo_txt.Text = "";
                    Vehicle_txt.Text = "";
                    Noofbag_txt.Text = "";
                    Name_txt.Text = "";
                    Ton_txt.Text = "";
                    KiloGram_txt.Text = "";
                    Gram_txt.Text = "";
                    Totalweight_txt.Text = "";
                    From_combo.Focus();
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
                From_combo.SelectedIndex = -1;
                For_combo.SelectedIndex = -1;
                Grade_combo.SelectedIndex = -1;
                MemoNo_txt.Text = "";
                Vehicle_txt.Text = "";
                Noofbag_txt.Text = "";
                Name_txt.Text = "";
                Ton_txt.Text = "";
                KiloGram_txt.Text = "";
                Gram_txt.Text = "";
                Totalweight_txt.Text = "";
                From_combo.Focus();

                Add_btn.Enabled = true;
                Update_btn.Enabled = false;

                getid = "";
                grid();
            }

        }

        private void GAdd_btn_Click(object sender, EventArgs e)
        {
            Stock.GradeSetting GS = new GradeSetting(login);
            GS.Show();
        }

        private void GRefresh_btn_Click(object sender, EventArgs e)
        {
            Grade();

        }

        private void Fromadd_btn_Click(object sender, EventArgs e)
        {
          //  Supplier.PaddyReceiver PS=new Supplier.PaddySupplier(login);
          //  PS.Show();
        }

        private void FromRefresh_btn_Click(object sender, EventArgs e)
        {
            paddysupplier();
            From_combo.Focus();
        }

        private void ForAdd_btn_Click(object sender, EventArgs e)
        {
            Supplier.PaddyReceiver PS = new Supplier.PaddyReceiver(login);
            PS.Show();
        }

        private void ForRefresh_btn_Click(object sender, EventArgs e)
        {
            paddyreceiver();
            For_combo.Focus();
        }

        private void Noofbag_txt_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(Noofbag_txt.Text, "[^0-9]"))
            {
                MessageBox.Show("Please enter only numbers.");
                Noofbag_txt.Text = Noofbag_txt.Text.Remove(Noofbag_txt.Text.Length - 1);
            }
        }

        private void Print_btn_Click(object sender, EventArgs e)
        {

        }

        private void From_combo_SelectedIndexChanged(object sender, EventArgs e)
        {
            string qry = "select * from PaddySupplier_tbl where name='" + From_combo.Text + "'";
            SqlCommand cmd = new SqlCommand(qry, Database.con);
            SqlDataReader rdr = cmd.ExecuteReader();
            if (rdr.Read())
            {
                For_combo.Text = rdr["cusname"].ToString();

            }
            MemoNo_txt.Focus();
        }
    }
}
