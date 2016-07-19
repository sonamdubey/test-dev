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
        IEnumerable<ArticleSummary> GetRecentNews(int makeId, int modelId, int totalRecords);
        IEnumerable<ArticleSummary> GetRecentExpertReviews(int makeId, int modelId, int totalRecords);
        ArticleDetails GetNewsDetails(uint basicId);
    }
}
