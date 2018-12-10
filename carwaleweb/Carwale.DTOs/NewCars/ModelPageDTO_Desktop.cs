using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CarData;
using Carwale.DTOs.Deals;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.ViewModels.New;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.CMS.UserReviews;
using Carwale.Entity.Dealers;
using Carwale.Entity.PriceQuote;
using Carwale.Entity.ViewModels.CarData;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Carwale.DTOs.NewCars
{
	/// <summary>
	/// Created By : Shalini Nair
	/// </summary>
	public class ModelPageDTO_Desktop
    {
        public CarModelDetails ModelDetails { get; set; }
        public List<ModelColors> ModelColors { get; set; }
        public SimilarCarsDTO SimilarCars { get; set; }
        public ModelDataSummary ModelDataSummary { get; set; }
        public ModelReview ModelReview { get; set; }
        public List<UserReviewEntity> ModelUserReviews { get; set; }
        public List<ArticleSummary> ExpertReviewsByModel { get; set; }
        public List<ArticleSummary> NewsByModel { get; set; }
        public List<NewCarVersionsDTO> NewCarVersions { get; set; }
        public List<CarVersions> DiscontinuedCarVersions { get; set; }
        public List<ModelImage> ModelPhotosList { get; set; }
        public List<Video> ModelVideos { get; set; }
        public UpcomingCarModel UpcomingCarDetails { get; set; }
        public List<UpcomingCarModel> UpcomingCarDetailList { get; set; }
        public PageMetaTags PageMetaTags { get; set; }
        public string Summary { get; set; }
        public DealsStockDTO AdvantageAdData { get; set; }
        public UsedCarCount UsedCarsCount { get; set; }
        public string ShowAssistancePopup { get; set; }
        public List<StockSummary> UsedLuxuryCars { get; set; }
        public SponsoredDealer SponsoredDealerAd { get; set; }
        public int NewCarDealersCount { get; set; }
        public NewCarDealerEntiy DealersList { get; set; }
        public SubNavigationDTO SubNavigation { get; set; }
        public CarOverviewDTO OverviewDetails { get; set; }
        public List<StockSummary> UsedCarSuggestions { get; set; }
        public PriceOverview CarPriceOverview { get; set; }
        public List<string> FuelTypes { get; set; }
        public List<BreadcrumbEntity> BreadcrumbEntitylist { get; set; }
        public List<string> TransmissionTypes { get; set; }
        public JObject JsonLdObject { get; set; }
        public bool ShowPriceColumn { get; set; }
        public int VersionCountWithSpecs { get; set; }
        public UpcomingCarModel UpgradedModelDetails { get; set; }
        public bool ShowCampaignLink { get; set; }
        public bool IsRenaultLeadCampaign { get; set; }
        public CarVersionsDTO CarVersion { get; set; }
        public CitiesDTO CityDetails { get; set; }
        public List<MileageDataEntity> MileageData { get; set; }
        public ModelFloatingCtaViewModel FloatingCtaViewModel { get; set; }
        public DealerAdDTO DealerAd { get; set; }
        public bool IsNewSpecsShow { get; set; }
        public List<string> SeatingCapacity { get; set; }
        public List<string> Drivetrain { get; set; }
        public bool ShowEmiCalculator { get; set; }
        public ArticlePageDetails ExpertReview { get; set; }
    }
}
