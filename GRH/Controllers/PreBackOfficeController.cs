using System.Data;
using GRH.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.Data.SqlClient;

namespace GRH.Controllers
{
    public class PreBackOfficeController : Controller
    {
        private readonly ILogger<PreBackOfficeController> _logger;

        public PreBackOfficeController(ILogger<PreBackOfficeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View("~/Views/Preback/Login.cshtml");
        }

        [HttpPost]
        public IActionResult checkLogin()
        {
            String email = Request.Form["email"];
            String mdp = Request.Form["mdp"];

            SqlConnection co = Connect.connectDB();
            
            if (co.State != ConnectionState.Open)
            {
                co = Connect.connectDB();
            }
            
            Services s = Services.checkLogin(email,mdp,co);
            RH r = RH.checkLogin(email, mdp, co);
            
            if (s != null)
            {
                HttpContext.Session.SetString("sess",s.idService.ToString());
                HttpContext.Session.SetString("user","service");
                return RedirectToAction("", "Home");
            }else if( r != null)
            {
                HttpContext.Session.SetString("sess",r.idRh.ToString());
                HttpContext.Session.SetString("user","rh");
                return RedirectToAction("", "Home");
            }
            else
            {
                return RedirectToAction("Index", "PreBackOffice");
            }


        }

        public IActionResult logout()
        {
            HttpContext.Session.Remove("sess");
            HttpContext.Session.Remove("user");
            return RedirectToAction("Index", "PreBackOffice");
        }
    }
}