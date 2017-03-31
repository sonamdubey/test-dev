
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Pager;
using System.Collections.Generic;
namespace Bikewale.Models
{
    public class IndexFeatureVM
    {
        public IList<ArticleSummary> ArticlesList { get; set; }
        public PagerEntity PagerEntity { get; set; }
        public string prevPageUrl { get; set; }
        public string nextPageUrl { get; set; }
        public uint StartIndex { get; set; }
        public uint EndIndex { get; set; }
        public uint TotalArticles { get; set; }
    }
}
