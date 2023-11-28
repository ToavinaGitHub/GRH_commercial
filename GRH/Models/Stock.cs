using Microsoft.Data.SqlClient;
using System.Data;

namespace GRH.Models
{
    public class Stock
    {

        public ArticleVente article { get; set; }

        public double quantiteInitiale { get; set; }

        public double reste { get; set; }

        public double prixTotalHT { get; set; }

        public double prixTotalTTC { get; set; }

        public double totalTVA { get; set; }

        public Stock() { }

        public Stock(ArticleVente article,double quantiteInitiale, double reste)
        {
            this.article = article;
            this.quantiteInitiale = quantiteInitiale;
            this.reste = reste;
        }

        public Stock getStockByArticle(int idArticle) 
        {
            SqlConnection co;
            Connect new_co = new Connect();
            co = Connect.connectDB();
            Stock stock = null;
            SqlCommand command = new SqlCommand("select sum(reste)reste from entreestock where idArticleVente="+idArticle+"", co);
            SqlDataReader reader = command.ExecuteReader();
            ArticleVente a = new ArticleVente(); 
            while (reader.Read())
            {
                double reste = (double)reader["reste"];
                stock = new Stock();   
                stock.reste= reste;
            }
            reader.Close();
            co.Close();
            if (stock != null)
            {
                stock.article= a.getArticleById(idArticle);       
            }
            return stock;
        }
    }
}
