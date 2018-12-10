using System;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using System.Configuration;
using System.Text.RegularExpressions;
using System.Web.Security;
using System.Security.Principal;
using System.Net.Mail;
using System.IO;
using System.Xml;
using MobileWeb.Common;
using RabbitMqPublishing;
using System.Collections.Specialized;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using MySql.Data.MySqlClient;
using AEPLCore.Queue;
using Google.Protobuf;

namespace MobileWeb.DataLayer 
{
	//this class is inheriting from Parent class
	public class Forum : Parent
	{
		//this function function fetches the active forum categories
		public void GetActiveForumCategories()
		{
            using (DbCommand cmd = DbFactory.GetDBCommand("GetForumActiveCategory_v16_11_7"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                LoadDataMySql(cmd,DbConnections.ForumsMySqlReadConnection);
            }
			
		}
		
		//this function fetches detail data for the active forum categories
        //modified by supriya on 28/8/2013 to fetch url from ForumSubCategories table
		public void GetCategoriesDetails()
		{
            using (DbCommand cmd = DbFactory.GetDBCommand("GetForumCategoryDetails_v16_11_7"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                LoadDataMySql(cmd, DbConnections.ForumsMySqlReadConnection);
            }
			
		}
		
		//this function fetches hot discussions
        //modified by supriya on 28/8/2013 to fetch url from Forums table
		public void GetHotDiscussions()
		{
            using (DbCommand cmd = DbFactory.GetDBCommand("GetHotDiscussionForums_v16_11_7"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                LoadDataMySql(cmd, DbConnections.ForumsMySqlReadConnection);
            }
		}
		
		//this function fetches new discussions
        //modified by supriya on 28/8/2013 to fetch url from Forums table
		public void GetNewDiscussions()
		{
            using (DbCommand cmd = DbFactory.GetDBCommand("GetNewDiscussionForum_v16_11_7"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                LoadDataMySql(cmd, DbConnections.ForumsMySqlReadConnection);
            }
		}
		
		//to fetch thread count for forum sub category
		public void GetSubCategoryThreadCount(string forumSubCategoryId)
		{
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetSubCategoryThreadCount_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumSubCategoryId", DbType.Int32, forumSubCategoryId));
                    LoadDataMySql(cmd, DbConnections.ForumsMySqlReadConnection);
                }
            }
            catch (SqlException ex)
            {
                ErrorClass obj = new ErrorClass(ex, "Forums GetSubCategoryThreadCount()");
                obj.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass obj = new ErrorClass(ex, "Forums GetSubCategoryThreadCount()");
                obj.SendMail();
            }
		}

        //This fetches the forum sub category details
        //modified by supriya on 28/8/2013 to fetch url from ForumSubCategories table
        public void GetSubCategoryDetails(string forumSubCategoryId)
        {
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetSubCategoryDetails_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumSubCategoryId",DbType.Int32, forumSubCategoryId));
                    LoadDataMySql(cmd, DbConnections.ForumsMySqlReadConnection);
                }
            }
            catch (SqlException ex)
            {
                ErrorClass obj = new ErrorClass(ex, "Forums GetSubCategoryDetails()");
                obj.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass obj = new ErrorClass(ex, "Forums GetSubCategoryDetails()");
                obj.SendMail();
            }
        }
		
		//this method fetches pagewise threads
        //modified by supriya on 28/8/2013 to fetch url from Forums table
        //modified by jitendra singh 23 jan 2017 change to mysql
		public void GetPagewiseThreads(string forumSubCategoryId, string pageSize, string pageNo)
		{
            int startIndex = ((Convert.ToInt32(pageNo) - 1) * Convert.ToInt32(pageSize)) + 1;
            int lastIndex = Convert.ToInt32(startIndex) + Convert.ToInt32(pageSize) - 1;
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetPagewiseThreads_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumSubCategoryId", DbType.Int64, Convert.ToInt32(forumSubCategoryId)));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_startindex", DbType.Int32, startIndex));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_endindex", DbType.Int32, lastIndex));
                    LoadDataMySql(cmd, DbConnections.ForumsMySqlReadConnection);
                }
            }
            catch (MySqlException ex)
            {
                ErrorClass obj = new ErrorClass(ex, "Forums GetPagewiseThreads()");
                obj.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass obj = new ErrorClass(ex, "Forums GetPagewiseThreads()");
                obj.SendMail();
            }
		}
		
		//to fetch the thread details
        //modified by supriya on 28/8/2013 to fetch thread url from ForumSubCategories table & post url from forums table
		public void GetThreadDetails(string threadId)
		{
            using (DbCommand cmd = DbFactory.GetDBCommand("GetThreadDetails_v16_11_7"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId", DbType.Int64, threadId));
                LoadDataMySql(cmd,DbConnections.ForumsMySqlReadConnection);	
            }			
		}
		
		//to fetch post count for thread
		public void GetThreadPostCount(string threadId)
		{
            using (DbCommand cmd = DbFactory.GetDBCommand("GetThreadPostCount_v16_11_7"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId", DbType.Int64, threadId));
                LoadDataMySql(cmd,DbConnections.ForumsMySqlReadConnection);	
            }
		}
		
		//this method fetches pagewise posts
		public void GetPagewisePosts(string threadId, string pageSize, string pageNo)
		{       			
            int startIndex = ((Convert.ToInt32(pageNo) - 1) * Convert.ToInt32(pageSize));
            using (DbCommand cmd = DbFactory.GetDBCommand("GetPageWisePosts_v17_6_3"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId", DbType.Int64, threadId));
                cmd.Parameters.Add(DbFactory.GetDbParam("v_StartIndex", DbType.Int16, startIndex));
                cmd.Parameters.Add(DbFactory.GetDbParam("v_PageSize", DbType.Int16, pageSize));
                LoadDataMySql(cmd,DbConnections.ForumsMySqlReadConnection);	
            }				
		}
		
		//update thread views
		public void UpdateThreadViews(string threadId)
		{
            PublishManager rmq = new PublishManager();
            var payload = new NewCarConsumers.ThreadId
            {
                Id = Convert.ToInt32(threadId)
            };

            var message = new QueueMessage
            {
                ModuleName = (ConfigurationManager.AppSettings["NewCarConsumerModuleName"] ?? string.Empty),
                FunctionName = "ForumThreadViewCount",
                Payload = payload.ToByteString(),
                InputParameterName = "ThreadId",
            };
            rmq.PublishMessageToQueue((ConfigurationManager.AppSettings["ThreadViewCountQueue"] ?? "FORUMTHREADVIEWCOUNTMYSQL").ToUpper(), message);
		}
		
		//to see whther user is banned or not
		public void IsUserBanned(string customerId)
		{ 
            using (DbCommand cmd = DbFactory.GetDBCommand("Forums_IsUserBanned_v16_11_7"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, customerId));
                LoadDataMySql(cmd,DbConnections.ForumsMySqlReadConnection);	
            }	
		}
		
		//to fetch thread count for forum sub category
        //modified by supriya on 28/8/2013 to fetch url from Forums table
		public void GetStickyThreads(string forumSubCategoryId)
		{
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("GetStickyThreads_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumSubCategoryId", DbType.Int64, Convert.ToInt32(forumSubCategoryId)));
                    LoadDataMySql(cmd, DbConnections.ForumsMySqlReadConnection);
                }
            }
            catch (SqlException ex)
            {
                ErrorClass obj = new ErrorClass(ex, "Forums GetStickyThreads()");
                obj.SendMail();
            }
            catch (Exception ex)
            {
                ErrorClass obj = new ErrorClass(ex, "Forums GetStickyThreads()");
                obj.SendMail();
            }
		}
		
		//to fetch posts to reply to
		public void GetPostsToReplyTo(string postId)
		{
            using (DbCommand cmd = DbFactory.GetDBCommand("GetPostReply_v16_11_7"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DbFactory.GetDbParam("v_PostId", DbType.String, 200, postId));
                LoadDataMySql(cmd,DbConnections.ForumsMySqlReadConnection);
            }
			
		}		
		
		//get forum thread subscribers
		public void GetThreadSubscribers(string threadId)
		{
            using (DbCommand cmd = DbFactory.GetDBCommand("GetThreadSubscribers_v16_11_7"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumThreadId", DbType.Int64, threadId));
                cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, CurrentUser.Id));
                LoadDataMySql(cmd,DbConnections.ForumsMySqlReadConnection);	
            }
		}
		
        //modified by supriya on 9/2/2013 to fetch url from forums table
        public void GetPagewiseSearchThreads(string searchId, string pageSize, string pageNo)
        {
            string startIndex = Convert.ToString(((Convert.ToInt32(pageNo) - 1) * Convert.ToInt32(pageSize)) + 1);
            string lastIndex = Convert.ToString(Convert.ToInt32(startIndex) + Convert.ToInt32(pageSize) - 1);

            using (DbCommand cmd = DbFactory.GetDBCommand("GetPageWiseThreadBySearchId_v16_11_7"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DbFactory.GetDbParam("v_SearchId", DbType.Int64, searchId));
                cmd.Parameters.Add(DbFactory.GetDbParam("v_StartIndex", DbType.Int64, searchId));
                cmd.Parameters.Add(DbFactory.GetDbParam("v_EndIndex", DbType.Int64, searchId));
                LoadDataMySql(cmd, DbConnections.ForumsMySqlReadConnection);
            }
        }

        /*Author: Rakesh    
         * Date Created:23/5/2013
         * Description: to fetch for particular customer post count
         */
        public void GetCustomerHistory(string customerId)
        {
            using (DbCommand cmd = DbFactory.GetDBCommand("GetCustomerHistory_v16_11_7"))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, customerId));
                LoadDataMySql(cmd,DbConnections.ForumsMySqlReadConnection);
            }            
        }
	}
}		