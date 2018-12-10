using Carwale.Entity.Classified.ListingPayment;
using Carwale.Interfaces.Classified;
using Carwale.Interfaces.Template;
using Carwale.Notifications.Emails;
using Carwale.Notifications.Logs;
using System;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Notifications.Classified
{
    public class ClassifiedMails: IClassifiedMails
    {
        private static EmailClient _client = new EmailClient();
        private static readonly string _localMail = ConfigurationManager.AppSettings["localMail"];
        private static readonly string _from = ConfigurationManager.AppSettings["MailFrom"];
        private readonly ITemplateRender _templateRenderer;

        public ClassifiedMails(ITemplateRender templateRenderer)
        {
            _templateRenderer = templateRenderer;
        }
        
        public static void MailToSellCarCustomer(string customerName, string mobile, string eMail, string profileId)
        {
            try
            {
                StringBuilder message = new StringBuilder();
                string subject = "Your ad is published on CarWale";

                message.Append("Congratulations! Your FREE ad is now published on CarWale.<br>");

                message.Append("Dear " + customerName + ",");
                message.Append("<br><br>Thank you for choosing CarWale to sell your car.<br>");

                message.Append("Your sell car ad <a href=\"https://www.carwale.com/used/CarDetails.aspx?car=" + profileId + "&ltsrc=17128\">" + profileId + "</a> has been published on <a href=\"https://www.carwale.com/?ltsrc=17128\" target=_blank style='color:#034fb6;'>www.carwale.com</a> ");
                message.Append("as a FREE listing and you will soon receive inquiries on email and SMS (on " + mobile + ") from interested buyers.");
                message.Append("<br>Good photos and description are more likely to attract buyers to your car. To add/ edit details of your ad, <a href=\"https://www.carwale.com/mycarwale/myinquiries/mysellinquiry.aspx?ltsrc=17128\">click here</a>. ");

                message.Append("For any clarity/ concern, please write to contact@carwale.com or call us at +91 22 6739 8888 (997).<br>");
                message.Append("We hope you sell your car soon.<br><br/>Regards,<br>Team CarWale");


                //Send Mail
                Send(eMail, subject, message.ToString());
            }
            catch (Exception err)
            {
                Logger.LogException(err);
            }
        }

        public void SendReceiptMail(Receipt receipt, string packageName)
        {
            if (receipt != null)
            {
                string subject = "Payment Receipt";
                StringBuilder message = new StringBuilder();
                message.Append("<div><div style='width:100%;height:40px;background:url(https://img.carwale.com/mailer/inquiryalert/carwale-logo.jpg) no-repeat;display: inline-block; '></div>");
                message.Append("<br /><div style='margin - top:20px'>Dear " + receipt.CustomerName + ",<br /><br />");
                message.Append("Thanks for making the payment of Rs " + receipt.Amount + "/- towards CarWale " + packageName);
                message.Append(" for your <strong>Listing id - S" + receipt.InquiryId + "</strong>. <br />");
                message.Append("Transaction Id for your payment is <strong>" + receipt.UniqueTransactionId + "</strong> <br /><br />");
                message.Append("For any clarity/ concern, please write to assist@carwale.com or call us at +91 8530482263.");
                message.Append("<br/><br/>Regards,<br/>Team CarWale</div></div>");
                Send(receipt.CustomerEmail, subject, message.ToString()); 
            }
        }

        public void SendUpgradeNotification<T>(T model, string email)
        {
            string templateId = ConfigurationManager.AppSettings["SellCarUpgradeMailTemplateId"];
            string message = _templateRenderer.Render(Convert.ToInt32(templateId), model);
            string subject = "Your CarWale Ad is upgraded";
            if (!string.IsNullOrWhiteSpace(message) && !string.IsNullOrWhiteSpace(email))
            {
                Send(email, subject, message);
            }
        }

        public static async Task AsyncNotifyPaymentComplete(dynamic model, string emailId)
        {
            if (model != null)
            {
                string subject = @"Payment recieved for Assisted Sales - S" + GetValueForDynamic(model, "InquiryId");
                StringBuilder message = new StringBuilder();
                message.Append($"Customer Name :{GetValueForDynamic(model, "CustomerName")}<br/>");
                message.Append($"Customer Mobile :{GetValueForDynamic(model, "CustomerMobile")}<br/>");
                message.Append($"Customer Email :{GetValueForDynamic(model, "CustomerEmail")}<br/>");
                message.Append($"Customer City :{GetValueForDynamic(model, "CustomerCity")}<br/>");
                message.Append($"Payment Amount :{GetValueForDynamic(model, "PaymentAmount")}<br/>");
                message.Append($"Payment Date :{GetValueForDynamic(model, "PaymentDate")}<br/>");
                await AsyncSend(emailId, subject, message.ToString()).ConfigureAwait(false);
            }
            
        }

        private static string GetValueForDynamic(dynamic myObject, string nameOfProperty)
        {
            var propertyInfo = myObject.GetType().GetProperty(nameOfProperty);
            var value = propertyInfo.GetValue(myObject, null);
            return value != null? value.ToString(): string.Empty;
        }

        private static async Task AsyncSend(string email, string subject, string result)
        {
            using (MailMessage m = CreateMailMessageObject(email, subject, result))
            {
                await EmailClient.AsyncSendEmail(m).ConfigureAwait(false);
            }
        }

        private static void Send(string email, string subject, string result)
        {
            using (MailMessage m = CreateMailMessageObject(email, subject, result))
            {
                _client.SendEmail(m);
            }
        }

        private static MailMessage CreateMailMessageObject(string email, string subject, string result)
        {
            MailMessage mailMessage = new MailMessage();
            mailMessage.Subject = subject;
            mailMessage.Body = result;
            mailMessage.From = new MailAddress(_localMail, _from);
            mailMessage.IsBodyHtml = true;
            mailMessage.To.Add(new MailAddress(email));
            return mailMessage;
        }

    }
}
