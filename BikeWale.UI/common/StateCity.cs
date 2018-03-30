using Bikewale.Entities.BikeData;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;

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

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
                    {
                        if (ds != null && ds.Tables != null && ds.Tables.Count > 0)
                            dt = ds.Tables[0];
                    }
                }

            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, requestType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbType.Int32, Convert.DBNull));

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
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
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, (int)bikeType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbType.Int32, (!string.IsNullOrEmpty(stateId)) ? stateId : null));

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
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
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
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
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, requestType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbType.Int32, Convert.DBNull));

                    using (DataSet ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly))
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
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
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
            ErrorClass.LogError(new Exception("Method not used/commented"), "SateCity.GetPriceQuoteCities");
            
            return null;

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
            //        ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //        
            //    }
            //    catch (Exception ex)
            //    {
            //        HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
            //        ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //        
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
            ErrorClass.LogError(new Exception("Method not used/commented"), "SateCity.GetCityDetails");
            
            return null;
           
        }   // End of GetCityDetails function

        /// <summary>
        /// Created By : Sadhana Upadhyay on 22nd Oct 2014
        /// Summary : Get Areas list by city id
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public DataTable GetAreas(string cityId)
        {
            ErrorClass.LogError(new Exception("Method not used/commented"), "SateCity.GetAreas");
            
            return null;

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
            //    ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    
            //}
            //catch (Exception ex)
            //{
            //    HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
            //    ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
            //    
            //}
            //return dt;
        }   // End of GetAreas method

    }   // End of class
}   // End of namespace