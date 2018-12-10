namespace Carwale.Entity.PriceQuote
{
    public class ModelSimilarPriceDetail
    {
        public int ModelId { get; set; }
        public int RefreshCount { get; set; }
        public bool IsPricesRefreshed { get; set; }
        public bool CanResetRefreshCount { get; set; }
        public string AvailableOn { get; set; }
        public string RefreshedOn { get; set; }
    }
}
