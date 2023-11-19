using System.Data;
using Microsoft.Data.SqlClient;

namespace GRH.Models;

public class Conge
{
    public int idConge { get; set; }
    public Mpiasa mpiasa { get; set; }
    public DateTime debut { get; set; }
    public DateTime fin { get; set; }
    public Raison raison { get; set; }

    public Conge(int idConge, Mpiasa mpiasa, DateTime debut, DateTime fin, Raison raison)
    {
        this.idConge = idConge;
        this.mpiasa = mpiasa;
        this.debut = debut;
        this.fin = fin;
        this.raison = raison;
    }

    public Conge()
    {
    }

    public static List<Conge> allByMpiasa(int idMpiasa,SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        List<Conge> all = new List<Conge>();
        String sql = "SELECT * FROM conge WHERE matricule="+idMpiasa;
        SqlCommand command = new SqlCommand(sql,con);
        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Conge c = new Conge
            {
                idConge = (int)reader["idConge"],
                debut = (DateTime)reader["debut"],
                fin = (DateTime)reader["fin"],
                mpiasa = new Mpiasa{matricule = idMpiasa},
                raison = new Raison{idRaison = (int)reader["idRaison"]}
                
            };
            all.Add(c);
        }
        reader.Close();
        foreach (Conge co in all)
        {
            co.mpiasa = Mpiasa.getById(idMpiasa,con);
            co.raison = Raison.getById(co.raison.idRaison, con);
        }

        return all;
    }
    public void save(SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        String sql = "INSERT INTO conge(idMpiasa,debut,fin,idRaison) VALUES("+this.mpiasa.matricule+",'"+this.debut+"','"+this.fin+"',"+this.raison.idRaison+")";
        SqlCommand command = new SqlCommand(sql,con);
        command.ExecuteNonQuery();
    }
}