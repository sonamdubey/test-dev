using System;
using System.Collections.Generic;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Collections;
using Carwale.Notifications;
using Carwale.Interfaces.Forums;
using Carwale.DAL.CoreDAL;
using Carwale.Notifications.Logs;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;
/// <summary>
/// Implements IPost interface methods.And other post related methods.
/// </summary>
/// 

namespace Carwale.DAL.Forums
{
    public class PostDAL : IPost
    {
        #region Delete Post
        /// <summary>
        /// This method deletes a post and updates the active post count of the user accordingly.
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="threadId"></param>
        /// <returns></returns>
        public bool DeletePost(string postId, string threadId, string customerId)
        {
            ForumsModeratorDAL modActionLog = new ForumsModeratorDAL();
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("Forums_DeletePost_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId", DbType.Int64, postId));
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                    LogLiveSps.LogSpInGrayLog(cmd);         
                }
                modActionLog.LogModAction(customerId, postId, "Delete-POST", threadId);
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Post DAL-delete post method-MYSQL Error.");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Post DAL-delete post method");
                objErr.SendMail();
            }         
            return true;
        }
        #endregion

        #region Merge Post Overload - 1
        /// <summary>
        /// Updates the merge id.
        /// </summary>
        /// <param name="mergeId"></param>
        public void MergePost(string mergeId)
        {
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forums_UpdateMergeIdInactive_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId", DbType.String,8000, mergeId));
                    MySqlDatabase.UpdateQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                }
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Post DAL-merge post method-SQL Error.");
                objErr.SendMail();
            }
        }
        #endregion

        #region Merge Post Overload - 2
        /// <summary>
        /// Merge the selected post (append the posts to the first one.)
        /// </summary>
        /// <param name="strMessage"></param>
        /// <param name="strMergeId"></param>
        public void MergePost(string strMessage, string strMergeId)
        {
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forums_MergeIdUpdate_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Message", DbType.String, 8000, strMessage));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_MergeId",DbType.Int64,strMergeId));
                    MySqlDatabase.UpdateQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                }
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Post DAL-merge post method-MYSQL Error.");
                objErr.SendMail();
            }
        }
        #endregion

        #region Append Message For Merge
        /// <summary>
        /// appends message for merge.
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        public string AppendMessagesForMerge(string ids)
        {
            string strMessage = "";
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forums_AppendMessagesForMerge_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PostIds", DbType.String, 8000, ids));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.ForumsMySqlReadConnection))
                    {
                        while (dr.Read())
                        {
                            strMessage = strMessage + "</BR>" + dr["Message"].ToString();
                        }
                    }
                }
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Post DAL - Append Messages For Merge - MYSQL Exception.");
                objErr.SendMail();
            } // catch Exception
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Post DAL - Append Messages For Merge - Exception.");
                objErr.SendMail();
            }
            return strMessage;
        }
        #endregion

        #region Edit Post by User
        /// <summary>
        ///  user edits an owned post.
        /// </summary>
        /// <param name="postId"></param>
        /// <param name="message"></param>
        /// <returns>true if the edit was successfull</returns>
        public bool EditPostByUser(int postId, string message, string customerId)
        {
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forums_EditPostByUser_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Message", DbType.String, 8000, message));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_UpdatedBy", DbType.Int64, customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PostId", DbType.Int64, postId));
                    MySqlDatabase.UpdateQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                }
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Post DAL - Edit post by user - SQL Exception.");
                objErr.SendMail();
            } // catch Exception
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Post DAL - Edit post by user - Exception.");
                objErr.SendMail();
            }     
            return true;
        }
        
        #endregion

        #region Save Post 
        /// <summary>
        /// Save the post entered by a user.
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="messageText"></param>
        /// <param name="alertType"></param>
        /// <param name="threadId"></param>
        /// <param name="IsModerated"></param>
        /// <returns>returns the postid.</returns>
        public  string SavePost(string customerId, string messageText, int alertType, string threadId, int IsModerated, string remoteAddr, string clientIp)
        {
            string postId = "-1";
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("EnterForumMessage_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumId",DbType.Int64,threadId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId",DbType.Int64,customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Message", DbType.String, 8000, messageText));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_MsgDateTime",DbType.DateTime,DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_AlertType", DbType.Int32, alertType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsModerated", DbType.Int32, IsModerated));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ClientIPRemote",DbType.String,50,remoteAddr != null ? remoteAddr : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ClientIP", DbType.String, 50, clientIp != null ? clientIp : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PostId",DbType.Int64,ParameterDirection.Output));                  
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                    postId = cmd.Parameters["v_PostId"].Value.ToString();
                }                   
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Error Saving The Post - MYSQL Error");
                objErr.SendMail();
            } // catch Exception
            catch (Exception err) 
            {
                ErrorClass objErr = new ErrorClass(err, "Error Saving The Post");
                objErr.SendMail();
            } // catch Exception        
            return postId;
        }
        #endregion

        #region Find Post
        /// <summary>
        /// returns the post count , which is used to calculate a post's position.
        /// </summary>
        /// <param name="post"></param>
        /// <param name="threadId"></param>
        /// <returns></returns>
        public int FindPost(int post,int threadId)
        {   
            int postcount = -1;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("Forums_FindPostPageNumber_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumThreadId", DbType.Int64, post));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId", DbType.Int64, threadId));
                    using(IDataReader dr = MySqlDatabase.SelectQuery(cmd,DbConnections.ForumsMySqlReadConnection))
                    {
                        if (dr.Read())
                        {
                            postcount = Convert.ToInt32(dr["OldPosts"]);
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
            return postcount;
        }
        #endregion

        #region Get Last Post Thread
        /// <summary>
        /// Get the last post thread.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="date"></param>
        /// <param name="lastPostById"></param>
        /// <returns></returns>
        public string GetLastPostThread(string name, string date, string lastPostById)
        {
            string retVal = "";
            if (name != "")
            {
                if (date != "")
                    retVal = Convert.ToDateTime(date).ToString("dd-MMM, yyyy hh:mm tt");
                if (name != "")
                    if (name != "anonymous")
                        retVal += "<br>by <a target='_blank' title=\"View " + name + "'s complete profile\" class='startBy' href='/community/members/" + name + ".html'>" + name + "</a>";
                    else
                        retVal += "<br>by <span class='startBy'>" + name + "</span>";
            }
            else
                retVal = " ";
            return retVal;
        }
        #endregion

        #region Get Post Method
        public string FillExistingPost(string postId)
        {
            string message = "-1";
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forums_GetPostMessage_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PostId", DbType.Int64, postId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Message", DbType.String, 8000, ParameterDirection.Output));
                    LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                    message = cmd.Parameters["v_Message"].Value.ToString();
                }
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Post DAL-Fill Existing Post-SQL Error.");
                objErr.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Post DAL-Fill Existing Post");
                objErr.SendMail();
            }
            return message;
        }
        #endregion

        #region  Save Post Thanks
        public bool SaveToPostThanks(string customerId, string postId)
        {
            bool IsSaved = false;
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("PostThanksSave_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerID", DbType.Int64, customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PostID", DbType.Int64, postId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsSaved", DbType.Int64, ParameterDirection.Output));
                    LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                    IsSaved = Convert.ToBoolean(cmd.Parameters["v_IsSaved"].Value);
                }            
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Post DAL - Edit post by user - SQL Exception.");
                objErr.SendMail();
            } // catch Exception
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Post DAL - Edit post by user - Exception.");
                objErr.SendMail();
            }        
            return IsSaved;

        }
        #endregion

        #region Move Post
        public void MovePost(string topic, int subCategoryId, int forumId)
        {
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forums_MovePost_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Topic", DbType.String, 200, topic));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumSubCategoryId", DbType.Int64, subCategoryId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumId", DbType.Int64, forumId));
                    MySqlDatabase.UpdateQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                }
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Post DAL - Edit post by user - MYSQL Exception.");
                objErr.SendMail();
            } // catch Exception
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Post DAL - Edit post by user - Exception.");
                objErr.SendMail();
            }        
        }
        #endregion

        #region Save Split Post Data

        public string SplitPostSaveData(int sId, int subCategory, string topic, int StrCustId, DateTime StrdateTime)
        {
            string lastSavedId = "-1";
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forum_InsertForum_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ID",DbType.Int32,sId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumSubCategoryId",DbType.Int32,subCategory));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Topic",DbType.String,100,topic));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId",DbType.Int64,StrCustId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_StartDateTime",DbType.DateTime,StrdateTime));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_TempID",DbType.Int32,ParameterDirection.Output));
                    LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                    if (cmd.Parameters["v_TempID"].Value.ToString() != "")
                        lastSavedId = cmd.Parameters["v_TempID"].Value.ToString();
                }
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Post DAL - Split Post Save Data - MYSQL Exception.");
                objErr.SendMail();
            } // catch Exception
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Post DAL - Split Post Save Data - Exception.");
                objErr.SendMail();
            }
            return lastSavedId;
        }
        #endregion

        #region Show Previous Posts
        public DataSet ShowPreviousPosts(string threadId)
        {
            DataSet ds = new DataSet();
            try
            {
                using(DbCommand cmd = DbFactory.GetDBCommand("Forums_ShowPreviousPosts_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumId", DbType.Int64, threadId));
                    ds = MySqlDatabase.SelectAdapterQuery(cmd, DbConnections.ForumsMySqlReadConnection);
                }
            }
            catch (MySqlException err)
            {
                ErrorClass objErr = new ErrorClass(err, "Post DAL - Show Previous Posts - MYSQL Exception.");
                objErr.SendMail();
            } // catch Exception
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Post DAL - Show Previous Posts - Exception.");
                objErr.SendMail();
            }
            return ds;
        }
        #endregion

    }
}