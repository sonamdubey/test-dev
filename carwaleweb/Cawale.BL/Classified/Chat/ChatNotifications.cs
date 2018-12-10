using Carwale.Entity.Enum;
using Carwale.Entity.Notifications;
using Carwale.Interfaces.Classified.Chat;
using Carwale.Interfaces.Template;
using Carwale.Notifications.Interface;
using Carwale.Notifications.Logs;
using Carwale.Utility;
using System;
using System.Configuration;

namespace Carwale.BL.Classified.Chat
{
    public class ChatNotifications : IChatNotifications
    {
        private readonly ITemplateRender _templateRenderer;
        private readonly ISmsLogic _smsLogic;
        private static readonly int _buyerSMSTemplateId = Convert.ToInt32(ConfigurationManager.AppSettings["UsedChatBuyerSMSTemplateId"]);
        private static readonly int _sellerSMSTemplateId = Convert.ToInt32(ConfigurationManager.AppSettings["UsedChatSellerSMSTemplateId"]);

        public ChatNotifications(ITemplateRender templateRenderer, ISmsLogic smsLogic)
        {
            _templateRenderer = templateRenderer;
            _smsLogic = smsLogic;
        }
        public bool SendSmsToChatUser(string mobile, string shortUrl, bool isBuyerToSellerChat)
        {
            string message;
            SMSType smsType;
            var isSmsSent = true;
            try
            {
                if (isBuyerToSellerChat)
                {
                    message = _templateRenderer.Render(_sellerSMSTemplateId, shortUrl);
                    smsType = SMSType.ChatFallbackSMSToDealer;
                }
                else
                {
                    message = _templateRenderer.Render(_buyerSMSTemplateId, shortUrl);
                    smsType = SMSType.ChatFallbackSMSToUser;
                }
                _smsLogic.Send(new SMS {
                    Mobile = mobile,
                    Message = message,
                    SourceModule = smsType,
                    Platform = Platform.CarwaleDesktop,
                    IpAddress = UserTracker.GetUserIp().Split(',')[0]
                });
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                isSmsSent = false;
            }
            return isSmsSent;
        }
    }
}
