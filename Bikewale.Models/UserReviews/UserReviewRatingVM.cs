
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.UserReviews;
using System.Collections.Generic;
namespace Bikewale.Models
{
    public class UserReviewRatingVM : ModelBase
    {
        public GenericBikeInfo BikeInfo { get; set; }
        public string OverAllRatingText { get; set; }
        public IEnumerable<UserReviewQuestion> RatingQuestion { get; set; }
    }
}
