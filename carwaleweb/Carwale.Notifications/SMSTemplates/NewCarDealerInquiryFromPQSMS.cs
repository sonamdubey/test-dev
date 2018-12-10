using Carwale.Entity.Enum;
using Carwale.Entity.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Notifications.SMSTemplates
{
    public class NewCarDealerInquiryFromPQSMS
    {
        public SMS GetSMSTemplateDealer(string customerName, string cutomerMobile, string modelName, string pageUrl, string dealerMobile, string AssistanceType)
        {
            string message = customerName + " (" + cutomerMobile + ") has requested for " + AssistanceType + " assistance for " + modelName + " on " + pageUrl;
            var sms = new SMS()
            {
                Message = message,
                Mobile = dealerMobile,
                ReturnedMsg = "",
                Status = true,
                SMSType = Convert.ToInt32(SMSType.NewCarQuote),
                PageUrl = pageUrl
            };

            return sms;
        }

        public SMS GetSMSTemplateCustomer(string custMobile, string dealerAdd, string dealerMobile, string dealerTitle)
        {
            string message = "Contact "+ dealerTitle+ " at " + dealerMobile+ " or visit at "+ dealerAdd +" for further help.";
            var sms = new SMS()
            {
                Message = message,
                Mobile = custMobile,
                ReturnedMsg = "",
                Status = true,
                SMSType = Convert.ToInt32(SMSType.NewCarQuote),
                PageUrl = "Carwale.com"
            };

            return sms;
        }
    }
}
