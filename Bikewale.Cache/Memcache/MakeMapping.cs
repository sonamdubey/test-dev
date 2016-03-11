using Bikewale.Notifications;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                ErrorClass objErr = new ErrorClass(ex, "GetMakeId");
                objErr.SendMail();
            }
            return makeId;
        }
    }
}
