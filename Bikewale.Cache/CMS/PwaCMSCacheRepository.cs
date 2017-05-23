using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Pwa.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using Bikewale.Entities.PWA.Articles;

namespace Bikewale.Cache.CMS
{
    public class PwaCMSCacheRepository : IPwaCMSCacheRepository
    {
        private readonly ICacheManager _cache;
        private readonly IArticles _objArticles;

        public PwaCMSCacheRepository(ICacheManager cache, IArticles objArticles)
        {
            _cache = cache;
            _objArticles = objArticles;
        }

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
        public PwaNewsListContent GetArticlesByCategoryList(string categoryIdList, int startIndex, int endIndex, int makeId, int modelId)
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
            return  new PwaNewsListContent() { Articles = _objArticlesList };
        }
    }
}
