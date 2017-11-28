using System.Collections.Generic;

namespace Bikewale.DTO.CMS.Articles
{
    public class CMSContent
    {
        public IList<CMSArticleSummary> Articles { get; set; }
        public uint RecordCount { get; set; }
    }
}
