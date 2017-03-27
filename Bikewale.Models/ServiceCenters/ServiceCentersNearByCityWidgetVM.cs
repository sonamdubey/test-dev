using Bikewale.Entities.BikeData;
using Bikewale.Entities.ServiceCenters;
using System.Collections.Generic;

namespace Bikewale.Models.ServiceCenters
{
    /// <summary>
    /// Created by Sajal Gupta on 24-03-2017
    /// This class is wrapper for ServiceCentersNearByCity widget entity
    /// </summary>
    public class ServiceCentersNearByCityWidgetVM
    {
        public IEnumerable<CityBrandServiceCenters> ServiceCentersNearbyCities { get; set; }
        public BikeMakeEntityBase Make { get; set; }
    }
}
