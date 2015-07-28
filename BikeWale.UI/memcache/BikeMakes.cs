using System;
using System.Web;
using System.Data;
using Bikewale.Common;

namespace Bikewale.Memcache
{
    /// <summary>
    /// Written By : Ashish G. Kamble on 31 Oct 2013
    /// Summary : Class have methods to get bike makes data from memcache.
    /// </summary>
    public class BikeMakes
    {
        /// <summary>
        /// Function to get new and used bike makes list
        /// </summary>
        /// <returns>dataset containing new and used bike makes list</returns>
        public DataSet GetNewBikeMakes()
        {
            DataSet ds = null;

            try
            {
                BWMemcache objCache = new BWMemcache();
                ds = objCache.GetDataSet("BW_BikeMakes");
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return ds;
        }   // End of GetNewBikeMakes

    }   // Class
}   // namespace