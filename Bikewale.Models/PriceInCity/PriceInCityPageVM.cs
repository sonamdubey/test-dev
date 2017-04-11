﻿using Bikewale.Entities.DealerLocator;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Models.PriceInCity;
using System;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 28 Mar 2017
    /// Description :   Price In city page view model
    /// </summary>
    public class PriceInCityPageVM : ModelBase
    {
        public PriceInTopCitiesWidgetVM PriceInTopCities { get; set; }
        public DealerCardVM Dealers { get; set; }
        public SimilarBikesWidgetVM AlternateBikes { get; set; }
        public uint ServiceCentersCount { get; set; }
        public BikeInfoVM BikeInfo { get; set; }
        public Entities.BikeData.BikeMakeEntityBase Make { get; set; }
        public Entities.BikeData.BikeModelEntityBase BikeModel { get; set; }
        public IEnumerable<BikeQuotationEntity> BikeVersionPrices { get; set; }
        public CityEntityBase CityEntity { get; set; }
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

        public String DealersWidget_H2 { get { return (HasDealers ? String.Format("{0} Dealers in {1}", Make.MakeName, CityEntity.CityName) : ""); } }
        public String DealersWidget_ViewAll_Title { get { return String.Format("{0} Showrooms in {1}", Make.MakeName, CityEntity.CityName); } }
        public String DealersWidget_ViewAll_Href { get { return String.Format("/{0}-dealer-showrooms-in-{1}/", Make.MaskingName, CityEntity.CityMaskingName); } }

        public String ServiceCenterWidget_H2 { get { return (HasServiceCenters ? String.Format("{0} Service centers in {1}", Make.MakeName, CityEntity.CityName) : ""); } }
        public String ServiceCenterWidget_ViewAll_Title { get { return String.Format("{0} Service Centers in {1}", Make.MakeName, CityEntity.CityName); } }
        public String ServiceCenterWidget_ViewAll_Href { get { return String.Format("/{0}-service-center-in-{1}/", Make.MaskingName, CityEntity.CityMaskingName); } }

        public String NearestPriceCitiesWidget_H2 { get { return (HasNearestPriceCities ? String.Format("{0} price in cities near {1}", BikeModel.ModelName, CityEntity.CityName) : ""); } }

        public BikeQuotationEntity FirstVersion { get { return (BikeVersionPrices != null && BikeVersionPrices.Count() > 0 ? BikeVersionPrices.FirstOrDefault() : null); } }

        public bool IsDiscontinued { get { return !IsNew; } }
        public bool HasNearestPriceCities { get { return (NearestPriceCities != null && NearestPriceCities.PriceQuoteList != null && NearestPriceCities.PriceQuoteList.Count() > 0); } }
        public bool HasPriceInTopCities { get { return PriceInTopCities != null && PriceInTopCities.PriceQuoteList != null && PriceInTopCities.PriceQuoteList.Count() > 0; } }
        public bool HasDealers { get { return (Dealers != null && Dealers.Dealers != null && Dealers.Dealers.Count() > 0); } }
        public bool HasAlternateBikes { get { return (AlternateBikes != null && AlternateBikes.Bikes != null && AlternateBikes.Bikes.Count() > 0); } }
        public bool HasServiceCenters { get { return (ServiceCentersCount > 0); } }
        public bool HasPopularCities { get; set; }
        public bool HasCampaignDealer { get; set; }

        public Bikewale.Entities.PriceQuote.v2.DetailedDealerQuotationEntity DetailedDealer { get; set; }
        public string MPQString { get; set; }
    }
}
