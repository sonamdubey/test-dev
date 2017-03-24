using Bikewale.Entities.BikeData;
using Bikewale.Entities.ServiceCenters;
using System.Collections.Generic;

namespace Bikewale.Models.ServiceCenters
{
    public class ServiceCentersNearByCityWidgetVM
    {
        public IEnumerable<CityBrandServiceCenters> ServiceCentersNearbyCities { get; set; }
        public BikeMakeEntityBase Make { get; set; }
    }
}
