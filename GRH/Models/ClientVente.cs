using Microsoft.Data.SqlClient;

namespace GRH.Models
{
    public class ClientVente
    {
        public int idClientVente { get; set; }

        public String nom { get; set; }

        public String email { get; set; }

        public String mdp { get; set; }

        public ClientVente() { }

        public ClientVente(int idClientVente, string nom, string email, string mdp)
        {
            this.idClientVente = idClientVente;
            this.nom = nom;
            this.email = email;
            this.mdp = mdp;
        }

        public ClientVente getClientById(int id)
        {
            SqlConnection co;
            Connect new_co = new Connect();
            co = Connect.connectDB();
            ClientVente client = new ClientVente();
            SqlCommand command = new SqlCommand("select * from clientvente where idClientVente="+id+"", co);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                string nom = (string)reader["nomclientvente"];
                string email = (string)reader["email"];
                string mpd = (string)reader["mdp"];
                client= new ClientVente(id,nom,email,mdp);
            }
            reader.Close();
            co.Close();
            return client;
        }
    }
}
