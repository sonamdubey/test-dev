using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using BikeWaleOpr.Common;
using BikeWaleOpr.VO;

namespace BikeWaleOpr.Common
{
    /// <summary>
    /// Author : Ashwini Todkar written on 1 Jan 2014
    /// Summary : This Class manages states information
    /// </summary>
    public class ManageStates
    {
        /// <summary>
        /// Written By : Ashwini Todkar on 2nd Jan 2013
        /// Summary : Method retrieves details of all states.
        /// </summary>
        /// <returns>DataSet</returns>
        public DataSet GetAllStatesDetails()
        {
            Database db = new Database();
            DataSet ds = null;

            try
            {
                using (SqlCommand cmd = new SqlCommand("GetAllStatesDetails"))
                {                
                    cmd.CommandType = CommandType.StoredProcedure;                    
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
        }//End of GetAllStatesDetails

        
        /// <summary>
        /// Written By : Ashwini Todkar on 2nd Jan 2014
        /// Summary : This method returns state details like State name, Masking name,state code
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns>state object</returns>
        public State GetStateDetails(string stateId)
        {
            Database db = null;
            SqlConnection conn = null;
            State objState = null; 

            try
            {
                db = new Database();

                using (conn = new SqlConnection(db.GetConString()))
                {
                    objState = new State();

                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.CommandText = "GetStateDetails";
                        cmd.Connection = conn;

                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = stateId;
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar, 30).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@MaskingName", SqlDbType.VarChar, 40).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@StateCode", SqlDbType.VarChar, 2).Direction = ParameterDirection.Output;
                        cmd.Parameters.Add("@IsDeleted", SqlDbType.Bit).Direction = ParameterDirection.Output;

                        conn.Open();
                        cmd.ExecuteNonQuery();
                        
                        objState.StateName = cmd.Parameters["@Name"].Value.ToString();
                        HttpContext.Current.Trace.Warn(" objState.StateName  :" + objState.StateName);
                        objState.MaskingName = cmd.Parameters["@MaskingName"].Value.ToString();
                        objState.StdCode = cmd.Parameters["@StateCode"].Value.ToString();
                        objState.IsDeleted = Convert.ToBoolean(cmd.Parameters["@IsDeleted"].Value);
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("GetStateDetails sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("GetStateDetails ex : " + ex.Message + ex.Source);
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
            return objState;
        }//End of GetStateDetails


        /// <summary>
        /// written By : Ashwini Todkar on 2nd Jan 2014
        /// summary : This method updates state information state name,nasking name,std code
        /// </summary>
        /// <param name="stateId">  stateId = -1 then it insert state to database otherwise it updates state info</param>
        /// <param name="stateName"></param>
        /// <param name="maskingName"></param>
        /// <param name="stdCode"></param>
        public void ManageStateDetails(string stateId, string stateName, string maskingName, string stdCode)
        {
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
                        cmd.CommandText = "ManageStates";
                        cmd.Connection = conn;

                        cmd.Parameters.Add("@ID", SqlDbType.Int).Value = stateId;
                        cmd.Parameters.Add("@Name", SqlDbType.VarChar, 30).Value = stateName;
                        cmd.Parameters.Add("@MaskingName", SqlDbType.VarChar, 40).Value = maskingName;
                        cmd.Parameters.Add("@StateCode", SqlDbType.VarChar, 2).Value = stdCode;
                        cmd.Parameters.Add("@UpdatedBy", SqlDbType.Int).Value = CurrentUser.Id;

                        conn.Open();
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (SqlException ex)
            {
                HttpContext.Current.Trace.Warn("Update State sql ex : " + ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Update State ex : " + ex.Message + ex.Source);
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
        }//End of ManageStateDetails method

        /// <summary>
        /// Written By : Ashwini Todkar
        /// Summary : Method to delete the state.
        /// </summary>
        /// <param name="stateId"></param>
        public void DeleteState(string stateId)
        {
            Database db = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand("DeleteState"))
                {
                    db = new Database();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add("@ID", SqlDbType.Int).Value = stateId;
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
        }//End of DeleteState method

        /// <summary>
        /// Written By : Ashwini Todkar on 2nd Jan 2014
        /// Summary : Function to get id as value and name as text of all states.
        /// </summary>
        /// <returns>datatable</returns>
        public DataTable FillStates()
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
                    HttpContext.Current.Trace.Warn("ManageStates sql ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
                catch (Exception ex)
                {
                    HttpContext.Current.Trace.Warn("ManageStates  ex : " + ex.Message + ex.Source);
                    ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                    objErr.SendMail();
                }
            }
            return dt;
        }   // End of FillStates method
    }
}