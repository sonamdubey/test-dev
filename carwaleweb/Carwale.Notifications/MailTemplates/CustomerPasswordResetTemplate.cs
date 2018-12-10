using System;
using System.Text;
using System.Web;

namespace Carwale.Notifications.MailTemplates
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 23 May 2013
    /// Summary : Class for creating the template for password recovery mail.
    /// </summary>
    public class CustomerPasswordResetTemplate : ComposeEmailBase
    {
        public string CustomerName { get; set; }
        public string Email { get; set; }
        public string PasswordToken { get; set; }

        /// <summary>
        /// Constructor to initialize the parameters. All parameters are mandatory.
        /// </summary>
        /// <param name="email">User Id of the customer.</param>
        /// <param name="customerName">Name of the customer.</param>
        /// <param name="passwordToken">Valid password recovery token.</param>
        public CustomerPasswordResetTemplate(string email, string customerName, string passwordToken)
        {
            this.CustomerName = customerName;
            this.Email = email;
            this.PasswordToken = passwordToken;
            Send(email, "CarWale: Reset Password");
        }

        /// <summary>
        /// Method to compose the mail body.
        /// </summary>
        /// <returns></returns>
        public override StringBuilder ComposeBody()
        {
            StringBuilder message = null;

            try
            {
                message = new StringBuilder();

                message.Append("<img align=\"right\" src=\"https://img.aeplcdn.com/logo_cw.jpg\" />");

                message.Append("<h4>Dear " + CustomerName + ",</h4>");

                message.Append("<p>Greetings!</p>");

                //message.Append("<p>Thank you for choosing CarWale.");
                //message.Append(" We are committed to improve your car trading experience through our services.</p>");

                //message.Append("<p>This mail is an auto-responder for your request to recover your CarWale password.");
                //message.Append(" Your User ID is as mentioned below. </p>");

                message.Append("<p>To reset your CarWale password for the User ID " + Email + ", please ");
                message.Append("<a href=\"https://www.carwale.com/users/resetpassword.aspx?at=" + PasswordToken + "\">click here</a>.</p>");

                //message.Append("Please click the below link to reset your password.<br>");
                message.Append("<p>If this doesn’t work, copy the link below and paste it in your browser.<br>");
                message.Append("<a href=\"https://www.carwale.com/users/resetpassword.aspx?at=" + PasswordToken + "\">");
                message.Append("https://www.carwale.com/users/resetpassword.aspx?at=" + PasswordToken + "</a></p>");

                //message.Append("<br>Above link is valid for 2 hours only.<br>");
                message.Append("<p>Please note this link is only valid for two hours and you need to raise another password recovery request thereafter.</p>");

                //message.Append("<p>We request your presence on the portal and look forward to serve your diverse needs.</p>");

                message.Append("<p>For any query or feedback, please write to us at contact@carwale.com</p>");

                message.Append("<p>Warm Regards,<br>");
                message.Append("Team CarWale</p>");
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
