using System;
using System.Text;
using System.Data;
using System.Web.UI.WebControls;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using Carwale.UI.Common;
using System.Collections;
using Ajax;
using AjaxPro;
using Carwale.Notifications;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications.Logs;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using Carwale.DAL.Forums;

namespace CarwaleAjax
{
    public class AjaxForum
    {
        // write all the Forum Ajax functions here
        // used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;

        [Ajax.AjaxMethod()]
        public bool AddSubscription(string subId, string customerId, int alertType)
        {
            bool returnVal = false;
            ForumSubscriptionsDAL subscriptionDAL = new ForumSubscriptionsDAL();
            returnVal = subscriptionDAL.AddSubscription(customerId, subId);                  
            return returnVal;
        }

        [Ajax.AjaxMethod()]
        public bool GetSubscribeLink(string subId, string customerId)
        {
            bool returnVal = false;
            CommonOpn op = new CommonOpn();
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("GetSubscribeLink_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_SubId", DbType.Int64, subId));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.ForumsMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            returnVal = true;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, objTrace.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return returnVal;
        }

        [Ajax.AjaxMethod()]
        public string GetTitle(string ThreadId)
        {
            if (ThreadId == "" || CommonOpn.CheckId(ThreadId) == false)
                return "";
            string retVal = "";
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetTitleByThreadId_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId", DbType.Int64, ThreadId));
                    using(IDataReader dr =  MySqlDatabase.SelectQuery(cmd,DbConnections.ForumsMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                         retVal = dr["Topic"].ToString();
                        }
                    }
                }            
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, objTrace.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }            
            return retVal;
        }

        [Ajax.AjaxMethod()]
        public bool ForumInsertReportAbuse(string subId, string customerId, string postId, string comment)
        {
            bool returnVal = false;         
            CommonOpn op = new CommonOpn();
            try
            {              
                using(DbCommand cmd = DbFactory.GetDBCommand("Forum_InsertReportAbuse_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_subid",DbType.Int64,subId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_postid",DbType.Int64,postId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_customerid",DbType.Int64,customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_comment",DbType.String,100,comment));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_createdate", DbType.DateTime, DateTime.Now));
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                    returnVal = true;
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, objTrace.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }           
            return returnVal;
        }

        //Code Added By Sentil On 20 OCT 2009
        //Changes Done on 19 Nov 2009
        [Ajax.AjaxMethod()]
        //Used to Check the existance of Handle Name 
        public bool GetHandleName(string handleName)
        {
            
            string retHandleName = "";
            bool result = false;

            if (handleName == "")
                return result;
            try
            {           
                DataSet ds = new DataSet();
                ForumsModeratorDAL userRepo = new ForumsModeratorDAL();
                ds = userRepo.GetCustomerDetailsByHandleName(handleName);
                if(ds !=null && ds.Tables.Count > 0)
                {
                    retHandleName = ds.Tables[0].Rows.Count > 0 ? ds.Tables[0].Rows[0]["HandleName"].ToString() :"";
                }

                if (retHandleName != "")
                {
                    result = true;
                }             
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message + ex.Source);
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }           
            return result;
        }
    

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public string GetOnlineForumUsers(string postedByIds)
        {
            string forumUsers = "";            
            return forumUsers;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public string ThankThePost(string postId)
        {
            string retVal = "";
            string customerId = "-1", handleExists = "-1", isSaved = "-1";
            try
            {
                customerId = CurrentUser.Id.ToString();

                if (customerId != "-1")
                {
                    if (DoesHandleExists(customerId))
                    {
                        handleExists = "1";
                        isSaved = SaveToPostThanks(customerId, postId);
                    }
                }

                //customer id : -1 when not logged in and is present when logged in
                //handleExists : -1 when not present and 1 when present
                //isSaved : -1 when error occured, 0 when already thanked, 1 when newly thanked
                retVal = customerId + "|" + handleExists + "|" + isSaved;
            }
            catch (Exception ex)
            {
                retVal = "";
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return retVal;
        }

        private bool DoesHandleExists(string cId)
        {
            bool handleExists = false;        
            try
            {            
                using (DbCommand cmd = DbFactory.GetDBCommand("GetExistingHandleDetails_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_userId", DbType.Int64, cId));
                    using (IDataReader dr = MySqlDatabase.SelectQuery(cmd, DbConnections.CarDataMySqlReadConnection))
                    {
                        if (dr.Read())
                            handleExists = true;
                    }
                }
            }
            catch (Exception ex)
            {
                handleExists = false;
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }           
            return handleExists;
        }

        private string SaveToPostThanks(string _customerId, string _postId)
        {
            string isSaved = "-1";
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("PostThanksSave_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerID", DbType.Int64, _customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PostID", DbType.Int64, _postId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsSaved", DbType.Boolean, ParameterDirection.Output));
                    LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                    if (Convert.ToBoolean(cmd.Parameters["v_IsSaved"].Value))
                        isSaved = "1";
                    else
                        isSaved = "0";
                }
            }
            catch (Exception ex)
            {
                isSaved = "-1";
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }
            return isSaved;
        }
      

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.None)]
        public string GetPostThanksCount(string postIds)
        {
            string retVal = "";
            try
            {      
                using (DbCommand cmd = DbFactory.GetDBCommand("GetPostThanksCount_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_postIds", DbType.String, 1000, postIds));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.ForumsMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            if (retVal == "")
                            {
                                retVal = dr["PostCount"].ToString();
                            }
                            else
                            {
                                retVal = retVal + "," + dr["PostCount"].ToString();
                            }
                        }
                    }
                }
               
            }
            catch (Exception ex)
            {
                retVal = ex.Message;
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.SendMail();
            }         
           return retVal;
        }
    }
}