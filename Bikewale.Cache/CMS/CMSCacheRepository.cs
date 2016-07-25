using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

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
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public ArticleDetails GetNewsDetails(uint basicId)
        {
            ArticleDetails _objArticleDetails = null;
            string key = string.Format("BW_NewsDetails_{0}", basicId);
            bool isDataFromCache = false;
            try
            {
                _objArticleDetails = _cache.GetFromCache<ArticleDetails>(key, new TimeSpan(1, 0, 0), () => _objArticles.GetNewsDetails(basicId), out isDataFromCache);
                if (isDataFromCache)
                {
                    //Update the view count
                    //_objArticles.UpdateViewCount(basicId);
                    if (basicId > 0)
                    {
                        NameValueCollection nvc = new NameValueCollection();
                        nvc.Add("ContentId", basicId.ToString());
                        SyncBWData.PushToQueue("cw.UpdateContentViewCount", DataBaseName.CW, nvc);
                    }
                }
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
                string key = string.Format("BW_Articles_Recent_{0}_Cnt_{1}", contentTypeIds.Replace(',', '_'), totalRecords);

                if (modelId > 0)
                {
                    key = string.Format("{0}_MO_{1}", key, modelId);
                }
                else if (makeId > 0)
                {
                    key = string.Format("{0}_MK_{1}", key, makeId);
                }

                _objArticlesList = _cache.GetFromCache<IEnumerable<ArticleSummary>>(key, new TimeSpan(1, 0, 0), () => _objArticles.GetMostRecentArticlesByIdList(contentTypeIds, totalRecords, makeId, modelId));

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
            string key = string.Empty;
            try
            {
                key = string.Format("BW_Articles_List_S_{0}_E_{1}_CL_{2}", startIndex, endIndex, categoryIdList.Replace(',', '_'));

                if (modelId > 0)
                {
                    key = string.Format("{0}_MO_{1}", key, modelId);
                }
                else if (makeId > 0)
                {
                    key = string.Format("{0}_MK_{1}", key, makeId);
                }

                _objArticlesList = _cache.GetFromCache<CMSContent>(key, new TimeSpan(1, 0, 0), () => _objArticles.GetArticlesByCategoryList(categoryIdList, startIndex, endIndex, makeId, modelId));
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
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public IEnumerable<ModelImage> GetArticlePhotos(int basicId)
        {
            IEnumerable<ModelImage> objImages = null;
            try
            {
                string cacheKey = String.Format("BW_Article_Photos_{0}", basicId);
                objImages = _cache.GetFromCache<IEnumerable<ModelImage>>(cacheKey, new TimeSpan(1, 0, 0), () => _objArticles.GetArticlePhotos(basicId));
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
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public ArticlePageDetails GetArticlesDetails(uint basicId)
        {

            ArticlePageDetails _objArticleDetails = null;
            string key = string.Format("BW_Article_Details_{0}", basicId);
            bool isDataFromCache = false;
            try
            {
                _objArticleDetails = _cache.GetFromCache<ArticlePageDetails>(key, new TimeSpan(1, 0, 0), () => _objArticles.GetArticleDetails(basicId), out isDataFromCache);
                if (isDataFromCache)
                {
                    //Update the view count
                    //_objArticles.UpdateViewCount(basicId);
                    if (basicId > 0)
                    {
                        NameValueCollection nvc = new NameValueCollection();
                        nvc.Add("ContentId", basicId.ToString());
                        SyncBWData.PushToQueue("cw.UpdateContentViewCount", DataBaseName.CW, nvc);
                    }
                }
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
