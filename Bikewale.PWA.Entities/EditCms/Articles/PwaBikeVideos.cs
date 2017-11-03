using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    [Serializable, DataContract]
    public class PwaBikeVideos: PwaBikeCms
    {
        [DataMember]
        public List<PwaBikeVideoEntity> VideosList { get; set; }    
    }
}
