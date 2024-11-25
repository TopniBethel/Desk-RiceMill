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
using System.Runtime.Remoting.Lifetime;
using System.Drawing.Imaging;
using System.IO;

namespace RiceMillManagement.Delivery
{
    public partial class DeliveryReceipt : Form
    {
        private string login;
        public DeliveryReceipt(string login)
        {
            InitializeComponent(); Database dbs = new Database();
            dbs.db();
            this.login = login;
            grid();
            paddysupplier();
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
            SNo_txt.Text = "";
            string opnoqry = "select top 1 serialno from ReceiptSerialNo_tbl order by serialno desc";
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
        void grid()
        {
            //string qry1 = "select orderno as SNo,stockfor as Stockfor,productionid as Productionid,date as Date,LTRIM(RIGHT(convert(varchar,time,100),7)) as Time from ProductionEntry_tbl where status='Delivered' order by id desc";
            string qry1 = "select sno as SNo,stockfor as Stockfor,productionid as Productionid,date as Date,LTRIM(RIGHT(convert(varchar,time,100),7)) as Time from packingentry_tbl where status='Delivered' order by id desc";
            SqlDataAdapter adb1 = new SqlDataAdapter(qry1, Database.con);
            DataTable dt1 = new DataTable();
            adb1.Fill(dt1);
            dataGridView2.DataSource = dt1;

            
        }
        void paddysupplier()
        {
            try
            {
                Stockfor_combo.Items.Clear();
                string qry = "select * from PaddySupplier_tbl";
                SqlCommand cmd = new SqlCommand(qry, Database.con);
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    Stockfor_combo.Items.Add(rdr["name"]);

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
        string productionid = "";
        string orderid = "";
        string stockfor = "";
        private void dataGridView2_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            foreach (DataGridViewRow dr in dataGridView2.Rows)
            {
                if (dr.Cells[0].Selected == true)
                {
                    productionid = dr.Cells[2].Value.ToString();
                    orderid = dr.Cells[0].Value.ToString();
                    stockfor = dr.Cells[1].Value.ToString();
                    Productionid_txt.Text = productionid;
                    Stockfor_combo.Text = stockfor;
                    ReceiptId_txt.Focus();
                }
            }
        }

        private void Paid_txt_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode==Keys.Enter)
            {
                if(Total_txt.Text=="")
                {
                    Total_txt.Focus();
                }
                if(Paid_txt.Text=="")
                {
                    Paid_txt.Text = "0";
                }
                if(Balance_txt.Text=="")
                {
                    Balance_txt.Text = "0";
                }
                decimal balance=Convert.ToDecimal(Total_txt.Text)-Convert.ToDecimal(Paid_txt.Text);
                Balance_txt.Text=balance.ToString("0.00");


            }
        }

        private void Browse_btn_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog open = new OpenFileDialog();
                open.Filter = "Image Files(*.jpg; *.jpeg; *.gif; *.bmp)|*.jpg; *.jpeg; *.gif; *.bmp";
                if (open.ShowDialog() == DialogResult.OK)
                {
                    pictureBox1.Image = new Bitmap(open.FileName);
                    pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
                    // MessageBox.Show("successfully insert");

                }
            }
            catch (Exception)
            {
                throw new ApplicationException("Failed loading image");
            }
        }
        int sno;
        int opno2;
      
        SqlCommand cmd;
        void img1()
        {
            //converting photo to binary data
            if (pictureBox1.Image != null)
            {
                //using FileStream:(will not work while updating, if image is not changed)
                //FileStream fs = new FileStream(openFileDialog1.FileName, FileMode.Open, FileAccess.Read);
                //byte[] photo_aray = new byte[fs.Length];
                //fs.Read(photo_aray, 0, photo_aray.Length);  
                MemoryStream ms;
                //using MemoryStream:
                ms = new MemoryStream();
                pictureBox1.Image.Save(ms, ImageFormat.Jpeg);
                byte[] photo_aray = new byte[ms.Length];
                ms.Position = 0;
                ms.Read(photo_aray, 0, photo_aray.Length);
                cmd.Parameters.AddWithValue("@photo", photo_aray);
            }
            else
            {

            }
        }
        private void Add_btn_Click(object sender, EventArgs e)
        {
            //  try
            //   {
            if (SNo_txt.Text == "" || Productionid_txt.Text == "" || Total_txt.Text == "" || ReceiptId_txt.Text == "")
            {
                MessageBox.Show("Kindly Fill Primary Information !!!", messagename, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {

                string qry1 = "select * from DeliveryReceipt_tbl where serialno=" + SNo_txt.Text + "";
                SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
                SqlDataReader rdr1 = cmd1.ExecuteReader();
                if (rdr1.Read())
                {
                    MessageBox.Show("This Delivery Id Already Inserted", messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    string memberid = "";
                    string opnoqry = "select top 1 serialno from ReceiptSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd = new SqlCommand(opnoqry, Database.con);
                    SqlDataReader opnordr = opnocmd.ExecuteReader();
                    if (opnordr.Read())
                    {
                        string no = opnordr["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        sno = opno1 + 1;
                        memberid = sno.ToString();
                    }
                    if (pictureBox1.Image != null)
                    {
                        cmd = new SqlCommand("INSERT INTO DeliveryReceipt_tbl (serialno,productionid,name,amount,paid,balance,receiptno,date,time,status,image) values('" + memberid + "','" + Productionid_txt.Text + "',N'" + Stockfor_combo.Text + "'," + Total_txt.Text + "," + Paid_txt.Text + "," + Balance_txt.Text + ",'" + ReceiptId_txt.Text + "','" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" +DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") + "','Process',@photo)", Database.con);
                        img1();
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        string qry211 = "insert into DeliveryReceipt_tbl values ('" + memberid + "','" + Productionid_txt.Text + "',N'" + Stockfor_combo.Text + "'," + Total_txt.Text + "," + Paid_txt.Text + "," + Balance_txt.Text + ",'" + ReceiptId_txt.Text + "','" + Convert.ToDateTime(dateTimePicker1.Text).ToString("yyyy-MM-dd") + "','" +DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss tt") +"','Process','')";
                        SqlCommand cmd211=new SqlCommand(qry211, Database.con);
                             
                        cmd211.ExecuteNonQuery();
                    }

                    string qry214 = "update ProductionEntry_tbl set status='DeliveryReceipt' where productionid='" + Productionid_txt.Text + "'";
                    SqlCommand cmd214 = new SqlCommand(qry214, Database.con);
                    cmd214.ExecuteNonQuery();

                    string qry21 = "insert into ReceiptSerialNo_tbl values(" + sno + ")";
                    SqlCommand cmd21 = new SqlCommand(qry21, Database.con);
                    cmd21.ExecuteNonQuery();

                    
                /*    string opnoqry1 = "select top 1 serialno from IncomeSerialNo_tbl order by serialno desc";
                    SqlCommand opnocmd1 = new SqlCommand(opnoqry1, Database.con);
                    SqlDataReader opnordr1 = opnocmd1.ExecuteReader();
                    if (opnordr1.Read())
                    {
                        string no = opnordr1["serialno"].ToString();
                        int opno1 = Convert.ToInt32(no);
                        opno2 = opno1 + 1;

                    }

                    string get1 = memberid + " - " + Productionid_txt.Text;

                       string qry31 = "insert into Income_tbl values(" + opno2 + ",'RiceOutput','" + DateTime.Today + "',N'" + get1 + "'," + Totalweight_txt.Text + ")";
                       SqlCommand cmd31 = new SqlCommand(qry31, Database.con);
                       cmd31.ExecuteNonQuery();*/

                    string qry3 = "insert into LogReport_tbl values('" + login + "','" + DateTime.Now + "','DeliveryReceipt','ReceiptId:" + Productionid_txt.Text + "-" + stockfor + "_Insert','" + DateTime.Now.ToString("yyyy-MM-dd") + "')";
                    SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
                    cmd3.ExecuteNonQuery();

                    MessageBox.Show("Packing Id " + SNo_txt.Text + " Saved Successfully...", messagename, MessageBoxButtons.OK, MessageBoxIcon.Information);

                    number();
                    grid();

                    Productionid_txt.Text = "";
                    Stockfor_combo.SelectedIndex = -1;
                    //  comboBox1.SelectedIndex = -1;
                    //  Grade_combo.SelectedIndex = -1;
                    Total_txt.Text = "";
                    Paid_txt.Text = "";
                    Balance_txt.Text = "";
                    ReceiptId_txt.Text = "";

                    Productionid_txt.Focus();

                }
            }
            /*  }
              catch (Exception ex)
              {
                  MessageBox.Show(ex.Message,messagename, MessageBoxButtons.OK, MessageBoxIcon.Error);

              }*/
        }
    }
}
