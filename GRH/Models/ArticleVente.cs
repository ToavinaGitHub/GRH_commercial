using Microsoft.Data.SqlClient;
using System.Data;

namespace GRH.Models
{
    public class ArticleVente
    {
        public int idArticleVente { get; set; }
        public String nomArticle { get; set; }

        public ArticleVente() { }
        public ArticleVente(int idArticleVente, string nomArticle)
        {
            this.idArticleVente = idArticleVente;
            this.nomArticle = nomArticle;
        }
        public static List<ArticleVente> GetAll(SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            List<ArticleVente> articleVentes = new List<ArticleVente>();
            string query = "SELECT * FROM ArticleVente";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ArticleVente article = new ArticleVente
                        {
                            idArticleVente = Convert.ToInt32(reader["idArticleVente"]),
                            nomArticle = reader["nomArticle"].ToString(),
                           
                            
                        };
                        articleVentes.Add(article);
                    }
                    reader.Close();
                }
               
            }
            return articleVentes;
        }

        public static ArticleVente GetById(int id, SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            ArticleVente article = null;
            string query = "SELECT * FROM ArticleVente WHERE idArticleVente = @Id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        article = new ArticleVente
                        {
                            idArticleVente = Convert.ToInt32(reader["idArticleVente"]),
                            nomArticle = reader["nomArticle"].ToString(),
                            
                        };
                    }
                    reader.Close();
                }
            }
           
            return article;
        }

    }
    
}
