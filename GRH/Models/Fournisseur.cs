using System.Data;
using Microsoft.Data.SqlClient;

namespace GRH.Models;

public class Fournisseur
{
    public int IdFournisseur { get; set; }
    public string NomFournisseur { get; set; }
    public string Adresse { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Responsable { get; set; }
    

    public static List<Fournisseur> GetAll(SqlConnection con)
    {

        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }
        List<Fournisseur> fournisseurs = new List<Fournisseur>();
        string query = "SELECT * FROM Fournisseur";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Fournisseur fournisseur = new Fournisseur
                        {
                            IdFournisseur = Convert.ToInt32(reader["idFournisseur"]),
                            NomFournisseur = reader["nomFournisseur"].ToString(),
                            Adresse = reader["adresse"].ToString(),
                            Email = reader["email"].ToString(),
                            Phone = reader["phone"].ToString(),
                            Responsable = reader["responsable"].ToString()
                        };
                        fournisseurs.Add(fournisseur);
                    }
                    reader.Close();
                }
            }
        

        return fournisseurs;
    }

    public static Fournisseur GetById(int id,SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }
        Fournisseur fournisseur = null;
            string query = "SELECT * FROM Fournisseur WHERE idFournisseur = @Id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        fournisseur = new Fournisseur
                        {
                            IdFournisseur = Convert.ToInt32(reader["idFournisseur"]),
                            NomFournisseur = reader["nomFournisseur"].ToString(),
                            Adresse = reader["adresse"].ToString(),
                            Email = reader["email"].ToString(),
                            Phone = reader["phone"].ToString(),
                            Responsable = reader["responsable"].ToString()
                        };
                    }
                    reader.Close();
                }
            }

        return fournisseur;
    }
}