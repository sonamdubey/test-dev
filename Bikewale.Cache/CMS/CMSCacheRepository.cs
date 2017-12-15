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
                ErrorClass.LogError(ex, "CMSCacheRepository.GetNewsDetails");

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
                ErrorClass.LogError(ex, "CMSCacheRepository.GetMostRecentArticlesByIdList");

            }
            return _objArticlesList;
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 14 June 2017
        /// Summary    : To get list of articles by bosystyle
        /// </summary>
        public IEnumerable<ArticleSummary> GetMostRecentArticlesByIdList(string contentTypeIds, uint totalRecords, string bodyStyleId, uint makeId, uint modelId)
        {
            IEnumerable<ArticleSummary> _objArticlesList = null;

            try
            {
                if (_objArticles != null)
                    _objArticlesList = _objArticles.GetMostRecentArticlesByIdList(contentTypeIds, totalRecords, bodyStyleId, makeId, modelId);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "CMSCacheRepository.GetMostRecentArticlesByIdList");

            }
            return _objArticlesList;
        }

        /// <summary>
        /// Created By :Snehal Dange on 30th Oct 2017
        /// Description : News articles for multiple modelids
        /// </summary>
        /// <param name="contentTypeIds"></param>
        /// <param name="totalRecords"></param>
        /// <param name="bodyStyleId"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public IEnumerable<ArticleSummary> GetMostRecentArticlesByIdList(string categoryIdList, uint totalRecords, uint makeId, string modelIdList)
        {
            IEnumerable<ArticleSummary> _objArticlesList = null;

            try
            {
                if (_objArticles != null)
                    _objArticlesList = _objArticles.GetMostRecentArticlesByIdList(categoryIdList, totalRecords, makeId, modelIdList);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "CMSCacheRepository.GetMostRecentArticlesByIdList");

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
                ErrorClass.LogError(ex, "CMSCacheRepository.GetArticlesByCategoryList");

            }
            return _objArticlesList;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 27th Nov 2017
        /// Summary : Overload for GetArticlesByCategoryList for fetching articles when list of modelids is given
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public CMSContent GetArticlesByCategoryList(string categoryIdList, int startIndex, int endIndex, int makeId, string modelId)
        {
            CMSContent _objArticlesList = null;

            try
            {
                if (_objArticles != null)
                    _objArticlesList = _objArticles.GetArticlesByCategoryList(categoryIdList, startIndex, endIndex, makeId, modelId);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("CMSCacheRepository.GetArticlesByCategoryList ModelIds = {0}", modelId));
            }
            return _objArticlesList;
        }

        /// <summary>
        /// Created by: Vivek Singh Tomar on 16th Aug 2017
        /// Summary: Get articles for given category and body style
        /// </summary>
        /// <param name="categoryIdList"></param>
        /// <param name="startIndex"></param>
        /// <param name="endIndex"></param>
        /// <param name="bodyStyleId"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public CMSContent GetArticlesByCategoryList(string categoryIdList, int startIndex, int endIndex, string bodyStyleId, int makeId)
        {
            CMSContent _objArticlesList = null;
            try
            {
                if (_objArticles != null)
                    _objArticlesList = _objArticles.GetArticlesByCategoryList(categoryIdList, startIndex, endIndex, bodyStyleId, makeId);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.CMS.CMSCacheRepository.GetArticlesByCategoryList");

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
                ErrorClass.LogError(ex, "CMSCacheRepository.GetArticlePhotos");

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
                ErrorClass.LogError(ex, "CMSCacheRepository.GetArticlesDetails");

            }
            return _objArticleDetails;
        }



        public CMSContent GetTrackDayArticlesByCategoryList(string categoryIdList, int startIndex, int endIndex, int makeId, int modelId)
        {
            CMSContent objFeaturedArticles = null;
            try
            {
                string apiUrl = string.Format("/webapi/article/listbycategory/?applicationid=2&categoryidlist={0}&startindex={1}&endindex={2}", categoryIdList, startIndex, endIndex);
                if (makeId > 0 && modelId > 0)
                {
                    apiUrl = string.Format("{0}&makeid={1}&modelid={2}", apiUrl, makeId, modelId);
                }
                else
                {
                    if (makeId > 0)
                    {
                        apiUrl = string.Format("{0}&makeid={1}", apiUrl, makeId);
                    }
                    else
                    {
                        apiUrl = string.Format("{0}&modelid={1}", apiUrl, modelId);
                    }
                }

                using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                {
                    return objClient.GetApiResponseSync<Bikewale.Entities.CMS.Articles.CMSContent>(Utility.APIHost.CW, Utility.BWConfiguration.Instance.APIRequestTypeJSON, apiUrl, objFeaturedArticles);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "");

            }

            return objFeaturedArticles;
        }

		public CMSContent GetContentListBySubCategoryId(uint startIndex, uint endIndex, string categoryIdList, string subCategoryIdList, int makeId = 0, int modelId = 0)
		{
			 
			try
			{
				return _objArticles.GetContentListBySubCategoryId(startIndex, endIndex, categoryIdList, subCategoryIdList, makeId, modelId);
			}
			catch (Exception ex)
			{
				ErrorClass.LogError(ex, "CMSCacheRepository.GetContentListBySubCategoryId");
			}
			return null;
		}
	}
}
