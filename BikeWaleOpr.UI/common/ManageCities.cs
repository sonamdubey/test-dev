using System;
using System.Collections.Generic;
using System.Web;
using BikeWaleOpr.Common;
using BikeWaleOpr.VO;
using System.Data;
using System.Data.SqlClient;

namespace BikeWaleOpr.Common
{
    /// <summary>
    /// Author  : Ashwini todkar written on 2nd Jan 2013 
    /// Summary : This Class manages city information 
    /// </summary>    
    public class ManageCities
    {
        /// <summary>
        /// Written By : Ashwini Todkar on 2nd Jan 2014
        /// summary    : This Method returns city name,id,masking name,default pin code,lattitude and longitude 
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns>Dataset</returns>
        public DataSet GetAllCitiesDetails(string stateId)
        {            
            DataSet ds = null;

            try
            {
                Database db = new Database();

                using (SqlCommand cmd = new SqlCommand("GetAllCitiesDetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = stateId;

                    ds = db.SelectAdaptQry(cmd);
                  
                }
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }

            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }//End of GetAllCitiesDetails


        /// <summary>
        /// Written By : Ashwini Todkar on 2nd Jan 2014
        /// summary    : This method deletes city
        /// </summary>
        /// <param name="cityId"></param>
        public void DeleteCity( string cityId)
        {
            Database db = null;   
  
            try
            {
                using (SqlCommand cmd = new SqlCommand("DeleteCity"))
                {
                    db = new Database();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = cityId;
                    db.UpdateQry(cmd);
                }
            }
            catch (SqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            finally
            {
                if (db != null)
                    db.CloseConnection();
            }
        }//End of DeleteCity method

        /// <summary>
        /// Written By : Ashwini Todkar on 2nd jan 2014
        /// summary    : This Method returns city name,maskingname,Lattitude,longitude,default pin code and std code for the provided cityId.
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns>City Object</returns>
        public City GetCityDetails(string cityId)
        {
            Database db = null;
            City objCity = null;
            SqlConnection conn = null;
            try
            {
                db = new Database();

                objCity = new City();

                using(conn = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetCityDetails";
                        cmd.Connection = conn;

                        HttpContext.Current.Trace.Warn("CITYID : " + cityId);

                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = cityId;
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MaskingName", SqlDbType.VarChar, 60).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Lattitude", SqlDbType.Float).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@Longitude", SqlDbType.Float).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@DefaultPinCode", SqlDbType.VarChar, 10).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@StateId", SqlDbType.Int).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@StdCode", SqlDbType.Int).Direction = ParameterDirection.Output;

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        
                        HttpContext.Current.Trace.Warn("qry success");

                        objCity.CityName = cmd.Parameters["@Name"].Value.ToString();                  
                        objCity.MaskingName = cmd.Parameters["@MaskingName"].Value.ToString();
                        objCity.Lattitude = cmd.Parameters["@Lattitude"].Value.ToString();
                        objCity.Longitude = cmd.Parameters["@Longitude"].Value.ToString();
                        objCity.DefaultPinCode = cmd.Parameters["@DefaultPinCode"].Value.ToString();
                        objCity.StateId = cmd.Parameters["@StateId"].Value.ToString();
                        objCity.IsDeleted = Convert.ToBoolean(cmd.Parameters["@IsDeleted"].Value);
                        objCity.StdCode = cmd.Parameters["@StdCode"].Value.ToString();
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetCityDetails sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetCityDetails ex : " + ex.Message + ex.Source);
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
            return objCity;
        }//End of GetCityDetails

        /// <summary>
        /// Written By : Ashwini Todkar on 2nd jan 2014
        /// Summary    : This method updates city details like name ,masking name,lattitude,longitude,std code,default pin and state id
        /// </summary>
        /// <param name="cityId"></param>
        /// <param name="city"></param>
        /// <param name="maskingName"></param>
        /// <param name="lattitude"></param>
        /// <param name="longitude"></param>
        /// <param name="stdCode"></param>
        /// <param name="defaultPinCode"></param>
        /// <param name="stateId"></param>
        public void ManageCityDetails(City objCity)
        {
            //string cityId, string city, string maskingName, string lattitude, string longitude,string stdCode,string defaultPinCode,string stateId
            Database db = null;
            SqlConnection conn = null;

            try
            {
                db = new Database();

                using (conn = new SqlConnection(db.GetConString()))
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "ManageCities";
                        cmd.Connection = conn;

                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = objCity.CityId;
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar, 50).Value = objCity.CityName;
                        cmd.Parameters.Add("@MaskingName", SqlDbType.VarChar, 60).Value = objCity.MaskingName;
                        cmd.Parameters.Add("@Lattitude", SqlDbType.Float).Value = objCity.Lattitude;
                        cmd.Parameters.Add("@Longitude", SqlDbType.Float).Value = objCity.Longitude;
                        cmd.Parameters.Add("@DefaultPinCode", SqlDbType.VarChar, 10).Value = objCity.DefaultPinCode;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.Int).Value = CurrentUser.Id;
                        cmd.Parameters.Add("@StateId", SqlDbType.Int).Value = objCity.StateId;

                        if (objCity.StdCode != "")
                        {
                            cmd.Parameters.Add("@StdCode", SqlDbType.Int).Value = objCity.StdCode;
                        }
                        HttpContext.Current.Trace.Warn("Update city sql ex : " + objCity.StdCode);
                        
                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("Update city sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Update city ex : " + ex.Message + ex.Source);
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
        }//End of ManageCityDetails

        /// <summary>
        /// Written By : Ashwini Todkar on 24 jan 2014
        /// Summary    : function returns city id as value and city name as text
        /// </summary>
        /// <param name="stateId"></param>
        /// <param name="requestType"></param>
        /// <returns></returns>
        public DataSet GetCities(int stateId, string requestType)
        {

            DataSet ds = null;

            try
            {
                Database db = new Database();

                using (SqlCommand cmd = new SqlCommand("GetCities"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (stateId > 0 )
                        cmd.Parameters.Add("@StateId", SqlDbType.Int).Value = stateId;
                    else
                        cmd.Parameters.Add("@StateId", SqlDbType.Int).Value = null;

                    cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = requestType;

                    ds = db.SelectAdaptQry(cmd);
                }
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }//End GetCities


        /// <summary>
        /// Written By : Ashish Kamble on 11 May 2015
        /// Summary    : function returns city id as value and city name as text from carwale cities
        /// </summary>
        /// <param name="stateId"></param>
        /// <param name="requestType">All</param>
        /// <returns></returns>
        public DataSet GetCWCities(int stateId, string requestType)
        {

            DataSet ds = null;

            try
            {
                Database db = new Database();

                using (SqlCommand cmd = new SqlCommand("GetCWCities"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    if (stateId > 0)
                        cmd.Parameters.Add("@StateId", SqlDbType.Int).Value = stateId;
                    else
                        cmd.Parameters.Add("@StateId", SqlDbType.Int).Value = null;

                    cmd.Parameters.Add("@RequestType", SqlDbType.VarChar, 20).Value = requestType;

                    ds = db.SelectAdaptQry(cmd);
                }
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass objErr = new ErrorClass(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return ds;
        }//End GetCities

    }
}