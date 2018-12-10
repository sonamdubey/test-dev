using FluentValidation.Attributes;

namespace Carwale.Entity.Classified.Leads
{
    public class Lead
    {
        public string ProfileId { get; set; }
        public Buyer Buyer { get; set; }
        public LeadTrackingParams LeadTrackingParams { get; set; }
        public bool IsChatSms { get; set; }
    }
}