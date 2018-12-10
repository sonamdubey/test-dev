using System;
using System.Text;

namespace Carwale.Notifications
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 26 apr 2013
    /// Summary : Class have methods for sending the mails. (Wrapper Class).
    /// Note : Please add mentioned keys in web.config file. SMTPSERVER, localMail, MailFrom, ReplyTo, errorMailTo, ApplicationName. These keys are Necessory to send the mail.
    ///        Web.Config file is case sensitive.
    /// </summary>
    public abstract class ComposeEmailBase
    {
        /// <summary>
        /// Written By : Ashish G. Kamble on 31 May 2013
        /// Summary : Method should be overridden in derived class. This method will return the mail body contents.
        /// </summary>
        /// <returns></returns>
        public abstract StringBuilder ComposeBody();

        /// <summary>
        /// Written By : Ashish G. Kamble on 31 May 2013
        /// Summary : Function to send the mail. To send mail create object of the mailing template first.
        /// </summary>
        /// <param name="sendTo">Email address(es) on which mail will be send.</param>
        /// <param name="subject">Subject of the email</param>
        public void Send(string sendTo, string subject)
        {
            var mail = new Email();
            mail.SendMail(sendTo, subject, Convert.ToString(ComposeBody()));
        }


        /// <summary>
        /// Written By : Ashish G. Kamble on 31 May 2013
        /// Summary : Function to send the mail. To send mail create object of the mailing template first.
        /// </summary>
        /// <param name="sendTo">Email address(es) on which mail will be send.</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="replyTo">Email address to which reply will be send. Optional parameter.</param>
        public void Send(string sendTo, string subject, string replyTo)
        {
            var mail = new Email();
            mail.SendMail(sendTo, subject, Convert.ToString(ComposeBody()), replyTo);
        }


        /// <summary>
        /// Written By : Ashish G. Kamble on 31 May 2013
        /// Summary : Function to send the mail. To send mail create object of the mailing template first.
        /// </summary>
        /// <param name="sendTo">Email address(es) on which mail will be send.</param>
        /// <param name="subject">Subject of the email</param>
        /// <param name="replyTo">Email address to which reply will be send. Optional parameter.</param>
        /// <param name="cc">Gets the address collection that contains carbony copy (CC) recepients for this email message. Optional parameter.</param>
        /// <param name="bcc">Gets the address collection that contains blank carbony copy (BCC) recepients for this email message. Optional parameter.</param>
        public void Send(string sendTo, string subject, string replyTo, string[] cc, string[] bcc)
        {
            var mail = new Email();
            mail.SendMail(sendTo, subject, Convert.ToString(ComposeBody()), replyTo, cc, bcc);
        }

    }   // End of class
}   // End of namespace
