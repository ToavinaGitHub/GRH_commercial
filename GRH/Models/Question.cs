using Microsoft.Data.SqlClient;
using System.Linq;

namespace GRH.Models
{
    public class Question
    {
        public int idQuestion { get; set; }
        public String nom { get; set; }
        public int coef { get; set; }
        public Services services { get; set; }

        public Question() { }

        public Question (int idQuestion, string nom,int coef, Services services)
        {
            this.idQuestion = idQuestion;
            this.nom = nom;
            this.coef = coef;
            this.services = services;
        }

        public Question(int idQuestion, string nom, int coef)
        {
            this.idQuestion = idQuestion;
            this.nom = nom;
            this.coef = coef;
        }

        public List<Question> questionsByService(SqlConnection co, int idService)
        {
            if (co == null)
            {
                co = Connect.connectDB();
            }
            List<Question> list = new List<Question>();
            SqlCommand command = new SqlCommand("select * from question where idService=" + idService + "", co);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                int idQuestion = (int)reader["idQuestion"];
                String nom = (string)reader["nom"];
                int coef = (int)reader["coef"];
                Question question = new Question(idQuestion,nom,coef);
                list.Add(question);
            }
            reader.Close();
            Services service = new Services();
            for(int i=0; i<list.Count; i++)
            {
                list[i].services= Services.getById(co,idService);
            }
            return list;
        }

        public void insertQuestionTest(SqlConnection co, int idQuestion, int idAnnonce)
        {
            if (co == null)
            {
                
                co = Connect.connectDB();
            }
            String querry = "insert into questionAnnonce(idQuestion,idAnnonce) values (" + idQuestion + "," + idAnnonce + ")";
            Console.WriteLine(querry);
            SqlCommand command = new SqlCommand(querry, co);
            command.ExecuteNonQuery();
        }

        public List<Question> getRandomQuestions(List<Question> listeOriginale, int nombre)
        {
            if (nombre >= listeOriginale.Count)
            {
                return new List<Question>(listeOriginale);
            }
            List<Question> listeAleatoire = new List<Question>();
            Random random = new Random();
            while (listeAleatoire.Count < nombre)
            {
                int indiceAleatoire = random.Next(0, listeOriginale.Count);
                if (!listeAleatoire.Contains(listeOriginale[indiceAleatoire]))
                {
                    listeAleatoire.Add(listeOriginale[indiceAleatoire]);
                }
            }
            return listeAleatoire;
        }

        public List<Question> questionsByAnnonce(SqlConnection co, int idAnnonce)
        {
            if (co == null)
            {
                co = Connect.connectDB();
            }
            List<Question> list = new List<Question>();
            SqlCommand command = new SqlCommand("select * from questionAnnonce qa join question q on qa.idQuestion=q.idQuestion where idAnnonce=" + idAnnonce + "", co);
            SqlDataReader reader = command.ExecuteReader();
            int idService = 0;
            while (reader.Read())
            {
                int idQuestion = (int)reader["idQuestion"];
                String nom = (string)reader["nom"];
                int coef = (int)reader["coef"];
                idService = (int)reader["idService"];
                Question question = new Question(idQuestion, nom, coef);
                list.Add(question);
            }
            reader.Close();
          
            for (int i = 0; i < list.Count; i++)
            {
                list[i].services = Services.getById(co, idService);
            }
            return list;
        }
    }
}
