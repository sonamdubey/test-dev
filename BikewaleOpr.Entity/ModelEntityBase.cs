using System;
using System.Runtime.Serialization;

namespace BikeWaleOpr.Entities
{
    [Serializable, DataContract]
    public class ModelEntityBase
    {
        [DataMember]
        public UInt32 ModelId { get; set; }
        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
        public string MaskingName { get; set; }
    }
}
