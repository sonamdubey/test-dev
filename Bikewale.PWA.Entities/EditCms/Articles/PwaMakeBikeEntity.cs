using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    /// Modified By : Pratibha Verma on 26 Feb, 2018
    /// </summary>
    [Serializable, DataContract]
    public class PwaMakeBikeEntity
    {
        [DataMember]
        public int MakeId { get; set; }

        [DataMember]
        public string MakeName { get; set; }

        [DataMember]
        public string MaskingName { get; set; }

        [DataMember]
        public uint TotalCount { get; set; }

    }
}
