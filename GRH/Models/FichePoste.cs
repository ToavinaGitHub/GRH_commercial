using Microsoft.Data.SqlClient;

namespace GRH.Models;
public class FichePoste
{
    public int idFichePoste { get; set; }
    public Mpiasa mpiasa { get; set; }
    public Postes poste { get; set; }
    public Annonce annonce { get; set; }
    public Contrat contrat { get; set; }
    public Cv cv { get; set; }
    public Mpiasa mpiasaSup { get; set; }

    public FichePoste()
    {
    }

    public FichePoste(int idFichePoste, Mpiasa mpiasa, Postes poste, Annonce annonce, Contrat contrat, Cv cv, Mpiasa mpiasaSup)
    {
        this.idFichePoste = idFichePoste;
        this.mpiasa = mpiasa;
        this.poste = poste;
        this.annonce = annonce;
        this.contrat = contrat;
        this.cv = cv;
        this.mpiasaSup = mpiasaSup;
    }

    public static List<FichePoste> getAll(SqlConnection con)
    {
        if (con == null)
        {
            con = Connect.connectDB();
        }
        List<FichePoste> all = new List<FichePoste>();
        String sql = "SELECT * FROM fichePoste";
        SqlCommand command = new SqlCommand(sql,con);
        SqlDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            FichePoste f = new FichePoste();
            f.idFichePoste = (int)reader["idFichePoste"];
            f.annonce = new Annonce { idAnnonce = (int)reader["idAnnonce"] };
            f.contrat = new Contrat { idContrat = (int)reader["idContrat"] };
            f.cv = new Cv { idCv = (int)reader["idCv"] };
            f.poste = new Postes { idPoste = (int)reader["idPoste"] };
            f.mpiasa = new Mpiasa { matricule = (int)reader["matricule"] };
            f.mpiasaSup = new Mpiasa { matricule = (int)reader["matriculeSup"] };
            all.Add(f);
        }
        reader.Close();

        foreach (FichePoste fi in all)
        {
            fi.annonce = Annonce.getAnnonceById(con,fi.annonce.idAnnonce);
            fi.contrat = Contrat.getById(fi.contrat.idContrat, con);
            fi.poste = Postes.getById(fi.poste.idPoste,con);
            fi.mpiasa = Models.Mpiasa.getById(fi.mpiasa.matricule,con);
            fi.mpiasaSup = Models.Mpiasa.getById(fi.mpiasaSup.matricule,con);
            
        }

        return all;
    }
}