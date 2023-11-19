using System.Data;
using Microsoft.Data.SqlClient;

namespace GRH.Models;

public class Unite
{
    public int IdUnite { get; set; }
    public string NomUnite { get; set; }

    public static List<Unite> GetAll(SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        List<Unite> unites = new List<Unite>();
        string query = "SELECT * FROM Unite";
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Unite unite = new Unite
                    {
                        IdUnite = Convert.ToInt32(reader["idUnite"]),
                        NomUnite = reader["nomUnite"].ToString(),
                    };
                    unites.Add(unite);
                }
                reader.Close();
            }
        }

        return unites;
    }

    public static Unite GetById(int id, SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        Unite unite = null;
        string query = "SELECT * FROM Unite WHERE idUnite = @Id";
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.AddWithValue("@Id", id);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    unite = new Unite
                    {
                        IdUnite = Convert.ToInt32(reader["idUnite"]),
                        NomUnite = reader["nomUnite"].ToString(),
                    };
                }
                reader.Close();
            }
        }

        return unite;
    }
}