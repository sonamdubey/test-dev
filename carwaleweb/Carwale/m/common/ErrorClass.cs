// ErrorClass.cs
//

using System;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;
using log4net;
using System.Reflection;

namespace MobileWeb.Common 
{
	public class ErrorClass 
	{
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
		Exception err;
		SqlException sqlErr;
		OleDbException oleErr;
		string pageUrl;
		bool sendMail = false;
		//string errorMail = "bugs@carwale.com";
		//string localMail = "";
		
		//used for writing the debug messages
		private HttpContext objTrace = HttpContext.Current;
					
		//constructor which assigns the exception
        public ErrorClass(Exception ex, string pageUrl) 
		{
			this.err = ex;	//assign the exception
			this.pageUrl = pageUrl;		//assign the page url
        }
		
		public ErrorClass(SqlException ex, string pageUrl) 
		{
			this.sqlErr = ex;	//assign the sql exeption
			err = (Exception)sqlErr;	//convert the sqlexceptio to exception
			this.pageUrl = pageUrl;		//assign the page url
        }
		
		public ErrorClass(OleDbException ex, string pageUrl) 
		{
			this.oleErr = ex;	//assign the sql exeption
			err = (Exception)oleErr;	//convert the sqlexceptio to exception
			this.pageUrl = pageUrl;		//assign the page url
        }
		
		/********************************************************************************************
		SendMail()
		THIS FUNCTION sends the mail for the error message as passed in the constructor.
		First it checks the value for the sendError flag in the we.config file. if it is set to 'On'
		then only it sends the message. Note that web.config file is case sensitive, hence the value 
		should be 'On' only. By default it is assumed to be Off. If it is not required to send the mail
		then it is set to 'Off'. If it is set to On, the we get the mail id to which the message is 
		to be sent, from the key,: 'errorMail'. default it is set to rajeevmantu@gmail.com.
		The mail id from which the mail is to be sent is get from the key, "localMail".
		********************************************************************************************/
		public void SendMail()
		{
			string sendError = "";
			//get the value of sendError from web.config file
			try
			{
                log4net.ThreadContext.Properties["ClientIP"] = HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].ToString();
                log4net.ThreadContext.Properties["Browser"] = HttpContext.Current.Request.Browser.Type;
                log4net.ThreadContext.Properties["Referrer"] = HttpContext.Current.Request.UrlReferrer;
                log4net.ThreadContext.Properties["UserAgent"] = HttpContext.Current.Request.UserAgent;
                log4net.ThreadContext.Properties["PhysicalPath"] = HttpContext.Current.Request.PhysicalPath;
                log4net.ThreadContext.Properties["Host"] = HttpContext.Current.Request.Url.Host;
                log4net.ThreadContext.Properties["Url"] = HttpContext.Current.Request.Url;
                log4net.ThreadContext.Properties["QueryString"] = HttpContext.Current.Request.QueryString.ToString();
                var Cookies = HttpContext.Current.Request.Cookies;
                log4net.ThreadContext.Properties["CityAndZone"] = string.Format("{0};{1};", (Cookies["_CustCityIdMaster"] != null ? Cookies["_CustCityIdMaster"].Value ?? "NULL" : "NULL"), (Cookies["_CustZoneIdMaster"] != null ? Cookies["_CustZoneIdMaster"].Value ?? "NULL" : "NULL"));
                log4net.ThreadContext.Properties["ABTEST"] = Cookies["_abtest"] != null ? (Cookies["_abtest"].Value ?? "NULL") : "NULL";
                log4net.ThreadContext.Properties["Cookie"] = HttpContext.Current.Request.Headers["Cookie"] ?? "NULL";
                log.Error(err);

                sendError = ConfigurationManager.AppSettings["sendError"].ToString();
				if(sendError != "On")
				{
					sendError = "Off";
				}
			}
			catch(Exception ex)
			{
				sendError = "Off";
				objTrace.Trace.Warn("Common:Error:SendMail: " + ex.Message);
			}
						
			if(sendError == "On")
				sendMail = true;	//default it is false
				
			if(sendMail == true)	
			{
				
				try
				{
					// make sure we use the local SMTP server
					//SmtpClient client = new SmtpClient("124.153.73.180");//"127.0.0.1";
                    SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPSERVER"]);	
					//get the from mail address
                    string localMail = ConfigurationManager.AppSettings["localMail"].ToString();
										
					MailAddress from = new MailAddress(localMail, "CarWale.com");

					//get the to mail id
                    string email = ConfigurationManager.AppSettings["errorMailTo"].ToString();
					string subject = " m.carwale.com : " + pageUrl;			
					
					// Set destinations for the e-mail message.
					MailAddress to = new MailAddress(email);
					
					// create mail message object
					MailMessage msg = new MailMessage(from, to);
					
					// Add Reply-to in the message header.
					msg.Headers.Add( "Reply-to", "contact@carwale.com" );
					
					// set some properties
					msg.IsBodyHtml = false;
					msg.Priority = MailPriority.High;
						
					//prepare the subject
					msg.Subject = subject;

					StringBuilder sb = new StringBuilder();
					
					sb.Append("Person Accessing the Page : " + CurrentUser.Email + "\n\n");							
					
					sb.Append("\nHOST : ");
					sb.Append(HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString());
					
					sb.Append("\nURL : ");
					sb.Append(HttpContext.Current.Request.ServerVariables["URL"].ToString());
					
					sb.Append("\nREWRITE URL : ");
					sb.Append(HttpContext.Current.Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());
					
					sb.Append("\nREFERRER : ");
					if(HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] != null)
						sb.Append(HttpContext.Current.Request.ServerVariables["HTTP_REFERER"].ToString());

                    sb.Append("<br />\nCityId;ZoneId: ");
                    var Cookies = HttpContext.Current.Request.Cookies;
                    sb.Append(string.Format("{0};{1};", (Cookies["_CustCityIdMaster"] != null ? Cookies["_CustCityIdMaster"].Value ?? "NULL" : "NULL"), (Cookies["_CustZoneIdMaster"] != null ? Cookies["_CustZoneIdMaster"].Value ?? "NULL" : "NULL")));

					sb.Append("\nServer IP Addr: ");
					if(HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"] != null)
						sb.Append(HttpContext.Current.Request.ServerVariables["LOCAL_ADDR"].ToString());
					
					
					sb.Append("\nIP ADD Remote Addr: ");
					if(HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null)
						sb.Append(HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].ToString());
					
					sb.Append("\nIP ADD Remote Host: ");
					sb.Append(HttpContext.Current.Request.ServerVariables["REMOTE_HOST"].ToString());
						
					//get the page 
					sb.Append("\nError on Page : ");
					sb.Append(pageUrl);
					
					//get the error message
					sb.Append("\nError Message : ");
					sb.Append(err.Message);
					
					//get the innerexception
					sb.Append("\nInner Exception : ");
					sb.Append(err.InnerException);
					
					//get the stack trace
					sb.Append("\nStack Trace : ");
					sb.Append(err.StackTrace);
								
					msg.Body = sb.ToString();
															
					// Send the e-mail
					client.Send(msg);
					
					objTrace.Trace.Warn(msg.From + "," + msg.To + "," + msg.Subject + "," + msg.Body);
				}
				catch(Exception err)
				{
					objTrace.Trace.Warn("CommonOpn:SendMail: " + err.Message);
					
				}				
			}
		}
    }//class
}//namespace
