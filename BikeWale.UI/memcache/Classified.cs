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
/// Created By : Ashwini Todkar on 11 march 2014
/// </summary>
namespace Bikewale.Memcache
{
    public static class Classified
    {
        /// <summary>
        /// Written By : Ashwini Todkar on 11 march 2014
        /// summary    : method to get used listed bike count of a model 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>number of listed bike for a model </returns>
        public static string GetModelWiseUsedBikeCount(string modelId)
        {
            string bikeListingCount = string.Empty;

            try
            {
                BWMemcache objM = new BWMemcache();
                Hashtable ht = objM.GetHashTable("BW_ModelWiseUsedBikesCount");
      
                bikeListingCount = Convert.ToString(ht[decimal.Parse(modelId)]);
                HttpContext.Current.Trace.Warn("model live listing count : ", bikeListingCount);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            return bikeListingCount;
        }//End of 


        /// <summary>
        /// Written By : Ashwini Todkar on 11 march 2014
        /// summary    : method to get used listed bike count of a make 
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        public static string GetMakeWiseUsedBikeCount(string makeId)
        {
            string bikeListingCount = string.Empty;

            try
            {
                BWMemcache objM = new BWMemcache();
                Hashtable ht = objM.GetHashTable("BW_MakeWiseUsedBikesCount");

                bikeListingCount = Convert.ToString(ht[decimal.Parse(makeId)]);
                HttpContext.Current.Trace.Warn("model live listing count : ", bikeListingCount);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass errObj = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                errObj.SendMail();
            }
            return bikeListingCount;
        }
    }
}