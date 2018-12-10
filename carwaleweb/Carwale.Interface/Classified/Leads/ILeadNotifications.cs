using Carwale.Entity.Classified.Leads;

namespace Carwale.Interfaces.Classified.Leads
{
    public interface ILeadNotifications
    {
        void SendEmailToBuyer(LeadNotificationData lead);
        void SendEmailToSeller(LeadNotificationData lead);
        void SendSMSToBuyer(LeadNotificationData lead);
        void SendSMSToSeller(LeadNotificationData lead);
    }
}