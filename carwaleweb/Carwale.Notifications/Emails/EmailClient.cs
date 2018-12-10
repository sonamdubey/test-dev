using Carwale.Notifications.Logs;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Threading.Tasks;

namespace Carwale.Notifications.Emails
{
    public class EmailClient
    {
        private static readonly string _stmpHost = ConfigurationManager.AppSettings["SMTPSERVER"];

        public void SendEmail(MailMessage mail)
        {
            try
            {
                using (SmtpClient client = new SmtpClient(_stmpHost))
                {
                    client.Send(mail);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }

        public static async Task AsyncSendEmail(MailMessage mail)
        {
            try
            {
                using (SmtpClient client = new SmtpClient(_stmpHost))
                {
                    await client.SendMailAsync(mail).ConfigureAwait(false);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
            }
        }
    }
}
