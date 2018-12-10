using System;
using System.Web;
using System.Text;

namespace Carwale.Notifications.MailTemplates
{
    /// <summary>
    /// Written By : Ashish G. Kamble on 26 Apr 2013
    /// Summary : Class have methods which returns customer registration mail templates.
    /// </summary>
    public class CustomerRegistrationTemplate : ComposeEmailBase
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string CustomerId { get; set; }
        public bool IsARR { get; set; }

        /// <summary>
        /// Summary : Constructor to initialize the parameters required for the mailing template.
        /// </summary>
        /// <param name="name">Name of the customer.</param>
        /// <param name="email">Email address to which mail is to be send.</param>
        /// <param name="password">Password of the customer.</param>
        public CustomerRegistrationTemplate(string name, string email, string password, string customerId, bool isARR)
        {
            Name = name;
            Email = email;
            Password = password;
            CustomerId = customerId;
            IsARR = isARR;
            Send(email, "Welcome to CarWale");
        }

        /// <summary>
        /// Summary : Overrided Method to provide mail body.
        /// </summary>
        /// <returns></returns>
        public override StringBuilder ComposeBody()
        {
            StringBuilder message = null;

            try
            {
                message = new StringBuilder();

                message.Append("<img align=\"right\" src=\"https://img.aeplcdn.com/logo_cw.jpg\" />");

                message.Append("<h4>Hi,</h4>");

                message.Append("<p>Greetings from CarWale!</p>");

                message.Append("<p>Thank you for choosing CarWale. ");
                message.Append(" We are committed to making your car buying and selling process simpler.</p>");

                if (IsARR)
                    message.Append("<p>For future reference your user id and password will be as listed below:</p>");
                else
                    message.Append("<p>For future reference your user id will be as listed below:</p>");

                message.Append("User ID : " + Email + "<br>");

                if(IsARR)
                    message.Append("Password : " + Password + "<br>");

                message.Append("<p>You can change your password by visiting at <a href='https://www.carwale.com/mycarwale/'>www.carwale.com/mycarwale/</a></p>");

                //string cipher = CustomTripleDES.EncryptTripleDES(CustomerId);

                //HttpContext.Current.Trace.Warn("cipher" + cipher);

                //message.Append("<p>Please <a target=\"_blank\" href=\"https://www.carwale.com/users/verifyemail.aspx?verify=");
                //message.Append(cipher + "\">click here</a>");
                //message.Append(" to activate your account or copy and paste the following link in the browser’s address-bar.</p>");
                //message.Append("<a target=\"_blank\" href=\"https://www.carwale.com/users/verifyemail.aspx?verify=");
                //message.Append(cipher + "\">https://www.carwale.com/users/verifyemail.aspx?verify=" + cipher + "</a>");

                //message.Append("<br>Warm Regards,<br><br>");
                //message.Append("Customer Care,<br><b>CarWale</b>");

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
        }   // End of CustomerRegistration method

    }   // End of class
}   // End of namespace
