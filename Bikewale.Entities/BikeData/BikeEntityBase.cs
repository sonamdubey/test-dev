
using System;
using System.Runtime.Serialization;
namespace Bikewale.Entities.BikeData
{
    [Serializable, DataContract]
    public class BasicBikeEntityBase
    {
        [DataMember]
        public BikeMakeEntityBase Make { get; set; }
        [DataMember]
        public BikeModelEntityBase Model { get; set; }
        [DataMember]
        public string OriginalImagePath { get; set; }
        [DataMember]
        public string HostUrl { get; set; }
        [DataMember]
        public bool IsDiscontinued { get; set; }
        [DataMember]
        public bool IsUpcoming { get; set; }
    }
}
