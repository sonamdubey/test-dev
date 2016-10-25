using Bikewale.Notifications;
using Enyim.Caching;
using System;

namespace BikewaleOpr.BAL
{

    /// <summary>
    /// Created by: Sangram Nandkhile on 19 Jul 2016
    /// Desc: Static object to hold reference for memcache and delete the data as and when needed
    /// </summary>
    public static class MemCachedUtil
    {
        static MemcachedClient _mc = null;
        public static void Remove(string key)
        {
            try
            {
                if (_mc == null)
                {
                    _mc = new MemcachedClient("memcached");
                }
                _mc.Remove(key);
            }
            catch(Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.BAL.MemCachedUtil");
                objErr.SendMail();
            }
        }
    }
}
