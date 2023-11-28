using Microsoft.Data.SqlClient;

namespace GRH.Models
{
    public class ArticleVente
    {
        public int idArticleVente { get; set; }

        public string nomArticleVente { get; set; }

        public string nomUnite { get; set; }

        public double puHT { get; set; }

        public double tva { get; set; }  

        public ArticleVente() { }

        public ArticleVente(int idArticleVente, string nomArticleVente, string nomUnite)
        {
            this.idArticleVente = idArticleVente;
            this.nomArticleVente = nomArticleVente;
            this.nomUnite = nomUnite;
        }


        public ArticleVente getArticleById(int idArticle)
        {
            SqlConnection co;
            Connect new_co = new Connect();
            co = Connect.connectDB();
            ArticleVente article = null;
            SqlCommand command = new SqlCommand("select a.idArticleVente,a.nomArticleVente,u.nomUnite from ArticleVente a join uniteArticle u on a.idArticleVente=u.idArticleVente where a.idArticleVente=" + idArticle + "", co);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string nom = (string)reader["nomArticleVente"];
                string nomUnite = (string)reader["nomUnite"];
                article = new ArticleVente(idArticle,nom, nomUnite);
                article.tva = 20;
            }
            reader.Close();
            co.Close();
            article.puHT = this.getPrix(idArticle);
            return article;
        }

        public double getPrix(int idArticle)    
        {
            SqlConnection co;
            Connect new_co = new Connect();
            co = Connect.connectDB();
            double prix = 0;
            SqlCommand command = new SqlCommand("select TOP(1) * from prixArticle where idArticleVente="+idArticle+"", co);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                prix = (double)reader["prix"];
            }
            reader.Close();
            co.Close();
            return prix;
        }
    }
}
