using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    /// Created By: Prasad Gawde
    /// </summary>
    [DataContract, Serializable]
    public class PwaVideosReducer
    {
        [DataMember]
        public PwaAllVideos VideosLanding { get; private set; }
        [DataMember]
        public PwaVideosBySubcategory VideosByCategory { get; private set; }
        [DataMember]
        public PwaVideoDetailReducer VideoDetail { get; private set; }

        public PwaVideosReducer()
        {
            VideosLanding = new PwaAllVideos();
            VideosByCategory = new PwaVideosBySubcategory();
            VideoDetail = new PwaVideoDetailReducer();
        }
    }

}
