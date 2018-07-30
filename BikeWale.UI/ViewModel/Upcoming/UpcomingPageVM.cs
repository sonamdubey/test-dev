
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Pager;
using System.Collections.Generic;
namespace Bikewale.Models
{
    /// <summary>
    /// Create by: Sangram Nandkhile on 12-Apr-2017
    /// Summary:  View Model for upcoming page
    /// Modified by :Snehal Dange on 30th Oct 2017
    /// Description : Added RecentNewsVM widget for recent news 
    /// </summary>
    public class UpcomingPageVM : ModelBase
    {
        public BikeMakeEntityBase Make { get; set; }
        public BrandWidgetVM Brands { get; set; }
        public NewLaunchedWidgetVM NewLaunches { get; set; }
        public MostPopularBikeWidgetVM PopularBikes { get; set; }
        public IEnumerable<UpcomingBikeEntity> UpcomingBikeModels { get; set; }
        public OtherMakesVM OtherMakes { get; set; }
        public bool HasBikes { get; set; }
        public uint TotalBikes { get; set; }
        public IEnumerable<int> YearsList { get; set; }
        public IEnumerable<BikeMakeEntityBase> MakesList { get; set; }
        public PagerEntity Pager { get; set; }
        public bool HasPages { get { return (Pager != null && Pager.TotalResults > 0); } }

        public RecentNewsVM News { get; set; }
    }
}
