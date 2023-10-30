namespace VipFitness_ADMIN.Models
{
    public class UserTrainings
    {
        public int id { get; set; }
        public int userID { get; set; }
        public int trainingID { get; set; }
        public string setInfo {  get; set; }
        public int trainingType { get; set; }
        public bool isActive { get; set; }
        public string username { get; set; }
        public string trainingName {  get; set; }
    }
}
