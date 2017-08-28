using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    [DataContract, Serializable]
    public class PwaAllVideosWrapper
    {
        [DataMember]
        public PwaAllVideos PwaVideos { get; set; }        
    }

}
