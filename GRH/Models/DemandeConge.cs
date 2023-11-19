using System.Data;
using Microsoft.Data.SqlClient;

namespace GRH.Models;


public class DemandeConge
{
    public int idDemConge { get; set; }
    public Mpiasa mpiasa { get; set; }
    public DateTime debut { get; set; }
    public DateTime fin { get; set; }
    public Raison raison { get; set; }
    
    public int etat { get; set; }

    public DemandeConge(int idDemConge, Mpiasa mpiasa, DateTime debut, DateTime fin, Raison raison,int etat)
    {
        this.idDemConge = idDemConge;
        this.mpiasa = mpiasa;
        this.debut = debut;
        this.fin = fin;
        this.raison = raison;
        this.etat = etat;
    }

    public DemandeConge()
    {
    }
    
    public static List<DemandeConge> getAll(SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        List<DemandeConge> all = new List<DemandeConge>();
        String sql = "SELECT * FROM demandeConge";
        SqlCommand command = new SqlCommand(sql,con);
        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            DemandeConge c = new DemandeConge
            {
                idDemConge =  (int)reader["idDemConge"],
                debut = (DateTime)reader["debut"],
                fin = (DateTime)reader["fin"],
                mpiasa = new Mpiasa{matricule = (int)reader["idMpiasa"]},
                raison = new Raison{idRaison = (int)reader["idRaison"]},
                etat = (int)reader["etat"]
            };
            all.Add(c);
        }
        reader.Close();
        foreach (DemandeConge co in all)
        {
            co.mpiasa = Mpiasa.getById(co.mpiasa.matricule,con);
            co.raison = Raison.getById(co.raison.idRaison, con);
        }

        return all;
    }
    public static List<DemandeConge> getAllEnCours(SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        List<DemandeConge> all = new List<DemandeConge>();
        String sql = "SELECT * FROM demandeConge WHERE debut > GETDATE() and etat=0";
        SqlCommand command = new SqlCommand(sql,con);
        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            DemandeConge c = new DemandeConge
            {
                idDemConge =  (int)reader["idDemConge"],
                debut = (DateTime)reader["debut"],
                fin = (DateTime)reader["fin"],
                mpiasa = new Mpiasa{matricule = (int)reader["idMpiasa"]},
                raison = new Raison{idRaison = (int)reader["idRaison"]},
                etat = (int)reader["etat"]
            };
            all.Add(c);
        }
        reader.Close();
        foreach (DemandeConge co in all)
        {
            co.mpiasa = Mpiasa.getById(co.mpiasa.matricule,con);
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

        String sql = "INSERT INTO demandeConge(idMpiasa,debut,fin,idRaison,etat) VALUES("+this.mpiasa.matricule+",'"+this.debut+"','"+this.fin+"',"+this.raison.idRaison+",0)";
        SqlCommand command = new SqlCommand(sql,con);
        command.ExecuteNonQuery();
    }
}