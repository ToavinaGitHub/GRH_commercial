using System.Data;
using Microsoft.Data.SqlClient;

namespace GRH.Models
{
    public class Cv
    {
       public int idCv {  get; set; }
        public Clients client { get; set; }
        public List<SousCritere> sousCritere { get; set; }   

        public String pdfDiplome { get; set; }

        public String pdfAttestation { get; set; }

        public Annonce annonce { get; set; }

        public Cv(int idCv, Clients client, List<SousCritere> sousCritere, string pdfDiplome, string pdfAttestation, Annonce annonce)
        {
            this.idCv = idCv;
            this.client = client;
            this.sousCritere = sousCritere;
            this.pdfDiplome = pdfDiplome;
            this.pdfAttestation = pdfAttestation;
            this.annonce = annonce;
        }

        public Cv(int idCv, Clients client, List<SousCritere> sousCritere, string pdfDiplome, string pdfAttestation)
        {
            this.idCv = idCv;
            this.client = client;
            this.sousCritere = sousCritere;
            this.pdfDiplome = pdfDiplome;
            this.pdfAttestation = pdfAttestation;
        }
        public Cv(int idCv, Clients client, Annonce annonce, string pdfDiplome, string pdfAttestation)
        {
            this.idCv = idCv;
            this.client = client;
            this.annonce = annonce;
            this.pdfDiplome = pdfDiplome;
            this.pdfAttestation = pdfAttestation;
        }

        public Cv() { }

        public List<int> listeCvByAnnonce(SqlConnection co,int idAnnonce)
        {
            if (co == null)
            {
                co = Connect.connectDB();
            }
            List<int> list = new List<int>();
            SqlCommand command = new SqlCommand("SELECT idCv FROM cv where idAnnonce=" + idAnnonce + " ", co);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add((int)reader["idCv"]);
            }
            reader.Close();
            return list;
        }

        public List<int> listeCvTesteByAnnonce(SqlConnection co,int idAnnonce)
        {
            if (co == null)
            {
                co = Connect.connectDB();
            }
            List<int> list = new List<int>();
            SqlCommand command = new SqlCommand("SELECT c.idCv idCv FROM etatClient e JOIN cv c on e.idClient=c.idClient and e.idAnnonce=c.idAnnonce WHERE c.idAnnonce="+idAnnonce+" and e.etat>=1", co);
            Console.WriteLine("SELECT c.idCv idCv FROM etatClient e JOIN cv c on e.idClient=c.idClient and e.idAnnonce=c.idAnnonce WHERE c.idAnnonce="+idAnnonce+" and e.etat>=1");
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add((int)reader["idCv"]);
            }
            reader.Close();
            return list;
        }
        public List<int> listeCvEntretienByAnnonce(SqlConnection co,int idAnnonce)
        {
            if (co == null)
            {
                co = Connect.connectDB();
            }
            List<int> list = new List<int>();
            SqlCommand command = new SqlCommand("SELECT c.idCv idCv FROM etatClient e JOIN cv c on e.idClient=c.idClient and e.idAnnonce=c.idAnnonce WHERE c.idAnnonce="+idAnnonce+" and e.etat=2", co);
            Console.WriteLine("SELECT c.idCv idCv FROM etatClient e JOIN cv c on e.idClient=c.idClient and e.idAnnonce=c.idAnnonce WHERE c.idAnnonce="+idAnnonce+" and e.etat=2");
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                list.Add((int)reader["idCv"]);
            }
            reader.Close();
            return list;
        }
        public int getNoteCv(SqlConnection co, int idCv,int idAnnonce)
        {
            if (co == null)
            {
                co = Connect.connectDB();
            }
            
            int note = 0;
            
            SqlCommand command = new SqlCommand("select sum(coef) somme FROM detailsCV dc join cv on dc.idCv=cv.idCv JOIN critereCoef cc on dc.idSousCritere=cc.idSousCritere where cv.idAnnonce="+idAnnonce+" and cc.idAnnonce="+idAnnonce+" and cv.idAnnonce="+idAnnonce+"and cv.idCv="+idCv+" group by cv.idCv",co);
            
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                
                note = int.Parse(reader["somme"].ToString());
            }
            reader.Close();
            return note;
         }

        public String[] getPDF(SqlConnection co, int idCv)
        {
            if (co == null)
            {
                co = Connect.connectDB();
            }
            String[] pdf= new string[2];
            SqlCommand command = new SqlCommand("select pdfDiplome,pdfAttestation from cv where idCv=" + idCv + "", co);
            SqlDataReader reader = command.ExecuteReader();
            while (reader.Read())
            {
                pdf[0] = (string)reader["pdfDiplome"];
                pdf[1] = (string)reader["pdfAttestation"];
            }
            reader.Close();
            return pdf;
        }
        public List<Cv> getCv(SqlConnection co,int idAnnonce)
        {
            if (co == null)
            {
                co = Connect.connectDB();
            }
            List<Cv> list = new List<Cv>();
            Cv cv1= new Cv();
            List<int> idCv= cv1.listeCvByAnnonce(co,idAnnonce);
            Clients client = new Clients();
            SousCritere sousCritere = new SousCritere();
            List<SousCritere> details= new List<SousCritere>();
            for(int i=0; i<idCv.Count; i++)
            {
                Cv cv = new Cv();
                cv.idCv= idCv[i];
                client= client.getClientAnnonceByCv(co, idAnnonce, idCv[i]);
                client.note = cv.getNoteCv(co, idCv[i],idAnnonce)+client.getNoteSexe(co,idAnnonce);
                
                cv.client= client;
                details= sousCritere.getDetailsByCv(co, idCv[i]);
                cv.sousCritere= details;
                String[] pdf = cv.getPDF(co, idCv[i]);
                cv.pdfDiplome = pdf[0];
                cv.pdfAttestation = pdf[1];
                list.Add(cv);
            }
            return list;
        }

        public static int getLastIdCv(SqlConnection con)
        {
            if (con == null)
            {
                con = Connect.connectDB();
            }

            String sql = "SELECT TOP 1 idCv FROM Cv ORDER BY idCv DESC";
            SqlCommand command = new SqlCommand(sql,con);
            SqlDataReader reader = command.ExecuteReader();

            int id = 0;
            if (reader.Read())
            {
                id = (int)reader["idCv"];
            }
            reader.Close();

            return id;
        }
        public List<Cv> getCvTest(SqlConnection co,int idAnnonce)
        {
            if (co == null)
            {
                co = Connect.connectDB();
            }
            List<Cv> list = new List<Cv>();
            Cv cv1= new Cv();
            List<int> idCv= cv1.listeCvTesteByAnnonce(co,idAnnonce);
            
            Clients client = new Clients();
            SousCritere sousCritere = new SousCritere();
            List<SousCritere> details= new List<SousCritere>();
            for(int i=0; i<idCv.Count; i++)
            {
                Cv cv = new Cv();
                cv.idCv= idCv[i];
             
                client= client.getClientAnnonceByCv(co, idAnnonce, idCv[i]);

                int a = 0;
           
                try
                {
                    a = Models.Cv.getNoteTeste(idCv[i], co);
                }
                catch (Exception e)
                {
                    
                }
                client.note = a;
                
                cv.client= client;
                details= sousCritere.getDetailsByCv(co, idCv[i]);
                
                cv.sousCritere= details;
                String[] pdf = cv.getPDF(co, idCv[i]);
                cv.pdfDiplome = pdf[0];
                cv.pdfAttestation = pdf[1];
                list.Add(cv);
            }
            
            return list;
        }
        public List<Cv> getCvEntretien(SqlConnection co,int idAnnonce)
        {
            if (co == null)
            {
                co = Connect.connectDB();
            }
            List<Cv> list = new List<Cv>();
            Cv cv1= new Cv();
            List<int> idCv= cv1.listeCvEntretienByAnnonce(co,idAnnonce);
            
            Clients client = new Clients();
            SousCritere sousCritere = new SousCritere();
            List<SousCritere> details= new List<SousCritere>();
            for(int i=0; i<idCv.Count; i++)
            {
                Cv cv = new Cv();
                cv.idCv= idCv[i];
             
                client= client.getClientAnnonceByCv(co, idAnnonce, idCv[i]);

                int a = 0;
           
                try
                {
                    a = Models.Cv.getNoteEntretien(idCv[i], co);
                }
                catch (Exception e)
                {
                    
                }
                client.note = a;
                
                cv.client= client;
                details= sousCritere.getDetailsByCv(co, idCv[i]);
                
                cv.sousCritere= details;
                String[] pdf = cv.getPDF(co, idCv[i]);
                cv.pdfDiplome = pdf[0];
                cv.pdfAttestation = pdf[1];
                list.Add(cv);
            }
            
            return list;
        }

        //Enregistrer les notes dans la table note
        public static void SaveNoteCv(SqlConnection co,int idAnnonce)
        {
            if (co == null)
            {
                co = Connect.connectDB();
            }
            List<Cv> list = new List<Cv>();
            Cv cv1= new Cv();
            List<int> idCv= cv1.listeCvByAnnonce(co,idAnnonce);
            Clients client = new Clients();
            SousCritere sousCritere = new SousCritere();
            List<SousCritere> details= new List<SousCritere>();
            for(int i=0; i<idCv.Count; i++)
            {
                Cv cv = new Cv();
                cv.idCv= idCv[i];
                client= client.getClientAnnonceByCv(co, idAnnonce, idCv[i]);
                client.note = cv.getNoteCv(co, idCv[i],idAnnonce)+client.getNoteSexe(co,idAnnonce);
                
                cv.client= client;
                details= sousCritere.getDetailsByCv(co, idCv[i]);
                cv.sousCritere= details;
                String[] pdf = cv.getPDF(co, idCv[i]);
                cv.pdfDiplome = pdf[0];
                cv.pdfAttestation = pdf[1];
                list.Add(cv);
            }
            
        }
        public void insertNoteCV(SqlConnection co,int idCv, int note)
        {
            if (co == null)
            {
                co = Connect.connectDB();
            }
            String querry = "insert into note values(" + idCv + "," + note + ",0,0)";
            Console.WriteLine(querry);
            SqlCommand command = new SqlCommand(querry, co);
            command.ExecuteNonQuery();
        }
       

        public void inserer(SqlConnection con)
        {
            String sql = "INSERT INTO Cv VALUES ("+this.annonce.idAnnonce+","+this.client.idClient+",'"+this.pdfDiplome+"','"+this.pdfAttestation+"')";
            Console.WriteLine(sql);
            
            if (con == null)
            {
                con = Connect.connectDB();
            
            }
            SqlCommand command = new SqlCommand(sql,con);
            
            command.ExecuteNonQuery();
        }

        public static void insertDetailCv(int idCv,int idSousCritere,SqlConnection con)
        {
            String sql = "INSERT INTO detailsCv VALUES ("+idCv+","+idSousCritere+")";
            Console.WriteLine(sql);
            
            if (con == null)
            {
                con = Connect.connectDB();
            
            }
            SqlCommand command = new SqlCommand(sql,con);
            
            command.ExecuteNonQuery();
        }
        public void insertEtatClient(SqlConnection con)
        {
            String sql = "INSERT INTO etatClient VALUES ("+this.client.idClient+",0,"+this.annonce.idAnnonce+",'"+DateTime.Now+"',NULL,NULL)";
            Console.WriteLine(sql);
            
            if (con == null)
            {
                con = Connect.connectDB();
            
            }
            SqlCommand command = new SqlCommand(sql,con);
            
            command.ExecuteNonQuery();
        }

        public void changeEtatToEntretien(int idAnnonce,DateTime d,SqlConnection con)
        {
            String sql = "UPDATE etatClient SET etat=2, dateEntretien='"+d+"' WHERE idClient="+this.client.idClient+" and idAnnonce="+idAnnonce;
            Console.WriteLine(sql);
            
            if (con == null)
            {
                con = Connect.connectDB();
            
            }
            SqlCommand command = new SqlCommand(sql,con);
            
            command.ExecuteNonQuery();
        }
        public static int getIdCvByClientAnnonce(int idClient, int idAnnonce, SqlConnection con)
        {
            if (con == null)
            {
                con = Connect.connectDB();
            }

            String sql = "SELECT idCv FROM Cv WHERE idClient="+idClient+" and idAnnonce="+idAnnonce;
            SqlCommand command = new SqlCommand(sql,con);
            SqlDataReader reader = command.ExecuteReader();
            int id = 0;
            if (reader.Read())
            {
                id = (int)reader["idCv"];
            }
            reader.Close();
            return id;
        }

        public static int getNoteTeste(int idCv, SqlConnection con)
        {
            if (con == null)
            {
                con = Connect.connectDB();
            }

            String sql = "SELECT noteTeste FROM note WHERE idCv="+idCv;
            Console.WriteLine(sql+" fff");
            SqlCommand command = new SqlCommand(sql,con);
            SqlDataReader reader = command.ExecuteReader();
            double note = 0;
            int res = 0;
            if (reader.Read())
            {
                note = (double)reader["noteTeste"];
                res = (int)note;
            }
            reader.Close();

            return res;
        }

        public static int getNoteEntretien(int idCv, SqlConnection con)
        {
            if (con == null)
            {
                con = Connect.connectDB();
            }

            String sql = "SELECT noteEntretien FROM note WHERE idCv="+idCv;
            Console.WriteLine(sql+" fff");
            SqlCommand command = new SqlCommand(sql,con);
            SqlDataReader reader = command.ExecuteReader();
            double note = 0;
            int res = 0;
            if (reader.Read())
            {
                note = (double)reader["noteEntretien"];
                res = (int)note;
            }
            reader.Close();

            return res;
        }

        public static DateTime getDateEntretien(int idClient,int idAnnonce, SqlConnection con)
        {
            DateTime res = new DateTime();

            if (con == null)
            {
                con = Connect.connectDB();
            }

            String sql = "SELECT dateEntretien FROM etatClient WHERE idClient=" + idClient + " and idAnnonce=" + idAnnonce;

            SqlCommand command = new SqlCommand(sql,con);
            SqlDataReader reader = command.ExecuteReader();

            if (reader.Read())
            {
                res = (DateTime)reader["dateEntretien"];
            }

            return res;
        }
    }
   
}
