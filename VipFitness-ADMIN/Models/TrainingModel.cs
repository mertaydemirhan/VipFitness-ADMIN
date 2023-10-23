namespace VipFitness_ADMIN.Models
{
    public class TrainingModel
    {
        public int Id { get; set; }
        public string TrainingName { get; set; }
        public int TrainingCategoryId { get; set; }
        public string Explanation { get; set; }
        public byte[] ImgData { get; set; }
        public bool Isdeleted { get; set; }
        public string? ImgExt { get; set; }
        public string ImgDataString { get; set; }
        public IEnumerable<TrainingCategory> TrainingCat { get; set; } // TrainingCategory sınıfını temsil eden nesne.

    }
}
