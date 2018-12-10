using System;
using System.Text;
using System.Web;

namespace Carwale.Notifications.MailTemplates
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 24 May 2013.
    /// Summary : Class for creating template for notifying users about the Forums discussion
    /// </summary>
    public class NotifyForumSubscribedUsersTemplate : ComposeEmailBase
    {        
        public string ReceiverName { get; set; }
        public string ReplierName { get; set; }
        public string Topic { get; set; }
        public string ThreadUrl { get; set; }

        /// <summary>
        /// Cosntructor to initialize the parameters. All parameters are mandatory.
        /// </summary>        
        /// <param name="receiverName">Name of the email receiver.</param>
        /// <param name="replierName">Name of the replier.</param>
        /// <param name="topic">Forum discussion topic.</param>
        /// <param name="threadUrl">Forum discussion thread url.</param>
        public NotifyForumSubscribedUsersTemplate(string receiverName, string replierName, string topic, string threadUrl)
        {            
            ReceiverName = receiverName;
            ReplierName = replierName;
            Topic = topic;
            ThreadUrl = threadUrl;
        }

        /// <summary>
        /// Method to generate mail contents.
        /// </summary>
        /// <returns></returns>
        public override StringBuilder ComposeBody()
        {
            StringBuilder message = null;

            try
            {
                message = new StringBuilder();                

                message.Append("<p>Dear " + ReceiverName + ",</p>");

                message.Append("<p>Your subscribed discussion <b>");
                message.Append(Topic + "</b> has just been replied by <b>" + ReplierName + "</b>.");

                message.Append("<p>Discussion Link:<br>");
                message.Append("<a href='" + ThreadUrl + "'>" + ThreadUrl + "</a></p>");

                message.Append("<p>We thank you for your active participation in CarWale Forums.</p>");
                message.Append("<p>Warm Regards,<br>CarWale Team</p>");

                message.Append("<p style='font-size:11px'>This mail was sent to you because ");
                message.Append("you have subscribed to this discussion as 'Instant Email'. ");
                message.Append("If you no longer wish to receive such emails, please visit ");
                message.Append("<a href='https://www.carwale.com/mycarwale/forums/subscriptions.aspx'>My Subscriptions</a> page to manage your existing subscriptions.<br><br>");
                message.Append("This is an automated mail. Please do not reply to this mail. For all queries contact "
                                + "<a href='mailto:contact@carwale.com'>contact@carwale.com</a></p>");                
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn(err.Message);
                ExceptionHandler objErr = new ExceptionHandler(err, HttpContext.Current.Request.ServerVariables["URL"]);
                objErr.LogException();
            } // catch Exception

            return message;
        }   // End of composebody method

    }   // End of class
}   // End of namespace
