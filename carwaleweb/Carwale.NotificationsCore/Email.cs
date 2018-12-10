using System;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Collections;

namespace Carwale.NotificationsCore
{
    /// <summary>
    /// Modified By : Ashish G. Kamble on 26 apr 2013
    /// Summary : Class have methods for sending the mails. Class contains actual business logic to send mails.
    /// </summary>
    public class Email
    {
        //used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;

        // Class level variables
        SmtpClient client = null;
        MailAddress from = null;
        public string localMail { get; set; }
        public string ReplyTo { get; set; }

        /// <summary>
        /// Note : Please add mentioned keys in web.config file. SMTPSERVER, localMail, MailFrom, ReplyTo, errorMailTo, ApplicationName. These keys are Necessory to send the mail.
        ///        Web.Config file is case sensitive.
        /// </summary>
        public Email()
        {
            ConfigureCommonMailSettings();
        }

               
        /// <summary>
        /// Written By : Ashish G. Kamble on 22 May 2013
        /// Summary : Function to send the mail. To send mail create object of the mailing template first.
        /// </summary>
        /// <param name="email">Email address on which mail will be send.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="body">Body content of the email.</param>
        public void SendMail(string email, string subject, string body)
        {
            ConfigureMailSettings(email, subject, body, ReplyTo, null, null);

        }   // End of SendMail method

        
        /// <summary>
        /// Written By : Ashish G. Kamble on 22 May 2013
        /// Summary : Function to send the mail. To send mail create object of the mailing template first.
        /// </summary>
        /// <param name="email">Email address on which mail will be send.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="body">Body content of the email.</param>
        /// <param name="replyTo">Email address to which reply will be send. Optional parameter.</param>
        public void SendMail(string email, string subject, string body, string replyTo)
        {
            ConfigureMailSettings(email, subject, body, replyTo, null, null);

        }   // End of SendMail method


        /// <summary>
        /// Written By : Ashish G. Kamble on 22 May 2013
        /// Summary : Function to send the mail. To send mail create object of the mailing template first.
        /// </summary>
        /// <param name="email">Email address on which mail will be send.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="body">Body content of the email.</param>
        /// <param name="replyTo">Email address to which reply will be send. Optional parameter.</param>
        /// <param name="cc">Gets the address collection that contains carbony copy (CC) recepients for this email message. Optional parameter.</param>
        /// <param name="bcc">Gets the address collection that contains blank carbony copy (BCC) recepients for this email message. Optional parameter.</param>
        public void SendMail(string email, string subject, string body, string replyTo, string[] cc, string[] bcc)
        {
            ConfigureMailSettings(email, subject, body, replyTo, cc, bcc);

        }   // End of SendMail method


        /// <summary>
        /// Written By : Ashish G. Kamble on 22 May 2013
        /// Summary : Function to configure the common mailing parameters.        
        /// </summary>
        private void ConfigureCommonMailSettings()
        {
            try
            {
                // make sure we use the local SMTP server
                client = new SmtpClient(ConfigurationManager.AppSettings["SMTPSERVER"]);//"127.0.0.1";

                //get the from mail address
                localMail = ConfigurationManager.AppSettings["localMail"].ToString();

                // email address for who is sending the mail
                from = new MailAddress(localMail, ConfigurationManager.AppSettings["MailFrom"]);

                ReplyTo = ConfigurationManager.AppSettings["ReplyTo"];
            }
            catch (Exception err)
            {
                objTrace.Trace.Warn("Notifications.SendMails ConfigureCommonMailSettings : " + err.Message);
                //ErrorClass objErr = new ErrorClass(err, "Notifications.SendMails ConfigureCommonMailSettings");
                //objErr.SendMail();
            }
        }   // End of ConfigureCommonMailSettings method


        /// <summary>
        ///     Written By : Ashish G. Kamble on 22 May 2013
        ///     Summary : Function to send the actual mail. Method contains algorithm to send the mail.
        /// </summary>
        /// <param name="email">Email address on which mail will be send.</param>
        /// <param name="subject">Subject of the email.</param>
        /// <param name="body">Body content of the email.</param>
        /// <param name="replyTo">Email address to which reply will be send. Optional parameter.</param>
        /// <param name="cc">Gets the address collection that contains carbony copy (CC) recepients for this email message. Optional parameter.</param>
        /// <param name="bcc">Gets the address collection that contains blank carbony copy (BCC) recepients for this email message. Optional parameter.</param>
        private void ConfigureMailSettings(string email, string subject, string body, string replyTo, string []cc, string []bcc)
        {

            try
            {
                // Set destinations for the e-mail message.
                MailAddress to = new MailAddress(email);

                // create mail message object
                MailMessage msg = new MailMessage(from, to);

                // Add Reply-to in the message header.
                msg.Headers.Add("Reply-to", String.IsNullOrEmpty(replyTo) ? ReplyTo : replyTo);

                // Check if cc is there or not
                if (cc != null && cc.Length > 0)
                {
                    MailAddress addCC = null;                    

                    for (int iTmp = 0; iTmp < cc.Length; iTmp++)
                    {
                        HttpContext.Current.Trace.Warn("CC " + iTmp + " : ", cc[iTmp] + " cc length : " + cc.Length.ToString());

                        addCC = new MailAddress(cc[iTmp]);

                        msg.CC.Add(addCC);

                        HttpContext.Current.Trace.Warn("CC count : ", msg.CC.Count.ToString());
                    }
                }
                
                // Check if BCC is there or not
                if (bcc != null && bcc.Length > 0)
                {
                    MailAddress addBCC = null;

                    for (int iTmp = 0; iTmp < bcc.Length; iTmp++)
                    {
                        HttpContext.Current.Trace.Warn("BCC " + iTmp + " : ", bcc[iTmp]);

                        addBCC = new MailAddress(bcc[iTmp]);

                        msg.Bcc.Add(addBCC);

                        HttpContext.Current.Trace.Warn("BCC count : ", msg.Bcc.Count.ToString());
                    }
                }
                
                // set some properties
                msg.IsBodyHtml = true;
                msg.Priority = MailPriority.High;

                //prepare the subject
                msg.Subject = subject;

                // mail body                         
                msg.Body = body;
                
                // Send the e-mail
                client.Send(msg);

                objTrace.Trace.Warn(msg.From + "," + msg.To + "," + msg.Subject + "," + msg.Body);
            }
            catch (Exception err)
            {
                objTrace.Trace.Warn("Notifications.SendMails ConfigureMailSettings : " + err.Message);
                //ErrorClass objErr = new ErrorClass(err, "Notifications.SendMails ConfigureMailSettings");
                //objErr.SendMail();
            }
        }   // End of ConfigureMailSettings method

    }   // End of class
}   // End of namespace
