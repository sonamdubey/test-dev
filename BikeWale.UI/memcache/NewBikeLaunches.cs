using System;
using System.Web;
using System.Data;
using Bikewale.Common;
using System.Data.SqlClient;


/// <summary>
/// Written By  : Ashwini Todkar
/// Purpose : This class has method to retrieve new bike launches details from memcache
/// </summary>

namespace Bikewale.Memcache
{
    public class NewBikeLaunches
    {
        /// <summary>
        /// Written By : ashwini todkar
        /// Summary    : PopulateWhere to retrieve new bike launches details
        /// </summary>
        /// <param name="TopCount">Optional parameter.</param>
        /// <returns></returns>
        public DataSet GetNewBikeLaunches(string TopCount)
        {
            DataSet ds = null;

            try
            {
                if (String.IsNullOrEmpty(TopCount))
                    TopCount = "3";

                BWMemcache objCache = new BWMemcache();

                SqlParameter param = null;
                param = new SqlParameter("par_topcount", SqlDbType.Int);
                param.Value = TopCount;

                ds = objCache.GetDataSet("BW_NewBikeLaunches", param);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            return ds;
        }   // End of GetNewBikeLaunches
    }
}//End of memcache