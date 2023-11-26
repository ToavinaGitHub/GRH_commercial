using Microsoft.Data.SqlClient;

namespace GRH.Models
{
    public class Connect
    {
        public static SqlConnection connectDB()
        {

            var datasource = @".\LENOVO";
            var database = "ServiceCo";

            string connString = @"Data Source=LENOVO;Initial Catalog="
                        + database + ";Persist Security Info=True; Trusted_Connection=True; TrustServerCertificate=True";

            SqlConnection conn = new SqlConnection(connString);
            
            try
            {
                conn.Open();
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
            }
            return conn;
        }
    }
}
