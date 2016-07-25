using Bikewale.Interfaces.Cache.Core;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using System;
using System.Configuration;

namespace Bikewale.Cache.Core
{
    public class MemcacheManager : ICacheManager
    {
        private static MemcachedClient mc = null;
        private bool _useMemcached;

        public MemcacheManager()
        {
            bool.TryParse(ConfigurationManager.AppSettings["IsMemcachedUsed"].ToLower(), out _useMemcached);
            LogManager.AssignFactory(new MemcacheLogFactory());
            if (mc == null && _useMemcached)
            {
                mc = new MemcachedClient("memcached");
            }
        }

        public T GetFromCache<T>(string key, TimeSpan cacheDuration, Func<T> dbCallback)
        {
            T t = default(T);

            try
            {
                if (_useMemcached) //  Check if memcache need to hit or not based on key in config file
                {
                    t = (T)mc.Get(key);
                    if (t == null) //Cache Miss
                    {
                        if (mc.Store(StoreMode.Add, key + "_lock", "lock", DateTime.Now.AddSeconds(60)))
                        {
                            t = dbCallback();

                            if (t != null)
                            {
                                mc.Store(StoreMode.Add, key, t, DateTime.Now.Add(cacheDuration));
                            }

                            mc.Remove(key + "_lock");
                        }
                        else
                        {
                            t = dbCallback();
                        }

                        return t;
                    }
                    else
                        return t;
                }
                else
                {
                    t = dbCallback();
                }
            }
            catch (Exception ex)
            {
                //ErrorClass objErr = new ErrorClass(ex, "MemcacheManager.GetFromCache");
                //objErr.SendMail();
            }
            finally
            {
                if (t == null)
                {
                    t = dbCallback();
                }
            }

            return t;
        }

        public T GetFromCache<T>(string key, TimeSpan cacheDuration, Func<T> dbCallback, out bool isDataFromCache)
        {
            T t = default(T);
            isDataFromCache = false;
            try
            {
                if (_useMemcached) //  Check if memcache need to hit or not based on key in config file
                {
                    t = (T)mc.Get(key);
                    if (t == null) //Cache Miss
                    {
                        if (mc.Store(StoreMode.Add, key + "_lock", "lock", DateTime.Now.AddSeconds(60)))
                        {
                            t = dbCallback();

                            if (t != null)
                            {
                                mc.Store(StoreMode.Add, key, t, DateTime.Now.Add(cacheDuration));
                            }

                            mc.Remove(key + "_lock");
                        }
                        else
                        {
                            t = dbCallback();
                        }

                        return t;
                    }
                    else
                    {
                        isDataFromCache = true;
                        return t;
                    }
                }
                else
                {
                    t = dbCallback();
                }
            }
            catch (Exception ex)
            {
                //ErrorClass objErr = new ErrorClass(ex, "MemcacheManager.GetFromCache");
                //objErr.SendMail();
            }
            finally
            {
                if (t == null)
                {
                    t = dbCallback();
                }
            }

            return t;
        }

        public void RefreshCache(string key)
        {
            mc.Remove(key);
        }
    }
}
