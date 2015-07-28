using Bikewale.Common;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;

/// <summary>
/// Summary description for SeriesMapping
/// </summary>
namespace Bikewale.Memcache
{
    public class SeriesMapping
    {
        public static string GetSeriesId(string mappingName)
        {
            string seriesId = string.Empty;

            try
            {
                BWMemcache objCache = new BWMemcache();

                Hashtable ht = objCache.GetHashTable("BW_SeriesMapping");
                HttpContext.Current.Trace.Warn("Series mapping count : ", ht.Count.ToString());

                seriesId = Convert.ToString(ht[mappingName]);
                HttpContext.Current.Trace.Warn("mapped series id : ", seriesId);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return seriesId;
        }   // End of GetModelId method
    }   //End of Class
}   //End of namespace