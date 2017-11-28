using Bikewale.Entities.UserReviews;
using System.Collections.Generic;
namespace Bikewale.Models
{
    /// <summary>
    /// Created By : Sangram Nandkhile on 23 Mar 2017
    /// Summary : View Model for Newpage
    /// Modified by : Aditi Srivastava on 27 Apr 2017
    /// Summary  : Added new viewModel for similar comparisons
    /// Modified by : Sajal Gupta on 02-08-2017
    /// Description : Added RecentUserReviewsList
    /// </summary>
    public class NewPageVM : ModelBase
    {
        public string LocationMasking { get; set; }
        public string Location { get; set; }
        public BrandWidgetVM Brands { get; set; }
        public NewLaunchedWidgetVM NewLaunchedBikes { get; set; }
        public MostPopularBikeWidgetVM PopularBikes { get; set; }
        public UpcomingBikesWidgetVM UpcomingBikes { get; set; }
        public BestBikeByCategoryVM BestBikes { get; set; }
        public RecentNewsVM News { get; set; }
        public RecentExpertReviewsVM ExpertReviews { get; set; }
        public RecentVideosVM Videos { get; set; }
        public PopularComparisonsVM ComparePopularBikes { get; set; }

        public bool IsPopularBikesDataAvailable { get; set; }
        public bool IsNewLaunchedDataAvailable { get; set; }
        public bool IsComparePopularBikesAvailable { get; set; }
        public bool IsUpcomingBikeAvailable { get; set; }

        public uint TabCount = 0;
        public bool IsNewsActive { get; set; }
        public bool IsExpertReviewActive { get; set; }
        public bool IsVideoActive = false;
        public IEnumerable<RecentReviewsWidget> RecentUserReviewsList { get; set; }

        public string Source { get; set; }
    }
}
