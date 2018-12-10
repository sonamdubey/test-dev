using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Entity.CMS.Articles
{
    /// <summary>
    /// written by Natesh kumar on 25/9/14
    /// </summary>
    [Serializable]
    public class CMSContent
    {
        public IList<ArticleSummary> Articles { get; set; }
        public uint RecordCount { get; set; }
        public int NewsRecordCount { get; set; }
        public int ExpertReviewsRecordCount { get; set; }
        public int FeaturesRecordCount { get; set; }
    }
}
