using Microsoft.Data.SqlClient;
using System.Data;

namespace GRH.Models
{
    public class ProformaVente
    {
        public int idProformaVente { get; set; }

        public ArticleVente articleVente { get; set; }
        public double quantite { get; set; }
        public DateTime date { get; set; }
        public int etat { get; set; }
         public UniteArticle  unite { get; set; }
        public ClientVente client { get; set; }

        public ProformaVente() { }

        public ProformaVente(int idProformaVente, ArticleVente articleVente, double quantite, DateTime date, UniteArticle unite, ClientVente client)
        {
            this.idProformaVente = idProformaVente;
            this.articleVente = articleVente;
            this.quantite = quantite;
            this.date = date;
            this.unite = unite;
            this.client = client;
        }

        public void save(SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            
            double quantiteBase = 0.0; 
            using (SqlCommand cmd = new SqlCommand("SELECT quantite FROM UniteArticle WHERE idUniteArticle = @unite", con))
            {
                cmd.Parameters.AddWithValue("@unite", this.unite.idUniteArticle);
                object result = cmd.ExecuteScalar();
                if (result != null && result != DBNull.Value)
                {
                    quantiteBase = Convert.ToDouble(result);
                }
            }

          
            double quantiteReelle = this.quantite * quantiteBase;

            Console.WriteLine("quantite= " + quantiteReelle);
            String sql = "INSERT INTO ProformaVente(idArticleVente, quantite, daty, etat, idUniteArticle, idClientVente) VALUES (@articleVente, @quantite, @date, 0, @unite, @client)";
            SqlCommand command = new SqlCommand(sql, con);
            Console.WriteLine(command);
            command.Parameters.AddWithValue("@articleVente", this.articleVente.idArticleVente);
            command.Parameters.AddWithValue("@quantite", quantiteReelle); 
            command.Parameters.AddWithValue("@date", this.date);
            command.Parameters.AddWithValue("@unite", this.unite.idUniteArticle);
            command.Parameters.AddWithValue("@client", this.client.idClientVente);

            command.ExecuteNonQuery();
        }

        public static ProformaVente GetLast(SqlConnection con)
        {
            if (con == null || con.State == ConnectionState.Closed)
            {
                con = Connect.connectDB();
            }

            ProformaVente proforma = null;
            //string query = "SELECT TOP 1 * FROM ProformaVente ORDER BY idProformaVente DESC";
            string query = "SELECT TOP 1 ProformaVente.*, ClientVente.nomClient FROM ProformaVente " +
                  "INNER JOIN ClientVente ON ProformaVente.idClientVente = ClientVente.idClientVente " +
                  "ORDER BY ProformaVente.idProformaVente DESC";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        proforma = new ProformaVente
                        {
                            idProformaVente = Convert.ToInt32(reader["idProformaVente"]),
                            articleVente =new ArticleVente
                            {
                                idArticleVente = (int)reader["idArticleVente"]
                            },
                            quantite = Convert.ToDouble(reader["quantite"]),
                            date = Convert.ToDateTime(reader["daty"]),
                            unite = new UniteArticle
                            {
                                idUniteArticle = (int)reader["idUniteArticle"]
                            },
                            client = new ClientVente
                            {
                                idClientVente = (int)reader["idClientVente"]
                            }
                           
                        };
                    }
                    reader.Close();
                }
            }
            proforma.articleVente = ArticleVente.GetById(proforma.articleVente.idArticleVente, con);
            proforma.unite = UniteArticle.GetById(proforma.unite.idUniteArticle, con);
            return proforma;
        }

    }


}
