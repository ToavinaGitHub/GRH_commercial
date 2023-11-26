using Microsoft.Data.SqlClient;
using System.Data;

namespace GRH.Models
{
    public class UniteArticle
    {
        public int idUniteArticle { get; set; }
        public ArticleVente articleVente { get; set; }
        public String nomUnite { get; set; }
        public double quantite { get; set; }

        public UniteArticle() { }

        public UniteArticle(int idUniteArticle, ArticleVente articleVente, string nomUnite, double quantite)
        {
            this.idUniteArticle = idUniteArticle;
            this.articleVente = articleVente;
            this.nomUnite = nomUnite;
            this.quantite = quantite;
        }

        public static List<UniteArticle> GetAll(SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            List<UniteArticle> listUnite = new List<UniteArticle>();
            string query = "SELECT * FROM UniteArticle";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        UniteArticle unite = new UniteArticle
                        {
                            idUniteArticle = Convert.ToInt32(reader["idUniteArticle"]),
                            nomUnite = reader["nomUnite"].ToString(),


                        };
                        listUnite.Add(unite);
                    }
                    reader.Close();
                }

            }
            return listUnite;
        }

        //public static List<UniteArticle> getAllByIdArticle(SqlConnection co, ArticleVente idArticleVente)
        //{
        //    if(co == null || co.State == ConnectionState.Closed)
        //    {
        //        co = Connect.connectDB();
        //    }
        //    String sql = "select * from UniteArticle where idArticleVente=@idArticleVente";
        //    SqlCommand command = new SqlCommand(sql, co);
        //    command.Parameters.AddWithValue("@idArticleVente",idArticleVente);
        //    List<UniteArticle> uniteArticles = new List<UniteArticle>();    
        //    using(SqlDataReader reader = command.ExecuteReader())
        //    {
        //        while (reader.Read())
        //        {
        //            UniteArticle articleVente = new UniteArticle
        //            {
        //                idUniteArticle = Convert.ToInt32(reader["idUniteArticle"]),
        //                articleVente = new ArticleVente
        //                {
        //                    idArticleVente = (int)reader["idArticleVente"]
        //                },
        //                nomUnite = reader["nomUnite"].ToString(),
        //                quantite = Convert.ToDouble(reader["quantite"])

        //            };

        //            uniteArticles.Add(articleVente);
        //        }
        //        reader.Close();
        //    }
        //    for (int i = 0; i < uniteArticles.Count; i++)
        //    {
        //        uniteArticles[i].articleVente = ArticleVente.GetById(uniteArticles[i].articleVente.idArticleVente, co);
        //    }
        //    return uniteArticles;
        //}

        public static List<UniteArticle> getAllByIdArticle(SqlConnection co, ArticleVente idArticleVente)
        {
            if (co == null || co.State == ConnectionState.Closed)
            {
                co = Connect.connectDB();
            }

            String sql = "SELECT * FROM UniteArticle WHERE idArticleVente = @idArticleVente";
            SqlCommand command = new SqlCommand(sql, co);

            command.Parameters.AddWithValue("@idArticleVente", idArticleVente.idArticleVente);

            List<UniteArticle> uniteArticles = new List<UniteArticle>();

            using (SqlDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    UniteArticle articleVente = new UniteArticle
                    {
                        idUniteArticle = Convert.ToInt32(reader["idUniteArticle"]),
                        articleVente = new ArticleVente
                        {
                            idArticleVente = (int)reader["idArticleVente"]
                        },
                        nomUnite = reader["nomUnite"].ToString(),
                        quantite = Convert.ToDouble(reader["quantite"])
                    };

                    uniteArticles.Add(articleVente);
                }
                reader.Close();
            }

            for (int i = 0; i < uniteArticles.Count; i++)
                   {
                        uniteArticles[i].articleVente = ArticleVente.GetById(uniteArticles[i].articleVente.idArticleVente, co);
                    }

                return uniteArticles;
        }

        public static UniteArticle GetById(int id, SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            UniteArticle unite = null;
            string query = "SELECT * FROM UniteArticle WHERE idUniteArticle = @Id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        unite = new UniteArticle
                        {
                            idUniteArticle = Convert.ToInt32(reader["idUniteArticle"]),
                            articleVente = new ArticleVente
                            {
                                idArticleVente = (int)reader["idArticleVente"]
                            },
                            nomUnite = reader["nomUnite"].ToString(),
                            quantite= Convert.ToDouble(reader["quantite"])
                        };
                    }
                    reader.Close();
                }
                unite.articleVente = ArticleVente.GetById(unite.articleVente.idArticleVente,con);
            }

            return unite;
        }
    }
}
