﻿using System.Linq;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 30 Mar 2017
    /// Description :   ScootersIndexPage View Model
    /// Modified by : Aditi Srivastava on 2 June 2017
    /// Summary     : Replaced ComparisonMinWidgetVM with new PopularComparisonsVM (carousel)
    /// </summary>
    public class ScootersIndexPageVM : ModelBase
    {
        public BrandWidgetVM Brands { get; set; }
        public MostPopularBikeWidgetVM PopularBikes { get; set; }
        public NewLaunchedWidgetVM NewLaunches { get; set; }
        public UpcomingBikesWidgetVM Upcoming { get; set; }
        public PopularComparisonsVM ComparePopularScooters { get; set; }
        public RecentNewsVM News { get; set; }
        public RecentExpertReviewsVM ExpertReviews { get; set; }
        public RecentVideosVM Videos { get; set; }
        public bool HasPopularBikes { get { return (PopularBikes != null && PopularBikes.Bikes != null && PopularBikes.Bikes.Count() > 0); } }
        public bool HasNewLaunches { get { return (NewLaunches != null && NewLaunches.Bikes != null && NewLaunches.Bikes.Count() > 0); } }
        public bool HasUpcoming { get { return (Upcoming != null && Upcoming.UpcomingBikes != null && Upcoming.UpcomingBikes.Count() > 0); } }
        public bool HasComparison { get; set; }
        public uint TabCount = 0;
        public bool IsNewsActive { get; set; }
        public bool IsExpertReviewActive { get; set; }
        public bool IsVideoActive { get; set; }
    }
}
