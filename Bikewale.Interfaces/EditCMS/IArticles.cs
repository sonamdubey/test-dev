using System.Collections.Generic;
using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.PWA.Articles;

namespace Bikewale.Interfaces.EditCMS
{
    /// <summary>
    /// Author : Vivek Gupta on 9-5-2016
    /// Desc : fn Definition added for the new class Articles
    /// Modified by :   Sumit Kate on 25 July 2016
    /// Description :   Added a method to Update the view count of article
    /// Modified by : Aditi Srivastava on 14 June 2017
    /// Summary     : Added overload for GetMostRecentArticlesByIdList(bodystyleId)
    /// Modified by: Vivek Singh Tomar on 16th Aug 2017
    /// Summary: Added overload for GetArticlesByCategoryList(bodyStyleId)
    /// Modified by: Ashutosh Sharma on 20-Sep-2017
    /// Description : Added GetArticlesByCategoryList with parameters 'categoryIdList, startIndex, endIndex'
    /// Modified by : Vivek Singh Tomar on 24th Nov 2017
    /// Summary : Added GetArticlesByCategoryList to fetch article details for given list of model ids
    /// </summary>
    public interface IArticles
    {
        ArticleDetails GetNewsDetails(uint basicId);
        ArticlePageDetails GetArticleDetails(uint basicId);
        IEnumerable<ModelImage> GetArticlePhotos(int basicId);
        IEnumerable<ArticleSummary> GetMostRecentArticlesByIdList(string categoryIdList, uint totalRecords, uint makeId, uint modelId);
        CMSContent GetArticlesByCategoryList(string categoryIdList, int startIndex, int endIndex, int makeId, int modelId);
        void UpdateViewCount(uint basicId);
        IEnumerable<ArticleSummary> GetMostRecentArticlesByIdList(string categoryIdList, uint totalRecords,string bodyStyleId, uint makeId, uint modelId);
        CMSContent GetArticlesByCategoryList(string categoryIdList, int startIndex, int endIndex, string bodyStyleId, int makeId);
        CMSContent GetArticlesByCategoryList(string categoryIdList, int startIndex, int endIndex);
        PwaContentBase GetArticlesByCategoryListPwa(string categoryIdList, int startIndex, int endIndex, int makeId, int modelId);
        IEnumerable<ArticleSummary> GetMostRecentArticlesByIdList(string categoryIdList, uint totalRecords, uint makeId, string modelIds);
        CMSContent GetArticlesByCategoryList(string categoryIdList, int startIndex, int endIndex, int makeId, string modelIds);
	CMSContent GetContentListBySubCategoryId(uint startIndex, uint endIndex, string categoryIdList, string subCategoryIdList, int makeId = 0, int modelId = 0);

	}
}
