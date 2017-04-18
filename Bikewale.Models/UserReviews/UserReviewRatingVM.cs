
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
    }
}
