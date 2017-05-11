
using Bikewale.Entities.CMS.Articles;
using System.Collections.Generic;
namespace Bikewale.Models
{

    /// Created By :- Subodh Jain 09 May 2017
    /// Summary :-View model for compare bike landing page
    /// </summary>

    public class CompareVM : ModelBase
    {
        public ComparisonWidgetVM CompareBikes { get; set; }
        public IEnumerable<ArticleSummary> ArticlesList { get; set; }
        public string CityName { get; set; }
    }
}
