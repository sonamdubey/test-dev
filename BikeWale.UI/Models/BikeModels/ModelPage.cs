using Bikewale.BindViewModels.Controls;
using Bikewale.BindViewModels.Webforms;
using Bikewale.common;
using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.PriceInCity;
using Bikewale.Models.ServiceCenters;
using Bikewale.Models.Used;
using Bikewale.Utility;
using Bikewale.Utility.GenericBikes;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;

namespace Bikewale.Models.BikeModels
{
    /// <summary>
    /// Modified By : Sangram Nandkhile on 07 Dec 2016.
    /// Description : Removed unncessary functions,
    /// </summary>
    public class ModelPage
    {
        #region Global Variables

        private readonly IDealerPriceQuote _objDealerPQ = null;
        private readonly IAreaCacheRepository _objAreaCache = null;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IPriceQuote _objPQ = null;
        private readonly IBikeModels<BikeModelEntity, int> _objModel = null;
        private readonly IDealerPriceQuoteDetail _objDealerDetails = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objVersionCache = null;
        private readonly IBikeModelsCacheRepository<int> _objBestBikes = null;
        private readonly ICMSCacheContent _objArticles = null;
        private readonly IVideos _objVideos = null;
        private readonly IUsedBikeDetailsCacheRepository _objUsedBikescache = null;
        private readonly IServiceCenter _objServiceCenter;
        private readonly IPriceQuoteCache _objPQCache;
        private readonly IBikeCompareCacheRepository _objCompare;
        private readonly IUserReviewsCache _userReviewCache;
        private readonly IUsedBikesCache _usedBikesCache;

        private ModelPageVM objData = null;
        private uint _modelId, _cityId, _areaId;
        private PQOnRoadPrice pqOnRoad;
        private uint totalUsedBikes = 0;
        private int colorCount = 0;
        private StringBuilder colorStr = new StringBuilder();



        public string RedirectUrl { get; set; }
        public StatusCodes Status { get; set; }
        public uint OtherDealersTopCount { get; set; }

        public ModelPage(string makeMasking, string modelMasking, IBikeModels<BikeModelEntity, int> objModel, IDealerPriceQuote objDealerPQ, IAreaCacheRepository objAreaCache, ICityCacheRepository objCityCache, IPriceQuote objPQ, IDealerCacheRepository objDealerCache, IDealerPriceQuoteDetail objDealerDetails, IBikeVersionCacheRepository<BikeVersionEntity, uint> objVersionCache, ICMSCacheContent objArticles, IVideos objVideos, IUsedBikeDetailsCacheRepository objUsedBikescache, IServiceCenter objServiceCenter, IPriceQuoteCache objPQCache, IBikeCompareCacheRepository objCompare, IUserReviewsCache userReviewCache, IUsedBikesCache usedBikesCache, IBikeModelsCacheRepository<int> objBestBikes)
        {
            _objModel = objModel;
            _objDealerPQ = objDealerPQ;
            _objAreaCache = objAreaCache;
            _objCityCache = objCityCache;
            _objPQ = objPQ;
            _objDealerCache = objDealerCache;
            _objDealerDetails = objDealerDetails;
            _objVersionCache = objVersionCache;
            _objBestBikes = objBestBikes;

            _objArticles = objArticles;
            _objVideos = objVideos;
            _objUsedBikescache = objUsedBikescache;
            _objServiceCenter = objServiceCenter;
            _objPQCache = objPQCache;
            _objCompare = objCompare;
            _userReviewCache = userReviewCache;
            _usedBikesCache = usedBikesCache;
            ParseQueryString(makeMasking, modelMasking);
        }

        #endregion Global Variables

        #region Methods

        public ModelPageVM GetData(uint? versionId)
        {
            try
            {
                objData = new ModelPageVM();

                if (_modelId > 0)
                {
                    objData.ModelId = _modelId;
                    #region Do Not change the sequence

                    CheckCityCookie();
                    objData.CityId = _cityId;
                    objData.AreaId = _areaId;
                    objData.VersionId = versionId.HasValue ? versionId.Value : 0;

                    objData.ModelPageEntity = FetchModelPageDetails(_modelId);

                    if (objData.IsModelDetails && objData.ModelPageEntity.ModelDetails.New)
                    {
                        FetchOnRoadPrice(objData.ModelPageEntity);
                    }

                    LoadVariants(objData.ModelPageEntity);

                    BindControls();

                    BindColorString();

                    CreateMetas();

                    #endregion Do Not change the sequence
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, String.Format("GetData({0})", ""));

            }

            return objData;

        }

        /// <summary>
        /// Created By :-Subodh Jain 07 oct 2016
        /// Desc:- To bind Description on model page
        /// Modified by :   Sumit Kate on 20 Jan 2017
        /// Description :   Model Page SEO changes
        /// </summary>
        private void BindDescription()
        {
            try
            {
                if (objData.IsModelDetails)
                {
                    string bikeModelName = objData.ModelPageEntity.ModelDetails.ModelName, specsDescirption = string.Empty;

                    string versionDescirption = objData.ModelPageEntity.ModelVersions.Count > 1 ? string.Format(" It is available in {0} versions", objData.ModelPageEntity.ModelVersions.Count) : string.Format(" It is available in {0} version", objData.ModelPageEntity.ModelVersions.Count);

                    string priceDescription = objData.ModelPageEntity.ModelDetails.MinPrice > 0 ? string.Format("Price - Rs. {0} onwards (Ex-showroom, {1}).", Bikewale.Utility.Format.FormatPrice(Convert.ToString(objData.ModelPageEntity.ModelDetails.MinPrice)), Bikewale.Utility.BWConfiguration.Instance.DefaultName) : string.Empty;
                    if (objData.ModelPageEntity != null && objData.ModelPageEntity.ModelVersionSpecs != null && (objData.ModelPageEntity.ModelVersionSpecs.TopSpeed > 0 || objData.ModelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall > 0))
                    {
                        if ((objData.ModelPageEntity.ModelVersionSpecs.TopSpeed > 0 && objData.ModelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall > 0))
                            specsDescirption = string.Format("{0} has a mileage of {1} kmpl and a top speed of {2} kmph.", bikeModelName, objData.ModelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall, objData.ModelPageEntity.ModelVersionSpecs.TopSpeed);
                        else if (objData.ModelPageEntity.ModelVersionSpecs.TopSpeed == 0)
                        {
                            specsDescirption = string.Format("{0} has a mileage of {1} kmpl.", bikeModelName, objData.ModelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall);
                        }
                        else
                        {
                            specsDescirption = string.Format("{0} has a top speed of {1} kmph.", bikeModelName, objData.ModelPageEntity.ModelVersionSpecs.TopSpeed);
                        }
                    }
                    objData.ModelSummary = string.Format("{0} {1}{2}.{3}{4}", objData.BikeName, priceDescription, versionDescirption, specsDescirption, colorStr);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.Models.BikeModels.ModelPage --> BindDescription()");

            }
        }

        /// <summary>
        /// Created By :-Subodh Jain 07 oct 2016
        /// Desc:- values to controls field
        /// Modified by :  Subodh Jain on 21 Dec 2016
        /// Description :  Added dealer card and service center card
        /// Modified by :   Sumit Kate on 02 Jan 2017
        /// Description :   Set makename,modelname,make and model masking name to news widget
        /// </summary>
        private void BindControls()
        {
            try
            {
                if (objData != null && objData.IsModelDetails)
                {
                    var objMake = objData.ModelPageEntity.ModelDetails.MakeBase;

                    objData.News = new RecentNews(3, (uint)objMake.MakeId, objData.ModelId, objMake.MakeName, objMake.MaskingName, objData.ModelPageEntity.ModelDetails.ModelName, objData.ModelPageEntity.ModelDetails.MaskingName, string.Format("{0} News", objData.BikeName), _objArticles).GetData();
                    objData.ExpertReviews = new RecentExpertReviews(3, (uint)objMake.MakeId, objData.ModelId, objMake.MakeName, objMake.MaskingName, objData.ModelPageEntity.ModelDetails.ModelName, objData.ModelPageEntity.ModelDetails.MaskingName, _objArticles, string.Format("{0} Reviews", objData.BikeName)).GetData();
                    objData.Videos = new RecentVideos(1, 3, (uint)objMake.MakeId, objMake.MakeName, objMake.MaskingName, objData.ModelId, objData.ModelPageEntity.ModelDetails.ModelName, objData.ModelPageEntity.ModelDetails.MaskingName, _objVideos).GetData();


                    if (!objData.IsUpcomingBike)
                    {
                        DealerCardWidget objDealer = new DealerCardWidget(_objDealerCache, _cityId, (uint)objMake.MakeId);
                        objDealer.TopCount = OtherDealersTopCount;
                        objData.OtherDealers = objDealer.GetData();


                        var objSimilarBikes = new SimilarBikesWidget(_objVersionCache, objData.VersionId, PQSourceEnum.Desktop_DPQ_Alternative);
                        if (objSimilarBikes != null)
                        {
                            objSimilarBikes.TopCount = 9;
                            objSimilarBikes.CityId = _cityId;
                            objData.SimilarBikes = objSimilarBikes.GetData();

                            objData.SimilarBikes.Make = objMake;
                            objData.SimilarBikes.Model = objData.ModelPageEntity.ModelDetails;
                            objData.SimilarBikes.VersionId = objData.VersionId;



                        }

                        if (_cityId > 0)
                        {
                            var dealerData = new DealerCardWidget(_objDealerCache, _cityId, (uint)objMake.MakeId);
                            dealerData.TopCount = 3;
                            objData.OtherDealers = dealerData.GetData();
                            objData.ServiceCenters = new ServiceCentersCard(_objServiceCenter, 3, (uint)objMake.MakeId, _cityId).GetData();
                        }
                        else
                        {
                            objData.DealersServiceCenter = new DealersServiceCentersIndiaWidgetModel((uint)objMake.MakeId, objMake.MakeName, objMake.MaskingName, _objDealerCache).GetData();
                        }

                        objData.UsedModels = BindUsedBikeByModel((uint)objMake.MakeId, _cityId);

                        objData.PriceInTopCities = new PriceInTopCities(_objPQCache, _modelId, 8).GetData();
                        if ((objData.PriceInTopCities != null && objData.PriceInTopCities.PriceQuoteList != null && objData.PriceInTopCities.PriceQuoteList.Count() > 0) || (objData.ModelPageEntity.ModelVersions != null && objData.ModelPageEntity.ModelVersions.Count > 0))
                        {
                            objData.IsShowPriceTab = true;
                        }


                        GetBikeRankingCategory();

                        BindUserReviews();

                        BindBestBikeWidget(objData.BikeRanking.BodyStyle, _cityId);

                        if (objData.IsNewBike)
                        {
                            objData.LeadCapture = new LeadCaptureEntity()
                            {
                                ModelId = _modelId,
                                CityId = _cityId,
                                AreaId = _areaId,
                                Area = objData.LocationCookie.Area,
                                City = objData.LocationCookie.City,
                                Location = objData.Location,
                                BikeName = objData.BikeName

                            };
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.ModelPage.BindControls");
            }
        }

        private void BindBestBikeWidget(EnumBikeBodyStyles BodyStyleType, uint? cityId = null)
        {
            try
            {

                objData.objBestBikesList = _objBestBikes.GetBestBikesByCategory(BodyStyleType, cityId).Reverse().Take(3);
                var PageMaskingName = GenericBikesCategoriesMapping.BodyStyleByType(BodyStyleType);
                objData.BestBikeHeading = new CultureInfo("en-US", false).TextInfo.ToTitleCase(PageMaskingName).Replace("-", " "); ;


            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("FetchBestBikesList{0} ", BodyStyleType));
            }
        }
        private UsedBikeByModelCityVM BindUsedBikeByModel(uint makeId, uint cityId)
        {
            UsedBikeByModelCityVM UsedBikeModel = new UsedBikeByModelCityVM();
            try
            {

                UsedBikesByModelCityWidget objUsedBike = new UsedBikesByModelCityWidget(_usedBikesCache, 6, makeId, _modelId, _cityId);
                UsedBikeModel = objUsedBike.GetData();
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ModelPage.BindUsedBikeByModel()");
            }

            return UsedBikeModel;

        }

        /// <summary>
        /// Created by  :   Sumit Kate on 03 Apr 2017
        /// Description :   Binds UserReviews for model
        /// </summary>
        private void BindUserReviews()
        {
            try
            {
                UserReviewsListWidget userReviews = new UserReviewsListWidget(_userReviewCache);
                userReviews.TopCount = 3;
                userReviews.ModelId = _modelId;
                userReviews.Filter = Entities.UserReviews.FilterBy.MostRecent;

                objData.UserReviews = userReviews.GetData();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.ModelPage.BindUserReviews");
            }
        }


        /// <summary>
        /// Created by : Aditi Srivastava on 13 Jan 2017
        /// Description: To get model ranking details
        /// </summary>
        /// <param name="modelId"></param>
        private void GetBikeRankingCategory()
        {
            BindGenericBikeRankingControl bikeRankingSlug = new BindGenericBikeRankingControl();
            bikeRankingSlug.ModelId = _modelId;
            var objBikeRanking = bikeRankingSlug.GetBikeRankingByModel();
            if (objBikeRanking != null)
            {
                objData.BikeRanking = new BikeRankingPropertiesEntity()
                {
                    ModelId = bikeRankingSlug.ModelId,
                    Rank = objBikeRanking.Rank,
                    BodyStyle = objBikeRanking.BodyStyle,
                    StyleName = bikeRankingSlug.StyleName,
                    BikeType = bikeRankingSlug.BikeType,
                    RankText = bikeRankingSlug.RankText

                };
            }

        }

        /// <summary>
        /// Created By :-Subodh Jain 07 oct 2016
        /// Desc:- Metas description according to discountinue,upcoming,continue bikes
        /// </summary>
        private void CreateMetas()
        {
            try
            {
                if (objData.IsModelDetails)
                {
                    if (objData.ModelPageEntity.ModelDetails.Futuristic && objData.ModelPageEntity.UpcomingBike != null)
                    {
                        objData.PageMetaTags.Description = string.Format("{0} {1} Price in India is expected between Rs. {2} and Rs. {3}. Check out {0} {1}  specifications, reviews, mileage, versions, news & images at BikeWale.com. Launch date of {1} is around {4}", objData.ModelPageEntity.ModelDetails.MakeBase.MakeName, objData.ModelPageEntity.ModelDetails.ModelName, Bikewale.Utility.Format.FormatNumeric(Convert.ToString(objData.ModelPageEntity.UpcomingBike.EstimatedPriceMin)), Bikewale.Utility.Format.FormatNumeric(Convert.ToString(objData.ModelPageEntity.UpcomingBike.EstimatedPriceMax)), objData.ModelPageEntity.UpcomingBike.ExpectedLaunchDate);
                    }
                    else if (!objData.ModelPageEntity.ModelDetails.New)
                    {
                        totalUsedBikes = objData.ModelPageEntity.ModelDetails.UsedListingsCnt;
                        objData.PageMetaTags.Description = string.Format("{0} {1} Price in India - Rs. {2}. It has been discontinued in India. There are {3} used {1} bikes for sale. Check out {1} specifications, reviews, mileage, versions, news & images at BikeWale.com", objData.ModelPageEntity.ModelDetails.MakeBase.MakeName, objData.ModelPageEntity.ModelDetails.ModelName, Bikewale.Utility.Format.FormatNumeric(objData.BikePrice.ToString()), totalUsedBikes);
                    }
                    else
                    {
                        objData.PageMetaTags.Description = String.Format("{0} Price in India - Rs. {1}. Find {2} Reviews, Specs, Features, Mileage, On Road Price and Images at Bikewale. {3}", objData.BikeName, Bikewale.Utility.Format.FormatNumeric(objData.BikePrice.ToString()), objData.ModelPageEntity.ModelDetails.ModelName, colorStr);
                    }

                    objData.PageMetaTags.Title = String.Format("{0} Price, Reviews, Spec, Images, Mileage, Colours | Bikewale", objData.BikeName);

                    objData.PageMetaTags.CanonicalUrl = String.Format("https://www.bikewale.com/{0}-bikes/{1}/", objData.ModelPageEntity.ModelDetails.MakeBase.MaskingName, objData.ModelPageEntity.ModelDetails.MaskingName);

                    objData.AdTags.TargetedModel = objData.ModelPageEntity.ModelDetails.ModelName;
                    objData.PageMetaTags.AlternateUrl = "https://www.bikewale.com/m/" + objData.ModelPageEntity.ModelDetails.MakeBase.MaskingName + "-bikes/" + objData.ModelPageEntity.ModelDetails.MaskingName + "/";
                    objData.AdTags.TargetedCity = objData.LocationCookie.City;
                    objData.PageMetaTags.Keywords = string.Format("{0},{0} Bike, bike, {0} Price, {0} Reviews, {0} Images, {0} Mileage", objData.BikeName);
                    objData.PageMetaTags.OGImage = Bikewale.Utility.Image.GetPathToShowImages(objData.ModelPageEntity.ModelDetails.OriginalImagePath, objData.ModelPageEntity.ModelDetails.HostUrl, Bikewale.Utility.ImageSize._476x268);


                    BindDescription();
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("Bikewale.Models.BikeModels.ModelPage --> CreateMetas() ModelId: {0}, MaskingName: {1}", _modelId, ""));
            }
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   20 Nov 2015
        /// Description     :   To Load version dropdown at Specs for each variant
        /// Modified by     :   Sumit Kate on 04 Apr 2017
        /// Description     :   Loads the default selected version based on pricing
        /// - Default version is selected based on the priority
        ///     Dealer Pricing (Highest priority) -> Bikewale Pricing -> Version pricing (Lowest)
        /// </summary>
        private void LoadVariants(BikeModelPageEntity modelPg)
        {
            try
            {
                if (modelPg != null && modelPg.ModelDetails != null)
                {
                    if (modelPg.ModelVersions != null && !modelPg.ModelDetails.Futuristic)
                    {
                        if (pqOnRoad != null)
                        {
                            ///Dealer Pricing
                            if (pqOnRoad.IsDealerPriceAvailable && pqOnRoad.DPQOutput != null && pqOnRoad.DPQOutput.Varients != null && pqOnRoad.DPQOutput.Varients.Count() > 0)
                            {
                                foreach (var version in modelPg.ModelVersions)
                                {
                                    var selectVersion = pqOnRoad.DPQOutput.Varients.Where(m => m.objVersion.VersionId == version.VersionId).FirstOrDefault();
                                    if (selectVersion != null)
                                        version.Price = selectVersion.OnRoadPrice;
                                }

                                ///Choose the min price version of dealer
                                if (objData.VersionId == 0)
                                {
                                    objData.VersionId = (uint)pqOnRoad.DPQOutput.Varients.OrderBy(m => m.OnRoadPrice).FirstOrDefault().objVersion.VersionId;
                                }
                            }//Bikewale Pricing
                            else if (pqOnRoad.BPQOutput != null && pqOnRoad.BPQOutput.Varients != null)
                            {
                                foreach (var version in modelPg.ModelVersions)
                                {
                                    var selected = pqOnRoad.BPQOutput.Varients.Where(p => p.VersionId == version.VersionId).FirstOrDefault();
                                    if (selected != null)
                                    {
                                        version.Price = selected.OnRoadPrice;
                                    }
                                }
                                ///Choose the min price version of city level pricing
                                if (objData.VersionId == 0)
                                {
                                    if (pqOnRoad.BPQOutput.Varients.Count() > 0)
                                        objData.VersionId = (uint)pqOnRoad.BPQOutput.Varients.OrderBy(m => m.OnRoadPrice).FirstOrDefault().VersionId;
                                }
                            }//Version Pricing
                            else
                            {
                                ///Choose the min price version
                                if (objData.VersionId == 0)
                                {
                                    var nonZeroVersion = modelPg.ModelVersions.Where(m => m.Price > 0);
                                    if (nonZeroVersion != null && nonZeroVersion.Count() > 0)
                                    {
                                        objData.SelectedVersion = nonZeroVersion.OrderBy(x => x.Price).FirstOrDefault();
                                        objData.VersionId = (uint)objData.SelectedVersion.VersionId;
                                        objData.BikePrice = objData.CityId == 0 ? (uint)objData.SelectedVersion.Price : 0;
                                    }
                                    else
                                    {
                                        objData.VersionId = (uint)modelPg.ModelVersions.FirstOrDefault().VersionId;
                                        objData.BikePrice = objData.CityId == 0 ? (uint)modelPg.ModelVersions.FirstOrDefault().Price : 0;
                                    }
                                }
                            }
                        }
                        else
                        {
                            ///Choose the min price version
                            if (objData.VersionId == 0)
                            {
                                var nonZeroVersion = modelPg.ModelVersions.Where(m => m.Price > 0);
                                if (nonZeroVersion != null && nonZeroVersion.Count() > 0)
                                {
                                    objData.SelectedVersion = nonZeroVersion.OrderBy(x => x.Price).FirstOrDefault();
                                    objData.VersionId = (uint)objData.SelectedVersion.VersionId;
                                    objData.BikePrice = objData.CityId == 0 ? (uint)objData.SelectedVersion.Price : 0;
                                }
                                else
                                {
                                    objData.VersionId = (uint)modelPg.ModelVersions.FirstOrDefault().VersionId;
                                    objData.BikePrice = objData.CityId == 0 ? (uint)objData.SelectedVersion.Price : 0;
                                }
                            }

                            if (objData.CityId != 0)
                            {
                                foreach (var version in modelPg.ModelVersions)
                                {
                                    version.Price = 0;
                                }
                            }


                        }

                        if (modelPg.ModelVersions.Count > 0)
                        {
                            var firstVer = modelPg.ModelVersions.FirstOrDefault();
                            if (firstVer != null)
                                objData.VersionName = firstVer.VersionName;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("Bikewale.Models.BikeModels.ModelPage --> LoadVariants() ModelId: {0}, MaskingName: {1}", _modelId, ""));
            }
        }


        private void ParseQueryString(string makeMasking, string modelMasking)
        {
            ModelMaskingResponse objResponse = null;
            try
            {
                if (!string.IsNullOrEmpty(modelMasking))
                {
                    objResponse = new ModelHelper().GetModelDataByMasking(modelMasking);

                    if (objResponse != null)
                    {
                        if (objResponse.StatusCode == 200)
                        {
                            _modelId = objResponse.ModelId;
                            Status = StatusCodes.ContentFound;
                        }
                        else if (objResponse.StatusCode == 301)
                        {
                            Status = StatusCodes.RedirectPermanent;
                            RedirectUrl = HttpContext.Current.Request.RawUrl.Replace(modelMasking, objResponse.MaskingName);
                        }
                        else
                        {
                            Status = StatusCodes.ContentNotFound;
                        }
                    }
                    else
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                }
                else
                {
                    Status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + "ParseQueryString");
                Status = StatusCodes.RedirectPermanent;
                RedirectUrl = "/new-bikes-in-india/";
            }
        }
        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   18 Nov 2015
        /// Modified by : Sajal Gupta on 28-02-2017
        /// Description : Get model page data from calling BAL layer instead of calling cache layer.
        /// </summary>
        private BikeModelPageEntity FetchModelPageDetails(uint modelID)
        {
            BikeModelPageEntity modelPg = null;
            try
            {
                if (modelID > 0)
                {

                    modelPg = _objModel.GetModelPageDetails(Convert.ToInt16(modelID), (int)objData.VersionId);

                    if (modelPg != null)
                    {
                        if (modelPg.ModelVersions != null)
                        {
                            if (objData.VersionId > 0)
                                objData.SelectedVersion = modelPg.ModelVersions.FirstOrDefault(v => v.VersionId == objData.VersionId);
                        }

                        if (objData.VersionId > 0 && objData.SelectedVersion != null)
                        {
                            objData.BikePrice = Convert.ToUInt32(objData.SelectedVersion.Price);
                        }
                        else if (modelPg.ModelDetails != null)
                        {
                            objData.BikePrice = Convert.ToUInt32(modelPg.ModelDetails.MinPrice);
                        }

                        // for new bike
                        if (!modelPg.ModelDetails.Futuristic && modelPg.ModelVersionSpecs != null && objData.SelectedVersion != null)
                        {
                            // Check it versionId passed through url exists in current model's versions
                            objData.VersionId = (uint)objData.SelectedVersion.VersionId;
                        }

                        //for all bikes including upcoming bikes as details are mandatory
                        if (modelPg.ModelDetails != null && modelPg.ModelDetails.ModelName != null && modelPg.ModelDetails.MakeBase != null)
                        {
                            objData.BikeName = string.Format("{0} {1}", modelPg.ModelDetails.MakeBase.MakeName, modelPg.ModelDetails.ModelName);
                        }

                        // Discontinued bikes
                        if (modelPg.ModelDetails != null && !modelPg.ModelDetails.New && modelPg.ModelVersions != null && modelPg.ModelVersions.Count > 1 && objData.SelectedVersion != null)
                        {
                            objData.BikePrice = (uint)objData.SelectedVersion.Price;
                        }

                        if (modelPg.ModelDetails != null && modelPg.ModelDetails.PhotosCount > 0 && modelPg.ModelColors != null && modelPg.ModelColors.Count() > 0)
                        {
                            var colorImages = modelPg.ModelColors.Where(x => x.ColorImageId > 0);
                            if (colorImages != null && colorImages.Count() > 0)
                            {
                                objData.ColourImageUrl = string.Format("/{0}-bikes/{1}/images/?modelpage=true&colorImageId={2}#modelGallery", modelPg.ModelDetails.MakeBase.MaskingName, modelPg.ModelDetails.MaskingName, colorImages.FirstOrDefault().ColorImageId);
                                objData.ModelColorPhotosCount = colorImages.Count();
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.BikeModels.ModelPage -> FetchmodelPgDetails(): Modelid ==> {0}", _modelId));
            }
            return modelPg;
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   20 Nov 2015
        /// Description     :   Fetch On road price depending on City, Area and DealerPQ and BWPQ
        /// Modified By     :   Sushil Kumar on 19th April 2016
        /// Description     :   Removed repeater binding for rptCategory and rptDiscount as view breakup popup removed
        /// Modified by     :   Sajal Gupta on 13-01-2017
        /// Description     :   Changed flag objData.IsLocationSelected if onroad price not available
        /// Modifide By :- Subodh jain on 02 March 2017
        /// Summary:- added manufacturer campaign leadpopup changes
        /// </summary>
        private void FetchOnRoadPrice(BikeModelPageEntity modelPage)
        {
            var errorParams = string.Empty;
            try
            {
                if (_cityId > 0 && objData.City != null)
                {
                    pqOnRoad = GetOnRoadPrice();
                    // Set Pricequote Cookie
                    if (pqOnRoad != null)
                    {

                        if (pqOnRoad.PriceQuote != null)
                        {
                            objData.DealerId = pqOnRoad.PriceQuote.DealerId;
                            //objData.VersionId = pqOnRoad.PriceQuote.DefaultVersionId > 0 ? pqOnRoad.PriceQuote.DefaultVersionId : pqOnRoad.PriceQuote.VersionId;

                        }
                        objData.MPQString = EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(_cityId.ToString(), pqOnRoad.PriceQuote.PQId.ToString(), _areaId.ToString(), objData.VersionId.ToString(), objData.DealerId.ToString()));

                        if (pqOnRoad.IsDealerPriceAvailable && pqOnRoad.DPQOutput != null && pqOnRoad.DPQOutput.Varients != null && pqOnRoad.DPQOutput.Varients.Count() > 0)
                        {
                            #region when dealer Price is Available

                            var selectedVariant = pqOnRoad.DPQOutput.Varients.Where(p => p.objVersion.VersionId == objData.VersionId).FirstOrDefault();
                            if (selectedVariant != null)
                            {
                                objData.BikePrice = selectedVariant.OnRoadPrice;
                                uint totalDiscountedPrice = 0;
                                if (selectedVariant.PriceList != null)
                                {
                                    totalDiscountedPrice = CommonModel.GetTotalDiscount(pqOnRoad.discountedPriceList);
                                }

                                if (pqOnRoad.discountedPriceList != null && pqOnRoad.discountedPriceList.Count > 0)
                                {
                                    objData.BikePrice = (objData.BikePrice - totalDiscountedPrice);
                                }
                            }
                            else // Show dealer properties and Bikewale priceQuote when dealer has pricing for any of the bike
                            // Added on 13 Feb 2017 Pivotal Id:138698777
                            {
                                SetBikeWalePQ(pqOnRoad);
                            }
                            #endregion when dealer Price is Available
                        }
                        else
                        {
                            SetBikeWalePQ(pqOnRoad);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(errorParams))
                    errorParams = "=== modelpage ===" + Newtonsoft.Json.JsonConvert.SerializeObject(modelPage);
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.Models.BikeModels.ModelPage -> FetchOnRoadPrice() " + " ===== parameters ========= " + errorParams);
            }
            finally
            {
                if (_cityId > 0 && _areaId > 0 && objData.VersionId > 0)
                {
                    objData.DetailedDealer = _objDealerDetails.GetDealerQuotationV2(_cityId, objData.VersionId, objData.DealerId, _areaId);
                }
                if (objData.ManufacturerCampaign != null)
                {
                    objData.ManufacturerCampaign.ShowAd = objData.DetailedDealer == null && objData.IsLocationSelected;

                }
            }
        }
        /// <summary>
        /// Created by : Sangram Nandkhile on 14 Feb 2017
        /// Summary: To set price variable with bikewale pricequote
        /// </summary>
        /// <param name="pqOnRoad"></param>
        private void SetBikeWalePQ(PQOnRoadPrice pqOnRoad)
        {
            if (pqOnRoad != null && pqOnRoad.BPQOutput != null)
            {
                objData.CampaignId = pqOnRoad.BPQOutput.CampaignId;
            }

            objData.ManufacturerCampaign = new ManufacturerCampaign()
            {
                Ad = Format.FormatManufacturerAd(pqOnRoad.BPQOutput.ManufacturerAd, pqOnRoad.BPQOutput.CampaignId, pqOnRoad.BPQOutput.ManufacturerName, pqOnRoad.BPQOutput.MaskingNumber, pqOnRoad.BPQOutput.ManufacturerId, pqOnRoad.BPQOutput.Area, objData.PQLeadSource.ToString(), objData.PQSourcePage.ToString(), string.Empty, string.Empty, string.Empty, string.IsNullOrEmpty(pqOnRoad.BPQOutput.MaskingNumber) ? "hide" : string.Empty, pqOnRoad.BPQOutput.LeadCapturePopupHeading, pqOnRoad.BPQOutput.LeadCapturePopupDescription, pqOnRoad.BPQOutput.LeadCapturePopupMessage, pqOnRoad.BPQOutput.PinCodeRequired),
                Name = pqOnRoad.BPQOutput.ManufacturerName,
                Id = pqOnRoad.BPQOutput.ManufacturerId
            };

            if (pqOnRoad.BPQOutput != null && pqOnRoad.BPQOutput.Varients != null && objData.VersionId > 0)
            {
                var objSelectedVariant = pqOnRoad.BPQOutput.Varients.Where(p => p.VersionId == objData.VersionId).FirstOrDefault();
                if (objSelectedVariant != null)
                    objData.BikePrice = objData.IsLocationSelected ? Convert.ToUInt32(objSelectedVariant.OnRoadPrice) : Convert.ToUInt32(objSelectedVariant.Price);

                objData.IsBPQAvailable = true;
            }
        }

        /// <summary>
        /// Author: Sangram Nandkhile
        /// Desc: Removed API Call for on road Price Quote
        /// Modified By : Lucky Rathore on 09 May 2016.
        /// Description : modelImage intialize.
        /// Modified By : Lucky Rathore on 27 June 2016
        /// Description : replace cookie __utmz with _bwutmz
        /// </summary>
        /// <returns></returns>
        private PQOnRoadPrice GetOnRoadPrice()
        {
            try
            {
                BikeQuotationEntity bpqOutput = null;
                PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                objPQEntity.CityId = Convert.ToUInt16(_cityId);
                objPQEntity.AreaId = Convert.ToUInt32(_areaId);
                objPQEntity.ClientIP = "";
                objPQEntity.SourceId = 1;
                objPQEntity.ModelId = _modelId;
                objPQEntity.VersionId = objData.VersionId;
                objPQEntity.PQLeadId = Convert.ToUInt16(PQSourceEnum.Desktop_ModelPage);
                objPQEntity.UTMA = HttpContext.Current.Request.Cookies["__utma"] != null ? HttpContext.Current.Request.Cookies["__utma"].Value : "";
                objPQEntity.UTMZ = HttpContext.Current.Request.Cookies["_bwutmz"] != null ? HttpContext.Current.Request.Cookies["_bwutmz"].Value : "";
                objPQEntity.DeviceId = HttpContext.Current.Request.Cookies["BWC"] != null ? HttpContext.Current.Request.Cookies["BWC"].Value : "";
                PQOutputEntity objPQOutput = _objDealerPQ.ProcessPQV2(objPQEntity);

                if (objPQOutput != null)
                {
                    if (objData.VersionId == 0)
                        objData.VersionId = objPQOutput.VersionId;
                    //objData.VersionId = objPQOutput.DefaultVersionId;
                    pqOnRoad = new PQOnRoadPrice();
                    pqOnRoad.PriceQuote = objPQOutput;
                    if (objPQOutput != null && objPQOutput.PQId > 0)
                    {
                        objData.PQId = (uint)objPQOutput.PQId;
                        bpqOutput = _objPQ.GetPriceQuoteById(objPQOutput.PQId, LeadSourceEnum.Model_Desktop);
                        bpqOutput.Varients = _objPQCache.GetOtherVersionsPrices(_modelId, _cityId);
                        if (bpqOutput != null)
                        {
                            pqOnRoad.BPQOutput = bpqOutput;
                        }
                        if (objPQOutput.DealerId != 0)
                        {
                            PQ_QuotationEntity oblDealerPQ = null;
                            AutoBizCommon dealerPq = new AutoBizCommon();
                            try
                            {
                                oblDealerPQ = dealerPq.GetDealePQEntity(_cityId, objPQOutput.DealerId, objData.VersionId);
                                if (oblDealerPQ != null)
                                {
                                    uint insuranceAmount = 0;
                                    foreach (var price in oblDealerPQ.PriceList)
                                    {
                                        pqOnRoad.IsInsuranceFree = Bikewale.Utility.DealerOfferHelper.HasFreeInsurance(objPQOutput.DealerId.ToString(), string.Empty, price.CategoryName, price.Price, ref insuranceAmount);
                                    }
                                    pqOnRoad.IsInsuranceFree = true;
                                    pqOnRoad.DPQOutput = oblDealerPQ;
                                    if (pqOnRoad.DPQOutput.objOffers != null && pqOnRoad.DPQOutput.objOffers.Count > 0)
                                        pqOnRoad.DPQOutput.discountedPriceList = OfferHelper.ReturnDiscountPriceList(pqOnRoad.DPQOutput.objOffers, pqOnRoad.DPQOutput.PriceList);
                                    pqOnRoad.InsuranceAmount = insuranceAmount;
                                    if (oblDealerPQ.discountedPriceList != null && oblDealerPQ.discountedPriceList.Count > 0)
                                    {
                                        pqOnRoad.IsDiscount = true;
                                        pqOnRoad.discountedPriceList = oblDealerPQ.discountedPriceList;
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("Bikewale.Models.BikeModels.ModelPage --> GetOnRoadPrice() ModelId: {0}, MaskingName: {1}", _modelId, ""));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("Bikewale.Models.BikeModels.ModelPage --> GetOnRoadPrice() ModelId: {0}, MaskingName: {1}", _modelId, ""));
            }
            return pqOnRoad;
        }

        /// <summary>
        /// Modified by :   Sumit Kate on 05 Jan 2016
        /// Description :   Replaced the Convert.ToXXX with XXX.TryParse method
        /// Modified By : Sushil Kumar on 26th August 2016
        /// Description : Replaced location name from location cookie to selected location objects for city and area respectively.
        /// </summary>
        private void CheckCityCookie()
        {

            if (_modelId > 0)
            {
                objData.LocationCookie = GlobalCityArea.GetGlobalCityArea();
                _cityId = objData.LocationCookie.CityId;
                _areaId = objData.LocationCookie.AreaId;
                if (objData.LocationCookie.CityId > 0)
                {
                    var cities = _objCityCache.GetPriceQuoteCities(_modelId);

                    if (cities != null)
                    {
                        var selectedCity = cities.FirstOrDefault(m => m.CityId == _cityId);

                        objData.City = selectedCity;
                        objData.ShowOnRoadButton = selectedCity != null && selectedCity.HasAreas && _areaId <= 0;
                        objData.IsAreaSelected = selectedCity != null && selectedCity.HasAreas && _areaId > 0;
                        if (!objData.IsAreaSelected) _areaId = 0;
                    }
                }
            }
        }

        private void BindColorString()
        {
            try
            {
                if (objData.ModelPageEntity != null && objData.ModelPageEntity.ModelColors != null && objData.ModelPageEntity.ModelColors.Count() > 0)
                {
                    colorCount = objData.ModelPageEntity.ModelColors.Count();
                    string lastColor = objData.ModelPageEntity.ModelColors.Last().ColorName;
                    if (colorCount > 1)
                    {
                        colorStr.AppendFormat("{0} is available in {1} different colours : ", objData.BikeName, colorCount);
                        var colorArr = objData.ModelPageEntity.ModelColors.Select(x => x.ColorName).Take(colorCount - 1);
                        // Comma separated colors (except last one)
                        colorStr.Append(string.Join(",", colorArr));
                        // Append last color with And
                        colorStr.AppendFormat(" and {0}.", lastColor);
                    }
                    else if (colorCount == 1)
                    {
                        colorStr.AppendFormat("{0} is available in {1} colour.", objData.BikeName, lastColor);
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.Models.BikeModels.ModelPage -->" + "BindColorString()");
            }
        }

        #endregion Methods

    }
}