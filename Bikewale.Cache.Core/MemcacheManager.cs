using Bikewale.Interfaces.Cache.Core;
using Bikewale.Notifications;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

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

        private IList<T> GetFromDb<T>(IDictionary<string, string> keyValuePair, TimeSpan cacheDuration, Func<string, IEnumerable<T>> doCallback)
        {
            IList<T> list = null;
            try
            {
                var dataList = doCallback(String.Join(",", keyValuePair.Keys.ToArray()));

                if (dataList != null && dataList.Any())
                {
                    list = dataList.ToList();
                    if (_useMemcached && list.Count > 0 && keyValuePair.Count == list.Count)
                    {
                        int i = 0;
                        foreach (var item in keyValuePair)
                        {
                            if (mc.Store(StoreMode.Add, item.Value + "_lock", "lock", DateTime.Now.AddSeconds(60)))
                            {
                                var data = list[i++];
                                if (data != null)
                                {
                                    mc.Store(StoreMode.Add, item.Value, data, DateTime.Now.Add(cacheDuration));
                                }
                                mc.Remove(item.Value + "_lock");
                            }
                        }
                    }
                }
            }
            catch (Exception) { }

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

        public IEnumerable<T> GetListFromCache<T>(IDictionary<string, string> keyValuePair, TimeSpan cacheDuration, Func<string, IEnumerable<T>> doCallback)
        {
            List<T> t = default(List<T>);
            IDictionary<string, string> kv = new Dictionary<string, string>();
            try
            {
                t = new List<T>();
                if (_useMemcached) //  Check if memcache need to hit or not based on key in config file
                {
                    foreach (var item in keyValuePair)
                    {
                        var val = (T)mc.Get(item.Value);
                        if (val == null) //Cache Miss
                        {
                            kv.Add(item);
                        }
                        else
                        {
                            t.Add(val);
                        }
                    }
                    if (kv != null && kv.Count > 0)
                    {
                        IList<T> list = GetFromDb(kv, cacheDuration, doCallback);
                        if (list != null && list.Count > 0)
                        {
                            t.AddRange(list);
                        }
                    }
                }
                else
                {
                    IList<T> list = GetFromDb(keyValuePair, cacheDuration, doCallback);
                    t = list != null ? list.ToList() : null;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "MemcacheManager.GetFromCache");
            }
            finally
            {
                if (t != null && t.Count == 0)
                {
                    IList<T> list = GetFromDb(keyValuePair, cacheDuration, doCallback);
                    t = list != null ? list.ToList() : null;
                }
            }
            return t;
        }
    }
}
