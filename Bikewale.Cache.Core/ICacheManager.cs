using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Interfaces.Cache.Core
{
    public interface ICacheManager
    {
        T GetFromCache<T>(string key, TimeSpan cacheDuration, Func<T> dbCallback);
        void RefreshCache(string key);
    }
}
