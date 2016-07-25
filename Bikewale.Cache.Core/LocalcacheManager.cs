using Bikewale.Interfaces.Cache.Core;
using System;
using System.Web;

namespace Bikewale.Cache.Core
{
    public class LocalcacheManager : ICacheManager
    {
        public T GetFromCache<T>(string key, TimeSpan cacheDuration, Func<T> callback)
        {
            T t = default(T);

            try
            {
                //checking if the DataSet object exists in the Cache before attempting to use it
                if (HttpContext.Current.Cache[key] == null)
                {
                    lock (this) // This lock is applied so that only one cache is created at the expiration of the cache and a new request is fired
                    {
                        if (HttpContext.Current.Cache[key] == null)
                        {
                            t = callback();
                            HttpContext.Current.Cache.Insert(key, t, null, System.Web.Caching.Cache.NoAbsoluteExpiration, cacheDuration);
                        }
                        else // in order to avoid a second hit to the Database when the lock is released
                        {
                            // Reading from the cache
                            HttpContext.Current.Trace.Warn("Read from cache");
                            t = (T)HttpContext.Current.Cache[key];
                        }
                    }
                }
                else
                {
                    // Reading from the cache
                    HttpContext.Current.Trace.Warn("Read from cache");
                    t = (T)HttpContext.Current.Cache[key];
                }
            }
            catch (Exception ex)
            {
                //var objErr = new ExceptionHandler(ex, "Carwale.MemcacheCore.LocalCacheManager.GetFromCache<T>()");
                //objErr.LogException();
            }

            return t;
        }

        public void RefreshCache(string key)
        {
            try
            {
                lock (this) // This lock is applied so that only one cache is created at the expiration of the cache and a new request is fired
                {
                    //HttpContext.Current.Cache.Insert(key, callback(), null, System.Web.Caching.Cache.NoAbsoluteExpiration, cacheDuration);
                    HttpContext.Current.Cache.Remove(key);
                }
            }
            catch (Exception ex)
            {
                //var objErr = new ExceptionHandler(ex, "Carwale.MemcacheCore.LocalCacheManager.RefreshCache()");
                //objErr.LogException();
            }
        }


        public T GetFromCache<T>(string key, TimeSpan cacheDuration, Func<T> dbCallback, out bool isDataFromCache)
        {
            throw new NotImplementedException();
        }
    }
}
