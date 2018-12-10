using Carwale.Entity.Classified.Leads;

namespace Carwale.DTOs.Classified.Leads
{
    public class LeadResponse
    {
        public SellerDTO Seller { get; set; }
        public string CertificationReportUrl { get; set; }
        public LeadStockSummary Stock { get; set; }
        public BuyerDTO Buyer { get; set; }
        public string AppId { get; set; } // Chat Application key
        public string WhatsAppMessage { get; set; }
    }
}
