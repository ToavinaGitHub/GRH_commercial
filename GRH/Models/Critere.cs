using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;

namespace GRH.Models
{
    public class Critere
    {
        public int idCritere { get; set; }
        public String nomCritere { get; set; }

        public Critere() { }

        public Critere(int idCritere, string nomCritere)
        {
            this.idCritere = idCritere;
            this.nomCritere = nomCritere;
        }

        public static List<Critere> getAll(SqlConnection con)
        {
            var criteres = new List<Critere>();
            if (con.State != ConnectionState.Open)
            {
                con = Connect.connectDB();
            }
            using (var connection = con)
            {
                var command = new SqlCommand("SELECT * FROM Critere", connection);
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var critere = new Critere()
                        {
                            idCritere = (int)reader["idCritere"],
                            nomCritere = (string)reader["nomCritere"],
                        };
                        criteres.Add(critere);
                    }
                    reader.Close(); 
                }
            }
            return criteres;
        }

        public static Critere getById(SqlConnection con, int id)
        {
            if (con.State != ConnectionState.Open)
            {
                con = Connect.connectDB();
            }
            using (var connection = con)
            {
                var command = new SqlCommand("SELECT * FROM Critere WHERE idCritere = @id", connection);
                command.Parameters.AddWithValue("@id", id);

                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Critere()
                        {
                            idCritere = (int)reader["idCritere"],
                            nomCritere = (string)reader["nomCritere"],
                        };
                    }
                    reader.Close(); 
                }
            }
            return null; // Retourne null si aucun critère avec cet ID n'a été trouvé.
        }
    }
}
