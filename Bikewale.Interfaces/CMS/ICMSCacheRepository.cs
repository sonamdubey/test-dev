using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using System.Collections.Generic;

namespace Bikewale.Interfaces.CMS
{
    /// <summary>
    /// Modified by : Aditi Srivastava on 14 June 2017
    /// Summary     : Added overload for GetMostRecentArticlesByIdList(bodystyleId)
    /// Modified by: Vivek Singh Tomar on 16th Aug 2017
    /// Summary: Added overload for GetArticlesByCategoryList(bodyStyleId)
    /// </summary>
    public interface ICMSCacheContent
    {
        ArticleDetails GetNewsDetails(uint basicId);
        ArticlePageDetails GetArticlesDetails(uint basicId);
        IEnumerable<ModelImage> GetArticlePhotos(int basicId);
        IEnumerable<ArticleSummary> GetMostRecentArticlesByIdList(string categoryIdList, uint totalRecords, uint makeId, uint modelId);
        IEnumerable<ArticleSummary> GetMostRecentArticlesByIdList(string categoryIdList, uint totalRecords, uint makeId, string modelIdList);
        IEnumerable<ArticleSummary> GetMostRecentArticlesByIdList(string categoryIdList, uint totalRecords, string bodyStyleId, uint makeId, uint modelId);
        CMSContent GetArticlesByCategoryList(string categoryIdList, int startIndex, int endIndex, int makeId, int modelId);
        CMSContent GetTrackDayArticlesByCategoryList(string categoryIdList, int startIndex, int endIndex, int makeId, int modelId);
        CMSContent GetArticlesByCategoryList(string categoryIdList, int startIndex, int endIndex, string bodyStyleId, int makeId);

    }
}
