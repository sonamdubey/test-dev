using System;
using System.Data;
using Bikewale.Common;
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
            string _cwBasicId = string.Empty;

            try
            {
                BWMemcache objCache = new BWMemcache();

                Hashtable ht = objCache.GetHashTable("BW_BasicIdMapping");
                //HttpContext.Current.Trace.Warn("basic  id count : ", ht.Count.ToString());

                //foreach (DictionaryEntry entry in ht)
                //{
                //    HttpContext.Current.Trace.Warn(entry.Key + ":" + entry.Value);
                //}
                if (ht[Convert.ToInt32(BWBasicId)] != null)
                    _cwBasicId = ht[Convert.ToInt32(BWBasicId)].ToString();
                //HttpContext.Current.Trace.Warn("mapped basic id : " + _cwBasicId);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return _cwBasicId;
        }   // End of GetModelId method
    }
}