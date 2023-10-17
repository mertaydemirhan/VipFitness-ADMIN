using VipFitness_ADMIN.Models;

namespace VipFitness_ADMIN.Repositories
{
    public interface IUserRepository
    {
        IEnumerable<UserModel> GetAllUsers();
        UserModel GetUserById(int userId);
        bool AddUser(UserModel user);
        void UpdateUser(UserModel user);
        void DeleteUser(int userId);
    }
}
