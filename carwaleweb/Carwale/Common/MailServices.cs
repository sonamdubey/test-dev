/*THIS CLASS HOLDS ALL TH EFUNCTION FOR BINDING GRID, FILLING DROPDOWN LIST AND OTHER SORTS OF
COMMON OPERATIONS.
*/

using System;
using System.Text;
using System.Web;
using System.Configuration;
using System.Net.Mail;
using Carwale.UI.Common;
using Carwale.Notifications;

namespace Carwale.UI.Common
{
	public class MailServices
	{
		//used for writing the debug messages
		private readonly HttpContext objTrace = HttpContext.Current;

		/********************************************************************************************
        SendMail()
        THIS FUNCTION sends the mail to the dealers with thte email id as passed, in the html format.
        Note that web.config file is case sensitive, 
        The mail id from which the mail is to be sent is get from the key, "localMail".
        ********************************************************************************************/
		public void SendMail(string email, string subject, string body, bool htmlType)
		{
			try
			{
				// make sure we use the local SMTP server
				SmtpClient client = new SmtpClient(ConfigurationManager.AppSettings["SMTPSERVER"]);

				//get the from mail address
				string localMail = ConfigurationManager.AppSettings["localMail"].ToString();

				MailAddress from = new MailAddress(localMail, "CarWale.com");

				// Set destinations for the e-mail message.
				MailAddress to = new MailAddress(email);

				// create mail message object
				MailMessage msg = new MailMessage(from, to);

				// Add Reply-to in the message header.
				msg.Headers.Add("Reply-to", "contact@carwale.com");

				msg.IsBodyHtml = true;
				msg.Priority = MailPriority.High;

				msg.Subject = subject;

				msg.Body = body;

				client.Send(msg);

				objTrace.Trace.Warn(msg.From + "," + msg.To + "," + msg.Subject + "," + msg.Body);
			}
			catch (Exception err)
			{
				objTrace.Trace.Warn("CommonOpn:SendMail: " + err.Message);
				ErrorClass objErr = new ErrorClass(err, "SendMail in CommonOpn");
				objErr.SendMail();
			}

		}
	}//End Class 
}//namespace