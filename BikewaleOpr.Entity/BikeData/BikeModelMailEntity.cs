using System;
using System.Runtime.Serialization;

namespace BikewaleOpr.Entities.BikeData
{
    [Serializable, DataContract]
    public class BikeModelMailEntity : BikeModelEntityBase
    {
        [DataMember]
        public string OldUrl { get; set; }
        [DataMember]
        public string NewUrl { get; set; }
    }
}
