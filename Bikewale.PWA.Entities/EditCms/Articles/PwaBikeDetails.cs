using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    [Serializable, DataContract]
    public class PwaBikeDetails
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string DetailPageUrl { get; set; }
        [DataMember]
        public string Price { get; set; }
        [DataMember]
        public string PriceSuffix { get; set; }
        [DataMember]
        public string PriceDescription { get; set; }
        [DataMember]
        public string ImgUrl { get; set; }
    }
}
