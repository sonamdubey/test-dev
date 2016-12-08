
using Bikewale.Entities.BikeData;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
namespace Bikewale.Entities.ServiceCenters
{
    [Serializable, DataContract]
    public class ServiceScheduleBase
    {
        [DataMember]
        public uint ServiceNo { get; set; }
        [DataMember]
        public string Kms { get; set; }
        [DataMember]
        public string Days { get; set; }
    }

    [Serializable, DataContract]
    public class ModelServiceSchedule : BikeModelEntityBase
    {
        [DataMember]
        public string HostUrl { get; set; }
        [DataMember]
        public string OriginalImagePath { get; set; }
        [DataMember]
        public ICollection<ServiceScheduleBase> Schedules { get; set; }
    }
}
