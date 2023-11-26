using System.Data;
using Microsoft.Data.SqlClient;

namespace GRH.Models;

public class DetailsBonCommande
{
        public int IdDetailsB { get; set; }
        public double Quantite { get; set; }
        public Proforma proforma { get; set; }
        public BonDeCommande bonDeCommande { get; set; }

        public DetailsBonCommande()
        {
        }

        public DetailsBonCommande(int idDetailsB, double quantite, Proforma proforma, BonDeCommande bonDeCommande)
        {
            IdDetailsB = idDetailsB;
            Quantite = quantite;
            this.proforma = proforma;
            this.bonDeCommande = bonDeCommande;
        }

        public List<DetailsBonCommande> GetAll(SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            List<DetailsBonCommande> detailsBonCommandes = new List<DetailsBonCommande>();
            string query = "SELECT * FROM DetailsBonCommande";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        DetailsBonCommande detailsBonCommande = new DetailsBonCommande
                        {
                            IdDetailsB = Convert.ToInt32(reader["idDetailsB"]),
                            Quantite = Convert.ToDouble(reader["quantite"]),
                            proforma = new Proforma
                            {
                                IdProforma = (int)reader["idProforma"]
                            },
                            bonDeCommande = new BonDeCommande
                            {
                                IdBonDeCommande = (int)reader["idBonDeCommande"]
                            }
                        };
                        detailsBonCommandes.Add(detailsBonCommande);
                    }
                    reader.Close();
                }
            }

            foreach (var all in detailsBonCommandes)
            {
                all.proforma = Proforma.GetById(all.proforma.IdProforma,con);
                all.bonDeCommande = BonDeCommande.GetById(all.bonDeCommande.IdBonDeCommande,con);
            }
            return detailsBonCommandes;
        }

        public DetailsBonCommande GetById(int id, SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            DetailsBonCommande detailsBonCommande = null;
            string query = "SELECT * FROM DetailsBonCommande WHERE idDetailsB = @Id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@Id", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        detailsBonCommande = new DetailsBonCommande
                        {
                            IdDetailsB = Convert.ToInt32(reader["idDetailsB"]),
                            Quantite = Convert.ToDouble(reader["quantite"]),
                            proforma = new Proforma
                            {
                                IdProforma = (int)reader["idProforma"]
                            },
                            bonDeCommande = new BonDeCommande
                            {
                                IdBonDeCommande = (int)reader["idBonDeCommande"]
                            }
                        };
                    }
                    reader.Close();
                }
            }
            detailsBonCommande.proforma = Proforma.GetById(detailsBonCommande.proforma.IdProforma,con);
            detailsBonCommande.bonDeCommande = BonDeCommande.GetById(detailsBonCommande.bonDeCommande.IdBonDeCommande,con);
            return detailsBonCommande;
        }

        public void save(SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            String sql = "INSERT INTO detailsBonCommande(quantite,idProforma,idBonDeCommande) VALUES("+this.Quantite+","+this.proforma.IdProforma+","+this.bonDeCommande.IdBonDeCommande+")";
            SqlCommand command = new SqlCommand(sql,con);
            command.ExecuteNonQuery();
        }
}