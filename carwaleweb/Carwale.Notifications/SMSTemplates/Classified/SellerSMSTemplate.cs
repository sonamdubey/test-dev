using Carwale.Entity.Enum;
using Carwale.Entity.Notifications;
using Carwale.Utility;

namespace Carwale.Notifications.SMSTemplates.Classified
{
    public class SellerSMSTemplate
    {
        public static SMS GetSellCarSMSTemplate(string mobile, string carName, string pageUrl, Platform platform)
        {
            SMS sms = null;
            if (!string.IsNullOrEmpty(mobile) && !string.IsNullOrWhiteSpace(carName))
            {
                string message = "Your ad for " + carName + " is now live. You will receive enquiries via SMS and email. Thank you for choosing CarWale.";
                sms = new SMS()
                {
                    Message = message,
                    Mobile = mobile,
                    ReturnedMsg = "",
                    Status = true,
                    SMSType = (int)SMSType.SellCarLiveNotification,
                    PageUrl = pageUrl,
                    SourceModule = SMSType.SellCarLiveNotification,
                    Platform = platform,
                    IpAddress = UserTracker.GetUserIp().Split(',')[0],
            };
            }
            return sms;
        }

    }
}
