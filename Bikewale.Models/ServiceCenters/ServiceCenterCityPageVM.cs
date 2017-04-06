
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.ServiceCenters;
namespace Bikewale.Models.ServiceCenters
{
    public class ServiceCenterCityPageVM : ModelBase
    {
        public ServiceCenterData ServiceCentersListObject { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public CityEntityBase City { get; set; }
        public ServiceCentersNearByCityWidgetVM NearByCityWidgetData { get; set; }
        public MostPopularBikeWidgetVM PopularWidgetData { get; set; }
        public UsedBikeModelsWidgetVM UsedBikesByMakeList { get; set; }
        public DealerCardVM DealersWidgetData { get; set; }
        public BrandCityPopupVM BrandCityPopupWidget { get; set; }
    }
}
