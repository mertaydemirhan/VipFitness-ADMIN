using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using VipFitness_ADMIN.Models;
using VipFitness_ADMIN.Repositories;

namespace VipFitness_ADMIN.Controllers
{
    public class TrainingCatController : Controller
    {
        private readonly ITrainingCatRepository _trainingCatRepository;
        public TrainingCatController(ITrainingCatRepository trainingCatRepository)
        {
            _trainingCatRepository = trainingCatRepository;
        }
        public ActionResult Create()
        {
            var categories = _trainingCatRepository.GetTrainingCategories();
            ViewBag.Categories = new SelectList(categories, "Id", "CategoryName");
            return View();
        }

        [HttpPost]
        public IActionResult Create(TrainingModel model)
        {
            // TrainingModel verilerini kaydetme işlemi burada gerçekleştirilir.
            // ...
            return RedirectToAction("Index");
        }

    }
}
