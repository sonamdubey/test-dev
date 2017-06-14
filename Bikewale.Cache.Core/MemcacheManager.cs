using Bikewale.Interfaces.Cache.Core;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using System;
using System.Linq;
using System.Configuration;
using System.Collections.Generic;

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

        public IEnumerable<T> GetListFromCache<T>(string[] reviewIds, string[] keys, TimeSpan cacheDuration, Func<string,IEnumerable<T>> doCallback)
        {
            List<T> t = default(List<T>);
            List<T> miss = default(List<T>);
            ICollection<string> missKeys = new List<string>();
            try
            {
                t = new List<T>();
                if (_useMemcached) //  Check if memcache need to hit or not based on key in config file
                {
                    for (int i = 0; i < reviewIds.Count(); i++)
                    {
                        var val = (T)mc.Get(keys[i]);
                        if (val == null) //Cache Miss
                        {
                            missKeys.Add(reviewIds[i]);
                        }
                        else
                        {
                            t.Add(val);
                        }
                    }
                    if(missKeys!=null && missKeys.Count > 0)
                    {
                        IList<T> list = GetFromDb(keys, cacheDuration, doCallback,  missKeys);
                        if (list !=null && list.Count > 0)
                        {                            
                            t.AddRange(list); 
                        }
                    }
                }
                else
                {
                    missKeys = reviewIds;   
                    IList<T> list = GetFromDb(keys, cacheDuration, doCallback,  missKeys);
                    t = list != null ? list.ToList() : null;
                }
            }
            catch (Exception ex)
            {
                //ErrorClass objErr = new ErrorClass(ex, "MemcacheManager.GetFromCache");
                //objErr.SendMail();
            }
            finally
            {
                if (t.Count == 0)
                {
                    missKeys = reviewIds;   
                    IList<T> list = GetFromDb(keys, cacheDuration, doCallback,  missKeys);
                    t = list != null ? list.ToList() : null;
                }
            }

            return t;
        }

        private IList<T> GetFromDb<T>(string[] keys, TimeSpan cacheDuration, Func<string, IEnumerable<T>> doCallback, ICollection<string> missKeys)
        {
            IList<T> list = null;
            try
            {
                var dataList = doCallback(String.Join(",", missKeys.ToArray()));

                if (dataList!= null && dataList.Count() > 0)
                {
                    list = dataList.ToList();
                    if (_useMemcached)
                    {
                        for (int i = 0; i < missKeys.Count; i++)
                        {
                            if (mc.Store(StoreMode.Add, keys[i] + "_lock", "lock", DateTime.Now.AddSeconds(60)))
                            {
                                var data = list[i];
                                if (data != null)
                                {
                                    mc.Store(StoreMode.Add, keys[i], data, DateTime.Now.Add(cacheDuration));                                    
                                }
                                mc.Remove(keys[i] + "_lock");
                            }
                        } 
                    }
                }
            }
            catch (Exception){}

            return list;
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
