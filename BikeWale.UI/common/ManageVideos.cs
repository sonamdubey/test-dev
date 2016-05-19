
using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using Bikewale.Common;
using System.Data.SqlClient;

namespace Bikewale.Common
{
    public class ManageVideos
    {
        /// Written By : Ashish G. Kamble on 6 Aug 2013
        /// Summary : Function to get the videos data as per requirement from database.
        /// </summary>
        /// <param name="makeId">Make Id of the bike. Its Optional.</param>
        /// <param name="modelId">Model Id of the bike. Its Optional.</param>
        /// <param name="isActive">Get active or inactive videos. Manadatory</param>
        /// <returns>Function returns data table containing videos data. If data is not available return null.</returns>
        public DataSet GetVideosData(string makeId, string modelId, bool isActive)
        {
            throw new Exception("Method not used/commented");

            //DataSet ds = null;
            //Database db = null;

            //try
            //{
            //    db = new Database();

            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.CommandText = "GetVideos";
            //        HttpContext.Current.Trace.Warn("GetVideosData : makeId : " + makeId + " : modelId : " + modelId + " : isActive : " + isActive.ToString());
            //        //Trace.Warn("GetVideosData : makeId : " + makeId + " : modelId : " + modelId + " : isActive : " + isActive.ToString());
            //        cmd.Parameters.Add("@makeId", SqlDbType.Int).Value = String.IsNullOrEmpty(makeId) ? Convert.DBNull : makeId;
            //        cmd.Parameters.Add("@modelId", SqlDbType.Int).Value = String.IsNullOrEmpty(modelId) ? Convert.DBNull : modelId;
            //        cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = isActive;

            //        ds = db.SelectAdaptQry(cmd);
            //    }
            //}
            //catch (SqlException ex)
            //{
            //    HttpContext.Current.Trace.Warn("sql ex : ", ex.Message);
            //    ErrorClass err = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    err.SendMail();
            //}
            //catch (Exception ex)
            //{
            //    HttpContext.Current.Trace.Warn("ex : ", ex.Message);
            //    ErrorClass err = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    err.SendMail();
            //}

            //return ds;

        }   // End of GetVideosData
    }//class
}//namespace