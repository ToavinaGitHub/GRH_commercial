using System.Data;
using Microsoft.Data.SqlClient;
        
namespace GRH.Models
{
    public class Annonce
    {
        public int idAnnonce {  get; set; }
        public Postes postes { get; set; }
        public String descriptions { get; set; }
        public int volumeTaches { get; set; }
        public int volumeJourHomme { get; set; }

        int nombreRecrue;
        public DateTime dateAnnonce { get; set; }
        public int etat { get; set; }
        
        //Durée (Determiné , indeterminé , temporaire)
        
        //Resaka periode d'essai 

        public void setNombreRecrue ()
        {
            double nombreRecrue = this.volumeTaches/this.nombreRecrue;
            
            this.nombreRecrue = (int) Math.Round(nombreRecrue);
        }

        public int getNombreRecrue ()
        {
            return this.nombreRecrue;
        }

        public Annonce() { }

        public Annonce (int idAnnonce, Postes postes, string descriptions, int volumeTaches, int volumeJourHomme, DateTime dateAnnonce, int etat)
        {
            this.idAnnonce = idAnnonce;
            this.postes = postes;
            this.descriptions = descriptions;
            this.volumeTaches = volumeTaches;
            this.volumeJourHomme = volumeJourHomme;
            this.dateAnnonce = dateAnnonce;
            this.etat = etat;
        }

        
        public void Insert(SqlConnection connection)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection = Connect.connectDB();
                }

                string query = "INSERT INTO Annonce (idPoste, descriptions, volumeTache, volumeJourHomme, dateAnnonce, etat) " +
                               "VALUES ( @idPoste, @descriptions, @volumeTaches, @volumeJourHomme, @dateAnnonce, @etat)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                 
                    command.Parameters.AddWithValue("@idPoste", postes.idPoste); // Remplacez par la valeur appropriée
                    command.Parameters.AddWithValue("@descriptions", descriptions);
                    command.Parameters.AddWithValue("@volumeTaches", volumeTaches);
                    command.Parameters.AddWithValue("@volumeJourHomme", volumeJourHomme);
                    command.Parameters.AddWithValue("@dateAnnonce", dateAnnonce);
                    command.Parameters.AddWithValue("@etat", etat);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Insertion réussie !");
                    }
                    else
                    {
                        Console.WriteLine("Aucune ligne n'a été insérée.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'insertion : " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        public static Annonce getLastAnnonce(SqlConnection connection)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection = Connect.connectDB();
                }

                string query = "SELECT TOP 1 * FROM Annonce ORDER BY dateAnnonce DESC";

                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Créez et retournez l'objet Annonce à partir des données de la base de données
                        Annonce lastAnnonce = new Annonce
                        {
                            idAnnonce = (int)reader["idAnnonce"],
                            postes = new Postes { idPoste = (int)reader["idPoste"] }, // Vous devez ajuster la création du poste
                            descriptions = (string)reader["descriptions"],
                            volumeTaches = (int)reader["volumeTache"],
                            volumeJourHomme = (int)reader["volumeJourHomme"],
                            dateAnnonce = (DateTime)reader["dateAnnonce"],
                            etat = (int)reader["etat"]
                        };
                        return lastAnnonce;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la récupération de la dernière annonce : " + ex.Message);
            }
            finally  
            {
                connection.Close();
            }
            return null; // Retournez null si aucune annonce n'a été trouvée
        }
        public static List<Annonce> getAllAnnonce(SqlConnection con)
        {
            var annonces = new List<Annonce>();
            List<int> allIdPost = new List<int>();
            if (con.State != ConnectionState.Open)
            {
                con = Connect.connectDB();
            }
            using (var connection = con)
            {
                var command = new SqlCommand("SELECT *,volumeTache/volumeJourHomme recru FROM Annonce", connection);
                using (var reader = command.ExecuteReader())
                { 
                    while (reader.Read())
                    {
                        var ann = new Annonce()
                        {
                            idAnnonce = (int)reader["idAnnonce"],
                            descriptions = (String)reader["descriptions"],
                            volumeTaches = (int)reader["volumeTache"],
                            volumeJourHomme = (int)reader["volumeJourHomme"],
                            dateAnnonce = (DateTime)reader["dateAnnonce"],
                            etat = (int)reader["etat"],
                            nombreRecrue = (int)reader["recru"]
                        };
                        annonces.Add(ann);
                         allIdPost.Add((int)reader["idPoste"]);
                    }
                    reader.Close();
                    int count = 0;    
                    foreach (Annonce a in annonces)
                    {
                        a.postes = Postes.getById(allIdPost[count],con);
                        count++;
                    }
                }
            }
            return annonces;           
        }
        public static List<Annonce> getAnnonceDispo(SqlConnection con)
        {
            var annonces = new List<Annonce>();
            List<int> allIdPost = new List<int>();
            if (con.State != ConnectionState.Open)
            {
                con = Connect.connectDB();
            }
            using (var connection = con)
            {
                var command = new SqlCommand("SELECT *,volumeTache/volumeJourHomme recru FROM Annonce WHERE etat=0", connection);
                using (var reader = command.ExecuteReader())
                { 
                    while (reader.Read())
                    {
                        var ann = new Annonce()
                        {
                            idAnnonce = (int)reader["idAnnonce"],
                            descriptions = (String)reader["descriptions"],
                            volumeTaches = (int)reader["volumeTache"],
                            volumeJourHomme = (int)reader["volumeJourHomme"],
                            dateAnnonce = (DateTime)reader["dateAnnonce"],
                            etat = (int)reader["etat"],
                            nombreRecrue = (int)reader["recru"]
                        };
                        annonces.Add(ann);
                        allIdPost.Add((int)reader["idPoste"]);
                    }
                    reader.Close();
                    int count = 0;    
                    foreach (Annonce a in annonces)
                    {
                        a.postes = Postes.getById(allIdPost[count],con);
                        count++;
                    }
                }
            }
            return annonces;           
        }
        public void cloturerAnnonce(SqlConnection co, int idAnnonce)
        {
            if (co == null)
            {
                co = Connect.connectDB();
            }
            String querry = "update annonce set etat=1 where idAnnonce=" + idAnnonce + "";
            SqlCommand command = new SqlCommand(querry, co);
            command.ExecuteNonQuery();
        }

        public static Annonce getAnnonceById(SqlConnection connection,int idAnnonce)
        {
            if (connection == null)
            {
                connection = Connect.connectDB();
            }
            try
            {
                string query = "SELECT TOP 1 *,volumeTache/volumeJourHomme recru FROM Annonce WHERE idAnnonce="+idAnnonce;

                Console.WriteLine(query);
                Annonce lastAnnonce = new Annonce();
                int idP = 0;
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        // Créez et retournez l'objet Annonce à partir des données de la base de données
                        idP = (int)reader["idPoste"];
                        lastAnnonce = new Annonce
                        {
                            idAnnonce = (int)reader["idAnnonce"],
                            postes = new Postes { idPoste = (int)reader["idPoste"] }, // Vous devez ajuster la création du poste
                            descriptions = (string)reader["descriptions"],
                            volumeTaches = (int)reader["volumeTache"],
                            volumeJourHomme = (int)reader["volumeJourHomme"],
                            dateAnnonce = (DateTime)reader["dateAnnonce"],
                            etat = (int)reader["etat"],
                            nombreRecrue = (int)reader["recru"]
                        };
                        
                       
                    }
                    reader.Close();
                    lastAnnonce.postes = Postes.getById(idP,connection);
                    return lastAnnonce;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de la récupération de la dernière annonce : " + ex.Message);
            }
            return null;
        }

        public static List<int> getTesteAfaire(int idClient, SqlConnection con)
        {
            if (con == null)
            {
                con = Connect.connectDB();
            }

            List<int> res = new List<int>();
            String sql = "SELECT * FROM etatClient WHERE etat=1 and idClient="+idClient;
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int idAnnonce = (int)reader["idAnnonce"];
                res.Add(idAnnonce);
            }
            return res;
        }
        
        public static List<int> getEntretienAFaire(int idClient, SqlConnection con)
        {
            if (con == null)
            {
                con = Connect.connectDB();
            }

            List<int> res = new List<int>();
            String sql = "SELECT * FROM etatClient WHERE etat=2 and dateEntretien>GETDATE() and idClient="+idClient;
            SqlCommand command = new SqlCommand(sql, con);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int idAnnonce = (int)reader["idAnnonce"];
                res.Add(idAnnonce);
            }
            return res;
        }

        public static bool estPostule(int idAnnonce, int idClient, SqlConnection con)
        {
            if (con == null)
            {
                con = Connect.connectDB();
            }

            String sql = "SELECT * FROM etatClient WHERE idClient="+idClient+" and idAnnonce="+idAnnonce;

            SqlCommand command = new SqlCommand(sql,con);

            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                return true;
            }
            reader.Close();
            return false;
        }
    }
}
