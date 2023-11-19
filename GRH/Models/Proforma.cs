using System.Data;
using Microsoft.Data.SqlClient;

namespace GRH.Models;

public class Proforma
{
    public int IdProforma { get; set; }
    public double PrixUnitaire { get; set; }
    public double TVA { get; set; }
    public DateTime Daty { get; set; }
    public Article article { get; set; }
    public Fournisseur fournisseur { get; set; }


    public Proforma()
    {
    }

    public Proforma(int idProforma, double prixUnitaire, double tva, DateTime daty, Article article, Fournisseur fournisseur)
    {
        IdProforma = idProforma;
        PrixUnitaire = prixUnitaire;
        TVA = tva;
        Daty = daty;
        this.article = article;
        this.fournisseur = fournisseur;
    }

    public static List<Proforma> GetAll(SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        List<Proforma> proformas = new List<Proforma>();
        string query = "SELECT * FROM Proforma";
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Proforma proforma = new Proforma
                    {
                        IdProforma = Convert.ToInt32(reader["idProforma"]),
                        PrixUnitaire = Convert.ToDouble(reader["prixUnitaire"]),
                        TVA = Convert.ToDouble(reader["tva"]),
                        Daty = Convert.ToDateTime(reader["daty"]),
                        article = new Article
                        {
                            IdArticle = (int)reader["idArticle"]
                        },
                        fournisseur = new Fournisseur
                        {
                            IdFournisseur = (int)reader["idFournisseur"]
                        }
                    };
                    proformas.Add(proforma);
                }
                reader.Close();
            }
        }

        foreach (var pr in proformas)
        {
            pr.article = Article.GetById(pr.article.IdArticle, con);
            pr.fournisseur = Fournisseur.GetById(pr.fournisseur.IdFournisseur,con);
        }
        return proformas;
    }
    public static Proforma GetById(int id, SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        Proforma proforma = null;
        string query = "SELECT * FROM Proforma WHERE idProforma = @Id";
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.AddWithValue("@Id", id);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    proforma = new Proforma
                    {
                        IdProforma = Convert.ToInt32(reader["idProforma"]),
                        PrixUnitaire = Convert.ToDouble(reader["prixUnitaire"]),
                        TVA = Convert.ToDouble(reader["tva"]),
                        Daty = Convert.ToDateTime(reader["daty"]),
                        article = new Article
                        {
                            IdArticle = (int)reader["idArticle"]
                        },
                        fournisseur = new Fournisseur
                        {
                            IdFournisseur = (int)reader["idFournisseur"]
                        }
                    };
                }
                reader.Close();
            }
        }
        proforma.article = Article.GetById(proforma.article.IdArticle, con);
        proforma.fournisseur = Fournisseur.GetById(proforma.fournisseur.IdFournisseur,con);
        return proforma;
    }
    public static Proforma GetByArticle(int idArticle, SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        Proforma proforma = null;
        string query = "SELECT TOP 1 * FROM proforma p1 WHERE idArticle = @id1 AND daty = (SELECT MAX(daty) FROM proforma p2 WHERE p1.idFournisseur = p2.idFournisseur AND p2.idArticle = @id2 ) ORDER BY p1.prixUnitaire ASC ; ";
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.AddWithValue("@id1", idArticle);
            cmd.Parameters.AddWithValue("@id2", idArticle);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    proforma = new Proforma
                    {
                        IdProforma = Convert.ToInt32(reader["idProforma"]),
                        PrixUnitaire = Convert.ToDouble(reader["prixUnitaire"]),
                        TVA = Convert.ToDouble(reader["tva"]),
                        Daty = Convert.ToDateTime(reader["daty"]),
                        article = new Article
                        {
                            IdArticle = (int)reader["idArticle"]
                        },
                        fournisseur = new Fournisseur
                        {
                            IdFournisseur = (int)reader["idFournisseur"]
                        }
                    };
                }
                reader.Close();
            }
        }
        proforma.article = Article.GetById(proforma.article.IdArticle, con);
        proforma.fournisseur = Fournisseur.GetById(proforma.fournisseur.IdFournisseur,con);
        return proforma;
    }
    public static Proforma GetByArticleFournisseur(int idArticle,int idFournisseur, SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        Proforma proforma = null;
        string query = "SELECT TOP 1 * FROM proforma p1 WHERE idArticle = @id1 AND idFournisseur = @idF AND daty = (SELECT MAX(daty) FROM proforma p2 WHERE p1.idFournisseur = p2.idFournisseur AND p2.idArticle = @id2 ) ORDER BY p1.prixUnitaire ASC ; ";
        using (SqlCommand cmd = new SqlCommand(query, con))
        {
            cmd.Parameters.AddWithValue("@id1", idArticle);
            cmd.Parameters.AddWithValue("@id2", idArticle);
            cmd.Parameters.AddWithValue("@idF", idFournisseur);
            
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                if (reader.Read())
                {
                    proforma = new Proforma
                    {
                        IdProforma = Convert.ToInt32(reader["idProforma"]),
                        PrixUnitaire = Convert.ToDouble(reader["prixUnitaire"]),
                        TVA = Convert.ToDouble(reader["tva"]),
                        Daty = Convert.ToDateTime(reader["daty"]),
                        article = new Article
                        {
                            IdArticle = (int)reader["idArticle"]
                        },
                        fournisseur = new Fournisseur
                        {
                            IdFournisseur = (int)reader["idFournisseur"]
                        }
                    };
                }
                reader.Close();
            }
        }
        proforma.article = Article.GetById(proforma.article.IdArticle, con);
        proforma.fournisseur = Fournisseur.GetById(proforma.fournisseur.IdFournisseur,con);
        return proforma;
    }

    public void save(SqlConnection con)
    {
        if (con == null || con.State == ConnectionState.Closed)
        {
            con = Connect.connectDB();
        }

        String sql = "INSERT INTO proforma(prixUnitaire,tva,daty,idArticle,idFournisseur) VALUES(@pu,@tva,@daty,@idArticle,@idFournisseur)";
        SqlCommand command = new SqlCommand(sql,con);
        command.Parameters.AddWithValue("@pu", this.PrixUnitaire);
        command.Parameters.AddWithValue("@tva", this.TVA);
        command.Parameters.AddWithValue("@daty", this.Daty);
        command.Parameters.AddWithValue("@idArticle", this.article.IdArticle);
        command.Parameters.AddWithValue("@idFournisseur", this.fournisseur.IdFournisseur);

        command.ExecuteNonQuery();
    }
}