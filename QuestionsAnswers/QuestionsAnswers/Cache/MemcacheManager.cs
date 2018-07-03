using Enyim.Caching;
using Enyim.Caching.Memcached;
using System;
using System.Collections.Generic;
using System.Linq;

namespace QuestionsAnswers.Cache
{
    public class MemcacheManager : ICacheManager
    {
        private static MemcachedClient mc;
        private readonly bool _useMemcached;
        private readonly int _memcacheDefaultObjDuration = 1;
        static readonly string _dummyData = "-1";
        public MemcacheManager()
        {
            if (mc == null)
            {
                mc = new MemcachedClient("memcached");
            }
        }

        private IList<T> GetFromDb<T>(IDictionary<string, string> keyValuePair, TimeSpan cacheDuration, Func<string, IEnumerable<T>> doCallback)
        {
            IList<T> list = null;
            try
            {
                IEnumerable<T> dataList = null;

                try
                {
                    dataList = doCallback(String.Join(",", keyValuePair.Keys.ToArray()));
                }
                catch (Exception)
                {
                    throw;
                }

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
                                else
                                {
                                    AddDummyData<T>(item.Value);
                                }
                                mc.Remove(item.Value + "_lock");
                            }
                        }
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }

            return list;
        }

        private T AddDummyData<T>(string key)
        {
            mc.Store(StoreMode.Add, key, _dummyData, DateTime.Now.AddMinutes(_memcacheDefaultObjDuration));
            return default(T);
        }


        public T GetFromCache<T>(string key, TimeSpan cacheDuration, Func<T> dbCallback)
        {
            object cacheObject;
            try
            {
                if (_useMemcached) //  Check if memcache need to hit or not based on key in config file
                {
                    if (!mc.TryGet(key, out cacheObject)) //Cache Miss
                    {
                        if (mc.Store(StoreMode.Add, key + "_lock", "lock", DateTime.Now.AddSeconds(60)))
                        {
                            T t = default(T);
                            try
                            {
                                t = dbCallback();
                                if (t != null)
                                {
                                    mc.Store(StoreMode.Add, key, t, DateTime.Now.Add(cacheDuration));
                                }
                                else
                                {
                                    t = AddDummyData<T>(key);
                                }
                            }
                            catch (Exception)
                            {
                                t = AddDummyData<T>(key);
                            }

                            mc.Remove(key + "_lock");
                            return t;
                        }
                        else
                        {
                            return dbCallback();
                        }
                    }
                    else
                    {
                        if (cacheObject is string && cacheObject.Equals(_dummyData))
                        {
                            return default(T);
                        }

                        return (T)cacheObject;
                    }
                }
                else
                {
                    return dbCallback();
                }
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        /// <summary>
        /// Created By : Ashish G. Kamble on 25 Apr 2018
        /// Summary : Function to set cache duration from DAL itself
        /// </summary>
        /// <typeparam name="T">Output type for callback function</typeparam>
        /// <param name="key">Memcache key name</param>
        /// <param name="dbCallback">Callback function to call incase of memcache miss</param>
        /// <returns>Returns specified entity of type T. In case of data from cache or db not available returns -1</returns>
        public T GetFromCache<T>(string key, Func<Tuple<T, TimeSpan>> dbCallback)
        {
            object cacheObject;
            Tuple<T, TimeSpan> t;

            try
            {
                if (_useMemcached) //  Check if memcache need to hit or not based on key in config file
                {
                    if (!mc.TryGet(key, out cacheObject)) //Cache Miss
                    {
                        if (mc.Store(StoreMode.Add, key + "_lock", "lock", DateTime.Now.AddSeconds(60)))
                        {
                            try
                            {
                                t = dbCallback();

                                if (t != null && t.Item1 != null)
                                {
                                    // Verify change                                      
                                    mc.Store(StoreMode.Add, key, t.Item1, DateTime.Now.Add(t.Item2));
                                }
                                else
                                {
                                    t = Tuple.Create(AddDummyData<T>(key), new TimeSpan());
                                }
                            }
                            catch (Exception)
                            {
                                t = Tuple.Create(AddDummyData<T>(key), new TimeSpan());
                            }

                            mc.Remove(key + "_lock");
                            return t.Item1;
                        }
                        else
                        {
                            t = dbCallback();
                            return t.Item1;
                        }
                    }
                    else
                    {
                        if (cacheObject is string && cacheObject.Equals(_dummyData))
                        {
                            return default(T);
                        }
                        return (T)cacheObject;
                    }
                }
                else
                {
                    t = dbCallback();
                    return t.Item1;
                }
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public T GetFromCache<T>(string key, TimeSpan cacheDuration, Func<T> dbCallback, out bool isDataFromCache)
        {
            isDataFromCache = false;
            object cacheObject;
            try
            {
                if (_useMemcached) //  Check if memcache need to hit or not based on key in config file
                {
                    if (!mc.TryGet(key, out cacheObject)) //Cache Miss
                    {
                        if (mc.Store(StoreMode.Add, key + "_lock", "lock", DateTime.Now.AddSeconds(60)))
                        {

                            T t = default(T);
                            try
                            {
                                t = dbCallback();
                                if (t != null)
                                {
                                    mc.Store(StoreMode.Add, key, t, DateTime.Now.Add(cacheDuration));
                                }
                                else
                                {
                                    t = AddDummyData<T>(key);
                                }
                            }
                            catch (Exception)
                            {
                                t = AddDummyData<T>(key);
                            }

                            mc.Remove(key + "_lock");
                            return t;
                        }
                        else
                        {
                            return dbCallback();
                        }

                    }
                    else
                    {
                        isDataFromCache = true;
                        if (cacheObject is string && cacheObject.Equals(_dummyData))
                        {
                            return default(T);
                        }

                        return (T)cacheObject;
                    }
                }
                else
                {
                    return dbCallback();
                }
            }
            catch (Exception)
            {
                return default(T);
            }
        }

        public void RefreshCache(string key)
        {
            if (mc != null)
            {
                mc.Remove(key);
            }
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
            catch (Exception)
            {
                throw;
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
