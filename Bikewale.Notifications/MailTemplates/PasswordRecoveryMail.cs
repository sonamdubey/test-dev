using System;
using System.Text;

namespace Bikewale.Notifications.MailTemplates
{
    /// <summary>
    /// Created By : Vivek Gupta
    /// Date : 09-08-2016
    /// Desc : Passoword Recovery Mail
    /// </summary>
    public class PasswordRecoveryMail : ComposeEmailBase
    {
        private string RecoveryMailHtml = null;

        public PasswordRecoveryMail(string customerName, string email, string token)
        {
            try
            {
                StringBuilder message = new StringBuilder();
                message.Append("<img align=\"right\" src=\"http://imgd4.aeplcdn.com/0x0/bw/static/design15/old-images/d/bw-logo.png\" />");

                message.Append("<h4>Dear " + customerName + ",</h4>");

                message.Append("<p>Greetings from BikeWale!</p>");

                message.Append("<p>Thank you for choosing BikeWale.");
                message.Append(" We are committed to improve your bike trading experience through our services.</p>");

                message.Append("<p>This mail is an auto-responder for your request to recover your BikeWale password.");
                message.Append(" Your loginId and password are as mentioned below. </p>");

                message.Append("User ID : " + email + "<br>");
                //message.Append( "Password : " + password + "<br>" );
                message.Append("Please click the below link to reset your password.<br>");
                message.Append("http://www.bikewale.com/users/resetcustomerpassword.aspx?tkn=" + token);
                message.Append("<br>Above link is valid for 24 hours only.<br>");

                message.Append("<p>We request your presence on the portal and look forward to serve your diverse needs.</p>");

                message.Append("<p>For any assistance or suggestions kindly mail us at contact@bikewale.com</p>");

                message.Append("<br>Warm Regards,<br><br>");
                message.Append("Customer Care,<br><b>BikeWale</b>");


                RecoveryMailHtml = message.ToString();
            }
            catch (Exception err)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(err, "Bikewale.Notification.PasswordRecoveryMail.ComposeBody");
                objErr.SendMail();
            } // catch Exception
        }

        public override string ComposeBody()
        {
            return RecoveryMailHtml;
        }
    }
}
