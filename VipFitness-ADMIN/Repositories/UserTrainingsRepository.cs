using Dapper;
using Microsoft.Data.SqlClient;
using NuGet.Protocol.Plugins;
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

        public bool AddUserTrainings(List<TrainingDataModel> trainingsData)
        {

            var GetList = GetUserTrainingsByUserId(trainingsData[0].UserId);

            if (GetList.Count() > 0)
            {
                using (var conn = new SqlConnection(connstring))
                {
                    conn.Open();
                    var query = "DELETE  FROM tblUserTrainings WHERE userId=@UserID";
                    var parameters = new { UserID = trainingsData[0].UserId };
                    conn.Query(query, parameters);
                    query = "INSERT INTO tblUserTrainings(userId,trainingId,setInfo,trainingType,isActive) VALUES(@UserID,@trainingID,@setInfo,@trainingType,@isActive)";
                    for (int i = 0; i < trainingsData.Count; i++)
                    {
                        var Insparameters = new { UserID = trainingsData[i].UserId, trainingID = trainingsData[i].TrainingId, setInfo = trainingsData[i].SetInfo, trainingType = trainingsData[i].TrainingType, isActive = trainingsData[i].IsActive };
                        var CreatedUser = conn.QuerySingleOrDefault<TrainingModel>(query, Insparameters);
                    }
                    return false;
                }
            }

            else 
            {
                using (var connection = new SqlConnection(connstring))
                {

                    connection.Open();
                    var query = "INSERT INTO tblUserTrainings(userId,trainingId,setInfo,trainingType,isActive) VALUES(@UserID,@trainingID,@setInfo,@trainingType,@isActive)";
                    for (int i = 0; i < trainingsData.Count; i++)
                    {
                        var parameters = new { UserID = trainingsData[i].UserId, trainingID = trainingsData[i].TrainingId, setInfo = trainingsData[i].SetInfo, trainingType = trainingsData[i].TrainingType, isActive = trainingsData[i].IsActive };
                        var CreatedUser = connection.QuerySingleOrDefault<TrainingModel>(query, parameters);
                    }
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

        public List<TrainingDataModel> GetUserTrainingsByUserId(int id)
        {
            using (var connection = new SqlConnection(connstring))
            {
                connection.Open();
                var query = "SELECT * FROM dbo.UserTrainings WHERE userId = @ID";
                var parameters = new { ID = id };
                var usertraining = connection.Query<TrainingDataModel>(query, parameters);

                return usertraining.ToList();
            }
        }

    }
}
