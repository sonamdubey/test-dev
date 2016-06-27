﻿using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Bikewale.Entities.BikeData;

namespace Bikewale.Common
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 3/8/2012
    /// Common class for getting details of states and cities
    /// </summary>
    public class StateCity
    {
        /// <summary>
        /// Written By : Ashish G. Kamble on 3/8/2012
        /// Getting States id and states names
        /// </summary>
        /// <returns></returns>
        public DataTable GetStates()
        {
            Database db = null;
            DataTable dt = null;

            using (SqlCommand cmd = new SqlCommand("GetStates"))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    db = new Database();
                    dt = db.SelectAdaptQry(cmd).Tables[0];
                }
                catch (SqlException ex)
                {
                    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
            }
            return dt;
        }   // End of GetStates method

        /// <summary>
        /// Written By : Ashish G. Kamble on 3/8/2012
        /// Function returns city id and city names by providing state id and request type
        /// </summary>
        /// <param name="stateId"></param>
        /// <param name="RequestType">Pass value All or PriceQuote</param>
        /// <returns></returns>
        public DataTable GetCities(string requestType)
        {
            Database db = null;
            DataTable dt = null;

            using (SqlCommand cmd = new SqlCommand("GetCities"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = requestType;                

                try
                {
                    db = new Database();
                    dt = db.SelectAdaptQry(cmd).Tables[0];
                }
                catch (SqlException ex)
                {
                    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
            }
            return dt;
        }   // End of GetCities method

        /// <summary>
        /// Written By : Ashish G. Kamble on 3/8/2012
        /// Function returns city id and city names by providing state id and request type
        /// </summary>
        /// <param name="stateId"></param>
        /// <param name="RequestType">Pass value All or PriceQuote</param>
        /// <returns></returns>
        public DataTable GetCities(string stateId, string requestType)
        {
            Database db = null;
            DataTable dt = null;

            EnumBikeType bikeType = (EnumBikeType)Enum.Parse(typeof(EnumBikeType), requestType, true);

            using (SqlCommand cmd = new SqlCommand("GetCities"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = (int)bikeType;
                cmd.Parameters.Add("@StateId", SqlDbType.BigInt).Value = stateId;

                try
                {
                    db = new Database();
                    dt = db.SelectAdaptQry(cmd).Tables[0];
                }
                catch (SqlException ex)
                {
                    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
            }
            return dt;
        }   // End of GetCities method

        /// <summary>
        /// Writen By : Ashwini Todkar on 4th Dec 2013
        /// </summary>
        /// <param name="requestType"></param>
        /// <returns>Table containg value as city Mapping name + id and city name</returns>

        public DataTable GetCitiesWithMappingName(string requestType)
        {

            Database db = null;
            DataTable dt = null;
            DataSet ds = null;

            using (SqlCommand cmd = new SqlCommand("GetCitiesWithMappingName"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = requestType;

                try
                {
                    db = new Database();
                    ds = db.SelectAdaptQry(cmd);

                    if (ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }
                }
                catch (SqlException ex)
                {
                    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
            }
            return dt;
        }//End GetCitiesWithMappingName

        /// <summary>
        /// Written By : Ashish G. Kamble on 3/8/2012
        /// Function returns city id and city names for price quote by providing model id
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public DataTable GetPriceQuoteCities(string modelId)
        {
            Database db = null;
            DataTable dt = null;

            using (SqlCommand cmd = new SqlCommand("GetPriceQuoteCities"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@modelId", SqlDbType.BigInt).Value = modelId;

                try
                {
                    db = new Database();
                    if(db != null)
                        dt = db.SelectAdaptQry(cmd).Tables[0];
                }
                catch (SqlException ex)
                {
                    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
            }
            return dt;
        }   // End of GetCities method

        /// <summary>
        ///     Written By : Ashish G. Kamble on 12 Dec 2012
        ///     Summary : Function will get the cities details for the given cityId.
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns>Function returns the Cities object in which city's details data is stored.</returns>
        public Cities GetCityDetails(string cityId)
        {
            Database db = null;
            SqlConnection conn = null;

            Cities objCity = null;

            string city = string.Empty;

            using (SqlCommand cmd = new SqlCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "GetCityDetails_SP";                
                
                cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;
                cmd.Parameters.Add("@City", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

                try
                {
                    db = new Database();
                    conn = new SqlConnection(db.GetConString());
                    cmd.Connection = conn;
                    Bikewale.Notifications.LogLiveSps.LogSpInGrayLog(cmd);
                    conn.Open();

                    cmd.ExecuteNonQuery();

                    // Set all details of the city into the cities object.
                    objCity = new Cities();

                    objCity.City = cmd.Parameters["@City"].Value.ToString();                    
                }
                catch (SqlException ex)
                {
                    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }

            return objCity;
        }   // End of GetCityDetails function

        /// <summary>
        /// Created By : Sadhana Upadhyay on 22nd Oct 2014
        /// Summary : Get Areas list by city id
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public DataTable GetAreas(string cityId)
        {
            Database db = null;
            DataTable dt = null;
            try
                {
                    using (SqlCommand cmd = new SqlCommand("GetAreas"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;

                        db = new Database();
                        dt = db.SelectAdaptQry(cmd).Tables[0];

                    }
                }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return dt;
        }   // End of GetAreas method
    
    }   // End of class
}   // End of namespace