using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.PWA.Articles
{
    /// <summary>
    ///  Created By: Prasad Gawde
    /// Modified By : Pratibha Verma on 24 February, 2018
    /// Summary : Added PageTitle for the page
    /// </summary>
    [Serializable, DataContract]
    public class PwaContentBase
    {
        [DataMember]
        public string PageTitle { get; set; }
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
