using System;
using System.Runtime.Serialization;

namespace BikewaleOpr.Entities
{
    [Serializable,DataContract]
    public class BikeModelEntityBase
    {
        [DataMember]
        public int ModelId { get; set; }
        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
        public string MaskingName { get; set; }
    }
}
