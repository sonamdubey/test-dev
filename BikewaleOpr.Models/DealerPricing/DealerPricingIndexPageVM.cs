namespace BikewaleOpr.Models.DealerPricing
{
    public class DealerPricingIndexPageVM
    {
        public string PageTitle = "Dealer Pricing Management";
        public string PageHeader = "Dealer Pricing Management";
        public DealerOperationVM DealerOperationParams { get; set; }
        public ShowPricingVM ShowPricingCities { get; set; }
        public AddCategoryVM AddCategoryType { get; set; }
        public CityCopyPricingVM CopyPricingCities { get; set; }
        public DealerCopyPricingVM CopyPricingDealers { get; set; }
    }
}
