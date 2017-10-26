using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    [DataContract, Serializable]
    public class PwaBikeVideoDetails
    {
        [DataMember]
        public PwaBikeVideoEntity VideoInfo { get; set; }
        [DataMember]
        public List<PwaBikeVideoRelatedInfo> RelatedInfoApi { get; set; }

    }
    [DataContract, Serializable]
    public class PwaBikeVideoRelatedInfo
    {
        [DataMember]
        public PwaRelatedInfoType Type { get; set; }
        [DataMember]
        public string Url { get; set; }
    }

    [DataContract, Serializable]
    public enum PwaRelatedInfoType
    {
        Bike=0,
        Video=1,
        News=2
    }
}
