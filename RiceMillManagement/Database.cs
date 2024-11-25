using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RiceMillManagement
{
    internal class Database
    {
        public static SqlConnection con = null;
        public static SqlConnection con1 = null;
        public void db()
        {
            try
            {
                // Original


              //  con = new SqlConnection("Data Source=DESKTOP-R89FPOG;Initial Catalog=RiceMillManagement;Integrated Security=true;MultipleActiveResultSets=True");
                con = new SqlConnection("Data Source=TOPNI;Initial Catalog=RiceMillManagement_Recovered;Integrated Security=true;MultipleActiveResultSets=True");
               // con = new SqlConnection("Data Source=DESKTOP-CE3IGS2;Initial Catalog=RiceMillManagement;Integrated Security=true;MultipleActiveResultSets=True");
                con1 = new SqlConnection("Data Source=DESKTOP-CE3IGS2;Initial Catalog=ComplaintManagement;User Id=logsr;Password=1234;MultipleActiveResultSets=True");

                //  con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=|DataDirectory|\GymManagement.mdf;Integrated Security=True;MultipleActiveResultSets=True;");
                //  con1 = new SqlConnection("Data Source=103.48.180.245;Initial Catalog=BuypClientsOffline;User Id=logsr;Password=1234;MultipleActiveResultSets=True");

                //con1 = new SqlConnection("Data Source=buyp.database.windows.net;Initial Catalog=BuypClientsOffline;User Id=logsr;Password=#SuperFastAdmin3;MultipleActiveResultSets=True");
                
                con.Open();

                bool connection = NetworkInterface.GetIsNetworkAvailable();
                if (connection == true)
                {
                    try
                    {
                       // con1.Open();
                    }
                    catch
                    {

                    }
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
    }
}
