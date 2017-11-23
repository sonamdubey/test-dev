using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    ///  Created By: Prasad Gawde
    /// </summary>
    [DataContract, Serializable]
    public class PwaBikeVideoDetails
    {
        [DataMember]
        public PwaBikeVideoEntity VideoInfo { get; set; }
        [DataMember]
        public List<PwaBikeVideoRelatedInfo> RelatedInfoApi { get; set; }

    }
    /// <summary>
    ///  Created By: Prasad Gawde
    /// </summary>
    [DataContract, Serializable]
    public class PwaBikeVideoRelatedInfo
    {
        [DataMember]
        public PwaRelatedInfoType Type { get; set; }
        [DataMember]
        public string Url { get; set; }

        public PwaBikeVideoRelatedInfo(PwaRelatedInfoType type,string url)
        {
            Type = type;
            Url = url;
        }
    }
    /// <summary>
    ///  Created By: Prasad Gawde
    /// </summary>
    [DataContract, Serializable]
    public enum PwaRelatedInfoType
    {
        Bike = 0,
        Video = 1,
        News = 2
    }
}
