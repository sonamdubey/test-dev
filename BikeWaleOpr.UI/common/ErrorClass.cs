// ErrorClass.cs
//

using System;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Data.SqlClient;
using System.Data.OleDb;

namespace BikeWaleOpr.Common
{
    public class ErrorClass
    {
        Exception err;
        SqlException sqlErr;
        OleDbException oleErr;
        string pageUrl;
        bool sendMail = false;
        //string errorMail = "vspl.bugs@gmail.com";
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
                sendError = ConfigurationManager.AppSettings["sendError"].ToString();
                if (sendError != "On")
                {
                    sendError = "Off";
                }
            }
            catch (Exception ex)
            {
                sendError = "Off";
                objTrace.Trace.Warn("Common:Error:SendMail: " + ex.Message);
            }

            if (sendError == "On")
                sendMail = true;	//default it is false

            if (sendMail == true)
            {
                //do further operation
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
                    string subject = "Error in opr.bikewale.com at page: " + pageUrl;

                    // Set destinations for the e-mail message.
                    MailAddress to = new MailAddress(email);

                    // create mail message object
                    MailMessage msg = new MailMessage(from, to);

                    // Add Reply-to in the message header.
                    msg.Headers.Add("Reply-to", "contact@carwale.com");

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
                    if (HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] != null)
                        sb.Append(HttpContext.Current.Request.ServerVariables["HTTP_REFERER"].ToString());

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

                    //body = " Person Accessing the Page : " + CurrentUser.Email + "\n" + body;                            

                    // Mail Server Configuration. Needed for Rediff Hosting.
                    //msg.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 1;
                    //msg.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverpickupdirectory"] = "C:\\inetpub\\mailroot\\pickup";

                    // Send the e-mail
                    client.Send(msg);

                    objTrace.Trace.Warn(msg.From + "," + msg.To + "," + msg.Subject + "," + msg.Body);
                }
                catch (Exception err)
                {
                    objTrace.Trace.Warn("CommonOpn:SendMail: " + err.Message);
                }
            }
        }

        /********************************************************************************************
        ConsumeError()
        THIS FUNCTION sends the mail for the error message as passed in the constructor.
        First it checks the value for the sendError flag in the we.config file. if it is set to 'On'
        then only it sends the message. Note that web.config file is case sensitive, hence the value 
        should be 'On' only. By default it is assumed to be Off. If it is not required to send the mail
        then it is set to 'Off'. If it is set to On, the we get the mail id to which the message is 
        to be sent, from the key,: 'errorMail'. default it is set to vspl.bugs@gmail.com.
        The mail id from which the mail is to be sent is get from the key, "localMail".
        ********************************************************************************************/
        public void ConsumeError()
        {
            string sendError = "";
            //get the value of sendError from web.config file
            try
            {
                sendError = ConfigurationManager.AppSettings["sendError"].ToString();
                if (sendError != "On")
                {
                    sendError = "Off";
                }
            }
            catch (Exception ex)
            {
                sendError = "Off";
                objTrace.Trace.Warn("Common:Error:SendMail: " + ex.Message);
            }

            if (sendError == "On")
                sendMail = true;	//default it is false

            if (sendMail == true)
            {
                //do further operation
                /*try
                {
                    // make sure we use the local SMTP server
                    SmtpMail.SmtpServer = "";
                    // create mail message object
                    MailMessage msg = new MailMessage();
					
                    // set some properties
                    msg.BodyFormat = MailFormat.Text;
                    msg.Priority = MailPriority.High;
					
                    //get the from mail address
                    localMail = ConfigurationSettings.AppSettings["localMail"].ToString();
                    msg.From = localMail;
					
                    //get the to mail id
                    errorMail = ConfigurationSettings.AppSettings["errorMailTo"].ToString();
                    msg.To = errorMail;
					
                    //prepare the subject
                    string subj = "Error in carwale.com at page: " + pageUrl;
                    msg.Subject = subj;
					
                    //prepare the body
                    StringBuilder sb = new StringBuilder();
					
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
                    SmtpMail.Send(msg);
                    objTrace.Trace.Warn(msg.From + "," + msg.To + "," + msg.Subject + "," + msg.Body);
                }
                catch(Exception ex)
                {
                    //
                    objTrace.Trace.Warn("Common:Error:ConsumeError: " + ex.Message);
                }*/
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
                    string subject = "Error in sify.carwale.com at page: " + pageUrl;

                    // Set destinations for the e-mail message.
                    MailAddress to = new MailAddress(email);

                    // create mail message object
                    MailMessage msg = new MailMessage(from, to);

                    // Add Reply-to in the message header.
                    msg.Headers.Add("Reply-to", "contact@carwale.com");

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
                    if (HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] != null)
                        sb.Append(HttpContext.Current.Request.ServerVariables["HTTP_REFERER"].ToString());

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

                    //body = " Person Accessing the Page : " + CurrentUser.Email + "\n" + body;                            

                    // Mail Server Configuration. Needed for Rediff Hosting.
                    //msg.Fields["http://schemas.microsoft.com/cdo/configuration/sendusing"] = 1;
                    //msg.Fields["http://schemas.microsoft.com/cdo/configuration/smtpserverpickupdirectory"] = "C:\\inetpub\\mailroot\\pickup";

                    // Send the e-mail
                    client.Send(msg);

                    objTrace.Trace.Warn(msg.From + "," + msg.To + "," + msg.Subject + "," + msg.Body);
                }
                catch (Exception err)
                {
                    objTrace.Trace.Warn("CommonOpn:SendMail: " + err.Message);
                    ErrorClass objErr = new ErrorClass(err, "SendMail in CommonOpn");
                    objErr.SendMail();
                }
            }
        }
    }//class
}//namespace
