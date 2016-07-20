using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
using System.Collections.Generic;

namespace Bikewale.Interfaces.EditCMS
{
    /// <summary>
    /// Author : Vivek Gupta on 9-5-2016
    /// Desc : fn Definition added for the new class Articles
    /// </summary>
    public interface IArticles
    {
        ArticleDetails GetNewsDetails(uint basicId);
        IEnumerable<ArticleSummary> GetMostRecentArticlesById(EnumCMSContentType categoryId, uint totalRecords, uint makeId, uint modelId);
        IEnumerable<ArticleSummary> GetMostRecentArticlesByIdList(string categoryIdList, uint totalRecords, uint makeId, uint modelId);
        CMSContent GetArticlesByCategory(EnumCMSContentType categoryId, int startIndex, int endIndex, int makeId, int modelId);
    }
}
