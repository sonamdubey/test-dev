using System;

namespace Bikewale.Interfaces.Cache.Core
{
    public interface ICacheManager
    {
        T GetFromCache<T>(string key, TimeSpan cacheDuration, Func<T> dbCallback);
        public T GetFromCache<T>(string key, TimeSpan cacheDuration, Func<T> dbCallback, out bool isDataFromCache);
        void RefreshCache(string key);
    }
}
