using System;
using System.Data;
using System.Data.SqlClient;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;

namespace GRH.Models
{
    public class TypeContrat
    {
        public int idTypeContrat { get; set; }
        public string nomType { get; set; }

        public TypeContrat()
        {
            
        }
        public TypeContrat(int idTypeContrat, string nomType)
        {
            this.idTypeContrat = idTypeContrat;
            this.nomType = nomType;
        }

        // Méthode pour récupérer tous les types de contrat
        public static List<TypeContrat> getAll(SqlConnection connection)
        {

            if (connection == null || connection.State == ConnectionState.Closed)
            {
                connection = Connect.connectDB();
            }
            
            List<TypeContrat> typeContrats = new List<TypeContrat>();

            using (SqlCommand command = new SqlCommand("SELECT * FROM typeContrat", connection))
            {
              
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int idTypeContrat = reader.GetInt32(0);
                        string nomType = reader.GetString(1);

                        TypeContrat typeContrat = new TypeContrat(idTypeContrat, nomType);
                        typeContrats.Add(typeContrat);
                    }
                    reader.Close();
                }
            }

            

            return typeContrats;
        }
        
        public static TypeContrat getById(int id, SqlConnection connection)
        
        {
            if (connection == null || connection.State == ConnectionState.Closed)
            {
                connection = Connect.connectDB();
            }
            using (SqlCommand command = new SqlCommand("SELECT * FROM typeContrat WHERE idTypeContrat = @id", connection))
            {
                command.Parameters.AddWithValue("@id", id);
              
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        int idTypeContrat = reader.GetInt32(0);
                        string nomType = reader.GetString(1);
                        return new TypeContrat(idTypeContrat, nomType);
                    }
                    reader.Close();
                }
            }

            // Retourne null si le type de contrat n'est pas trouvé
            return null;
        }
    }
}
