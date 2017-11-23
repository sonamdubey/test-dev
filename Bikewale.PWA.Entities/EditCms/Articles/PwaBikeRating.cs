using System;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    ///  Created By: Prasad Gawde
    /// </summary>
    [Serializable, DataContract]
    public class PwaBikeRating
    {
        [DataMember]
        public UInt16 ReviewCount { get; set; }
        [DataMember]
        public string Rating { get; set; }
        [DataMember]
        public UInt16 Count { get; set; }
        [DataMember]
        public string ReviewUrl { get; set; }
    }
}
