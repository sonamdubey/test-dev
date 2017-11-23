using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    ///  Created By: Prasad Gawde
    /// </summary>
    [DataContract, Serializable]
    public class PwaAllVideos
    {
        [DataMember]
        public PwaVideosLandingPageTopVideos TopVideos { get; set; }
        [DataMember]
        public PwaVideosLandingPageOtherVideos OtherVideos { get; set; }
    }

}
