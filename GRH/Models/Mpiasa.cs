using System.Data;
using Microsoft.Data.SqlClient;

namespace GRH.Models;

public class Mpiasa
{
    public int matricule { get; set; }
    public Clients clients { get; set; }
    public DateTime entre { get; set; }
    public DateTime sortie { get; set; }
    
    public Postes postes { get; set; }

    public Mpiasa()
    {
    }

    public Mpiasa(int matricule, Clients clients, DateTime entre, DateTime sortie,Postes postes)
    {
        this.matricule = matricule;
        this.clients = clients;
        this.entre = entre;
        this.sortie = sortie;
        this.postes = postes;
    }

    public void saveMpiasa(SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        String sql = "INSERT INTO mpiasa(idClient,entre,sortie,idPoste) VALUES("+this.clients.idClient+",'"+this.entre+"',NULL,"+this.postes.idPoste+")";
        SqlCommand command = new SqlCommand(sql,con);
        command.ExecuteNonQuery();
    }
    public static List<Mpiasa> getAll(SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }
        List<Mpiasa> all = new List<Mpiasa>();
        String sql = "SELECT * FROM mpiasa";
        SqlCommand command = new SqlCommand(sql,con);
        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Mpiasa mp = new Mpiasa();
            mp.matricule = (int)reader["matricule"];
            mp.clients = new Clients { idClient = (int)reader["idClient"] };
            mp.entre = (DateTime)reader["entre"];
            try
            {
                mp.sortie = (DateTime)reader["sortie"];
            }
            catch (Exception e)
            {
                mp.sortie = new DateTime();
            }
          
            mp.postes = new Postes { idPoste = (int)reader["idPoste"]};
            all.Add(mp);
        }
        reader.Close();
        
        foreach (Mpiasa mp in all)
        {
            mp.postes = Postes.getById(mp.postes.idPoste, con);
            mp.clients = Clients.getClientsById(mp.clients.idClient,con);
        }

        return all;
    }
    public static Mpiasa getById(int matricule,SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        Mpiasa mpi = null;
        String sql = "SELECT * FROM mpiasa WHERE matricule="+matricule;
        SqlCommand command = new SqlCommand(sql,con);
        SqlDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            mpi = new Mpiasa();
            mpi.matricule = (int)reader["matricule"];
            mpi.clients = new Clients { idClient = (int)reader["idClient"] };
            mpi.entre = (DateTime)reader["entre"];
            try
            {
                mpi.sortie = (DateTime)reader["sortie"];
            }
            catch (Exception e)
            {
                mpi.sortie = new DateTime();
            }
            mpi.postes = new Postes { idPoste = (int)reader["idPoste"]};
        }
        reader.Close();

        if (mpi!=null)
        {
            mpi.postes = Postes.getById(mpi.postes.idPoste, con);
            mpi.clients = Clients.getClientsById(mpi.clients.idClient, con);   
        }

        return mpi;
    }
    public static Mpiasa getByIdClient(int idClient,SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        Mpiasa mpi = null;
        String sql = "SELECT * FROM mpiasa WHERE idClient="+idClient;
        SqlCommand command = new SqlCommand(sql,con);
        SqlDataReader reader = command.ExecuteReader();
        if (reader.Read())
        {
            mpi = new Mpiasa();
            mpi.matricule = (int)reader["matricule"];
            mpi.clients = new Clients { idClient = (int)reader["idClient"] };
            mpi.entre = (DateTime)reader["entre"];
            try
            {
                mpi.sortie = (DateTime)reader["sortie"];
            }
            catch (Exception e)
            {
                mpi.sortie = new DateTime();
            }
           
            mpi.postes = new Postes { idPoste = (int)reader["idPoste"]};
        }
        reader.Close();

        if (mpi!=null)
        {
            mpi.postes = Postes.getById(mpi.postes.idPoste, con);
            mpi.clients = Clients.getClientsById(mpi.clients.idClient, con);   
        }

        return mpi;
    }

    public static bool estMpiasa(int idClient, SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        String sql = "SELECT * FROM mpiasa WHERE idClient="+idClient;
        SqlCommand command = new SqlCommand(sql,con);
        SqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            return true;
        }
        return false;
    }
    public  bool canMakaConge(SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        double day = 0;
        
        string sql = "SELECT CAST(DATEDIFF(MONTH, entre, GETDATE())*1.0 AS DECIMAL(18,2)) AS diff FROM mpiasa WHERE matricule = " + this.matricule;
        SqlCommand command = new SqlCommand(sql,con);
        SqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            decimal diff = (decimal)reader["diff"];
            day = Convert.ToDouble(diff); 
        }

        if (day>=12)
        {
            return true;
        }
        return false;
    }
    public double moisNiasana(SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        double months = 0;

        string sql = "SELECT CAST(DATEDIFF(MONTH, entre, GETDATE())*1.0 AS DECIMAL(18,2)) AS diff FROM mpiasa WHERE matricule = " + this.matricule;
        SqlCommand command = new SqlCommand(sql, con);
        SqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            decimal diff = (decimal)reader["diff"];
            months = Convert.ToDouble(diff); 
        }

        return months;
    }


    public double congeTokonyAnanany(SqlConnection con)
    {
        double mois = this.moisNiasana(con);
        double jMois = 2.5;

        return jMois * mois;
    }

    public double totalJourCongeNalainy(SqlConnection con)
    {

        double res = 0;
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        String sql = " SELECT SUM(CAST((DATEDIFF(HOUR, debut, fin) * 1.0) / 24 AS DECIMAL(18, 2))) as jour FROM conge WHERE idMpiasa="+this.matricule+" GROUP BY idMpiasa";
        SqlCommand command = new SqlCommand(sql,con);
        SqlDataReader reader = command.ExecuteReader();

        if (reader.Read())
        {
            
            decimal diff = (decimal)reader["jour"];
            res = Convert.ToDouble(diff);

        }

        return res;
    }

    public double congeRestant(SqlConnection con)
    {
        return this.congeTokonyAnanany(con) - this.totalJourCongeNalainy(con);
    }
}