
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Entities.UsedBikes;
namespace Bikewale.Models.ServiceCenters
{
    public class ServiceCenterCityPageVM : ModelBase
    {
        public ServiceCenterData ServiceCentersListObject { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public CityEntityBase City { get; set; }
        public ServiceCentersNearByCityWidgetVM NearByCityWidgetData { get; set; }
        public MostPopularBikeWidgetVM PopularWidgetData { get; set; }
        public UsedBikeModels UsedBikesByMakeList { get; set; }
        public DealersEntity DealersWidgetData { get; set; }
        public BrandCityPopupVM BrandCityPopupWidget { get; set; }
    }
}
