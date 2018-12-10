namespace Carwale.Entity.Classified.Leads
{
    public class LeadTrackingParams
    {
        public int OriginId { get; set; }
        public string Rank { get; set; }
        public int DeliveryCity { get; set; }
        public LeadType LeadType { get; set; }
        public string QueryString { get; set; } = "-1";  //default value to know whether value passed in request body or not
        public double Latitude { get; set; } = -100;
        public double Longitude { get; set; } = -200;
        public int SlotId { get; set; }
    }
}
