using System;
using System.Runtime.Serialization;

namespace BikeWaleOpr.Entities
{
    [Serializable, DataContract]
    public class MakeEntityBase
    {
        [DataMember]
        public UInt32 Id { get; set; }
        [DataMember]
        public string MakeName { get; set; }
        [DataMember]
        public string MaskingName { get; set; }
    }
}
