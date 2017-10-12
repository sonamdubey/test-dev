namespace Bikewale.Models.UserReviews
{
    public class UserReviewLandingVM : ModelBase
    {
        public TopRatedBikesWidgetVM TopRatedBikesWidget { get; set; }
        public PopularBikesWithExpertReviewsWidgetVM BikesWithExpertReviews { get; set; }

    }
}
