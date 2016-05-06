using Bikewale.Entities.CMS.Articles;
using System.Collections.Generic;

namespace Bikewale.Interfaces.EditCMS
{
    public interface IArticles
    {
        IEnumerable<ArticleSummary> GetRecentNews(int makeId, int modelId, int totalRecords);
        IEnumerable<ArticleSummary> GetRecentExpertReviews(int makeId, int modelId, int totalRecords);
    }
}
