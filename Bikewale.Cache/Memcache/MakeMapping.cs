using Bikewale.Notifications;
using System;
using System.Collections;

namespace Bikewale.Cache.Memcache
{
    /// <summary>
    /// Created By : Sumit Kate
    /// Created on : 10 march 2016
    /// Description : Bikewale.UI.Memcache is used.
    /// </summary>
    public static class MakeMapping
    {
        public static string GetMakeId(string mappingName)
        {
            string makeId = string.Empty;
            try
            {
                BWMemcache objCache = new BWMemcache();
                Hashtable ht = objCache.GetHashTable("BW_MakeMapping");
                makeId = Convert.ToString(ht[mappingName]);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "GetMakeId");
                
            }
            return makeId;
        }
    }
}
