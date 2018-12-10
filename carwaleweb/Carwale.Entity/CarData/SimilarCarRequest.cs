namespace Carwale.Entity.CarData
{
    public class SimilarCarRequest
    {
        public int CarId { get; set; }
        public int Count { get; set; }
        public bool IsBoost { get; set; }
        public bool IsVersion { get; set; }
        public string UserIdentifier { get; set; }
    }
}
