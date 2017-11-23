using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    ///  Created By: Prasad Gawde
    /// </summary>
    [Serializable, DataContract]
    public class PwaBikeInfo
    {
        [DataMember]
        public string ModelName { get; set; }
        [DataMember]
        public int ModelId { get; set; }
        [DataMember]
        public string ModelDetailUrl { get; set; }
        [DataMember]
        public string ImageUrl { get; set; }
        [DataMember]
        public string PriceDescription { get; set; }
        [DataMember]
        public string Price { get; set; }
        [DataMember]
        public string Upcoming { get; set; }
        [DataMember]
        public string Discontinued { get; set; }
        [DataMember]
        public List<PwaBikeInfoExtraDetails> MoreDetailsList { get; set; }
        [DataMember]
        public PwaBikeInfoUsedBikeDetails UsedBikesLink { get; set; }
        [DataMember]
        public PwaBikeRating Rating { get; set; }
    }

    /// <summary>
    ///  Created By: Prasad Gawde
    /// </summary>
    [Serializable, DataContract]
    public class PwaBikeInfoExtraDetails
    {
        [DataMember]
        public string Type { get; set; }
        [DataMember]
        public string DetailUrl { get; set; }
        [DataMember]
        public string Title { get; set; }
    }
    
}
