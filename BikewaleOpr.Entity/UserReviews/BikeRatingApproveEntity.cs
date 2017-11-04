namespace BikewaleOpr.Entity.UserReviews
{
    public class BikeRatingApproveEntity
    {
        public string MakeMaskingName { get; set; }
        public string ModelMaskingName { get; set; }
        public string BikeName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public uint ReviewId { get; set; }
        public uint ModelId { get; set; }
    }
}
