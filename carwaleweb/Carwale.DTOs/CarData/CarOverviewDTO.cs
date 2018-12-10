using Carwale.DTOs.Campaigns;
using Carwale.DTOs.CMS.Photos;
using Carwale.DTOs.Deals;
using Carwale.DTOs.PriceQuote;
using Carwale.Entity.CarData;
using Carwale.Entity.PriceQuote;
using System.Collections.Generic;
using Carwale.DTOs.NewCars;
using Carwale.Entity.CMS;
using Carwale.Entity.Common;
using Carwale.DTOs.Geolocation;

namespace Carwale.DTOs.CarData
{
	public class CarOverviewDTO
    {
        public bool Futuristic { get; set; }
        public bool New { get; set; }
        public int RootId { get; set; }
        public string RootName { get; set; }
        public bool Discontinue { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImage { get; set; }
        public string VersionName { get; set; }
        public string CarName { get; set; }
        public string ModelName { get; set; }
        public string MakeName { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public int MinAvgPrice { get; set; }
        public bool AdAvailable { get; set; }
        public bool IsUsedCarAvial { get; set; }
        public bool IsVersionPage { get; set; }
        public string MaskingName { get; set; }
        public double LiveListingCount { get; set; }
        public double MinLiveListingPrice { get; set; }
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public int DealerId { get; set; }
        public int ActualDealerId { get; set; }
        public int BucketType { get; set; }
        public string ExpectedLaunch { get; set; }
        public string EstimatedPrice { get; set; }
        public int PageId { get; set; }
        public int MaxDiscount { get; set; }
        public string StockMaskingName { get; set; }
        public int DealsCount { get; set; }
        public int DealsCityId { get; set; }
        public string DealsModelName { get; set; }
        public int PhotoCount { get; set; }
        public string EMI { get; set; }
        public DealsStockDTO AdvantageAdData { get; set; }
        public PriceOverview CarPriceOverview { get; set; }
        public int VersionId { get; set; }
        public PredictionData PredictionData { get; set; }
        public string LeadCTA { get; set; }
        public bool Is360ExteriorAvailable { get; set; }
        public bool Is360OpenAvailable { get; set; }
        public bool Is360InteriorAvailable { get; set; }
        public string ImageUrl360Slug { get; set; }
        public List<ModelImageDTO> ModelPhotosListCarousel { get; set; }
        public List<NewCarVersionsDTOV2> NewCarVersions { get; set; }
        public int VideosCount { get; set; }
        public string VideoUrl{ get; set; }
        public List<Video> ModelVideos { get; set; }
        public int ColoursCount { get; set; }
        public bool ShowColours { get; set; }
        public SimilarCarModels ReplacedModelDetails { get; set; }
        public bool ShowCampaignLink { get; set; }
        public Dictionary<int, IdName> CampaignTemplates { get; set; }
        public int BodyStyleId { get; set; }
        public EmiCalculatorModelData EmiCalculatorModelData { get; set; }
        public bool IsPicSnippetExperiment { get; set; }
        public string CampaignLeadCTA { get; set; }
	}

    //ONLY DIFFERENCE BETWEEN CarOverviewDTO AND CarOverviewDTOV2 IS
    //1.PriceOverviewDTO used instead of PriceOverview entity
    //So if u want to further version this DTO THEN FIRST CHECK IF THERE IS NEED TO VERSIONING OR U CAN ALTER THE SAME
    public class CarOverviewDTOV2
    {
        public bool Futuristic { get; set; }
        public bool New { get; set; }
        public int RootId { get; set; }
        public bool Discontinue { get; set; }
        public string HostUrl { get; set; }
        public string OriginalImage { get; set; }
        public string VersionName { get; set; }
        public string CarName { get; set; }
        public string ModelName { get; set; }
        public string MakeName { get; set; }
        public double MinPrice { get; set; }
        public double MaxPrice { get; set; }
        public int MinAvgPrice { get; set; }
        public bool AdAvailable { get; set; }
        public bool IsUsedCarAvial { get; set; }
        public bool IsVersionPage { get; set; }
        public string MaskingName { get; set; }
        public double LiveListingCount { get; set; }
        public double MinLiveListingPrice { get; set; }
        public int MakeId { get; set; }
        public int ModelId { get; set; }
        public int DealerId { get; set; }
        public int BucketType { get; set; }
        public string ExpectedLaunch { get; set; }
        public string EstimatedPrice { get; set; }
        public int PageId { get; set; }
        public int MaxDiscount { get; set; }
        public string StockMaskingName { get; set; }
        public int DealsCount { get; set; }
        public int DealsCityId { get; set; }
        public string DealsModelName { get; set; }
        public int PhotoCount { get; set; }
        public string EMI { get; set; }
        public DealsStockDTO AdvantageAdData { get; set; }
        public PriceOverviewDTOV2 CarPriceOverview { get; set; }
        public int VersionId { get; set; }
        public VersionPriceQuote VersionPriceQuote { get; set; }
        public TrackingDataDTO TrackingData { get; set; }
        public PredictionData PredictionData { get; set; }
        public string LeadCTA { get; set; }
        public CityAreaDTO UserLocation { get; set; }
        public List<ModelImageDTO> ModelPhotosListCarousel { get; set; }
        public int PQPageId { get; set; }
        public SimilarCarModels ReplacedModelDetails { get; set; }
        public bool ShowCampaignLink { get; set; }
        public Dictionary<int, IdName> CampaignTemplates { get; set; }
        public EmiCalculatorModelData EmiCalculatorModelData { get; set; }
        public bool ShowCustomiseYourEmiLink { get; set; }
        public bool ShowCustomiseYourEmiButton { get; set; }
        public bool ShowChangeTextLink { get; set; }
        public bool IsToolTipCampaign { get; set; }
        public string CampaignLeadCTA { get; set; }
        public bool ShowGetEmiOffersCalcButton { get; set; }
        public string CampaignDealerId { get; set; }
        public DealerAdDTO DealerAd { get; set; }
    }
}
