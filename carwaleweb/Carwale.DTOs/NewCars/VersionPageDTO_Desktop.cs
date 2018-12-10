using Carwale.DTOs.CarData;
using Carwale.DTOs.Deals;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.ViewModels.New;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Classified;
using Carwale.Entity.CMS.Articles;
using Carwale.Entity.CMS.Photos;
using Carwale.Entity.CompareCars;
using Carwale.Entity.Dealers;
using Carwale.Entity.PriceQuote;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace Carwale.DTOs.NewCars
{
    /// <summary>
    /// Created By : Shalini 
    /// </summary>
    public class VersionPageDTO_Desktop
    {
        public CarVersionDetails VersionDetails { get; set; }
        public List<ModelImage> ModelPhotos { get; set; }
        public UsedCarCount UsedCarsCount { get; set; }
        public bool OfferExists { get; set; }
        public List<ArticleSummary> ModelNews { get; set; }
        public int ModelExpertReviewsCount { get; set; }
        public int ModelVideosCount { get; set; }
        public CarModelDetails ModelDetails { get; set; }
        public CCarData VersionData { get; set; }
        public string ShowAssistancePopup { get; set; }
        public SponsoredDealer SponsoredDealerAd { get; set; }
        public int NewCarDealersCount { get; set; }
        public DealsStockDTO AdvantageAdData { get; set; }
        public SubNavigationDTO SubNavigation { get; set; }
        public CarOverviewDTO OverviewDetails { get; set; }
        public PriceOverview CarPriceOverview { get; set; }
        public List<BreadcrumbEntity> BreadcrumbEntitylist { get; set; }
        public SimilarCarsDTO SimilarCars { get; set; }
        public JObject JsonLdObject { get; set; }
        public PageMetaTags PageMetaTags { get; set; }
        public List<VideoDTO> ModelVideos { get; set; }
        public bool ShowCampaignLink { get; set; }
        public bool IsRenaultLeadCampaign { get; set; }
        public List<NewCarVersionsDTO> NewCarVersions { get; set; }
        public CitiesDTO CityDetails { get; set; }
        public bool ShowTyresLink { get; set; }
        public ModelFloatingCtaViewModel FloatingCtaViewModel { get; set; }
        public bool ShowEmiCalculator { get; set; }
        public UpcomingCarModel UpcomingCarDetails { get; set; }
    }
}

