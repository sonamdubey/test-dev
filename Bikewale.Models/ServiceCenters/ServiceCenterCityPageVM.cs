
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Models.PopUp;

namespace Bikewale.Models.ServiceCenters
{
    /// <summary>
    /// Modified by: Snehal Dange on 29th Sep 2017
    /// Description: Added DealersServiceCentersIndiaWidgetVM   
    /// Modified by: Snehal Dange on 14th Nov 2017
    /// Descritpion: Added SimilarBrandsByCity to get similar brands in city
    /// </summary>
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
        public DealersServiceCentersIndiaWidgetVM DealersServiceCenterPopularCities { get; set; }
        public BikeCityPopup BikeCityPopup { get; set; }
        public OtherMakesVM SimilarBrandsByCity { get; set; }
        public bool IsShowroomPresentInCity { get; set; }
    }
}
