using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.manufacturecampaign;
using Bikewale.Entities.PhotoGallery;
using Bikewale.Entities.PriceQuote;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.Models.Gallery;
using Bikewale.Models.PriceInCity;
using Bikewale.Models.ServiceCenters;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by  : Sumit Kate on 28 Mar 2017
    /// Description : Price In city page view model
    /// Modified by: Vivek Singh Tomar on 30th Aug 2017
    /// Summary: Added property to hold city list for given model id
    /// Modified by : Ashutosh Sharma on 05 Oct 2017
    /// Description : Added PhotoGallery, IsGalleryLoaded and ModelGallery.
    /// Modified by : Ashutosh Sharma on 08 Dec 2017
    /// Description : Removed PhotoGallery, IsGalleryLoaded and ModelGallery.
    /// Modified by: Snehal Dange on 20th dec 2017
    /// Summary : added MoreAboutScootersWidgetVM
    /// Modified by : Snehal Dange on 24th Jan 2018
    /// Summary: added IsElectricBike flag
    /// Modified by : Pratibha Verma on 11 October 2018
    /// Description : Added IsOffersShownOnLeadPopup flag for showing offers on lead popup
    /// Modified by : Pratibha Verma on 29 November 2018
    /// Description : Added property ManufacturerFinanceCampaignEntity to bind finance campaign
    /// </summary>
    public class PriceInCityPageVM : ModelBase
    {

        public IEnumerable<PriceQuoteOfTopCities> PriceInNearestCities { get; set; }
        public PriceInTopCitiesWidgetVM PriceTopCities { get; set; }
        public DealerCardVM Dealers { get; set; }
        public SimilarBikesWidgetVM AlternateBikes { get; set; }
        public ServiceCenterDetailsWidgetVM ServiceCenters { get; set; }
        public uint ServiceCenterCount { get; set; }
        public PriceInTopCitiesWidgetVM PriceInTopCities { get; set; }
        public uint ServiceCentersCount { get; set; }

        public BikeInfoVM BikeInfo { get; set; }
        public Entities.BikeData.BikeMakeEntityBase Make { get; set; }
        public Entities.BikeData.BikeModelEntityBase BikeModel { get; set; }
        public IEnumerable<BikeVersionMinSpecs> VersionSpecs { get; set; }
        public IEnumerable<BikeQuotationEntity> BikeVersionPrices { get; set; }
        public CityEntityBase CityEntity { get; set; }
        public CityEntityBase CookieCityEntity { get; set; }
        public BikeModelRankVM BikeRank { get; set; }
        public string ModelImage { get; set; }
        public uint VersionId { get; set; }
        public String PageDescription { get; set; }
        public bool IsNew { get; set; }
        public bool IsAreaSelected { get; set; }
        public bool IsAreaAvailable { get; set; }
        public string CookieCityArea { get; set; }
        public PriceInTopCitiesWidgetVM NearestPriceCities { get; set; }
        public String BikeName { get { return (String.Format("{0} {1}", Make.MakeName, BikeModel.ModelName)); } }

        public String DealersWidget_H2 { get { return (HasDealers ? String.Format("{0} Showrooms in {1}", Make.MakeName, CityEntity.CityName) : ""); } }
        public String DealersWidget_ViewAll_Title { get { return String.Format("{0} Showrooms in {1}", Make.MakeName, CityEntity.CityName); } }
        public String DealersWidget_ViewAll_Href { get { return String.Format("/dealer-showrooms/{0}/{1}/", Make.MaskingName, CityEntity.CityMaskingName); } }


        public String ServiceCenterWidget_H2 { get { return (HasServiceCenters ? String.Format("{0} Service centers in {1}", Make.MakeName, CityEntity.CityName) : ""); } }
        public String ServiceCenterWidget_ViewAll_Title { get { return String.Format("{0} Service Centers in {1}", Make.MakeName, CityEntity.CityName); } }
        public String ServiceCenterWidget_ViewAll_Href { get { return String.Format("/service-centers/{0}/{1}/", Make.MaskingName, CityEntity.CityMaskingName); } }

        public String NearestPriceCitiesWidget_H2 { get { return (HasNearestPriceCities ? String.Format("{0} price in cities near {1}", BikeModel.ModelName, CityEntity.CityName) : ""); } }

        public BikeQuotationEntity FirstVersion { get { return (BikeVersionPrices != null && BikeVersionPrices.Any() ? BikeVersionPrices.FirstOrDefault() : null); } }
        public bool IsDiscontinued { get { return !IsNew; } }
        public bool HasNearestPriceCities { get { return (NearestPriceCities != null && NearestPriceCities.PriceQuoteList != null && NearestPriceCities.PriceQuoteList.Any()); } }
        public bool HasPriceInTopCities { get { return PriceInTopCities != null && PriceInTopCities.PriceQuoteList != null && PriceInTopCities.PriceQuoteList.Any(); } }
        public bool HasDealers { get { return (Dealers != null && Dealers.Dealers != null && Dealers.Dealers.Any()); } }
        public bool HasAlternateBikes { get { return (AlternateBikes != null && AlternateBikes.Bikes != null && AlternateBikes.Bikes.Any()); } }
        public bool HasTopCities { get { return (PriceTopCities != null && PriceTopCities.PriceQuoteList != null && PriceTopCities.PriceQuoteList.Any()); } }
        public string JSONBikeVersions { get; set; }

        public bool HasServiceCenters { get { return (ServiceCentersCount > 0); } }
        public bool HasCampaignDealer { get { return (DetailedDealer != null && DetailedDealer.PrimaryDealer != null && DetailedDealer.PrimaryDealer.DealerDetails != null); } }

        public Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity DetailedDealer { get; set; }
        public string MPQString { get; set; }
        public IEnumerable<SpecsItem> MinSpecsList { get; set; }
        public string GABikeName { get { return string.Format("{0}_{1}", Make.MakeName, BikeModel.ModelName); } }
        public LeadCaptureEntity LeadCapture { get; set; }
        public string PQId { get; set; }
        public Bikewale.Entities.manufacturecampaign.v2.ManufactureCampaignLeadEntity LeadCampaign { get; set; }
        public bool IsManufacturerLeadAdShown { get; set; }
        public ManufactureCampaignEMIEntity EMICampaign { get; set; }
        public bool IsManufacturerEMIAdShown { get; set; }

        public EnumBikeBodyStyles BodyStyle { get; set; }
        public string BodyStyleText { get; set; }
        public PopularBodyStyleVM PopularBodyStyle { get; set; }
        public IEnumerable<CityEntityBase> Cities { get; set; }
        public bool IsPopularBodyStyleAvailable { get { return (PopularBodyStyle != null && PopularBodyStyle.PopularBikes != null && PopularBodyStyle.PopularBikes.Any()); } }
        public string ReturnUrl { get; set; }
        public bool IsGalleryLoaded { get; set; }
        public ModelGalleryVM ModelGallery { get; set; }
        public ModelPhotoGalleryEntity PhotoGallery { get; set; }
        public MoreAboutScootersWidgetVM objMoreAboutScooter { get; set; }
        public bool IsElectricBike { get; set; }
        public bool IsOffersShownOnLeadPopup { get; set; }
        public ManufacturerFinanceCampaignEntity ManufacturerFinanceCampaign { get; set; }
        public bool VersionPriceAvailable { get; set; }
    }
}
