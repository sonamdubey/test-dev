using System;
using System.Text;
using System.Web;

namespace Carwale.Notifications.MailTemplates
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 27 May 2013
    /// Summary : Class to create mail template for customer review comment.
    /// </summary>
    public class CustomerReviewCommentTemplate : ComposeEmailBase
    {       
        public string ReviewerName { get; set; }
        public string Title { get; set; }
        public string ReviewUrl { get; set; }

        /// <summary>
        /// Summary : Constructor to initialize the parameters required for generating template.
        /// </summary>
        /// <param name="reviewerEmail">Email address of the person who written review.</param>
        /// <param name="reviewerName">Name of the person who written review.</param>
        /// <param name="title">Title of the review.</param>
        /// <param name="reviewUrl">Review url.</param>
        public CustomerReviewCommentTemplate(string reviewerName, string title, string reviewUrl)
        {            
            ReviewerName = reviewerName;
            Title = title;
            ReviewUrl = reviewUrl;
        }

        /// <summary>
        /// Function to compose the mail body.
        /// </summary>
        /// <returns></returns>
        public override StringBuilder ComposeBody()
        {
            StringBuilder message = null;

            try
            {
                message = new StringBuilder();
                
                message.Append("<table width=\"100%\" border=\"0\" "
                                + "style=\"border:1px solid #FDA8D7;background-color:#FFE8EB;font-size:12px;font-family:Verdana, Arial, Helvetica, sans-serif;\">"
                                + "<tr><td><b>Dear " + ReviewerName + ",</b></td></tr>");


                message.Append("<tr><td><p>Greetings from <a href='https://www.carwale.com'>Carwale!</a></p>");

                message.Append("<p>A comment is posted on your review <a href='" + ReviewUrl + "'>" + Title + "</a>.</p>");

                message.Append("<p><a href='" + ReviewUrl + "'>Click here</a> to read the comment on your review.</p>");

                message.Append("<p>We thank you for writing review for CarWale.<br><br>"
                                + "With Warm Regards,<br><br><b>Team CarWale</b></p>");

                message.Append("<p style='font-size:11px'>Disclaimer:<br>"
                                + "This is an automated mail. Please do not reply to this mail. For all queries contact<br>"
                                + "<a href='mailto:contact@carwale.com'>contact@carwale.com</a></p>");

                message.Append("</td></tr></table>");
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
