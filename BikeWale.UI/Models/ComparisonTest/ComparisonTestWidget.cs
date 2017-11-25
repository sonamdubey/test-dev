
using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.CMS;
using Bikewale.Notifications;
using Bikewale.Utility;
using System.Collections.Generic;
namespace Bikewale.Models
{
    /// <summary>
    /// Created By :- Subodh Jain 09 May 2017
    /// Summary :- Model for comparison test widget
    /// </summary>
    public class ComparisonTestWidget
    {
        private readonly ICMSCacheContent _CompareTest = null;

        public uint topCount { get; set; }

        public ComparisonTestWidget(ICMSCacheContent CompareTest)
        {
            _CompareTest = CompareTest;
        }
        /// <summary>
        /// Created By :- Subodh Jain 09 May 2017
        /// Summary :-Fetch data for content type comparisontest
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ArticleSummary> GetData()
        {
            IEnumerable<ArticleSummary> ArticlesList = null;
            try
            {
                IList<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.ComparisonTests);
                string _contentType = CommonApiOpn.GetContentTypesString(categorList);
                ArticlesList = _CompareTest.GetMostRecentArticlesByIdList(_contentType, topCount, 0, 0);
            }
            catch (System.Exception ex)
            {

                ErrorClass.LogError(ex, "Bikewale.Models.ComparisonTestWidget.GetData()");
            }
            return ArticlesList;
        }
    }
}