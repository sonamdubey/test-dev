using Carwale.Entity.Classified.Leads;
using Carwale.Entity.Enum;
using Carwale.Entity.Notifications;
using Carwale.Interfaces.Classified.Leads;
using Carwale.Interfaces.Notifications;
using Carwale.Interfaces.Template;
using Carwale.Notifications.Interface;
using Carwale.Utility;
using System;
using System.Configuration;

namespace Carwale.BL.Classified.Leads
{
    public class LeadNotifications : ILeadNotifications
    {
        private readonly ITemplateRender _templateRenderer;
        private readonly IEmailNotifications _emailNotifications;
        private readonly ISmsLogic _smsLogic;

        private static readonly int _buyerEmailTemplateId = Convert.ToInt32(ConfigurationManager.AppSettings["UsedLeadBuyerEmailTemplateId"]);
        private static readonly int _dealerEmailTemplateId = Convert.ToInt32(ConfigurationManager.AppSettings["UsedLeadDealerEmailTemplateId"]);
        private static readonly int _indEmailTemplateId = Convert.ToInt32(ConfigurationManager.AppSettings["UsedLeadIndEmailTemplateId"]);
        private static readonly int _buyerSMSTemplateId = Convert.ToInt32(ConfigurationManager.AppSettings["UsedLeadBuyerSMSTemplateId"]);
        private static readonly int _sellerSMSTemplateId = Convert.ToInt32(ConfigurationManager.AppSettings["UsedLeadSellerSMSTemplateId"]);

        public LeadNotifications(ITemplateRender templateRenderer, IEmailNotifications emailNotifications, ISmsLogic smsLogic)
        {
            _templateRenderer = templateRenderer;
            _emailNotifications = emailNotifications;
            _smsLogic = smsLogic;
        }

        public void SendEmailToBuyer(LeadNotificationData lead)
        {
            string subject = "Seller Details of Car #" + lead.Stock.ProfileId;
            string body = _templateRenderer.Render(_buyerEmailTemplateId, lead);

            if (!string.IsNullOrWhiteSpace(body))
            {
                _emailNotifications.SendMail(lead.Buyer.Email, subject, body);
            }
        }

        public void SendEmailToSeller(LeadNotificationData lead)
        {
            string subject = "Purchase Request Arrived : Car #" + lead.Stock.ProfileId;
            string body = _templateRenderer.Render(lead.Stock.IsDealer ? _dealerEmailTemplateId : _indEmailTemplateId, lead);

            if (!string.IsNullOrWhiteSpace(body))
            {
                _emailNotifications.SendMail(lead.Seller.Email, subject, body);
            }
        }

        public void SendSMSToBuyer(LeadNotificationData lead)
        {
            SMSType smsType = lead.Stock.IsDealer ? SMSType.UsedPurchaseInquiryDealerBuyer : SMSType.UsedPurchaseInquiryIndividualBuyer;
            string message = _templateRenderer.Render(_buyerSMSTemplateId, lead);

            if (!string.IsNullOrWhiteSpace(message))
            {
                _smsLogic.Send(new SMS
                {
                    Mobile = lead.Buyer.Mobile,
                    Message = message,
                    SourceModule = smsType,
                    Platform = lead.LeadSource,
                    IpAddress = UserTracker.GetUserIp().Split(',')[0]
                });
            }
        }

        public void SendSMSToSeller(LeadNotificationData lead)
        {
            SMSType smsType = lead.Stock.IsDealer ? SMSType.UsedPurchaseInquiryDealerSeller : SMSType.UsedPurchaseInquiryIndividualSeller;     
            string message = _templateRenderer.Render(_sellerSMSTemplateId, lead);
            if (!string.IsNullOrWhiteSpace(message))
            {
                if (string.IsNullOrWhiteSpace(lead.Buyer.Name))
                {
                    message = $"Buyer{message}";
                }
                _smsLogic.Send(new SMS
                {
                    Mobile = lead.Seller.Mobile,
                    Message = message,
                    SourceModule = smsType,
                    Platform = lead.LeadSource,
                    IpAddress = UserTracker.GetUserIp().Split(',')[0]
                });
            }
        }
    }
}
