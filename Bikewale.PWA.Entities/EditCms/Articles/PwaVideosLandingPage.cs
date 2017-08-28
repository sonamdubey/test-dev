using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    [DataContract, Serializable]
    public class PwaVideosLandingPageTopVideos
    {
       
        [DataMember]
        public IEnumerable<PwaBikeVideoEntity> LandingFirstVideos { get; set; }
        [DataMember]     
        public PwaVideosBySubcategory ExpertReviews { get; set; }        
    }

    [DataContract, Serializable]
    public class PwaVideosLandingPageOtherVideos
    {
        [DataMember]
        public PwaBrandsInfo Brands { get; set; }     
        [DataMember]
        public PwaVideosBySubcategory FirstRide { get; set; }
        [DataMember]
        public PwaVideosBySubcategory LaunchAlert { get; set; }
        [DataMember]
        public PwaVideosBySubcategory FirstLook { get; set; }
        [DataMember]
        public PwaVideosBySubcategory PowerDriftBlockbuster { get; set; }
        [DataMember]
        public PwaVideosBySubcategory MotorSports { get; set; }
        [DataMember]
        public PwaVideosBySubcategory PowerDriftSpecials { get; set; }
        [DataMember]
        public PwaVideosBySubcategory PowerDriftTopMusic { get; set; }
        [DataMember]
        public PwaVideosBySubcategory Miscellaneous { get; set; }
    }
}
