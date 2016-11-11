
using Bikewale.Entities.BikeData;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Bikewale.Entities.ServiceCenters
{
    [Serializable, DataContract]
    public class ServiceScheduleBase
    {
        [DataMember, JsonProperty("ServiceNo")]
        public uint ServiceNo { get; set; }
        [DataMember, JsonProperty("Kms")]
        public string Kms { get; set; }
        [DataMember, JsonProperty("Days")]
        public uint Days { get; set; }
    }

    [Serializable, DataContract]
    public class ModelServiceSchedule : BikeModelEntityBase
    {
        [DataMember, JsonProperty("Schedules")]
        public IList<ServiceScheduleBase> Schedules { get; set; }
    }
}
