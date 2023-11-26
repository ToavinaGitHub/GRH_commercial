using System.Data;
using Microsoft.Data.SqlClient;

namespace GRH.Models
{
    public class Services
    {
        public int idService { get; set; }
        public String nomService { get; set; }
        public String email { get; set; }
        public String mdp { get; set; }

        public Services()
        {

        }

        public Services(int idService, string nomService, string email, string mdp)
        {
            this.idService = idService;
            this.nomService = nomService;
            this.email = email;
            this.mdp = mdp;
        }
        
        public static List<Services> getAllService(SqlConnection con)
        {
            var service = new List<Services>(); 
            if (con.State != ConnectionState.Open)
            {
                con = Connect.connectDB();
            }
            using (var connection = con) 
            {
                var command = new SqlCommand("SELECT * FROM Services", connection);
                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        var serv = new Services() {
                            idService = (int)reader["idService"],
                            nomService = (string)reader["nomService"],
                            email = (String)reader["email"],
                            mdp = (String) reader["mdp"]
                        };
                        service.Add(serv);
                    } 
                    reader.Close(); 
                }
            }
            return service;
        }
        public static Services getById(SqlConnection con , int id)
        {
            var service = new List<Services>(); 
            if (con.State != ConnectionState.Open)
            {
                con = Connect.connectDB();
            }
            using (var connection = con) 
            {
               
                var command = new SqlCommand("SELECT * FROM Services WHERE idService="+id, connection);
                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        var serv = new Services() {
                            idService = (int)reader["idService"],
                            nomService = (string)reader["nomService"],
                            email = (String)reader["email"],
                            mdp = (String) reader["mdp"]
                        };
                        return serv;
                    } 
                    reader.Close(); 
                }
            }
            return null;
        }

        public static Services checkLogin(String email, String mdp, SqlConnection con)
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
                        var serv = new Services() {
                            idService = (int)reader["idService"],
                            nomService = (string)reader["nomService"],
                            email = (String)reader["email"],
                            mdp = (String) reader["mdp"]
                        };
                        return serv;
                    } 
                    reader.Close(); 
                }
            }
            return null;
        }
    }
}
