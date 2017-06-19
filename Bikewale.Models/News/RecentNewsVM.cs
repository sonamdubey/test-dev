using Bikewale.Entities.CMS.Articles;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 23 Mar 2017
    /// Summary    : View model for news widget
    /// </summary>
    public class RecentNewsVM
    {
        public string Title { get; set; }
        public IEnumerable<ArticleSummary> ArticlesList { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string MakeMasking { get; set; }
        public string ModelMasking { get; set; }
        public int FetchedCount { get; set; }
    }
}
