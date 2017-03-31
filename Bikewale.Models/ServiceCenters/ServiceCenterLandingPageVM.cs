using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Models;
using Bikewale.Models.BikeCare;
using System.Collections.Generic;

namespace Bikewale.ServiceCenters
{
    public class ServiceCenterLandingPageVM : ModelBase
    {
        public IEnumerable<BikeMakeEntityBase> MakesList { get; set; }
        public BrandWidgetVM Brands { get; set; }
        public RecentBikeCareVM BikeCareWidgetVM { get; set; }
        public NewLaunchedWidgetVM NewLaunchWidgetData { get; set; }
        public UpcomingBikesWidgetVM UpcomingWidgetData { get; set; }
        public MostPopularBikeWidgetVM PopularWidgetData { get; set; }
        public UsedBikeModelsWidgetVM UsedBikesModelWidgetData { get; set; }
        public UsedBikeCitiesWidgetVM UsedBikesCityWidgetData { get; set; }
        public CityEntityBase City { get; set; }
    }
}
