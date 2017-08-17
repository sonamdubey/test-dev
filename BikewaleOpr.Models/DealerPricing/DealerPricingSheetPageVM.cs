using BikewaleOpr.Entity.Dealers;
using System.Collections.Generic;

namespace BikewaleOpr.Models.DealerPricing
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 11 Aug 2017
    /// Description :   View model for dealer pricing management pricing page
    /// </summary>
    public class DealerPricingSheetPageVM
    {
        public string PageTitle { get; set; }
        public uint CityId { get; set; }
        public uint MakeId { get; set; }
        public uint DealerId { get; set; }
        public uint OtherCityId { get; set; }
        public uint CurrentCityId { get { return (OtherCityId > 0 ? OtherCityId : CityId); } }
        public uint EnteredBy { get; set; }
        public IEnumerable<VersionPriceEntity> DealerVersionCategories { get; set; }
        public string SelectedCategoriesString { get; set; }
        public DealerPriceSheetVM DealerPriceSheet { get; set; }
        public DealerOperationPricingVM DealerOperationParams { get; set; }
        public ShowPricingVM ShowPricingCities { get; set; }
        public AddCategoryVM AddCategoryType { get; set; }
        public CityCopyPricingVM CopyPricingCities { get; set; }
        public DealerCopyPricingVM CopyPricingDealers { get; set; }
    }
}
