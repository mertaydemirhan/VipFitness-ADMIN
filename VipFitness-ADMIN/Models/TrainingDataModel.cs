namespace VipFitness_ADMIN.Models
{
    public class TrainingDataModel
    {

            public int Id { get; set; }
            public int UserId { get; set; } 
            public int TrainingId { get; set; } 
            public string SetInfo { get; set; } 
            public int TrainingType { get; set; } 
            public bool IsActive { get; set; }
            public string Username { get; set; }
            public string TrainingName { get; set; } 

            // public DateTime CreatedAt { get; set; }

    }
}
