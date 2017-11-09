using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    ///  Created By: Prasad Gawde
    /// </summary>
    [DataContract, Serializable]
    public class PwaAllVideosLandingWrapper
    {
        [DataMember]
        public PwaAllVideos VideosLanding { get; set; }        
    }

    /// <summary>
    ///  Created By: Prasad Gawde
    /// </summary>
    [DataContract, Serializable]
    public class PwaAllVideosWrapper
    {
        [DataMember]
        public PwaAllVideosLandingWrapper Videos { get; set; }
    }
}
