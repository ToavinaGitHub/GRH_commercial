using System.Data;
using Microsoft.Data.SqlClient;

namespace GRH.Models;

public class TypePayement
{
    
    public int IdTypePayement { get; set; }
    public string NomTypePayement { get; set; }
    
    public static List<TypePayement> GetAll(SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            List<TypePayement> typePayments = new List<TypePayement>();
            string query = "SELECT * FROM TypePayement";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        TypePayement typePayment = new TypePayement
                        {
                            IdTypePayement = Convert.ToInt32(reader["idTypePayement"]),
                            NomTypePayement = reader["nomTypePayement"].ToString(),
                        };
                        typePayments.Add(typePayment);
                    }
                    reader.Close();
                }
            }

            return typePayments;
        }

        public static TypePayement GetById(int id, SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            TypePayement typePayment = null;
            string query = "SELECT * FROM TypePayment WHERE idTypePayement = @Id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        typePayment = new TypePayement
                        {
                            IdTypePayement = Convert.ToInt32(reader["idTypePayement"]),
                            NomTypePayement = reader["nomTypePayement"].ToString(),
                        };
                    }
                    reader.Close();
                }
            }

            return typePayment;
        }
}