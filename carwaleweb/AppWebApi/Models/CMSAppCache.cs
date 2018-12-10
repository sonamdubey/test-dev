using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AEPLCore.Cache;
using Newtonsoft.Json;
using RabbitMqPublishing;
using System.Collections.Specialized;
using Carwale.Utility;

namespace AppWebApi.Models
{
    public static class CMSAppCache
    {
        public static string GetContentDetails(string id)
        {
            string newsDetailsJson = null;
            try
            {
                CacheManager memcacheManager = new CacheManager();
                string key = string.Format("ContentDetails_App_v1_{0}", id);
                NewsDetail newsDetails = null;
                newsDetailsJson = memcacheManager.GetFromCache<string>(key);

                if (string.IsNullOrEmpty(newsDetailsJson))
                {
                    newsDetails = new NewsDetail(id);
                    newsDetailsJson = JsonConvert.SerializeObject(newsDetails);
                    memcacheManager.StoreToCache<string>(key, CacheRefreshTime.DefaultRefreshTime(), newsDetailsJson);
                }
                else
                    QueueUpdateView(id);
            }
            catch (Exception)
            {
                throw;
            }
            return newsDetailsJson;
        }

        private static void QueueUpdateView(string basicId)
        {
            RabbitMqPublish ra = new RabbitMqPublish();
            NameValueCollection nvc = new NameValueCollection();
            nvc.Add("basicId", basicId.ToString());
            ra.PublishToQueue(CWConfiguration.ArticleViewQueue, nvc);
        }
    }
}