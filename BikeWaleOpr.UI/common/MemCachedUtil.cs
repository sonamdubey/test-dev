
using Enyim.Caching;
namespace BikewaleOpr.common
{

    /// <summary>
    /// Created by: Sangram Nandkhile on 19 Jul 2016
    /// Desc: Static object to hold reference for memcache and delete the data as and when needed
    /// </summary>
    public static class MemCachedUtility
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
            catch
            {
            }
        }
    }

}