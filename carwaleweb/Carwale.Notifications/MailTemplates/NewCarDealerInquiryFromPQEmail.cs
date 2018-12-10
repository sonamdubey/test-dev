using Carwale.Entity.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Notifications.MailTemplates
{
    public class NewCarDealerInquiryFromPQEmail
    {
        public EmailEntity GetEmailTemplateDealer(string dealerEmail, string dealerName, string customerName, string cutomerMobile, string modelName, string customerEmail, string customerCity, string AssistanceType)
        {
            StringBuilder message = new StringBuilder();
            message.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">" +
          "<html xmlns=\"http://www.w3.org/1999/xhtml\">" +
          "<head>" +
              "<meta name=\"viewport\" content=\"width=device-width\" />" +
              "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />" +
              "<title></title>" +
          "</head>" +
          "<body>" +
        "<div style=\"max-width:680px; margin:0 auto; border:1px solid #d8d8d8; font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; background:#eeeeee;padding:10px 10px 20px 10px; word-wrap:break-word\">" +
        "<div style=\"background:#fff; margin:5px 0px 0px 0px\">"+
        "<div style=\"padding:10px;  border-top:solid 7px #0e3a51;\">"+
        "<div style=\"float:left; max-width:130px;\">"+
        "<a target=\"_blank\" style=\"text-decoration:none\" href=\"https://www.carwale.com/?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan\"><img border=\"0\" style=\"width:130px;\" title=\"CarWale\" alt=\"CarWale\" src=\"https://img.carwale.com/Mailer/PQimages/CW-offer-logo.jpg\"></a>" +
        "</div>"+
        "<div style=\"clear:both\"></div>"+
        "</div>            "+
        "<div style=\"background:url(https://img.carwale.com/Mailer/PQimages/offer-bottom-shadow.jpg) repeat-x 0px 0px #fff; padding:0px 0px 0 10px; height:6px;\"></div>" +
        
        "<div style=\"padding:10px;\">"+
        "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">Dear Dealer</p>"+
        "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\"> Please find below the details of the user who has expressed interest in the <strong>" + modelName + "</strong>.</p>"+
        "<div style=\"border:1px solid #e2e2e2; border-radius:5px; padding:10px; margin-bottom:15px;\">"+
        "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\"><strong>Name:</strong> " + customerName + "</p>"+
        "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\"><strong>Mobile:</strong> " + cutomerMobile + "</p>"+
        "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333;\"><strong>Email Id:</strong> " + customerEmail + "</p>"+
        "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333;\"><strong>City:</strong> " + customerCity + "</p>"+
        "</div>"+
        "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\"> Please get in touch with the user with " + AssistanceType + " assistance for buying the car. You can also email the brochure of the car alongwith your contact details, so that the user gets in touch with you at his/her convenience.</p>     " +
        "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:20px 0 15px 0;\">" +
        "Regards,<br />" +
        "<strong>Team CarWale</strong>" +
        "</p>" +
        "</div>" +
        "<!-- Body content part code ends here -->" +
        "</div>" +
        "<div style=\"background:url(images/offer-bottom-shadow.jpg) center center no-repeat #eeeeee; height:9px; width:100%\"></div>" +
        "<div style=\"padding:0px 0px 5px 0px;width:100%\">						" +
        "<div style=\"width:100px; margin:0 auto\">" +
        "<div style=\" float:left;padding-right:5px;\"><a href=\"https://twitter.com/Carwale/?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan\" target=\"_blank\"><img src=\"https://img.carwale.com/Mailer/PQimages/offer-t-icon.jpg\" alt=\"Twitter\" title=\"Twitter\" border=\"0\"/></a></div>" +
        "<div style=\"float:left; padding-right:5px;\"><a href=\"https://www.facebook.com/CarWale/?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan\" target=\"_blank\"><img src=\"https://img.carwale.com/Mailer/PQimages/offer-fb-icon.jpg\" alt=\"Facebook\" title=\"Facebook\" border=\"0\" /></a></div>" +
        "<div style=\"float:left; \"><a href=\"https://plus.google.com/+CarWale/posts?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan\" target=\"_blank\"><img src=\"https://img.carwale.com/Mailer/PQimages/offer-g-icon.jpg\" alt=\"Google+\" title=\"Google+\" border=\"0\" /></a></div>" +
        "<div style=\"clear:both;\"></div>" +
        "</div>" +
        "<div style=\"clear:both;\"></div>" +
        "</div>" +
        "</div>" +
        "</body></html>");

            var email = new EmailEntity()
            {
                Email = dealerEmail,
                Subject = "Enquiry for : Car #" + modelName,
                Body = message.ToString(),
                ReplyTo = customerEmail
            };

            return email;
        }

        public EmailEntity GetEmailTemplateCustomer(string custName, string custEmail, string modelName, string dealerName, string dealerAddress, string dealerMobile, string dealerEmail,string makeName)
        {
            StringBuilder message = new StringBuilder();
            message.Append("<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Transitional//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd\">" +
           "<html xmlns=\"http://www.w3.org/1999/xhtml\">" +
           "<head>" +
               "<meta name=\"viewport\" content=\"width=device-width\" />" +
               "<meta http-equiv=\"Content-Type\" content=\"text/html; charset=UTF-8\" />" +
               "<title></title>" +
           "</head>" +
           "<body>"
        + "<div style=\"max-width:680px; margin:0 auto; border:1px solid #d8d8d8; font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; background:#eeeeee;padding:10px 10px 20px 10px; word-wrap:break-word\">"
        + "<div style=\"background:#fff; margin:5px 0px 0px 0px\">"
        + "<div style=\"padding:10px;  border-top:solid 7px #0e3a51;\">"
        + "<div style=\"float:left; max-width:130px;\">"
        + "<a target=\"_blank\" style=\"text-decoration:none\" href=\"https://www.carwale.com/?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan\"><img border=\"0\" style=\"width:130px;\" title=\"CarWale\" alt=\"CarWale\" src=\"https://img.carwale.com/Mailer/PQimages/CW-offer-logo.jpg\"></a>"
        + "</div>"
        + "<div style=\"clear:both\"></div>"
        + "</div>"
        + "<div style=\"background:url(https://img.carwale.com/Mailer/PQimages/offer-bottom-shadow.jpg) repeat-x 0px 0px #fff; padding:0px 0px 0 10px; height:6px;\"></div>"
        + "<!-- Body content part code starts here -->"
        + "<div style=\"padding:10px;\">"
        + "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">Dear <strong>" + custName + ",</strong></p>"
        + "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">Thank you for expressing interest in the <strong>" +makeName +" "+modelName + ".</strong></p><p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">Given below are the details of your nearby " + makeName + " Dealer - <strong>" + dealerName + "</strong></p>"
        + "<div style=\"border:3px solid #969696; border-radius:5px; padding:10px 10px 0 10px; margin-bottom:15px;\">"
        + "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\"><strong>" + dealerName + "</strong></p>"
        + "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">" + dealerAddress + "</p>"
        + "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin-bottom:0px;\"><span style=\"font-weight:bold;\">Ph No.: " + dealerMobile + "</span></p>"
        + "</div>"
        + "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; line-height:20px;\">"
        + "For further assistance you can get in touch with the dealer at  <strong>" + dealerMobile + "</strong>. <br>"
        + "The dealership can also help you in getting attractive exchange deals for your old car, in addition to loyalty and corporate bonuses, wherever applicable."
        + "</p><p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:20px 0 15px 0;\">"
        + "Regards,<br /><strong>Team CarWale</strong></p></div>"
        + "<!-- Body content part code ends here -->"
        + "</div>"
        + "<div style=\"background:url(images/offer-bottom-shadow.jpg) center center no-repeat #eeeeee; height:9px; width:100%\"></div>"
        + "<div style=\"padding:0px 0px 5px 0px;width:100%\">"
        + "<div style=\"width:100px; margin:0 auto\">"
        +"<div style=\" float:left;padding-right:5px;\"><a href=\"https://twitter.com/Carwale/?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan\" target=\"_blank\"><img src=\"https://img.carwale.com/Mailer/PQimages/offer-t-icon.jpg\" alt=\"Twitter\" title=\"Twitter\" border=\"0\"/></a></div>" 
        +"<div style=\"float:left; padding-right:5px;\"><a href=\"https://www.facebook.com/CarWale/?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan\" target=\"_blank\"><img src=\"https://img.carwale.com/Mailer/PQimages/offer-fb-icon.jpg\" alt=\"Facebook\" title=\"Facebook\" border=\"0\" /></a></div>"
        +"<div style=\"float:left; \"><a href=\"https://plus.google.com/+CarWale/posts?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan\" target=\"_blank\"><img src=\"https://img.carwale.com/Mailer/PQimages/offer-g-icon.jpg\" alt=\"Google+\" title=\"Google+\" border=\"0\" /></a></div>"
        +"<div style=\"clear:both;\"></div>"
        + "</div><div style=\"clear:both;\"></div></div></div></body></html>");

            var email = new EmailEntity()
            {
                Email = custEmail,
                Subject = "Contact Details of your nearby " +makeName + " dealer - "+dealerName,
                Body = message.ToString(),
                ReplyTo = dealerEmail
            };

            return email;
        }
    
    }
}
