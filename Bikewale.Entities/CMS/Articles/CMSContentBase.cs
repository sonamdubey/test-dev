using Bikewale.Entities.CMS.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.CMS.Articles
{
    [DataContract, Serializable]
    public class CMSContent
    {
        [DataMember]
        public IList<ArticleSummary> Articles { get; set; }
        [DataMember]
        public uint RecordCount { get; set; }
    }
}
