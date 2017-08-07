using BikewaleOpr.Entity.Dealers;
using System.Collections.Generic;

namespace BikewaleOpr.Models.DealerPricing
{
    public class DealerPricingSheetPageVM
    {
        public string PageTitle { get; set; }
        public string PageHeader { get; set; }
        public uint CityId { get; set; }
        public uint MakeId { get; set; }
        public uint DealerId { get; set; }
        public uint OtherCityId { get; set; }
        public IEnumerable<VersionPriceEntity> dealerVersionCategories { get; set; }
        public DealerPriceSheetVM DealerPriceSheet { get; set; }
        public DealerOperationPricingVM DealerOperationParams { get; set; }
        public ShowPricingVM ShowPricingCities { get; set; }
        public AddCategoryVM AddCategoryType { get; set; }
        public CityCopyPricingVM CopyPricingCities { get; set; }
        public DealerCopyPricingVM CopyPricingDealers { get; set; }
    }
}
