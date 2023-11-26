using Microsoft.Data.SqlClient;

namespace GRH.Models
{
    public class Reponse
    {
        public int idReponse { get; set; }
        public Question question { get; set; }
        public String nom { get; set; }
        public int etat { get; set; }

        public Reponse() { }
        public Reponse(int idReponse, Question question, string nom, int etat)
        {
            this.idReponse = idReponse;
            this.question = question;
            this.nom = nom;
            this.etat = etat;
        }

        public Reponse(int idReponse, string nom, int etat)
        {
            this.idReponse = idReponse;
            this.nom = nom;
            this.etat = etat;
        }

        public List<List<Reponse>> getReponses(SqlConnection co, List<Question> questions)
        {
            if (co == null)
            {
                co = Connect.connectDB();
            }
            List<List<Reponse>> reponses = new List<List<Reponse>>();
            foreach (Question question in questions)
            {
                List<Reponse> rep = new List<Reponse>();
                SqlCommand command = new SqlCommand("select * from reponse where idQuestion= " + question.idQuestion + "", co);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    int idReponse = (int)reader["idReponse"];
                    string nom = (string)reader["nom"];
                    int etat = (int)reader["etat"];
                    Reponse reponse = new Reponse(idReponse, nom, etat);
                    reponse.question = question;
                    rep.Add(reponse);
                }
                reader.Close();
                reponses.Add(rep);
            }
            return reponses;
        }

        public static List<Reponse> getReponseQuestion(Question q, SqlConnection co)
        {
            if (co == null)
            {
                co = Connect.connectDB();
            }
            List<Reponse> reponses = new List<Reponse>();
            SqlCommand command = new SqlCommand("select * from reponse where idQuestion= " + q.idQuestion + "", co);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int idReponse = (int)reader["idReponse"]; 
                string nom = (string)reader["nom"];
                int etat = (int)reader["etat"];
                Reponse reponse = new Reponse(idReponse, nom, etat); 
                reponse.question = q; 
                reponses.Add(reponse);
            }
            reader.Close();
            return reponses;
        }
    }
}