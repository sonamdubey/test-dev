using Carwale.Entity.Classified.Leads;
using Carwale.Entity.Enum;

namespace Carwale.Interfaces.Classified.Leads
{
    public interface ILeadBL
    {
        LeadReport ProcessLead(LeadDetail lead);
        void SendLeadNotifications(int leadId, LeadStockSummary stock, Buyer buyer, Seller seller, Platform leadSource);
        Seller GetSeller(int inquiryId, bool isDealer);
    }
}
