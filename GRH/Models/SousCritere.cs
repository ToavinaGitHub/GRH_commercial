using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace GRH.Models
{
    public class SousCritere
    {
        public int idSousCritere { get; set; }
        public Critere critere { get; set; }
        public String nomSousCritere { get; set; }

        public SousCritere() { }

        public SousCritere(int idSousCritere, Critere critere, string nomSousCritere)
        {
            this.idSousCritere = idSousCritere;
            this.critere = critere;
            this.nomSousCritere = nomSousCritere;
        }

        public static List<SousCritere> getAll(SqlConnection con)
        {
            var sousCriteres = new List<SousCritere>();
            List<int> idCri = new List<int>();
            if (con.State != ConnectionState.Open)
            {
                con = Connect.connectDB();
            }
            using (var connection = con)
            {
                var command = new SqlCommand("SELECT * FROM SousCritere", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var critereId = (int)reader["idCritere"];

                        var sousCritere = new SousCritere()
                        {
                            idSousCritere = (int)reader["idSousCritere"],
                            nomSousCritere = (string)reader["nomSousCritere"],
                        };
                        sousCriteres.Add(sousCritere);
                        idCri.Add(critereId);
                    }
                    // Fermez le DataReader ici, après la boucle while
                    reader.Close();
                }
                // Ensuite, vous pouvez appeler la fonction Critere.getById
                int count = 0;
                foreach (var sousCritere in sousCriteres)
                {
                    sousCritere.critere = Critere.getById(con, idCri[count]);
                    count++;
                }
            }
            return sousCriteres;
        }



        public static SousCritere getById(SqlConnection con, int id)
        {
            if (con.State != ConnectionState.Open)
            {
                con = Connect.connectDB();
            }
            using (var connection = con)
            {
               

                var command = new SqlCommand("SELECT * FROM SousCritere WHERE idSousCritere = @id", connection);
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        var critereId = (int)reader["idCritere"];
                        var critere = Critere.getById(connection, critereId); // Obtenez le critère associé

                        return new SousCritere()
                        {
                            idSousCritere = (int)reader["idSousCritere"],
                            critere = critere,
                            nomSousCritere = (string)reader["nomSousCritere"],
                        };
                    }
                }
            }
            return null; // Retourne null si aucun sous-critère avec cet ID n'a été trouvé.
        }
        public static List<SousCritere> getByIdCritere(SqlConnection con, int id)
        {
            var sousCriteres = new List<SousCritere>();
            if (con.State != ConnectionState.Open)
            {
                con = Connect.connectDB();
            }
            using (var connection = con)
            {

                String sql = "SELECT * FROM SousCritere WHERE idCritere =" + id;
                Console.Write(sql+" --");
                var command = new SqlCommand(sql, connection);
               

                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var critereId = (int)reader["idCritere"];
                       // var critere = Critere.getById(connection, critereId); // Obtenez le critère associé

                        SousCritere s =  new SousCritere()
                        {
                            idSousCritere = (int)reader["idSousCritere"],
                           // critere = critere,
                            nomSousCritere = (string)reader["nomSousCritere"],
                        };
                    
                        sousCriteres.Add(s);
                    }
                }
                
                return sousCriteres;
            }
            return null; // Retourne null si aucun sous-critère avec cet ID n'a été trouvé.
        }
        public List<SousCritere> getDetailsByCv(SqlConnection co, int idCv)
        {
            if (co == null)
            {
                co = Connect.connectDB();
            }
            Critere critere = null;
            SousCritere sousCritere = null;
            List<SousCritere> list= new List<SousCritere>();
            SqlCommand command = new SqlCommand("SELECT * FROM v_detailcv where idCv=" + idCv + "", co);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int idCritere = (int)reader["idCritere"];
                string nomCritere = (string)reader["nomCritere"];
                critere= new Critere(idCritere, nomCritere);
                int idSousCritere = (int)reader["idSousCritere"];
                string nomSousCritere = (string)reader["nomSousCritere"];
                sousCritere = new SousCritere(idSousCritere, critere, nomSousCritere);
                list.Add(sousCritere);          
            }
            reader.Close();
            return list;
        }
    }
}
