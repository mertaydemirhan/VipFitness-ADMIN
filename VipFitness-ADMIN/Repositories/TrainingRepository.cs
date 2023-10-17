using Dapper;
using VipFitness_ADMIN.Models;
using Microsoft.Data.SqlClient;
using VipFitness_ADMIN.Controllers;

namespace VipFitness_ADMIN.Repositories
{
    public class TrainingRepository : ITrainingRepository
    {
        private string connstring;
        public TrainingRepository(IConfiguration configuration)
        {
            connstring = configuration.GetConnectionString("DefaultConnection");
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
            bool ExistUser = false;

            // User exist OR NOT  

            var GetList = GetAllTrainings();
            foreach (var item in GetList)
            {
                if (item.TrainingName == Training.TrainingName)
                {
                    ExistUser = true; break;
                }
            }
            if (!ExistUser)
            {
                using (var connection = new SqlConnection(connstring))
                {

                    connection.Open();
                    var query = "INSERT INTO tblTrainings(TrainingName,TrainingCategory,Explanation,ImgData,isdeleted) VALUES(@TrainingName,@TraningCategory,@Explanation,@ImgData,@IsDeleted)";
                    var parameters = new { TrainingName = Training.TrainingName, TraningCategory = Training.TrainingCategory, Explanation = Training.Explanation, ImgData = Training.ImgData, IsDeleted = Training.Isdeleted };
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
                 
                        var query = "Update tblTrainings SET TrainingName=@TrainingName,TrainingCategory=@TraningCategory,Explanation=@Explanation,ImgData=@ImgData,isdeleted=@IsDeleted WHERE id=@ID";
                        var parameters = new { TrainingName = Training.TrainingName, TraningCategory = Training.TrainingCategory, Explanation = Training.Explanation, ImgData = Training.ImgData, IsDeleted = Training.Isdeleted, ID= Training.Id };
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
                var Training = connection.QuerySingleOrDefault<TrainingModel>(query, parameters);
                return Training;
            }
        }


    }
}
