using System.Data;
using Microsoft.Data.SqlClient;

namespace GRH.Models;

public class BesoinArticle
{
       public int IdBesoinArticle { get; set; }
        public double Quantite { get; set; }
        public int Etat { get; set; }
        public string Description { get; set; }
        public Article article { get; set; }
        public Services services { get; set; }

        public BesoinArticle()
        {
        }

        public BesoinArticle(int idBesoinArticle, double quantite, int etat, string description, Article article, Services services)
        {
            IdBesoinArticle = idBesoinArticle;
            Quantite = quantite;
            Etat = etat;
            Description = description;
            this.article = article;
            this.services = services;
        }

        public List<BesoinArticle> GetAll(SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            List<BesoinArticle> besoinArticles = new List<BesoinArticle>();
            string query = "SELECT * FROM BesoinArticle";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BesoinArticle besoinArticle = new BesoinArticle
                        {
                            IdBesoinArticle = Convert.ToInt32(reader["idBesoinArticle"]),
                            Quantite = (double)reader["quantite"],
                            Etat = Convert.ToInt32(reader["etat"]),
                            Description = reader["descri"].ToString(),
                            article = new Article
                            {
                                IdArticle = (int)reader["idArticle"]
                            },
                            services = new Services
                            {
                                idService = (int)reader["idService"]
                            }
                        };
                        besoinArticles.Add(besoinArticle);
                    }
                    reader.Close();
                }
            }

            foreach (var all in besoinArticles)
            {
                all.article = Article.GetById(all.article.IdArticle,con);
                all.services = Services.getById(con,all.services.idService);
            }
            return besoinArticles;
        }

        public static List<BesoinArticle> GetAllDejaValider(SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            List<BesoinArticle> besoinArticles = new List<BesoinArticle>();
            string query = "SELECT idArticle,SUM(quantite) qt FROM BesoinArticle WHERE etat=5 GROUP BY idArticle";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BesoinArticle besoinArticle = new BesoinArticle
                        {
                            Quantite = (double)reader["qt"],
                            article = new Article
                            {
                                IdArticle = (int)reader["idArticle"]
                            }
                        };
                        besoinArticles.Add(besoinArticle);
                    }
                    reader.Close();
                }
            }

            foreach (var all in besoinArticles)
            {
                all.article = Article.GetById(all.article.IdArticle,con);
            }
            return besoinArticles;
        }

        public static List<BesoinArticle> GetAllByService(int idService,SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            List<BesoinArticle> besoinArticles = new List<BesoinArticle>();
            string query = "SELECT * FROM BesoinArticle WHERE idService="+idService +" and etat = 0";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BesoinArticle besoinArticle = new BesoinArticle
                        {
                            IdBesoinArticle = Convert.ToInt32(reader["idBesoinArticle"]),
                            Quantite = (double)reader["quantite"],
                            Etat = Convert.ToInt32(reader["etat"]),
                            Description = reader["descri"].ToString(),
                            article = new Article
                            {
                                IdArticle = (int)reader["idArticle"]
                            },
                            services = new Services
                            {
                                idService = (int)reader["idService"]
                            }
                        };
                        besoinArticles.Add(besoinArticle);
                    }
                    reader.Close();
                }
            }

            foreach (var all in besoinArticles)
            {
                all.article = Article.GetById(all.article.IdArticle,con);
                all.services = Services.getById(con,all.services.idService);
            }
            return besoinArticles;
        }

        public BesoinArticle GetById(int id, SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            BesoinArticle besoinArticle = null;
            string query = "SELECT * FROM BesoinArticle WHERE idBesoinArticle = @Id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        besoinArticle = new BesoinArticle
                        {
                            IdBesoinArticle = Convert.ToInt32(reader["idBesoinArticle"]),
                            Quantite = (double)reader["quantite"],
                            Etat = Convert.ToInt32(reader["etat"]),
                            Description = reader["descri"].ToString(),
                            article = new Article
                            {
                                IdArticle = (int)reader["idArticle"]
                            },
                            services = new Services
                            {
                                idService = (int)reader["idService"]
                            }
                        };
                    }
                    reader.Close();
                }
            }
            besoinArticle.article = Article.GetById(besoinArticle.article.IdArticle,con);
            besoinArticle.services = Services.getById(con,besoinArticle.services.idService);
            
            return besoinArticle;
        }

        public void save(SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }
            String sql = "INSERT INTO besoinArticle(descri,quantite,etat,idArticle,idService) VALUES (@descri,@qt,0,@idArticle,@idService)";
            SqlCommand command = new SqlCommand(sql,con);

            command.Parameters.AddWithValue("@descri", this.Description);
            command.Parameters.AddWithValue("@qt", this.Quantite);
            command.Parameters.AddWithValue("@idArticle", this.article.IdArticle);
            command.Parameters.AddWithValue("@idService", this.services.idService);

            command.ExecuteNonQuery();
        }
        
        

        public static void valider(int idBesoin,SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            String sql = "UPDATE besoinArticle SET etat=5 WHERE idBesoinArticle="+idBesoin;
            SqlCommand command = new SqlCommand(sql,con);
            command.ExecuteNonQuery();
        }
        
        public static void supprimer(int idBesoin,SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            String sql = "UPDATE besoinArticle SET etat=-1 WHERE idBesoinArticle="+idBesoin;
            SqlCommand command = new SqlCommand(sql,con);
            command.ExecuteNonQuery();
        }
        
        
}