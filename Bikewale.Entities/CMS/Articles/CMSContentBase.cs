using System.Collections.Generic;

namespace Bikewale.Entities.CMS.Articles
{
    public class CMSContent
    {
        public IList<ArticleSummary> Articles { get; set; }
        public uint RecordCount { get; set; }
    }
}
