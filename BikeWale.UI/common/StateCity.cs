using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Bikewale.Entities.BikeData;
using System.Data.Common;
using Bikewale.Notifications.CoreDAL;

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
            DataTable dt = null;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("getstates"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            dt = ds.Tables[0];
                    }
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
            DataTable dt = null;

                try
                {
                    using (DbCommand cmd = DbFactory.GetDBCommand("getcities"))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        //cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = (int)bikeType;
                        //cmd.Parameters.Add("@StateId", SqlDbType.BigInt).Value = stateId;
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 20, requestType));
                        cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbParamTypeMapper.GetInstance[SqlDbType.Int], null));

                        using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd))
                        {
                            if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            {
                                dt = ds.Tables[0];
                            }
                        }
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
            DataTable dt = null;

            EnumBikeType bikeType = (EnumBikeType)Enum.Parse(typeof(EnumBikeType), requestType, true); 
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getcities"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = (int)bikeType;
                    //cmd.Parameters.Add("@StateId", SqlDbType.BigInt).Value = stateId;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 20, (int)bikeType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbParamTypeMapper.GetInstance[SqlDbType.Int], (!string.IsNullOrEmpty(stateId)) ? stateId : null));

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                        {
                            dt = ds.Tables[0];
                        }
                    }
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
        }   // End of GetCities method

        /// <summary>
        /// Writen By : Ashwini Todkar on 4th Dec 2013
        /// </summary>
        /// <param name="requestType"></param>
        /// <returns>Table containg value as city Mapping name + id and city name</returns>

        public DataTable GetCitiesWithMappingName(string requestType)
        {
            DataTable dt = null;

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getcitieswithmappingname"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    //cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = requestType;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 20, requestType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbParamTypeMapper.GetInstance[SqlDbType.Int], null));

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                        {
                            dt = ds.Tables[0];
                        }
                    }

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
        }//End GetCitiesWithMappingName

        /// <summary>
        /// Written By : Ashish G. Kamble on 3/8/2012
        /// Function returns city id and city names for price quote by providing model id
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public DataTable GetPriceQuoteCities(string modelId)
        {
            throw new Exception("Method not used/commented");

            //Database db = null;
            //DataTable dt = null;

            //using (SqlCommand cmd = new SqlCommand("GetPriceQuoteCities"))
            //{
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.Parameters.Add("@modelId", SqlDbType.BigInt).Value = modelId;

            //    try
            //    {
            //        db = new Database();
            //        if (db != null)
            //            dt = db.SelectAdaptQry(cmd).Tables[0];
            //    }
            //    catch (SqlException ex)
            //    {
            //        HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
            //        ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //        objErr.SendMail();
            //    }
            //    catch (Exception ex)
            //    {
            //        HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
            //        ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //        objErr.SendMail();
            //    }
            //}
            //return dt;
        }   // End of GetCities method

        /// <summary>
        ///     Written By : Ashish G. Kamble on 12 Dec 2012
        ///     Summary : Function will get the cities details for the given cityId.
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns>Function returns the Cities object in which city's details data is stored.</returns>
        public Cities GetCityDetails(string cityId)
        {
            throw new Exception("Method not used/commented");

            //Database db = null;
            //SqlConnection conn = null;

            //Cities objCity = null;

            //string city = string.Empty;

            //using (SqlCommand cmd = new SqlCommand())
            //{
            //    cmd.CommandType = CommandType.StoredProcedure;
            //    cmd.CommandText = "GetCityDetails_SP";

            //    cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;
            //    cmd.Parameters.Add("@City", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;

            //    try
            //    {
            //        db = new Database();
            //        conn = new SqlConnection(db.GetConString());
            //        cmd.Connection = conn;

            //        conn.Open();

            //        cmd.ExecuteNonQuery();

            //        // Set all details of the city into the cities object.
            //        objCity = new Cities();

            //        objCity.City = cmd.Parameters["@City"].Value.ToString();
            //    }
            //    catch (SqlException ex)
            //    {
            //        HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
            //        ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //        objErr.SendMail();
            //    }
            //    catch (Exception ex)
            //    {
            //        HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
            //        ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //        objErr.SendMail();
            //    }
            //    finally
            //    {
            //        if (conn.State == ConnectionState.Open)
            //        {
            //            conn.Close();
            //        }
            //    }
            //}

            //return objCity;
        }   // End of GetCityDetails function

        /// <summary>
        /// Created By : Sadhana Upadhyay on 22nd Oct 2014
        /// Summary : Get Areas list by city id
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public DataTable GetAreas(string cityId)
        {
            throw new Exception("Method not used/commented");

            //Database db = null;
            //DataTable dt = null;
            //try
            //{
            //    using (SqlCommand cmd = new SqlCommand("GetAreas"))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.Add("@CityId", SqlDbType.Int).Value = cityId;

            //        db = new Database();
            //        dt = db.SelectAdaptQry(cmd).Tables[0];

            //    }
            //}
            //catch (SqlException ex)
            //{
            //    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
            //    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //catch (Exception ex)
            //{
            //    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
            //    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    objErr.SendMail();
            //}
            //return dt;
        }   // End of GetAreas method

    }   // End of class
}   // End of namespace