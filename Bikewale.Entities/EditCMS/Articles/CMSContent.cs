using System.Collections.Generic;

namespace Bikewale.Entity.CMS.Articles
{

    public class CMSContent
    {
        public IList<ArticleSummary> Articles { get; set; }
        public uint RecordCount { get; set; }
    }
}
