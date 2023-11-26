using System.Data;
using Microsoft.Data.SqlClient;

namespace GRH.Models
{
    public class CritereCoef
    {
        public int idCritereCoef { get; set; }
        public SousCritere sousCritere { get; set; }
        public int coef { get; set; }
        public Annonce annonce { get; set; }

        public CritereCoef() { }

        public CritereCoef (int idCritereCoef, SousCritere sousCritere, int coef, Annonce annonce)
        {
            this.idCritereCoef = idCritereCoef;
            this.sousCritere = sousCritere;
            this.coef = coef;
            this.annonce = annonce;
        }
        public void Insert(SqlConnection connection)
        {
            try
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection = Connect.connectDB();
                }

                string query = "INSERT INTO CritereCoef (idSousCritere, coef, idAnnonce) " +
                               "VALUES (@idSousCritere, @coef, @idAnnonce)";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@idSousCritere", sousCritere.idSousCritere);
                    command.Parameters.AddWithValue("@coef", coef);
                    command.Parameters.AddWithValue("@idAnnonce", annonce.idAnnonce);

                    int rowsAffected = command.ExecuteNonQuery();

                    if (rowsAffected > 0)
                    {
                        Console.WriteLine("Insertion réussie !");
                    }
                    else
                    {
                        Console.WriteLine("Aucune ligne n'a été insérée.");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erreur lors de l'insertion : " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        
        
    }
}
