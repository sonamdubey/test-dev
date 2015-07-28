using System;
using System.Collections;
using System.Collections.Generic;
using System.Web;
using Enyim.Caching;
using Enyim.Caching.Memcached;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Bikewale.Common;

/// <summary>
/// Written By : Ashwini Todkar on 2nd Dec 2013
/// Summary description for CitiMapping
/// Class contains method which returns city mapping name 
/// </summary>
/// 
namespace Bikewale.Memcache
{
    public static class CitiMapping
    {
        /// <summary>
        /// Written By : Ashwini Todkar on 2nd Dec 2013
        /// </summary>
        /// <param name="mappingName"></param>
        /// <returns>city id of city for url rewritting</returns>
        public static string GetCityId(string mappingName)
        {            
            string cityId = string.Empty;

            try
            {
                BWMemcache objM = new BWMemcache();
                Hashtable ht = objM.GetHashTable("BW_CityMapping");
                HttpContext.Current.Trace.Warn("city mapping count : ", ht.Count.ToString());

                cityId = Convert.ToString(ht[mappingName]);
                HttpContext.Current.Trace.Warn("mapped city id : ", cityId);

                //foreach (DictionaryEntry entry in ht)
                //{
                //    HttpContext.Current.Trace.Warn("mapping name :" + entry.Key.ToString() + "   id :" + entry.Value.ToString());
                //}
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            return cityId;         
        }
    }
}