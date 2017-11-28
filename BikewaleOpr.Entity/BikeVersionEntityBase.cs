using System;
using System.Runtime.Serialization;

namespace BikewaleOpr.Entities
{
    [Serializable, DataContract]
    public class BikeVersionEntityBase
    {
        [DataMember]
        public int VersionId { get; set; }
        [DataMember]
        public string VersionName { get; set; }

    }
}
