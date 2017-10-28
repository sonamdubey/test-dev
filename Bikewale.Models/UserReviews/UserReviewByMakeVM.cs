using Bikewale.Entities.UserReviews;
using System.Collections.Generic;

namespace Bikewale.Models.UserReviews
{
    public class UserReviewByMakeVM : ModelBase
    {
        public IEnumerable<PopularBikesWithUserReviews> PopularBikes { get; set; }
        public IEnumerable<PopularBikesWithUserReviews> OtherBikes { get; set; }
        public string Make { get; set; }
    }
}
