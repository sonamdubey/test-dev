using System;
using System.Text;
using System.Web;

namespace Carwale.Notifications.MailTemplates
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 23 May 2013
    /// Summary : Class for creating the template for password recovery mail.
    /// </summary>
    public class CustomerPasswordRecoveryTemplate : ComposeEmailBase
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
        public CustomerPasswordRecoveryTemplate(string email, string customerName, string passwordToken)
        {
            CustomerName = customerName;
            Email = email;
            PasswordToken = passwordToken;
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
				
				message.Append( "<img align=\"right\" src=\"https://www.carwale.com/images/logo.gif\" />" );
				
				message.Append( "<h4>Dear " + CustomerName + ",</h4>" );
				
				message.Append( "<p>Greetings from Carwale!</p>" ); 
				
				message.Append( "<p>Thank you for choosing Carwale." );
				message.Append( " We are committed to making your car buying and selling process simpler.</p>" );
				
				message.Append( "<p>This mail is an auto-responder for your request to recover your Carwale password." );
				message.Append( " Your loginId and password are as mentioned below. </p>" );
				
				message.Append( "User ID : " + Email + "<br>" );
				
                message.Append("Please click the below link to reset your password.<br>");
                message.Append("https://www.carwale.com/users/resetcustomerpassword.aspx?tkn=" + PasswordToken + "</br>");
                message.Append("Above link is valid for 24 hours only.<br>");

				message.Append( "<p>We request your presence on the portal and look forward to serve your diverse needs.</p>" );
				
				message.Append( "<p>For any assistance or suggestions kindly mail us at contact@carwale.com</p>" ); 
				
				message.Append( "<br>Warm Regards,<br><br>" );
				message.Append( "Customer Care,<br><b>CarWale</b>" );								
			}
			catch(Exception err)
			{
				HttpContext.Current.Trace.Warn(err.Message);
				ExceptionHandler objErr = new ExceptionHandler(err,HttpContext.Current.Request.ServerVariables["URL"]);
				objErr.LogException();
			} // catch Exception

            return message;
        }   // End of composebody method

    }   // End of class
}   // End of namespace
