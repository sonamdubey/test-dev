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
    /// Created By : Ashish G. Kamble on 29 Oct 2013
    /// Summary : Class have business logic related to bike model mapping.
    /// Class have methods to interact with bikewale memcache.
    /// </summary>
    public class ModelMapping
    {
        public string GetModelId(string mappingName)
        {
            string modelId = string.Empty;

            try
            {
                BWMemcache objCache = new BWMemcache();

                Hashtable ht = objCache.GetHashTable("BW_ModelMapping");
                HttpContext.Current.Trace.Warn("model mapping count : ", ht.Count.ToString());
                //foreach (DictionaryEntry entry in ht)
                //{
                //    HttpContext.Current.Trace.Warn(entry.Key +  ":" + entry.Value);
                //}
                modelId = Convert.ToString(ht[mappingName]);
                HttpContext.Current.Trace.Warn("mapped model id : ", modelId);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return modelId;
        }   // End of GetModelId method

        public int GetTopVersionId(string mappingName)
        {
            int versionId = 0;
            try
            {
                BWMemcache objCache = new BWMemcache();

                Hashtable ht = objCache.GetHashTable("BW_TopVersionId");

                versionId = Convert.ToInt32(ht[mappingName]);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return versionId;
        }

    }   // class
}   // namespace