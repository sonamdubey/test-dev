using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.DealerLocator
{
    [Serializable, DataContract]
    public class DealerLocatorList
    {
        [DataMember]
        public IEnumerable<StateCityEntity> StateCityList { get; set; }
        [DataMember]
        public uint TotalDealers { get; set; }
        [DataMember]
        public uint TotalCities { get; set; }
    }
}
