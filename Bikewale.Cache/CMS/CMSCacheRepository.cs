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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public ArticleDetails GetNewsDetails(uint basicId)
        {
            ArticleDetails _objArticleDetails = null;
            string key = string.Format("BW_NewsDetails_", basicId);
            try
            {
                _objArticleDetails = _cache.GetFromCache<ArticleDetails>(key, new TimeSpan(1, 0, 0), () => _objArticles.GetNewsDetails(basicId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "CMSCacheRepository.GetNewsDetails");
                objErr.SendMail();
            }
            return _objArticleDetails;
        }

        /// <summary>
        /// 
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
                    key += string.Format("_MO_{0}", modelId);
                }
                else if (makeId > 0)
                {
                    key += string.Format("_MK_{0}", makeId);
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

        /// <summary>
        /// 
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

                if (modelId > 0)
                {
                    key = String.Format("BW_Articles_List_S_{0}_E_{1}_CL_{2}_M_{3}", startIndex, endIndex, categoryIdList.Replace(',', '_'), modelId);

                }
                else
                {
                    key = String.Format("BW_Articles_List_S_{0}_E_{1}_CL_{2}", startIndex, endIndex, categoryIdList.Replace(',', '_'));
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

        /// <summary>
        /// Author : Vivek Gupta on 18-07-2016
        /// Desc: this function moved from content/RoadTest/ViewRT.aspx.cs for caching and used for feature details
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="basicId"></param>
        /// <returns></returns>
        public ArticlePageDetails GetArticlesDetails(uint basicId)
        {

            ArticlePageDetails _objArticleDetails = null;
            string key = string.Format("BW_Article_Details_", basicId);
            try
            {
                _objArticleDetails = _cache.GetFromCache<ArticlePageDetails>(key, new TimeSpan(1, 0, 0), () => _objArticles.GetArticleDetails(basicId));
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
