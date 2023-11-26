using System.Data;
using System.Drawing;
using Microsoft.Data.SqlClient;

namespace GRH.Models
{
    public class Clients
    {
        public int idClient {  get; set; }

        public String nom { get; set; }

        public String prenoms { get; set; }

        public DateTime dtn { get; set; }

        public String email { get; set; }  

        public String mdp { get; set; }

        public String adresse { get; set; }
        
        public String sexe;

        public int note { get; set; }
        public void setSexe(int sexe)
        {
            if(sexe == 0)
            {
                this.sexe = "Homme";
            }
            if(sexe == 1)
            {
                this.sexe = "Femme";
            }
        }

        public String getSexe()
        {
            return this.sexe;
        }

        public Clients() { }

        public Clients (int idClient, string nom, string prenoms, DateTime dtn, string email, string mdp, string adresse,int sexe)
        {
            this.idClient = idClient;
            this.nom = nom;
            this.prenoms = prenoms;
            this.dtn = dtn;
            this.email = email;
            this.mdp = mdp;
            this.adresse = adresse;
            this.setSexe(sexe);
        }
      
        public Clients getClientAnnonceByCv(SqlConnection co, int idAnnonce, int idCv)
        {
            if (co == null)
            {
                co = Connect.connectDB();
            }
            Clients client = null;
            SqlCommand command = new SqlCommand("SELECT * FROM v_clientAnnonce where idAnnonce="+idAnnonce+" and idCv="+idCv+"", co);
            Console.WriteLine("SELECT * FROM v_clientAnnonce where idAnnonce="+idAnnonce+" and idCv="+idCv);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int idClient = (int)reader["idClient"];
                string nom = (string)reader["nom"];
                string prenom = (string)reader["prenom"];
                DateTime dtn = (DateTime)reader["dtn"];
                string email = (string)reader["email"];
                string mdp = (string)reader["mdp"];
                string adresse = (string)reader["addresse"];
                int sexe = (int)reader["sexe"];
                client= new Clients(idClient,nom, prenom, dtn,email, mdp,adresse,sexe);
            }
            reader.Close();
            return client;
        }

        public int getNoteSexe(SqlConnection co, int idAnnonce)
        {
            if (co == null)
            {
                co = Connect.connectDB();
            }
            int note = 0;
                SqlCommand command = new SqlCommand("select * from v_notesexe where nomSousCritere='"+this.getSexe()+"' and idAnnonce="+idAnnonce+"",co);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                note = int.Parse(reader["coef"].ToString());
            }
            reader.Close();
            return note;
        }

        public void passageTest(SqlConnection co, DateTime dateTest,int idClient,int idAnnonce )
        {
            if (co == null)
            {
                co = Connect.connectDB();
            }
            String querry = "update etatClient set dateTeste='" + dateTest + "',etat=1 where idClient=" + idClient + " and idAnnonce = "+idAnnonce;
            Console.WriteLine(querry);
            SqlCommand command = new SqlCommand(querry, co);
            command.ExecuteNonQuery();
        }

        public static Clients checkLogin(String email, String mdp, SqlConnection co)
        {
            if (co == null)
            {
                co = Connect.connectDB();
            }

            String sql = "SELECT * FROM Clients WHERE email='"+email+"' and mdp='"+mdp+"'";
            SqlCommand command = new SqlCommand(sql,co);
            SqlDataReader reader = command.ExecuteReader();
            Clients client = null;
            if (reader.Read())
            {
                int idClient = (int)reader["idClient"];
                string nom = (string)reader["nom"];
                string prenom = (string)reader["prenom"];
                DateTime dtn = (DateTime)reader["dtn"];
                string mail = (string)reader["email"];
                string pass = (string)reader["mdp"];
                string adresse = (string)reader["addresse"];
                int sexe = (int)reader["sexe"];
                client= new Clients(idClient,nom, prenom, dtn,mail, pass,adresse,sexe);
            }
            return client;
        }

        public static Clients getClientsById(int id, SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            String sql = "SELECT * FROM Clients WHERE idClient = "+id;
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataReader reader = command.ExecuteReader();

            Clients client = new Clients();
            
            if (reader.Read())
            {
                int idClient = (int)reader["idClient"];
                string nom = (string)reader["nom"];
                string prenom = (string)reader["prenom"];
                DateTime dtn = (DateTime)reader["dtn"];
                string email = (string)reader["email"];
                string mdp = (string)reader["mdp"];
                string adresse = (string)reader["addresse"];
                int sexe = (int)reader["sexe"];
                client= new Clients(idClient,nom, prenom, dtn,email, mdp,adresse,sexe);
            }
            reader.Close();
            return client;
        }

        // 11 ny etat raha efa nanao test QCM 
        public static void updateEtatEfaTest(int idAnnonce,int idClient,SqlConnection con)
        {
            if (con == null)
            {
                con = Connect.connectDB();
            }
            String sql = "UPDATE etatClient SET etat=11 WHERE idAnnonce="+idAnnonce+" AND idClient="+idClient;
            SqlCommand command = new SqlCommand(sql,con);
            command.ExecuteNonQuery();
        }
        
        public static void updateNoteTest(int Cv,int note,SqlConnection con)
        {
            if (con == null)
            {
                con = Connect.connectDB();
            }
            String sql = "UPDATE note SET noteTeste="+note+" WHERE idCv="+Cv;
            SqlCommand command = new SqlCommand(sql,con);
            command.ExecuteNonQuery();
        }
        public static void updateNoteEntretien(int Cv,int note,SqlConnection con)
        {
            if (con == null)
            {
                con = Connect.connectDB();
            }
            String sql = "UPDATE note SET noteEntretien="+note+" WHERE idCv="+Cv;
            SqlCommand command = new SqlCommand(sql,con);
            command.ExecuteNonQuery();
        }

        public void insert(int sexe,SqlConnection con)
        {
            if (con == null)
            {
                con = Connect.connectDB();
            }

            String sql = "INSERT INTO Clients VALUES ('"+this.nom+"','"+this.prenoms+"','"+this.dtn+"','"+this.email+"','"+this.mdp+"','"+this.adresse+"',"+sexe+",0)";
            SqlCommand command = new SqlCommand(sql, con);
            command.ExecuteNonQuery();
        }
        
    }
}
