using Microsoft.AspNetCore.Mvc;
using VipFitness_ADMIN.Models;
using VipFitness_ADMIN.Repositories;

namespace VipFitness_ADMIN.Controllers
{
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
            var userTrainings = _userTrainingsRepository.GetUserTrainingsById(UserID);
            var user = _userRepository.GetUserById(UserID);
            ViewBag.ActiveTrainings = _trainingRepository.GetActiveTrainings();



            if (userTrainings.Count() !> 0 && user != null)
            {
                ViewBag.SelectedUser = user;
                userTrainings = null;
            }

            return View(userTrainings);
        }

    }
}
