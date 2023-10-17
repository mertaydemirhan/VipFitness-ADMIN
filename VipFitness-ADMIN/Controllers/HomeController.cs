using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VipFitness_ADMIN.Repositories;

namespace VipFitness_ADMIN.Controllers
{
    public class HomeController : Controller
    {

        
        public ActionResult Home()
        {

            return View();
        }


        public ActionResult Users() 
        {
            return RedirectToAction("Users","Users"); 
        }



    }
}
