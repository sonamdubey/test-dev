using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.BikeData
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
