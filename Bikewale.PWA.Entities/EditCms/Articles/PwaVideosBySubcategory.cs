using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    [DataContract, Serializable]
    public class PwaVideosBySubcategory
    {
        [DataMember]
        public IEnumerable<PwaBikeVideoEntity> Videos { get; set; }
        [DataMember]
        public string SectionTitle { get; set; }
        [DataMember]
        public string MoreVideosUrl { get; set; }
    }
}
