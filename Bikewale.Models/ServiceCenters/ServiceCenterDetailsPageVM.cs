using Bikewale.Entities.BikeData;
using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Entities.UsedBikes;
using System.Collections.Generic;

namespace Bikewale.Models.ServiceCenters
{
    public class ServiceCenterDetailsPageVM : ModelBase
    {
        public UsedBikeModels UsedBikesByMakeList { get; set; }
        public DealersEntity DealersWidgetData { get; set; }
        public MostPopularBikeWidgetVM PopularWidgetData { get; set; }
        public ServiceCenterCompleteData ServiceCenterData { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public CityEntityBase City { get; set; }
        public ServiceCenterDetailsWidgetVM OtherServiceCentersWidgetData { get; set; }
        public IEnumerable<ModelServiceSchedule> BikeScheduleList { get; set; }
    }
}
