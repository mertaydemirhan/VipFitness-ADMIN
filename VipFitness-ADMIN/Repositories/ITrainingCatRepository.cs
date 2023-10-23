using VipFitness_ADMIN.Models;

namespace VipFitness_ADMIN.Repositories
{
    public interface ITrainingCatRepository
    {
        IEnumerable<TrainingCatRepository> GetTrainingCategories();
    }
}
