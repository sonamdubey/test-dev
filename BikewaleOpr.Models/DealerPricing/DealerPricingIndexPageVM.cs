namespace BikewaleOpr.Models.DealerPricing
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
    /// Description :   View model for dealer pricing management index page
    /// </summary>
    public class DealerPricingIndexPageVM
    {
        public string PageTitle { get; set; }
        public DealerOperationVM DealerOperationParams { get; set; }
        public ShowPricingVM ShowPricingCities { get; set; }
        public AddCategoryVM AddCategoryType { get; set; }
        public CityCopyPricingVM CopyPricingCities { get; set; }
        public DealerCopyPricingVM CopyPricingDealers { get; set; }
    }
}
