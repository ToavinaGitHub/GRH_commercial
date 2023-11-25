using Microsoft.Data.SqlClient;
using System.Data;

namespace GRH.Models
{
    public class ClientVente
    {
        public int idClientVente { get; set; }
        public String nomClient { get; set; }
        public String email { get; set; }  
        public String mdp { get; set; }

        public ClientVente() { }

       public ClientVente(int idClientVente, string nom, string email, string mdp)
        {
            this.idClientVente = idClientVente;
            this.nomClient = nom;
            this.email = email;
            this.mdp = mdp;
        }

        public static ClientVente checkLogin(String email, String mdp, SqlConnection con)
        {
            if (con.State != ConnectionState.Open)
            {
                con = Connect.connectDB();
            }
            using (var connection = con)
            {
                var command = new SqlCommand("SELECT * FROM ClientVente WHERE email='" + email + "' and mdp='" + mdp + "'", connection);
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var cli = new ClientVente()
                        {
                            idClientVente = (int)reader["idClientVente"],
                            nomClient = (string)reader["nomClient"],
                            email = (String)reader["email"],
                            mdp = (String)reader["mdp"]
                        };
                        return cli;
                    }
                    reader.Close();
                }
            }
            return null;
        }
    }
}
