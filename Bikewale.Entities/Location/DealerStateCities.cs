using System.Collections.Generic;

namespace Bikewale.Entities.Location
{
    public class DealerStateCities
    {
        public IEnumerable<DealerCityEntity> dealerCities { get; set; }
        public DealerStateEntity dealerStates { get; set; }
    }
}
