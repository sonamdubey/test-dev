using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Deals;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.OfferAndDealerAd;
using Carwale.DTOs.PriceQuote;
using Carwale.DTOs.LeadForm;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.CMS.UserReviews;
using Carwale.Entity.Dealers;
using Carwale.Entity.PriceQuote;
using System.Collections.Generic;

namespace Carwale.DTOs.NewCars
{
    /// <summary>
    /// Created By : Shalini on 30/10/14
    /// </summary>
    public class ModelPageDTO_Mobile
    {
        public CarModelDetails ModelDetails { get; set; }
        public List<ModelColors> ModelColors { get; set; }
        public SimilarCarsDTO SimilarCars { get; set; }
        public ModelReview ModelReview { get; set; }
        public List<UserReviewEntity> ModelUserReviews { get; set; }
        public List<ArticleSummary> ExpertReviews { get; set; }
        public List<NewCarVersionsDTOV2> NewCarVersions { get; set; }
        public UpcomingCarModel UpcomingCarDetails { get; set; }
        public List<ModelImage> ModelPhotosList { get; set; }
        public List<ModelImage> ModelPhotosListCarousel { get; set; }
        public PageMetaTags PageMetaTags { get; set; }
        public string Title { get; set; }
        public List<Video> ModelVideos { get; set; }
        public List<CarVersionDetails> SelectedVersions { get; set; }
        public CarOverviewDTOV2 MobileOverviewDetails { get; set; }
        public CallSlugDTO CallSlugInfo { get; set; }
        public PriceOverview CarPriceOverview { get; set; }
        public DealsStockDTO AdvantageAdData { get; set; }
        public List<ArticleSummary> TopNews { get; set; }
        public CarVersionsDTO CarVersion { get; set; }
        public ModelMenuDTO ModelMenu { get; set; }
        public List<MileageDataEntity> MileageData { get; set; }
        public CarSynopsisEntity CarSynopsis { get; set; }
        public DealerAdDTO DealerAd { get; set; }
        public string Summary { get; set; }
        public int Rank { get; set; }
        public int NoOfTopCarsInBodyType { get; set; }
        public UpcomingCarModel UpgradedModelDetails { get; set; }
        public bool ShowPriceColumn { get; set; }
        public bool ShowCompareColumn { get; set; }
        public bool IsRenaultLeadCampaign { get; set; }
        public bool ShowCampaignLink { get; set; }
        public CitiesDTO CityDetails { get; set; }
        public VariantListDTO VariantList { get; set; }
        public OfferAndDealerAdDTO OfferAndDealerAd { get; set; }
        public ArticlePageDetails WhatsNew { get; set; }
        public ArticlePageDetails ProsCons { get; set; }
        public ArticlePageDetails Verdict { get; set; }
        public ArticlePageDetails ExpertReviewOpinion { get; set; }
        public LeadFormModelData LeadFormModelData { get; set; }
    }
}
