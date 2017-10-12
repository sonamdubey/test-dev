using Bikewale.Entities.Authors;
using System.Collections.Generic;

namespace Bikewale.Models.UserReviews
{
    public class UserReviewLandingVM : ModelBase
    {
        public TopRatedBikesWidgetVM TopRatedBikesWidget { get; set; }
        public PopularBikesWithExpertReviewsWidgetVM BikesWithExpertReviews { get; set; }
        public RecentExpertReviewsVM ExpertReviews { get; set; }
        public IEnumerable<AuthorEntityBase> Authors { get; set; }

    }
}
