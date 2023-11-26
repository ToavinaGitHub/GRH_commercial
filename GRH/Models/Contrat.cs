using Microsoft.Data.SqlClient;

namespace GRH.Models;

public class Contrat
{
    public int idContrat { get; set; }
    public Clients clients { get; set; }
    public String addresse { get; set; }
    public DateTime debut { get; set; }
    public DateTime fin { get; set; }
    public double salaire { get; set; }
    public RH rh { get; set; }
    
    public Postes poste { get; set; }
    
    public TypeContrat typeContrat { get; set; }

    public Contrat()
    {
    }

    public Contrat(int idContrat, Clients clients, string addresse, DateTime debut, DateTime fin, double salaire, RH rh,Postes poste,TypeContrat typeContrat)
    {
        this.idContrat = idContrat;
        this.clients = clients;
        this.addresse = addresse;
        this.debut = debut;
        this.fin = fin;
        this.salaire = salaire;
        this.rh = rh;
        this.poste = poste;
        this.typeContrat = typeContrat;
    }
     public static List<Contrat> getAll(SqlConnection con)
     {
         if (con == null)
         {
             con = Connect.connectDB();
         }
            List<int> allCli = new List<int>();
            List<int> allRh = new List<int>();
            List<Contrat> contrats = new List<Contrat>();

            using (SqlConnection connection = con)
            {
                
                string query = "SELECT * FROM Contrat";
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allCli.Add(reader.GetInt32(1));
                        allRh.Add(reader.GetInt32(6));
                        Contrat contrat = new Contrat
                        {
                            idContrat = reader.GetInt32(0),
                            clients = new Clients(),
                            addresse = reader.GetString(2),
                            debut = reader.GetDateTime(3),
                            salaire = reader.GetDouble(5),
                            rh = new RH(),
                            poste = new Postes{idPoste = (int)reader["idPoste"]},
                            typeContrat = new TypeContrat{idTypeContrat = (int)reader["idTypeContrat"]}
                        };
                        if (contrat.typeContrat.idTypeContrat == 3)
                        {
                            contrat.fin = new DateTime();
                        }
                        else
                        {
                            contrat.fin = reader.GetDateTime(4);
                        }
                        contrats.Add(contrat);
                    }
                    reader.Close();
                }
            }

            int i = 0;
            con = Connect.connectDB();
            foreach (Contrat c in contrats)
            {
                c.clients = Clients.getClientsById(allCli[i], con);
                c.rh = RH.GetById(allRh[i], con);
                c.poste = Postes.getById(c.poste.idPoste,con);
                con = Connect.connectDB();
                c.typeContrat = TypeContrat.getById(c.typeContrat.idTypeContrat,con);
                i += 1;
            }
            return contrats;
        }
     
       public static List<Contrat> getAllByType(int idType,SqlConnection con)
     {
         if (con == null)
         {
             con = Connect.connectDB();
         }
            List<int> allCli = new List<int>();
            List<int> allRh = new List<int>();
            List<Contrat> contrats = new List<Contrat>();

            using (SqlConnection connection = con)
            {
                
                string query = "SELECT * FROM Contrat WHERE idTypeContrat="+idType;
                using (SqlCommand command = new SqlCommand(query, connection))
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allCli.Add(reader.GetInt32(1));
                        allRh.Add(reader.GetInt32(6));
                        Contrat contrat = new Contrat
                        {
                            idContrat = reader.GetInt32(0),
                            clients = new Clients(),
                            addresse = reader.GetString(2),
                            debut = reader.GetDateTime(3),
                            salaire = reader.GetDouble(5),
                            rh = new RH(),
                            poste = new Postes{idPoste = (int)reader["idPoste"]},
                            typeContrat = new TypeContrat{idTypeContrat = (int)reader["idTypeContrat"]}
                        };
                        if (contrat.typeContrat.idTypeContrat == 3)
                        {
                            contrat.fin = new DateTime();
                        }
                        else
                        {
                            contrat.fin = reader.GetDateTime(4);
                        }
                        contrats.Add(contrat);
                    }
                    reader.Close();
                }
            }

            int i = 0;
            con = Connect.connectDB();
            foreach (Contrat c in contrats)
            {
                c.clients = Clients.getClientsById(allCli[i], con);
                c.rh = RH.GetById(allRh[i], con);
                c.poste = Postes.getById(c.poste.idPoste,con);
                con = Connect.connectDB();
                c.typeContrat = TypeContrat.getById(c.typeContrat.idTypeContrat,con);
                i += 1;
            }
            return contrats;
        }

        public static Contrat getById(int id,SqlConnection con)
        {
            if (con == null)
            {
                con = Connect.connectDB();
            }
            Contrat c = null;
            using (SqlConnection connection = con)
            {

                string query = "SELECT * FROM Contrat WHERE idContrat = @id";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            c = new Contrat
                            {
                                idContrat = reader.GetInt32(0),
                                clients = new Clients { idClient = reader.GetInt32(1) },
                                addresse = reader.GetString(2),
                                debut = reader.GetDateTime(3),
                                salaire = reader.GetDouble(5),
                                rh = new RH { idRh = reader.GetInt32(6) },
                                poste = new Postes{idPoste = (int)reader["idPoste"]},
                                typeContrat = new TypeContrat{idTypeContrat = (int)reader["idTypeContrat"]}
                            };
                            if (c.typeContrat.idTypeContrat == 3)
                            {
                                c.fin = new DateTime();
                            }
                            else
                            {
                                c.fin = reader.GetDateTime(4);
                            }
                        }
                        reader.Close();
                    }
                }
                c.clients = Clients.getClientsById(c.clients.idClient, con);
                c.rh = RH.GetById(c.rh.idRh, con);
                c.poste = Postes.getById(c.poste.idPoste, con);
                c.typeContrat = TypeContrat.getById(c.typeContrat.idTypeContrat, con);
            }
            return c; 
        }
        public static Contrat getLastContrat(SqlConnection con)
        {
            if (con == null)
            {
                con = Connect.connectDB();
            }
            Contrat c = null;
            using (SqlConnection connection = con)
            {
                

                string query = "SELECT TOP 1 *FROM contrat ORDER BY idContrat DESC";
                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            c = new Contrat
                            {
                                idContrat = reader.GetInt32(0),
                                clients = new Clients { idClient = reader.GetInt32(1) },
                                addresse = reader.GetString(2),
                                debut = reader.GetDateTime(3),
                                salaire = reader.GetDouble(5),
                                rh = new RH { idRh = reader.GetInt32(6) },
                                poste = new Postes{idPoste = (int)reader["idPoste"]},
                                typeContrat = new TypeContrat{idTypeContrat = (int)reader["idTypeContrat"]}
                            };
                            if (c.typeContrat.idTypeContrat == 3)
                            {
                                c.fin = new DateTime();
                            }
                            else
                            {
                                c.fin = reader.GetDateTime(4);
                            }
                        }
                        reader.Close();
                    }
                }
                c.clients = Clients.getClientsById(c.clients.idClient, con);
                c.rh = RH.GetById(c.rh.idRh, con);
                c.poste = Postes.getById(c.poste.idPoste, con);
                c.typeContrat = TypeContrat.getById(c.typeContrat.idTypeContrat, con);
            }
            return c; 
        }

        public void save(SqlConnection con)
        {
            if (con == null)
            {
                con = Connect.connectDB();
            }

            String sql = "INSERT INTO contrat(idClient,addresse,debut,fin,salaire,idRh,idPoste,idTypeContrat) VALUES("+this.clients.idClient+",'"+this.addresse+"','"+this.debut+"','"+this.fin+"',"+this.salaire+","+this.rh.idRh+","+this.poste.idPoste+","+this.typeContrat.idTypeContrat+")";
            if (this.typeContrat.idTypeContrat == 3)
            {
                 sql = "INSERT INTO contrat(idClient,addresse,debut,fin,salaire,idRh,idPoste,idTypeContrat) VALUES("+this.clients.idClient+",'"+this.addresse+"','"+this.debut+"',NULL,"+this.salaire+","+this.rh.idRh+","+this.poste.idPoste+","+this.typeContrat.idTypeContrat+")";     
            }
           
            Console.WriteLine(sql);
            SqlCommand command = new SqlCommand(sql,con);
            command.ExecuteNonQuery();
        }

        public static bool dejaContrat(Cv cv, SqlConnection con)
        {
            if (con == null)
            {
                con = Connect.connectDB();
            }

            String sql = "SELECT * FROM contrat WHERE idTypeContrat=1 AND idClient="+cv.client.idClient;
          
            SqlCommand command = new SqlCommand(sql,con);
            SqlDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                return true;
            }
            reader.Close();
            return false;
        }
}