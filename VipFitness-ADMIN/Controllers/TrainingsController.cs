using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VipFitness_ADMIN.Models;
using VipFitness_ADMIN.Repositories;

namespace VipFitness_ADMIN.Controllers
{
    public class TrainingsController : Controller
    {
        private readonly ITrainingRepository _trainingRepository;
        public ActionResult Trainings()
        {
            var trainings = _trainingRepository.GetAllTrainings();
            return View(trainings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateTraining(TrainingModel model)
        {
            if (model == null)
            {
                ModelState.AddModelError("", "Geçersiz istek. Lütfen tekrar deneyin.");
                return View("Error"); // Hata sayfasına yönlendir
            }

            bool registered = _trainingRepository.AddTraining(model);
            if (registered)
            {
                return RedirectToAction("Trainings");
            }
            else
            {
                // Kullanıcı kaydedilemedi, uygun bir hata mesajı döndür
                ModelState.AddModelError("", "Kullanıcı kaydedilemedi. Lütfen tekrar deneyin.");
                return View("Error"); // Hata sayfasına yönlendir
            }

        }


        public JsonResult GetTrainingDetails(int Id)
        {
            var training = _trainingRepository.GetTrainingById(Id);


            return Json(training);
        }

        public ActionResult Edit(TrainingModel model)
        {
            _trainingRepository.UpdateTraining(model);

            return RedirectToAction("Trainings");
        }

        public ActionResult Delete(int Id)
        {
            _trainingRepository.DeleteTraining(Id);
            return RedirectToAction("Trainings");
        }


    }
}
