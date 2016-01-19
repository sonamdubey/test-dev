using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace AppNotification.Notifications.MailTemplates
{
    public class ErrorMailTemplate : ComposeEmailBase
    {
        public Exception err { get; set; }
        public string PageUrl { get; set; }

        /// <summary>
        /// Summary : Constructor to intialize the parameters for creating error mail template.
        /// </summary>
        /// <param name="ex">Exception object.</param>
        /// <param name="pageUrl">Page url on which exception occured.</param>
        public ErrorMailTemplate(Exception ex, string pageUrl)
        {
            err = ex;
            PageUrl = pageUrl;
        }

        /// <summary>
        /// Summary : Overrided Method to provide mail body.
        /// </summary>
        /// <returns></returns>
        public override StringBuilder ComposeBody()
        {
            StringBuilder sb = null;

            try
            {
                sb = new StringBuilder();

                sb.Append("Person Accessing the Page : <br />\n\n");

                sb.Append("<br />\nHOST : ");
                sb.Append(HttpContext.Current.Request.ServerVariables["HTTP_HOST"].ToString());

                sb.Append("<br />\nURL : ");
                sb.Append(HttpContext.Current.Request.ServerVariables["URL"].ToString());

                sb.Append("<br />\nREWRITE URL : ");
                sb.Append(HttpContext.Current.Request.ServerVariables["HTTP_X_REWRITE_URL"].ToString());

                sb.Append("<br />\nREFERRER : ");
                if (HttpContext.Current.Request.ServerVariables["HTTP_REFERER"] != null)
                    sb.Append(HttpContext.Current.Request.ServerVariables["HTTP_REFERER"].ToString());

                sb.Append("<br />\nIP ADD Remote Addr: ");
                if (HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"] != null)
                    sb.Append(HttpContext.Current.Request.ServerVariables["HTTP_CLIENT_IP"].ToString());

                sb.Append("<br />\nIP ADD Remote Host: ");
                sb.Append(HttpContext.Current.Request.ServerVariables["REMOTE_HOST"].ToString());

                //get the page 
                sb.Append("<br />\nError on Page : ");
                sb.Append(PageUrl);

                //get the error message
                sb.Append("<br />\nError Message : ");
                sb.Append(err.Message);

                //get the innerexception
                sb.Append("<br />\nInner Exception : ");
                sb.Append(err.InnerException);

                //get the stack trace
                sb.Append("<br />\nStack Trace : ");
                sb.Append(err.StackTrace);
            }
            catch (Exception ex)
            {
                HttpContext.Current.Trace.Warn("Notifications.ErrorTempate ComposeBody : " + ex.Message);
            }
            return sb;
        }   // End of ComposeBody method

    }   // End of class
}
