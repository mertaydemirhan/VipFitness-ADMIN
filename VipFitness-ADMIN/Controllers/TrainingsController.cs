using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VipFitness_ADMIN.Models;
using VipFitness_ADMIN.Repositories;

namespace VipFitness_ADMIN.Controllers
{
    public class TrainingsController : Controller
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        private readonly ITrainingRepository _trainingRepository;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public TrainingsController(ITrainingRepository trainingRepository)
        {
            _trainingRepository = trainingRepository;
        }


        public ActionResult Trainings()
        {
            var trainings = _trainingRepository.GetAllTrainings();
            return View(trainings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTraining(TrainingModel model, IFormFile ImgData)
        {


            // ImgData ile alakalı işlem yapılacak beklemede....
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
