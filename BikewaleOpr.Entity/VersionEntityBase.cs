using System;
using System.Runtime.Serialization;

namespace BikeWaleOpr.Entities
{
    [Serializable,DataContract]
    public class VersionEntityBase
    {
        [DataMember]
        public UInt32 VersionId { get; set; }
        [DataMember]
        public string VersionName { get; set; }
    }
}
