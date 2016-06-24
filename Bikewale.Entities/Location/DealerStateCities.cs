using System;
using System.Collections.Generic;

namespace Bikewale.Entities.Location
{
    /// <summary>
    /// Created By Vivek Gupta on 24 june 2016
    /// Desc : dealer state cities
    /// </summary>
    [Serializable]
    public class DealerStateCities
    {
        public IEnumerable<DealerCityEntity> dealerCities { get; set; }
        public DealerStateEntity dealerStates { get; set; }
    }
}
