using VipFitness_ADMIN.Models;

namespace VipFitness_ADMIN.Repositories
{
    public interface IUserTrainingsRepository
    {
        IEnumerable<UserTrainings> GetAllUserTrainings();
        IEnumerable<UserTrainings> GetUserTrainingsById(int userId);
        bool AddUserTrainings(UserTrainings userTrainings);
        void UpdateUserTrainings(UserTrainings userTrainings);
        void DeleteUserTrainings(int id);
    }
}
