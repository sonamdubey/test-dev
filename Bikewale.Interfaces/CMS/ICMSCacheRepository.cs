using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using System.Collections.Generic;

namespace Bikewale.Interfaces.CMS
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICMSCacheContent
    {
        ArticleDetails GetNewsDetails(uint basicId);
        IEnumerable<ArticleSummary> GetMostRecentArticlesById(EnumCMSContentType contentType, uint totalRecords, uint makeId, uint modelId);
        IEnumerable<ArticleSummary> GetMostRecentArticlesByIdList(string categoryIdList, uint totalRecords, uint makeId, uint modelId);
        CMSContent GetArticlesByCategory(EnumCMSContentType categoryId, int startIndex, int endIndex, int makeId, int modelId);

    }
}
