using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    [DataContract, Serializable]
    public class PwaAllVideosLandingWrapper
    {
        [DataMember]
        public PwaAllVideos VideosLanding { get; set; }        
    }



    [DataContract, Serializable]
    public class PwaAllVideosWrapper
    {
        [DataMember]
        public PwaAllVideosLandingWrapper Videos { get; set; }
    }
}
