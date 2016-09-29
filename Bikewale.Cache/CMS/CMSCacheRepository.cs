using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;

namespace Bikewale.Cache.CMS
{
    public class CMSCacheRepository : ICMSCacheContent
    {
        private readonly ICacheManager _cache;
        private readonly IArticles _objArticles;

        public CMSCacheRepository(ICacheManager cache, IArticles objArticles)
        {
            _cache = cache;
            _objArticles = objArticles;
        }

        #region Get News Details
        /// <summary>
        /// Created By : Sushil Kumar on 21st July 2016
        /// Description : Caching for News Details based on basic id 
        /// Modified by :   Sumit Kate on 25 July 2016
        /// Description :   If data is fetched from cache update the view count
        /// When data is fetched from Memcache the view count should be updated in carwale edit CMS
        /// Modified by : Sajal Gupta on 22/09/2016
        /// Description : Bikewale caching is disabled. Directly call to BAL which in turn call Grpc and hence use Grpc caching.
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public ArticleDetails GetNewsDetails(uint basicId)
        {
            ArticleDetails _objArticleDetails = null;

            try
            {
                if (_objArticles != null)
                    _objArticleDetails = _objArticles.GetNewsDetails(basicId);
                if (_objArticleDetails != null && basicId > 0)
                    _objArticles.UpdateViewCount(basicId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "CMSCacheRepository.GetNewsDetails");
                objErr.SendMail();
            }
            return _objArticleDetails;
        }
        #endregion

        #region Most Recent Articles
        /// <summary>
        /// Created By : Sushil Kumar on 21st July 2016
        /// Description : Caching for Most recent Articles Details based on based on contentslistIds and make,model
        /// Modified by : Sajal Gupta on 22/09/2016
        /// Description : Bikewale caching is disabled. Directly call to BAL which in turn call Grpc and hence use Grpc caching.
        /// </summary>
        /// <param name="contentTypeIds"></param>
        /// <param name="totalRecords"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<ArticleSummary> GetMostRecentArticlesByIdList(string contentTypeIds, uint totalRecords, uint makeId, uint modelId)
        {
            IEnumerable<ArticleSummary> _objArticlesList = null;

            try
            {
                if (_objArticles != null)
                    _objArticlesList = _objArticles.GetMostRecentArticlesByIdList(contentTypeIds, totalRecords, makeId, modelId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "CMSCacheRepository.GetMostRecentArticlesByIdList");
                objErr.SendMail();
            }
            return _objArticlesList;
        }
        #endregion

        #region Articles By category
        /// <summary>
        /// Created By : Sushil Kumar on 21st July 2016
        /// Description : Caching for Articles by list according to pagination
        /// Modified by : Sajal Gupta on 22/09/2016
        /// Description : Bikewale caching is disabled. Directly call to BAL which in turn call Grpc and hence use Grpc caching.
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public CMSContent GetArticlesByCategoryList(string categoryIdList, int startIndex, int endIndex, int makeId, int modelId)
        {
            CMSContent _objArticlesList = null;

            try
            {
                if (_objArticles != null)
                    _objArticlesList = _objArticles.GetArticlesByCategoryList(categoryIdList, startIndex, endIndex, makeId, modelId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "CMSCacheRepository.GetArticlesByCategoryList");
                objErr.SendMail();
            }
            return _objArticlesList;
        }
        #endregion

        #region Article Photos
        /// <summary>
        /// Created By : Vivek Gupta on 18th July 2016
        /// Description : Caching for Articles Photos based on basic id
        /// Modified by : Sajal Gupta on 22/09/2016
        /// Description : Bikewale caching is disabled. Directly call to BAL which in turn call Grpc and hence use Grpc caching.
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public IEnumerable<ModelImage> GetArticlePhotos(int basicId)
        {
            IEnumerable<ModelImage> objImages = null;

            try
            {
                if (_objArticles != null)
                    objImages = _objArticles.GetArticlePhotos(basicId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "CMSCacheRepository.GetArticlePhotos");
                objErr.SendMail();
            }

            return objImages;
        }
        #endregion

        /// <summary>
        /// Created By : Sushil Kumar on 21st July 2016
        /// Description : Caching for Articles Details based on basic id
        /// Modified by :   Sumit Kate on 25 July 2016
        /// Description :   If data is fetched from cache update the view count
        /// When data is fetched from Memcache the view count should be updated in carwale edit CMS
        /// Modified by : Sajal Gupta on 22/09/2016
        /// Description : Bikewale caching is disabled. Directly call to BAL which in turn call Grpc and hence use Grpc caching.
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public ArticlePageDetails GetArticlesDetails(uint basicId)
        {
            ArticlePageDetails _objArticleDetails = null;

            try
            {
                if (_objArticles != null)
                    _objArticleDetails = _objArticles.GetArticleDetails(basicId);
                if (_objArticleDetails != null && basicId > 0)
                    _objArticles.UpdateViewCount(basicId);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "CMSCacheRepository.GetArticlesDetails");
                objErr.SendMail();
            }
            return _objArticleDetails;
        }

    }
}
