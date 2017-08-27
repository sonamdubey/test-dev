using Bikewale.BindViewModels.Controls;
using Bikewale.BindViewModels.Webforms;
using Bikewale.common;
using Bikewale.Common;
using Bikewale.DTO.PriceQuote;
using Bikewale.Entities;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.manufacturecampaign;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.UserReviews;

using Bikewale.Entities.UserReviews.Search;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.UserReviews.Search;
using Bikewale.Interfaces.Videos;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Models.PriceInCity;
using Bikewale.Models.ServiceCenters;
using Bikewale.Models.Used;
using Bikewale.Models.UserReviews;
using Bikewale.Utility;
using Bikewale.Utility.GenericBikes;
using System;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using Bikewale.Entities.Pages;

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
        private readonly IBikeModels<Entities.BikeData.BikeModelEntity, int> _objModel = null;
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
        private readonly IUpcoming _upcoming = null;

        private readonly IUserReviewsCache _userReviewsCache = null;
        private readonly IUserReviewsSearch _userReviewsSearch = null;

        private uint _modelId, _cityId, _areaId;




        private readonly IManufacturerCampaign _objManufacturerCampaign = null;


        private ModelPageVM _objData = null;
        private PQOnRoadPrice _pqOnRoad;
        private StringBuilder _colorStr = new StringBuilder();



        public string RedirectUrl { get; set; }
        public StatusCodes Status { get; set; }
        public uint OtherDealersTopCount { get; set; }
        public PQSources Source { get; set; }
        public PQSourceEnum PQSource { get; set; }
        public LeadSourceEnum LeadSource { get; set; }
        public bool IsMobile { get; set; }
        public ManufacturerCampaignServingPages ManufacturerCampaignPageId { get; set; }

        public ModelPage(string makeMasking, string modelMasking, IUserReviewsSearch userReviewsSearch, IUserReviewsCache userReviewsCache, IBikeModels<Entities.BikeData.BikeModelEntity, int> objModel, IDealerPriceQuote objDealerPQ, IAreaCacheRepository objAreaCache, ICityCacheRepository objCityCache, IPriceQuote objPQ, IDealerCacheRepository objDealerCache, IDealerPriceQuoteDetail objDealerDetails, IBikeVersionCacheRepository<BikeVersionEntity, uint> objVersionCache, ICMSCacheContent objArticles, IVideos objVideos, IUsedBikeDetailsCacheRepository objUsedBikescache, IServiceCenter objServiceCenter, IPriceQuoteCache objPQCache, IBikeCompareCacheRepository objCompare, IUserReviewsCache userReviewCache, IUsedBikesCache usedBikesCache, IBikeModelsCacheRepository<int> objBestBikes, IUpcoming upcoming, IManufacturerCampaign objManufacturerCampaign)
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
            _upcoming = upcoming;

            _objArticles = objArticles;
            _objVideos = objVideos;
            _objUsedBikescache = objUsedBikescache;
            _objServiceCenter = objServiceCenter;
            _objPQCache = objPQCache;
            _objCompare = objCompare;
            _userReviewCache = userReviewCache;
            _usedBikesCache = usedBikesCache;
            _objManufacturerCampaign = objManufacturerCampaign;
            _userReviewsSearch = userReviewsSearch;
            _userReviewsCache = userReviewsCache;
            ParseQueryString(makeMasking, modelMasking);
        }

        #endregion Global Variables

        #region Methods

        public ModelPageVM GetData(uint? versionId)
        {
            try
            {
                _objData = new ModelPageVM();

                if (_modelId > 0)
                {
                    _objData.ModelId = _modelId;
                    #region Do Not change the sequence

                    CheckCityCookie();
                    _objData.CityId = _cityId;
                    _objData.AreaId = _areaId;
                    _objData.VersionId = versionId.HasValue ? versionId.Value : 0;

                    _objData.ModelPageEntity = FetchModelPageDetails(_modelId);

                    if (_objData.IsModelDetails && _objData.ModelPageEntity.ModelDetails.New)
                    {
                        FetchOnRoadPrice(_objData.ModelPageEntity);
                        GetManufacturerCampaign();
                    }

                    LoadVariants(_objData.ModelPageEntity);

                    BindControls();

                    BindColorString();

                    CreateMetas();

                    BindVersionPriceListSummary();

                    #endregion Do Not change the sequence
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, String.Format("GetData({0})", ""));
            }

            return _objData;

        }

        private void BindVersionPriceListSummary()
        {
            try
            {
                if (_objData.IsModelDetails)
                {
                    string specsDescirption = string.Empty;
                    string cityList = string.Empty;
                    string priceDescription = string.Empty;
                    string versionDescirption = _objData.ModelPageEntity.ModelVersions.Count > 1 ? string.Format(" It is available in {0} versions", _objData.ModelPageEntity.ModelVersions.Count) : string.Format(" It is available in {0} version", _objData.ModelPageEntity.ModelVersions.Count);
                    ushort index = 0;
                    if (_objData.ModelPageEntity.ModelVersions != null)
                    {
                        if (_objData.ModelPageEntity.ModelVersions.Count() > 1)
                        {
                            versionDescirption += " - ";
                            foreach (var version in _objData.ModelPageEntity.ModelVersions)
                            {
                                index++;
                                if (_objData.ModelPageEntity.ModelVersions.Count() <= index)
                                    break;
                                else if (index > 1)
                                    versionDescirption += ",";
                                versionDescirption = string.Format("{0} {1}", versionDescirption, version.VersionName);


                            }
                            versionDescirption = string.Format("{0} and {1}", versionDescirption, _objData.ModelPageEntity.ModelVersions.Last().VersionName);
                        }
                        else if (_objData.ModelPageEntity.ModelVersions.Count() == 1)
                            versionDescirption += string.Format(" - {0}", _objData.ModelPageEntity.ModelVersions.First().VersionName);
                    }



                    if (_objData.BikePrice > 0 && _objData.IsLocationSelected && _objData.City != null && !_objData.ShowOnRoadButton)
                        priceDescription = string.Format("Price - &#x20B9; {0} onwards (On-road, {1}).", Bikewale.Utility.Format.FormatPrice(Convert.ToString(_objData.BikePrice)), _objData.City.CityName);
                    else
                        priceDescription = _objData.ModelPageEntity.ModelDetails.MinPrice > 0 ? string.Format("Price - &#x20B9; {0} onwards (Ex-showroom, {1}).", Bikewale.Utility.Format.FormatPrice(Convert.ToString(_objData.ModelPageEntity.ModelDetails.MinPrice)), Bikewale.Utility.BWConfiguration.Instance.DefaultName) : string.Empty;


                    index = 0;
                    if (_objData.PriceInTopCities != null && _objData.PriceInTopCities.PriceQuoteList != null && _objData.PriceInTopCities.PriceQuoteList.Count() > 1)
                    {
                        cityList = _objData.PriceInTopCities.PriceQuoteList != null && _objData.PriceInTopCities.PriceQuoteList.Count() > 0 ? string.Format(". See price of {0} in top {1}:", _objData.ModelPageEntity.ModelDetails.ModelName, _objData.PriceInTopCities.PriceQuoteList.Count() > 1 ? "cities" : "city") : string.Empty;
                        foreach (var city in _objData.PriceInTopCities.PriceQuoteList)
                        {
                            index++;
                            cityList = string.Format("{0} {1}", cityList, city.CityName);
                            if (index >= 3 || _objData.PriceInTopCities.PriceQuoteList.Count() <= index)
                                break;
                            else if (index >= 1)
                                cityList += ",";

                        }
                        cityList = string.Format("{0} and {1}", cityList, _objData.PriceInTopCities.PriceQuoteList.Last().CityName);
                    }

                    if (_objData.PriceInTopCities != null && _objData.PriceInTopCities.PriceQuoteList != null && _objData.PriceInTopCities.PriceQuoteList.Count() == 1)
                    {
                        cityList = string.Format("{0}", _objData.PriceInTopCities.PriceQuoteList.First().CityName);
                    }


                    _objData.VersionPriceListSummary = string.Format("{0} {1}{2}{3}.", _objData.BikeName, priceDescription, versionDescirption, cityList);
                }
            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.Models.BikeModels.ModelPage --> BindVersionPriceListSummary()");
            }



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
                if (_objData.IsModelDetails)
                {
                    string bikeModelName = _objData.ModelPageEntity.ModelDetails.ModelName, specsDescirption = string.Empty;

                    string versionDescirption = _objData.ModelPageEntity.ModelVersions.Count > 1 ? string.Format(" It is available in {0} versions", _objData.ModelPageEntity.ModelVersions.Count) : string.Format(" It is available in {0} version", _objData.ModelPageEntity.ModelVersions.Count);

                    string priceDescription = string.Empty;
                    if (_objData.BikePrice > 0 && _objData.IsLocationSelected && _objData.City != null && !_objData.ShowOnRoadButton)
                        priceDescription = string.Format("Price - &#x20B9; {0} onwards (On-road, {1}).", Bikewale.Utility.Format.FormatPrice(Convert.ToString(_objData.BikePrice)), _objData.City.CityName);
                    else
                        priceDescription = _objData.ModelPageEntity.ModelDetails.MinPrice > 0 ? string.Format("Price - &#x20B9; {0} onwards (Ex-showroom, {1}).", Bikewale.Utility.Format.FormatPrice(Convert.ToString(_objData.ModelPageEntity.ModelDetails.MinPrice)), Bikewale.Utility.BWConfiguration.Instance.DefaultName) : string.Empty;
                    if (_objData.ModelPageEntity != null && _objData.ModelPageEntity.ModelVersionSpecs != null && (_objData.ModelPageEntity.ModelVersionSpecs.TopSpeed > 0 || _objData.ModelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall > 0))
                    {
                        if ((_objData.ModelPageEntity.ModelVersionSpecs.TopSpeed > 0 && _objData.ModelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall > 0))
                            specsDescirption = string.Format("{0} has a mileage of {1} kmpl and a top speed of {2} kmph.", bikeModelName, _objData.ModelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall, _objData.ModelPageEntity.ModelVersionSpecs.TopSpeed);
                        else if (_objData.ModelPageEntity.ModelVersionSpecs.TopSpeed == 0)
                        {
                            specsDescirption = string.Format("{0} has a mileage of {1} kmpl.", bikeModelName, _objData.ModelPageEntity.ModelVersionSpecs.FuelEfficiencyOverall);
                        }
                        else
                        {
                            specsDescirption = string.Format("{0} has a top speed of {1} kmph.", bikeModelName, _objData.ModelPageEntity.ModelVersionSpecs.TopSpeed);
                        }
                    }
                    _objData.ModelSummary = string.Format("{0} {1}{2}. {3} {4}", _objData.BikeName, priceDescription, versionDescirption, specsDescirption, _colorStr);
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
                if (_objData != null && _objData.IsModelDetails)
                {
                    var objMake = _objData.ModelPageEntity.ModelDetails.MakeBase;

                    _objData.News = new RecentNews(3, (uint)objMake.MakeId, _objData.ModelId, objMake.MakeName, objMake.MaskingName, _objData.ModelPageEntity.ModelDetails.ModelName, _objData.ModelPageEntity.ModelDetails.MaskingName, "News", _objArticles).GetData();
                    _objData.ExpertReviews = new RecentExpertReviews(3, (uint)objMake.MakeId, _objData.ModelId, objMake.MakeName, objMake.MaskingName, _objData.ModelPageEntity.ModelDetails.ModelName, _objData.ModelPageEntity.ModelDetails.MaskingName, _objArticles, string.Format("{0} Reviews", _objData.BikeName)).GetData();
                    _objData.Videos = new RecentVideos(1, 3, (uint)objMake.MakeId, objMake.MakeName, objMake.MaskingName, _objData.ModelId, _objData.ModelPageEntity.ModelDetails.ModelName, _objData.ModelPageEntity.ModelDetails.MaskingName, _objVideos).GetData();
                    _objData.ReturnUrl = Utils.Utils.EncryptTripleDES(string.Format("returnUrl=/{0}-bikes/{1}/&sourceid={2}", objMake.MaskingName, _objData.ModelPageEntity.ModelDetails.MaskingName, (int)(IsMobile ? UserReviewPageSourceEnum.Mobile_ModelPage : UserReviewPageSourceEnum.Desktop_ModelPage)));



                    if (!_objData.IsUpcomingBike)
                    {
                        DealerCardWidget objDealer = new DealerCardWidget(_objDealerCache, _cityId, (uint)objMake.MakeId);
                        objDealer.TopCount = OtherDealersTopCount;
                        _objData.OtherDealers = objDealer.GetData();


                        var objSimilarBikes = new SimilarBikesWidget(_objVersionCache, _objData.VersionId, PQSourceEnum.Desktop_DPQ_Alternative);
                        if (objSimilarBikes != null)
                        {
                            objSimilarBikes.TopCount = 9;
                            objSimilarBikes.CityId = _cityId;
                            _objData.SimilarBikes = objSimilarBikes.GetData();

                            _objData.SimilarBikes.Make = objMake;
                            _objData.SimilarBikes.Model = _objData.ModelPageEntity.ModelDetails;
                            _objData.SimilarBikes.VersionId = _objData.VersionId;



                        }

                        if (_cityId > 0)
                        {
                            var dealerData = new DealerCardWidget(_objDealerCache, _cityId, (uint)objMake.MakeId);
                            dealerData.TopCount = 3;
                            _objData.OtherDealers = dealerData.GetData();
                            _objData.ServiceCenters = new ServiceCentersCard(_objServiceCenter, 3, (uint)objMake.MakeId, _cityId).GetData();
                        }
                        else
                        {
                            _objData.DealersServiceCenter = new DealersServiceCentersIndiaWidgetModel((uint)objMake.MakeId, objMake.MakeName, objMake.MaskingName, _objDealerCache).GetData();
                        }

                        _objData.UsedModels = BindUsedBikeByModel((uint)objMake.MakeId, _cityId);

                        _objData.PriceInTopCities = new PriceInTopCities(_objPQCache, _modelId, 8).GetData();
                        if ((_objData.PriceInTopCities != null && _objData.PriceInTopCities.PriceQuoteList != null && _objData.PriceInTopCities.PriceQuoteList.Count() > 0) || (_objData.ModelPageEntity.ModelVersions != null && _objData.ModelPageEntity.ModelVersions.Count > 0))
                        {
                            _objData.IsShowPriceTab = true;
                        }


                        GetBikeRankingCategory();


                        BindUserReviewsWidget(_objData);


                        if (_objData.BikeRanking != null)
                        {
                            BindBestBikeWidget(_objData.BikeRanking.BodyStyle, _cityId);
                        }

                        if (_objData.IsNewBike)
                        {
                            _objData.LeadCapture = new LeadCaptureEntity()
                            {
                                ModelId = _modelId,
                                CityId = _cityId,
                                AreaId = _areaId,
                                Area = _objData.LocationCookie.Area,
                                City = _objData.LocationCookie.City,
                                Location = _objData.Location,
                                BikeName = _objData.BikeName,
                                IsManufacturerCampaign = _objData.IsManufacturerLeadAdShown || _objData.IsManufacturerEMIAdShown || _objData.IsManufacturerTopLeadAdShown

                            };

                            _objData.EMIDetails = setDefaultEMIDetails(_objData.BikePrice);
                        }



                    }
                    if (_objData.IsUpcomingBike)
                    {

                        _objData.objUpcomingBikes = BindUpCompingBikesWidget();
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.ModelPage.BindControls");
            }
        }
        /// <summary>
        /// created by :- Subodh Jain on 17 july 2017
        /// Summary added BindUserReviewSWidget
        /// </summary>
        /// <param name="makeMasking"></param>
        /// <param name="modelMasking"></param>
        /// <param name="versionId"></param>
        /// <returns></returns>
        public void BindUserReviewsWidget(ModelPageVM objPage)
        {
            try
            {
                InputFilters filters = null;

                filters = new InputFilters()
                {
                    Model = _modelId.ToString(),
                    SO = 1,
                    PN = 1,
                    PS = 3,
                    Reviews = true

                };
                var objUserReviews = new UserReviewsSearchWidget(_modelId, filters, _userReviewsCache, _userReviewsSearch);
                if (objUserReviews != null)
                {
                    objUserReviews.ActiveReviewCateory = FilterBy.MostRecent;
                    objPage.UserReviews = objUserReviews.GetData();

                }


            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Model.BindUserReviewSWidget()"));
            }
        }
        /// <summary>
        /// Created By:- Subodh Jain 23 March 2017
        /// Summary:- Binding data for upcoming bike widget
        /// </summary>
        /// <returns></returns>
        private UpcomingBikesWidgetVM BindUpCompingBikesWidget()
        {
            UpcomingBikesWidgetVM objUpcomingBikes = null;
            try
            {
                UpcomingBikesWidget objUpcoming = new UpcomingBikesWidget(_upcoming);

                objUpcoming.Filters = new Bikewale.Entities.BikeData.UpcomingBikesListInputEntity()
                {
                    PageSize = 10,
                    PageNo = 1,
                };
                objUpcoming.SortBy = Bikewale.Entities.BikeData.EnumUpcomingBikesFilter.Default;
                objUpcomingBikes = objUpcoming.GetData();
                if (objUpcomingBikes != null && objUpcomingBikes.UpcomingBikes != null)
                    objUpcomingBikes.UpcomingBikes = objUpcomingBikes.UpcomingBikes.Where(x => x.ModelBase.ModelId != _modelId);
                if (objUpcomingBikes != null && objUpcomingBikes.UpcomingBikes != null)
                    objUpcomingBikes.UpcomingBikes = objUpcomingBikes.UpcomingBikes.Take(9);

            }
            catch (Exception ex)
            {

                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.Models.ModelPage.BindUpCompingBikesWidget()");
            }
            return objUpcomingBikes;
        }

        /// <summary>
        /// Created BY : Sushil Kumar on 14th March 2015
        /// Summary : To set EMI details for the dealer if no EMI Details available for the dealer
        /// </summary>
        private EMI setDefaultEMIDetails(uint bikePrice)
        {
            EMI _objEMI = null;
            try
            {
                _objEMI = new EMI();
                _objEMI.MaxDownPayment = 40 * bikePrice / 100;
                _objEMI.MinDownPayment = 10 * bikePrice / 100;
                _objEMI.MaxTenure = 48;
                _objEMI.MinTenure = 12;
                _objEMI.MaxRateOfInterest = 15;
                _objEMI.MinRateOfInterest = 10;
                _objEMI.ProcessingFee = 0; //2000 

                _objEMI.Tenure = Convert.ToUInt16((_objEMI.MaxTenure - _objEMI.MinTenure) / 2 + _objEMI.MinTenure);
                _objEMI.RateOfInterest = (_objEMI.MaxRateOfInterest - _objEMI.MinRateOfInterest) / 2 + _objEMI.MinRateOfInterest;
                _objEMI.MinLoanToValue = Convert.ToUInt32(Math.Round(bikePrice * 0.7, MidpointRounding.AwayFromZero));
                _objEMI.MaxLoanToValue = bikePrice;
                _objEMI.EMIAmount = Convert.ToUInt32((_objEMI.MinLoanToValue * _objEMI.Tenure * _objEMI.RateOfInterest) / (12 * 100));
                _objEMI.EMIAmount = Convert.ToUInt32(Math.Round((_objEMI.MinLoanToValue + _objEMI.EMIAmount + _objEMI.ProcessingFee) / _objEMI.Tenure, MidpointRounding.AwayFromZero));

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "setDefaultEMIDetails");
            }
            return _objEMI;
        }

        private void BindBestBikeWidget(EnumBikeBodyStyles BodyStyleType, uint? cityId = null)
        {
            try
            {

                _objData.objBestBikesList = _objBestBikes.GetBestBikesByCategory(BodyStyleType, cityId).Reverse().Take(3);
                var PageMaskingName = GenericBikesCategoriesMapping.BodyStyleByType(BodyStyleType);
                _objData.BestBikeHeading = new CultureInfo("en-US", false).TextInfo.ToTitleCase(PageMaskingName).Replace("-", " "); ;


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
        private ReviewListBase BindUserReviews()
        {
            ReviewListBase objReviews = new ReviewListBase();
            try
            {
                UserReviewsListWidget userReviews = new UserReviewsListWidget(_userReviewCache);
                userReviews.TopCount = 3;
                userReviews.ModelId = _modelId;
                userReviews.Filter = Entities.UserReviews.FilterBy.MostRecent;

                objReviews = userReviews.GetData();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.ModelPage.BindUserReviews");
            }

            return objReviews;
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
                _objData.BikeRanking = new BikeRankingPropertiesEntity()
                {
                    ModelId = bikeRankingSlug.ModelId,
                    Rank = objBikeRanking.Rank,
                    BodyStyle = objBikeRanking.BodyStyle,
                    StyleName = bikeRankingSlug.StyleName,
                    BikeType = bikeRankingSlug.BikeType,
                    RankText = bikeRankingSlug.RankText

                };
            }
            else
                _objData.BikeRanking = new BikeRankingPropertiesEntity();

        }

        /// <summary>
        /// Created By :-Subodh Jain 07 oct 2016
        /// Desc:- Metas description according to discountinue,upcoming,continue bikes
        /// Modified by :- Subodh Jain 19 june 2017
        /// Summary :- Added TargetModels and Target Make
        /// </summary>
        private void CreateMetas()
        {
            try
            {
                if (_objData.IsModelDetails)
                {
                    if (_objData.ModelPageEntity.ModelDetails.Futuristic && _objData.ModelPageEntity.UpcomingBike != null)
                    {
                        _objData.PageMetaTags.Description = string.Format("{0} {1} Price in India is expected between Rs. {2} and Rs. {3}. Check out {0} {1}  specifications, reviews, mileage, versions, news & images at BikeWale.com. Launch date of {1} is around {4}", _objData.ModelPageEntity.ModelDetails.MakeBase.MakeName, _objData.ModelPageEntity.ModelDetails.ModelName, Bikewale.Utility.Format.FormatNumeric(Convert.ToString(_objData.ModelPageEntity.UpcomingBike.EstimatedPriceMin)), Bikewale.Utility.Format.FormatNumeric(Convert.ToString(_objData.ModelPageEntity.UpcomingBike.EstimatedPriceMax)), _objData.ModelPageEntity.UpcomingBike.ExpectedLaunchDate);
                    }
                    else if (!_objData.ModelPageEntity.ModelDetails.New)
                    {
                        _objData.PageMetaTags.Description = string.Format("{0} {1} Price in India - Rs. {2}. It has been discontinued in India. There are {3} used {1} bikes for sale. Check out {1} specifications, reviews, mileage, versions, news & images at BikeWale.com", _objData.ModelPageEntity.ModelDetails.MakeBase.MakeName, _objData.ModelPageEntity.ModelDetails.ModelName, Bikewale.Utility.Format.FormatNumeric(_objData.BikePrice.ToString()), _objData.ModelPageEntity.ModelDetails.UsedListingsCnt);
                    }
                    else
                    {
                        _objData.PageMetaTags.Description = String.Format("{0} Price in India - Rs. {1}. Find {2} Images, Mileage, Reviews, Specs, Features and GST On Road Price at Bikewale. {3}", _objData.BikeName, Bikewale.Utility.Format.FormatNumeric(_objData.BikePrice.ToString()), _objData.ModelPageEntity.ModelDetails.ModelName, _colorStr);
                    }

                    _objData.PageMetaTags.Title = String.Format("{0} Price (GST Rates), Images, Colours, Mileage | BikeWale", _objData.BikeName);

                    _objData.PageMetaTags.CanonicalUrl = String.Format("{0}/{1}-bikes/{2}/", BWConfiguration.Instance.BwHostUrl, _objData.ModelPageEntity.ModelDetails.MakeBase.MaskingName, _objData.ModelPageEntity.ModelDetails.MaskingName);

                    _objData.AdTags.TargetedModel = _objData.ModelPageEntity.ModelDetails.ModelName;
                    _objData.PageMetaTags.AlternateUrl = BWConfiguration.Instance.BwHostUrl + "/m/" + _objData.ModelPageEntity.ModelDetails.MakeBase.MaskingName + "-bikes/" + _objData.ModelPageEntity.ModelDetails.MaskingName + "/";
                    _objData.AdTags.TargetedCity = _objData.LocationCookie.City;
                    _objData.PageMetaTags.Keywords = string.Format("{0},{0} Bike, bike, {0} Price, {0} Reviews, {0} Images, {0} Mileage", _objData.BikeName);
                    _objData.PageMetaTags.OGImage = Bikewale.Utility.Image.GetPathToShowImages(_objData.ModelPageEntity.ModelDetails.OriginalImagePath, _objData.ModelPageEntity.ModelDetails.HostUrl, Bikewale.Utility.ImageSize._476x268);
                    _objData.Page_H1 = _objData.BikeName;

                    BindDescription();

                    CheckCustomPageMetas();
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, string.Format("Bikewale.Models.BikeModels.ModelPage --> CreateMetas() ModelId: {0}, MaskingName: {1}", _modelId, ""));
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 13th Aug 2017
        /// Description : Function to check and set custom page metas and summary for the page
        /// </summary>
        /// <param name="objData"></param>
        /// <param name="objMakeBase"></param>
        private void CheckCustomPageMetas()
        {
            try
            {
                if (_objData.IsModelDetails && _objData.ModelPageEntity.ModelDetails.Metas != null)
                {
                    var metas = _objData.ModelPageEntity.ModelDetails.Metas.FirstOrDefault(m => m.PageId == (int)(IsMobile ? BikewalePages.Mobile_ModelPage : BikewalePages.Desktop_ModelPage));

                    if (metas != null)
                    {
                        if (!string.IsNullOrEmpty(metas.Title))
                        {
                            _objData.PageMetaTags.Title = metas.Title;
                        }
                        if (!string.IsNullOrEmpty(metas.Description))
                        {
                            _objData.PageMetaTags.Description = metas.Description;
                        }
                        if (!string.IsNullOrEmpty(metas.Keywords))
                        {
                            _objData.PageMetaTags.Keywords = metas.Keywords;
                        }
                        if (!string.IsNullOrEmpty(metas.Heading))
                        {
                            _objData.Page_H1 = metas.Heading;
                        }
                        if (!string.IsNullOrEmpty(metas.Summary))
                        {
                            _objData.ModelSummary = metas.Summary;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("Bikewale.Models.BikeModels.ModelPage.CheckCustomPageMetas() modelId:{0}", _modelId));
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
                        if (_pqOnRoad != null)
                        {
                            ///Dealer Pricing
                            if (_pqOnRoad.IsDealerPriceAvailable && _pqOnRoad.DPQOutput != null && _pqOnRoad.DPQOutput.Varients != null && _pqOnRoad.DPQOutput.Varients.Count() > 0)
                            {
                                foreach (var version in modelPg.ModelVersions)
                                {
                                    var selectVersion = _pqOnRoad.DPQOutput.Varients.Where(m => m.objVersion.VersionId == version.VersionId).FirstOrDefault();
                                    if (selectVersion != null)
                                    {
                                        version.Price = selectVersion.OnRoadPrice; break;
                                    }

                                }

                                ///Choose the min price version of dealer
                                if (_objData.VersionId == 0)
                                {
                                    _objData.VersionId = (uint)_pqOnRoad.DPQOutput.Varients.OrderBy(m => m.OnRoadPrice).FirstOrDefault().objVersion.VersionId;
                                }
                            }//Bikewale Pricing
                            else if (_pqOnRoad.BPQOutput != null && _pqOnRoad.BPQOutput.Varients != null)
                            {
                                foreach (var version in modelPg.ModelVersions)
                                {
                                    var selected = _pqOnRoad.BPQOutput.Varients.Where(p => p.VersionId == version.VersionId).FirstOrDefault();
                                    if (selected != null)
                                    {
                                        version.Price = !_objData.ShowOnRoadButton ? selected.OnRoadPrice : selected.Price;
                                        break;
                                    }
                                }
                                ///Choose the min price version of city level pricing
                                if (_objData.VersionId == 0)
                                {
                                    if (_pqOnRoad.BPQOutput.Varients.Count() > 0)
                                        _objData.VersionId = (uint)_pqOnRoad.BPQOutput.Varients.OrderBy(m => m.OnRoadPrice).FirstOrDefault().VersionId;
                                }
                            }//Version Pricing
                            else
                            {
                                ///Choose the min price version
                                if (_objData.VersionId == 0)
                                {
                                    var nonZeroVersion = modelPg.ModelVersions.Where(m => m.Price > 0);
                                    if (nonZeroVersion != null && nonZeroVersion.Count() > 0)
                                    {
                                        _objData.SelectedVersion = nonZeroVersion.OrderBy(x => x.Price).FirstOrDefault();
                                        _objData.VersionId = (uint)_objData.SelectedVersion.VersionId;
                                        _objData.BikePrice = _objData.CityId == 0 ? (uint)_objData.SelectedVersion.Price : 0;
                                        _objData.IsGstPrice = _objData.SelectedVersion.IsGstPrice;
                                    }
                                    else
                                    {
                                        _objData.VersionId = (uint)modelPg.ModelVersions.FirstOrDefault().VersionId;
                                        _objData.BikePrice = _objData.CityId == 0 ? (uint)modelPg.ModelVersions.FirstOrDefault().Price : 0;
                                        _objData.IsGstPrice = modelPg.ModelVersions.FirstOrDefault().IsGstPrice;
                                    }
                                }
                            }
                        }
                        else
                        {
                            ///Choose the min price version
                            if (_objData.VersionId == 0)
                            {
                                if (_objData.IsDiscontinuedBike)
                                {
                                    var nonZeroVersion = modelPg.ModelVersions.Where(m => m.Price > 0);
                                    if (nonZeroVersion != null && nonZeroVersion.Count() > 0)
                                    {
                                        _objData.SelectedVersion = nonZeroVersion.OrderBy(x => x.Price).FirstOrDefault();
                                        _objData.VersionId = (uint)_objData.SelectedVersion.VersionId;
                                        _objData.BikePrice = (uint)_objData.SelectedVersion.Price;
                                    }
                                    else
                                    {
                                        _objData.VersionId = (uint)modelPg.ModelVersions.FirstOrDefault().VersionId;
                                        _objData.BikePrice = Convert.ToUInt32(_objData.SelectedVersion != null ? _objData.SelectedVersion.Price : modelPg.ModelVersions.FirstOrDefault().Price);
                                    }
                                }
                                else
                                {
                                    var nonZeroVersion = modelPg.ModelVersions.Where(m => m.Price > 0);
                                    if (nonZeroVersion != null && nonZeroVersion.Count() > 0)
                                    {
                                        _objData.SelectedVersion = nonZeroVersion.OrderBy(x => x.Price).FirstOrDefault();
                                        _objData.VersionId = (uint)_objData.SelectedVersion.VersionId;
                                        _objData.BikePrice = _objData.ShowOnRoadButton ? (uint)_objData.SelectedVersion.Price : (_objData.CityId == 0 ? (uint)_objData.SelectedVersion.Price : 0);
                                        _objData.IsGstPrice = _objData.SelectedVersion.IsGstPrice;
                                    }
                                    else
                                    {
                                        _objData.VersionId = (uint)modelPg.ModelVersions.FirstOrDefault().VersionId;
                                        _objData.BikePrice = _objData.ShowOnRoadButton ? (uint)_objData.SelectedVersion.Price : (_objData.CityId == 0 ? (uint)_objData.SelectedVersion.Price : 0);
                                        _objData.IsGstPrice = modelPg.ModelVersions.FirstOrDefault().IsGstPrice;

                                    }
                                }
                            }
                            else
                            {

                            }
                            if (_objData.CityId != 0 && !_objData.IsDiscontinuedBike && !_objData.HasCityPricing)
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
                                _objData.VersionName = firstVer.VersionName;
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

                    modelPg = _objModel.GetModelPageDetails(Convert.ToInt16(modelID), (int)_objData.VersionId);

                    if (modelPg != null)
                    {
                        if (modelPg.ModelVersions != null)
                        {
                            if (_objData.VersionId > 0)
                                _objData.SelectedVersion = modelPg.ModelVersions.FirstOrDefault(v => v.VersionId == _objData.VersionId);
                            _objData.IsGstPrice = modelPg.ModelDetails != null ? modelPg.ModelDetails.IsGstPrice : false;
                        }

                        if (_objData.VersionId > 0 && _objData.SelectedVersion != null)
                        {
                            _objData.BikePrice = _objData.CityId == 0 ? Convert.ToUInt32(_objData.SelectedVersion.Price) : (_objData.HasCityPricing ? Convert.ToUInt32(_objData.SelectedVersion.Price) : 0);
                            _objData.IsGstPrice = modelPg.ModelDetails != null ? modelPg.ModelDetails.IsGstPrice : false;
                        }
                        else if (modelPg.ModelDetails != null)
                        {
                            _objData.BikePrice = _objData.CityId == 0 ? Convert.ToUInt32(modelPg.ModelDetails.MinPrice) : (_objData.HasCityPricing ? Convert.ToUInt32(modelPg.ModelDetails.MinPrice) : 0);
                            _objData.IsGstPrice = modelPg.ModelDetails != null ? modelPg.ModelDetails.IsGstPrice : false;
                        }

                        // for new bike
                        if (!modelPg.ModelDetails.Futuristic && modelPg.ModelVersionSpecs != null && _objData.SelectedVersion != null)
                        {
                            // Check it versionId passed through url exists in current model's versions
                            _objData.VersionId = (uint)_objData.SelectedVersion.VersionId;

                        }

                        //for all bikes including upcoming bikes as details are mandatory
                        if (modelPg.ModelDetails != null && modelPg.ModelDetails.ModelName != null && modelPg.ModelDetails.MakeBase != null)
                        {
                            _objData.BikeName = string.Format("{0} {1}", modelPg.ModelDetails.MakeBase.MakeName, modelPg.ModelDetails.ModelName);
                        }

                        // Discontinued bikes
                        if (modelPg.ModelDetails != null && !modelPg.ModelDetails.New && modelPg.ModelVersions != null && modelPg.ModelVersions.Count > 1 && _objData.SelectedVersion != null)
                        {
                            _objData.BikePrice = (uint)_objData.SelectedVersion.Price;
                            _objData.IsGstPrice = modelPg.ModelVersions.FirstOrDefault().IsGstPrice;
                        }

                        if (modelPg.ModelDetails != null && modelPg.ModelDetails.PhotosCount > 0 && modelPg.ModelColors != null && modelPg.ModelColors.Count() > 0)
                        {
                            var colorImages = modelPg.ModelColors.Where(x => x.ColorImageId > 0);
                            if (colorImages != null && colorImages.Count() > 0)
                            {
                                string returnUrl;

                                if (IsMobile)
                                    returnUrl = string.Format("/m/{0}-bikes/{1}/", modelPg.ModelDetails.MakeBase.MaskingName, modelPg.ModelDetails.MaskingName);
                                else
                                    returnUrl = string.Format("/{0}-bikes/{1}/", modelPg.ModelDetails.MakeBase.MaskingName, modelPg.ModelDetails.MaskingName);

                                _objData.ColourImageUrl = string.Format("/{0}-bikes/{1}/images/?q={2}", modelPg.ModelDetails.MakeBase.MaskingName, modelPg.ModelDetails.MaskingName, EncodingDecodingHelper.EncodeTo64(string.Format("colorImageId={0}&retUrl={1}", colorImages.FirstOrDefault().ColorImageId, returnUrl)));

                                _objData.ModelColorPhotosCount = colorImages.Count();
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
                if (_cityId > 0 && _objData.City != null && ((_objData.City.HasAreas && _areaId > 0) || !_objData.City.HasAreas))
                {
                    _pqOnRoad = GetOnRoadPrice();
                    // Set Pricequote Cookie
                    if (_pqOnRoad != null)
                    {

                        if (_pqOnRoad.PriceQuote != null)
                        {
                            _objData.DealerId = _pqOnRoad.PriceQuote.DealerId;
                            //objData.VersionId = pqOnRoad.PriceQuote.DefaultVersionId > 0 ? pqOnRoad.PriceQuote.DefaultVersionId : pqOnRoad.PriceQuote.VersionId;

                        }
                        _objData.MPQString = EncodingDecodingHelper.EncodeTo64(PriceQuoteQueryString.FormQueryString(_cityId.ToString(), _pqOnRoad.PriceQuote.PQId.ToString(), _areaId.ToString(), _objData.VersionId.ToString(), _objData.DealerId.ToString()));

                        if (_pqOnRoad.IsDealerPriceAvailable && _pqOnRoad.DPQOutput != null && _pqOnRoad.DPQOutput.Varients != null && _pqOnRoad.DPQOutput.Varients.Count() > 0)
                        {
                            #region when dealer Price is Available

                            var selectedVariant = _pqOnRoad.DPQOutput.Varients.Where(p => p.objVersion.VersionId == _objData.VersionId).FirstOrDefault();
                            if (selectedVariant != null)
                            {
                                _objData.BikePrice = selectedVariant.OnRoadPrice;
                                uint totalDiscountedPrice = 0;
                                if (selectedVariant.PriceList != null)
                                {
                                    totalDiscountedPrice = CommonModel.GetTotalDiscount(_pqOnRoad.discountedPriceList);
                                    _objData.IsGstPrice = _pqOnRoad.DPQOutput.PriceList.FirstOrDefault().IsGstPrice;
                                }

                                if (_pqOnRoad.discountedPriceList != null && _pqOnRoad.discountedPriceList.Count > 0)
                                {
                                    _objData.BikePrice = (_objData.BikePrice - totalDiscountedPrice);

                                }

                            }
                            else // Show dealer properties and Bikewale priceQuote when dealer has pricing for any of the bike
                            // Added on 13 Feb 2017 Pivotal Id:138698777
                            {
                                SetBikeWalePQ(_pqOnRoad);
                            }
                            #endregion when dealer Price is Available
                        }
                        else
                        {
                            SetBikeWalePQ(_pqOnRoad);
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
                if (_pqOnRoad != null && _pqOnRoad.PriceQuote != null && _pqOnRoad.PriceQuote.IsDealerAvailable && _cityId > 0 && _objData.VersionId > 0)
                {
                    _objData.DetailedDealer = _objDealerDetails.GetDealerQuotationV2(_cityId, _objData.VersionId, _objData.DealerId, _areaId);
                }
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Jun 2017
        /// Description :   Fetches Manufacturer Campaigns
        /// Modified by  :  Sushil Kumar on 11th Aug 2017
        /// Description :   Store dealerid for manufacturer campaigns for impressions tracking
        /// </summary>
        private void GetManufacturerCampaign()
        {
            try
            {
                if (_objManufacturerCampaign != null && !(_objData.IsDealerDetailsExists))
                {
                    ManufacturerCampaignEntity campaigns = _objManufacturerCampaign.GetCampaigns(_modelId, _cityId, ManufacturerCampaignPageId);
                    if (campaigns.LeadCampaign != null)
                    {
                        _objData.LeadCampaign = new ManufactureCampaignLeadEntity()
                        {
                            Area = GlobalCityArea.GetGlobalCityArea().Area,
                            CampaignId = campaigns.LeadCampaign.CampaignId,
                            DealerId = campaigns.LeadCampaign.DealerId,
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
                            MakeName = _objData.ModelPageEntity.ModelDetails.MakeBase.MakeName,
                            Organization = campaigns.LeadCampaign.Organization,
                            MaskingNumber = campaigns.LeadCampaign.MaskingNumber,
                            PincodeRequired = campaigns.LeadCampaign.PincodeRequired,
                            PopupDescription = campaigns.LeadCampaign.PopupDescription,
                            PopupHeading = campaigns.LeadCampaign.PopupHeading,
                            PopupSuccessMessage = campaigns.LeadCampaign.PopupSuccessMessage,
                            ShowOnExshowroom = campaigns.LeadCampaign.ShowOnExshowroom
                        };

                        _objData.IsManufacturerTopLeadAdShown = !_objData.ShowOnRoadButton;
                        _objData.IsManufacturerLeadAdShown = (_objData.LeadCampaign.ShowOnExshowroom || (_objData.IsLocationSelected && !_objData.LeadCampaign.ShowOnExshowroom));

                        _objManufacturerCampaign.SaveManufacturerIdInPricequotes(_objData.PQId, campaigns.LeadCampaign.DealerId);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("ModelPage.GetManufacturerCampaign({0},{1},{2})", _modelId, _cityId, ManufacturerCampaignPageId));
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
                _objData.CampaignId = pqOnRoad.BPQOutput.CampaignId;
            }

            if (pqOnRoad.BPQOutput != null && pqOnRoad.BPQOutput.Varients != null && _objData.VersionId > 0)
            {
                var objSelectedVariant = pqOnRoad.BPQOutput.Varients.Where(p => p.VersionId == _objData.VersionId).FirstOrDefault();
                if (objSelectedVariant != null)
                    _objData.BikePrice = _objData.IsLocationSelected && !_objData.ShowOnRoadButton ? Convert.ToUInt32(objSelectedVariant.OnRoadPrice) : Convert.ToUInt32(objSelectedVariant.Price);

                _objData.IsBPQAvailable = true;
                _objData.IsGstPrice = pqOnRoad.BPQOutput.IsGstPrice;
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
                objPQEntity.SourceId = Convert.ToUInt16(Source);
                objPQEntity.ModelId = _modelId;
                objPQEntity.VersionId = _objData.VersionId;
                objPQEntity.PQLeadId = Convert.ToUInt16(PQSource);
                objPQEntity.UTMA = HttpContext.Current.Request.Cookies["__utma"] != null ? HttpContext.Current.Request.Cookies["__utma"].Value : "";
                objPQEntity.UTMZ = HttpContext.Current.Request.Cookies["_bwutmz"] != null ? HttpContext.Current.Request.Cookies["_bwutmz"].Value : "";
                objPQEntity.DeviceId = HttpContext.Current.Request.Cookies["BWC"] != null ? HttpContext.Current.Request.Cookies["BWC"].Value : "";
                PQOutputEntity objPQOutput = _objDealerPQ.ProcessPQV2(objPQEntity);

                if (objPQOutput != null)
                {
                    if (_objData.VersionId == 0)
                        _objData.VersionId = objPQOutput.VersionId;
                    _pqOnRoad = new PQOnRoadPrice();
                    _pqOnRoad.PriceQuote = objPQOutput;
                    if (objPQOutput != null && objPQOutput.PQId > 0)
                    {
                        _objData.PQId = (uint)objPQOutput.PQId;
                        //bpqOutput = _objPQ.GetPriceQuoteById(objPQOutput.PQId, LeadSource);
                        bpqOutput = new BikeQuotationEntity();
                        bpqOutput.Varients = _objPQCache.GetOtherVersionsPrices(_modelId, _cityId);
                        if (bpqOutput != null)
                        {
                            _pqOnRoad.BPQOutput = bpqOutput;
                        }
                        if (objPQOutput.DealerId != 0)
                        {
                            _objData.ShowOnRoadButton = false;
                            _objData.IsAreaSelected = true;
                            PQ_QuotationEntity oblDealerPQ = null;
                            AutoBizCommon dealerPq = new AutoBizCommon();
                            try
                            {
                                oblDealerPQ = dealerPq.GetDealePQEntity(_cityId, objPQOutput.DealerId, _objData.VersionId);
                                if (oblDealerPQ != null)
                                {
                                    uint insuranceAmount = 0;
                                    foreach (var price in oblDealerPQ.PriceList)
                                    {
                                        _pqOnRoad.IsInsuranceFree = Bikewale.Utility.DealerOfferHelper.HasFreeInsurance(objPQOutput.DealerId.ToString(), string.Empty, price.CategoryName, price.Price, ref insuranceAmount);
                                    }
                                    _pqOnRoad.IsInsuranceFree = true;
                                    _pqOnRoad.DPQOutput = oblDealerPQ;
                                    if (_pqOnRoad.DPQOutput.objOffers != null && _pqOnRoad.DPQOutput.objOffers.Count > 0)
                                        _pqOnRoad.DPQOutput.discountedPriceList = OfferHelper.ReturnDiscountPriceList(_pqOnRoad.DPQOutput.objOffers, _pqOnRoad.DPQOutput.PriceList);
                                    _pqOnRoad.InsuranceAmount = insuranceAmount;
                                    if (oblDealerPQ.discountedPriceList != null && oblDealerPQ.discountedPriceList.Count > 0)
                                    {
                                        _pqOnRoad.IsDiscount = true;
                                        _pqOnRoad.discountedPriceList = oblDealerPQ.discountedPriceList;
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
            return _pqOnRoad;
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
                _objData.LocationCookie = GlobalCityArea.GetGlobalCityArea();
                _cityId = _objData.LocationCookie.CityId;
                _areaId = _objData.LocationCookie.AreaId;
                if (_objData.LocationCookie.CityId > 0)
                {
                    var cities = _objCityCache.GetPriceQuoteCities(_modelId);

                    if (cities != null)
                    {
                        var selectedCity = cities.FirstOrDefault(m => m.CityId == _cityId);

                        if (selectedCity != null)
                        {
                            _objData.HasCityPricing = true;
                            _objData.City = selectedCity;
                            _objData.ShowOnRoadButton = selectedCity.HasAreas && _areaId <= 0;
                            _objData.IsAreaSelected = selectedCity.HasAreas && _areaId > 0;
                            if (!_objData.IsAreaSelected) _areaId = 0;
                        }
                    }
                }
                else
                {
                    _objData.ShowOnRoadButton = true;
                }
            }
        }

        private void BindColorString()
        {
            try
            {
                if (_objData.ModelPageEntity != null && _objData.ModelPageEntity.ModelColors != null && _objData.ModelPageEntity.ModelColors.Count() > 0)
                {
                    int colorCount = _objData.ModelPageEntity.ModelColors.Count();
                    string lastColor = _objData.ModelPageEntity.ModelColors.Last().ColorName;
                    if (colorCount > 1)
                    {
                        _colorStr.AppendFormat("{0} is available in {1} different colours : ", _objData.BikeName, colorCount);
                        var colorArr = _objData.ModelPageEntity.ModelColors.Select(x => x.ColorName).Take(colorCount - 1);
                        // Comma separated colors (except last one)
                        _colorStr.Append(string.Join(", ", colorArr));
                        // Append last color with And
                        _colorStr.AppendFormat(" and {0}.", lastColor);
                    }
                    else if (colorCount == 1)
                    {
                        _colorStr.AppendFormat("{0} is available in {1} colour.", _objData.BikeName, lastColor);
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