
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.UsedBikes;
namespace Bikewale.Models
{
    public class DealerShowroomCityPageVM : ModelBase
    {
        public NearByCityDealer DealerCountCity;
        public DealersEntity DealersList;
        public BikeMakeEntityBase Make;
        public UsedBikeModels UsedBikeModel;
        public CityEntityBase CityDetails;
        public uint TotalDealers;
        public BrandCityPopupVM BrandCityPopupWidget { get; set; }
    }
}
