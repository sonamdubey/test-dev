using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.Videos
{
    [Serializable, DataContract]
    public class BikeVideosListEntity
    {
        [DataMember]
        public int TotalRecords { get; set; }
        [DataMember]
        public IEnumerable<BikeVideoEntity> Videos { get; set; }
        [DataMember]
        public string NextPageUrl { get; set; }
        [DataMember]
        public string PrevPageUrl { get; set; }

    }
}
