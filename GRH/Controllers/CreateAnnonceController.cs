using System.Data;
using GRH.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Data.SqlClient;

namespace GRH.Controllers
{
    public class CreateAnnonceController : Controller
    {
        private readonly ILogger<CreateAnnonceController> _logger;

        public CreateAnnonceController(ILogger<CreateAnnonceController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            int idService = int.Parse(HttpContext.Session.GetString("sess"));
            SqlConnection c = Connect.connectDB();
            
            List<Critere> allC = Critere.getAll(c);
            List<Postes> allP = Postes.getAllPosteByService(idService, c);
           
            
            @ViewBag.con = c;
            @ViewBag.allPoste = allP;

            @ViewBag.allCritere = allC;
            
            
            
            return View("~/Views/Annonce/Index.cshtml");
        }

    
        [HttpPost]
        public IActionResult inserer()
        {
            SqlConnection co = Connect.connectDB();
            
            if (co.State != ConnectionState.Open)
            {
                co = Connect.connectDB();
            }
            
            List<SousCritere> allS = SousCritere.getAll(co);
            
            int idPoste = int.Parse(Request.Form["idPoste"]);
            String desc = Request.Form["description"];
            int tache = int.Parse(Request.Form["tache"]);
            int jh = int.Parse(Request.Form["jourHomme"]);

            Annonce a = new Annonce(1,Postes.getById(idPoste,co),desc,tache,jh,DateTime.Now,0);
            
            a.Insert(co);
            
            Annonce current = Annonce.getLastAnnonce(co);

            foreach (SousCritere s in SousCritere.getAll(co))
            {
                CritereCoef cTemp = new CritereCoef(1,s,int.Parse(Request.Form[s.idSousCritere.ToString()]),current);
                cTemp.Insert(co);
            }
            
            return RedirectToAction("", "Home");
            
        }
      
    }
}