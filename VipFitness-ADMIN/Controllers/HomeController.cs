using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VipFitness_ADMIN.Repositories;

namespace VipFitness_ADMIN.Controllers
{
    public class HomeController : Controller
    {
        private readonly IAntiforgery _antiforgery;

        public HomeController(IAntiforgery antiforgery)
        {
            _antiforgery = antiforgery;
        }

        public ActionResult Home()
        {

            return View();
        }


        public ActionResult Users() 
        {
            return RedirectToAction("Users","Users"); 
        }

        public ActionResult Trainings()
        {
            return RedirectToAction("Trainings", "Trainings");
        }

        public ActionResult SignOut()
        {
            HttpContext.SignOutAsync();

            // Çerezleri temizle
            Response.Cookies.Delete("VipFitness");

            // Antiforgery token'ı yeniden oluştur (CSRF saldırılarına karşı koruma sağlar)
            ViewData["AntiForgeryToken"] = _antiforgery.GetAndStoreTokens(HttpContext).RequestToken;

            // Çıkış başarılı mesajını göster
            ViewBag.Message = "Başarıyla Çıkış Yaptınız.";

            // Giriş sayfasına yönlendir
            return View("../Login/Login");
        }

    }
}
