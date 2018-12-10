using Carwale.Entity.Enum;

namespace Carwale.Entity.Classified.Leads
{
    public class LeadNotificationData
    {
        public LeadStockSummary Stock { get; set; }
        public Buyer Buyer { get; set; }
        public Seller Seller { get; set; }
        public Platform LeadSource { get; set; }
    }
}
