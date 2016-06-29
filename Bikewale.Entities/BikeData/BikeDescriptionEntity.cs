using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
{
    [Serializable,DataContract]
    public class BikeDescriptionEntity
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string SmallDescription { get; set; }
        [DataMember]
        public string FullDescription { get; set; }
    }
}
