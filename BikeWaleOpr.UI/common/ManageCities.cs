using BikeWaleOpr.VO;
using MySql.CoreDAL;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Web;

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

                using (DbCommand cmd = DbFactory.GetDBCommand("getallcitiesdetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.Int32, stateId));

                    ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly);

                }
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }

            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            return ds;
        }//End of GetAllCitiesDetails


        /// <summary>
        /// Written By : Ashwini Todkar on 2nd Jan 2014
        /// summary    : This method deletes city
        /// </summary>
        /// <param name="cityId"></param>
        public void DeleteCity(string cityId)
        {
            throw new Exception("Method not used/commented");

            //Database db = null;   

            //try
            //{
            //    using (SqlCommand cmd = new SqlCommand("DeleteCity"))
            //    {
            //        db = new Database();
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = cityId;
            //        db.UpdateQry(cmd);
            //    }
            //}
            //catch (SqlException err)
            //{
            //    ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    
            //}
            //catch (Exception err)
            //{
            //    ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
            //    
            //}
            //finally
            //{
            //    if (db != null)
            //        db.CloseConnection();
            //}
        }//End of DeleteCity method

        /// <summary>
        /// Written By : Ashwini Todkar on 2nd jan 2014
        /// summary    : This Method returns city name,maskingname,Lattitude,longitude,default pin code and std code for the provided cityId.
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns>City Object</returns>
        public City GetCityDetails(string cityId)
        {
            throw new Exception("City GetCityDetails(string cityId) : Method not used/commented");

            
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
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "managecities";

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbType.Int32, objCity.CityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbType.String, 50, objCity.CityName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maskingname", DbType.String, 60, objCity.MaskingName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_lattitude", DbType.Double, objCity.Lattitude));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_longitude", DbType.Double, objCity.Longitude));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_defaultpincode", DbType.String, 10, objCity.DefaultPinCode));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbType.Int32, CurrentUser.Id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbType.Int32, objCity.StateId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stdcode", DbType.Int32, (objCity.StdCode != "") ? objCity.StdCode : Convert.DBNull));


                    MySqlDatabase.ExecuteNonQuery(cmd, ConnectionType.MasterDatabase);
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("Update city sql ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Update city ex : " + ex.Message + ex.Source);
                ErrorClass.LogError(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                
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
                using (DbCommand cmd = DbFactory.GetDBCommand("getcities"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, requestType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbType.Int32, (stateId > 0) ? stateId : Convert.DBNull));


                    ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly);

                }
            }
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
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
                using (DbCommand cmd = DbFactory.GetDBCommand("getcwcities"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_requesttype", DbType.String, 20, requestType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_stateid", DbType.Int32, (stateId > 0) ? stateId : Convert.DBNull));

                    ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly);
                }
            }
            catch (SqlException err)
            {
                //HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            catch (Exception err)
            {
                //HttpContext.Current.Trace.Warn(err.Message + err.Source);
                ErrorClass.LogError(err, HttpContext.Current.Request.ServerVariables["URL"]);
                
            }
            return ds;
        }//End GetCities

        /// <summary>
        /// Created By : Sadhana Upadhyay on 28 Oct 2015
        /// Summary : To get Price quote cities by modelId
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public DataSet GetPriceQuoteCities(uint modelId)
        {
            DataSet ds = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandText = "getpricequotecities";
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_modelid", DbType.Int32, modelId));

                    ds = MySqlDatabase.SelectAdapterQuery(cmd, ConnectionType.ReadOnly);
                }
            }
            catch (SqlException ex)
            {
                ErrorClass.LogError(ex, "sqlex in BikewaleOpr.ManageCities.GetPriceQuoteCities : " + ex.Message);
                
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "ex in BikewaleOpr.ManageCities.GetPriceQuoteCities : " + ex.Message);
                
            }
            return ds;
        }   //End of GetPriceQuoteCities
    }   //End of Class
}   //End of namespace