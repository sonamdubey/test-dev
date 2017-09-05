﻿using Bikewale.Common;
using Bikewale.DTO.PriceQuote;
using Bikewale.Entities;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.manufacturecampaign;
using Bikewale.Entities.PriceQuote;
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
using Bikewale.Utility;
using System;
using System.Linq;
using System.Web;
namespace Bikewale.Models
{
    /// <summary>
    /// Created by  :   Sumit Kate on 28 Mar 2017
    /// Description :   PriceInCityPage model
    /// </summary>
    public class PriceInCityPage
    {
        private readonly ICityMaskingCacheRepository _cityMaskingCache = null;
        private readonly IBikeMaskingCacheRepository<Entities.BikeData.BikeModelEntity, int> _modelMaskingCache = null;
        private readonly IPriceQuote _objPQ = null;
        private readonly IPriceQuoteCache _objPQCache = null;
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IServiceCenter _objServiceCenter = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _versionCache = null;
        private readonly PQSourceEnum pqSource;
        private readonly IBikeInfo _bikeInfo = null;
        private readonly ICityCacheRepository _cityCache = null;
        private readonly IBikeModelsCacheRepository<int> _modelCache = null;
        private readonly IDealerPriceQuoteDetail _objDealerDetails = null;
        private readonly IDealerPriceQuote _objDealerPQ = null;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IAreaCacheRepository _objAreaCache = null;
        private readonly IManufacturerCampaign _objManufacturerCampaign = null;
        private uint cityId, modelId, versionCount, colorCount, dealerCount, areaId;
        private string modelMaskingName, cityMaskingName, pageDescription, area, city;
        private BikeQuotationEntity firstVersion;
        private uint primaryDealerId;
        private bool isNew, isAreaSelected, hasAreaAvailable;
        public StatusCodes Status { get; private set; }
        public String RedirectUrl { get; private set; }
        public uint NearestCityCount { get; set; }
        public uint BikeInfoTabCount { get; set; }
        public uint TopCount { get; set; }
        public PQSourceEnum PQSource { get; set; }
        public PQSources Platform { get; set; }
        public LeadSourceEnum LeadSource { get; set; }
        public ManufacturerCampaignServingPages ManufacturerCampaignPageId { get; set; }

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
        public PriceInCityPage(ICityMaskingCacheRepository cityMaskingCache, IBikeMaskingCacheRepository<Entities.BikeData.BikeModelEntity, int> modelMaskingCache, IPriceQuote objPQ, IPriceQuoteCache objPQCache, IDealerCacheRepository objDealerCache, IServiceCenter objServiceCenter, IBikeVersionCacheRepository<BikeVersionEntity, uint> versionCache, IBikeInfo bikeInfo, ICityCacheRepository cityCache, IBikeModelsCacheRepository<int> modelCache, IDealerPriceQuoteDetail objDealerDetails, IDealerPriceQuote objDealerPQ, ICityCacheRepository objCityCache, IAreaCacheRepository objAreaCache, IManufacturerCampaign objManufacturerCampaign, PQSourceEnum pqSource, string modelMaskingName, string cityMaskingName)
        {
            _cityMaskingCache = cityMaskingCache;
            _modelMaskingCache = modelMaskingCache;
            _objPQ = objPQ;
            _objPQCache = objPQCache;
            _objDealerCache = objDealerCache;
            _objServiceCenter = objServiceCenter;
            _versionCache = versionCache;
            _bikeInfo = bikeInfo;
            _cityCache = cityCache;
            _modelCache = modelCache;
            _objDealerDetails = objDealerDetails;
            _objDealerPQ = objDealerPQ;
            _objCityCache = objCityCache;
            _objAreaCache = objAreaCache;
            this.pqSource = pqSource;
            this.modelMaskingName = modelMaskingName;
            this.cityMaskingName = cityMaskingName;
            _objManufacturerCampaign = objManufacturerCampaign;
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
            String rawUrl = HttpContext.Current.Request.RawUrl;
            try
            {

                if (!(String.IsNullOrEmpty(modelMaskingName) || String.IsNullOrEmpty(cityMaskingName)))
                {
                    objCityResponse = _cityMaskingCache.GetCityMaskingResponse(cityMaskingName);
                    objModelResponse = _modelMaskingCache.GetModelMaskingResponse(modelMaskingName);
                }

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, String.Format("ProcessQueryString({0},{1})", modelMaskingName, cityMaskingName));
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
                        GlobalCityAreaEntity currentCityArea = GlobalCityArea.GetGlobalCityArea();
                        if (currentCityArea.CityId > 0 && currentCityArea.CityId == cityId && currentCityArea.AreaId > 0)
                        {
                            areaId = currentCityArea.AreaId;
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
                    else if (objModelResponse.StatusCode == 301)
                    {
                        //redirect permanent to new page                         
                        rawUrl = rawUrl.Replace(modelMaskingName, objModelResponse.MaskingName);
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
        /// Modified by :   Sumit Kate on 05 Jan 2016
        /// Description :   Replaced the Convert.ToXXX with XXX.TryParse method
        /// Modified By : Sushil Kumar on 26th August 2016
        /// Description : Replaced location name from location cookie to selected location objects for city and area respectively.
        /// </summary>
        private void CheckCityCookie(PriceInCityPageVM objVM)
        {
            try
            {
                if (modelId > 0)
                {
                    if (cityId > 0)
                    {
                        objVM.Cities = _objCityCache.GetPriceQuoteCities(modelId);
                        if (objVM.Cities != null)
                        {
                            var selectedCity = objVM.Cities.FirstOrDefault(m => m.CityId == cityId);
                            objVM.CookieCityEntity = selectedCity;
                            if (selectedCity != null && selectedCity.HasAreas && areaId > 0)
                            {
                                var areas = _objAreaCache.GetAreaList(modelId, cityId);
                                city = selectedCity.CityName;
                                if (areas != null && areas.Count() > 0)
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
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, String.Format("CheckCityCookie({0},{1})", modelMaskingName, cityMaskingName));
            }
        }

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
                    if (objVM.BikeVersionPrices != null && objVM.BikeVersionPrices.Count() > 0)
                    {
                        firstVersion = objVM.BikeVersionPrices.OrderByDescending(m => m.IsVersionNew).OrderBy(v => v.ExShowroomPrice).First();
                        objVM.IsNew = isNew = firstVersion.IsModelNew;
                        if (objVM.IsNew)
                        {
                            objVM.BikeVersionPrices = objVM.BikeVersionPrices.Where(x => x.IsVersionNew);
                        }
                        versionCount = (uint)objVM.BikeVersionPrices.Count();
                        objVM.VersionSpecs = _versionCache.GetVersionMinSpecs(modelId, true);
                        if (objVM.VersionSpecs != null)
                        {
                            var objMin = objVM.VersionSpecs.FirstOrDefault(x => x.VersionId == firstVersion.VersionId);
                            if (objMin != null)
                            {
                                objVM.MinSpecsHtml = FormatVarientMinSpec(objMin);

                                // Set body style
                                objVM.BodyStyle = objMin.BodyStyle;
                            }
                            else
                            {
                                var firstVersion = objVM.VersionSpecs.FirstOrDefault();
                                if (firstVersion != null)
                                {
                                    objVM.BodyStyle = objVM.VersionSpecs.FirstOrDefault().BodyStyle;

                                }
                            }

                            foreach (var version in objVM.VersionSpecs)
                            {
                                var versionPrice = objVM.BikeVersionPrices.Where(m => m.VersionId.Equals(version.VersionId)).FirstOrDefault();
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
                        BindDealersWidget(objVM);

                        var objModelColours = _modelCache.GetModelColor(Convert.ToInt16(modelId));
                        colorCount = (uint)(objModelColours != null ? objModelColours.Count() : 0);

                        objVM.PageDescription = PageDescription();
                        objVM.IsAreaSelected = isAreaSelected;
                        objVM.IsAreaAvailable = hasAreaAvailable;
                        objVM.Page_H1 = String.Format("{0} price in {1}", objVM.BikeName, objVM.CityEntity.CityName);

                        var locationCookie = GlobalCityArea.GetGlobalCityArea();

                        objVM.CookieCityArea = String.Format("{0} {1}", locationCookie.City, locationCookie.Area);
                        BuildPageMetas(objVM);

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
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, String.Format("FetchVersionPrices({0},{1})", modelMaskingName, cityMaskingName));
            }
            return objVM;
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
                uint[] topCityId = new uint[] { 1, 2, 10, 12, 105, 176, 198, 220 };
                objVM.PriceInTopCities = new PriceInTopCities(_objPQCache, modelId, 8).GetData();
                if (objVM.HasPriceInTopCities)
                {
                    objVM.PriceInTopCities.PriceQuoteList = objVM.PriceInTopCities.PriceQuoteList.Where(m => !m.CityMaskingName.Equals(cityMaskingName));
                    objVM.PriceInTopCities.PriceQuoteList = objVM.PriceInTopCities.PriceQuoteList.Where(m => topCityId.Contains(m.CityId));
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, String.Format("BindPriceInTopCities({0},{1})", modelMaskingName, cityMaskingName));
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
                objVM.Make = new BikeMakeEntityBase() { MakeName = firstVersion.MakeName, MaskingName = firstVersion.MakeMaskingName, MakeId = (int)firstVersion.MakeId };
                objVM.BikeModel = new BikeModelEntityBase() { ModelId = (int)modelId, ModelName = firstVersion.ModelName, MaskingName = firstVersion.ModelMaskingName };
                objVM.ModelImage = objVM.PageMetaTags.OGImage = Image.GetPathToShowImages(firstVersion.OriginalImage, firstVersion.HostUrl, ImageSize._310x174, QualityFactor._75);
                objVM.CityEntity = new CityEntityBase() { CityId = cityId, CityMaskingName = cityMaskingName, CityName = firstVersion.City };
                objVM.VersionId = firstVersion.VersionId;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("BindBikeBasicDetails({0},{1})", modelMaskingName, cityMaskingName));
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

                objVM.BikeInfo = (new BikeInfoWidget(_bikeInfo, _cityCache, modelId, cityId, BikeInfoTabCount, Entities.GenericBikes.BikeInfoTabType.PriceInCity)).GetData();
                objVM.BikeRank = (new BikeModelRank(_modelCache, modelId)).GetData();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("BindBikeInfoRank({0},{1})", modelMaskingName, cityMaskingName));
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 Mar 2017
        /// Description :   Bind Similar Bikes
        /// Modified by: Vivek Singh Tomar on 23 Aug 2017
        /// Summary: Added page enum to similar bike widget
        /// </summary>
        /// <param name="objVM"></param>
        private void BindSimilarBikes(PriceInCityPageVM objVM)
        {
            try
            {
                var similarBikes = new SimilarBikesWidget(_versionCache, firstVersion.VersionId, pqSource, false, true);
                similarBikes.CityId = cityId;
                similarBikes.TopCount = 9;
                similarBikes.IsNew = objVM.IsNew;
                similarBikes.IsDiscontinued = objVM.IsDiscontinued;
                var similarBikesVM = similarBikes.GetData();
                if (similarBikesVM != null)
                {
                    similarBikesVM.Make = objVM.Make;
                    similarBikesVM.Model = objVM.BikeModel;
                    similarBikesVM.VersionId = objVM.FirstVersion.VersionId;
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
                ErrorClass objErr = new ErrorClass(ex, String.Format("BindSimilarBikes({0},{1})", modelMaskingName, cityMaskingName));
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
                    var modelPopularBikesByBodyStyle = new PopularBikesByBodyStyle(_modelCache);
                    modelPopularBikesByBodyStyle.CityId = cityId;
                    modelPopularBikesByBodyStyle.ModelId = modelId;
                    modelPopularBikesByBodyStyle.TopCount = 9;

                    objData.PopularBodyStyle = modelPopularBikesByBodyStyle.GetData();
                    objData.PopularBodyStyle.PQSourceId = PQSource;
                    objData.PopularBodyStyle.ShowCheckOnRoadCTA = true;
                    objData.BodyStyle = objData.PopularBodyStyle.BodyStyle;
                    objData.BodyStyleText = objData.BodyStyle == EnumBikeBodyStyles.Scooter ? "Scooters" : "Bikes";
                }
            }
            catch (Exception ex)
            {
                ErrorClass ec = new ErrorClass(ex, String.Format("Bikewale.Models.PriceInCity.BindPopularBodyStyle({0},{1})", modelId, cityId));
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
                ErrorClass objErr = new ErrorClass(ex, String.Format("BindServiceCenters({0},{1})", modelMaskingName, cityMaskingName));
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
                ErrorClass objErr = new ErrorClass(ex, String.Format("BindDealersWidget({0},{1})", modelMaskingName, cityMaskingName));
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
                var nearestCityModel = new ModelPriceInNearestCities(_objPQCache, modelId, cityId, (ushort)NearestCityCount);
                var cityPriceList = nearestCityModel.GetData();
                if (cityPriceList != null && cityPriceList.Count() > 0)
                {
                    objVM.NearestPriceCities = new PriceInTopCitiesWidgetVM();
                    objVM.NearestPriceCities.PriceQuoteList = cityPriceList;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("BindPriceInNearestCities({0},{1})", modelMaskingName, cityMaskingName));
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 12 Apr 2017
        /// Summary    : Format min specs
        /// </summary>
        private string FormatVarientMinSpec(BikeVersionMinSpecs objVersion)
        {
            string minSpecsStr = string.Empty;

            try
            {
                minSpecsStr = string.Format("{0}<li>{1} Wheels</li>", minSpecsStr, objVersion.AlloyWheels ? "Alloy" : "Spoke");
                minSpecsStr = string.Format("{0}<li>{1} Start</li>", minSpecsStr, objVersion.ElectricStart ? "Electric" : "Kick");

                if (objVersion.AntilockBrakingSystem)
                {
                    minSpecsStr = string.Format("{0}<li>ABS</li>", minSpecsStr);
                }

                if (!String.IsNullOrEmpty(objVersion.BrakeType))
                {
                    minSpecsStr = string.Format("{0}<li>{1} Brake</li>", minSpecsStr, objVersion.BrakeType);
                }


                if (!string.IsNullOrEmpty(minSpecsStr))
                {
                    minSpecsStr = string.Format("<ul id='version-specs-list'>{0}</ul>", minSpecsStr);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.PriceInCityPAge.FormatVarientMinSpec(): versionId {0}", objVersion.VersionId));
            }

            return minSpecsStr;

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
                    string newBikeDescription = string.Format("{0} {1} on-road price in {2} -   &#x20B9; {3} onwards. It is available in {4} version{5}{6}", firstVersion.MakeName, firstVersion.ModelName, firstVersion.City, CommonOpn.FormatPrice(firstVersion.OnRoadPrice.ToString()), versionCount, multiVersion, multiColour);

                    if (dealerCount > 0)
                        newBikeDescription = string.Format("{0} {1} is sold by {2} dealership{3} in {4}.", newBikeDescription, firstVersion.ModelName, dealerCount, multiDealer, firstVersion.City);

                    newBikeDescription = string.Format("{0} All the colour options and versions of {1} might not be available at all the dealerships in {2}.", newBikeDescription, firstVersion.ModelName, firstVersion.City);

                    string discontinuedDescription = string.Format("The last known ex-showroom price of {0} {1} in {2} was   &#x20B9; {3} onwards. This bike has now been discontinued. It was available in {4} version{5}{6} Click on a {1} version name to know the last known ex-showroom price in {2}.", firstVersion.MakeName, firstVersion.ModelName, firstVersion.City, CommonOpn.FormatPrice(firstVersion.OnRoadPrice.ToString()), versionCount, multiVersion, multiColour);

                    if (isNew)
                        pageDescription = newBikeDescription;
                    else
                        pageDescription = discontinuedDescription;
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("PageDescription({0},{1})", modelMaskingName, cityMaskingName));
            }
            return pageDescription;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 28 mar 2017
        /// Description :   Binds the page metas
        /// Modified By :- Subodh Jain 2 june 2017
        /// Added target city and model
        /// </summary>
        /// <param name="metas"></param>
        private void BuildPageMetas(PriceInCityPageVM objVM)
        {
            try
            {
                string bikeName = String.Format("{0} {1}", firstVersion.MakeName, firstVersion.ModelName);
                objVM.PageMetaTags.AlternateUrl = string.Format("{0}/m/{1}-bikes/{2}/price-in-{3}/", BWConfiguration.Instance.BwHostUrlForJs, firstVersion.MakeMaskingName, modelMaskingName, cityMaskingName);
                objVM.PageMetaTags.CanonicalUrl = string.Format("{0}/{1}-bikes/{2}/price-in-{3}/", BWConfiguration.Instance.BwHostUrlForJs, firstVersion.MakeMaskingName, modelMaskingName, cityMaskingName);
                objVM.ReturnUrl = string.Format("/m/{1}-bikes/{2}/price-in-{3}/", BWConfiguration.Instance.BwHostUrlForJs, firstVersion.MakeMaskingName, modelMaskingName, cityMaskingName);
                objVM.PageMetaTags.AmpUrl = string.Format("{0}/m/{1}-bikes/{2}/price-in-{3}/amp/", BWConfiguration.Instance.BwHostUrlForJs, firstVersion.MakeMaskingName, modelMaskingName, cityMaskingName);
                objVM.PageMetaTags.Title = string.Format("{0} price in {1} - Check GST On Road Price &amp; Dealer Info - BikeWale", bikeName, firstVersion.City);

                if (firstVersion != null && !isNew)
                    objVM.PageMetaTags.Description = string.Format("{0} price in {1} - Rs. {2} (Ex-Showroom price). Get its detailed on road price in {1}. Check your nearest {0} Dealer in {1}", bikeName, firstVersion.City, CommonOpn.FormatPrice(firstVersion.ExShowroomPrice.ToString()));
                else if (firstVersion != null)
                    objVM.PageMetaTags.Description = string.Format("{0} price in {1} - Rs. {2} (Ex-Showroom price). Get GST prices for all the versions of and check out the nearest {0} Dealer in {1}", bikeName, firstVersion.City, CommonOpn.FormatPrice(firstVersion.ExShowroomPrice.ToString()));
                objVM.PageMetaTags.Keywords = string.Format("{0} price in {1}, {0} on-road price, {0} bike, buy {0} bike in {1}, new {2} price", bikeName, firstVersion.City, firstVersion.ModelName);

                objVM.AdTags.TargetedCity = firstVersion.City;
                objVM.AdTags.TargetedModel = firstVersion.ModelName;

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("BuildPageMetas({0},{1})", modelMaskingName, cityMaskingName));
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
                ErrorClass objErr = new ErrorClass(ex, String.Format("GetDealerPriceQuote({0},{1})", modelMaskingName, cityMaskingName));
            }
            finally
            {
                if (objPQOutput != null)
                {
                    objVM.PQId = objPQOutput.PQId;
                    var bpqOutput = _objPQ.GetPriceQuoteById(objPQOutput.PQId, LeadSource);
                    if (objPQOutput.IsDealerAvailable)
                    {
                        try
                        {
                            primaryDealerId = objPQOutput.DealerId;
                            objVM.DetailedDealer = _objDealerDetails.GetDealerQuotationV2(cityId, objPQOutput.VersionId, objPQOutput.DealerId, areaId);
                            objVM.MPQString = EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(cityId.ToString(), objPQOutput.PQId.ToString(), areaId.ToString(), objPQOutput.VersionId.ToString(), objPQOutput.DealerId.ToString()));
                        }
                        catch (Exception ex)
                        {
                            ErrorClass objErr = new ErrorClass(ex, String.Format("GetDealerQuotationV2({0},{1})", modelMaskingName, cityMaskingName));
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
        /// </summary>
        private void GetManufacturerCampaign(PriceInCityPageVM objData)
        {
            try
            {
                if (_objManufacturerCampaign != null && !(objData.HasCampaignDealer))
                {
                    ManufacturerCampaignEntity campaigns = _objManufacturerCampaign.GetCampaigns(modelId, cityId, ManufacturerCampaignPageId);
                    if (campaigns.LeadCampaign != null)
                    {
                        objData.LeadCampaign = new ManufactureCampaignLeadEntity()
                        {
                            Area = GlobalCityArea.GetGlobalCityArea().Area,
                            CampaignId = campaigns.LeadCampaign.CampaignId,
                            DealerId = campaigns.LeadCampaign.DealerId,
                            Organization = campaigns.LeadCampaign.Organization,
                            DealerRequired = campaigns.LeadCampaign.DealerRequired,
                            EmailRequired = campaigns.LeadCampaign.EmailRequired,
                            LeadsButtonTextDesktop = campaigns.LeadCampaign.LeadsButtonTextDesktop,
                            LeadsButtonTextMobile = campaigns.LeadCampaign.LeadsButtonTextMobile,
                            LeadSourceId = (int)LeadSource,
                            PqSourceId = (int)PQSource,
                            LeadsHtmlDesktop = campaigns.LeadCampaign.LeadsHtmlDesktop,
                            LeadsHtmlMobile = campaigns.LeadCampaign.LeadsHtmlMobile,
                            LeadsPropertyTextDesktop = campaigns.LeadCampaign.LeadsPropertyTextDesktop,
                            LeadsPropertyTextMobile = campaigns.LeadCampaign.LeadsPropertyTextMobile,
                            MakeName = objData.Make.MakeName,
                            MaskingNumber = campaigns.LeadCampaign.MaskingNumber,
                            PincodeRequired = campaigns.LeadCampaign.PincodeRequired,
                            PopupDescription = campaigns.LeadCampaign.PopupDescription,
                            PopupHeading = campaigns.LeadCampaign.PopupHeading,
                            PopupSuccessMessage = campaigns.LeadCampaign.PopupSuccessMessage,
                            ShowOnExshowroom = campaigns.LeadCampaign.ShowOnExshowroom
                        };
                        objData.IsManufacturerLeadAdShown = true;
                    }
                    if (campaigns.EMICampaign != null)
                    {
                        objData.EMICampaign = new ManufactureCampaignEMIEntity()
                        {
                            Area = GlobalCityArea.GetGlobalCityArea().Area,
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
                            PopupSuccessMessage = campaigns.EMICampaign.PopupSuccessMessage
                        };
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("ModelPage.GetManufacturerCampaign({0},{1},{2})", modelId, cityId, ManufacturerCampaignPageId));
            }
        }
    }
}