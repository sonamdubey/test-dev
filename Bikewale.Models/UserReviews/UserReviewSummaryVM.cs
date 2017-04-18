using Bikewale.Entities.UserReviews;

namespace Bikewale.Models
{
    public class UserReviewSummaryVM : ModelBase
    {
        public UserReviewSummary Summary { get; set; }
        public string WriteReviewLink { get; set; }
    }

}
