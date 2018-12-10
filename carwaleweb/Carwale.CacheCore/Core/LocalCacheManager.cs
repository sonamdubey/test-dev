using AEPLCore.Cache;
using AEPLCore.Cache.Interfaces;
using Carwale.Interfaces;
using Carwale.Notifications;
using System;
using System.Collections.Generic;
using System.Web;

namespace Carwale.Cache.Core
{
    public class LocalCacheManager : ICacheManager
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
                var objErr = new ExceptionHandler(ex, "Carwale.MemcacheCore.LocalCacheManager.GetFromCache<T>()");
                objErr.LogException();
            }

            return t;
        }

        public bool ExpireCache(string key)
        {
            try
            {
                lock (this) // This lock is applied so that only one cache is created at the expiration of the cache and a new request is fired
                {
                    //HttpContext.Current.Cache.Insert(key, callback(), null, System.Web.Caching.Cache.NoAbsoluteExpiration, cacheDuration);
                    HttpContext.Current.Cache.Remove(key);
                }
				return true;
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "Carwale.MemcacheCore.LocalCacheManager.RefreshCache()");
                objErr.LogException();
            }
			return false;
        }


        public T GetFromCache<T>(string key, TimeSpan cacheDuration, Func<T> dbCallback, out bool isKeyNewlyCreated)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 30 Sept 2014
        /// Summary : Function to the cache object for a given key. Only data is retrieved not saved.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        public T GetFromCache<T>(string key)
        {
            T t = default(T);

            try
            {
                HttpContext.Current.Trace.Warn("Read from cache");
                t = (T)HttpContext.Current.Cache[key];
            }
            catch (Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "Carwale.MemcacheCore.LocalCacheManager.RefreshCache() with key only.");
                objErr.LogException();
            }

            return t;
        }

        public void MultiRefreshCache(List<string> keyList)
        {
            throw new NotImplementedException();
        }

        public T GetKey<T>(string key)
        {
            throw new NotImplementedException();
        }

        public bool StoreToCache<T>(string key, TimeSpan cacheDuration, T t)
        {
            throw new NotImplementedException();
        }

        public Dictionary<string, T> MultiGetFromCache<T>(System.Collections.Generic.IDictionary<string, MultiGetCallback<T>> callBacks)
        {
            throw new NotImplementedException();
        }


        public Dictionary<string, T> MultiGetFromCache<T>(System.Collections.Generic.IEnumerable<string> keys)
        {
            throw new NotImplementedException();
        }


        public bool Update(string key, object newValue, TimeSpan time)
        {
            throw new NotImplementedException();
        }


        public void ReplaceToCache<T>(string key, TimeSpan cacheDuration, T t)
        {
            throw new NotImplementedException();
        }

        public T GetFromCache<T>(string key, TimeSpan cacheDuration, Func<T> dbCallback, Func<T> dbCriticalCallback)
        {
            throw new NotImplementedException();
        }


        public void RefreshCacheWithoutDelay(string key)
        {
            throw new NotImplementedException();
        }
		public string GenerateCacheKey(string cacheKeyPrefix, string[] cacheKeySuffixArr)
		{
			throw new NotImplementedException();
		}

		public T GetFromCache<T>(string key, Func<T> dbCallback, TimeSpan cacheDuration)
		{
			throw new NotImplementedException();
		}

		public bool StoreToCache<T>(string key, T t, TimeSpan cacheDuration)
		{
			throw new NotImplementedException();
		}

		public void ExpireCacheWithoutDelay(string key)
		{
			throw new NotImplementedException();
		}

		public void ExpireMultipleCache(List<string> keyList)
		{
			throw new NotImplementedException();
		}

		public bool ExpireCacheWithCriticalRead(string key)
		{
			throw new NotImplementedException();
		}

		public void ExpireMultipleCacheWithoutDelay(List<string> keyList)
		{
			throw new NotImplementedException();
		}
	}
}
