using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using System.Collections.Generic;

namespace Bikewale.Interfaces.EditCMS
{
    /// <summary>
    /// Author : Vivek Gupta on 9-5-2016
    /// Desc : fn Definition added for the new class Articles
    /// Modified by :   Sumit Kate on 25 July 2016
    /// Description :   Added a method to Update the view count of article
    /// </summary>
    public interface IArticles
    {
        ArticleDetails GetNewsDetails(uint basicId);
        ArticlePageDetails GetArticleDetails(uint basicId);
        IEnumerable<ModelImage> GetArticlePhotos(int basicId);
        IEnumerable<ArticleSummary> GetMostRecentArticlesByIdList(string categoryIdList, uint totalRecords, uint makeId, uint modelId);
        CMSContent GetArticlesByCategoryList(string categoryIdList, int startIndex, int endIndex, int makeId, int modelId);
        void UpdateViewCount(uint basicId);
    }
}
