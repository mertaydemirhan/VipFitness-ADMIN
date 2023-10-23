using System.Configuration;
using System.Data;
using Dapper;
using Microsoft.Data.SqlClient;
using VipFitness_ADMIN.Models;

namespace VipFitness_ADMIN.Repositories
{
    public class TrainingCatRepository : ITrainingCatRepository
    {
        private string connstring;

        public TrainingCatRepository(IConfiguration configuration)
        {
            connstring = configuration.GetConnectionString("DefaultConnection");
        }

        public IEnumerable<TrainingCatRepository> GetTrainingCategories()
        {
            using (var connection = new SqlConnection(connstring))
            {
                connection.Open();
                string query = "SELECT * FROM tblTrainingCategory";
                return connection.Query<TrainingCatRepository>(query);
            }
        }
    }
}
