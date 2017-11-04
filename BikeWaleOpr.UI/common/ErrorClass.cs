// ErrorClass.cs
//

using log4net;
using System;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Web;

namespace BikeWaleOpr.Common
{
    public class ErrorClass
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(ErrorClass));
        /// <summary>
        /// constructor which assigns the exception
        /// </summary>
        /// <param name="ex">Exception object.</param>
        /// <param name="pageUrl">Page URL on which error occured. If possible pass function name also.</param>
        public ErrorClass(Exception ex, string pageUrl)
        {
            LogCurrentHttpParameters();
            log.Error(pageUrl, ex);

        }

        /// <summary>
        /// constructor which assigns the exception
        /// </summary>
        /// <param name="ex">Sql Exception object.</param>
        /// <param name="pageUrl">Page URL on which error occured. If possible pass function name also.</param>
        public ErrorClass(SqlException ex, string pageUrl)
        {
            LogCurrentHttpParameters();
            log.Error(pageUrl, ex);
        }

        /// <summary>
        /// constructor which assigns the exception
        /// </summary>
        /// <param name="ex">OleDbException Exception object.</param>
        /// <param name="pageUrl">Page URL on which error occured. If possible pass function name also.</param>
        public ErrorClass(OleDbException ex, string pageUrl)
        {
            LogCurrentHttpParameters();
            log.Error(pageUrl, ex);
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 21 Dec 2016
        /// Description :   Log Current Http Parameters to GreyLog
        /// </summary>
        private static void LogCurrentHttpParameters()
        {
            HttpContext objTrace = HttpContext.Current;

            if (objTrace != null && objTrace.Request != null)
            {
                ThreadContext.Properties["ClientIP"] = Convert.ToString(objTrace.Request.ServerVariables["HTTP_CLIENT_IP"]);
                ThreadContext.Properties["Browser"] = objTrace.Request.Browser.Type;
                ThreadContext.Properties["Referrer"] = objTrace.Request.UrlReferrer;
                ThreadContext.Properties["UserAgent"] = objTrace.Request.UserAgent;
                ThreadContext.Properties["PhysicalPath"] = objTrace.Request.PhysicalPath;
                ThreadContext.Properties["Host"] = objTrace.Request.Url.Host;
                ThreadContext.Properties["Url"] = objTrace.Request.Url;
                ThreadContext.Properties["QueryString"] = Convert.ToString(objTrace.Request.QueryString);
                var Cookies = objTrace.Request.Cookies;
                if (Cookies != null)
                {
                    ThreadContext.Properties["BWC"] = (Cookies["BWC"] != null ? Cookies["BWC"].Value : "NULL");
                    ThreadContext.Properties["location"] = (Cookies["location"] != null ? Cookies["location"].Value : "NULL");
                }
            }
        }


        public static void LogError(Exception ex, string pageUrl)
        {
            LogCurrentHttpParameters();
            log.Error(pageUrl, ex);
        }

        /********************************************************************************************
        SendMail()
        
        ********************************************************************************************/
        /// <summary>
        /// Modified By : Ashish G. Kamble on 22 May 2013.
        /// This function sends the mail for the error message as passed in the constructor.
        /// First it checks the value for the sendError flag in the web.config file. if it is set to 'On'
        /// then only it sends the message. Note that web.config file is case sensitive, hence the value 
        /// should be 'On' only. By default it is assumed to be Off. If it is not required to send the mail
        /// then it is set to 'Off'. If it is set to On, the we get the mail id to which the message is 
        /// to be sent, from the key,: 'errorMail'. default it is set to rajeevmantu@gmail.com.
        /// The mail id from which the mail is to be sent is get from the key, "localMail".
        /// Add mentioned keys in web.config file : errorMailTo, ApplicationName
        /// Modified by :   Sumit Kate on 20 Dec 2016
        /// Description :   Commented Send Mail for Error Emails
        /// </summary>
        public static void SendMail()
        {
            //exception log
            //ExceptionLogging.SendErrorToText(Error);


            //string email = ErrorMailConfiguration.ERRORMAILTO;
            //string subject = "Error in " + ErrorMailConfiguration.APPLICATIONNAME + " at page: " + PageUrl;

            //ComposeEmailBase mail = new ErrorMailTemplate(Error, PageUrl);

            //mail.Send(email, subject);
        }

    }
}//namespace
