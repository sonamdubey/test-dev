using Bikewale.Interfaces.Cache.Core;
using System;
using System.Collections.Generic;
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
                object lockObj = new object();
                //checking if the DataSet object exists in the Cache before attempting to use it
                if (HttpContext.Current.Cache[key] == null)
                {
                    lock (lockObj) // This lock is applied so that only one cache is created at the expiration of the cache and a new request is fired
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
                object lockObj = new object();
                lock (lockObj) // This lock is applied so that only one cache is created at the expiration of the cache and a new request is fired
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

        public IEnumerable<T> GetListFromCache<T>(IDictionary<string, string> keyValuePair, TimeSpan cacheDuration, Func<string, IEnumerable<T>> doCallback)
        {
            throw new NotImplementedException();
        }


        public T GetFromCache<T>(string key, Func<Tuple<T, TimeSpan>> dbCallback)
        {
            throw new NotImplementedException();
        }
    }
}
