using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using BikeWaleOpr.Common;
using BikeWaleOpr.VO;
using System.Data.Common;
using BikeWaleOPR.DAL.CoreDAL;
using BikeWaleOPR.Utilities;

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
            DataSet ds = null;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("getallstatesdetails"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ds = MySqlDatabase.SelectAdapterQuery(cmd);
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
            State objState = null;

            try
            {
                objState = new State();

                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "getstatedetails";


                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbParamTypeMapper.GetInstance[SqlDbType.Int], stateId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maskingname", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 40, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_statecode", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 2, ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_isdeleted", DbParamTypeMapper.GetInstance[SqlDbType.Bit], ParameterDirection.Output));


                    MySqlDatabase.ExecuteNonQuery(cmd);

                    objState.StateName = cmd.Parameters["par_name"].Value.ToString();

                    objState.MaskingName = cmd.Parameters["par_maskingname"].Value.ToString();
                    objState.StdCode = cmd.Parameters["par_statecode"].Value.ToString();
                    objState.IsDeleted = Convert.ToBoolean(cmd.Parameters["par_isdeleted"].Value);
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
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand())
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.CommandText = "managestates";


                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbParamTypeMapper.GetInstance[SqlDbType.Int], stateId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_name", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 30, stateName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_maskingname", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 40, maskingName));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_statecode", DbParamTypeMapper.GetInstance[SqlDbType.VarChar], 2, stdCode));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_updatedby", DbParamTypeMapper.GetInstance[SqlDbType.Int], CurrentUser.Id));

                    MySqlDatabase.ExecuteNonQuery(cmd);
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
        }//End of ManageStateDetails method

        /// <summary>
        /// Written By : Ashwini Todkar
        /// Summary : Method to delete the state.
        /// </summary>
        /// <param name="stateId"></param>
        public void DeleteState(string stateId)
        {

            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("deletestate"))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_id", DbParamTypeMapper.GetInstance[SqlDbType.Int], stateId));
                    MySqlDatabase.UpdateQuery(cmd);
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
        }//End of DeleteState method

        /// <summary>
        /// Written By : Ashwini Todkar on 2nd Jan 2014
        /// Summary : Function to get id as value and name as text of all states.
        /// </summary>
        /// <returns>datatable</returns>
        public DataTable FillStates()
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
            return dt;
        }   // End of FillStates method
    }
}