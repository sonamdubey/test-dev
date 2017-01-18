﻿using Bikewale.Notifications;
using Enyim.Caching;
using System;

namespace BikewaleOpr.Cache
{
    /// <summary>
    /// Created by: Sajal Gupta on 09-01-2017
    /// Desc: Static object to hold reference for memcache and delete the data as and when needed
    /// </summary>
    public static class MemCachedUtil
    {
        static MemcachedClient _mc = null;

        static MemCachedUtil()
        {
            try
            {
                if (_mc == null)
                {
                    _mc = new MemcachedClient("memcached");
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Cache.MemCachedUtil.MemCachedUtil()");
            }
        }

        public static void Remove(string key)
        {
            try
            {
                _mc.Remove(key);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Cache.MemCachedUtil.Remove");
            }
        }
    }
}
