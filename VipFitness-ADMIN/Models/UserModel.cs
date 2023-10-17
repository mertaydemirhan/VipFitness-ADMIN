namespace VipFitness_ADMIN.Models
{
    public class UserModel
    {
        public int id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int UType { get; set; }
        public string UTypeString { get; set; }
        public bool isdeleted { get; set; }
    }
}
