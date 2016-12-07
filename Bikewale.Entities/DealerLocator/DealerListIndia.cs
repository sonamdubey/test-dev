using Bikewale.Entities.Location;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.DealerLocator
{
    [Serializable, DataContract]
    public class StateCityEntity
    {
        [DataMember]
        public string Link { get; set; }
        [DataMember]
        public uint Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Lat { get; set; }
        [DataMember]
        public string Long { get; set; }
        [DataMember]
        public string stateMaskingName { get; set; }
        [DataMember]
        public uint DealerCountState { get; set; }
        [DataMember]
        public uint totalDealerIndia { get; set; }
        [DataMember]
        public uint totalCities { get; set; }
        [DataMember]
        public IEnumerable<DealerCityEntity> Cities { get; set; }
    }
}
