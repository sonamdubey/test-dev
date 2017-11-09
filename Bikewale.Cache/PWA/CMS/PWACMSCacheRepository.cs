using Bikewale.Entities.CMS.Articles;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.PWA.CMS;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using Bikewale.Entities.PWA.Articles;
using System.Web;
using Bikewale.Utility;

namespace Bikewale.Cache.PWA.CMS
{
    public class PWACMSCacheRepository : IPWACMSCacheRepository
    {
        private readonly ICacheManager _cache;
        private readonly IPWACMSContentRepository _objPWACmsContents;

        static readonly int _minsToCacheCms = BWConfiguration.Instance. PwaRenderedHtmlCacheLimitMins;

        public PWACMSCacheRepository(ICacheManager cache, IPWACMSContentRepository objPWACmsContents)
        {
            _cache = cache;
            _objPWACmsContents = objPWACmsContents;
        }
    

        /// <summary>
        /// Created By : Prasad Gawde on 25 May 2017
        /// Description : Caching for News List Rendered Data                 
        public IHtmlString GetNewsListDetails(string key, PwaNewsArticleListReducer reducer, string url, string containerId, string componentName)
        {
            IHtmlString outStr=null;    
            try
            {
                outStr = new HtmlString(_cache.GetFromCache<String>(key, new TimeSpan(0, 0, _minsToCacheCms, 0), () => Convert.ToString(_objPWACmsContents.GetNewsListDetails(reducer,url,containerId,componentName))));
            }
            catch (Exception ex)
            {
                PwaErrorClass objErr = new PwaErrorClass(ex, "PWACMSCacheRepository.GetNewsListDetails",url);
            }
            return outStr;
        }

       

        /// <summary>
        /// Created By : Prasad Gawde on 25 May 2017
        /// Description : Caching for News Detail Rendered Data
        public IHtmlString GetNewsDetails(string key, PwaNewsDetailReducer reducer, string url, string containerId, string componentName)
        {
            IHtmlString outStr = null;
            try
            {
                outStr = new HtmlString(_cache.GetFromCache<string>(key, new TimeSpan(0, 0, _minsToCacheCms, 0), () => Convert.ToString(_objPWACmsContents.GetNewsDetails(reducer, url, containerId, componentName))));
            }
            catch (Exception ex)
            {
                PwaErrorClass objErr = new PwaErrorClass(ex, "PWACMSCacheRepository.GetNewsDetails: ",url);
            }
            return outStr;
        }

        /// <summary>
        /// Cache Rendered Html for Video List
        /// </summary>
        /// <param name="key"></param>
        /// <param name="reducer"></param>
        /// <param name="url"></param>
        /// <param name="containerId"></param>
        /// <param name="componentName"></param>
        /// <returns></returns>
        public IHtmlString GetVideoListDetails(string key, PwaAllVideos reducer, string url, string containerId, string componentName)
        {
            IHtmlString outStr = null;
            try
            {
                outStr = new HtmlString(_cache.GetFromCache<String>(key, new TimeSpan(0, 0, _minsToCacheCms, 0), () => Convert.ToString(_objPWACmsContents.GetVideoListDetails(reducer, url, containerId, componentName))));
            }
            catch (Exception ex)
            {
                PwaErrorClass objErr = new PwaErrorClass(ex, "PWACMSCacheRepository.GetVideoListDetails: ",url);
            }
            return outStr;
        }

        /// <summary>
        /// Cache Rendered Html for Sub Category Video List
        /// </summary>
        /// <param name="key"></param>
        /// <param name="reducer"></param>
        /// <param name="url"></param>
        /// <param name="containerId"></param>
        /// <param name="componentName"></param>
        /// <returns></returns>
        public IHtmlString GetVideoBySubCategoryListDetails(string key, PwaVideosBySubcategory reducer, string url, string containerId, string componentName)
        {
            IHtmlString outStr = null;
            try
            {
                outStr = new HtmlString(_cache.GetFromCache<String>(key, new TimeSpan(0, 0, _minsToCacheCms, 0), () => Convert.ToString(_objPWACmsContents.GetVideoBySubCategoryListDetails(reducer, url, containerId, componentName))));
            }
            catch (Exception ex)
            {
                PwaErrorClass objErr = new PwaErrorClass(ex, "PWACMSCacheRepository.GetVideoBySubCategoryListDetails: ",url);
            }
            return outStr;
        }

        /// <summary>
        /// Cache Rendered Html for Video Detail Page
        /// </summary>
        /// <param name="key"></param>
        /// <param name="reducer"></param>
        /// <param name="url"></param>
        /// <param name="containerId"></param>
        /// <param name="componentName"></param>
        /// <returns></returns>
        public IHtmlString GetVideoDetails(string key, PwaVideoDetailReducer reducer, string url, string containerId, string componentName)
        {
            IHtmlString outStr = null;
            try
            {
                outStr = new HtmlString(_cache.GetFromCache<String>(key, new TimeSpan(0, 0, _minsToCacheCms, 0), () => Convert.ToString(_objPWACmsContents.GetVideoDetails(reducer, url, containerId, componentName))));
            }
            catch (Exception ex)
            {
                PwaErrorClass objErr = new PwaErrorClass(ex, "PWACMSCacheRepository.GetVideoDetails",url);
            }
            return outStr;
        }
    }
}
