using System.Collections.Generic;

namespace Bikewale.Entities.UserReviews
{
    public class UserReviewsData
    {
        public IEnumerable<UserReviewQuestion> Questions { get; set; }
        public IEnumerable<UserReviewRating> Ratings { get; set; }
        public IEnumerable<UserReviewOverallRating> OverallRating { get; set; }
        public IEnumerable<UserReviewPriceRange> PriceRange { get; set; }
    }
}
