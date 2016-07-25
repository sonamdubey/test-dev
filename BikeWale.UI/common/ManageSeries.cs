using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Configuration;

/// <summary>
/// Created By : Sadhana Upadhyay on 4th March 2014
/// Summary : class for manage bike series
/// </summary>
namespace Bikewale.Common
{
    public class ManageSeries
    {
        /// <summary>
        /// Created By : Sadhana Upadhyay on 4th March 2014
        /// Summary : To get bike series details
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public DataSet GetModelSeries(string seriesId)
        {
            throw new Exception("Method not used/commented");

            //DataSet ds = null;
            //try
            //{
            //    Database db = new Database();
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        cmd.CommandText = "GetBikeModelSeries";
            //        cmd.CommandType = CommandType.StoredProcedure;

            //        cmd.Parameters.Add("@SeriesId", SqlDbType.Int).Value = seriesId;
            //        cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = ConfigurationManager.AppSettings["DefaultCity"];
            //        ds = db.SelectAdaptQry(cmd);
            //    }
            //}
            //catch (SqlException err)
            //{
            //    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //catch (Exception err)
            //{
            //    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //return ds;
        }   //End of GetModelSeries

        /// <summary>
        /// Created By : Sadhana Upadhyay on 4th March 2014
        /// Summary : To get bike series Synopsis
        /// </summary>
        /// <param name="seriesId"></param>
        /// <param name="seriesDesc"></param>
        public void GetSeriesSynopsis(string seriesId, ref string seriesDesc , ref int makeId)
        {
            throw new Exception("Method not used/commented");

            //try
            //{ 
            //    Database db = new Database();

            //    using (SqlConnection con = new SqlConnection(db.GetConString()))
            //    {
            //        using (SqlCommand cmd = new SqlCommand())
            //        {
            //            HttpContext.Current.Trace.Warn("series id = "+seriesId);
            //            cmd.CommandText = "GetSeriesSynopsis";
            //            cmd.CommandType = CommandType.StoredProcedure;
            //            cmd.Connection = con;

            //            cmd.Parameters.Add("@SeriesId", SqlDbType.Int).Value = seriesId;
            //            cmd.Parameters.Add("@Synopsis", SqlDbType.VarChar, -1).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@Series", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@MakeName", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
            //            cmd.Parameters.Add("@MakeId", SqlDbType.Int).Direction = ParameterDirection.Output;
            //            con.Open();
            //            cmd.ExecuteNonQuery();
            //            seriesDesc = cmd.Parameters["@Synopsis"].Value.ToString();
            //            makeId = Convert.ToInt32(cmd.Parameters["@MakeId"].Value);
            //        }
            //    }
            //}
            //catch (SqlException err)
            //{
            //    HttpContext.Current.Trace.Warn("error : "+ err);
            //    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //catch (Exception err)
            //{
            //    HttpContext.Current.Trace.Warn("error : " + err);
            //    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
        }   //End of GetSeriesSynopsis

        /// <summary>
        /// Wirtten By : Ashwini Todkar on 4 March 2014
        /// summary    : function to get series and their respective models of a particular make.
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns>Return dataset containing series and models data</returns>
        public DataSet ShowSeriesModels(string makeId)
        {
            throw new Exception("Method not used/commented");

            //Database db = null;
            //DataSet ds = null;

            //try
            //{
            //    using (SqlCommand cmd = new SqlCommand())
            //    {
            //        cmd.CommandText = "GetSeriesModels";
            //        cmd.CommandType = CommandType.StoredProcedure;

            //        cmd.Parameters.Add("@MakeId", SqlDbType.VarChar, 10).Value = makeId;
            //        cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = ConfigurationManager.AppSettings["DefaultCity"];

            //        db = new Database();

            //        ds = db.SelectAdaptQry(cmd);
            //    }
            //}
            //catch (SqlException err)
            //{
            //    HttpContext.Current.Trace.Warn("SQL Exception in ShowSeriesModels");
            //    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //catch (Exception err)
            //{
            //    HttpContext.Current.Trace.Warn("Exception in ShowSeriesModels");
            //    ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //finally
            //{
            //    db.CloseConnection();
            //}

            //return ds;
        }        

    }   //End of Class
}   //End of namespace