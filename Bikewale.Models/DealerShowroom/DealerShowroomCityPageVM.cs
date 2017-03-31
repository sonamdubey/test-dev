
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Dealer;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Models.ServiceCenters;
namespace Bikewale.Models
{
    /// <summary>
    /// Created By Subodh Jain 30 March 2017
    /// Summary :- Dealer in city Page View Model
    /// </summary>
    public class DealerShowroomCityPageVM : ModelBase
    {

        public NearByCityDealer DealerCountCity { get; set; }
        public DealersEntity DealersList { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public UsedBikeModelsWidgetVM UsedBikeModel { get; set; }
        public CityEntityBase CityDetails { get; set; }
        public uint TotalDealers { get; set; }
        public LeadCaptureEntity LeadCapture { get; set; }
        public BrandCityPopupVM BrandCityPopupWidget { get; set; }
        public MostPopularBikeWidgetVM PopularBikes { get; set; }
        public ServiceCenterDetailsWidgetVM ServiceCenterDetails { get; set; }
        public BrandCityPopupVM BrandCityPopUp { get; set; }
    }
}
