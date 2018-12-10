using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using Carwale.Notifications;
using Carwale.Interfaces.Forums;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications.Logs;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;

/// <summary>
/// This DAL implements the IModerator interface.
/// The methods include - 1) Log Moderator action  - 2) Display Abuse reported posts
/// </summary>
/// 
namespace Carwale.DAL.Forums
{
    public class ForumsModeratorDAL : IModerator
    {
        #region Log Moderator action
        public void LogModAction(string modId, string threadId, string actionType, string forumId)
        {     
            try
            {          
               using(DbCommand cmd = DbFactory.GetDBCommand("cwmasterdb.ModActionLog_v16_11_7"))
               {
                   cmd.CommandType = CommandType.StoredProcedure;
                   cmd.Parameters.Add(DbFactory.GetDbParam("v_Customerid", DbType.Int64, Convert.ToInt32(modId)));
                   cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId", DbType.String, 100, threadId));
                   cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumId", DbType.String, 100, forumId));
                   cmd.Parameters.Add(DbFactory.GetDbParam("v_FctionType", DbType.String, 50, actionType));
                   MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.CarDataMySqlMasterConnection);
               }
            }
            catch (MySqlException sqlEx)
            {
                ErrorClass objErr = new ErrorClass(sqlEx, "Forums-Log Mod Action -Moderator DAL - SQL Error");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Forums-Log Mod Action -Moderator DAL= error");
                objErr.SendMail();
            }
        }
        #endregion

        #region Show Report Abuse Report To Moderator
        /// <summary>
        /// This method gets the list of all post(s) reported as abusive by the users.
        /// </summary>
        /// <returns></returns>
        public DataSet ShowRepotAbuseReport()
        {
            DataSet ds = new DataSet();
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("ShowReportAbuseReport_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                }
            }
            catch (MySqlException sqlEx)
            {
              ErrorClass objErr = new ErrorClass(sqlEx, "Forums-Show Repot Abuse Report -SQL error");
              objErr.SendMail();
            }
            catch (Exception ex)
            {
              ErrorClass objErr = new ErrorClass(ex, "Forums-Show Repot Abuse Report - error");
              objErr.SendMail();
            }
            return ds;
        }
        #endregion


        public DataSet GetCustomerDetailsByHandleName(string HandleName)
        {
            DataSet ds = new DataSet();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetCustomerDetailsByHandleName_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_HandleName", DbType.String, 100, HandleName));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.CarDataMySqlReadConnection);
                }
            }
            catch (MySqlException sqlEx)
            {
                ErrorClass objErr = new ErrorClass(sqlEx, "GetCustomerDetailsByHandleName-mysql");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetCustomerDetailsByHandleName");
                objErr.SendMail();
            }
            return ds;
        }      
        public DataSet GetAction(string customerId)
        {
            DataSet ds = new DataSet();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetActionByCustomerId_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, customerId));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                }
            }
            catch (MySqlException sqlEx)
            {
                ErrorClass objErr = new ErrorClass(sqlEx, "GetAction-mysql");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "GetAction");
                objErr.SendMail();
            }
            return ds;
        }

        public bool BanCustomerById(string customerId,string UserId)
        {
            bool status = false;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("AddToBannedList_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_BannedBy", DbType.Int64, UserId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsAdded", DbType.Boolean, ParameterDirection.Output));
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                    status = Convert.ToBoolean(cmd.Parameters["v_IsAdded"].Value);
                }
            }
            catch (MySqlException sqlEx)
            {
                ErrorClass objErr = new ErrorClass(sqlEx, "BanCustomerById-mysql");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BanCustomerById");
                objErr.SendMail();
            }
            return status;
        }

        public void RemoveBanById(string customerId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("RemoveBanByCustomerId_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, customerId));
                    MySqlDatabase.UpdateQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                }
            }
            catch (MySqlException sqlEx)
            {
                ErrorClass objErr = new ErrorClass(sqlEx, "RemoveBanById-mysql");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "RemoveBanById");
                objErr.SendMail();
            }
        }
        public void ReActivateForumUser(string forumId)
        {
            try
            {

                using(DbCommand cmd = DbFactory.GetDBCommand("ActivateForumUser_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Id", DbType.Int64, forumId));
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                }
            }
            catch (MySqlException sqlEx)
            {
                ErrorClass objErr = new ErrorClass(sqlEx, "ReActivateForumUser-mysql");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ReActivateForumUser");
                objErr.SendMail();
            }
        }
        public void ActivateForNewsPost(string formId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("ActivateForNewPosts_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Id", DbType.Int64, formId));
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                }
            }
            catch (MySqlException sqlEx)
            {
                ErrorClass objErr = new ErrorClass(sqlEx, "ActivateForNewsPost-mysql");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ActivateForNewsPost");
                objErr.SendMail();
            }
        }
        public void ActivateForumThread(string threadId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("ActivateForumThread_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Id", DbType.Int64, threadId));
                    MySqlDatabase.UpdateQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                }
            }
            catch (MySqlException sqlEx)
            {
                ErrorClass objErr = new ErrorClass(sqlEx, "ActivateForNewsPost-mysql");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "ActivateForNewsPost");
                objErr.SendMail();
            }
        }
        public DataSet BindUnverifiedReviews()
        {
            DataSet ds = new DataSet();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetUserReviewsToModerate_v18_9_3"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.CarDataMySqlReadConnection);
                }           
            }
            catch (MySqlException err)
            {
                Exception ex = new Exception(err.Message);
            } // catch SqlException
            catch (Exception err)
            {
                Exception ex = new Exception(err.Message);
            } // catch Exception
            return ds;
        }


        public DataSet BindUnverifiedUpdatedReviews()
        {
            DataSet ds = new DataSet();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetUnverifiedUpdatedReviews_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.CarDataMySqlReadConnection);
                }
            }
            catch (MySqlException err)
            {
                Exception ex = new Exception(err.Message);
            } // catch SqlException
            catch (Exception err)
            {
                Exception ex = new Exception(err.Message);
            } // catch Exception
            return ds;
        }


    }//class
}//namespace