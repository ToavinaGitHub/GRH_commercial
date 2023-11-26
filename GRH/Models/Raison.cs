using System.Data;
using Microsoft.Data.SqlClient;

namespace GRH.Models;

public class Raison
{
    public int idRaison { get; set; }
    public String nom { get; set; }
    public double jourAns { get; set; }

    public Raison()
    {
    }

    public Raison(int idRaison, string nom, double jourAns)
    {
        this.idRaison = idRaison;
        this.nom = nom;
        this.jourAns = jourAns;
    }

    public static List<Raison> getAll(SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        List<Raison> all = new List<Raison>();
        String sql = "SELECT * FROM raison";
        SqlCommand command = new SqlCommand(sql,con);
        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Raison r = new Raison
            {
                idRaison = (int)reader["idRaison"],
                jourAns = (double)reader["jourAns"],
                nom = (String)reader["nom"]
            };
            all.Add(r);
        }
        reader.Close();
        return all;
    }
    public static Raison getById(int id,SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        List<Raison> all = new List<Raison>();
        String sql = "SELECT * FROM raison WHERE idRaison="+id;
        SqlCommand command = new SqlCommand(sql,con);
        SqlDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            Raison r = new Raison
            {
                idRaison = (int)reader["idRaison"],
                jourAns = (double)reader["jourAns"],
                nom = (String)reader["nom"]
            };
            return r;
        }
        reader.Close();
        return null;
    }
}