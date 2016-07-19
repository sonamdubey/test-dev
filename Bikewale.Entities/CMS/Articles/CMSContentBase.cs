using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Bikewale.Entities.CMS.Articles
{
    [Serializable, DataContract]
    public class CMSContent
    {
        [DataMember]
        public IList<ArticleSummary> Articles { get; set; }
        [DataMember]
        public uint RecordCount { get; set; }
    }
}
