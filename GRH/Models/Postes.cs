using System.Data;
using Microsoft.Data.SqlClient;

namespace GRH.Models
{
    public class Postes
    {

        public int idPoste { get; set; }

        public String nomPoste { get; set; }

        public Services service { get; set; }

        public Postes()
        {

        }

        public Postes(int idPoste, string nomPoste, Services service)
        {
            this.idPoste = idPoste;
            this.nomPoste = nomPoste;
            this.service = service;
        }

        public static List<Postes> getAllPoste(SqlConnection con)
        {
            var postes = new List<Postes>(); 
            if (con.State != ConnectionState.Open)
            {
                con = Connect.connectDB();
            }
            using (var connection = con) 
            {
                var command = new SqlCommand("SELECT * FROM postes", connection);
                using (var reader = command.ExecuteReader()) {
                    while (reader.Read()) {
                        var chaine = new Postes() {
                            idPoste = (int)reader["idPoste"],
                            nomPoste = (string)reader["nomPoste"],
                            service = Services.getById(con,(int)reader["idService"])
                        };
                        postes.Add(chaine);
                    } 
                    reader.Close();
                }
            }
            return postes;
        }
        public static Postes getById(int id,SqlConnection con)
        {
            var poste = new Postes();
            int idS = 0;
            if (con.State != ConnectionState.Open)
            {
                con = Connect.connectDB();
            }
            using (var connection = con) 
            {
                var command = new SqlCommand("SELECT * FROM postes WHERE idPoste="+id, connection);
                using (var reader = command.ExecuteReader()) {
                    while (reader.Read())
                    {
                        idS = (int)reader["idService"];
                        poste = new Postes() {
                            idPoste = (int)reader["idPoste"],
                            nomPoste = (string)reader["nomPoste"],
                        };
                        
                    } 
                    reader.Close();
                }

                poste.service = Services.getById( connection,idS);
            }
            return poste;
        }
        public static List<Postes> getAllPosteByService(int idService, SqlConnection con)
        {
            var postes = new List<Postes>(); 
            
            if (con.State != ConnectionState.Open)
            {
                con = Connect.connectDB();
            }
            using (var connection = con) 
            {
                var command = new SqlCommand("SELECT * FROM postes WHERE idService=" + idService, connection);
                using (var reader = command.ExecuteReader()) 
                {
                    try
                    {
                        while (reader.Read())
                        {
                            var chaine = new Postes()
                            {
                                idPoste = (int)reader["idPoste"],
                                nomPoste = (string)reader["nomPoste"],
                            };
                            postes.Add(chaine);
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }
                    finally
                    {
                        reader.Close(); 
                    }
                   
                    
                }
            }
            return postes;
        }

        
    }
}
