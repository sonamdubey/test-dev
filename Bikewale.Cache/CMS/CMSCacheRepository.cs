using Bikewale.Entities.CMS;
using Bikewale.Entities.CMS.Articles;
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

        public IEnumerable<ArticleSummary> GetMostRecentArticlesById(EnumCMSContentType contentType, uint totalRecords, uint makeId, uint modelId)
        {

            IEnumerable<ArticleSummary> _objArticlesList = null;

            try
            {
                string key = string.Format("BW_Articles_{0}_Cnt_{1}", contentType, totalRecords);

                if (makeId > 0 && modelId > 0)
                {
                    key += string.Format("_MK_{0}_MO_{1}" + makeId, modelId);
                }
                else if (makeId > 0 && modelId == 0)
                {
                    key += string.Format("_MK_{0}", makeId);
                }
                else if (makeId == 0 && modelId > 0)
                {
                    key += string.Format("_MO_{0}", modelId);
                }

                _objArticlesList = _cache.GetFromCache<IEnumerable<ArticleSummary>>(key, new TimeSpan(1, 0, 0), () => _objArticles.GetMostRecentArticlesById(contentType, totalRecords, makeId, modelId));

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "CMSCacheRepository.GetMostRecentArticlesById");
                objErr.SendMail();
            }
            return _objArticlesList;
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
                string key = string.Format("BW_Articles_{0}_Cnt_{1}", contentTypeIds.Replace(',', '_'), totalRecords);

                if (makeId > 0 && modelId > 0)
                {
                    key += string.Format("_MK_{0}_MO_{1}" + makeId, modelId);
                }
                else if (makeId > 0 && modelId == 0)
                {
                    key += string.Format("_MK_{0}", makeId);
                }
                else if (makeId == 0 && modelId > 0)
                {
                    key += string.Format("_MO_{0}", modelId);
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


        public CMSContent GetArticlesByCategory(EnumCMSContentType categoryId, int startIndex, int endIndex, int makeId, int modelId)
        {
            CMSContent _objArticlesList = null;
            string key = string.Empty;
            try
            {
                if (modelId > 0)
                {
                    key = String.Format("BW_ArticlesByCat_SI_{0}_EI_{1}_CL_{2}_M_{3}", startIndex, endIndex, categoryId, modelId);

                }
                else
                {
                    key = String.Format("BW_News_SI_{0}_EI_{1}_CL_{2}", startIndex, endIndex, categoryId);
                }

                _objArticlesList = _cache.GetFromCache<CMSContent>(key, new TimeSpan(0, 30, 0), () => _objArticles.GetArticlesByCategory(categoryId, startIndex, endIndex, makeId, modelId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "CMSCacheRepository.GetArticlesByCategory");
                objErr.SendMail();
            }
            return _objArticlesList;
        }
    }
}
