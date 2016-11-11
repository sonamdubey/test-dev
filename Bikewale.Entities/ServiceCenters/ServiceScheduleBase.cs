
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
        public uint Days { get; set; }
    }

    [Serializable, DataContract]
    public class ModelServiceSchedule : BikeModelEntityBase
    {
        [DataMember]
        public IList<ServiceScheduleBase> Schedules { get; set; }
    }
}
