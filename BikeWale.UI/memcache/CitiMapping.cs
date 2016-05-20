using Bikewale.Common;
using System;
using System.Collections;
using System.Web;

/// <summary>
/// Written By : Ashwini Todkar on 2nd Dec 2013
/// Summary description for CitiMapping
/// Class contains method which returns city mapping name 
/// </summary>

namespace Bikewale.Memcache
{
    public static class CitiMapping
    {
        /// <summary>
        /// Written By : Ashwini Todkar on 2nd Dec 2013
        /// Updated By; Sangram Nandkhile on 20 May 2016
        /// Summary: Changed return type to Uint
        /// </summary>
        /// <param name="mappingName"></param>
        /// <returns>city id of city for url rewritting</returns>
        public static uint GetCityId(string mappingName)
        {
            uint cityId = 0;
            try
            {
                BWMemcache objM = new BWMemcache();
                Hashtable ht = objM.GetHashTable("BW_CityMapping");
                cityId = Convert.ToUInt32(ht[mappingName]);
            }
            catch (Exception ex)
            {
                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            return cityId;
        }

    }
}
