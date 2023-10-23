using Dapper;
using VipFitness_ADMIN.Models;
using Microsoft.Data.SqlClient;
using VipFitness_ADMIN.Controllers;
using System.Reflection;
using Microsoft.IdentityModel.Tokens;
using NuGet.Protocol;

namespace VipFitness_ADMIN.Repositories
{
    public class TrainingRepository : ITrainingRepository
    {
        private string connstring;
        public TrainingRepository(IConfiguration configuration)
        {
            connstring = configuration.GetConnectionString("DefaultConnection");
        }
        public IEnumerable<TrainingCategory> GetTrainingCategories()
        {
            using (var connection = new SqlConnection(connstring))
            {
                string query = "SELECT * FROM tblTrainingCategory";
                return connection.Query<TrainingCategory>(query);
            }
        }
        public IEnumerable<TrainingModel> GetAllTrainings()
        {
            using (var connection = new SqlConnection(connstring))
            {
                connection.Open();

                var query = "SELECT * FROM tblTrainings WHERE isdeleted=0";
                var trainings = connection.Query<TrainingModel>(query);
                return trainings;
            }
        }

        public bool AddTraining(TrainingModel Training)
        {
            bool ExistTraining = false;

            // Training exist OR NOT  

            var GetList = GetAllTrainings();
            foreach (var item in GetList)
            {
                if (item.TrainingName == Training.TrainingName)
                {
                    ExistTraining = true; break;
                }
            }
            if (!ExistTraining)
            {
                using (var connection = new SqlConnection(connstring))
                {

                    connection.Open();
                    string base64String = Convert.ToBase64String(Training.ImgData);
                    var query = "INSERT INTO tblTrainings(TrainingName,TrainingCategoryId,Explanation,ImgData,isdeleted,ImgExt) VALUES(@TrainingName,@TraningCategory,@Explanation,@ImgData,@IsDeleted,@ImgExt)";
                    var parameters = new { TrainingName = Training.TrainingName, TraningCategory = Training.TrainingCategoryId, Explanation = Training.Explanation, ImgData = Training.ImgData, IsDeleted = Training.Isdeleted, ImgExt = Training.ImgExt };
                    var CreatedUser = connection.QuerySingleOrDefault<TrainingModel>(query, parameters);

                    return true;
                }
            }
            else
            {
                return false;
            }
        }

        public void UpdateTraining(TrainingModel Training)
        {
            try
            {
                using (var connection = new SqlConnection(connstring))
                {
                    connection.Open();
                 
                        var query = "Update tblTrainings SET TrainingName=@TrainingName,TrainingCategoryId=@TraningCategory,Explanation=@Explanation,ImgData=@ImgData,isdeleted=@IsDeleted,ImgExt=@ImgExt WHERE id=@ID";
                        var parameters = new { TrainingName = Training.TrainingName, TraningCategory = Training.TrainingCategoryId, Explanation = Training.Explanation, ImgData = Training.ImgData, IsDeleted = Training.Isdeleted, ID= Training.Id, ImgExt = Training.ImgExt };
                        connection.QuerySingleOrDefault<TrainingModel>(query, parameters);
                }
            }
            catch (Exception ex)
            {
               //
            }

        }

        public void DeleteTraining(int Id)
        {
            using (var connection = new SqlConnection(connstring))
            {
                connection.Open();

                var query = "UPDATE tblTrainings SET isdeleted=1 WHERE id = @ID";
                var parameters = new { ID = Id };
                connection.QuerySingleOrDefault<TrainingModel>(query, parameters);
            }

        }

        public TrainingModel GetTrainingById(int Id)
        {
            using (var connection = new SqlConnection(connstring))
            {
                connection.Open();
                var query = "SELECT * FROM tblTrainings WHERE id = @ID";
                var parameters = new { ID = Id };
                var training = connection.QuerySingleOrDefault<TrainingModel>(query, parameters);
                return training;

            }
        }


    }
}
