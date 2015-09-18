using Bikewale.Entities.CMS.Articles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Entities.CMS.Articles
{    
    public class CMSContent
    {
        public IList<ArticleSummary> Articles { get; set; }
        public uint RecordCount { get; set; }
    }
}
