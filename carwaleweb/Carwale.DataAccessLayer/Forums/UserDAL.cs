using System;
using System.Web;
using Carwale.Notifications;
using System.Data;
using System.Data.SqlClient;
using Carwale.Interfaces.Forums;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications.Logs;
using Carwale.Entity.Forums;
using Dapper;
using System.Linq;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using Carwale.Utility;
using MySql.Data.MySqlClient;

/// <summary>
/// Summary description for IUserDAL
/// </summary>
/// 
namespace Carwale.DAL.Forums
{
    public class UserDAL : RepositoryBase, IUser
    {

        #region Check For Banned User
        /// <summary>
        /// Checks if a user is banned or not
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool IsUserBanned(string customerId)
        {
            bool banned = false;
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forums_IsUserBanned_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, customerId));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.ForumsMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            banned = true;
                        }
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "User DAL - IsUserBanned");
                objErr.SendMail();
            }
            return banned;
        }
        #endregion

        ///
        ///This was the original ManageLastLogin() that was taken from VS repo, now moved to 'UserBusinessLogic' class in BL
        ///

        #region Get Last Login
        /// <summary>
        /// This method gets the last login time of the user. if not available, returns DateTime.now
        /// </summary>
        public DateTime GetLastLogin(string customerId, string forumLastLogin)
        {
            DateTime returnLogin = new DateTime();

            try    // try fetching last login from database.
            {
                DynamicParameters param = new DynamicParameters();
                param.Add("v_UserId",CustomParser.parseIntObject(customerId));
                param.Add("v_LastLoginTime",DbType.String,direction:ParameterDirection.Output);

                using (var con = ForumsMySqlMasterConnection)
                {
                    con.Query("Forums_FetchLastLogin_v16_11_7",param,commandType: CommandType.StoredProcedure);
                    returnLogin = !string.IsNullOrEmpty(param.Get<string>("v_LastLoginTime")) ? Convert.ToDateTime(param.Get<string>("v_LastLoginTime")) :new DateTime();
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "User DAL - GetLastLogin.");
                objErr.SendMail();
                returnLogin = System.DateTime.Now;
            }
            return returnLogin;
        }

        #endregion

        #region Check User Handle Name
        /// <summary>
        /// Validates user handle name.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public bool CheckUserHandle(string userId)
        {      
            bool result = false;
            if (userId == "")
                return result;
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("GetExistingHandleDetails_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_userId", DbType.Int64, Convert.ToInt32(userId)));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.CarDataMySqlReadConnection))
                    {
                        if (dr.Read())
                        {                      
                            if (dr["HandleName"].ToString() == "" || dr["IsUpdated"].ToString() == "False")
                                result = true;                        
                        }
                        else
                        {                       
                            result = true;
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                HttpContext.Current.Trace.Warn(ex.Message.ToString());
                ErrorClass objErr = new ErrorClass(ex, "ForumsCommon.CheckUserHandle()");
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "ForumsCommon.CheckUserHandle()");
                objErr.SendMail();
            }      
            return result;
        }
        #endregion

        #region Check if post(s) belong to same customerid.
        public bool CheckCustomerIds(string ids)
        {         
            DataSet dsCustomerId = new DataSet();
            bool retVal = false;
            try
            {               
                using(DbCommand cmd = DbFactory.GetDBCommand("Forums_CheckCustomerIds_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PostIds", DbType.String, 8000, ids));
                    dsCustomerId = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                    if(dsCustomerId != null && dsCustomerId.Tables.Count > 0)
                    {
                         if (dsCustomerId.Tables[0].Rows.Count == 1)
                            retVal = true;
                    }

                }

            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "User DAL - CheckCustomerIds - SQL Exception");
                objErr.SendMail();
            } // catch Exception
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "User DAL - CheckCustomerIds.");
                objErr.SendMail();
            }
            return retVal;
        }

        #endregion

        #region Get Login Status
        public bool GetLoginStatus(string userId, string postId)
        {        
            bool modLogin = false;
            string CustomerId = "-1";
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forums_GetCustomerForPost_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PostId", DbType.Int64, postId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, ParameterDirection.Output));
                    LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                    CustomerId = cmd.Parameters["v_CustomerId"].Value.ToString();
                }
                if (CustomerId == userId)
                {
                    modLogin = true;
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "User DAL - IsUserBanned");
                objErr.SendMail();
                modLogin = false;
            }
            return modLogin;
        }
        #endregion

        #region Load Handles
        public DataTable LoadHandles(string postId)
        {
           // Database db = new Database();
            DataSet ds = null;
            DataTable dt = null;
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forums_GetThankedHandles_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PostId", DbType.Int64, postId));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                    if(ds != null && ds.Tables.Count > 0)
                    {
                        dt = ds.Tables[0];
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "User DAL - IsUserBanned");
                objErr.SendMail();
            }
            return dt;
        }
        #endregion

        #region Save User Activity
        public void SaveActivity(string userId, string activityId, string categoryId, string threadId, string sessionId)
        {       
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("ForumUserTrackingSave_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_SessionID", DbType.String, 100, sessionId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_UserID", DbType.Int64, userId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ActivityId", DbType.Int64, activityId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CategoryId", DbType.Int32, categoryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId", DbType.Int64, threadId));
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                    LogLiveSps.LogSpInGrayLog(cmd);

                }            
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "SaveForumActivity");
                objErr.SendMail();
            }      
        }
        #endregion

        #region Get Users Count
        public string GetUsersCount()
        {       
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("ManageForumUserCount_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Result", DbType.String, 100, ParameterDirection.Output));
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                    LogLiveSps.LogSpInGrayLog(cmd);
                    return cmd.Parameters["v_Result"].Value.ToString();
                }          
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetUsersCount");
                objErr.SendMail();
                return "";
            }        
        }
        #endregion

        #region Load Category Views
        public DataSet LoadCategoryViews()
        {
            DataSet ds = new DataSet();       
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forums_GetCategoryViews_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                }              
            }
            catch (Exception ex)
            {
                ds = null;
                ErrorClass objErr = new ErrorClass(ex, "LoadCategoryViews");
                objErr.SendMail();
            }
            return ds;
        }
        #endregion        

        public UserProfile GetProfileDetails(int UserId)
        {
            UserProfile result = new UserProfile();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_userId", UserId > 0 ?  UserId : 0);
                using (var con = CarDataMySqlReadConnection)
                {
                    result = con.Query<UserProfile>("GetUserProfileDetails_v16_11_7", param, commandType: CommandType.StoredProcedure).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "UserDAL.GetProfileDetails(int UserId)");
                objErr.LogException();
            }
            return result;
        }

       public UserProfile GetExistingHandleDetails(int UserId)
        {
            UserProfile result = new UserProfile();
            try
            {
                var param = new DynamicParameters();
                param.Add("v_userId", UserId > 0 ? UserId : 0);
                using (var con = CarDataMySqlReadConnection)
                {
                    result = con.Query<UserProfile>("GetExistingHandleDetails_v16_11_7", param, commandType: CommandType.StoredProcedure).SingleOrDefault();
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "UserDAL.GetExistingHandleDetails(int UserId)");
                objErr.LogException();
            }
            return result;
        }
        public bool InsertHandle(int UserId,string HandleName,bool IsUpdated)
       {
           bool result = false;
           try
           {
               var param = new DynamicParameters();
               param.Add("v_UserId", UserId > 0 ? Convert.ToInt32(UserId) :0);
               param.Add("v_HandleName",HandleName);
               param.Add("v_IsUpdated", IsUpdated);
               param.Add("v_Status", DbType.Boolean, direction:ParameterDirection.Output);
               using(var con = CarDataMySqlMasterConnection)
               {
                con.Query<int>("InsertHandle_v16_11_7", param, commandType: CommandType.StoredProcedure).SingleOrDefault();
                result = Convert.ToBoolean(param.Get<int>("v_Status"));
               }
           }
            catch(Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "UserDAL.InsertHandle(int UserId,string HandleName,bool IsUpdated)");
                objErr.LogException();
            }
           return result;

       }

        public bool InsertImages(string userId, UserProfile entity)
        {
            var status = false;
            try
            {
                var param = new DynamicParameters();
                param.Add("v_UserId",userId);
                param.Add("v_AboutMe",entity.AboutMe);
                param.Add("v_Signature",entity.Signature);
                param.Add("v_AvtarPhoto",entity.AvtarPhoto);
                param.Add("v_ThumbNailPhoto",entity.ThumbNailUrl);
                param.Add("v_RealPhoto",entity.RealPhoto);
                param.Add("v_HostUrl",entity.HostURL);
                param.Add("v_UploadCount",0);
                param.Add("v_Status",DbType.Boolean,direction:ParameterDirection.Output);
                using(var con = CarDataMySqlMasterConnection)
                {
                    con.Execute("InsertUserProfile_v16_11_7", param, commandType: CommandType.StoredProcedure);
                    status = Convert.ToBoolean(param.Get<int>("v_Status"));
                }
                 
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "UserDAL.InsertImages(string userId, UserProfile entity)");
                objErr.LogException();
            }
            return status;
        }       
    }// class
}// namespace