using Microsoft.AspNetCore.Mvc;
using VipFitness_ADMIN.Models;
using VipFitness_ADMIN.Repositories;

namespace VipFitness_ADMIN.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserTrainingsController : Controller
    {

        private readonly IUserTrainingsRepository _userTrainingsRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITrainingRepository _trainingRepository;


        public UserTrainingsController(IUserTrainingsRepository userTrainingRepository, IUserRepository userRepository, ITrainingRepository trainingRepository)
        {
            _userTrainingsRepository = userTrainingRepository;
            _userRepository = userRepository;
            _trainingRepository = trainingRepository;
        }

        
        public ActionResult UserTrainings(int UserID)
        {
            var userTrainings = _userTrainingsRepository.GetUserTrainingsByUserId(UserID);
            var user = _userRepository.GetUserById(UserID);
            ViewBag.ActiveTrainings = _trainingRepository.GetActiveTrainings();

            ViewBag.SelectedUser = user;
            if ((userTrainings.Count() == 0) && user != null)
                userTrainings = null;
            else
            {
                _ = userTrainings.Count();
            }

            return View(userTrainings);
        }

        [HttpPost("GetDatas")]
        public JsonResult GetDatas([FromBody] List<TrainingDataModel> trainingData)
        {
            _userTrainingsRepository.AddUserTrainings(trainingData);
            var result = new
            {
                success = true,
                message = "Veriler başarıyla alındı."
            };

            return Json(result);
        }

    }
}
