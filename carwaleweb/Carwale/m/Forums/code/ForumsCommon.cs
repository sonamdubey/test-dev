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
using MobileWeb.DataLayer;
using MobileWeb.Common;
using Carwale.Notifications.Logs;
using System.Data.Common;
using Carwale.DAL.CoreDAL.MySql;
using Carwale.DAL.Forums;

namespace MobileWeb.Forums
{
	public class ForumsCommon
	{		
        public static string CreateNewThread(string customerId, string messageText, int alertType, string forumId, string title, int IsModerated)
		{
			string threadId = "-1";
            string url = "-1";
            try
            {
                using (DbCommand cmd = DbFactory.GetDBCommand("CreateForumThread_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumSubCategoryId", DbType.Int64, forumId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Topic", DbType.String, 200, title));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_StartDateTime", DbType.DateTime, DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Message", DbType.String, 4000, messageText));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_AlertType", DbType.Int32, alertType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsModerated", DbType.Int32, IsModerated));
                    url = GenerateUrl(title);
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Url", DbType.String, 500, url));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ClientIPRemote", DbType.String, 50, !string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"]) ? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString() : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ClientIP", DbType.String, 50, !string.IsNullOrEmpty(HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"]) ? HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].ToString() : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ThreadId", DbType.Int64, ParameterDirection.Output));
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                    threadId = cmd.Parameters["v_ThreadId"].Value.ToString();
                }
            }
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			} // catch Exception			
			return threadId;
		}

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

        public static string SavePost(string customerId, string messageText, int alertType, string threadId, int IsModerated)
		{
			string postId = "-1";
			try
			{
                using (DbCommand cmd = DbFactory.GetDBCommand("EnterForumMessage_v16_11_7"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ForumId", DbType.Int64, threadId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_CustomerId", DbType.Int64, customerId));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_Message", DbType.String, 8000, messageText));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_MsgDateTime", DbType.DateTime, DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_AlertType", DbType.Int32, alertType));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_IsModerated", DbType.Int32, IsModerated));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ClientIPRemote", DbType.String, 50, HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"] != null ? HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"].ToString() : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_ClientIP", DbType.String, 50, HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null ? HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].ToString() : Convert.DBNull));
                    cmd.Parameters.Add(DbFactory.GetDbParam("v_PostId", DbType.Int64, ParameterDirection.Output));
                    LogLiveSps.LogSpInGrayLog(cmd);
                    MySqlDatabase.ExecuteNonQuery(cmd, DbConnections.ForumsMySqlMasterConnection);
                    postId = cmd.Parameters["v_PostId"].Value.ToString();
                }          
			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}		
			return postId;
		}
		
		public static void NotifySubscribers( string discussionUrl, string threadId, string threadName )
		{
			string handleName = GetHandleName(CurrentUser.Id);
			string url = "https://www.carwale.com" + discussionUrl;
			GetThreadSubscribers(url, threadId, threadName, handleName);
		}
		
		public static void GetThreadSubscribers(string url, string threadId, string threadName, string handleName)
		{
			IDataReader dr = null;
			Forum obj = new Forum();
			try
			{
				obj.GetReader = true;
				obj.GetThreadSubscribers(threadId);
				dr = obj.drReader;
				while (dr.Read())
				{
					NotifyForumSubscribedUsers(dr["email"].ToString(), dr["name"].ToString(), handleName, threadName, url);
				}
			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			finally
			{
				obj.drReader.Close();
			}
		}
		
		public static void NotifyForumSubscribedUsers( string receiverEmail, string receiverName, string replierName, string topic, string threadUrl )
		{
			try
			{
				StringBuilder message = new StringBuilder();
				string subject = "Reply to discussion '" + topic + "'";
				
				message.Append("<p>Dear " + receiverName + ",</p>");
							
				message.Append("<p>Your subscribed discussion <b>" );
				message.Append( topic + "</b> has just been replied by <b>" + replierName + "</b>." );
				
				message.Append("<p>Discussion Link:<br>" );
				message.Append("<a href='" + threadUrl + "'>" + threadUrl + "</a></p>");
				
				message.Append("<p>We thank you for your active participation in CarWale Forums.</p>" );
				message.Append("<p>Warm Regards,<br>CarWale Team</p>");
				
				message.Append("<p style='font-size:11px'>This mail was sent to you because " );
				message.Append("you have subscribed to this discussion as 'Instant Email'. " );
				message.Append("If you no longer wish to receive such emails, please visit " );
				message.Append("<a href='https://www.carwale.com/mycarwale/forums/subscriptions.aspx'>My Subscriptions</a> page to manage your existing subscriptions.<br><br>" );
				message.Append("This is an automated mail. Please do not reply to this mail. For all queries contact "
								+ "<a href='mailto:contact@carwale.com'>contact@carwale.com</a></p>");

				CommonOpn op = new CommonOpn();
				op.SendMail( receiverEmail, subject, message.ToString(), true );
			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
		}
		
		public static string GetHandleName(string customerId)
		{
			string retVal = "Anonymous";
			try
			{
                UserDAL dal = new UserDAL();
                var userdata = dal.GetExistingHandleDetails(Convert.ToInt32(customerId));
                if(userdata != null) return userdata.HandleName;
			}
			catch(Exception err)
			{
				ErrorClass objErr = new ErrorClass(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.SendMail();
			}
			return retVal;
		}
	}
}
	