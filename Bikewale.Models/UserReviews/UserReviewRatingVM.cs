
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
namespace Bikewale.Models
{
    public class UserReviewRatingVM : ModelBase
    {
        public BikeModelEntity objModelEntity { get; set; }
        public string OverAllRatingText { get; set; }
        public string RatingQuestion { get; set; }
        public string ErrorMessage { get; set; }
    }
}
