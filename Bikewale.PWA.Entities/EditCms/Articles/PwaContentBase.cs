using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    [Serializable, DataContract]
    public class PwaContentBase
    {
        [DataMember]
        public IList<PwaArticleSummary> Articles { get; set; }
        [DataMember]
        public uint RecordCount { get; set; }
        [DataMember]
        public uint StartIndex { get; set; }
        [DataMember]
        public uint EndIndex { get; set; }
    }
}
