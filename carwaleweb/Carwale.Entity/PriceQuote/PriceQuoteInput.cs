namespace Carwale.Entity.Price
{
    public class PriceQuoteInput
    {
        public int ModelId { get; set; }
        public int VersionId { get; set; }
        public int CityId { get; set; }
        public int AreaId { get; set; }
        public bool IsCrossSellPriceQuote { get; set; }
        public int PageId { get; set; }
        public bool HideCampaign { get; set; }
    }
}
