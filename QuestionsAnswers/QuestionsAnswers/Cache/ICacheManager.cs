using System;
using System.Collections.Generic;

namespace QuestionsAnswers.Cache
{
    public interface ICacheManager
    {
        T GetFromCache<T>(string key, TimeSpan cacheDuration, Func<T> dbCallback);
        T GetFromCache<T>(string key, TimeSpan cacheDuration, Func<T> dbCallback, out bool isDataFromCache);
        void RefreshCache(string key);
        IEnumerable<T> GetListFromCache<T>(IDictionary<string, string> keyValuePair, TimeSpan cacheDuration, Func<string, IEnumerable<T>> doCallback);

        /// <summary>
        /// Created By : Ashish G. Kamble on 25 Apr 2018
        /// Summary : Function to set cache duration from DAL itself
        /// </summary>
        /// <typeparam name="T">Output type for callback function</typeparam>
        /// <param name="key">Memcache key name</param>
        /// <param name="dbCallback">Callback function to call incase of memcache miss</param>
        /// <returns>Returns specified entity of type T. In case of data from cache or db not available returns -1</returns>
        T GetFromCache<T>(string key, Func<Tuple<T, TimeSpan>> dbCallback);
    }
}
