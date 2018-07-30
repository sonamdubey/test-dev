using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    ///  Created By: Prasad Gawde
    /// </summary>
    [Serializable, DataContract]
    public class PwaBikeInfoUsedBikeDetails
    {
        [DataMember]
        public string DescriptionLabel { get; set; }
        [DataMember]
        public string PricePrefix { get; set; }
        [DataMember]
        public string Price { get; set; }
        [DataMember]
        public string UsedBikesLinkUrl { get; set; }
    }
}
