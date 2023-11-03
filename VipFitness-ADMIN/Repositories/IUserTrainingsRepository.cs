using VipFitness_ADMIN.Models;

namespace VipFitness_ADMIN.Repositories
{
    public interface IUserTrainingsRepository
    {
        IEnumerable<UserTrainings> GetAllUserTrainings();
        List<TrainingDataModel> GetUserTrainingsByUserId(int userId);
        bool AddUserTrainings(List<TrainingDataModel> trainingDat);
        void DeleteUserTrainings(int id);
    }
}
