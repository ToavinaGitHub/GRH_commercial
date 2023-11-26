using System.Data;
using Microsoft.Data.SqlClient;

namespace GRH.Models;

public class ContratAvantage
{
    public int idContAvantage { get; set; }
    public Avantage avantage { get; set; }
    public Contrat contrat { get; set; }

    public ContratAvantage(int idContAvantage, Avantage avantage, Contrat contrat)
    {
        this.idContAvantage = idContAvantage;
        this.avantage = avantage;
        this.contrat = contrat;
    }

    public ContratAvantage()
    {
    }

    public static void insert(int idAvantage,int idContrat,SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        String sql = "INSERT INTO contratAvantage(idAvantage,idContrat) VALUES ("+idAvantage+","+idContrat+")";
        SqlCommand command = new SqlCommand(sql,con);
        command.ExecuteNonQuery();
    }

    public static List<ContratAvantage> getByContrat(int idContrat,SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        List<ContratAvantage> all = new List<ContratAvantage>();
        String sql = "SELECT * FROM contratAvantage WHERE idContrat="+idContrat;
        SqlCommand command = new SqlCommand(sql,con);
        SqlDataReader reader = command.ExecuteReader();
        
        while (reader.Read())
        {
            ContratAvantage a = new ContratAvantage
            {
                idContAvantage = (int)reader["idContAvantage"],
                avantage = new Avantage{ idAvantage = (int)reader["idAvantage"]},
                contrat = new Contrat{idContrat = (int)reader["idContrat"]}
            };
            all.Add(a);
        }
        reader.Close();

        foreach (var ca in all)
        {
            ca.avantage = Avantage.getById(ca.avantage.idAvantage,con);
            ca.contrat = Contrat.getById(ca.contrat.idContrat, con);
        }
        return all;
    }
}   