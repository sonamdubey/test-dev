using Bikewale.Entities.CMS.Articles;
using System.Collections.Generic;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 23 Mar 2017
    /// Summary    : View model for expert reviews widget
    /// </summary>
    public class RecentExpertReviewsVM
    {
        public string Title { get; set; }
        public IEnumerable<ArticleSummary> ArticlesList { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string MakeMasking { get; set; }
        public string ModelMasking { get; set; }
        public string LinkTitle { get; set; }
        public string MoreExpertReviewUrl { get; set; }
        public string BikeName { get; set; }
        public int FetchedCount { get; set; }
        public bool IsViewAllLink { get; set; }
        public uint ModelCount { get; set; }
        public uint ExpertReviewCount { get; set; }
    }
}
