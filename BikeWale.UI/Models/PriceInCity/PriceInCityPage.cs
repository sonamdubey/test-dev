using Bikewale.DTO.PriceQuote;
using Bikewale.Entities;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.manufacturecampaign;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.AdSlot;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Models.BestBikes;
using Bikewale.Models.PriceInCity;
using Bikewale.Notifications;
using Bikewale.Utility;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 28 Mar 2017
    /// Description :   PriceInCityPage model
    /// Modified by :   Sanskar Gupta on 21 Mar 2018
    /// Description :   Added `AdPath_Mobile` and `AdId_Mobile`
    /// </summary>
    public class PriceInCityPage
    {
        private readonly ICityMaskingCacheRepository _cityMaskingCache = null;
        private readonly IBikeMaskingCacheRepository<Entities.BikeData.BikeModelEntity, int> _modelMaskingCache = null;
        private readonly IPriceQuote _objPQ = null;
        private readonly IPriceQuoteCache _objPQCache = null;
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IServiceCenter _objServiceCenter = null;
        private readonly IBikeVersions<BikeVersionEntity, uint> _version;
        private readonly PQSourceEnum pqSource;
        private readonly IBikeInfo _bikeInfo = null;
        private readonly IBikeModelsCacheRepository<int> _modelCache = null;
        private readonly IDealerPriceQuoteDetail _objDealerDetails = null;
        private readonly IDealerPriceQuote _objDealerPQ = null;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IAreaCacheRepository _objAreaCache = null;
        private readonly IManufacturerCampaign _objManufacturerCampaign = null;
        private uint cityId, modelId, versionCount, colorCount, dealerCount, areaId;
        private readonly string modelMaskingName, cityMaskingName, makeMaskingName;
        private string pageDescription, area, city;
        private BikeQuotationEntity firstVersion;
        private uint primaryDealerId;
        private bool isNew, isAreaSelected, hasAreaAvailable;
        private readonly IBikeModels<Entities.BikeData.BikeModelEntity, int> _objModelEntity;
        public StatusCodes Status { get; private set; }
        public String RedirectUrl { get; private set; }
        public uint NearestCityCount { get; set; }
        public uint BikeInfoTabCount { get; set; }
        public uint TopCount { get; set; }
        public PQSourceEnum PQSource { get; set; }
        public PQSources Platform { get; set; }
        public LeadSourceEnum LeadSource { get; set; }
        public ManufacturerCampaignServingPages ManufacturerCampaignPageId { get; set; }
        public string CurrentPageUrl { get; set; }
        public bool IsMobile { get; internal set; }
        private GlobalCityAreaEntity locationCookie = null;
        private readonly IAdSlot _adSlot = null;
        private BikeSeriesEntityBase Series;

        private readonly String _adPath_Mobile = "/1017752/Bikewale_CityPrice_Mobile";
        private readonly String _adId_Mobile = "1516080888816";


        private readonly String _adPath_Desktop = "/1017752/Bikewale_CityPrice";
        private readonly String _adId_Desktop = "1517407919554";

        private readonly String _adId_SimilarBikes = "1505919734321";
        private readonly String _adPath_SimilarBikes_Desktop = "/1017752/SimilarBikes_Desktop";
        private readonly String _adPath_SimilarBikes_Mobile = "/1017752/SimilarBikes_Mobile";

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Mar 2017
        /// Description :   Constructor to initialize the member variables
        /// </summary>
        /// <param name="cityMaskingCache"></param>
        /// <param name="modelMaskingCache"></param>
        /// <param name="objPQ"></param>
        /// <param name="objPQCache"></param>
        /// <param name="objDealerCache"></param>
        /// <param name="objServiceCenter"></param>
        /// <param name="versionCache"></param>
        /// <param name="bikeInfo"></param>
        /// <param name="cityCache"></param>
        /// <param name="modelCache"></param>
        /// <param name="pqSource"></param>
        /// <param name="modelMaskingName"></param>
        /// <param name="cityMaskingName"></param>
        public PriceInCityPage(ICityMaskingCacheRepository cityMaskingCache, IBikeMaskingCacheRepository<Entities.BikeData.BikeModelEntity, int> modelMaskingCache, IPriceQuote objPQ, IPriceQuoteCache objPQCache, IDealerCacheRepository objDealerCache, IServiceCenter objServiceCenter, IBikeVersions<BikeVersionEntity, uint> version, IBikeInfo bikeInfo, IBikeModelsCacheRepository<int> modelCache, IDealerPriceQuoteDetail objDealerDetails, IDealerPriceQuote objDealerPQ, ICityCacheRepository objCityCache, IAreaCacheRepository objAreaCache, IManufacturerCampaign objManufacturerCampaign, PQSourceEnum pqSource, string modelMaskingName, string cityMaskingName, IBikeModels<Entities.BikeData.BikeModelEntity, int> modelEntity, string makeMaskingName)
        {
            _cityMaskingCache = cityMaskingCache;
            _modelMaskingCache = modelMaskingCache;
            _objPQ = objPQ;
            _objPQCache = objPQCache;
            _objDealerCache = objDealerCache;
            _objServiceCenter = objServiceCenter;
            _version = version;
            _bikeInfo = bikeInfo;
            _modelCache = modelCache;
            _objDealerDetails = objDealerDetails;
            _objDealerPQ = objDealerPQ;
            _objCityCache = objCityCache;
            _objAreaCache = objAreaCache;
            this.pqSource = pqSource;
            this.modelMaskingName = modelMaskingName;
            this.cityMaskingName = cityMaskingName;
            this.makeMaskingName = makeMaskingName;
            _objManufacturerCampaign = objManufacturerCampaign;
            _objModelEntity = modelEntity;
            ProcessQueryString();
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 11 Oct 2017
        /// Description : Added IBikeModels<Entities.BikeData.BikeModelEntity, int> instance in constructor for image gallery.
        /// Modifed by : Ashutosh Sharma on 13 Nov 2017
        /// Description : Added IAdSlot.
        /// </summary>
        /// <param name="cityMaskingCache"></param>
        /// <param name="modelMaskingCache"></param>
        /// <param name="objPQ"></param>
        /// <param name="objPQCache"></param>
        /// <param name="objDealerCache"></param>
        /// <param name="objServiceCenter"></param>
        /// <param name="versionCache"></param>
        /// <param name="bikeInfo"></param>
        /// <param name="modelCache"></param>
        /// <param name="objDealerDetails"></param>
        /// <param name="objDealerPQ"></param>
        /// <param name="objCityCache"></param>
        /// <param name="objAreaCache"></param>
        /// <param name="objManufacturerCampaign"></param>
        /// <param name="pqSource"></param>
        /// <param name="modelMaskingName"></param>
        /// <param name="cityMaskingName"></param>
        /// <param name="modelEntity"></param>
        public PriceInCityPage(ICityMaskingCacheRepository cityMaskingCache, IBikeMaskingCacheRepository<Entities.BikeData.BikeModelEntity, int> modelMaskingCache, IPriceQuote objPQ, IPriceQuoteCache objPQCache, IDealerCacheRepository objDealerCache, IServiceCenter objServiceCenter, IBikeVersions<BikeVersionEntity, uint> version, IBikeInfo bikeInfo, IBikeModelsCacheRepository<int> modelCache, IDealerPriceQuoteDetail objDealerDetails, IDealerPriceQuote objDealerPQ, ICityCacheRepository objCityCache, IAreaCacheRepository objAreaCache, IManufacturerCampaign objManufacturerCampaign, PQSourceEnum pqSource, string modelMaskingName, string cityMaskingName, IBikeModels<Entities.BikeData.BikeModelEntity, int> modelEntity, IAdSlot adSlot, string makeMaskingName)
        {
            _cityMaskingCache = cityMaskingCache;
            _modelMaskingCache = modelMaskingCache;
            _objPQ = objPQ;
            _objPQCache = objPQCache;
            _objDealerCache = objDealerCache;
            _objServiceCenter = objServiceCenter;
            _version = version;
            _bikeInfo = bikeInfo;
            _modelCache = modelCache;
            _objDealerDetails = objDealerDetails;
            _objDealerPQ = objDealerPQ;
            _objCityCache = objCityCache;
            _objAreaCache = objAreaCache;
            this.pqSource = pqSource;
            this.modelMaskingName = modelMaskingName;
            this.cityMaskingName = cityMaskingName;
            this.makeMaskingName = makeMaskingName;
            _objManufacturerCampaign = objManufacturerCampaign;
            _objModelEntity = modelEntity;
            _adSlot = adSlot;
            ProcessQueryString();
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 27 Mar 2017
        /// Description :   Processes Query string. It validates model and city name
        /// </summary>
        private void ProcessQueryString()
        {
            ModelMaskingResponse objModelResponse = null;
            CityMaskingResponse objCityResponse = null;
            locationCookie = GlobalCityArea.GetGlobalCityArea();
            String rawUrl = HttpContext.Current.Request.RawUrl;
            string newMakeMasking = string.Empty;
            bool isMakeRedirection = false;
            try
            {

                if (!(String.IsNullOrEmpty(modelMaskingName) || String.IsNullOrEmpty(cityMaskingName)))
                {
                    objCityResponse = _cityMaskingCache.GetCityMaskingResponse(cityMaskingName);
                    newMakeMasking = ProcessMakeMaskingName(makeMaskingName, out isMakeRedirection);
                    objModelResponse = _modelMaskingCache.GetModelMaskingResponse(string.Format("{0}_{1}", makeMaskingName, modelMaskingName));
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("ProcessQueryString({0},{1},{2})", makeMaskingName, modelMaskingName, cityMaskingName));
                Status = StatusCodes.ContentNotFound;
            }
            finally
            {
                if (objCityResponse != null && objModelResponse != null)
                {
                    // Get cityId
                    // Code to check whether masking name is changed or not. If changed redirect to appropriate url
                    if (objCityResponse.StatusCode == 200)
                    {
                        cityId = objCityResponse.CityId;

                        if (locationCookie.CityId > 0 && locationCookie.CityId == cityId && locationCookie.AreaId > 0)
                        {
                            areaId = locationCookie.AreaId;
                        }
                    }
                    else if (objCityResponse.StatusCode == 301)
                    {
                        //redirect permanent to new page                               
                        rawUrl = rawUrl.Replace(cityMaskingName, objCityResponse.MaskingName);
                        Status = StatusCodes.RedirectPermanent;
                    }
                    else
                    {
                        Status = StatusCodes.ContentNotFound;
                    }

                    // Get ModelId
                    // Code to check whether masking name is changed or not. If changed redirect to appropriate url
                    if (objModelResponse.StatusCode == 200)
                    {
                        modelId = objModelResponse.ModelId;
                    }
                    else if (objModelResponse.StatusCode == 301 || isMakeRedirection)
                    {
                        //redirect permanent to new page                         
                        rawUrl = rawUrl.Replace(modelMaskingName, objModelResponse.MaskingName).Replace(makeMaskingName, newMakeMasking);
                        Status = StatusCodes.RedirectPermanent;
                    }
                    else
                    {
                        Status = StatusCodes.ContentNotFound;
                    }

                    if (objCityResponse.StatusCode == 200 && objModelResponse.StatusCode == 200)
                    {
                        Status = StatusCodes.ContentFound;
                    }
                    RedirectUrl = rawUrl;
                }
                else
                {
                    Status = StatusCodes.ContentNotFound;
                }

            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 11th Dec 2017
        /// Description : Process make masking name for redirection
        /// </summary>
        /// <param name="make"></param>
        /// <param name="isMakeRedirection"></param>
        /// <returns></returns>
        private string ProcessMakeMaskingName(string make, out bool isMakeRedirection)
        {
            MakeMaskingResponse makeResponse = null;
            Common.MakeHelper makeHelper = new Common.MakeHelper();
            isMakeRedirection = false;
            if (!string.IsNullOrEmpty(make))
            {
                makeResponse = makeHelper.GetMakeByMaskingName(make);
            }
            if (makeResponse != null)
            {
                if (makeResponse.StatusCode == 200)
                {
                    return makeResponse.MaskingName;
                }
                else if (makeResponse.StatusCode == 301)
                {
                    isMakeRedirection = true;
                    return makeResponse.MaskingName;
                }
                else
                {
                    return "";
                }
            }

            return "";
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 05 Jan 2016
        /// Description :   Replaced the Convert.ToXXX with XXX.TryParse method
        /// Modified By : Sushil Kumar on 26th August 2016
        /// Description : Replaced location name from location cookie to selected location objects for city and area respectively.
        /// </summary>
        private void CheckCityCookie(PriceInCityPageVM objVM)
        {
            try
            {
                if (modelId > 0 && cityId > 0)
                {
                    var cities = _objCityCache.GetPriceQuoteCities(modelId);
                    if (cities != null)
                    {
                        objVM.Cities = cities;
                        var selectedCity = cities.FirstOrDefault(m => m.CityId == cityId);
                        objVM.CookieCityEntity = selectedCity;
                        if (selectedCity != null && selectedCity.HasAreas && areaId > 0)
                        {
                            var areas = _objAreaCache.GetAreaList(modelId, cityId);
                            city = selectedCity.CityName;
                            if (areas != null && areas.Any())
                            {
                                var selectedArea = areas.FirstOrDefault(m => m.AreaId == areaId);
                                if (selectedArea != null)
                                {
                                    area = selectedArea.AreaName;
                                    isAreaSelected = true;
                                }
                            }

                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("CheckCityCookie({0},{1})", modelMaskingName, cityMaskingName));
            }
        }

        /// <summary>
        /// Modified by : Ashutosh Sharma on 13 Nov 2017
        /// Description : Added call to BindAdSlotTags.
        /// Modified by : Ashutosh Sharma on 08 Dec 2017
        /// Description : Removed Images load with Ajax for honda and hero.
        /// Modified By : Deepak Israni on 19 Feb 2018
        /// Description : Sending isNew flag with GetVersionMinSpecs instead of the hardcoded true value.
        /// </summary>
        /// <returns></returns>
        public PriceInCityPageVM GetData()
        {
            PriceInCityPageVM objVM = null;
            try
            {
                if (Status == StatusCodes.ContentFound)
                {
                    objVM = new PriceInCityPageVM();
                    CheckCityCookie(objVM);
                    //Get Bike version Prices
                    objVM.BikeVersionPrices = _objPQ.GetVersionPricesByModelId(modelId, cityId, out hasAreaAvailable);
                    if (objVM.BikeVersionPrices != null && objVM.BikeVersionPrices.Any())
                    {
                        firstVersion = objVM.BikeVersionPrices.OrderByDescending(m => m.IsVersionNew).OrderBy(v => v.ExShowroomPrice).FirstOrDefault();
                        objVM.IsNew = isNew = firstVersion.IsModelNew;
                        var newVersions = objVM.BikeVersionPrices.Where(x => x.IsVersionNew);
                        if (objVM.IsNew && newVersions != null && newVersions.Any())
                        {
                            objVM.BikeVersionPrices = newVersions;
                        }
                        versionCount = (uint)objVM.BikeVersionPrices.Count();
                        objVM.VersionSpecs = _version.GetVersionMinSpecs(modelId, objVM.IsNew);
                        if (objVM.VersionSpecs != null)
                        {
                            var objMin = objVM.VersionSpecs.FirstOrDefault(x => x.VersionId == firstVersion.VersionId);
                            if (objMin != null)
                            {
                                objVM.MinSpecsList = objMin.MinSpecsList;
                                // Set body style
                                objVM.BodyStyle = objMin.BodyStyle;
                            }
                            else
                            {
                                var firstVersionSpec = objVM.VersionSpecs.FirstOrDefault();
                                if (firstVersionSpec != null)
                                {
                                    objVM.BodyStyle = objVM.VersionSpecs.FirstOrDefault().BodyStyle;

                                }
                            }

                            foreach (var version in objVM.VersionSpecs)
                            {
                                var versionPrice = objVM.BikeVersionPrices.FirstOrDefault(m => m.VersionId == version.VersionId);
                                if (versionPrice != null)
                                {
                                    version.Price = versionPrice.OnRoadPrice;
                                }
                            }

                            objVM.BodyStyleText = objVM.BodyStyle == EnumBikeBodyStyles.Scooter ? "Scooters" : "Bikes";
                        }

                        BindBikeBasicDetails(objVM);
                        BindServiceCenters(objVM);
                        BindSimilarBikes(objVM);
                        BindBikeInfoRank(objVM);

                        if (objVM.IsNew)
                        {
                            BindPriceInNearestCities(objVM);
                            BindPriceInTopCities(objVM);
                            if (objVM.CookieCityEntity != null)
                            {
                                if ((objVM.CookieCityEntity.HasAreas && areaId > 0) || !objVM.CookieCityEntity.HasAreas)
                                {
                                    GetDealerPriceQuote(objVM);
                                }
                                else
                                {
                                    if (objVM.CookieCityEntity.HasAreas && areaId == 0)
                                    {
                                        objVM.IsAreaAvailable = true;
                                    }
                                }

                                GetManufacturerCampaign(objVM);
                                objVM.LeadCapture = new LeadCaptureEntity()
                                {
                                    ModelId = modelId,
                                    CityId = cityId,
                                    AreaId = areaId,
                                    Area = area,
                                    City = city,
                                    Location = String.Format("{0} {1}", area, city),
                                    BikeName = objVM.BikeName
                                };
                            }
                        }

                        BindDealersWidget(objVM);

                        var objModelColours = _modelCache.GetModelColor(Convert.ToInt16(modelId));
                        colorCount = (uint)(objModelColours != null ? objModelColours.Count() : 0);

                        objVM.PageDescription = PageDescription();
                        objVM.IsAreaSelected = isAreaSelected;
                        objVM.IsAreaAvailable = hasAreaAvailable;
                        objVM.Page_H1 = String.Format("{0} price in {1}", objVM.BikeName, objVM.CityEntity.CityName);

                        objVM.CookieCityArea = String.Format("{0} {1}", locationCookie.City, locationCookie.Area);
                        BuildPageMetas(objVM);
                        ShowInnovationBanner(objVM, modelId);
                    }
                    else
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                    if (objVM.AlternateBikes != null)
                    {
                        var objVersionSpec = objVM.VersionSpecs.FirstOrDefault();
                        if (objVersionSpec != null)
                        {
                            objVM.AlternateBikes.BodyStyle = objVersionSpec.BodyStyle;
                        }
                    }
                    objVM.Page = Entities.Pages.GAPages.PriceInCity_Page;


                    BindAdSlotTags(objVM);

                    if (objVM.BodyStyle.Equals(EnumBikeBodyStyles.Scooter))
                    {
                        BindMoreAboutScootersWidget(objVM);

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("PriceInCityPage.GetData({0},{1})", modelMaskingName, cityMaskingName));
            }
            return objVM;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 13 Nov 2017
        /// Description : Bind ad slot to adtags.
        /// Modified by : Sanskar Gupta on 21 Mar 2018
        /// Description : Added New way of loading the Ads for Mobile.
        /// </summary>
        private void BindAdSlotTags(PriceInCityPageVM objVM)
        {
            try
            {
                if (objVM.AdTags != null)
                {
                    IDictionary<string, AdSlotModel> ads = new Dictionary<string, AdSlotModel>();
                    var adTag = objVM.AdTags;

                    if (IsMobile)
                    {
                        adTag.AdPath = _adPath_Mobile;
                        adTag.AdId = _adId_Mobile;
                        adTag.Ad_320x50 = !objVM.AdTags.ShowInnovationBannerMobile;
                        adTag.Ad_300x250 = true;
                        adTag.Ad_Bot_320x50 = true;
                        adTag.Ad_200x253 = _adSlot.CheckAdSlotStatus("Ad_200x253");  //For similar bikes widget mobile

                        NameValueCollection adInfo = new NameValueCollection();
                        adInfo["adId"] = _adId_Mobile;
                        adInfo["adPath"] = _adPath_Mobile;

                        if (objVM.AdTags.Ad_320x50)
                            ads.Add(String.Format("{0}-0", _adId_Mobile), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._320x50], 0, 320, AdSlotSize._320x50, "Top", true));

                        if (objVM.AdTags.Ad_300x250)
                            ads.Add(String.Format("{0}-2", _adId_Mobile), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._300x250], 2, 300, AdSlotSize._300x250));

                        if (objVM.AdTags.Ad_Bot_320x50)
                            ads.Add(String.Format("{0}-1", _adId_Mobile), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._320x50], 1, 320, AdSlotSize._320x50, "Bottom"));

                        if (objVM.AdTags.Ad_200x253)
                        {
                            NameValueCollection adInfo_OldAd = new NameValueCollection();
                            adInfo_OldAd["adId"] = _adId_SimilarBikes;
                            adInfo_OldAd["adPath"] = _adPath_SimilarBikes_Mobile;
                            ads.Add(String.Format("{0}-11", adInfo_OldAd["adId"]), GoogleAdsHelper.SetAdSlotProperties(adInfo_OldAd, ViewSlotSize.ViewSlotSizes[AdSlotSize._200x253], 11, 200, AdSlotSize._200x253));
                        }
                    }
                    else
                    {
                        adTag.AdPath = _adPath_Desktop;
                        adTag.AdId = _adId_Desktop;
                        adTag.Ad_300x250 = objVM.IsNew;
                        adTag.Ad_Model_BTF_300x250 = (objVM.NearestPriceCities != null && objVM.NearestPriceCities.PriceQuoteList != null && objVM.NearestPriceCities.PriceQuoteList.Count() > 8) ? true : false;
                        adTag.Ad_970x90 = !objVM.AdTags.ShowInnovationBannerDesktop;
                        adTag.Ad_970x90Bottom = true;
                        adTag.Ad_292x399 = _adSlot.CheckAdSlotStatus("Ad_292x399");  //For similar bikes widget desktop

                        NameValueCollection adInfo = new NameValueCollection();
                        adInfo["adId"] = _adId_Desktop;
                        adInfo["adPath"] = _adPath_Desktop;

                        if (objVM.AdTags.Ad_300x250)
                        {
                            ads.Add(String.Format("{0}-1", _adId_Desktop), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._300x250], 1, 300, AdSlotSize._300x250, true));
                        }
                        if (objVM.AdTags.Ad_Model_BTF_300x250)
                        {
                            ads.Add(String.Format("{0}-11", _adId_Desktop), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._300x250], 11, 300, AdSlotSize._300x250, "BTF"));

                        }
                        if (objVM.AdTags.Ad_970x90)
                        {
                            ads.Add(String.Format("{0}-3", _adId_Desktop), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._970x90 + "_C"], 3, 970, AdSlotSize._970x90, true));
                        }
                        if (objVM.AdTags.Ad_970x90Bottom)
                        {
                            ads.Add(String.Format("{0}-5", _adId_Desktop), GoogleAdsHelper.SetAdSlotProperties(adInfo, ViewSlotSize.ViewSlotSizes[AdSlotSize._970x90 + "_C"], 5, 970, AdSlotSize._970x90, "Bottom"));

                        }
                        if (objVM.AdTags.Ad_292x399)
                        {
                            NameValueCollection adInfo_OldAd = new NameValueCollection();
                            adInfo_OldAd["adId"] = _adId_SimilarBikes;
                            adInfo_OldAd["adPath"] = _adPath_SimilarBikes_Desktop;
                            ads.Add(String.Format("{0}-14", adInfo_OldAd["adId"]), GoogleAdsHelper.SetAdSlotProperties(adInfo_OldAd, ViewSlotSize.ViewSlotSizes[AdSlotSize._292x399], 14, 292, AdSlotSize._292x399));
                        }
                    }

                    objVM.AdSlots = ads;

                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "PriceInCityPage.BindAdSlotTags");
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Sep 2017
        /// Description :   To Show Innovation Banner
        /// </summary>
        /// <param name="_modelId"></param>
        private void ShowInnovationBanner(PriceInCityPageVM objData, uint _modelId)
        {
            try
            {
                if (!String.IsNullOrEmpty
                    (BWConfiguration.Instance.InnovationBannerModels))
                {
                    objData.AdTags.ShowInnovationBannerDesktop = objData.AdTags.ShowInnovationBannerMobile = BWConfiguration.Instance.InnovationBannerModels.Split(',').Contains(_modelId.ToString());
                    objData.AdTags.InnovationBannerGALabel =
                        String.Format("{0}_PriceIn_{1}", objData.BikeName.Replace(" ", "_"), objData.CityEntity.CityName.Replace(" ", "_"));
                    //String.Join("_", objData.BikeName.Replace(" ", "_"), objData.FirstVersion.City.Replace(" ", "_"));
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, String.Format("ShowInnovationBanner({0})", _modelId));
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 06-Sep-2017
        /// Description : Get data for PriceInCity AMP page
        /// Modified by : Ashutosh Sharma on 27 Oct 2017
        /// Description : Added call to BindAmpJsTags.
        /// Modified by : Ashutosh Sharma on 11 Dec 2017
        /// Description : Added IsNew check for GetManufacturerCampaign and BindManufacturerLeadAdAMP
        /// Modified by : Deepak Israni on 20 Dec 2018
        /// Description : Fixed call to GetVersionMinSpecs.
        /// </summary>
        /// <returns></returns>
        public PriceInCityPageAMPVM GetDataAMP()
        {
            PriceInCityPageAMPVM objVM = null;
            try
            {
                if (Status == StatusCodes.ContentFound)
                {
                    objVM = new PriceInCityPageAMPVM();
                    CheckCityCookie(objVM);
                    //Get Bike version Prices
                    IEnumerable<BikeQuotationEntity> objBikePQList = objVM.BikeVersionPrices = _objPQ.GetVersionPricesByModelId(modelId, cityId, out hasAreaAvailable);
                    ICollection<BikeQuotationAMPEntity> objBikePQAMPList = new List<BikeQuotationAMPEntity>();
                    BikeQuotationAMPEntity objPq = null;
                    foreach (var item in objBikePQList)
                    {
                        objPq = new BikeQuotationAMPEntity();
                        objPq.BikeQuotationEntity = item;
                        objPq.FormatedExShowroomPrice = Format.FormatPrice(Convert.ToString(item.ExShowroomPrice));
                        objPq.FormatedInsurance = Format.FormatPrice(Convert.ToString(item.Insurance));
                        objPq.FormatedOnRoadPrice = Format.FormatPrice(Convert.ToString(item.OnRoadPrice));
                        objPq.FormatedRTO = Format.FormatPrice(Convert.ToString(item.RTO));
                        objBikePQAMPList.Add(objPq);
                    }
                    objVM.FormatedBikeVersionPrices = objBikePQAMPList;

                    if (objVM.FormatedBikeVersionPrices != null && objVM.FormatedBikeVersionPrices.Any())
                    {
                        firstVersion = objVM.FormatedBikeVersionPrices.OrderByDescending(m => m.BikeQuotationEntity.IsVersionNew).OrderBy(v => v.BikeQuotationEntity.ExShowroomPrice).First().BikeQuotationEntity;
                        objVM.IsNew = isNew = firstVersion.IsModelNew;
                        var newVersions = objVM.FormatedBikeVersionPrices.Where(x => x.BikeQuotationEntity.IsVersionNew);
                        if (objVM.IsNew && newVersions != null && newVersions.Any())
                        {
                            objVM.FormatedBikeVersionPrices = newVersions;
                            objVM.BikeVersionPrices = objVM.BikeVersionPrices.Where(x => x.IsVersionNew);
                        }
                        versionCount = (uint)objVM.FormatedBikeVersionPrices.Count();
                        objVM.VersionSpecs = _version.GetVersionMinSpecs(modelId, objVM.IsNew);

                        ICollection<KeyValuePair<uint, BikeQuotationAMPEntity>> values = new Dictionary<uint, BikeQuotationAMPEntity>();
                        foreach (var item in objVM.FormatedBikeVersionPrices)
                        {
                            values.Add(new KeyValuePair<uint, BikeQuotationAMPEntity>(item.BikeQuotationEntity.VersionId, item));
                        }
                        objVM.JSONBikeVersions = JsonConvert.SerializeObject(values);
                        if (objVM.VersionSpecs != null)
                        {
                            var objMin = objVM.VersionSpecs.FirstOrDefault(x => x.VersionId == firstVersion.VersionId);
                            if (objMin != null)
                            {
                                objVM.MinSpecsList = objMin.MinSpecsList;

                                // Set body style
                                objVM.BodyStyle = objMin.BodyStyle;
                            }
                            else
                            {
                                var firstVersionSpec = objVM.VersionSpecs.FirstOrDefault();
                                if (firstVersionSpec != null)
                                {
                                    objVM.BodyStyle = objVM.VersionSpecs.FirstOrDefault().BodyStyle;
                                }
                            }

                            foreach (var version in objVM.VersionSpecs)
                            {
                                var versionPrice = objVM.FormatedBikeVersionPrices.FirstOrDefault(m => m.BikeQuotationEntity.VersionId == version.VersionId);
                                if (versionPrice != null)
                                {
                                    version.Price = Convert.ToUInt64(versionPrice.BikeQuotationEntity.OnRoadPrice);
                                }
                            }
                            objVM.BodyStyleText = objVM.BodyStyle == EnumBikeBodyStyles.Scooter ? "Scooters" : "Bikes";
                        }

                        if (firstVersion != null)
                        {
                            BindBikeBasicDetails(objVM);

                            if (firstVersion.OnRoadPrice > 0)
                            {
                                BindEMISlider(objVM);
                                BindSimilarBikes(objVM);
                            }
                        }


                        BindServiceCenters(objVM);

                        BindBikeInfoRank(objVM);

                        if (objVM.IsNew)
                        {
                            BindPriceInNearestCities(objVM);
                            BindPriceInTopCities(objVM);
                            if (objVM.CookieCityEntity != null)
                            {
                                if ((objVM.CookieCityEntity.HasAreas && areaId > 0) || !objVM.CookieCityEntity.HasAreas)
                                {
                                    GetDealerPriceQuote(objVM);
                                }
                                else
                                {
                                    if (objVM.CookieCityEntity.HasAreas && areaId == 0)
                                    {
                                        objVM.IsAreaAvailable = true;
                                    }
                                }
                            }


                            objVM.LeadCapture = new LeadCaptureEntity()
                            {
                                ModelId = modelId,
                                CityId = cityId,
                                AreaId = areaId,
                                Area = area,
                                City = city,
                                Location = String.Format("{0} {1}", area, city),
                                BikeName = objVM.BikeName
                            };
                        }

                        BindDealersWidget(objVM);

                        var objModelColours = _modelCache.GetModelColor(Convert.ToInt16(modelId));
                        colorCount = (uint)(objModelColours != null ? objModelColours.Count() : 0);

                        objVM.PageDescription = PageDescription();
                        objVM.IsAreaSelected = isAreaSelected;
                        objVM.IsAreaAvailable = hasAreaAvailable;
                        objVM.Page_H1 = String.Format("{0} price in {1}", objVM.BikeName, objVM.CityEntity.CityName);

                        objVM.CookieCityArea = String.Format("{0} {1}", locationCookie.City, locationCookie.Area);
                        #region Do not change the order
                        BuildPageMetas(objVM);
                        if (objVM.IsNew)
                        {
                            GetManufacturerCampaign(objVM);
                            BindManufacturerLeadAdAMP(objVM);
                        }

                        #endregion

                    }
                    else
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                    if (objVM.AlternateBikes != null)
                    {
                        var objVersionSpec = objVM.VersionSpecs.FirstOrDefault();
                        if (objVersionSpec != null)
                        {
                            objVM.AlternateBikes.BodyStyle = objVersionSpec.BodyStyle;
                        }
                    }
                    objVM.Page = Entities.Pages.GAPages.PriceInCity_Page;
                    BindAmpJsTags(objVM);
                    Series = _objModelEntity.GetSeriesByModelId(modelId);
                    SetBreadcrumList(objVM);
                    if (objVM.BodyStyle.Equals(EnumBikeBodyStyles.Scooter))
                    {
                        BindMoreAboutScootersWidget(objVM);
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GetDataAMP({0},{1})", modelMaskingName, cityMaskingName));
            }
            return objVM;
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 27 Oct 2017
        /// Description : Method to bind required JS for AMP page.
        /// </summary>
        /// <param name="objVM"></param>
        private void BindAmpJsTags(PriceInCityPageAMPVM objVM)
        {
            try
            {
                objVM.AmpJsTags = new Entities.Models.AmpJsTags();
                objVM.AmpJsTags.IsAccordion = true;
                objVM.AmpJsTags.IsAd = true;
                objVM.AmpJsTags.IsAnalytics = true;
                objVM.AmpJsTags.IsBind = true;
                objVM.AmpJsTags.IsCarousel = true;
                objVM.AmpJsTags.IsSelector = (objVM.FormatedBikeVersionPrices != null && objVM.FormatedBikeVersionPrices.Count() > 1);
                objVM.AmpJsTags.IsSidebar = true;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BindAmpJsTags_{0}", objVM));
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 10 Sep 2017
        /// Description : Bind Manufacturer Lead Ad and href, remove AMP prohibitated attribute
        /// Modified by : Ashutosh Sharma on 11 Dec 2017
        /// Description : LeadCapture null check added.
        /// </summary>
        /// <param name="priceInCityAMPVM"></param>
        private void BindManufacturerLeadAdAMP(PriceInCityPageAMPVM priceInCityAMPVM)
        {
            string str = string.Empty;
            if (priceInCityAMPVM.LeadCampaign != null && priceInCityAMPVM.LeadCapture != null)
            {

                try
                {
                    priceInCityAMPVM.LeadCampaign.IsAmp = true;
                    str = MvcHelper.GetRenderedContent(String.Format("LeadCampaign_Mobile_AMP_{0}", priceInCityAMPVM.LeadCampaign.CampaignId), priceInCityAMPVM.LeadCampaign.LeadsHtmlMobile, priceInCityAMPVM.LeadCampaign);

                    // Code to remove name attribute form span tags, remove style css tag and replace javascript:void(0) in href with url (not supported in AMP)

                    if (!string.IsNullOrEmpty(str))
                    {
                        str = str.ConvertToAmpContent();
                        str = str.RemoveAttribure("name");
                        str = str.RemoveStyleElement();

                        string url = "/m/popup/leadcapture/?q=" + Utils.Utils.EncryptTripleDES(string.Format(@"modelid={0}&cityid={1}&areaid={2}&bikename={3}&location={4}&city={5}&area={6}&ismanufacturer={7}&dealerid={8}&dealername={9}&dealerarea={10}&versionid={11}&leadsourceid={12}&pqsourceid={13}&mfgcampid={14}&pqid={15}&pageurl={16}&clientip={17}&dealerheading={18}&dealermessage={19}&dealerdescription={20}&pincoderequired={21}&emailrequired={22}&dealersrequired={23}&url={24}&sendLeadSMSCustomer={25}&organizationName={26}",
                                               priceInCityAMPVM.BikeModel.ModelId, priceInCityAMPVM.CityEntity.CityId, string.Empty, string.Format(priceInCityAMPVM.BikeName), string.Empty, string.Empty, string.Empty,
                                               priceInCityAMPVM.IsManufacturerLeadAdShown, priceInCityAMPVM.LeadCampaign.DealerId, String.Format(priceInCityAMPVM.LeadCampaign.LeadsPropertyTextMobile,
                                               priceInCityAMPVM.LeadCampaign.Organization), priceInCityAMPVM.LeadCampaign.Area, priceInCityAMPVM.VersionId, priceInCityAMPVM.LeadCampaign.LeadSourceId, priceInCityAMPVM.LeadCampaign.PqSourceId,
                                               priceInCityAMPVM.LeadCampaign.CampaignId, priceInCityAMPVM.PQId, string.Empty, Bikewale.Common.CommonOpn.GetClientIP(), priceInCityAMPVM.LeadCampaign.PopupHeading,
                                               String.Format(priceInCityAMPVM.LeadCampaign.PopupSuccessMessage, priceInCityAMPVM.LeadCampaign.Organization), priceInCityAMPVM.LeadCampaign.PopupDescription,
                                               priceInCityAMPVM.LeadCampaign.PincodeRequired, priceInCityAMPVM.LeadCampaign.EmailRequired, priceInCityAMPVM.LeadCampaign.DealerRequired,
                                               string.Format("{0}/m/{1}-bikes/{2}/price-in-{3}/", BWConfiguration.Instance.BwHostUrlForJs, firstVersion.MakeMaskingName, modelMaskingName, cityMaskingName),priceInCityAMPVM.LeadCampaign.SendLeadSMSCustomer,priceInCityAMPVM.LeadCampaign.Organization));

                        str = str.ReplaceHref("leadcapturebtn", url);

                        priceInCityAMPVM.LeadCapture.ManufacturerLeadAdAMPConvertedContent = str;
                    }
                }
                catch (Exception ex)
                {
                    Bikewale.Notifications.ErrorClass.LogError(ex, String.Format("ManufacturerCampaign.Mobile.AMP(CampaignId : {0})", priceInCityAMPVM.LeadCampaign.CampaignId));
                }

            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 08-Sep-2017
        /// Description : Method to bind required parameters for PriceInCity AMP page EMI Calculator slider
        /// </summary>
        /// <param name="objVM"></param>
        private void BindEMISlider(PriceInCityPageAMPVM objVM)
        {
            try
            {
                if (objVM != null && objVM.FirstVersion != null)
                {
                    ulong bikePrice = objVM.FirstVersion.OnRoadPrice;
                    double loanAmount = Math.Round(objVM.FirstVersion.OnRoadPrice * .7);
                    int downPayment = Convert.ToInt32(bikePrice - loanAmount);

                    float minDnPay = (float)(10 * bikePrice) / 100;
                    float maxDnPay = (float)(40 * bikePrice) / 100;

                    ushort minTenure = 12;
                    ushort maxTenure = 48;

                    int minROI = 10;
                    int maxROI = 15;

                    float rateOfInterest = Convert.ToSingle((maxROI - minROI) / 2.0 + minROI);

                    ushort tenure = (ushort)((maxTenure - minTenure) / 2 + minTenure);

                    int procFees = 0;
                    int monthlyEMI = 0;
                    if (tenure != 0)
                    {
                        monthlyEMI = Convert.ToInt32(Math.Round((loanAmount * rateOfInterest / 1200) / (1 - Math.Pow((1 + (rateOfInterest / 1200)), (-1.0 * tenure)))));
                    }

                    int totalAmount = downPayment + monthlyEMI * tenure + procFees;

                    objVM.EMI = new EMI();
                    objVM.EMI.MinDownPayment = minDnPay;
                    objVM.EMI.MaxDownPayment = maxDnPay;

                    objVM.EMI.MinTenure = minTenure;
                    objVM.EMI.MaxTenure = maxTenure;

                    objVM.EMI.MinRateOfInterest = minROI;
                    objVM.EMI.MaxRateOfInterest = maxROI;

                    objVM.EMI.RateOfInterest = rateOfInterest;
                    objVM.EMI.Tenure = tenure;


                    objVM.EMISliderAMP = new EMISliderAMP();
                    objVM.EMISliderAMP.TotalAmount = Format.FormatPrice(Convert.ToString(totalAmount));
                    objVM.EMISliderAMP.FormatedTotalAmount = "0";
                    objVM.EMISliderAMP.DownPayment = Convert.ToString(downPayment);
                    objVM.EMISliderAMP.FormatedDownPayment = "0";
                    objVM.EMISliderAMP.LoanAmount = Convert.ToString((int)loanAmount);
                    objVM.EMISliderAMP.FormatedLoanAmount = "0";
                    objVM.EMISliderAMP.Tenure = tenure;
                    objVM.EMISliderAMP.FormatedTenure = "0";
                    objVM.EMISliderAMP.RateOfInterest = rateOfInterest;
                    objVM.EMISliderAMP.Fees = procFees;
                    objVM.EMISliderAMP.BikePrice = bikePrice;
                    objVM.EMISliderAMP.EMI = "0";

                    objVM.JSONEMISlider = JsonConvert.SerializeObject(objVM.EMISliderAMP);
                    objVM.EMISliderAMP.EMI = Convert.ToString(monthlyEMI);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BindEMISlider({0})", objVM));
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 11 Apr 2017
        /// Description :   Bind Price in Top Cities
        /// </summary>
        /// <param name="objVM"></param>
        private void BindPriceInTopCities(PriceInCityPageVM objVM)
        {
            try
            {
                uint[] topCityId = BWConfiguration.Instance.PopularCitiesId.Split(',').Select(uint.Parse).ToArray();
                objVM.PriceInTopCities = new PriceInTopCities(_objPQCache, modelId, 8).GetData();
                if (objVM.HasPriceInTopCities)
                {
                    objVM.PriceInTopCities.PriceQuoteList = objVM.PriceInTopCities.PriceQuoteList.Where(m => !m.CityMaskingName.Equals(cityMaskingName));
                    objVM.PriceInTopCities.PriceQuoteList = objVM.PriceInTopCities.PriceQuoteList.Where(m => topCityId.Contains(m.CityId));
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BindPriceInTopCities({0},{1})", modelMaskingName, cityMaskingName));
            }
        }


        /// <summary>
        /// Created by  :   Sumit Kate on 28 Mar 2017
        /// Description :   Bind Bike Basic Details
        /// </summary>
        /// <param name="objVM"></param>
        private void BindBikeBasicDetails(PriceInCityPageVM objVM)
        {
            try
            {
                objVM.Make = new BikeMakeEntityBase() { MakeName = firstVersion.MakeName, MaskingName = firstVersion.MakeMaskingName, MakeId = (int)firstVersion.MakeId, IsScooterOnly = firstVersion.IsScooterOnly };
                objVM.BikeModel = new BikeModelEntityBase() { ModelId = (int)modelId, ModelName = firstVersion.ModelName, MaskingName = firstVersion.ModelMaskingName };
                objVM.ModelImage = objVM.PageMetaTags.OGImage = Image.GetPathToShowImages(firstVersion.OriginalImage, firstVersion.HostUrl, ImageSize._310x174, QualityFactor._75);
                objVM.CityEntity = new CityEntityBase() { CityId = cityId, CityMaskingName = cityMaskingName, CityName = firstVersion.City };
                objVM.VersionId = firstVersion.VersionId;

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BindBikeBasicDetails({0},{1})", modelMaskingName, cityMaskingName));
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Mar 2017
        /// Description :   Bind Bike Info rank
        /// </summary>
        /// <param name="objVM"></param>
        private void BindBikeInfoRank(PriceInCityPageVM objVM)
        {
            try
            {

                objVM.BikeInfo = (new BikeInfoWidget(_bikeInfo, _objCityCache, modelId, cityId, BikeInfoTabCount, Entities.GenericBikes.BikeInfoTabType.PriceInCity)).GetData();
                objVM.BikeRank = (new BikeModelRank(_modelCache, modelId)).GetData();
                objVM.IsElectricBike = objVM.BikeInfo.BikeInfo.FuelType.Equals(5);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BindBikeInfoRank({0},{1})", modelMaskingName, cityMaskingName));
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Mar 2017
        /// Description :   Bind Similar Bikes
        /// Modified by: Vivek Singh Tomar on 23 Aug 2017
        /// Summary: Added page enum to similar bike widget
        /// Modified by : Vivek Singh Tomar on 27th Oct 2017
        /// Description: Add city and return url details for redirection from amp to bikewale pages on check on road CTA popup
        /// </summary>
        /// <param name="objVM"></param>
        private void BindSimilarBikes(PriceInCityPageVM objVM)
        {
            try
            {
                var similarBikes = new SimilarBikesWidget(_version, firstVersion.VersionId, pqSource, false, true);
                similarBikes.CityId = cityId;
                similarBikes.TopCount = 9;
                similarBikes.IsNew = objVM.IsNew;
                similarBikes.IsDiscontinued = objVM.IsDiscontinued;
                var similarBikesVM = similarBikes.GetData();
                if (similarBikesVM != null)
                {
                    similarBikesVM.Make = objVM.Make;
                    similarBikesVM.Model = objVM.BikeModel;
                    similarBikesVM.VersionId = firstVersion.VersionId;
                    similarBikesVM.ReturnUrlForAmpPages = string.Format("{0}/m/{1}-bikes/{2}/price-in-{3}", BWConfiguration.Instance.BwHostUrl, objVM.Make.MaskingName, objVM.BikeModel.MaskingName, objVM.CityEntity.CityMaskingName);
                    similarBikesVM.CityId = objVM.CityEntity.CityId;
                    objVM.AlternateBikes = similarBikesVM;
                    objVM.AlternateBikes.Page = Entities.Pages.GAPages.PriceInCity_Page;
                }
                if (!objVM.HasAlternateBikes)
                {
                    BindPopularBodyStyle(objVM);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BindSimilarBikes({0},{1})", modelMaskingName, cityMaskingName));
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 30 Aug 2017
        /// Description :   Bind Popular by BodyStyle
        /// </summary>
        /// <param name="objData"></param>
        private void BindPopularBodyStyle(PriceInCityPageVM objData)
        {
            try
            {
                if (modelId > 0)
                {
                    var modelPopularBikesByBodyStyle = new PopularBikesByBodyStyle(_objModelEntity);
                    modelPopularBikesByBodyStyle.CityId = cityId;
                    modelPopularBikesByBodyStyle.ModelId = modelId;
                    modelPopularBikesByBodyStyle.TopCount = 9;

                    objData.PopularBodyStyle = modelPopularBikesByBodyStyle.GetData();
                    objData.PopularBodyStyle.PQSourceId = PQSource;
                    objData.PopularBodyStyle.ShowCheckOnRoadCTA = true;
                    objData.PopularBodyStyle.ReturnUrlForAmpPages = string.Format("{0}/m/{1}-bikes/{2}/price-in-{3}", BWConfiguration.Instance.BwHostUrl, objData.Make.MaskingName, objData.BikeModel.MaskingName, objData.CityEntity.CityMaskingName);
                    objData.PopularBodyStyle.CityId = objData.CityEntity.CityId;
                    objData.BodyStyle = objData.PopularBodyStyle.BodyStyle;
                    objData.BodyStyleText = objData.BodyStyle == EnumBikeBodyStyles.Scooter ? "Scooters" : "Bikes";
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Models.PriceInCity.BindPopularBodyStyle({0},{1})", modelId, cityId));
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Mar 2017
        /// Description :   Bind Service centers
        /// Modified by : Aditi Srivatava on 10 Apr 2017
        /// Summary     : Added service center count
        /// </summary>
        /// <param name="objVM"></param>
        private void BindServiceCenters(PriceInCityPageVM objVM)
        {
            try
            {
                var serviceCenters = _objServiceCenter.GetServiceCentersByCity(cityId, objVM.Make.MakeId);
                if (serviceCenters != null)
                    objVM.ServiceCentersCount = serviceCenters.Count;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BindServiceCenters({0},{1})", modelMaskingName, cityMaskingName));
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Mar 2017
        /// Description :   Bind Dealers
        /// </summary>
        /// <param name="objVM"></param>
        private void BindDealersWidget(PriceInCityPageVM objVM)
        {
            try
            {
                DealerCardWidget objDealer = new DealerCardWidget(_objDealerCache, cityId, firstVersion.MakeId);
                objDealer.ModelId = modelId;
                objDealer.TopCount = TopCount;
                if (primaryDealerId > 0)
                    objDealer.DealerId = primaryDealerId;
                objVM.Dealers = objDealer.GetData();
                dealerCount = (uint)(objVM.HasDealers ? objVM.Dealers.TotalCount : 0);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BindDealersWidget({0},{1})", modelMaskingName, cityMaskingName));
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Mar 2017
        /// Description :   Bind PriceInNearestCities widget
        /// </summary>
        /// <param name="objVM"></param>
        private void BindPriceInNearestCities(PriceInCityPageVM objVM)
        {
            try
            {
                objVM.NearestPriceCities = new ModelPriceInNearestCities(_objPQCache, modelId, cityId, (ushort)NearestCityCount).GetData();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BindPriceInNearestCities({0},{1})", modelMaskingName, cityMaskingName));
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Mar 2017
        /// Description :   Page Description
        /// </summary>
        /// <returns></returns>
        private string PageDescription()
        {
            try
            {
                char multiVersion = '\0', multiDealer = '\0';

                if (versionCount > 1)
                    multiVersion = 's';

                if (dealerCount > 1)
                    multiDealer = 's';

                string multiColour = ".";

                if (colorCount > 1)
                    multiColour = string.Format(" and {0} colors.", colorCount);
                else if (colorCount == 1)
                    multiColour = string.Format(" and 1 colour.");

                if (firstVersion != null)
                {
                    string newBikeDescription = string.Format("{0} {1} on-road price in {2} -   &#x20B9; {3} onwards. It is available in {4} version{5}{6}", firstVersion.MakeName, firstVersion.ModelName, firstVersion.City, Bikewale.Common.CommonOpn.FormatPrice(firstVersion.OnRoadPrice.ToString()), versionCount, multiVersion, multiColour);

                    if (dealerCount > 0)
                        newBikeDescription = string.Format("{0} {1} is sold by {2} dealership{3} in {4}.", newBikeDescription, firstVersion.ModelName, dealerCount, multiDealer, firstVersion.City);

                    newBikeDescription = string.Format("{0} All the colour options and versions of {1} might not be available at all the dealerships in {2}.", newBikeDescription, firstVersion.ModelName, firstVersion.City);

                    string discontinuedDescription = string.Format("The last known ex-showroom price of {0} {1} in {2} was   &#x20B9; {3} onwards. This bike has now been discontinued. It was available in {4} version{5}{6} Click on a {1} version name to know the last known ex-showroom price in {2}.", firstVersion.MakeName, firstVersion.ModelName, firstVersion.City, Bikewale.Common.CommonOpn.FormatPrice(firstVersion.OnRoadPrice.ToString()), versionCount, multiVersion, multiColour);

                    if (isNew)
                        pageDescription = newBikeDescription;
                    else
                        pageDescription = discontinuedDescription;
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("PageDescription({0},{1})", modelMaskingName, cityMaskingName));
            }
            return pageDescription;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 mar 2017
        /// Description :   Binds the page metas
        /// Modified By :- Subodh Jain 2 june 2017
        /// Added target city and model
        /// Modified by : Ashutosh Sharma on 30 Aug 2017 
        /// Description : Removed GST from Title and Description 
        /// Modified by : Snehal Dange on 29th Jan 2018
        /// Description: Modified title for the page
        /// </summary>
        /// <param name="metas"></param>
        private void BuildPageMetas(PriceInCityPageVM objVM)
        {
            try
            {
                string bikeName = String.Format("{0} {1}", firstVersion.MakeName, firstVersion.ModelName);
                objVM.PageMetaTags.AlternateUrl = string.Format("{0}/m/{1}-bikes/{2}/price-in-{3}/", BWConfiguration.Instance.BwHostUrlForJs, firstVersion.MakeMaskingName, modelMaskingName, cityMaskingName);
                objVM.PageMetaTags.CanonicalUrl = string.Format("{0}/{1}-bikes/{2}/price-in-{3}/", BWConfiguration.Instance.BwHostUrlForJs, firstVersion.MakeMaskingName, modelMaskingName, cityMaskingName);
                objVM.PageMetaTags.Title = string.Format("{0} price in {1} | Check on-road price - BikeWale", bikeName, firstVersion.City);
                objVM.ReturnUrl = string.Format("/m/{1}-bikes/{2}/price-in-{3}/", BWConfiguration.Instance.BwHostUrlForJs, firstVersion.MakeMaskingName, modelMaskingName, cityMaskingName);
                objVM.PageMetaTags.AmpUrl = string.Format("{0}/m/{1}-bikes/{2}/price-in-{3}/amp/", BWConfiguration.Instance.BwHostUrlForJs, firstVersion.MakeMaskingName, modelMaskingName, cityMaskingName);

                if (firstVersion != null && !isNew)
                    objVM.PageMetaTags.Description = string.Format("{0} price in {1} - Rs. {2} (Ex-Showroom price). Get its detailed on road price in {1}. Check your nearest {0} Dealer in {1}", bikeName, firstVersion.City, Bikewale.Common.CommonOpn.FormatPrice(firstVersion.ExShowroomPrice.ToString()));
                else if (firstVersion != null)
                    objVM.PageMetaTags.Description = string.Format("{0} price in {1} - Rs. {2} (Ex-Showroom price). Get prices for all the versions of and check out the nearest {0} Dealer in {1}", bikeName, firstVersion.City, Bikewale.Common.CommonOpn.FormatPrice(firstVersion.ExShowroomPrice.ToString()));
                objVM.PageMetaTags.Keywords = string.Format("{0} price in {1}, {0} on-road price, {0} bike, buy {0} bike in {1}, new {2} price", bikeName, firstVersion.City, firstVersion.ModelName);

                objVM.AdTags.TargetedCity = firstVersion.City;
                objVM.AdTags.TargetedModel = firstVersion.ModelName;

                Series = _objModelEntity.GetSeriesByModelId(modelId);
                SetBreadcrumList(objVM);

                SetPageJSONLDSchema(objVM);


            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BuildPageMetas({0},{1})", modelMaskingName, cityMaskingName));
            }
        }

        /// <summary>
        /// Created By  : Sushil Kumar on 14th Sep 2017
        /// Description : Added breadcrum and webpage schema
        /// </summary>
        private void SetPageJSONLDSchema(PriceInCityPageVM objPageMeta)
        {
            //set webpage schema for the model page
            WebPage webpage = SchemaHelper.GetWebpageSchema(objPageMeta.PageMetaTags, objPageMeta.BreadcrumbList);

            if (webpage != null)
            {
                objPageMeta.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12th Sep 2017
        /// Description : Function to create page level schema for breadcrum
        /// Modified by Sajal Gupta on 02-11-2017
        /// Descriptition : Changed breadcrumb for scooter
        /// Modified by : Snehal Dange on 27th Dec 2017
        /// Description: Added 'new bikes' in breadcrumb
        /// </summary>
        private void SetBreadcrumList(PriceInCityPageVM objPage)
        {
            try
            {
                IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
                string url, scooterUrl, seriesUrl;
                url = scooterUrl = string.Format("{0}/", BWConfiguration.Instance.BwHostUrl);
                ushort position = 1;
                if (IsMobile)
                {
                    url += "m/";
                }

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Home"));
                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, string.Format("{0}new-bikes-in-india/", url), "New Bikes"));

                if (objPage.Make != null)
                {
                    url = string.Format("{0}{1}-bikes/", url, objPage.Make.MaskingName);

                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, string.Format("{0} Bikes", objPage.Make.MakeName)));
                }

                if (objPage.Make != null && objPage.BodyStyle.Equals(EnumBikeBodyStyles.Scooter) && !objPage.Make.IsScooterOnly)
                {
                    if (IsMobile)
                    {
                        scooterUrl += "m/";
                    }

                    scooterUrl = string.Format("{0}{1}-scooters/", scooterUrl, objPage.Make.MaskingName);

                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, scooterUrl, string.Format("{0} Scooters", objPage.Make.MakeName)));
                }

                if (Series != null && Series.IsSeriesPageUrl)
                {

                    seriesUrl = string.Format("{0}{1}/", url, Series.MaskingName);

                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, seriesUrl, Series.SeriesName));
                }

                if (objPage.Make != null && objPage.BikeModel != null)
                {
                    url = string.Format("{0}{1}/", url, objPage.BikeModel.MaskingName);

                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, objPage.BikeModel.ModelName));
                }

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, null, objPage.Page_H1));


                objPage.BreadcrumbList.BreadcrumListItem = BreadCrumbs;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("SetBreadcrumList({0},{1})", modelMaskingName, cityMaskingName));
            }

        }

        private void GetDealerPriceQuote(PriceInCityPageVM objVM)
        {
            PQOutputEntity objPQOutput = null;
            try
            {
                PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                objPQEntity.CityId = Convert.ToUInt16(cityId);
                objPQEntity.AreaId = isAreaSelected ? areaId : 0;
                objPQEntity.ClientIP = "";
                objPQEntity.SourceId = Convert.ToUInt16(Platform);
                objPQEntity.ModelId = modelId;
                objPQEntity.PQLeadId = Convert.ToUInt16(PQSource);
                objPQEntity.UTMA = HttpContext.Current.Request.Cookies["__utma"] != null ? HttpContext.Current.Request.Cookies["__utma"].Value : "";
                objPQEntity.UTMZ = HttpContext.Current.Request.Cookies["_bwutmz"] != null ? HttpContext.Current.Request.Cookies["_bwutmz"].Value : "";
                objPQEntity.DeviceId = HttpContext.Current.Request.Cookies["BWC"] != null ? HttpContext.Current.Request.Cookies["BWC"].Value : "";
                objPQOutput = _objDealerPQ.ProcessPQV2(objPQEntity);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("GetDealerPriceQuote({0},{1})", modelMaskingName, cityMaskingName));
            }
            finally
            {
                if (objPQOutput != null)
                {
                    objVM.PQId = objPQOutput.PQId;
                    if (objPQOutput.IsDealerAvailable)
                    {
                        try
                        {
                            primaryDealerId = objPQOutput.DealerId;
                            objVM.DetailedDealer = _objDealerDetails.GetDealerQuotationV2(cityId, objPQOutput.VersionId, objPQOutput.DealerId, areaId);
                            objVM.MPQString = EncodingDecodingHelper.EncodeTo64(Bikewale.Common.PriceQuoteQueryString.FormQueryString(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), objPQOutput.VersionId.ToString(), objPQOutput.DealerId.ToString()));
                        }
                        catch (Exception ex)
                        {
                            ErrorClass.LogError(ex, String.Format("GetDealerQuotationV2({0},{1})", modelMaskingName, cityMaskingName));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Jun 2017
        /// Description :   Fetches Manufacturer Campaigns
        /// Modified by  :  Sushil Kumar on 11th Aug 2017
        /// Description :   Store dealerid for manufacturer campaigns for impressions tracking
        /// Modified by : Ashutosh Sharma on 13 Mar 2018
        /// Description : Registering price quote before binding lead and emi campaign.
        /// </summary>
        private void GetManufacturerCampaign(PriceInCityPageVM objData)
        {
            try
            {
                if (_objManufacturerCampaign != null && !(objData.HasCampaignDealer))
                {
                    if (objData.PQId == 0)
                    {
                        PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                        objPQEntity.CityId = Convert.ToUInt16(cityId);
                        objPQEntity.AreaId = Convert.ToUInt32(areaId);
                        objPQEntity.ClientIP = "";
                        objPQEntity.SourceId = Convert.ToUInt16(Platform);
                        objPQEntity.ModelId = modelId;
                        objPQEntity.VersionId = objData.VersionId;
                        objPQEntity.PQLeadId = Convert.ToUInt16(PQSource);
                        objPQEntity.UTMA = HttpContext.Current.Request.Cookies["__utma"] != null ? HttpContext.Current.Request.Cookies["__utma"].Value : "";
                        objPQEntity.UTMZ = HttpContext.Current.Request.Cookies["_bwutmz"] != null ? HttpContext.Current.Request.Cookies["_bwutmz"].Value : "";
                        objPQEntity.DeviceId = HttpContext.Current.Request.Cookies["BWC"] != null ? HttpContext.Current.Request.Cookies["BWC"].Value : "";
                        objData.PQId = (uint)_objPQ.RegisterPriceQuote(objPQEntity);
                    }

                    ManufacturerCampaignEntity campaigns = _objManufacturerCampaign.GetCampaigns(modelId, cityId, ManufacturerCampaignPageId);
                    if (campaigns.LeadCampaign != null)
                    {
                        objData.LeadCampaign = new ManufactureCampaignLeadEntity()
                        {
                            Area = locationCookie.Area,
                            CampaignId = campaigns.LeadCampaign.CampaignId,
                            DealerId = campaigns.LeadCampaign.DealerId,
                            Organization = campaigns.LeadCampaign.Organization,
                            DealerRequired = campaigns.LeadCampaign.DealerRequired,
                            EmailRequired = campaigns.LeadCampaign.EmailRequired,
                            LeadsButtonTextDesktop = campaigns.LeadCampaign.LeadsButtonTextDesktop,
                            LeadsButtonTextMobile = campaigns.LeadCampaign.LeadsButtonTextMobile,
                            LeadSourceId = (int)LeadSource,
                            PqSourceId = (int)PQSource,
                            GACategory = "Price_in_City_Page",
                            GALabel = string.Format("{0}_{1}", objData.BikeName, cityMaskingName),
                            LeadsHtmlDesktop = campaigns.LeadCampaign.LeadsHtmlDesktop,
                            LeadsHtmlMobile = campaigns.LeadCampaign.LeadsHtmlMobile,
                            LeadsPropertyTextDesktop = campaigns.LeadCampaign.LeadsPropertyTextDesktop,
                            LeadsPropertyTextMobile = campaigns.LeadCampaign.LeadsPropertyTextMobile,
                            PriceBreakUpLinkDesktop = campaigns.LeadCampaign.PriceBreakUpLinkDesktop,
                            PriceBreakUpLinkMobile = campaigns.LeadCampaign.PriceBreakUpLinkMobile,
                            PriceBreakUpLinkTextDesktop = campaigns.LeadCampaign.PriceBreakUpLinkTextDesktop,
                            PriceBreakUpLinkTextMobile = campaigns.LeadCampaign.PriceBreakUpLinkTextMobile,
                            MakeName = objData.Make.MakeName,
                            MaskingNumber = campaigns.LeadCampaign.MaskingNumber,
                            PincodeRequired = campaigns.LeadCampaign.PincodeRequired,
                            PopupDescription = campaigns.LeadCampaign.PopupDescription,
                            PopupHeading = campaigns.LeadCampaign.PopupHeading,
                            PopupSuccessMessage = campaigns.LeadCampaign.PopupSuccessMessage,
                            ShowOnExshowroom = campaigns.LeadCampaign.ShowOnExshowroom,
                            PQId = (uint)objData.PQId,
                            VersionId = objData.VersionId,
                            CurrentPageUrl = CurrentPageUrl,
                            PlatformId = (ushort)Platform,
                            BikeName = objData.BikeName,
                            LoanAmount = Convert.ToUInt32((objData.FirstVersion.OnRoadPrice) * 0.8),
                            SendLeadSMSCustomer = campaigns.LeadCampaign.SendLeadSMSCustomer
                        };



                        objData.IsManufacturerLeadAdShown = true;
                        objData.LeadCampaign.PageUrl = string.Format("{0}/m/popup/leadcapture/?q={1}", BWConfiguration.Instance.BwHostUrl, Utils.Utils.EncryptTripleDES(string.Format("modelid={0}&cityid={1}&areaid={2}&bikename={3}&location={4}&city={5}&area={6}&ismanufacturer={7}&dealerid={8}&dealername={9}&dealerarea={10}&versionid={11}&leadsourceid={12}&pqsourceid={13}&mfgcampid={14}&pqid={15}&pageurl={16}&clientip={17}&dealerheading={18}&dealermessage={19}&dealerdescription={20}&pincoderequired={21}&emailrequired={22}&dealersrequired={23}&url={24}&sendLeadSMSCustomer={25}&organizationName={26}", 
                            objData.BikeModel.ModelId, objData.CityEntity.CityId, string.Empty, string.Format(objData.BikeName), string.Empty, string.Empty, string.Empty, 
                            objData.IsManufacturerLeadAdShown, objData.LeadCampaign.DealerId, String.Format(objData.LeadCampaign.LeadsPropertyTextMobile, 
                            objData.LeadCampaign.Organization), objData.LeadCampaign.Area, objData.VersionId, objData.LeadCampaign.LeadSourceId, objData.LeadCampaign.PqSourceId,
                            objData.LeadCampaign.CampaignId, objData.PQId, string.Empty, Bikewale.Common.CommonOpn.GetClientIP(), objData.LeadCampaign.PopupHeading, 
                            String.Format(objData.LeadCampaign.PopupSuccessMessage, objData.LeadCampaign.Organization), objData.LeadCampaign.PopupDescription, 
                            objData.LeadCampaign.PincodeRequired, objData.LeadCampaign.EmailRequired, objData.LeadCampaign.DealerRequired, 
                            string.Format("{0}/m/{1}-bikes/{2}/price-in-{3}/", BWConfiguration.Instance.BwHostUrlForJs, firstVersion.MakeMaskingName, modelMaskingName, cityMaskingName),
                            objData.LeadCampaign.SendLeadSMSCustomer, objData.LeadCampaign.Organization)));
                    }
                    if (campaigns.EMICampaign != null)
                    {
                        objData.EMICampaign = new ManufactureCampaignEMIEntity()
                        {
                            Area = locationCookie.Area,
                            CampaignId = campaigns.EMICampaign.CampaignId,
                            DealerId = campaigns.EMICampaign.DealerId,
                            Organization = campaigns.EMICampaign.Organization,
                            DealerRequired = campaigns.EMICampaign.DealerRequired,
                            EmailRequired = campaigns.EMICampaign.EmailRequired,
                            EMIButtonTextDesktop = campaigns.EMICampaign.EMIButtonTextDesktop,
                            EMIButtonTextMobile = campaigns.EMICampaign.EMIButtonTextMobile,
                            LeadSourceId = (int)LeadSource,
                            PqSourceId = (int)PQSource,
                            EMIPropertyTextDesktop = campaigns.EMICampaign.EMIPropertyTextDesktop,
                            EMIPropertyTextMobile = campaigns.EMICampaign.EMIPropertyTextMobile,
                            MakeName = objData.Make.MakeName,
                            MaskingNumber = campaigns.EMICampaign.MaskingNumber,
                            PincodeRequired = campaigns.EMICampaign.PincodeRequired,
                            PopupDescription = campaigns.EMICampaign.PopupDescription,
                            PopupHeading = campaigns.EMICampaign.PopupHeading,
                            PopupSuccessMessage = campaigns.EMICampaign.PopupSuccessMessage,
                            VersionId = objData.VersionId,
                            CurrentPageUrl = CurrentPageUrl,
                            PlatformId = (ushort)Platform,
                            LoanAmount = Convert.ToUInt32((objData.FirstVersion.OnRoadPrice) * 0.8),
                            SendLeadSMSCustomer = campaigns.EMICampaign.SendLeadSMSCustomer
                        };

                        objData.EMICampaign.PageUrl = string.Format("{0}/m/popup/leadcapture/?q={1}", BWConfiguration.Instance.BwHostUrl, Utils.Utils.EncryptTripleDES(string.Format("modelid={0}&cityid={1}&areaid={2}&bikename={3}&location={4}&city={5}&area={6}&ismanufacturer={7}&dealerid={8}&dealername={9}&dealerarea={10}&versionid={11}&leadsourceid={12}&pqsourceid={13}&mfgcampid={14}&pqid={15}&pageurl={16}&clientip={17}&dealerheading={18}&dealermessage={19}&dealerdescription={20}&pincoderequired={21}&emailrequired={22}&dealersrequired={23}&url={24}&sendLeadSMSCustomer={25}&organizationName={26}", 
                            objData.BikeModel.ModelId, objData.CityEntity.CityId, string.Empty, string.Format(objData.BikeName), string.Empty, string.Empty, string.Empty, 
                            objData.IsManufacturerLeadAdShown, objData.EMICampaign.DealerId, String.Format(objData.EMICampaign.EMIPropertyTextDesktop, 
                            objData.EMICampaign.Organization), objData.EMICampaign.Area, objData.VersionId, objData.EMICampaign.LeadSourceId, objData.EMICampaign.PqSourceId, 
                            objData.EMICampaign.CampaignId, objData.PQId, string.Empty, Bikewale.Common.CommonOpn.GetClientIP(), objData.EMICampaign.PopupHeading, 
                            String.Format(objData.EMICampaign.PopupSuccessMessage, objData.EMICampaign.Organization), objData.EMICampaign.PopupDescription, 
                            objData.EMICampaign.PincodeRequired, objData.EMICampaign.EmailRequired, objData.EMICampaign.DealerRequired, objData.PageMetaTags.AlternateUrl,
                            objData.EMICampaign.SendLeadSMSCustomer,objData.EMICampaign.Organization)));

                        objData.IsManufacturerEMIAdShown = true;
                    }



                    if (objData.IsManufacturerLeadAdShown)
                    {
                        _objManufacturerCampaign.SaveManufacturerIdInPricequotes(Convert.ToUInt32(objData.PQId), campaigns.LeadCampaign.DealerId);
                    }
                    else if (objData.IsManufacturerEMIAdShown)
                    {
                        _objManufacturerCampaign.SaveManufacturerIdInPricequotes(Convert.ToUInt32(objData.PQId), campaigns.EMICampaign.DealerId);
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("ModelPage.GetManufacturerCampaign({0},{1},{2})", modelId, cityId, ManufacturerCampaignPageId));
            }
        }

        /// <summary>
        /// Created By: Snehal Dange on 20th Dec 2017
        /// Summary : Bind more about scooter widget
        /// </summary>
        /// <param name="objData"></param>
        private void BindMoreAboutScootersWidget(PriceInCityPageVM objData)
        {
            try
            {
                MoreAboutScootersWidget obj = new MoreAboutScootersWidget(_modelCache, _objCityCache, _version, _bikeInfo, Entities.GenericBikes.BikeInfoTabType.PriceInCity);
                obj.modelId = modelId;
                objData.objMoreAboutScooter = obj.GetData();
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Bikewale.Models.PriceInCityPAge.BindMoreAboutScootersWidget : ModelId {0}", modelId));
            }



        }
    }
}
