
using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Models.ServiceCenters;

namespace Bikewale.Models
{
    public class DealerShowroomDealerDetailsVM : ModelBase
    {
        public DealerCardVM DealersList;
        public BikeMakeEntityBase Make;
        public CityEntityBase CityDetails;
        public DealerBikesEntity DealerDetails;
        public MostPopularBikeWidgetVM PopularBikes;
        public ServiceCenterDetailsWidgetVM ServiceCenterDetails;
        public uint PQCityId;
        public uint PQAreaID;
        public string PQAreaName;
        public string CustomerAreaName;

    }
}
