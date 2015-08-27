
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.DTO.CMS.Articles
{
    [Serializable]
    public class CMSContent
    {
        public IList<CMSArticleSummary> Articles { get; set; }
        public uint RecordCount { get; set; }
    }
}
