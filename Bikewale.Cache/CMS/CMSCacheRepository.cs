using Bikewale.Entities.CMS.Articles;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Notifications;
using System;

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
                _objArticleDetails = _cache.GetFromCache<ArticleDetails>(key, new TimeSpan(0, 30, 0), () => _objArticles.GetNewsDetails(basicId));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "DealerCacheRepository.GetDealerByMakeCity");
                objErr.SendMail();
            }
            return _objArticleDetails;
        }
    }
}
