using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    ///  Created By: Prasad Gawde
    /// </summary>
    [Serializable, DataContract]
    public class PwaVideoDetailReducer
    {
        [DataMember]
        public PwaBikeVideoDetails VideoInfo { get; private set; }
        [DataMember]
        public PwaBikeInfo ModelInfo { get; set; }
        [DataMember]
        public PwaVideoDetailVideoBikeList RelatedInfo { get; private set; }

        public PwaVideoDetailReducer()
        {
            VideoInfo = new PwaBikeVideoDetails();
            RelatedInfo = new PwaVideoDetailVideoBikeList();
        }
    }

    /// <summary>
    ///  Created By: Prasad Gawde
    /// </summary>
    [Serializable, DataContract]
    public class PwaVideoDetailVideoBikeList
    {
        [DataMember]
        [JsonProperty(PropertyName = "0")]
        public PwaBikeVideos VideoList { get;  set; }
        [DataMember]
        [JsonProperty(PropertyName = "1")]
        public PwaBikeNews BikeList { get;  set; }
    }

}
