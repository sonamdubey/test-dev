using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Models.PriceInCity;
using Bikewale.Models.ServiceCenters;
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
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _modelMaskingCache = null;
        private readonly IPriceQuote _objPQ = null;
        private readonly IPriceQuoteCache _objPQCache = null;
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IServiceCenter _objServiceCenter = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _versionCache = null;
        private readonly PQSourceEnum pqSource;
        private readonly IBikeInfo _bikeInfo = null;
        private readonly ICityCacheRepository _cityCache = null;
        private readonly IBikeModelsCacheRepository<int> _modelCache = null;

        private uint cityId, modelId, versionCount, colorCount, dealerCount;
        private string modelMaskingName, cityMaskingName, pageDescription;

        private BikeQuotationEntity firstVersion;
        private bool isNew, isAreaSelected, hasAreaAvailable;
        public StatusCodes Status { get; private set; }
        public String RedirectUrl { get; private set; }
        public uint NearestCityCount { get; set; }
        public uint BikeInfoTabCount { get; set; }
        public uint TopCount { get; set; }
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
        public PriceInCityPage(ICityMaskingCacheRepository cityMaskingCache, IBikeMaskingCacheRepository<BikeModelEntity, int> modelMaskingCache, IPriceQuote objPQ, IPriceQuoteCache objPQCache, IDealerCacheRepository objDealerCache, IServiceCenter objServiceCenter, IBikeVersionCacheRepository<BikeVersionEntity, uint> versionCache, IBikeInfo bikeInfo, ICityCacheRepository cityCache, IBikeModelsCacheRepository<int> modelCache, PQSourceEnum pqSource, string modelMaskingName, string cityMaskingName)
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
            this.pqSource = pqSource;
            this.modelMaskingName = modelMaskingName;
            this.cityMaskingName = cityMaskingName;
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
                            isAreaSelected = true;
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
        /// Created by  :   Sumit Kate on 28 Mar 2017
        /// Description :   returns Price in city page view model
        /// </summary>
        /// <returns></returns>
        //public PriceInCityPageVM GetData()
        //{
        //    PriceInCityPageVM objVM = null;
        //    try
        //    {
        //        if (Status == StatusCodes.ContentFound)
        //        {
        //            objVM = new PriceInCityPageVM();
        //            //Get Bike version Prices
        //            objVM.BikeVersionPrices = _objPQ.GetVersionPricesByModelId(modelId, cityId, out hasAreaAvailable);
        //            if (objVM.BikeVersionPrices != null && objVM.BikeVersionPrices.Count() > 0)
        //            {
        //                firstVersion = objVM.BikeVersionPrices.FirstOrDefault();
        //                objVM.IsNew = isNew = firstVersion.IsModelNew;
        //                if (isNew)
        //                {
        //                    objVM.BikeVersionPrices = objVM.BikeVersionPrices.Where(m => m.IsVersionNew);
        //                }
        //                versionCount = (uint)objVM.BikeVersionPrices.Count();

        //                BindBikeBasicDetails(objVM);
        //                BindPriceInNearestCities(objVM);
        //                BindDealersWidget(objVM);
        //                BindServiceCenters(objVM);
        //                BindSimilarBikes(objVM);
        //                BindBikeInfoRank(objVM);

        //                var objModelColours = _modelCache.GetModelColor(Convert.ToInt16(modelId));
        //                colorCount = (uint)(objModelColours != null ? objModelColours.Count() : 0);

        //                objVM.PageDescription = PageDescription();
        //                objVM.IsAreaSelected = isAreaSelected;
        //                objVM.IsAreaAvailable = hasAreaAvailable;
        //                objVM.Page_H1 = String.Format("{0} price in {1}", objVM.BikeName, objVM.CityEntity.CityName);

        //                var locationCookie = GlobalCityArea.GetGlobalCityArea();

        //                objVM.CookieCityArea = String.Format("{0} {1}", locationCookie.City, locationCookie.Area);
        //                BuildPageMetas(objVM.PageMetaTags);
        //            }
        //            else
        //            {
        //                Status = StatusCodes.ContentNotFound;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, String.Format("FetchVersionPrices({0},{1})", modelMaskingName, cityMaskingName));
        //    }
        //    return objVM;
        //}

        public PriceInCityPageVM GetData()
        {
            PriceInCityPageVM objVM = null;
            try
            {
                if (Status == StatusCodes.ContentFound)
                {
                    objVM = new PriceInCityPageVM();
                    //Get Bike version Prices
                    objVM.BikeVersionPrices = _objPQ.GetVersionPricesByModelId(modelId, cityId, out hasAreaAvailable);
                    if (objVM.BikeVersionPrices != null && objVM.BikeVersionPrices.Count() > 0)
                    {
                        objVM.JSONBikeVersions = Bikewale.Utility.EncodingDecodingHelper.EncodeTo64((new System.Web.Script.Serialization.JavaScriptSerializer()).Serialize(objVM.BikeVersionPrices));

                        firstVersion = objVM.BikeVersionPrices.FirstOrDefault();
                        objVM.IsNew = isNew = firstVersion.IsModelNew;
                        //if (isNew)
                        //{
                        //    objVM.BikeVersionPrices = objVM.BikeVersionPrices.Where(m => m.IsVersionNew);
                        //}
                        versionCount = (uint)objVM.BikeVersionPrices.Count();

                        BindBikeBasicDetails(objVM);
                        BindPriceInNearestCities(objVM);
                        BindPriceinTopCitis(objVM);
                        BindDealersWidget(objVM);
                        BindServiceCenters(objVM);
                        BindSimilarBikes(objVM);
                        BindBikeInfoRank(objVM);

                        var objModelColours = _modelCache.GetModelColor(Convert.ToInt16(modelId));
                        colorCount = (uint)(objModelColours != null ? objModelColours.Count() : 0);

                        objVM.PageDescription = PageDescription();
                        objVM.IsAreaSelected = isAreaSelected;
                        objVM.IsAreaAvailable = hasAreaAvailable;
                        objVM.Page_H1 = String.Format("{0} price in {1}", objVM.BikeName, objVM.CityEntity.CityName);

                        var locationCookie = GlobalCityArea.GetGlobalCityArea();

                        objVM.CookieCityArea = String.Format("{0} {1}", locationCookie.City, locationCookie.Area);
                        BuildPageMetas(objVM.PageMetaTags);
                    }
                    else
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, String.Format("FetchVersionPrices({0},{1})", modelMaskingName, cityMaskingName));
            }
            return objVM;
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
        /// </summary>
        /// <param name="objVM"></param>
        private void BindSimilarBikes(PriceInCityPageVM objVM)
        {
            try
            {
                var similarBikes = new SimilarBikesWidget(_versionCache, firstVersion.VersionId, pqSource, false, true);
                similarBikes.CityId = cityId;
                similarBikes.TopCount = 9;
                objVM.AlternateBikes = similarBikes.GetData();
                objVM.AlternateBikes.Make = objVM.Make;
                objVM.AlternateBikes.Model = objVM.BikeModel;
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("BindSimilarBikes({0},{1})", modelMaskingName, cityMaskingName));
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
                objVM.ServiceCenters = (new ServiceCentersCard(_objServiceCenter, 3, Convert.ToUInt32(objVM.Make.MakeId), objVM.CityEntity.CityId)).GetData();
                objVM.ServiceCenterCount = objVM.ServiceCenters.Count;
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
                objVM.Dealers = objDealer.GetData();
                dealerCount = (uint)(objVM.HasDealers ? objVM.Dealers.Dealers.Count() : 0);
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
                objVM.PriceInNearestCities = (new ModelPriceInNearestCities(_objPQCache, modelId, cityId, (ushort)NearestCityCount)).GetData();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("BindPriceInNearestCities({0},{1})", modelMaskingName, cityMaskingName));
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 11 Apr 2017
        /// summary    : Bind price in top cities widget
        /// </summary>
        private void BindPriceinTopCitis(PriceInCityPageVM objVM)
        {
            try
            {
                PriceInTopCities objPricesTop = new PriceInTopCities(_objPQCache, modelId, 6);
                objVM.PriceTopCities = objPricesTop.GetData();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("BindPriceinTopCitis({0},{1})", modelMaskingName, cityMaskingName));
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
                    string newBikeDescription = string.Format("{0} {1} on-road price in {2} - Rs. {3} onwards. It is available in {4} version{5}{6}", firstVersion.MakeName, firstVersion.ModelName, firstVersion.City, CommonOpn.FormatPrice(firstVersion.OnRoadPrice.ToString()), versionCount, multiVersion, multiColour);

                    if (dealerCount > 0)
                        newBikeDescription = string.Format("{0} {1} is sold by {2} dealership{3} in {4}.", newBikeDescription, firstVersion.ModelName, dealerCount, multiDealer, firstVersion.City);

                    newBikeDescription = string.Format("{0} All the colour options and versions of {1} might not be available at all the dealerships in {2}. Click on a {1} version name to know on-road price in {2}.", newBikeDescription, firstVersion.ModelName, firstVersion.City);

                    string discontinuedDescription = string.Format("The last known ex-showroom price of {0} {1} in {2} was Rs. {3} onwards. This bike has now been discontinued. It was available in {4} version{5}{6} Click on a {1} version name to know the last known ex-showroom price in {2}.", firstVersion.MakeName, firstVersion.ModelName, firstVersion.City, CommonOpn.FormatPrice(firstVersion.OnRoadPrice.ToString()), versionCount, multiVersion, multiColour);

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
        /// </summary>
        /// <param name="metas"></param>
        private void BuildPageMetas(PageMetaTags metas)
        {
            try
            {
                string bikeName = String.Format("{0} {1}", firstVersion.MakeName, firstVersion.ModelName);
                metas.AlternateUrl = string.Format("{0}/m/{1}-bikes/{2}/price-in-{3}/", BWConfiguration.Instance.BwHostUrlForJs, firstVersion.MakeMaskingName, modelMaskingName, cityMaskingName);
                metas.CanonicalUrl = string.Format("{0}/{1}-bikes/{2}/price-in-{3}/", BWConfiguration.Instance.BwHostUrlForJs, firstVersion.MakeMaskingName, modelMaskingName, cityMaskingName);
                metas.Title = string.Format("{0} price in {1} - Check On Road Price & Dealer Info. - BikeWale", bikeName, firstVersion.City);

                if (firstVersion != null && !isNew)
                    metas.Description = string.Format("{0} price in {1} - Rs. {2} (On road price). Get its detailed on road price in {1}. Check your nearest {0} Dealer in {1}", bikeName, firstVersion.City, firstVersion.OnRoadPrice);
                else if (firstVersion != null)
                    metas.Description = string.Format("{0} price in {1} - Rs. {2} (Ex-Showroom). Get prices for all the versions of and check out the nearest {0} Dealer in {1}", bikeName, firstVersion.City, firstVersion.ExShowroomPrice);
                metas.Keywords = string.Format("{0} price in {1}, {0} on-road price, {0} bike, buy {0} bike in {1}, new {2} price", bikeName, firstVersion.City, firstVersion.ModelName);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, String.Format("BuildPageMetas({0},{1})", modelMaskingName, cityMaskingName));
            }
        }
    }
}