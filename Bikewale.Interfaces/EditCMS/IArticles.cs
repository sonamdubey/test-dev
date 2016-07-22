using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
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
        ArticlePageDetails GetArticleDetails(uint basicId);
        IEnumerable<ModelImage> GetArticlePhotos(int basicId);
        IEnumerable<ArticleSummary> GetMostRecentArticlesByIdList(string categoryIdList, uint totalRecords, uint makeId, uint modelId);
        CMSContent GetArticlesByCategoryList(string categoryIdList, int startIndex, int endIndex, int makeId, int modelId);


    }
}
