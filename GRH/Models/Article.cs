using System.Data;
using Microsoft.Data.SqlClient;

namespace GRH.Models;

public class Article
{
    public int IdArticle { get; set; }
        public string Reference { get; set; }
        public string NomArticle { get; set; }
        public Unite unite { get; set; }

        public static List<Article> GetAll(SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            List<Article> articles = new List<Article>();
            string query = "SELECT * FROM Article";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Article article = new Article
                        {
                            IdArticle = Convert.ToInt32(reader["idArticle"]),
                            Reference = reader["reference"].ToString(),
                            NomArticle = reader["nomArticle"].ToString(),
                            unite = new Unite
                            {
                                IdUnite = (int)reader["idUnite"]
                            }
                        };
                        articles.Add(article);
                    }
                    reader.Close();
                }
            }

            foreach (var a in articles)
            {
                a.unite = Unite.GetById(a.unite.IdUnite, con);
            }

            return articles;
        }

        public static Article GetById(int id, SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            Article article = null;
            string query = "SELECT * FROM Article WHERE idArticle = @Id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        article = new Article
                        {
                            IdArticle = Convert.ToInt32(reader["idArticle"]),
                            Reference = reader["reference"].ToString(),
                            NomArticle = reader["nomArticle"].ToString(),
                            unite = new Unite
                            {
                                IdUnite = (int)reader["idUnite"]
                            }
                        };
                    }
                    reader.Close();
                }
            } 
            article.unite = Unite.GetById(article.unite.IdUnite, con);
            return article;
        }
}