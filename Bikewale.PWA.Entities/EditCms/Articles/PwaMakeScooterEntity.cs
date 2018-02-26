using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    [Serializable, DataContract]
    public class PwaMakeScooterEntity
    {
        [DataMember]
        public int MakeId { get; set; }

        [DataMember]
        public string MakeName { get; set; }

        [DataMember]
        public string MaskingName { get; set; }

        [DataMember]
        public bool IsScooterOnly { get; set; }

        [DataMember]
        public uint TotalCount { get; set; }

    }
}
