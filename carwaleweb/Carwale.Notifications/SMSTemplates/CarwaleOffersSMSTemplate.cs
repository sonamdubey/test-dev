using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Carwale.Entity.Notifications;
using Carwale.Entity.Enum;

namespace Carwale.Notifications.SMSTemplates
{
    public class CarwaleOffersSMSTemplate
    {
        
        public SMS GetOffersCustSMSTemplate(string customerName, string custMobile, string couponCode, string carName,string dealerName,string dealerPhone, string dealerArea, string dealerAddress, string expiryDate)
        {
            string message = "Your coupon code is " + couponCode + " on booking of " + carName + " from " + dealerName + "," + dealerArea + " (" + dealerPhone + "). Valid till " + expiryDate + " or stocks last";
            
            var sms = new SMS()
            {
              Message = message,
              Mobile = custMobile,
              ReturnedMsg = "",
              Status = true,
              SMSType = Convert.ToInt32(SMSType.PQOffer),
              PageUrl = "CWPQOffer"
            };

            return sms;
        }

        public SMS GetOffersDealerSMSTemplate(string customerName, string custMobile,string custEmail, string carName, string dealerMobile, string offerTitle)
        {
            string message = "Coupon code generated for " + offerTitle + " by " + customerName + ", " + custEmail + ", " + custMobile + " for " + carName ;
            var sms = new SMS()
            {
                Message = message,
                Mobile = dealerMobile,
                ReturnedMsg = "",
                Status = true,
                SMSType = Convert.ToInt32(SMSType.PQOffer),
                PageUrl = "CWPQOffer"
            };

            return sms;
        }


        public SMS GetOffersPaymentSuccessCustSMSTemplate(string customerName, string custMobile, string couponCode, string carName, string dealerName, string dealerPhone, string dealerArea, string dealerAddress, string expiryDate)
        {
            string message = "Your coupon code is " + couponCode + " on booking of " + carName + " from " + dealerName + "," + dealerArea + " (" + dealerPhone + "). Valid till " + expiryDate + " or stocks last";

            var sms = new SMS()
            {
                Message = message,
                Mobile = custMobile,
                ReturnedMsg = "",
                Status = true,
                SMSType = Convert.ToInt32(SMSType.PQOffer),
                PageUrl = "CWPQOffer"
            };

            return sms;
        }

        public SMS GetOffersDealerPaymentSuccessSMSTemplate(string customerName, string custMobile, string custEmail, string carName, string dealerMobile, string offerTitle)
        {
            string message = "Coupon code generated for " + offerTitle + " by " + customerName + ", " + custEmail + ", " + custMobile + " for " + carName;
            var sms = new SMS()
            {
                Message = message,
                Mobile = dealerMobile,
                ReturnedMsg = "",
                Status = true,
                SMSType = Convert.ToInt32(SMSType.PQOffer),
                PageUrl = "CWPQOffer"
            };

            return sms;
        }

        public SMS SendBookingSmsCustomer(string mobile, string carName, string dealerName)
        {
            string message = "Your offer has been reserved on your pre-booking the "+carName+". Please proceed to "+dealerName+ " to complete your booking.";

            var sms = new SMS()
            {
                Message = message,
                Mobile = mobile,
                ReturnedMsg = "",
                Status = true,
                SMSType = Convert.ToInt32(SMSType.PQOffer),
                PageUrl = "CWPQOffer"
            };

            return sms;
        }

        public SMS SendBookingSmsDealer(string name, string mobile, string email, string dealerMobile, string carName)
        {
            string message = "Pre-booking has been done by "+name+", "+mobile+", "+email+ " for "+carName+" by paying Rs.5000";

            var sms = new SMS()
            {
                Message = message,
                Mobile = dealerMobile,
                ReturnedMsg = "",
                Status = true,
                SMSType = Convert.ToInt32(SMSType.PQOffer),
                PageUrl = "CWPQOffer"
            };

            return sms;
        }
    }
}
