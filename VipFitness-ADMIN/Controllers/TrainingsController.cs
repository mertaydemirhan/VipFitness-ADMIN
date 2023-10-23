using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
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
            var categories = _trainingRepository.GetTrainingCategories();
            var trainingModels = trainings.Select(training => new TrainingModel
            {
                Id = training.Id,
                TrainingName = training.TrainingName,
                TrainingCategoryId = training.TrainingCategoryId, 
                Explanation = training.Explanation,
                ImgData = training.ImgData,
                Isdeleted = training.Isdeleted,
                ImgExt = training.ImgExt,
                ImgDataString = training.ImgDataString,
                TrainingCat = categories
            });

            ViewBag.TrainingCategories = new SelectList(categories, "Id", "CategoryName");

            return View(trainingModels);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateTraining(TrainingModel model, IFormFile ImgData)
        {
            if (ImgData != null && ImgData.Length > 0)
            {
                var fileName = Path.GetFileName(ImgData.FileName);
                var fileExtension = Path.GetExtension(fileName);
                string extensionWithoutDot = fileExtension.TrimStart('.');

                // Dosya uzantısını veritabanında saklayabilirsiniz (model.ImgExt = fileExtension)

                using (var memoryStream = new MemoryStream())
                {
                    ImgData.CopyTo(memoryStream);
                    model.ImgData = memoryStream.ToArray(); // IFormFile'ı byte[] dizisine dönüştür ve modele ata
                    model.ImgExt = extensionWithoutDot;
                }
            }

            if (model == null)
            {
                ModelState.AddModelError("", "Geçersiz istek. Lütfen tekrar deneyin.");
                return View("Error"); // Hata sayfasına yönlendir
            }


            bool registered = _trainingRepository.AddTraining(model);
            if (registered)
            {
                return Json(new { success = true, message = "Antrenman başarıyla oluşturuldu." });
            }
            else
            {
                // Kullanıcı kaydedilemedi, uygun bir hata mesajı döndür
                ModelState.AddModelError("", "Antreman kaydedilemedi. Lütfen tekrar deneyin.");
                return Json(new { success = true, message = "Antrenman başarıyla oluşturuldu." });// Hata sayfasına yönlendir
            }

        }



        public JsonResult GetTrainingDetails(int Id)
        {
            var training = _trainingRepository.GetTrainingById(Id);

            // Resim verisini Base64'e dönüştür
            if (training != null && training.ImgData != null && training.ImgData.Length > 0)
            {
                training.ImgDataString = Convert.ToBase64String(training.ImgData);
            }

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
