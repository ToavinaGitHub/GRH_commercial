using GRH.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace GRH.Controllers
{
    public class FrontOfficeClientController : Controller
    {
        public IActionResult LoginClient()
        {
            return View("~/Views/FrontOfficeClient/LoginClient.cshtml");
        }
        public IActionResult Accueil()
        {
            return View("~/Views/FrontOfficeClient/Accueil.cshtml");
        }
        public IActionResult checkLogin()
        {
            String email = Request.Form["email"];
            String mdp = Request.Form["mdp"];

            SqlConnection co = Connect.connectDB();

            if (co.State != ConnectionState.Open)
            {
                co = Connect.connectDB();
            }
            ClientVente cli = ClientVente.checkLogin(email, mdp,co);

            
            if (cli != null)
            {
                HttpContext.Session.SetString("sess", cli.idClientVente.ToString());
                HttpContext.Session.SetString("user", "Client");
                return RedirectToAction("Accueil", "FrontOfficeClient");
            }
           
            else
            {
                return RedirectToAction("LoginClient", "FrontControllerClient");
            }


        }
    }
}
