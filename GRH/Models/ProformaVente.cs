using Microsoft.Data.SqlClient;

namespace GRH.Models
{
    public class ProformaVente
    {
        public int idProformaVente { get; set; }

        public ArticleVente article { get; set; }

        public double quantite { get; set; }

        public DateTime date { get; set; }

        public ClientVente client { get; set; }

        public ProformaVente() { }

        public ProformaVente(int idProformaVente,double quantite, DateTime date)
        {
            this.idProformaVente = idProformaVente;
            this.quantite = quantite;
            this.date = date;
        }

        public ProformaVente getById(int id) {
            SqlConnection co;
            Connect new_co = new Connect();
            co = Connect.connectDB();
            ProformaVente proformaVente = new ProformaVente();
            SqlCommand command = new SqlCommand("select * from proformaVente where idProformaVente=" + id + "", co);
            SqlDataReader reader = command.ExecuteReader();
            ArticleVente a= new ArticleVente();
            ClientVente c= new ClientVente();
            int idA = 0;
            int idC= 0;
            while (reader.Read())
            {
                idA= (int)reader["idArticleVente"];
                idC = (int)reader["idClientVente"];
                double quantite = (double)reader["quantite"];
                DateTime date= (DateTime)reader["dateProformaVente"];
                proformaVente= new ProformaVente(id, quantite, date);
            }
            reader.Close();
            co.Close();
            proformaVente.article= a.getArticleById(idA);
            proformaVente.client= c.getClientById(idC);
            return proformaVente;
        }
    }
}
