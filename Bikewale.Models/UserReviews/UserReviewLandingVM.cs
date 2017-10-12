using Bikewale.Entities.BikeData;
using Bikewale.Entities.Authors;
using Bikewale.Entities.UserReviews;
using System.Collections.Generic;

namespace Bikewale.Models.UserReviews
{
    public class UserReviewLandingVM : ModelBase
    {
        public TopRatedBikesWidgetVM TopRatedBikesWidget { get; set; }
        public PopularBikesWithExpertReviewsWidgetVM BikesWithExpertReviews { get; set; }
        public RecentExpertReviewsVM ExpertReviews { get; set; }
        public IEnumerable<RecentReviewsWidget>  RecentUserReviewsList { get; set; }
        public IEnumerable<AuthorEntityBase> Authors { get; set; }
        public IEnumerable<BikeMakeEntityBase> Makes { get; set; }

    }
}
