
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.UserReviews;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace Bikewale.Models.BikeModels
{
    public class ModelPageVM : ModelBase
    {

        public BikeModelPageEntity ModelPageEntity { get; set; }
        public PQOnRoadPrice PriceQuote { get; set; }
        public BikeVersionMinSpecs SelectedVersion { get; set; }
        public Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity DetailedDealer { get; set; }
        public ManufacturerCampaign ManufacturerCampaign { get; set; }
        public LeadCaptureEntity LeadCapture { get; set; }
        public IEnumerable<BestBikeEntityBase> objBestBikesList { get; set; }
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
        public bool IsLocationSelected { get { return (this.LocationCookie != null && this.LocationCookie.CityId > 0); } }
        public string Location { get { return (this.IsAreaSelected ? string.Format("{0}, {1}", LocationCookie.Area, LocationCookie.City) : (this.IsLocationSelected ? LocationCookie.City : "Mumbai")); } }
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
        public NewBikeDealers DealerDetails { get { return this.DetailedDealer.PrimaryDealer.DealerDetails; } }
        public string MPQString { get; set; }
        public string DealerArea { get { return (IsDealerDetailsExists && DealerDetails.objArea != null ? DealerDetails.objArea.AreaName : LocationCookie.Area); } }


        public string ClientIP { get; set; }
        public string PageUrl { get; set; }
        public int PQSourcePage { get { return (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_ModelPage; } }
        public int PQLeadSource { get { return 32; } }


        #region Model Page Widgets with Flags
        public RecentNewsVM News { get; set; }
        public RecentExpertReviewsVM ExpertReviews { get; set; }
        public RecentVideosVM Videos { get; set; }
        public SimilarBikesWidgetVM SimilarBikes { get; set; }
        public DealerCardVM OtherDealers { get; set; }
        public ServiceCenters.ServiceCenterDetailsWidgetVM ServiceCenters { get; set; }
        public DealersServiceCentersIndiaWidgetVM DealersServiceCenter { get; set; }
        public PriceInCity.PriceInTopCitiesWidgetVM PriceInTopCities { get; set; }
        public System.Collections.Generic.ICollection<SimilarCompareBikeEntity> PopularComparisions { get; set; }
        public UsedBikeByModelCityVM UsedModels { get; set; }
        public ReviewListBase UserReviews { get; set; }

        public bool AreModelPhotosAvailable { get { return (this.ModelPageEntity != null && this.ModelPageEntity.AllPhotos.Count() > 0); } }
        public bool IsNewsAvailable { get { return (News != null && News.FetchedCount > 0); } }
        public bool IsReviewsAvailable { get { return (ExpertReviews != null && ExpertReviews.FetchedCount > 0); } } //includes user reviews need to add
        public bool IsVideosAvailable { get { return (Videos != null && Videos.FetchedCount > 0); } }
        public bool IsSimilarBikesAvailable { get { return (SimilarBikes != null && SimilarBikes.Bikes != null && SimilarBikes.Bikes.Count() > 0); } }
        public bool IsOtherDealersAvailable { get { return (OtherDealers != null && OtherDealers.Dealers != null && OtherDealers.Dealers.Count() > 0); } }
        public bool IsServiceCentersAvailable { get { return (ServiceCenters != null && ServiceCenters.ServiceCentersList != null && ServiceCenters.ServiceCentersList.Count() > 0); } }
        public bool IsPopularComparisionsAvailable { get { return (PopularComparisions != null && PopularComparisions.Count() > 0); } }
        public bool IsPriceInTopCitiesAvailable { get { return (PriceInTopCities != null && PriceInTopCities.PriceQuoteList != null && PriceInTopCities.PriceQuoteList.Count() > 0); } }
        public bool IsDealersServiceCenterAvailable { get { return (DealersServiceCenter != null && DealersServiceCenter.DealerServiceCenters != null && (DealersServiceCenter.DealerServiceCenters.TotalDealerCount > 0 || DealersServiceCenter.DealerServiceCenters.TotalServiceCenterCount > 0)); } }
        public bool IsVersionSpecsAvailable { get { return (ModelPageEntity != null && ModelPageEntity.ModelVersionSpecs != null); } }
        public bool IsModelDescriptionAvailable { get { return (this.IsVersionSpecsAvailable || (this.ModelPageEntity.ModelDesc != null && !string.IsNullOrEmpty(this.ModelPageEntity.ModelDesc.SmallDescription))); } }
        public bool IsModelColorsAvailable { get { return (this.ModelPageEntity != null && this.ModelPageEntity.ModelColors != null && this.ModelPageEntity.ModelColors.Count() > 0); } }
        public bool IsUsedBikesAvailable { get { return (UsedModels != null && UsedModels.RecentUsedBikesList != null && UsedModels.RecentUsedBikesList.Count() > 0); } }

        public bool IsUserReviewsAvailable { get { return UserReviews != null && UserReviews.ReviewList != null && UserReviews.ReviewList.Count > 0; } }

        #endregion

        private StringBuilder colorStr = new StringBuilder();
        //public ModelPageVM viewModel = null;
        public BikeRankingPropertiesEntity BikeRanking { get; set; }
        public string ModelSummary { get; set; }

        public uint CampaignId { get; set; }
    }

}
