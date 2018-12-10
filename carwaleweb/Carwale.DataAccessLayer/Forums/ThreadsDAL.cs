using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Carwale.Notifications;
using System.Text.RegularExpressions;
using Carwale.Interfaces.Forums;
using Carwale.Entity;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications.Logs;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;

/// <summary>
/// Summary description for ThreadsDAL
/// </summary>
/// 

namespace Carwale.DAL.Forums
{

    public class ThreadsDAL : IThread
    {

        #region Create New Thread
        /// <summary>
        /// Details of a new thread created is entered into the database.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="messageText"></param>
        /// <param name="alertType"></param>
        /// <param name="forumId"></param>
        /// <param name="title"></param>
        /// <param name="IsModerated"></param>
        /// <returns></returns>
        public string CreateNewThread(string customerId, string messageText, int alertType, string forumId, string title, int IsModerated, string remoteAddr, string clientIP )
        {
            string threadId = "-1";
            string url = "-1";
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("CreateForumThread_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumSubCategoryId",DbType.Int64,forumId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId",DbType.Int64,customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Topic",DbType.String,200,title));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_StartDateTime",DbType.DateTime,DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Message",DbType.String,4000,messageText));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_AlertType",DbType.Int32,alertType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsModerated",DbType.Int32,IsModerated));
                    url = GenerateUrl(title);
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Url",DbType.String,500,url));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ClientIPRemote",DbType.String,50,!string.IsNullOrEmpty(remoteAddr) ? remoteAddr : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ClientIP", DbType.String, 50, !string.IsNullOrEmpty(clientIP) ? clientIP : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId",DbType.Int64,ParameterDirection.Output));
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                    threadId = cmd.Parameters["v_ThreadId"].Value.ToString();
                }

            }
            catch (MySqlException ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Carwalevs/ThreadsDAL - CreateNewThread - Sql Error");
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Carwalevs/ThreadsDAL - CreateNewThread - Error");
                objErr.SendMail();
            } // catch Exception        
            return threadId;
        }
        #endregion

        #region Generate URL
        /// <summary>
        /// Generates the url from thread topic.
        /// </summary>
        /// <param name="title"></param>
        /// <returns>the generated url.</returns>
        private static string GenerateUrl(string title)
        {
            string tempurl;
            if (title == "")
            {
                return "";
            }
            else
            {
                string tmp = Regex.Replace(title, @"[^a-zA-Z 0-9]", "");//replaces all characters other than a character or a number with a space.
                string iurl = Regex.Replace(tmp, @"\s+", " ");// Replace multiple spaces with a single space
                tempurl = iurl.Trim().Replace(" ", "-").ToLower();//replace space with -.
            }
            return tempurl;
        }
        #endregion

        #region Close Thread
        /// <summary>
        /// This method is called to close thread from any further replies.
        /// </summary>
        /// <param name="threadId"></param>
        /// <returns>true if the thread was successfully closed</returns>
        public bool CloseThread(string threadId, string customerId)
        {
            ForumsModeratorDAL modActionLog = new ForumsModeratorDAL();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("Forums_CloseThread_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId", DbType.Int64, threadId));
                    MySqlDatabase.UpdateQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                }
                modActionLog.LogModAction(customerId, "", "Close-Thread", threadId);//variabale name mistakenly set previously to threadid , it is actually forumid
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Thread DAL-close thread method - MYSQL Error");
                objErr.SendMail();
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Thread DAL-close thread method- Error");
                objErr.SendMail();
            }
            return true;
        }
        #endregion

        #region Delete Thread
        /// <summary>
        /// This method deletes the selected thread in DB. 
        /// </summary>
        /// <param name="threadId"></param>
        /// <returns></returns>
        public bool DeleteThreadFromDB(string threadId, string customerId)
        {
            bool returnFlag=true;
            ForumsModeratorDAL modActionLog = new ForumsModeratorDAL();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("Forums_DeleteThread_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId", DbType.Int64, threadId));
                    MySqlDatabase.UpdateQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                }
                modActionLog.LogModAction(customerId, "", "Delete-Thread", threadId);
            }

            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Thread DAL-delete thread method- MYSQL Error");
                objErr.SendMail();
                returnFlag = false;
            } // catch Exception
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Thread DAL-delete thread method - Error");
                objErr.SendMail();
                returnFlag = false;
            }
            return returnFlag;
        }

        #endregion

        #region Get Thread Details
        /// <summary>
        /// Used to get thread specific details.
        /// </summary>
        /// <param name="threadId"></param>
        /// <returns></returns>
        public ThreadBasicInfo GetAllForums(string threadId)
        {
            ThreadBasicInfo ThreadInfo = new ThreadBasicInfo();
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forums_GetAllThreadDetails_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId",DbType.Int64,threadId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ID",DbType.Int64,ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Name",DbType.String,200,ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Description",DbType.String,200,ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Topic",DbType.String,200,ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ReplyStatus",DbType.Boolean,ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Url",DbType.String,400,ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumUrl",DbType.String,400,ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_StartedOn",DbType.DateTime,ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_StartedByEmail",DbType.String,400,ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_StartedByName",DbType.String,400,ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsStarterFake",DbType.Int16,ParameterDirection.Output));
                    LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd,DbConnections.ForumsMySqlReadConnection);
                    ThreadInfo.ForumId = cmd.Parameters["v_ID"].Value.ToString();
                    ThreadInfo.ForumName =cmd.Parameters["v_Name"].Value.ToString();
                    ThreadInfo.ForumDescription = cmd.Parameters["v_Description"].Value.ToString();
                    ThreadInfo.ThreadName = cmd.Parameters["v_Topic"].Value.ToString();
                    ThreadInfo.StartedOn = cmd.Parameters["v_StartedOn"].Value.ToString();                
                    ThreadInfo.ThreadUrl = cmd.Parameters["v_Url"].Value.ToString();
                    ThreadInfo.ForumUrl = cmd.Parameters["v_ForumUrl"].Value.ToString();
                    ThreadInfo.StartedByEmail = cmd.Parameters["v_StartedByEmail"].Value.ToString();
                    ThreadInfo.StartedByName = cmd.Parameters["v_StartedByName"].Value.ToString();
                    if (cmd.Parameters["v_IsStarterFake"].Value.ToString() != null && cmd.Parameters["v_IsStarterFake"].Value.ToString() != "")
                        ThreadInfo.IsStarterFake = Convert.ToBoolean(cmd.Parameters["v_IsStarterFake"].Value);
                    else
                        ThreadInfo.IsStarterFake = false;
                    if (cmd.Parameters["v_ReplyStatus"].Value.ToString() != null && cmd.Parameters["v_ReplyStatus"].Value.ToString() != "")
                        ThreadInfo.replyStatus = Convert.ToBoolean(cmd.Parameters["v_ReplyStatus"].Value);
                    else
                        ThreadInfo.replyStatus = false;
                }
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Threads DAL-Get All Forums");
                objErr.SendMail();
            } // catch Exception
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Threads DAL-Get All Forums");
                objErr.SendMail();
            } // catch Exception
            return ThreadInfo;
        }
        #endregion

        #region Get Forum For User Post Edit
        public ThreadBasicInfo GetForumForUserPostEdit(string threadId)
        {         
            ThreadBasicInfo ThreadInfo = new ThreadBasicInfo();
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forums_GetForumForUserPostEdit_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId",DbType.Int64,Convert.ToInt32(threadId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ID",DbType.Int32,ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Name",DbType.String,200,ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Topic",DbType.String,200,ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Url",DbType.String,400,ParameterDirection.Output));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumUrl",DbType.String,400,ParameterDirection.Output));
                    LogLiveSps.LogSpInGrayLog(cmd);
                    ThreadInfo.ForumId = cmd.Parameters["v_ID"].Value.ToString();
                    ThreadInfo.ForumName = cmd.Parameters["v_Name"].Value.ToString();
                    ThreadInfo.ThreadName = cmd.Parameters["v_Topic"].Value.ToString();
                    ThreadInfo.ThreadUrl = cmd.Parameters["v_Url"].Value.ToString();
                    ThreadInfo.ForumUrl = cmd.Parameters["v_ForumUrl"].Value.ToString();
                }

            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Threads DAL-Get All Forums For User Post Edit - SQL Error");
                objErr.SendMail();
            } // catch Exception
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Threads DAL-Get All Forums For User Post Edit");
                objErr.SendMail();
            } // catch Exception
            return ThreadInfo;
        }
        #endregion

        #region Get Thread Details
        /// <summary>
        /// This method gets the details of a thread.
        /// </summary>
        /// <param name="threadId">The Id of thread.</param>
        /// <param name="startIndex">The index number from where the result-set will start.</param>
        /// <param name="endIndex">The end index for the result-set.</param>
        /// <param name="totalPostCount">total post count for the selected thread.</param>
        /// <returns></returns>
        public DataSet GetThreadDetails(int threadId, int startIndex, int endIndex)
        {       
            DataSet ds = new DataSet();
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("GetForumPageDetails_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId", DbType.Int64, Convert.ToInt32(threadId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_startindex", DbType.Int32, startIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_endindex", DbType.Int32, endIndex));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                }         
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Thread DAL-get thread details method");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Threads DAL - check sticky threads.");
                objErr.SendMail();
            }
            return ds;
        }
        #endregion

        #region Get Moderator Login Status
        /// <summary>
        /// Gets the status of moderator login
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool GetModeratorLoginStatus(string customerId)
        {
            bool modLogin = false;        
            try
            {       
                using(DbCommand cmd = DbFactory.GetDBCommand("Forums_GetModeratorLoginStatus_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, customerId));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.ForumsMySqlReadConnection))
                    {
                         if (dr.Read())
                        {
                            if (dr["Role"].ToString().ToLower() == "moderator")
                            {
                                modLogin = true;
                            }
                        }
                    }
                }                              
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "ThreadsDAL.GetModeratorLoginStatus()");
                objErr.SendMail();

                modLogin = false;
            } // catch Exception
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Threads DAL - check sticky threads.");
                objErr.SendMail();
            }
            return modLogin;
        }
        #endregion 

        #region Insert Sticky Thread
        public string InsertStickyThreads(int id, int threadId, int strCat, int customerId)
        {
           // Database db = new Database();
            string lastSavedId = "-1";
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forum_InsertStickyThread_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ID",DbType.Int64,id));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId",DbType.Int64,threadId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CatId",DbType.Int16,strCat));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CreatedBy",DbType.Int64,customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_TempID",DbType.Int64,ParameterDirection.Output));
                    LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd,DbConnections.ForumsMySqlMasterConnection);
                    lastSavedId = cmd.Parameters["v_TempID"].Value.ToString();
                }
            }
            catch (MySqlException err)
            {
                HttpContext.Current.Trace.Warn("error = " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Threads DAL - check sticky threads-SQL Exception.");
                objErr.SendMail();
            } // catch Exception
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("error = " + ex.Message);
                ErrorClass objErr = new ErrorClass(ex, "Threads DAL - check sticky threads.");
                objErr.SendMail();
            }       
            return lastSavedId;
        }
        #endregion

        #region Delete From Sticky
        public void DeleteStickyThreads(int threadId, int customerId)
        {
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forums_DeleteFromSticky_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId", DbType.Int64, threadId));
                    MySqlDatabase.UpdateQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                }
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Threads DAL - check sticky threads-MYSQL Exception.");
                objErr.SendMail();
            } // catch Exception
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Threads DAL - check sticky threads.");
                objErr.SendMail();
            }        
        }
        #endregion

        #region Check Sticky Threads
        /// <summary>
        /// Checks for sticky threads.
        /// </summary>
        /// <param name="threadId"></param>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public bool chkStickyThreads(string threadId, string customerId)
        {
            bool retVal = false;
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forums_CheckStickyThreads_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CreatedBy",DbType.Int64,customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId",DbType.Int64,threadId));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.ForumsMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            retVal = true;
                        }
                    }
                }
                   
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Threads DAL - check sticky threads-MYSQL Exception.");
                objErr.SendMail();
            } // catch Exception
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Threads DAL - check sticky threads.");
                objErr.SendMail();
            }
            return retVal;
        }
        #endregion

        #region Update Forum Stats.
        public bool UpdateStats(string ForumId, string SubCategoryId)
        {
            bool returnVal = false;
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forum_UpdateStats_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumId", DbType.Int64, ForumId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_SubCategoryId", DbType.Int64, SubCategoryId));
                    returnVal = MySqlDatabase.UpdateQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                }
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Threads DAl - Update Stats - MYSQL Exception.");
                objErr.SendMail();
            } // catch Exception
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Threads DAL - Update Stats");
                objErr.SendMail();
            }
            return returnVal;
        }
        #endregion

        #region Fill Categories
        public DataSet FillCategories()
        {
          //  Database db = new Database();
            DataSet ds = new DataSet();
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forums_FillCategories_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                }          
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Thread DAL-get thread details method");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Threads DAL - check sticky threads.");
                objErr.SendMail();
            }
            return ds;
        }
        #endregion

        #region Fill Existing categories
        public DataSet FillExistingCategories(string threadId)
        {
            DataSet ds = new DataSet();
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forums_FillExsistingCategories_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId", DbType.Int64, Convert.ToInt32(threadId)));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                }
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Thread DAL-get thread details method");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Threads DAL - check sticky threads.");
                objErr.SendMail();
            }
            return ds;
        }
        #endregion

    } //class
} //namespace