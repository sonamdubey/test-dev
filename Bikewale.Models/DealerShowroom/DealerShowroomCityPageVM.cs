
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.Location;
using Bikewale.Models.ServiceCenters;
namespace Bikewale.Models
{
    public class DealerShowroomCityPageVM : ModelBase
    {
        public NearByCityDealer DealerCountCity;
        public DealerCardVM DealersList;
        public BikeMakeEntityBase Make;
        public UsedBikeModelsWidgetVM UsedBikeModel;
        public CityEntityBase CityDetails;
        public uint TotalDealers;
        public BrandCityPopupVM BrandCityPopupWidget { get; set; }
        public MostPopularBikeWidgetVM PopularBikes;
        public ServiceCenterDetailsWidgetVM ServiceCenterDetails;
        public BrandCityPopupVM BrandCityPopUp;
    }
}
