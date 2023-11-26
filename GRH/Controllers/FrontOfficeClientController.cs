using GRH.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Data.Common;
using System.Net.Mail;
using System.Net.Mime;
using System.Net;

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

        public IActionResult DemandeProforma()
        {
            SqlConnection co = Connect.connectDB();

            if (co.State != ConnectionState.Open)
            {
                co = Connect.connectDB();
            }

            List<ArticleVente> listArticle = ArticleVente.GetAll(co);
            ViewBag.listArticle = listArticle;

            return View("~/Views/FrontOfficeClient/DemandeProforma.cshtml");
        }

        public IActionResult sendProforma()
        {
            SqlConnection co = Connect.connectDB();

            if (co.State != ConnectionState.Open)
            {
                co = Connect.connectDB();
            }
            int idArticle = int.Parse(Request.Form["article"]);
            double quantite = double.Parse(Request.Form["quantite"]);
            int uniteArticle = int.Parse(Request.Form["unite"]);
            int idClientVente = Convert.ToInt32(HttpContext.Session.GetString("sess"));
            DateTime date = DateTime.Now;

            ProformaVente proformaVente = new ProformaVente
            {
                articleVente = new ArticleVente { idArticleVente = idArticle },
                quantite = quantite,
                date = date,
                unite = new UniteArticle { idUniteArticle = uniteArticle},
                client = new ClientVente { idClientVente = idClientVente }
            };
            proformaVente.save(co);


            ProformaVente dernierProforma = ProformaVente.GetLast(co);
            UniteArticle unite = UniteArticle.GetUniteBase(co,proformaVente.articleVente.idArticleVente);
            string nomClient =ClientVente.GetClientNameById(co, dernierProforma.client.idClientVente);

            try
            {
                
                using (var smtpClient = new SmtpClient("smtp.gmail.com"))
                {
                    smtpClient.Port = 587;
                    smtpClient.Credentials = new NetworkCredential("tokywilly03@gmail.com", "mgsu utre byfi eaga");
                    smtpClient.EnableSsl = true;

                    var message = new MailMessage
                    {
                        From = new MailAddress("tokywilly03@gmail.com"),
                        Subject = "Proforma.",
                       
                        Body = $"Cher Service,\n\n{nomClient} demande un proforma de {dernierProforma.articleVente.nomArticle} pour une quantité de {dernierProforma.quantite} {unite.nomUnite}\n\nMerci d'avance.\n\nCordialement,\nDimpex\n 034 78 684 89 ",
                        IsBodyHtml = false
                    };

                    
                    message.To.Add("tokywilly03@gmail.com");
                    //var pdfAttachment = new Attachment("wwwroot/pdfProforma/" + dernierProforma.idProformaVente + ".pdf", MediaTypeNames.Application.Pdf);
                    //message.Attachments.Add(pdfAttachment);


                    smtpClient.Send(message);
                    Console.Out.WriteLine("ato ");
                    ViewBag.Message = "Email sent successfully!";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Error = $"Error sending email: {ex.Message}";
            }
            return  RedirectToAction("DemandeProforma", "FrontOfficeClient");
        }

        // Controleur amle JS
        [HttpGet]
        public JsonResult GetUnitsByArticle(int articleId)
        {
            SqlConnection co = Connect.connectDB();
            List<UniteArticle> uniteArticles = UniteArticle.getAllByIdArticle(co, new ArticleVente { idArticleVente = articleId });

            return Json(uniteArticles);
        }

        public IActionResult logout()
        {
            HttpContext.Session.Remove("sess");
            return RedirectToAction("LoginClient", "FrontOfficeClient");
        }

    }
}
