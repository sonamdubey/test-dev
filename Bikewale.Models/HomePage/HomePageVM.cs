
using Bikewale.Entities.BikeData.NewLaunched;
using System.Collections.Generic;
namespace Bikewale.Models
{
    /// <summary>
    /// Created By : Sangram Nandkhile on 23 Mar 2017
    /// Summary : View Model for Homepage
    /// </summary>
    public class HomePageVM : ModelBase
    {
        public string LocationMasking { get; set; }
        public string Location { get; set; }
        public BrandWidgetVM Brands { get; set; }
        public NewLaunchedWidgetVM NewLaunchedBikes { get; set; }
        public MostPopularBikeWidgetVM PopularBikes { get; set; }
        public BestBikeWidgetVM BestBikes { get; set; }
        public UsedBikeCitiesWidgetVM UsedBikeCities { get; set; }
    }

}
