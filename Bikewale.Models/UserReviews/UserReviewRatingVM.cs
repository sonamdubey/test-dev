using Bikewale.Entities.BikeData;
namespace Bikewale.Models
{
    public class UserReviewRatingVM : ModelBase
    {
        public BikeModelEntity objModelEntity { get; set; }
        public string OverAllRatingText { get; set; }
        public string RatingQuestion { get; set; }
        public string ErrorMessage { get; set; }
        public uint PriceRangeId { get; set; }
        public string ReviewsOverAllrating { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public uint ReviewId { get; set; }
    }
}
