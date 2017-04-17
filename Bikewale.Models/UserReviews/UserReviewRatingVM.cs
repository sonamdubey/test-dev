
using Bikewale.Entities.GenericBikes;
namespace Bikewale.Models
{
    public class UserReviewRatingVM : ModelBase
    {
        public GenericBikeInfo BikeInfo { get; set; }
        public string OverAllRatingText { get; set; }
        public string RatingQuestion { get; set; }
        public string ErrorMessage { get; set; }
    }
}
