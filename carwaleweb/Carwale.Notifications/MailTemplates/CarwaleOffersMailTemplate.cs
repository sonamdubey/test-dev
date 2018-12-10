using Carwale.Entity.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Notifications.MailTemplates
{
    public class CarwaleOffersMailTemplate
    {
        public EmailEntity GetOffersCustEmailTemplate(string customerName, string couponCode, string carName, string dealerName, string dealerMobile, string dealerAddress, string cutomerMobile, string modelName, string customerEmail, string expiryDate, string dealerEmail, string offerTitle, string offerDesc)
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
                   "<div style=\"background:#fff; margin:5px 0px 0px 0px\">" +
                       "<div style=\"padding:10px;  border-top:solid 7px #0e3a51;\">" +
                           "<div style=\"float:left; max-width:130px;\">" +
                             "<a target=\"_blank\" style=\"text-decoration:none\" href=\"https://www.carwale.com/?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan\"><img border=\"0\" style=\"width:130px;\" title=\"CarWale\" alt=\"CarWale\" src=\"https://img.carwale.com/Mailer/PQimages/CW-offer-logo.jpg\"></a>" +
                           "</div>" +
                           "<div style=\"clear:both\"></div>" +
                       "</div>" +
                       "<div style=\"background:url(https://img.carwale.com/Mailer/PQimages/offer-shadow.png) repeat-x 0px 0px #fff; padding:0px 0px 0 10px; height:6px;\"></div>" +
                //<!-- Body content part code starts here -->
                       "<div style=\"padding:10px;\">" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">Dear <strong>" + customerName + "</strong></p>" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">Your CarWale coupon code is <strong style=\"font-size:20px;\">" + couponCode + "</strong></p>" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">This coupon code is valid against the booking of <strong>" + carName + "</strong> from</p>" +
                           "<div style=\"border:1px solid #e2e2e2; border-radius:5px; padding:10px; margin-bottom:15px;\">" +
                               "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">" + dealerName + "</p>" +
                               "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">" + dealerAddress + "</p>" +
                               "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333;\">" + dealerMobile + "</p>" +
                           "</div>" +
                           "<b>Offer</b>: " + offerTitle + " - " + offerDesc + "<br><br>" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">This offer expires on <strong>" + expiryDate + "</strong> and is a limited quantity offer available only till stock lasts. So please rush to book the car and claim your gift till stocks last.</p>" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 25px 0;\">For any queries, call us at 1800 2090 230.</p>" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:14px; color:#333333; margin:0 0 15px 0; font-weight:bold;\"><a href=\"https://www.carwale.com/offerTermsAndConditions.aspx\" target=\"_blank\">Terms and Conditions </a></p>" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:20px 0 15px 0;\">" +
                               "Regards,<br /><strong>Team CarWale</strong></p></div>" +
                //<!-- Body content part code ends here -->
                   "</div><div style=\"background:url(https://img.carwale.com/Mailer/PQimages/offer-bottom-shadow.jpg) center center no-repeat #eeeeee; height:9px; width:100%\"></div>" +
                   "<div style=\"padding:0px 0px 5px 0px;width:100%\">" +
                       "<div style=\"width:100px; margin:0 auto\">" +
                           "<div style=\" float:left;padding-right:5px;\"><a href=\"https://twitter.com/Carwale/?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan\" target=\"_blank\"><img src=\"https://img.carwale.com/Mailer/PQimages/offer-t-icon.jpg\" alt=\"Twitter\" title=\"Twitter\" border=\"0\"/></a></div>" +
                           "<div style=\"float:left; padding-right:5px;\"><a href=\"https://www.facebook.com/CarWale/?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan\" target=\"_blank\"><img src=\"https://img.carwale.com/Mailer/PQimages/offer-fb-icon.jpg\" alt=\"Facebook\" title=\"Facebook\" border=\"0\" /></a></div>" +
                           "<div style=\"float:left; \"><a href=\"https://plus.google.com/+CarWale/posts?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan\" target=\"_blank\"><img src=\"https://img.carwale.com/Mailer/PQimages/offer-g-icon.jpg\" alt=\"Google+\" title=\"Google+\" border=\"0\" /></a></div>" +
                           "<div style=\"clear:both;\"></div>" +
                       "</div>" +
                       "<div style=\"clear:both;\"></div>" +
                   "</div></div></body></html>");

            var email = new EmailEntity()
            {
                Email = customerEmail,
                Subject = "Carwale offer Coupon Code For " + carName,
                Body = message.ToString(),
                ReplyTo = dealerEmail
            };

            return email;
        }

        public EmailEntity GetOffersDealerEmailTemplate(string custName,string custEmail,string custMobile, string couponCode, string carName, string dealerEmail,string offerTitle,string offerDescription)
        {
            StringBuilder message = new StringBuilder();
            
            
            message.Append("<p>Dear Dealer Partner,<br><br>"

            + "A user has expressed interest in the " + offerTitle + " offer running against the " + carName + "<br><br>"

            + ".Given below are the details for the same.<br><br>"

            + "User Name: " + custName + "<br><br>"

            + "Email Id: " + custEmail + "<br><br>"

            + "Mobile number: " + custMobile + "<br><br>"

            + "Car: " + carName + "<br><br></p>"

            + "Offer: <strong>" + offerTitle + "</strong> : <br>" + offerDescription + "<br><br>"
            +"<p>Please do get in touch with the user regarding the same, and assist in completing the car purchase and "
            + "availing the offer.<br><br>"
            +"The offer can be claimed only when the user books a corresponding car with this offer at your dealership "
            + "within the offer period when stocks of the offer item are still available.<br><br>"
            +"In case of a customer with a CarWale coupon code booking a car with you, please follow the below steps"
            + "to help in availing the offer:<br><br>"
            + "i) Click on the “Book” link in the lead corresponding to the user’s details in your Autobiz panel.<br>In case you are having difficulty finding the lead, create a fresh lead with the user’s details.<br>"
            + "ii) Fill the required details in the “Booking Details” window.<br>"
            + "iii) In case the offer is applicable for the chosen car, and is still available, a text box for entering <br>coupon code appears at the bottom, along with a “Claim Offer” link.<br>"
            + "iv) On entering the customer’s coupon code in the box and clicking the link, a prompt appears <br>"
            + "In case you are having difficulty finding the lead, create a fresh lead with the user’s details.<br>"
            + "coupon code appears at the bottom, along with a “Claim Offer” link.<br>"
            + "alongside the button with the result. In case the coupon code matches with the available<br> "
            + "offer for the car, a “Right Coupon Code” prompt appears. In case the coupon code doesn’t <br>"
            + "match, a “Sorry. Coupon Code doesn’t match” prompt appears.<br>"
            + "successfully hitting “Save” on the “Booking Details” window.<br>"
            + "v) In case the coupon code is right, the offer gets claimed when the booking is confirmed by <br>"
            + "vi) In case the coupon code didn’t match, the offer doesn’t get claimed.<br> "
            + "vii) Please do communicate to the customer that the delivery of the offer item would happen<br>only when the car delivery process is completed.<br> "
            + "viii) The offer claim stands cancelled on cancellation of Booking.<br>"
            +"<br><br>"
            +"Please do call your CarWale service executive in case of any queries.</p>");
          
            message.Append("<br><br>Regards,<br>");
            message.Append("<b>Team CarWale</b>");

            var email = new EmailEntity()
            {
                Email = dealerEmail,
                Subject = "Carwale Coupon Code For Customer For " + offerTitle,
                Body = message.ToString(),
                ReplyTo = custEmail
            };

            return email;
        }


        public EmailEntity GetOffersPaymentSuccessCustEmailTemplate(string customerName, string couponCode, string carName, string dealerName, string dealerMobile, string dealerAddress, string cutomerMobile, string modelName, string customerEmail, string expiryDate, string dealerEmail)
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
                   "<div style=\"background:#fff; margin:5px 0px 0px 0px\">" +
                       "<div style=\"padding:10px;  border-top:solid 7px #0e3a51;\">" +
                           "<div style=\"float:left; max-width:130px;\">" +
                             "<a target=\"_blank\" style=\"text-decoration:none\" href=\"https://www.carwale.com/?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan\"><img border=\"0\" style=\"width:130px;\" title=\"CarWale\" alt=\"CarWale\" src=\"https://img.carwale.com/Mailer/PQimages/CW-offer-logo.jpg\"></a>" +
                           "</div>" +
                           "<div style=\"clear:both\"></div>" +
                       "</div>" +
                       "<div style=\"background:url(https://img.carwale.com/Mailer/PQimages/offer-shadow.png) repeat-x 0px 0px #fff; padding:0px 0px 0 10px; height:6px;\"></div>" +
                //<!-- Body content part code starts here -->
                       "<div style=\"padding:10px;\">" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">Dear <strong>" + customerName + "</strong></p>" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">Your CarWale coupon code is <strong style=\"font-size:20px;\">" + couponCode + "</strong></p>" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">This coupon code is valid against the booking of <strong>" + carName + "</strong> from</p>" +
                           "<div style=\"border:1px solid #e2e2e2; border-radius:5px; padding:10px; margin-bottom:15px;\">" +
                               "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">" + dealerName + "</p>" +
                               "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">" + dealerAddress + "</p>" +
                               "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333;\">" + dealerMobile + "</p>" +
                           "</div>" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">This offer expires on <strong>" + expiryDate + "</strong> and is a limited quantity offer available only till stock lasts. So please rush to book the car and claim your gift till stocks last.</p>" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 25px 0;\">For any queries, call us at 1800 2090 230.</p>" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:14px; color:#333333; margin:0 0 15px 0; font-weight:bold;\">Terms and Conditions: </p>" +
                           "<ul style=\"list-style:decimal; padding-left:20px; font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; line-height:1.8\">" +
                               "<li style=\"padding-left:5px; margin-bottom:5px; font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333;\">The offer is valid only till the offer expiry date or till stock of offer items last, whichever may occur earlier.</li>" +
                               "<li style=\"padding-left:5px; margin-bottom:5px; font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333;\">The user is responsible for getting the offer claimed at the earliest from their dealership where they had completed their booking to maximize their chances of getting the offer item.</li>" +
                               "<li style=\"padding-left:5px; margin-bottom:5px; font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333;\">The offer is reserved, if available, against the user's name when the booking is done. The offer item shipment happens only when the user takes delivery of the car after final payment.</li>" +
                               "<li style=\"padding-left:5px; margin-bottom:5px; font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333;\">The offer images shown in any communication from CarWale is strictly for illustrative purposes and the original item may differ in appearance.</li>" +
                               "<li style=\"padding-left:5px; margin-bottom:5px; font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333;\">Any claim of the offer is at the discretion of Carwale and CarWale has the right to suspend the offer at any time, without prior intimation. </li>" +
                           "</ul>" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:20px 0 15px 0;\">" +
                               "Regards,<br /><strong>Team CarWale</strong></p></div>" +
                //<!-- Body content part code ends here -->
                   "</div><div style=\"background:url(https://img.carwale.com/Mailer/PQimages/offer-bottom-shadow.jpg) center center no-repeat #eeeeee; height:9px; width:100%\"></div>" +
                   "<div style=\"padding:0px 0px 5px 0px;width:100%\">" +
                       "<div style=\"width:100px; margin:0 auto\">" +
                           "<div style=\" float:left;padding-right:5px;\"><a href=\"https://twitter.com/Carwale/?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan\" target=\"_blank\"><img src=\"https://img.carwale.com/Mailer/PQimages/offer-t-icon.jpg\" alt=\"Twitter\" title=\"Twitter\" border=\"0\"/></a></div>" +
                           "<div style=\"float:left; padding-right:5px;\"><a href=\"https://www.facebook.com/CarWale/?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan\" target=\"_blank\"><img src=\"https://img.carwale.com/Mailer/PQimages/offer-fb-icon.jpg\" alt=\"Facebook\" title=\"Facebook\" border=\"0\" /></a></div>" +
                           "<div style=\"float:left; \"><a href=\"https://plus.google.com/+CarWale/posts?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan\" target=\"_blank\"><img src=\"https://img.carwale.com/Mailer/PQimages/offer-g-icon.jpg\" alt=\"Google+\" title=\"Google+\" border=\"0\" /></a></div>" +
                           "<div style=\"clear:both;\"></div>" +
                       "</div>" +
                       "<div style=\"clear:both;\"></div>" +
                   "</div></div></body></html>");

            var email = new EmailEntity()
            {
                Email = customerEmail,
                Subject = "Carwale offer Coupon Code For " + carName,
                Body = message.ToString(),
                ReplyTo = dealerEmail
            };

            return email;
        }

        public EmailEntity GetOffersPaymentSuccessDealerEmailTemplate(string custName, string custEmail, string custMobile, string couponCode, string carName, string dealerEmail, string offerTitle, string offerDescription)
        {
            StringBuilder message = new StringBuilder();


            message.Append("<p>Dear Dealer Partner,<br><br>"

            + "A user has expressed interest in the " + offerTitle + " offer running against the " + carName + "<br><br>"

            + ".Given below are the details for the same.<br><br>"

            + "User Name: " + custName + "<br><br>"

            + "Email Id: " + custEmail + "<br><br>"

            + "Mobile number: " + custMobile + "<br><br>"

            + "Car: " + carName + "<br><br>"

            + "Offer: <strong>" + offerTitle + "</strong> : <br>" + offerDescription + "<br><br>"
            + "Please do get in touch with the user regarding the same, and assist in completing the car purchase and "
            + "availing the offer.<br><br>"
            + "The offer can be claimed only when the user books a corresponding car with this offer at your dealership "
            + "within the offer period when stocks of the offer item are still available.<br><br>"
            + "In case of a customer with a CarWale coupon code booking a car with you, please follow the below steps"
            + "to help in availing the offer:<br><br>"
            + "i) Click on the “Book” link in the lead corresponding to the user’s details in your Autobiz panel.<br>In case you are having difficulty finding the lead, create a fresh lead with the user’s details.<br>"
            + "ii) Fill the required details in the “Booking Details” window.<br>"
            + "iii) In case the offer is applicable for the chosen car, and is still available, a text box for entering <br>coupon code appears at the bottom, along with a “Claim Offer” link.<br>"
            + "iv) On entering the customer’s coupon code in the box and clicking the link, a prompt appears<br>"
            + "alongside the button with the result. In case the coupon code matches with the available <br>"
            + "offer for the car, a “Right Coupon Code” prompt appears. In case the coupon code doesn’t <br>"
            + "match, a “Sorry. Coupon Code doesn’t match” prompt appears.<br>"
            + "v) In case the coupon code is right, the offer gets claimed when the booking is confirmed by <br>"
            + "vi) In case the coupon code didn’t match, the offer doesn’t get claimed.<br> "
            + "vii) Please do communicate to the customer that the delivery of the offer item would happen<br>only when the car delivery process is completed.<br> "
            + "viii) The offer claim stands cancelled on cancellation of Booking.<br>"
            + "<br><br>"
            + "Please do call your CarWale service executive in case of any queries.</p>");

            message.Append("<br><br>Regards,<br>");
            message.Append("<b>Team CarWale</b>");

            var email = new EmailEntity()
            {
                Email = dealerEmail,
                Subject = "Carwale Coupon Code For Customer For " + offerTitle,
                Body = message.ToString(),
                ReplyTo = custEmail
            };

            return email;
        }


        public EmailEntity GetOffersBookingSuccessCustomerEmailTemplate(string custName, string custEmail, string dealerName, string dealerEmail, string dealerMobile,string dealerAddr, string carName,string offerTitle,int bookRefId)
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
                   "<div style=\"background:#fff; margin:5px 0px 0px 0px\">" +
                       "<div style=\"padding:10px;  border-top:solid 7px #0e3a51;\">" +
                           "<div style=\"float:left; max-width:130px;\">" +
                             "<a target=\"_blank\" style=\"text-decoration:none\" href=\"https://www.carwale.com/?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan\"><img border=\"0\" style=\"width:130px;\" title=\"CarWale\" alt=\"CarWale\" src=\"https://img.carwale.com/Mailer/PQimages/CW-offer-logo.jpg\"></a>" +
                           "</div>" +
                           "<div style=\"clear:both\"></div>" +
                       "</div>" +
                       "<div style=\"background:url(https://img.carwale.com/Mailer/PQimages/offer-shadow.png) repeat-x 0px 0px #fff; padding:0px 0px 0 10px; height:6px;\"></div>" +
                //<!-- Body content part code starts here -->
                       "<div style=\"padding:10px;\">" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">Dear <strong>" + custName + "</strong></p>" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">Your CarWale Pre-Booking Reference Number is <strong style=\"font-size:20px;\">" + bookRefId + "</strong></p>" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">This is against your pre-booking of  <strong>" + carName + "</strong></p><br>" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">By pre-booking this car the offer "+offerTitle+" has been reserved against your name. This reservation would be confirmed once you complete the booking formalities and pay the rest of the booking amount by visiting.</p>" +
                           "<div style=\"border:1px solid #e2e2e2; border-radius:5px; padding:10px; margin-bottom:15px;\">" +
                               "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">" + dealerName + "</p>" +
                               "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">" + dealerAddr + "</p>" +
                               "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333;\">" + dealerMobile + "</p>" +
                           "</div>" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">This reservation is valid only for the next 15 days, so please complete your booking formalities at the earliest, in the absence of which the pre-booking would stand cancelled, and the pre-booking amount would get <br>refunded to you as per the " + "<a href=\"https://www.carwale.com/refundpolicy.aspx\" target=\"_blank\">Refund Policy</a>" + "</p>" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 15px 0;\">The booking process ensures the offer item gets reserved against your name. The item would then be shipped to you once you take delivery of the car from the dealership..</p>" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:0 0 25px 0;\">For any queries, call us at 1800 2090 230.</p>" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:14px; color:#333333; margin:0 0 15px 0; font-weight:bold;\"><a href=\"https://www.carwale.com/offerTermsAndConditions.aspx\" target=\"_blank\">Terms And Conditions</a></p>" +
                           "<p style=\"font-family: Arial, Helvetica, sans-serif; font-size:12px; color:#333333; margin:20px 0 15px 0;\">" +
                               "Regards,<br /><strong>Team CarWale</strong></p></div>" +
                //<!-- Body content part code ends here -->
                   "</div><div style=\"background:url(https://img.carwale.com/Mailer/PQimages/offer-bottom-shadow.jpg) center center no-repeat #eeeeee; height:9px; width:100%\"></div>" +
                   "<div style=\"padding:0px 0px 5px 0px;width:100%\">" +
                       "<div style=\"width:100px; margin:0 auto\">" +
                           "<div style=\" float:left;padding-right:5px;\"><a href=\"https://twitter.com/Carwale/?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan\" target=\"_blank\"><img src=\"https://img.carwale.com/Mailer/PQimages/offer-t-icon.jpg\" alt=\"Twitter\" title=\"Twitter\" border=\"0\"/></a></div>" +
                           "<div style=\"float:left; padding-right:5px;\"><a href=\"https://www.facebook.com/CarWale/?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan\" target=\"_blank\"><img src=\"https://img.carwale.com/Mailer/PQimages/offer-fb-icon.jpg\" alt=\"Facebook\" title=\"Facebook\" border=\"0\" /></a></div>" +
                           "<div style=\"float:left; \"><a href=\"https://plus.google.com/+CarWale/posts?utm_source=CouponEmailer&utm_medium=email&utm_campaign=CouponDecJan\" target=\"_blank\"><img src=\"https://img.carwale.com/Mailer/PQimages/offer-g-icon.jpg\" alt=\"Google+\" title=\"Google+\" border=\"0\" /></a></div>" +
                           "<div style=\"clear:both;\"></div>" +
                       "</div>" +
                       "<div style=\"clear:both;\"></div>" +
                   "</div></div></body></html>");

            var email = new EmailEntity()
            {
                Email = custEmail,
                Subject = " Congratulations on pre-booking the "+carName+" and reserving your offer",
                Body = message.ToString(),
                ReplyTo = dealerEmail
            };

            return email;
        }

        public EmailEntity GetOffersBookingSuccessDealerEmailTemplate(string name, string mobile, string custEmail, string dealerEmail, string carName, string offerTitle ,string offerDesc)
        {
            StringBuilder message = new StringBuilder();


            message.Append("Dear Dealer Partner,<br><br>"
            + "A user has pre-booked the " + carName + " by paying a pre-booking amount of Rs.5000 to CarWale. CarWale has collected this pre-<br>booking amount on behalf of your dealership. This pre-booking makes the user eligible to claim the " + offerTitle + " offer, if the user completes<br> the rest of the booking formalities within 15 days. Given below are the pre-booking details.<br><br>"
            + "<b>User Name:</b> " + name + "<br><br>"
            + "<b>Email Id:</b> " + custEmail + "<br><br>"
            + "<b>Mobile number</b>: " + mobile + "<br><br>"
            + "<b>Car</b>: " + carName + "<br><br>"
            + "<b>Offer</b>: " + offerTitle + " - " + offerDesc + "<br><br>"
            + "Please do get in touch with the user and assist him with completing his booking.<br><br>"
            + "The offer can be claimed only when the user completes the rest of the booking formalities for a corresponding car at your dealership. CarWale <br>would credit you the pre-booking amount of Rs.5000 as soon as the customer completes his booking with you. For this, please do intimate <br>your CarWale service executive of any such pre-booking customers you receive.<br><br>"
            + "Please do call your CarWale service executive in case of any queries.");

            message.Append("<br><br>Regards,<br>");
            message.Append("<b>Team CarWale</b>");

            var email = new EmailEntity()
            {
                Email = dealerEmail,
                Subject = name+" has pre-booked "+carName+" through CarWale",
                Body = message.ToString(),
                ReplyTo = custEmail
            };

            return email;
        }
    }
}
