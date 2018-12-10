namespace Carwale.DTOs.Classified.CarValuation
{
    public class ValuationReportAmp
    {
        public string Make { get; set; }
        public string Model { get; set; }
        public string Version { get; set; }
        public short Year { get; set; }
        public string City { get; set; }
        public string Fuel { get; set; }
        public string SellerAskingPrice { get; set; }
        public int? IndicatorPosition { get; set; }
        public ValuationAmp Valuation { get; set; }
        public string DealerRatingText { get; set; }
    }
}
