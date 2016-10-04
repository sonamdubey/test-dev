using System.Collections.Generic;

namespace Bikewale.Entities.DealerLocator
{
    public class DealerLocatorList
    {
        public IEnumerable<StateCityEntity> stateCityList { get; set; }
        public uint totalDealers { get; set; }
        public uint totalCities { get; set; }
    }
}
