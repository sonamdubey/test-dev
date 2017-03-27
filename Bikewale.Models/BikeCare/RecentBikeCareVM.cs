using Bikewale.Entities.CMS.Articles;
using System.Collections.Generic;

namespace Bikewale.Models.BikeCare
{
    /// <summary>
    /// Created by : Aditi Srivastava on 24 Mar 2017
    /// Summary    : View model for bike care widget
    /// </summary>
    public class RecentBikeCareVM
    {
        public IEnumerable<ArticleSummary> ArticlesList { get; set; }
        public string MakeName { get; set; }
        public string ModelName { get; set; }
        public string MakeMasking { get; set; }
        public string ModelMasking { get; set; }
    }
}
