using Carwale.Entity.Enum;

namespace Carwale.Entity.Classified.Leads
{
    public class LeadDetail
    {
        public LeadStockSummary Stock { get; set; }
        public Buyer Buyer { get; set; }
        public Platform LeadSource { get; set; }
        public LeadTrackingParams LeadTrackingParams { get; set; }
        public string IPAddress { get; set; }
        public string IMEICode { get; set; }
        public int AppVersion { get; set; }
        public string LTSrc { get; set; }
        public string Cwc { get; set; }
        public string UtmaCookie { get; set; }
        public string UtmzCookie { get; set; }
        public string CWUtmzCookie { get; set; }
        public string AbTestCookie { get; set; }
        public bool IsLeadFromChatSms { get; set; }
        public bool IsVerified { get; set; }
    }
}
