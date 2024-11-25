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

namespace RiceMillManagement.Report
{
    public partial class OverallStock : Form
    {
        private string login;
        public OverallStock(string login)
        {
            InitializeComponent();Database dbs=new Database();
            dbs.db();
            this.login = login;
            stock();
        }
        void stock()
        {
            string paddycredit = "";
            string qry215 = "select sum(paddycredit) from  creditdebit_tbl where mode='StockReceived'";
            SqlCommand cmd215 = new SqlCommand(qry215, Database.con);
            SqlDataReader rdr215 = cmd215.ExecuteReader();
            if (rdr215.Read())
            {
                paddycredit = rdr215[0].ToString();
                StockReceived_txt.Text = paddycredit;
            }
            else
            {
                StockReceived_txt.Text = "0.00";
            }


            string paddydebit = "";
            string qry216 = "select sum(paddydebit) from  creditdebit_tbl where mode='CreateProduction'";
            SqlCommand cmd216 = new SqlCommand(qry216, Database.con);
            SqlDataReader rdr216 = cmd216.ExecuteReader();
            if (rdr216.Read())
            {
                paddydebit = rdr216[0].ToString();
                Productionuses_txt.Text =paddydebit.ToString();

            }
            else
            {
                Productionuses_txt.Text = "0.00";
            }

            //paddy selling

            string paddyselling = "";
            string qry217 = "select sum(paddycredit) from  creditdebit_tbl where mode='PaddySelling'";
            SqlCommand cmd217 = new SqlCommand(qry217, Database.con);
            SqlDataReader rdr217 = cmd217.ExecuteReader();
            if (rdr217.Read())
            {
                paddyselling = rdr217[0].ToString();
                PaddySell_txt.Text = paddyselling;
            }
            else
            {
                PaddySell_txt.Text = "0.00";
            }

            //Rice Purchase

            string RicePurchase = "";
            string qry218 = "select sum(ricecredit) from  creditdebit_tbl where mode='RicePurchase'";
            SqlCommand cmd218 = new SqlCommand(qry218, Database.con);
            SqlDataReader rdr218 = cmd218.ExecuteReader();
            if (rdr218.Read())
            {
                RicePurchase = rdr218[0].ToString();
                RicePur_txt.Text = RicePurchase;
            }
            else
            {
                RicePur_txt.Text = "0.00";
            }

            //No of Production

            string Production = "";
            string qry219 = "select count(*) from  ProductionEntry_tbl";
            SqlCommand cmd219 = new SqlCommand(qry219, Database.con);
            SqlDataReader rdr219 = cmd219.ExecuteReader();
            if (rdr219.Read())
            {
                Production = rdr219[0].ToString();
                Noofproduction_txt.Text = Production;
                
            }
            else
            {
                Noofproduction_txt.Text = "0.00";
            }

            //No of Delivery

            string delivery = "";
            string qry211= "select count(*) from  DeliveryEntry_tbl";
            SqlCommand cmd211 = new SqlCommand(qry211, Database.con);
            SqlDataReader rdr211 = cmd211.ExecuteReader();
            if (rdr211.Read())
            {
                delivery = rdr211[0].ToString();
                DeliveryNo_txt.Text = delivery;

            }
            else
            {
                DeliveryNo_txt.Text = "0.00";
            }
            //Paddy stock

            string qry213 = "select * from  Overallstock_tbl";
            SqlCommand cmd213 = new SqlCommand(qry213, Database.con);
            SqlDataReader rdr213 = cmd213.ExecuteReader();
            if (rdr213.Read())
            {
                string paddy = rdr213["paddy"].ToString();
                string rice = rdr213["totalrice"].ToString();

                CurrentPaddy_txt.Text = paddy;
                CurrentRice_txt.Text = rice;

            }
            else
            {
                CurrentPaddy_txt.Text = "0.00";
                CurrentRice_txt.Text = "0.00";
            }

            //Rice Output
            string riceoutput = "";
            string qry212 = "select sum(ricecredit) from  creditdebit_tbl where mode='RiceOutput'";
            SqlCommand cmd212 = new SqlCommand(qry212, Database.con);
            SqlDataReader rdr212 = cmd212.ExecuteReader();
            if (rdr212.Read())
            {
                riceoutput = rdr212[0].ToString();
                Riceoutput_txt.Text = riceoutput;

            }
            else
            {
                Riceoutput_txt.Text = "0.00";
            }

            // Delivery weight
            string ricedelivery = "";
            string qry210 = "select sum(ricedebit) from  creditdebit_tbl where mode='Delivery'";
            SqlCommand cmd210= new SqlCommand(qry210, Database.con);
            SqlDataReader rdr210 = cmd210.ExecuteReader();
            if (rdr210.Read())
            {
                ricedelivery = rdr210[0].ToString();
                Delivery_txt.Text = ricedelivery;

            }
            else
            {
                Delivery_txt.Text = "0.00";
            }

            //No of bag
            string totalbag = "";
            string qry1 = "select sum(bag) from  StockEntry_tbl";
            SqlCommand cmd1 = new SqlCommand(qry1, Database.con);
            SqlDataReader rdr1 = cmd1.ExecuteReader();
            if (rdr1.Read())
            {
                totalbag = rdr1[0].ToString();
                Totalbag_txt.Text = totalbag;
                
            }
            else
            {
                Totalbag_txt.Text = "0";
            }
            string productionbag = "";
            string qry2 = "select sum(bag) from  ProductionStatus_tbl where type='Production'";
            SqlCommand cmd2 = new SqlCommand(qry2, Database.con);
            SqlDataReader rdr2 = cmd2.ExecuteReader();
            if (rdr2.Read())
            {
                productionbag = rdr2[0].ToString();
                Productionbag_txt.Text = productionbag;

            }
            else
            {
                Productionbag_txt.Text = "0";
            }

            string Deliverybag = "";
            string qry3 = "select sum(bag) from  ProductionStatus_tbl where type='Delivery'";
            SqlCommand cmd3 = new SqlCommand(qry3, Database.con);
            SqlDataReader rdr3 = cmd3.ExecuteReader();
            if (rdr3.Read())
            {
                Deliverybag = rdr3[0].ToString();
                Packingbag_txt.Text = Deliverybag;
                
            }
            else
            {
                Packingbag_txt.Text = "0";
            }
            if(totalbag == "")
            {
                totalbag = "0";
            }
            if(Productionbag_txt.Text== "")
            {
                Productionbag_txt.Text = "0";
            }
            if(Packingbag_txt.Text=="")
            {
                Packingbag_txt.Text = "0";
            }
            decimal balancebag = Convert.ToDecimal(totalbag) - (Convert.ToDecimal(Productionbag_txt.Text) + Convert.ToDecimal(Packingbag_txt.Text));
            Bagcount_txt.Text+= balancebag;
            
        }
        private void Close_btn_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void guna2CustomGradientPanel3_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
