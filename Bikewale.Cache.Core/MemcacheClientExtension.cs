using Bikewale.Notifications;
using Enyim.Caching;
using System;

namespace Bikewale.Cache.Core
{
    /// <summary>
    /// Created by  :   Sumit Kate on 16 Mar 2018
    /// Description :   Extension method to bulk key removal from Memcache
    /// </summary>
    public static class MemcachedClientExn
    {
        public static void Remove(this MemcachedClient mc, System.Collections.Generic.IEnumerable<String> keys)
        {
            try
            {
                foreach (var key in keys)
                {
                    mc.Remove(key);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Cache.Core.MemcachedClientExn.Remove");
            }
        }
    }
}
