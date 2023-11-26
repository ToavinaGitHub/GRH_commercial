using GRH.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;


namespace GRH.Controllers
{
    public class FrontOfficeController : Controller
    {
        public IActionResult ListeAnnonce()
        {
            SqlConnection co = Connect.connectDB();
          
            var listAnnonce = Annonce.getAnnonceDispo(co);

            @ViewBag.all = listAnnonce;
            @ViewBag.allService = Services.getAllService(co);
            
            return View("Index");
        }

        public IActionResult Index()
        {
            return View("Login");
        }

        public IActionResult Inscription()
        {
            return View();
        }

        public IActionResult Cv()
        {

            SqlConnection co = Connect.connectDB();
            int idAnnonce = int.Parse(Request.Query["idAnnonce"]);
            
            Annonce a = Annonce.getAnnonceById(co,idAnnonce);

            @ViewBag.allCritere = Critere.getAll(co);
            @ViewBag.annonce = a;
            
            return View();
        }

        [HttpPost]
        public IActionResult CheckLogin()
        {
            String email = Request.Form["email"];
            String mdp = Request.Form["mdp"];

            SqlConnection co = Connect.connectDB();
            
            if (co == null)
            {
                co = Connect.connectDB();
            }

            Clients c = Clients.checkLogin(email, mdp, co);
            
            if (c != null)
            {
                HttpContext.Session.SetString("userSession",c.idClient.ToString());
                
                return RedirectToAction("ListeAnnonce", "FrontOffice");
            }
            else
            {
                return RedirectToAction("Index", "FrontOffice");
            }
            
        }
        
        [HttpPost]
        public IActionResult Insc()
        {
            SqlConnection co = Connect.connectDB();
            
            if (co == null)
            {
                co = Connect.connectDB();
            }

            String email = Request.Form["email"];
            String mdp = Request.Form["mdp"];
            String nom = Request.Form["nom"];
            String prenom = Request.Form["prenom"];
            String dtn = Request.Form["dtn"];
            String adresse = Request.Form["adresse"];
            int sexe = int.Parse(Request.Form["sexe"]);

            DateTime dateTimeValue;
            
            if (DateTime.TryParse(dtn, out dateTimeValue))
            {

                Clients cli = new Clients(1,nom,prenom,dateTimeValue,email,mdp,adresse,sexe);
                cli.insert(sexe,co);
            }
            return RedirectToAction("Index", "FrontOffice");

        }

        public IActionResult logout()
        {
            
            HttpContext.Session.Remove("userSession");
            return RedirectToAction("Index", "FrontOffice");
        }

        [HttpPost]
        public IActionResult insertCv()
        {
            SqlConnection co = Connect.connectDB();
            int idClient = int.Parse(HttpContext.Session.GetString("userSession"));
            Clients clients = Clients.getClientsById(idClient,co);
            int idAnnonce = int.Parse(Request.Form["idAnnonce"]);
            Annonce annonce = Annonce.getAnnonceById(co,idAnnonce);
            
            var dipPdfFile = Request.Form.Files["diplPdf"];
            var attPdfFile = Request.Form.Files["attPdf"];

            String pdfDip = dipPdfFile.FileName;
            String pdfAtt = attPdfFile.FileName;

            Cv cv = new Cv(1,clients,annonce,pdfDip,pdfAtt);
            
            Console.WriteLine(clients.idClient+"--"+annonce.idAnnonce+"--"+pdfDip+"--"+pdfAtt);
            co = Connect.connectDB();
            
            cv.inserer(co);

            int idCv = Models.Cv.getLastIdCv(co);

            List<Critere> allCritere = Critere.getAll(co);
        
            co = Connect.connectDB();
            foreach (Critere critere in allCritere)
            {
                int idSous = int.Parse(Request.Form[critere.idCritere.ToString()]);
                Models.Cv.insertDetailCv(idCv,idSous,co);
            }
            
            cv.insertEtatClient(co);
            
            if (dipPdfFile != null && attPdfFile != null)
            {
                
                var dipPdfFileName = idCv+".pdf";
                var attPdfFileName = idCv+".pdf";
                
                var localPathD = Path.Combine("D:/GitHub_clone/GRH/GRH/wwwroot/pdfDiplome");
                var localPathA = Path.Combine("D:/GitHub_clone/GRH/GRH/wwwroot/pdfAttestation");
                
                Directory.CreateDirectory(localPathD);
                Directory.CreateDirectory(localPathA);
                
                var dipPdfLocalFileName = Path.Combine(localPathD, dipPdfFileName);
                var attPdfLocalFileName = Path.Combine(localPathA, attPdfFileName);
                
                using (var dipPdfFileStream = new FileStream(dipPdfLocalFileName, FileMode.Create))
                {
                    dipPdfFile.CopyTo(dipPdfFileStream);
                }

                using (var attPdfFileStream = new FileStream(attPdfLocalFileName, FileMode.Create))
                {
                    attPdfFile.CopyTo(attPdfFileStream);
                }
            }
            
            return RedirectToAction("ListeAnnonce", "FrontOffice");
        }

        public IActionResult QCM()
        {
            SqlConnection con = Connect.connectDB();
            int idAnnonce = int.Parse(Request.Query["idAnnonce"]);

            Question q = new Question();
            List<Question> allQ = q.questionsByAnnonce(con,idAnnonce);

            @ViewBag.allQuestion = allQ;
            @ViewBag.idAnnonce = idAnnonce;
            
            return View();
        }

        [HttpPost]
        public IActionResult submitQcm()
        {
            SqlConnection co = Connect.connectDB();
            int idAnnonce = int.Parse(Request.Form["idAnnonce"]);

            
            
            Question q = new Question();
            List<Question> allQ = q.questionsByAnnonce(co,idAnnonce);

            int note = 0;
            
            
            foreach (Question question in allQ)
            {
                List<Reponse> allRep = Reponse.getReponseQuestion(question,Connect.connectDB());

                String[] allIdReponse = Request.Form[question.idQuestion.ToString()];
                int o = 0;
                foreach (Reponse rep in allRep)
                {
                    if (rep.etat == 1)
                    {
                        for (int i = 0; i < allIdReponse.Length; i++)
                        {
                            if(int.Parse(allIdReponse[i])!=rep.idReponse)
                            {
                                o++;
                            }
                        }
                    }
                }
                if (o == 0)
                {
                    note += question.coef;
                }
            }
            int idClient = int.Parse(HttpContext.Session.GetString("userSession"));
            Console.WriteLine(note+"------------------------");
            co = Connect.connectDB();
            int idCv = Models.Cv.getIdCvByClientAnnonce(idClient,idAnnonce,co);
            Clients.updateNoteTest(idCv,note,co);
            Clients.updateEtatEfaTest(idAnnonce,idClient,co);
            
            return RedirectToAction("ListeAnnonce", "FrontOffice");
        }

        public IActionResult toDemandeConge()
        {
            SqlConnection con = Connect.connectDB();

            List<Raison> allRaison = Raison.getAll(con);

            int idClient = int.Parse(HttpContext.Session.GetString("userSession"));
            
            
            Mpiasa mp = Mpiasa.getByIdClient(idClient,con);

            @ViewBag.restant = mp.congeRestant(con);
            @ViewBag.allRaison = allRaison;
            return View("DemandeConge");
        }

        [HttpPost]
        public IActionResult demandeConge()
        {
            SqlConnection con = Connect.connectDB();
            
            
            int idClient = int.Parse(HttpContext.Session.GetString("userSession"));
            Mpiasa mpiasa = Mpiasa.getByIdClient(idClient, con);

            int idRaison = int.Parse(Request.Form["idRaison"]);
            Raison rs = Raison.getById(idRaison,con);

            DateTime debut;
            DateTime fin;
            
            if (DateTime.TryParse(Request.Form["debut"], out debut) && DateTime.TryParse(Request.Form["fin"], out fin))
            {
                DemandeConge dm = new DemandeConge(1,mpiasa,debut,fin,rs,0);
                dm.save(con);
            }
            
            return RedirectToAction("ListeAnnonce", "FrontOffice");
        }
        
    }
    
}
