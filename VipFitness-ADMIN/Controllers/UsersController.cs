using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VipFitness_ADMIN.Repositories;
using VipFitness_ADMIN.Models;

namespace VipFitness_ADMIN.Controllers
{
    public class UsersController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UsersController(IUserRepository userRepository)
        {
            _userRepository = userRepository;

        }
        public ActionResult Users()
        {
            var users = _userRepository.GetAllUsers();
            return View(users);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateUser(UserModel model)
        {
            if (model == null)
            {
                ModelState.AddModelError("", "Geçersiz istek. Lütfen tekrar deneyin.");
                return View("Error"); // Hata sayfasına yönlendir
            }

            bool registered = _userRepository.AddUser(model);
            if (registered)
            {
                // Kullanıcı başarılı bir şekilde kaydedildi, kullanıcıları listeleyen sayfaya yönlendir
                return RedirectToAction("Users");
            }
            else
            {
                // Kullanıcı kaydedilemedi, uygun bir hata mesajı döndür
                ModelState.AddModelError("", "Kullanıcı kaydedilemedi. Lütfen tekrar deneyin.");
                return View("Error"); // Hata sayfasına yönlendir
            }

        }

        public IActionResult AddTrainingToUser(int userID)
        {
            return RedirectToAction("UserTrainings", "UserTrainings", new { userID = userID });
        }

        public JsonResult GetUserDetails(int Id)
        {
            var user = _userRepository.GetUserById(Id);


            return Json(user);
        }


        // GET: UsersController/Edit/5
        public ActionResult Edit(UserModel model)
        {
            _userRepository.UpdateUser(model);

            return RedirectToAction("Users");
        }


        // GET: UsersController/Delete/5
        public ActionResult Delete(int Id)
        {
            _userRepository.DeleteUser(Id);
            return RedirectToAction("Users");
        }


    }
}
