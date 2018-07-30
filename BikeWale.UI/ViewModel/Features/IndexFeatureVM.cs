
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.Pager;
using Bikewale.Models.Shared;
using System.Collections.Generic;
namespace Bikewale.Models
{
    public class IndexFeatureVM : ModelBase
    {
        /// <summary>
        /// Modified by sajal Gupta on 01-12-2017
        /// Summary : Added PopularBikesAndPopularScootersWidget and UpcomingBikesAndUpcomingScootersWidget
        /// </summary>
        public IList<ArticleSummary> ArticlesList { get; set; }
        public PagerEntity PagerEntity { get; set; }
        public string prevPageUrl { get; set; }
        public string nextPageUrl { get; set; }
        public uint StartIndex { get; set; }
        public uint EndIndex { get; set; }
        public uint TotalArticles { get; set; }
        public IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> PageWidgets { get; set; }
    }
}
