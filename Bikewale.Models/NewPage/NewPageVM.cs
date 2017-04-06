﻿
namespace Bikewale.Models
{
    /// <summary>
    /// Created By : Sangram Nandkhile on 23 Mar 2017
    /// Summary : View Model for Newpage
    /// </summary>
    public class NewPageVM : ModelBase
    {
        public string LocationMasking { get; set; }
        public string Location { get; set; }
        public BrandWidgetVM Brands { get; set; }
        public NewLaunchedWidgetVM NewLaunchedBikes { get; set; }
        public MostPopularBikeWidgetVM PopularBikes { get; set; }
        public UpcomingBikesWidgetVM UpcomingBikes { get; set; }
        public BestBikeWidgetVM BestBikes { get; set; }
        public ComparisonMinWidgetVM CompareBikes { get; set; }
        public RecentNewsVM News { get; set; }
        public RecentExpertReviewsVM ExpertReviews { get; set; }
        public RecentVideosVM Videos { get; set; }

        public bool IsPopularBikesDataAvailable { get; set; }
        public bool IsNewLaunchedDataAvailable { get; set; }

        public bool IsUpcomingBikeAvailable { get; set; }

        public uint TabCount = 0;
        public bool IsNewsActive { get; set; }
        public bool IsExpertReviewActive { get; set; }
        public bool IsVideoActive = false;
    }
}
