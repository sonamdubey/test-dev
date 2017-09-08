﻿using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pager;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Models.BestBikes;
using Bikewale.Models.Scooters;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 21 Mar 2017
    /// Summary : Model for the expert reviews landing page
    /// </summary>
    public class ExpertReviewsIndexPage
    {
        #region Variables for dependency injection
        private readonly ICMSCacheContent _cmsCache = null;
        private readonly IPager _pager;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IBikeMakesCacheRepository<int> _objMakeCache = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objBikeVersionsCache = null;
        #endregion

        #region Page level variables
        private const int pageSize = 10, pagerSlotSize = 5;
        private int curPageNo = 1;
        private uint _totalPagesCount;
        string make = string.Empty, model = string.Empty;
        private uint MakeId, ModelId, CityId, pageCatId = 0;
        public string redirectUrl;
        public StatusCodes status;
        private MakeHelper makeHelper = null;
        private ModelHelper modelHelper = null;
        private GlobalCityAreaEntity currentCityArea;
        private BikeModelEntity objModel = null;
        private BikeMakeEntityBase objMake = null;
        private EnumBikeType bikeType = EnumBikeType.All;
        private bool showCheckOnRoadCTA = false;
        private PQSourceEnum pqSource = 0;
        #endregion

        #region Public properties
        public bool IsMobile { get; set; }
        #endregion

        #region Constructor
        public ExpertReviewsIndexPage(ICMSCacheContent cmsCache, IPager pager, IBikeModelsCacheRepository<int> models, IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming, IBikeMakesCacheRepository<int> objMakeCache, IBikeVersionCacheRepository<BikeVersionEntity, uint> objBikeVersionsCache)
        {
            _cmsCache = cmsCache;
            _pager = pager;
            _models = models;
            _bikeModels = bikeModels;
            _upcoming = upcoming;
            _objMakeCache = objMakeCache;
            _objBikeVersionsCache = objBikeVersionsCache;
            ProcessQueryString();
        }
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Aditi Srivastava on 30 Mar 2017
        /// Summary    : Function to get the expert reviews landing page data
        /// </summary>
        public ExpertReviewsIndexPageVM GetData(int widgetTopCount)
        {
            ExpertReviewsIndexPageVM objData = new ExpertReviewsIndexPageVM();

            try
            {
                if (objMake != null)
                    objData.Make = objMake;
                if (objModel != null)
                    objData.Model = objModel;

                int _startIndex = 0, _endIndex = 0;
                _pager.GetStartEndIndex(pageSize, curPageNo, out _startIndex, out _endIndex);

                objData.StartIndex = _startIndex;
                objData.EndIndex = _endIndex;

                objData.Articles = _cmsCache.GetArticlesByCategoryList(Convert.ToString((int)EnumCMSContentType.RoadTest), _startIndex, _endIndex, (int)MakeId, (int)ModelId);

                _totalPagesCount = (uint)_pager.GetTotalPages((int)objData.Articles.RecordCount, pageSize);

                if (objData.Articles != null && objData.Articles.RecordCount > 0)
                {
                    status = StatusCodes.ContentFound;
                    objData.StartIndex = _startIndex;
                    objData.EndIndex = _endIndex > objData.Articles.RecordCount ? Convert.ToInt32(objData.Articles.RecordCount) : _endIndex;
                    BindLinkPager(objData);
                    SetPageMetas(objData);
                    CreatePrevNextUrl(objData);
                    GetWidgetData(objData, widgetTopCount);
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.Models.ExpertReviewsIndexPage.GetData");
            }

            return objData;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 30 Mar 2017
        /// Summary    : Process query string for expert reviews page
        /// </summary>
        private void ProcessQueryString()
        {
            var request = HttpContext.Current.Request;
            var queryString = request != null ? request.QueryString : null;

            if (queryString != null)
            {

                if (!string.IsNullOrEmpty(queryString["pn"]))
                {
                    string _pageNo = queryString["pn"];
                    if (!string.IsNullOrEmpty(_pageNo))
                    {
                        int.TryParse(_pageNo, out curPageNo);
                    }
                }
                make = queryString["make"];
                model = queryString["model"];

                ProcessMakeMaskingName(request, make);
                ProcessModelMaskingName(request, model);
            }
        }
        /// <summary>
        /// Created by  :  Aditi Srivasava on 30 Mar 2017
        /// Summary     :  Processes model masking name
        /// </summary>
        private void ProcessModelMaskingName(HttpRequest request, string model)
        {
            ModelMaskingResponse modelResponse = null;
            if (!string.IsNullOrEmpty(model))
            {
                modelResponse = new ModelMaskingResponse();
                modelHelper = new ModelHelper();
                modelResponse = modelHelper.GetModelDataByMasking(model);
            }
            if (modelResponse != null)
            {
                if (modelResponse.StatusCode == 200)
                {
                    ModelId = modelResponse.ModelId;
                    objModel = modelHelper.GetModelDataById(ModelId);
                }
                else if (modelResponse.StatusCode == 301)
                {
                    status = StatusCodes.RedirectPermanent;
                    redirectUrl = request.RawUrl.Replace(model, modelResponse.MaskingName);
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
        }

        /// <summary>
        /// Created by  :  Aditi Srivasava on 30 Mar 2017
        /// Summary     :  Processes Make masking name
        /// </summary>
        private void ProcessMakeMaskingName(HttpRequest request, string make)
        {
            MakeMaskingResponse makeResponse = null;
            if (!string.IsNullOrEmpty(make))
            {
                makeResponse = new MakeMaskingResponse();
                makeHelper = new MakeHelper();
                makeResponse = makeHelper.GetMakeByMaskingName(make);
            }
            if (makeResponse != null)
            {
                if (makeResponse.StatusCode == 200)
                {
                    MakeId = makeResponse.MakeId;
                    objMake = makeHelper.GetMakeNameByMakeId(MakeId);
                }
                else if (makeResponse.StatusCode == 301)
                {
                    status = StatusCodes.RedirectPermanent;
                    redirectUrl = request.RawUrl.Replace(make, makeResponse.MaskingName);
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
        }

        /// <summary>
        /// Created by  :  Aditi Srivasava on 28 Mar 2017
        /// Summary     :  Set page metas and headings
        /// Modified by :- Subodh Jain 19 june 2017
        /// Summary :- Added TargetModels and Target Make
        /// </summary>
        private void SetPageMetas(ExpertReviewsIndexPageVM objData)
        {
            objData.PageMetaTags.CanonicalUrl = string.Format("{0}{1}{2}", BWConfiguration.Instance.BwHostUrlForJs, UrlFormatter.FormatExpertReviewUrl(make, model), (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
            objData.PageMetaTags.AlternateUrl = string.Format("{0}/m{1}{2}", BWConfiguration.Instance.BwHostUrlForJs, UrlFormatter.FormatExpertReviewUrl(make, model), (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
            if (ModelId > 0)
            {
                objData.PageMetaTags.Title = string.Format("{0} {1} Expert Reviews India - Bike Comparison & Road Tests - BikeWale", objMake.MakeName, objModel.ModelName);
                objData.PageMetaTags.Description = string.Format("Latest expert reviews on {0} {1} in India. Read {0} {1} comparison tests and road tests exclusively on BikeWale", objMake.MakeName, objModel.ModelName);
                objData.PageMetaTags.Keywords = string.Format("{0} {1} expert reviews, {0} {1} road tests, {0} {1} comparison tests, {0} {1} reviews, {0}{1} bike comparison", objMake.MakeName, objModel.ModelName);
                objData.PageH1 = string.Format("{0} {1} Expert Reviews", objMake.MakeName, objModel.ModelName);
                objData.AdTags.TargetedModel = objModel.ModelName;
                objData.AdTags.TargetedMakes = objMake.MakeName;
            }
            else if (MakeId > 0)
            {
                objData.PageMetaTags.Title = string.Format("{0} Bikes Expert Reviews India - Bike Comparison & Road Tests - BikeWale", objMake.MakeName);
                objData.PageMetaTags.Description = string.Format("Latest expert reviews on upcoming and new {0} bikes in India. Read {0} bike comparison tests and road tests exclusively on BikeWale", objMake.MakeName);
                objData.PageMetaTags.Keywords = string.Format("{0} bike expert reviews, {0} bike road tests, {0} bike comparison tests, {0} bike reviews, {0} road tests, {0} expert reviews, {0} bike comparison, {0} comparison tests.", objMake.MakeName);
                objData.PageH1 = string.Format("{0} Bikes Expert Reviews", objMake.MakeName);
                objData.AdTags.TargetedMakes = objMake.MakeName;
            }
            else
            {
                objData.PageMetaTags.Title = "Expert Bike Reviews India - Bike Comparison & Road Tests - BikeWale";
                objData.PageMetaTags.Description = "Latest expert reviews on upcoming and new bikes in India. Read bike comparison tests and road tests exclusively on BikeWale";
                objData.PageMetaTags.Keywords = "Expert bike reviews, bike road tests, bike comparison tests, bike reviews, road tests, expert reviews, bike comparison, comparison tests";
                objData.PageH1 = string.Format("Expert Reviews");
            }

            if (curPageNo > 1)
            {
                objData.PageMetaTags.Description = string.Format("{0} {1}", curPageNo + " of " + _totalPagesCount + " -", objData.PageMetaTags.Description);
                objData.PageMetaTags.Title = string.Format("{0} {1}", curPageNo + " of " + _totalPagesCount + " -", objData.PageMetaTags.Title);
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 30 Mar 2017
        /// Summary    : Get view model for page widgets
        /// Modified by Sajal Gupta on 24-08-2017
        /// description : Added Popular Scooter Brands widget
        /// </summary>
        private void GetWidgetData(ExpertReviewsIndexPageVM objData, int topCount)
        {
            try
            {
                currentCityArea = GlobalCityArea.GetGlobalCityArea();
                if (currentCityArea != null)
                    CityId = currentCityArea.CityId;

                EnumBikeBodyStyles bodyStyle = EnumBikeBodyStyles.AllBikes;

                if (ModelId > 0)
                {
                    List<BikeVersionMinSpecs> objVersionsList = _objBikeVersionsCache.GetVersionMinSpecs(ModelId, false);

                    if (objVersionsList != null && objVersionsList.Count > 0)
                        bodyStyle = objVersionsList.FirstOrDefault().BodyStyle;

                    if (bodyStyle.Equals(EnumBikeBodyStyles.Scooter))
                    {
                        PopularScooterBrandsWidget objPopularScooterBrands = new PopularScooterBrandsWidget(_objMakeCache);
                        objPopularScooterBrands.TopCount = 4;
                        objData.PopularScooterMakesWidget = objPopularScooterBrands.GetData();
                        bikeType = EnumBikeType.Scooters;
                    }
                    else
                    {
                        PopularBikesByBodyStyle objPopularStyle = new PopularBikesByBodyStyle(_models);
                        objPopularStyle.ModelId = ModelId;
                        objPopularStyle.CityId = CityId;
                        objPopularStyle.TopCount = topCount;
                        objData.PopularBodyStyle = objPopularStyle.GetData();
                        if (objData.PopularBodyStyle != null)
                        {
                            objData.PopularBodyStyle.WidgetHeading = string.Format("Popular {0}", objData.PopularBodyStyle.BodyStyleText);
                            objData.PopularBodyStyle.WidgetLinkTitle = string.Format("Best {0} in India", objData.PopularBodyStyle.BodyStyleLinkTitle);
                            objData.PopularBodyStyle.WidgetHref = UrlFormatter.FormatGenericPageUrl(objData.PopularBodyStyle.BodyStyle);
                        }
                    }
                }
                else
                {
                    UpcomingBikesWidget objUpcomingBikes = new UpcomingBikesWidget(_upcoming);
                    objUpcomingBikes.Filters = new UpcomingBikesListInputEntity();
                    objUpcomingBikes.Filters.PageNo = 1;
                    objUpcomingBikes.Filters.PageSize = topCount;
                    if (MakeId > 0)
                    {
                        objUpcomingBikes.Filters.MakeId = (int)MakeId;
                    }
                    objUpcomingBikes.SortBy = EnumUpcomingBikesFilter.Default;
                    objData.UpcomingBikes = objUpcomingBikes.GetData();

                    if (objMake != null)
                    {
                        objData.UpcomingBikes.WidgetHeading = string.Format("Upcoming {0} bikes", objMake.MakeName);
                        objData.UpcomingBikes.WidgetHref = string.Format("/{0}-bikes/upcoming/", objMake.MaskingName);
                    }
                    else
                    {
                        objData.UpcomingBikes.WidgetHeading = "Upcoming bikes";
                        objData.UpcomingBikes.WidgetHref = "/upcoming-bikes/";
                    }
                    objData.UpcomingBikes.WidgetLinkTitle = "Upcoming Bikes in India";
                }

                MostPopularBikesWidget objPopularBikes = new MostPopularBikesWidget(_bikeModels, bikeType, showCheckOnRoadCTA, false, pqSource, pageCatId, MakeId);
                objPopularBikes.TopCount = topCount;
                objPopularBikes.CityId = CityId;
                objData.MostPopularBikes = objPopularBikes.GetData();

                if (bikeType.Equals(EnumBikeType.Scooters))
                {
                    objData.MostPopularBikes.WidgetHeading = string.Format("Popular {0} scooters", objMake.MakeName);
                    objData.MostPopularBikes.WidgetHref = string.Format("/{0}-{1}/", objMake.MaskingName, objMake.IsScooterOnly ? "bikes" : "scooters");
                    objData.MostPopularBikes.WidgetLinkTitle = "View all scooters";
                    objData.MostPopularBikes.CtaText = "View all scooters";
                }
                else if (MakeId > 0 && objMake != null)
                {
                    objData.MostPopularBikes.WidgetHeading = string.Format("Popular {0} bikes", objMake.MakeName);
                    objData.MostPopularBikes.WidgetHref = string.Format("/{0}-bikes/", objMake.MaskingName);
                    objData.MostPopularBikes.WidgetLinkTitle = string.Format("{0} Bikes", objMake.MakeName);
                    objData.MostPopularBikes.CtaText = "View all bikes";
                }
                else
                {
                    objData.MostPopularBikes.WidgetHeading = "Popular bikes";
                    objData.MostPopularBikes.WidgetHref = "/best-bikes-in-india/";
                    objData.MostPopularBikes.WidgetLinkTitle = "Best Bikes in India";
                    objData.MostPopularBikes.CtaText = "View all bikes";
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.Models.ExpertReviewsIndexPage.GetWidgetData");
            }
        }

        /// <summary>
        /// Created By : Aditi Srivastava on 30 Mar 2017
        /// Summary    : Bind link pager
        /// </summary>
        private void BindLinkPager(ExpertReviewsIndexPageVM objData)
        {
            try
            {
                objData.PagerEntity = new PagerEntity();
                objData.PagerEntity.BaseUrl = string.Format("{0}{1}", (IsMobile ? "/m" : ""), UrlFormatter.FormatExpertReviewUrl(make, model));
                objData.PagerEntity.PageNo = curPageNo;
                objData.PagerEntity.PagerSlotSize = pagerSlotSize;
                objData.PagerEntity.PageUrlType = "page/";
                objData.PagerEntity.TotalResults = (int)objData.Articles.RecordCount;
                objData.PagerEntity.PageSize = pageSize;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.Models.ExpertReviewsIndexPage.BindLinkPager");
            }
        }
        /// <summary>
        /// Created By : Aditi Srivastava on 30 Mar 2017
        /// Summary    : Create previous and next page urls
        /// </summary>
        /// <param name="objData"></param>
        private void CreatePrevNextUrl(ExpertReviewsIndexPageVM objData)
        {
            string _mainUrl = String.Format("{0}{1}page/", BWConfiguration.Instance.BwHostUrl, objData.PagerEntity.BaseUrl);
            string prevPageNumber = string.Empty, nextPageNumber = string.Empty;
            int totalPages = _pager.GetTotalPages((int)objData.Articles.RecordCount, pageSize);
            if (totalPages > 1)
            {
                if (curPageNo == 1)
                {
                    nextPageNumber = "2";
                    objData.PageMetaTags.NextPageUrl = string.Format("{0}{1}/", _mainUrl, nextPageNumber);
                }
                else if (curPageNo == totalPages)
                {
                    prevPageNumber = Convert.ToString(curPageNo - 1);
                    objData.PageMetaTags.PreviousPageUrl = string.Format("{0}{1}/", _mainUrl, prevPageNumber);
                }
                else
                {
                    prevPageNumber = Convert.ToString(curPageNo - 1);
                    objData.PageMetaTags.PreviousPageUrl = string.Format("{0}{1}/", _mainUrl, prevPageNumber);
                    nextPageNumber = Convert.ToString(curPageNo + 1);
                    objData.PageMetaTags.NextPageUrl = string.Format("{0}{1}/", _mainUrl, nextPageNumber);
                }
            }
        }
        #endregion
    }
}