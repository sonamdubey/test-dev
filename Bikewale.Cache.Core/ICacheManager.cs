using System;
using System.Collections.Generic;

namespace Bikewale.Interfaces.Cache.Core
{
    public interface ICacheManager
    {
        T GetFromCache<T>(string key, TimeSpan cacheDuration, Func<T> dbCallback);
        T GetFromCache<T>(string key, TimeSpan cacheDuration, Func<T> dbCallback, out bool isDataFromCache);
        void RefreshCache(string key);
        IEnumerable<T> GetListFromCache<T>(string[] reviewIds, string[] keys, TimeSpan cacheDuration, Func<string, IEnumerable<T>> doCallback);
    }
}
