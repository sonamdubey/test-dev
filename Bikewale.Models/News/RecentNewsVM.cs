using Bikewale.Entities.CMS.Articles;
using System.Collections.Generic;

namespace Bikewale.Models.News
{
    /// <summary>
    /// Created by : Aditi Srivastava on 23 Mar 2017
    /// Summary    : View model for news widget
    /// </summary>
    public class RecentNewsVM
    {
        public IEnumerable<ArticleSummary> ArticlesList { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get;set;}
        public string MakeMasking { get; set; }
        public string ModelMasking { get; set; }
    }
}
