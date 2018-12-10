using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Deals;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.OfferAndDealerAd;
using Carwale.DTOs.LeadForm;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.CompareCars;
using Carwale.Entity.Dealers;
using Carwale.Entity.PriceQuote;
using System.Collections.Generic;

namespace Carwale.DTOs.NewCars
{
    /// <summary>
    /// Created By : Shalini on 30/12/14
    /// </summary>
    public class VersionPageDTO_Mobile
    {
        public CarVersionDetails VersionDetails { get; set; }
        public CCarDataDto VersionData { get; set; }
        public CarModelDetails ModelDetails { get; set; }
        public int ModelPhotosCount { get; set; }
        public bool OfferExists { get; set; }
        public CarOverviewDTOV2 MobileOverviewDetails { get; set; }
        public CallSlugDTO CallSlugInfo { get; set; }
        public PriceOverview CarPriceOverview { get; set; }
        public DealsStockDTO AdvantageAdData { get; set; }
        public List<NewCarVersionsDTOV2> NewCarVersions { get; set; }
        public SimilarCarsDTO SimilarCars { get; set; }
        public List<ModelImage> ModelPhotosListCarousel { get; set; }
        public List<VideoDTO> ModelVideos { get; set; }
        public bool IsOtherVariantPriceAvailable { get; set; }
        public bool IsRenaultLeadCampaign { get; set; }
        public bool ShowCampaignLink { get; set; }
		public CitiesDTO CityDetails { get; set; }
        public bool ShowTyresLink { get; set; }
        public OfferAndDealerAdDTO OfferAndDealerAd { get; set; }
        public UpcomingCarModel UpcomingCarDetails { get; set; }
        public DealerAdDTO DealerAd { get; set; }
        public LeadFormModelData LeadFormModelData { get; set; }
    }
}
