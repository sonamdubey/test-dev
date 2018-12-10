using Carwale.Entity.Enum;
using Carwale.Entity.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Carwale.Notifications.SMSTemplates
{
    public class OTPVerificationTemplate
    {
        public SMS GetOTPVerificationTemplate(string custMobile, string cwiCode, string pageUrl)
        {
            string message = cwiCode + " is the Verification code for registering your mobile no with CarWale. This is a one-time registration process.";

            var sms = new SMS()
            {
                Message = message,
                Mobile = custMobile,
                ReturnedMsg = "",
                Status = true,
                SMSType = Convert.ToInt32(SMSType.MobileVerification),
                PageUrl = pageUrl
            };

            return sms;
        }
    }
}
