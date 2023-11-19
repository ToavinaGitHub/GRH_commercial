using System.Data;
using Microsoft.Data.SqlClient;

namespace GRH.Models;

public class BonDeCommande
{
    public int IdBonDeCommande { get; set; }
        public string Titre { get; set; }
        public DateTime Daty { get; set; }
        public int JourLivraison { get; set; }
        public string ConditionPayement { get; set; }
        public int Etat { get; set; }
        public TypePayement typePayement { get; set; }
        public Fournisseur fournisseur { get; set; }
        
        public Dictionary<Article,Double> artQt { get; set; }

        public BonDeCommande()
        {
        }

        public BonDeCommande(int idBonDeCommande, string titre, DateTime daty, int jourLivraison, string conditionPayement, int etat, TypePayement typePayement, Fournisseur fournisseur)
        {
            IdBonDeCommande = idBonDeCommande;
            Titre = titre;
            Daty = daty;
            JourLivraison = jourLivraison;
            ConditionPayement = conditionPayement;
            Etat = etat;
            this.typePayement = typePayement;
            this.fournisseur = fournisseur;
        }

        public static List<BonDeCommande> GetAll(SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            List<BonDeCommande> bonDeCommandes = new List<BonDeCommande>();
            string query = "SELECT * FROM BonDeCommande";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BonDeCommande bonDeCommande = new BonDeCommande
                        {
                            IdBonDeCommande = Convert.ToInt32(reader["idBonDeCommande"]),
                            Titre = reader["titre"].ToString(),
                            Daty = Convert.ToDateTime(reader["daty"]),
                            JourLivraison = Convert.ToInt32(reader["jourLivraison"]),
                            ConditionPayement = reader["conditionPayement"].ToString(),
                            Etat = Convert.ToInt32(reader["etat"]),
                            typePayement = new TypePayement
                            {
                                IdTypePayement = (int)reader["idTypePayement"]
                            },
                            fournisseur = new Fournisseur
                            {
                                IdFournisseur = (int)reader["idFournisseur"]
                            }
                        };
                        bonDeCommandes.Add(bonDeCommande);
                    }
                    reader.Close();
                }
            }

            foreach (var all in bonDeCommandes)
            {
                all.typePayement = TypePayement.GetById(all.typePayement.IdTypePayement,con);
                all.fournisseur = Fournisseur.GetById(all.fournisseur.IdFournisseur, con);
            }
            return bonDeCommandes;
        }

        public static List<BonDeCommande> GetAllByEtat(int state,SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            List<BonDeCommande> bonDeCommandes = new List<BonDeCommande>();
            
            string query = "SELECT * FROM BonDeCommande WHERE etat="+state;

            if (state == 999)
            {
                query = "SELECT * FROM BonDeCommande";
            }
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        BonDeCommande bonDeCommande = new BonDeCommande
                        {
                            IdBonDeCommande = Convert.ToInt32(reader["idBonDeCommande"]),
                            Titre = reader["titre"].ToString(),
                            Daty = Convert.ToDateTime(reader["daty"]),
                            JourLivraison = Convert.ToInt32(reader["jourLivraison"]),
                            ConditionPayement = reader["conditionPayement"].ToString(),
                            Etat = Convert.ToInt32(reader["etat"]),
                            typePayement = new TypePayement
                            {
                                IdTypePayement = (int)reader["idTypePayement"]
                            },
                            fournisseur = new Fournisseur
                            {
                                IdFournisseur = (int)reader["idFournisseur"]
                            }
                        };
                        bonDeCommandes.Add(bonDeCommande);
                    }
                    reader.Close();
                }
            }

            foreach (var all in bonDeCommandes)
            {
                all.typePayement = TypePayement.GetById(all.typePayement.IdTypePayement,con);
                all.fournisseur = Fournisseur.GetById(all.fournisseur.IdFournisseur, con);
            }
            return bonDeCommandes;
        }
        public static BonDeCommande GetById(int id, SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            BonDeCommande bonDeCommande = null;
            string query = "SELECT * FROM BonDeCommande WHERE idBonDeCommande = @Id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bonDeCommande = new BonDeCommande
                        {
                            IdBonDeCommande = Convert.ToInt32(reader["idBonDeCommande"]),
                            Titre = reader["titre"].ToString(),
                            Daty = Convert.ToDateTime(reader["daty"]),
                            JourLivraison = Convert.ToInt32(reader["jourLivraison"]),
                            ConditionPayement = reader["conditionPayement"].ToString(),
                            Etat = Convert.ToInt32(reader["etat"]),
                            typePayement = new TypePayement
                            {
                                IdTypePayement = (int)reader["idTypePayement"]
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
            bonDeCommande.typePayement = TypePayement.GetById(bonDeCommande.typePayement.IdTypePayement,con);
            bonDeCommande.fournisseur = Fournisseur.GetById(bonDeCommande.fournisseur.IdFournisseur, con);
            return bonDeCommande;
        }
        public static BonDeCommande GetLast(SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            BonDeCommande bonDeCommande = null;
            string query = "SELECT TOP 1 * FROM BonDeCommande ORDER BY idBonDeCommande DESC";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        bonDeCommande = new BonDeCommande
                        {
                            IdBonDeCommande = Convert.ToInt32(reader["idBonDeCommande"]),
                            Titre = reader["titre"].ToString(),
                            Daty = Convert.ToDateTime(reader["daty"]),
                            JourLivraison = Convert.ToInt32(reader["jourLivraison"]),
                            ConditionPayement = reader["conditionPayement"].ToString(),
                            Etat = Convert.ToInt32(reader["etat"]),
                            typePayement = new TypePayement
                            {
                                IdTypePayement = (int)reader["idTypePayement"]
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
            bonDeCommande.typePayement = TypePayement.GetById(bonDeCommande.typePayement.IdTypePayement,con);
            bonDeCommande.fournisseur = Fournisseur.GetById(bonDeCommande.fournisseur.IdFournisseur, con);
            return bonDeCommande;
        }

        
        
        public void save(SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }
           
            String sql = "INSERT INTO bonDeCommande(titre,daty,jourLivraison,conditionPayement,etat,idTypePayement,idFournisseur) " +
                         "VALUES('"+this.Titre+"','"+this.Daty+"',"+this.JourLivraison+",'"+this.ConditionPayement+"',"+this.Etat+","+this.typePayement.IdTypePayement+","+this.fournisseur.IdFournisseur+")";

            SqlCommand command = new SqlCommand(sql,con);
            command.ExecuteNonQuery();
        }

        public void saveDetails(SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            BonDeCommande bd = BonDeCommande.GetLast(con);
            foreach (var dic in this.artQt)
            {
                Proforma temp = Proforma.GetByArticle(dic.Key.IdArticle,con);

                DetailsBonCommande d = new DetailsBonCommande(1,dic.Value,temp,bd);
                d.save(con);
            }
        }

        public void transaction(int etat, SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            String sql = "UPDATE bonDeCommande SET etat="+etat+" WHERE idBonDeCommande="+this.IdBonDeCommande;
            SqlCommand command = new SqlCommand(sql,con);
            command.ExecuteNonQuery();
        }
}