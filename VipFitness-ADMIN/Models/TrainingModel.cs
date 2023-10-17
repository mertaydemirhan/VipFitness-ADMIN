namespace VipFitness_ADMIN.Models
{
    public class TrainingModel
    {
        public int Id { get; set; }
        public string TrainingName { get; set; }
        public int TrainingCategory { get; set; }
        public string Explanation { get; set; }
        public byte[] ImgData { get; set; }
        public bool Isdeleted { get; set; }
    }
}
