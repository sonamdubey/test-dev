using System;
using System.Configuration;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Text;
using System.Web;
using Bikewale.Notifications.MailTemplates;
using Bikewale.Notifications.Configuration;
using log4net;

namespace Bikewale.Notifications
{
    public class ErrorClass
    {
        protected static readonly ILog log = LogManager.GetLogger(typeof(ErrorClass));
        //used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;
        static ErrorClass()
        {
            log4net.Config.XmlConfigurator.Configure();
        }

        /// <summary>
        /// Error object.
        /// </summary>
        private Exception  _err;
        public Exception  Error
        {
            get { return _err; }
            set { _err = value; }
        }
        
        /// <summary>
        /// Page URL on which error occured.
        /// </summary>
        private string _pageUrl;
        public string PageUrl
        {
            get { return _pageUrl; }
            set { _pageUrl = value; }
        }
        
        /// <summary>
        /// constructor which assigns the exception
        /// </summary>
        /// <param name="ex">Exception object.</param>
        /// <param name="pageUrl">Page URL on which error occured. If possible pass function name also.</param>
        public ErrorClass(Exception ex, string pageUrl)
        {

            Error = ex;	//assign the exception
            PageUrl = pageUrl;		//assign the page url
            log.Error(pageUrl, ex);
            
        }

        /// <summary>
        /// constructor which assigns the exception
        /// </summary>
        /// <param name="ex">Sql Exception object.</param>
        /// <param name="pageUrl">Page URL on which error occured. If possible pass function name also.</param>
        public ErrorClass(SqlException ex, string pageUrl)
        {
            Error = (Exception)ex;	//convert the sqlexceptio to exception
            PageUrl = pageUrl;		//assign the page url
            log.Error(pageUrl, ex);
        }

        /// <summary>
        /// constructor which assigns the exception
        /// </summary>
        /// <param name="ex">OleDbException Exception object.</param>
        /// <param name="pageUrl">Page URL on which error occured. If possible pass function name also.</param>
        public ErrorClass(OleDbException ex, string pageUrl)
        {
            Error = (Exception)ex;	//convert the sqlexceptio to exception
            PageUrl = pageUrl;		//assign the page url
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
        /// </summary>
        public void SendMail()
        {
            //exception log
            //ExceptionLogging.SendErrorToText(Error);


            string email = ErrorMailConfiguration.ERRORMAILTO;
            string subject = "Error in " + ErrorMailConfiguration.APPLICATIONNAME + " at page: " + PageUrl;

            ComposeEmailBase mail = new ErrorMailTemplate(Error, PageUrl);
            
            mail.Send(email, subject);
        }

    }//class
}   // namespace
