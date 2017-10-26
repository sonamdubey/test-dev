using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    [Serializable, DataContract]
    public class PwaBikeCms
    {
        [DataMember]
        public string Heading { get; set; }
        [DataMember]
        public string CompleteListUrl { get; set; }
        [DataMember]
        public string CompleteListUrlLabel { get; set; }
        [DataMember]
        public string CompleteListUrlAlternateLabel { get; set; }

    }
}
