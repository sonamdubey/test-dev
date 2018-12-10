using System;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Collections;
using Carwale.Interfaces.Notifications;
using Carwale.Notifications.Logs;

namespace Carwale.Notifications
{
    /// <summary>
    /// Modified By : Ashish G. Kamble on 26 apr 2013
    /// Summary : Class have methods for sending the mails. Class contains actual business logic to send mails.
    /// </summary>
    public class Email : IEmailNotifications
    {
        //used for writing the debug messages
        private HttpContext objTrace = HttpContext.Current;

        // Class level variables
        MailAddress from = null;
        public string localMail { get; set; }
        public string ReplyTo { get; set; }

        public Email()
        {
            //get the from mail address
            localMail = ConfigurationManager.AppSettings["localMail"].ToString();

            // email address for who is sending the mail
            from = new MailAddress(localMail, ConfigurationManager.AppSettings["MailFrom"]);
            ReplyTo = ConfigurationManager.AppSettings["ReplyTo"];
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
            SendEmail(email, subject, body, ReplyTo, null, null);
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
            SendEmail(email, subject, body, replyTo, null, null);
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
            SendEmail(email, subject, body, replyTo, cc, bcc);
        }   // End of SendMail method


        /// <summary>
        /// Written By : Ashish G. Kamble on 22 May 2013
        /// Summary : Function to configure the common mailing parameters.        
        /// </summary>
        private void Send(MailMessage msg)
        {
            try
            {
                using (SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPSERVER"]))
                {
                    if (ConfigurationManager.AppSettings["isLocal"] == "true") //If mail sending from local is enabled
                    {
                        client.UseDefaultCredentials = false;
                        client.Credentials = new System.Net.NetworkCredential("bugs@carwale.com", "arrow1851");
                        client.EnableSsl = true;
                    }
                    client.Send(msg); 
                }
            }
            catch (Exception err)
            {
                Logger.LogException(err);
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
        private void SendEmail(string email, string subject, string body, string replyTo, string []cc, string []bcc)
        {            
            try
            {
                // Modified By : Supriya Bhide on 9 Feb 2015
                // Summary : Removed multiple clients section from CommomOpn & handled here 

                MailMessage msg = new MailMessage();
                msg.From = from;

                //Check if there are multiple clients
                string[] emailList = email.Split(',');
                if (emailList.Length > 0)
                {
                    for (int i = 0; i < emailList.Length; i++)
                    {
                        msg.To.Add(new MailAddress(emailList[i].ToString()));// Set destinations for the e-mail message.
                    }
                }
                              
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
                Send(msg);
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
        }   // End of ConfigureMailSettings method
    }   // End of class
}   // End of namespace
