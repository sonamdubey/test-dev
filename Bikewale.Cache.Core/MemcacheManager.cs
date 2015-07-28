﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Web;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using Bikewale.Interfaces.Cache.Core;

namespace Bikewale.Cache.Core
{
    public class MemcacheManager : ICacheManager
    {
        private static MemcachedClient mc = null;
        private bool _useMemcached;

        public MemcacheManager()
        {
            _useMemcached = ConfigurationManager.AppSettings["IsMemcachedUsed"].ToLower() == "true" ? true : false;

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

                            mc.Store(StoreMode.Add, key, t, DateTime.Now.Add(cacheDuration));

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
                //var objErr = new ExceptionHandler(ex, "Carwale.MemcacheCore.MemcacheManager.GetFromCache<T>()");
                //objErr.LogException();
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
