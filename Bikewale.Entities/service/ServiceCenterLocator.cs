
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Bikewale.Entities.service
{
    [Serializable, DataContract]
    public class ServiceCenterLocator
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
        public uint ServiceCenterCountState { get; set; }
        [DataMember]
        public uint totalDealerIndia { get; set; }
        [DataMember]
        public uint totalCities { get; set; }
        [DataMember]
        public IEnumerable<ServiceCityEntity> Cities { get; set; }
    }
}
