using System.Collections.Generic;

namespace Bikewale.Models.UserReviews
{
    public class UserReviewSimilarBikesWidgetVM
    {
        public IEnumerable<Bikewale.Entities.SimilarBikeUserReview> SimilarBikes { get; set; }
        public string GlobalCityName { get; set; }
    }
}
