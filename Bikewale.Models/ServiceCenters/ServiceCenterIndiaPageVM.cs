using Bikewale.Entities.BikeData;
using Bikewale.Entities.service;
using Bikewale.Entities.ServiceCenters;
using Bikewale.Models.BikeCare;
using System.Collections.Generic;
namespace Bikewale.Models.ServiceCenters
{
    /// <summary>
    /// Created by Sajal Gupta on 28-03-2017
    /// This is view model for service centers in india page
    /// </summary>
    public class ServiceCenterIndiaPageVM : ModelBase
    {
        public IEnumerable<BrandServiceCenters> ServiceCenterBrandsList { get; set; }
        public UsedBikeModelsWidgetVM UsedBikesByMakeList { get; set; }
        public BikeMakeEntityBase Make { get; set; }
        public ServiceCenterLocatorList ServiceCentersCityList { get; set; }
        public RecentBikeCareVM BikeCareWidgetVM { get; set; }
    }
}
