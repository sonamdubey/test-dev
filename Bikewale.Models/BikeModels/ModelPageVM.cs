
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.manufacturecampaign;
using Bikewale.Entities.PriceQuote;
using Bikewale.Models.BikeSeries;
using Bikewale.Models.UserReviews;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Bikewale.Models.BikeModels
{
    /// <summary>
    /// Modified by :   Sumit Kate on 26 Apr 2017
    /// Description :   Replace Count with Count()
    /// Modified By :- Subodh Jain added objUpcomingBikes
    /// Modified by : Ashutosh Sharma on 30 Aug 2017 
    /// Description : Removed IsGstPrice property
    /// Modified by : Vivek Singh Tomar on 12th Oct 2017
    /// Summary : Removed service center property
    /// Modified by :   Sumit Kate on 30 Nov 2017
    /// Description :   Added EMICalculator
    /// </summary>
    public class ModelPageVM : ModelBase
    {

        public BikeModelPageEntity ModelPageEntity { get; set; }
        public PQOnRoadPrice PriceQuote { get; set; }
        public BikeVersionMinSpecs SelectedVersion { get; set; }
        public Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity DetailedDealer { get; set; }
        public LeadCaptureEntity LeadCapture { get; set; }
        public OtherBestBikesVM OtherBestBikes { get; set; }
        public UpcomingBikesWidgetVM objUpcomingBikes { get; set; }
        public EMI EMIDetails { get; set; }
        public uint VersionId { get; set; }
        public uint DealerId { get; set; }
        public uint PQId { get; set; }
        public uint ModelId { get; set; }
        public uint CityId { get; set; }
        public uint AreaId { get; set; }
        public uint BikePrice { get; set; }
        public string VersionName { get; set; }
        public CityEntityBase City { get; set; }

        public string BikeName { get; set; }
        public bool IsOnRoadPriceAvailable { get; set; }
        public GlobalCityAreaEntity LocationCookie { get; set; }
        public bool IsAreaSelected { get; set; }
        public bool IsLocationSelected { get { return (this.City != null && this.City.CityId > 0); } }
        public string Location { get { return (this.IsAreaSelected ? (string.IsNullOrEmpty(LocationCookie.Area) ? LocationCookie.City : string.Format("{0}, {1}", LocationCookie.Area, LocationCookie.City)) : (this.IsLocationSelected ? LocationCookie.City : "Mumbai")); } }
        public string LeadBtnLongText { get { return "Get offers from dealer"; } }
        public string LeadBtnShortText { get { return "Get offers"; } }

        public bool IsModelDetails { get { return (this.ModelPageEntity != null && this.ModelPageEntity.ModelDetails != null); } }

        public bool IsNewBike { get { return (IsModelDetails && this.ModelPageEntity.ModelDetails.New); } }
        public bool IsUpcomingBike { get { return (IsModelDetails && this.ModelPageEntity.ModelDetails.Futuristic); } }
        public bool IsDiscontinuedBike { get { return (IsModelDetails && !IsNewBike && !IsUpcomingBike); } }
        public bool IsDPQAvailable { get; set; }
        public bool IsBPQAvailable { get; set; }
        public bool IsPrimaryDealer { get { return (this.DetailedDealer != null && this.DetailedDealer.PrimaryDealer != null); } }
        public bool IsDealerDetailsExists { get { return (this.IsPrimaryDealer && this.DetailedDealer.PrimaryDealer.DealerDetails != null); } }
        public bool IsPremiumDealer { get { return (IsPrimaryDealer && this.DetailedDealer.PrimaryDealer.IsPremiumDealer); } }
        public NewBikeDealers DealerDetails { get { return IsPrimaryDealer ? this.DetailedDealer.PrimaryDealer.DealerDetails : null; } }
        public string MPQString { get; set; }
        public string DealerArea { get { return (IsDealerDetailsExists && DealerDetails.objArea != null ? DealerDetails.objArea.AreaName : LocationCookie.Area); } }
        public string BestBikeHeading { get; set; }
        public string ColourImageUrl { get; set; }

        public string ClientIP { get; set; }
        public string PageUrl { get; set; }
        public int PQSourcePage { get { return (int)PQSourceEnum.Desktop_ModelPage; } }
        public int PQLeadSource { get { return 32; } }
        public IEnumerable<BikeQuotationEntity> BikeVersionPrices { set; get; }
        public string VersionPriceListSummary { get; set; }

        #region Model Page Widgets with Flags
        public RecentNewsVM News { get; set; }
        public RecentExpertReviewsVM ExpertReviews { get; set; }
        public RecentVideosVM Videos { get; set; }
        public SimilarBikesWidgetVM SimilarBikes { get; set; }
        public DealerCardVM OtherDealers { get; set; }
        public DealersServiceCentersIndiaWidgetVM DealersServiceCenter { get; set; }
        public PriceInCity.PriceInTopCitiesWidgetVM PriceInTopCities { get; set; }
        public System.Collections.Generic.ICollection<SimilarCompareBikeEntity> PopularComparisions { get; set; }
        public UsedBikeByModelCityVM UsedModels { get; set; }
        public UserReviewsSearchVM UserReviews { get; set; }

        public bool AreModelPhotosAvailable { get { return (this.ModelPageEntity != null && ModelPageEntity.AllPhotos != null && this.ModelPageEntity.AllPhotos.Any()); } }
        public bool IsNewsAvailable { get { return (News != null && News.FetchedCount > 0); } }
        public bool IsReviewsAvailable { get { return (ExpertReviews != null && ExpertReviews.FetchedCount > 0); } } //includes user reviews need to add
        public bool IsVideosAvailable { get { return (Videos != null && Videos.VideosList != null && Videos.FetchedCount > 0 && Videos.VideosList.Any()); } }
        public bool IsSimilarBikesAvailable { get { return (SimilarBikes != null && SimilarBikes.Bikes != null && SimilarBikes.Bikes.Any()); } }
        public bool IsOtherDealersAvailable { get { return (OtherDealers != null && OtherDealers.Dealers != null && OtherDealers.Dealers.Any()); } }
        public bool IsPopularComparisionsAvailable { get { return (PopularComparisions != null && PopularComparisions.Any()); } }
        public bool IsPriceInTopCitiesAvailable { get { return (PriceInTopCities != null && PriceInTopCities.PriceQuoteList != null && PriceInTopCities.PriceQuoteList.Any()); } }
        public bool IsDealersServiceCenterAvailable { get { return (DealersServiceCenter != null && DealersServiceCenter.DealerServiceCenters != null && DealersServiceCenter.DealerServiceCenters.TotalDealerCount > 0); } }
        public bool IsVersionSpecsAvailable { get { return (ModelPageEntity != null && ModelPageEntity.ModelVersionSpecs != null); } }
        public bool IsModelDescriptionAvailable { get { return (this.IsVersionSpecsAvailable || (this.ModelPageEntity.ModelDesc != null && !string.IsNullOrEmpty(this.ModelPageEntity.ModelDesc.SmallDescription))); } }
        public bool IsModelColorsAvailable { get { return (this.ModelPageEntity != null && this.ModelPageEntity.ModelColors != null && this.ModelPageEntity.ModelColors.Any()); } }
        public bool IsUsedBikesAvailable { get { return (UsedModels != null && UsedModels.RecentUsedBikesList != null && UsedModels.RecentUsedBikesList.Any()); } }
        public bool IsShowPriceTab { get; set; }
        public bool IsUserReviewsAvailable { get { return UserReviews != null && UserReviews.UserReviews != null && UserReviews.ReviewsInfo != null && UserReviews.ReviewsInfo.TotalReviews > 0; } }
        public IEnumerable<ColorImageBaseEntity> ColorImages { get { return AreModelPhotosAvailable ? ModelPageEntity.AllPhotos.Where(x => x.ColorId > 0) : null; } }

        #endregion

        private StringBuilder colorStr = new StringBuilder();
        public BikeRankingPropertiesEntity BikeRanking { get; set; }
        public string ModelSummary { get; set; }
        public uint CampaignId { get; set; }

        public int ModelColorPhotosCount { get; set; }

        public bool ShowOnRoadButton { get; set; }
        public string ReturnUrl { get; set; }
        public bool HasCityPricing { get; set; }
        public ManufactureCampaignLeadEntity LeadCampaign { get; set; }
        public bool IsManufacturerLeadAdShown { get; set; }
        public bool IsManufacturerTopLeadAdShown { get; set; }
        public ManufactureCampaignEMIEntity EMICampaign { get; set; }
        public bool IsManufacturerEMIAdShown { get; set; }

        public EnumBikeBodyStyles BodyStyle { get; set; }
        public string BodyStyleName { get; set; }
        // Will add "Scooters" or "Bikes"
        public string BodyStyleText { get; set; }
        // Will add "scooter" or "bike"
        public string BodyStyleTextSingular { get; set; }
        public PopularBodyStyleVM PopularBodyStyle { get; set; }
        public bool IsPopularBodyStyleAvailable { get { return (PopularBodyStyle != null && PopularBodyStyle.PopularBikes != null && PopularBodyStyle.PopularBikes.Any()); } }
        public BikeSeriesModelsVM ModelsBySeries { get; set; }
        public ModelMileageWidgetVM Mileage { get; set; }

        public bool IsMileageByUsersAvailable { get; set; }

        public EMICalculatorVM EMICalculator { get; set; }
    }

}
