
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
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

        public uint VersionId { get; set; }
        public uint DealerId { get; set; }
        public uint PQId { get; set; }
        public uint ModelId { get; set; }
        public uint CityId { get; set; }
        public uint AreaId { get; set; }
        public uint BikePrice { get; set; }
        public string VersionName { get; set; }


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

        public bool AreModelPhotosAvailable { get { return (this.ModelPageEntity != null && this.ModelPageEntity.AllPhotos.Count() > 0); } }

        public string ClientIP { get; set; }
        public string PageUrl { get; set; }
        public int PQSourcePage { get { return (int)Bikewale.Entities.PriceQuote.PQSourceEnum.Desktop_ModelPage; } }
        public int PQLeadSource { get { return 32; } }

        public RecentNewsVM News { get; set; }
        public SimilarBikesWidgetVM SimilarBikesVM { get; set; }




        public Bikewale.Entities.Used.Search.SearchResult UsedBikes = null;
        private StringBuilder colorStr = new StringBuilder();
        //public ModelPageVM viewModel = null;
        public BikeRankingEntity bikeRankObj;
        public string styleName = string.Empty, rankText = string.Empty, bikeType = string.Empty;


        public RecentExpertReviewsVM ExpertReviews { get; set; }

        public RecentVideosVM Videos { get; set; }

        public DealerCardVM OtherDealers { get; set; }

        public ServiceCenters.ServiceCenterDetailsWidgetVM ServiceCenters { get; set; }

        public DealersServiceCentersIndiaWidgetVM DealersServiceCenter { get; set; }


        public PriceInCity.PriceInTopCitiesWidgetVM PriceInTopCities { get; set; }

        public System.Collections.Generic.ICollection<SimilarCompareBikeEntity> PopularComparisions { get; set; }
    }

}
