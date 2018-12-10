using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Deals;
using Carwale.DTOs.LeadForm;
using Carwale.DTOs.OfferAndDealerAd;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.Dealers;
using Carwale.Entity.Geolocation;
using Carwale.Entity.PriceQuote;
using Carwale.Entity.UserProfiling;
using System.Collections.Generic;

namespace Carwale.DTOs.NewCars
{
    public class PriceInCityPageDTO
    {
        public CarModelDetails ModelDetails { get; set; }
        public List<ModelColors> ModelColors { get; set; }
        public SimilarCarsDTO SimilarCars { get; set; }
        public PageMetaTags PageMetaTags { get; set; }
        public string Summary { get; set; }
        public DealsStockDTO AdvantageAdData { get; set; }
        public UsedCarCount UsedCarsCount { get; set; }
        public string ShowAssistancePopup { get; set; }
        public List<StockSummary> UsedLuxuryCars { get; set; }
        public DealerAdDTO DealerAd { get; set; }
        public int NewCarDealersCount { get; set; }
        public NewCarDealerEntiy DealersList { get; set; }
        public SubNavigationDTO SubNavigation { get; set; }
        public CarOverviewDTOV2 MobileOverviewDetails { get; set; }
        public CarOverviewDTO DesktopOverviewDetails { get; set; }
        public PriceOverview CarPriceOverview { get; set; }
        public List<BreadcrumbEntity> BreadcrumbEntitylist { get; set; }
        public CallSlugDTO CallSlugInfo { get; set; }
        public List<ModelImage> ModelPhotosListCarousel { get; set; }
        public ModelMenuDTO ModelMenu { get; set; }
        public List<CarVersionDetails> SelectedVersions { get; set; }
        //namespace because conflict with Carwale.Entity.Dealers namespace
        public Carwale.DTO.Dealers.DealerDetails DealerDetails { get; set; }
        public int VersionWidgetSource { get; set; }
        public List<VideoDTO> ModelVideos { get; set; }
        public CarVersionsDTO CarVersion { get; set; }
        public List<NewCarVersionsDTOV2> NewCarVersions { get; set; }
        public bool ShowCampaignLink { get; set; }
        public bool IsRenaultLeadCampaign { get; set; }
        public OfferAndDealerAdDTO OfferAndDealerAd { get; set; }
        public VariantListDTO VariantList { get; set; }
        public EmiCalculatorModelData EmiCalculatorModelData { get; set; }
        public LeadFormModelData LeadFormModelData { get; set; }
        public bool ShowEmiCalculator { get; set; }
    }
}
