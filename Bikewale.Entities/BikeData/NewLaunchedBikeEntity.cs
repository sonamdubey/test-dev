using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    [Serializable, DataContract]
    public class NewLaunchedBikeEntity : BikeModelEntity
    {
        [DataMember]
        public uint BikeLaunchId { get; set; }
        [DataMember]
        public DateTime LaunchDate { get; set; }
        [DataMember]
        public ulong BasicId { get; set; }
        [DataMember]
        public MinSpecsEntity Specs { get; set; }
    }
}