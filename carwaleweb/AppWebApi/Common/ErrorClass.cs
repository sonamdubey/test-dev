// ErrorClass.cs
//
/*
 Note: Taken as itis from mobile site
 */

using System;
using System.Web;
//using System.Configuration;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;
using log4net;
using System.Reflection;

namespace AppWebApi.Common
{
	public class ErrorClass 
	{
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        Exception err;
		SqlException sqlErr;
		OleDbException oleErr;
		string pageUrl;
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
			}
			catch(Exception ex)
			{
				objTrace.Trace.Warn("Common:Error:SendMail: " + ex.Message);
			}
						
			
		}
    }//class
}//namespace
