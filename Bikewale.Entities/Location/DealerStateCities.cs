using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Location
{
    /// <summary>
    /// Created By Vivek Gupta on 24 june 2016
    /// Desc : dealer state cities
    /// </summary>
    [Serializable, DataContract]
    public class DealerStateCities
    {
        [DataMember]
        public IEnumerable<DealerCityEntity> dealerCities { get; set; }
        [DataMember]
        public DealerStateEntity dealerStates { get; set; }
    }
}
