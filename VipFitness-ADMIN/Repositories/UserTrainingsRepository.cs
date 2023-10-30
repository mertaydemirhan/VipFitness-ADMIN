using Dapper;
using Microsoft.Data.SqlClient;
using VipFitness_ADMIN.Models;

namespace VipFitness_ADMIN.Repositories
{
    public class UserTrainingsRepository : IUserTrainingsRepository
    {
        private string connstring;
        public UserTrainingsRepository(IConfiguration configuration)
        {
            connstring = configuration.GetConnectionString("DefaultConnection");
        }

        public bool AddUserTrainings(UserTrainings userTrainings)
        {
            bool ExistUserTraining = false;

            // Training exist OR NOT  
            /* DÜZENLENECEK.... */

            var GetList = GetAllUserTrainings();
            if (GetList.Count() > 0)
            {
                return false;
            }

            else 
            {
                using (var connection = new SqlConnection(connstring))
                {

                    connection.Open();
                    var query = "INSERT INTO tblUserTrainings(userId,trainingId,setInfo,trainingType,isActive) VALUES(@UserID,@trainingID,@setInfo,@trainingType,@isActive)";
                    var parameters = new { UserID = userTrainings.userID, trainingID = userTrainings.trainingID, setInfo = userTrainings.setInfo, trainingType = userTrainings.trainingType, isActive = userTrainings.isActive };
                    var CreatedUser = connection.QuerySingleOrDefault<TrainingModel>(query, parameters);

                    return true;
                }
            }

        }

        public void DeleteUserTrainings(int id)
        {
            using (var connection = new SqlConnection(connstring))
            {
                connection.Open();

                var query = "UPDATE tblUserTrainings SET isActive=0 WHERE id = @ID";
                var parameters = new { ID = id };
                connection.QuerySingleOrDefault<UserTrainings>(query, parameters);
            }
        }

        public IEnumerable<UserTrainings> GetAllUserTrainings()
        {

            using (var connection = new SqlConnection(connstring))
            {
                string query = "SELECT * FROM dbo.UserTrainings";
                return connection.Query<UserTrainings>(query);
            }

        }

        public IEnumerable<UserTrainings> GetUserTrainingsById(int id)
        {
            using (var connection = new SqlConnection(connstring))
            {
                connection.Open();
                var query = "SELECT * FROM dbo.UserTrainings WHERE id = @ID";
                var parameters = new { ID = id };
                var usertraining = connection.QuerySingleOrDefault<UserTrainings>(query, parameters);

                yield return usertraining;
            }
        }

        public void UpdateUserTrainings(UserTrainings userTrainings)
        {
            try
            {
                using (var connection = new SqlConnection(connstring))
                {
                    connection.Open();

                    var query = "Update tblUserTrainings SET userId=@userID,trainingId=@trainingID,setInfo=@setINFO,trainingType=@trainingType,isActive=@IsActive WHERE id=@ID";
                    var parameters = new 
                    {
                        userId = userTrainings.userID,
                        trainingId = userTrainings.trainingID,
                        setINFO = userTrainings.setInfo,
                        trainingType = userTrainings.trainingType,
                        IsActive = userTrainings.isActive,
                        ID = userTrainings.id
                    };
                    connection.QuerySingleOrDefault<UserTrainings>(query, parameters);
                }
            }
            catch (Exception ex)
            {
                //
            }
        }
    }
}
