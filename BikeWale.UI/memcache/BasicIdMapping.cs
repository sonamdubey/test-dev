using Bikewale.Common;
using System;
using System.Collections;
using System.Web;

namespace Bikewale.Memcache
{
    /// <summary>
    /// Written By : Ashwini Todkar on 12 Aug 2014
    /// Summary    : Class has methods to get new basic ids of an article for old basic ids 
    /// </summary>
    public class BasicIdMapping
    {

        public static string GetCWBasicId(string BWBasicId)
        {
            string _cwBasicId = BWBasicId;

            try
            {
                BWMemcache objCache = new BWMemcache();

                Hashtable ht = objCache.GetHashTable("BW_BasicIdMapping");
                if (ht[Convert.ToInt32(BWBasicId)] != null)
                    _cwBasicId = ht[Convert.ToInt32(BWBasicId)].ToString();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return _cwBasicId;
        }   // End of GetModelId method
    }
}