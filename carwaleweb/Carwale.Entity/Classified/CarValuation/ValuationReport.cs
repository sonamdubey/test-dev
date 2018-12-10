namespace Carwale.Entity.Classified.CarValuation
{
    public class ValuationReport
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string Version { get; set; }
        public short Year { get; set; }
        public int CityId { get; set; }
        public string City { get; set; }
        public string Fuel { get; set; }
        public int? SellerAskingPrice { get; set; }
        public Valuation Valuation { get; set; }
        public string DealerRatingText { get; set; }
        public bool IsChatAvailable { get; set; }
    }
}
