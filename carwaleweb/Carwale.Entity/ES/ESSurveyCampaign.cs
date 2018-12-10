using System;

namespace Carwale.Entity.ES
{
    [Serializable]
    public class ESSurveyCampaign
    {
        public int Id { get; set; }
        public DateTime EndDate { get; set; }
        public string GATrackingCategory { get; set; }
        public bool ShowLeadForm { get; set; }
        public string ThankYouText { get; set; }
    }
}
