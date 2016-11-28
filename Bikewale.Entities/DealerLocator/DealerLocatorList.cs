using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.DealerLocator
{
    [Serializable, DataContract]
    public class DealerLocatorList
    {
        [DataMember]
        public IEnumerable<StateCityEntity> stateCityList { get; set; }
        [DataMember]
        public uint totalDealers { get; set; }
        [DataMember]
        public uint totalCities { get; set; }
    }
}
