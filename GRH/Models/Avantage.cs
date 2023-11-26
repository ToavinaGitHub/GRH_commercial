using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.SqlClient;

namespace GRH.Models
{
    public class Avantage
    {
        public int idAvantage { get; set; }
        public string nomAvantage { get; set; }

        public Avantage()
        {
        }

        public Avantage(int idAvantage, string nomAvantage)
        {
            this.idAvantage = idAvantage;
            this.nomAvantage = nomAvantage;
        }

        // Méthode pour obtenir tous les avantages depuis la base de données
        public static List<Avantage> getAll(SqlConnection connection)
        {
            if (connection == null || connection.State == ConnectionState.Closed)
            {
                connection = Connect.connectDB();
            }

            List<Avantage> avantages = new List<Avantage>();

            using (SqlCommand command = new SqlCommand("SELECT idAvantage, nomAvantage FROM avantage", connection))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Avantage avantage = new Avantage
                        {
                            idAvantage = reader.GetInt32(0),
                            nomAvantage = reader.GetString(1)
                        };
                        avantages.Add(avantage);
                    }
                    reader.Close();
                }
            }

            return avantages;
        }

        // Méthode pour obtenir un avantage par son ID depuis la base de données
        public static Avantage getById(int id, SqlConnection connection)
        {
            if (connection == null || connection.State == ConnectionState.Closed)
            {
                connection = Connect.connectDB();
            }

            Avantage avantage = null;

            using (SqlCommand command = new SqlCommand("SELECT idAvantage, nomAvantage FROM avantage WHERE idAvantage = @id", connection))
            {
                command.Parameters.AddWithValue("@id", id);
               
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        avantage = new Avantage
                        {
                            idAvantage = reader.GetInt32(0),
                            nomAvantage = reader.GetString(1)
                        };
                    }
                    reader.Close();
                }
                
               
            }

            return avantage;
        }
    }
}
