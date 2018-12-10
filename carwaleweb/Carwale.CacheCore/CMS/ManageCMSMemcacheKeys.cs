using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.CMS;
using Carwale.Interfaces;
using AEPLCore.Cache;
using Carwale.Notifications;
using Carwale.Interfaces.CMS;
using AEPLCore.Cache.Interfaces;

namespace Carwale.Cache.CMS
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 24 Sept 2014
    /// Summary : Class to store keys for editCMS into memcache.
    /// These keys can be used when articles are published, unpublished or edited to refresh memcache object.
    /// </summary>
    public class ManageCMSMemcacheKeys : ICMSMemcacheKeysRepository
    {
        /// <summary>
        /// Written By : Ashish G. Kamble on 24 Sept 2014
        /// </summary>
        /// <param name="key"></param>
        /// <param name="applicationId"></param>
        /// <param name="categoryIdList">categoryIdList should be "," separated string.</param>
        public void AddKeyToMemcache(string key, ushort applicationId, string categoryIdList)
        {
            string cmsCacheKey = "CMSCacheKeys";
            bool isKeyFirstTimeCreated = false;

            try
            {
                var catList = categoryIdList.Split(',').Select(ushort.Parse).ToList().OrderBy(i => i).ToList();

                foreach (var categoryId in catList)
                {
                    ICacheManager _memCacheProvider = new CacheManager();

                    //Add new key to memcache
                    List<CMSCacheKey> objKeysList = _memCacheProvider.GetFromCache<List<CMSCacheKey>>(cmsCacheKey, new TimeSpan(730, 0, 0, 0),
                                                        () => ManageCMSKeys(key, applicationId, categoryId), out isKeyFirstTimeCreated);

                    // If CMSCacheKeys object already exists just add new key into the object
                    if (!isKeyFirstTimeCreated)
                    {
                        //var tempList = objKeysList.Exists(k => k.ApplicationId == applicationId && k.CategoryId == categoryId && k.Key == key);

                        objKeysList.Add(new CMSCacheKey { ApplicationId = applicationId, Key = key, CategoryId = categoryId });

                        // Remove old CMSCacheKeys object from memcache
                        _memCacheProvider.ExpireCache(cmsCacheKey);

                        // Add new object to Memcache
                        _memCacheProvider.GetFromCache<List<CMSCacheKey>>(cmsCacheKey, new TimeSpan(730, 0, 0, 0), () => objKeysList);
                    }
                }
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "ManageCMSMemcacheKeys.AddKeyToMemcache Exception");
                objErr.LogException();
            }
        }   // End of AddKeyToMemcache

        /// <summary>
        /// Written By : Ashish G. Kamble on 24 Sept 2014
        /// Summary : Function to create the new list object for CMS cache keys if not exists in the memcache.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="applicationId"></param>
        /// <param name="categoryId"></param>
        /// <returns></returns>
        private List<CMSCacheKey> ManageCMSKeys(string key, ushort applicationId, ushort categoryId)
        {
            List<CMSCacheKey> objKeysList = new List<CMSCacheKey>();

            objKeysList.Add(new CMSCacheKey()
            {
                ApplicationId = applicationId,
                CategoryId = categoryId,
                Key = key
            });

            return objKeysList;
        }   // End of ManageCMSKeys

    }   // End of class
}   // End of namespace
