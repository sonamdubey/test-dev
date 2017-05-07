using Bikewale.Entities.UserReviews;
using Bikewale.Entities.UserReviews.Search;

namespace Bikewale.Models.UserReviews
{
    public class UserReviewsSearchVM
    {
        public BikeReviewsInfo ReviewsInfo { get; set; }
        public uint ModelId { get; set; }
        public string BikeName { get; set; }
        public SearchResult UserReviews { get; set; }
    }
}
