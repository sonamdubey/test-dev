using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.ServiceCenters;
using System.Collections.Generic;

namespace Bikewale.Models.ServiceCenters
{
    public class ServiceCenterDetailsPageVM : ModelBase
    {
        public UsedBikeModelsWidgetVM UsedBikesByMakeList { get; set; }
        public DealerCardVM DealersWidgetData { get; set; }
        public MostPopularBikeWidgetVM PopularWidgetData { get; set; }
        public ServiceCenterCompleteData ServiceCenterData { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public CityEntityBase City { get; set; }
        public ServiceCenterDetailsWidgetVM OtherServiceCentersWidgetData { get; set; }
        public IEnumerable<ModelServiceSchedule> BikeScheduleList { get; set; }
    }
}
