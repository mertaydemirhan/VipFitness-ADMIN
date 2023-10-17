using VipFitness_ADMIN.Models;

namespace VipFitness_ADMIN.Repositories
{
    public interface ITrainingRepository
    {
        IEnumerable<TrainingModel> GetAllTrainings();
        TrainingModel GetTrainingById(int TrainingId);
        bool AddTraining(TrainingModel Training);
        void UpdateTraining(TrainingModel Training);
        void DeleteTraining(int TrainingId);
    }
}
