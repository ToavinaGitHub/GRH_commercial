using System.Data;
using Microsoft.Data.SqlClient;

namespace GRH.Models;

public class Note
{
    public int idNote { get; set; }
    public int idCv { get; set; }
    public double noteCv { get; set; }
    public double noteTeste { get; set; }
    public double noteEntretien { get; set; }

    public void save(SqlConnection connection)
    {
        try
        {
            if (connection.State != ConnectionState.Open)
            {
                connection = Connect.connectDB();
            }

            string query = "INSERT INTO Note (idCv, noteCv, noteTeste, noteEntretien) " +
                           "VALUES ( @idCv, @noteCv, @noteTeste, @noteEntretien)";

            using (SqlCommand command = new SqlCommand(query, connection))
            { 
                 
                command.Parameters.AddWithValue("@idCv", this.idCv); // Remplacez par la valeur appropriée
                command.Parameters.AddWithValue("@noteCv",noteCv);
                command.Parameters.AddWithValue("@noteTeste", noteTeste);
                command.Parameters.AddWithValue("@noteEntretien", noteEntretien);
             

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
    //11 etat raha efa nanao test
    
}