
using Bikewale.Entities.DealerLocator;
using Bikewale.Models.ServiceCenters;
namespace Bikewale.Models
{
    public class PriceInCityPageVM
    {
        public PriceInCity.PriceInTopCitiesWidgetVM TopCities { get; private set; }
        public DealersEntity Dealers { get; private set; }
        public SimilarBikesWidgetVM AlternateBikes { get; private set; }
        public ServiceCentersNearByCityWidgetVM ServiceCenters { get; private set; }
        public BikeInfoVM BikeInfo { get; private set; }
    }
}
