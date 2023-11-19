using System.Data;
using Microsoft.Data.SqlClient;

namespace GRH.Models
{
    public class RH
    {
        public int idRh { get; set; }

        public String email { get; set; }

        public String mdp { get; set; }

        public RH()
        {

        }

        public RH(int idRh, string email, string mdp)
        {
            this.idRh = idRh;
            this.email = email;
            this.mdp = mdp;
        }
        
        public static RH checkLogin(String email, String mdp, SqlConnection con)
        {
            if (con.State != ConnectionState.Open)
            {
                con = Connect.connectDB();
            }
            using (var connection = con) 
            {
                var command = new SqlCommand("SELECT * FROM Services WHERE email='"+email+"' and mdp='"+mdp+"'", connection);
                using (var reader = command.ExecuteReader()) {
                    if (reader.Read())
                    {
                        var rh = new RH() {
                            idRh = (int)reader["idService"],
                            email = (String)reader["email"],
                            mdp = (String) reader["mdp"]
                        };
                        return rh;
                    } 
                    reader.Close(); 
                }
            }
            return null;
        }
        
        public static RH GetById(int id,SqlConnection con)
        {
            // Assurez-vous d'avoir une connexion à votre base de données
            if (con.State != ConnectionState.Open)
            {
                con = Connect.connectDB();
            }
            using (SqlConnection connection = con)
            {

                string query = "SELECT * FROM RH WHERE idRH = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Créez un nouvel objet RH à partir des données de la base de données
                            return new RH
                            {
                                idRh = (int)reader["idRH"],
                                email = (string)reader["email"],
                                mdp = (string)reader["mdp"]
                            };
                        }
                        // Aucun enregistrement correspondant trouvé, retournez null
                        return null;
                        reader.Close();
                    }
                }
            }
        }
    }
}
