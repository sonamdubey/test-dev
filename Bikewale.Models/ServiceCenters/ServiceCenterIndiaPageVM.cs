using Bikewale.Entities.BikeData;
using Bikewale.Entities.service;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Entities.UsedBikes;
using Bikewale.Models.BikeCare;
using System.Collections.Generic;
namespace Bikewale.Models.ServiceCenters
{
    public class ServiceCenterIndiaPageVM : ModelBase
    {
        public IEnumerable<BrandServiceCenters> ServiceCenterBrandsList { get; set; }
        public UsedBikeModels UsedBikesByMakeList { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public ServiceCenterLocatorList ServiceCentersCityList { get; set; }
        public RecentBikeCareVM BikeCareWidgetVM { get; set; }
    }
}
