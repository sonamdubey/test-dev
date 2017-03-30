using System.Linq;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 30 Mar 2017
    /// Description :   ScootersIndexPage View Model
    /// </summary>
    public class ScootersIndexPageVM : ModelBase
    {
        public BrandWidgetVM Brands { get; set; }
        public MostPopularBikeWidgetVM PopularBikes { get; set; }
        public NewLaunchedWidgetVM NewLaunches { get; set; }
        public UpcomingBikesWidgetVM Upcoming { get; set; }
        public ComparisonMinWidgetVM Comparison { get; set; }

        public bool HasPopularBikes { get { return (PopularBikes != null && PopularBikes.Bikes != null && PopularBikes.Bikes.Count() > 0); } }
        public bool HasNewLaunches { get { return (NewLaunches != null && NewLaunches.Bikes != null && NewLaunches.Bikes.Count() > 0); } }
        public bool HasUpcoming { get { return (Upcoming != null && Upcoming.UpcomingBikes != null && Upcoming.UpcomingBikes.Count() > 0); } }
        public bool HasComparison { get { return (Comparison != null && Comparison.TopComparisonRecord != null && Comparison.RemainingCompareList != null && Comparison.RemainingCompareList.Count() > 0); } }
    }
}
