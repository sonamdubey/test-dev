using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using System.Configuration;
using System.Data;
using Bikewale.Common;

namespace Bikewale.Memcache
{
/// <summary>
/// Summary description for MakeMapping
/// Written By : Ashwini Todkar
/// Purpose    : This class has logic of make mapping 
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
                     HttpContext.Current.Trace.Warn("make mapping count : ", ht.Count.ToString());
                     makeId = Convert.ToString(ht[mappingName]);
                     HttpContext.Current.Trace.Warn("mapped make id : ", makeId);
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                return makeId;
            }
	 }
}
