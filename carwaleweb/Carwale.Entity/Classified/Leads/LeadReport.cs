namespace Carwale.Entity.Classified.Leads
{
    public class LeadReport
    {
        public LeadStatus Status { get; set; }
        public int LeadId { get; set; }
        public Seller Seller { get; set; }
        public BuyerInfo BuyerInfo { get; set; }
    }
}
