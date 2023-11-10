using Dapper;
using VipFitness_ADMIN.Models;
using Microsoft.Data.SqlClient;
using System.Text;
using System.Security.Cryptography;

namespace VipFitness_ADMIN.Repositories
{
    public class UserRepository : IUserRepository
    {
        private string connstring;

        public UserRepository(IConfiguration configuration)
        {
            connstring = configuration.GetConnectionString("DefaultConnection");
        }


        public IEnumerable<UserModel> GetAllUsers()
        {
            using (var connection = new SqlConnection(connstring))
            {
                connection.Open();

                var query = "SELECT id,username,UType, CASE WHEN UType=0 THEN 'Super Admin' WHEN UType=1 THEN 'Admin' ELSE 'Kullanıcı' END UTypeString ,isdeleted FROM tblUsers WHERE isdeleted=0";
                var users = connection.Query<UserModel>(query);

                return users;
            }
        }
        public static string HashPassword(string password)
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        public bool AddUser(UserModel user)
        {
            bool ExistUser = false;

            // User exist OR NOT  

            var GetList = GetAllUsers();
            foreach (var item in GetList)
            {
                if (item.username == user.username)
                {
                    ExistUser = true; break;
                }
            }
            if (!ExistUser)
            {
                using (var connection = new SqlConnection(connstring))
                {

                    connection.Open();
                    var query = "INSERT INTO tblUsers(username,password,UType,isdeleted) VALUES(@UserName,@Password,@UType,@IsDeleted)";
                    var parameters = new { UserName = user.username, Password = HashPassword(user.password), UType = user.UType, IsDeleted = user.isdeleted };
                    var CreatedUser = connection.QuerySingleOrDefault<UserModel>(query, parameters);

                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public void UpdateUser(UserModel user)
        {
            try
            {
                using (var connection = new SqlConnection(connstring))
                {
                    connection.Open();
                    string query = "";
                    if (user.password == null)
                    {
                        query = "Update tblUsers SET username=@UserName,UType=@UType WHERE id=@UserID";
                        var parameters = new { UserName = user.username, UType = user.UType, UserID = user.id };
                        connection.QuerySingleOrDefault<UserModel>(query, parameters);
                    }
                    else
                    {
                        query = "Update tblUsers SET username=@UserName,password=@Password,UType=@UType WHERE id=@UserID";
                        var parameters = new { UserName = user.username, Password = HashPassword(user.password), UType = user.UType, UserID = user.id };
                        connection.QuerySingleOrDefault<UserModel>(query, parameters);
                    }

                }
            }
            catch (Exception ex)
            {
                object value = ex;
            }

        }
        public void DeleteUser(int userId)
        {
            using (var connection = new SqlConnection(connstring))
            {
                connection.Open();

                var query = "UPDATE tblUsers SET isdeleted=1 WHERE id = @UserID";
                var parameters = new { UserID = userId };
                connection.QuerySingleOrDefault<UserModel>(query, parameters);
            }

        }
        public UserModel GetUserById(int userId)
        {
            using (var connection = new SqlConnection(connstring))
            {
                connection.Open();

                var query = "SELECT id,username,UType,isdeleted FROM tblUsers WHERE id = @UserID";
                var parameters = new { UserID = userId };
                var user = connection.QuerySingleOrDefault<UserModel>(query, parameters);
                user.UTypeString = ReturnMyType(user.UType);
                return user;
            }
        }

        private string ReturnMyType(int value)
        {
            switch (value)
            {
                case 0: return "Super Admin";
                case 1: return "Admin";
                case 2: return "Kullanıcı";
                default: return "";
            }
        }
    }

}

