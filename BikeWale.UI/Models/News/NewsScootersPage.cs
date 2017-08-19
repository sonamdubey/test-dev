using Bikewale.Common;
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
using Bikewale.Interfaces.PWA.CMS;
using Bikewale.Models.BestBikes;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models.News
{
    /// <summary>
    /// Created by: Snehal Dange on 17th Aug 2017
    /// Summary: Model for News for scooters landing page
    /// </summary>
    public class NewsScootersPage
    {
        #region Variables for dependency injection and constructor
        private readonly ICMSCacheContent _articles = null;
        private readonly IPWACMSCacheRepository _renderedArticles = null;
        private readonly IPager _pager = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        #endregion

        #region Page level variables
        private uint MakeId, ModelId, pageCatId = 0, CityId;
        private const int pageSize = 10, pagerSlotSize = 5;
        private int curPageNo = 1;
        private string make = string.Empty, model = string.Empty;
        private MakeHelper makeHelper = null;
        private ModelHelper modelHelper = null;
        private GlobalCityAreaEntity currentCityArea = null;
        public string redirectUrl;
        public StatusCodes status;
        private BikeModelEntity objModel = null;
        private BikeMakeEntityBase objMake = null;
        private EnumBikeType bikeType = EnumBikeType.Scooters;
        private bool showCheckOnRoadCTA = false;
        private PQSourceEnum pqSource = 0;
        #endregion

        #region Public properties
        public bool IsMobile { get; set; }

        public string CityName
        {
            get
            {
                if (currentCityArea == null)
                {
                    currentCityArea = GlobalCityArea.GetGlobalCityArea();
                    if (currentCityArea != null)
                        CityId = currentCityArea.CityId;
                }

                return string.IsNullOrEmpty(currentCityArea.City) ? string.Empty : currentCityArea.City;
            }
        }

        #endregion

        #region Constructor
        public NewsScootersPage(ICMSCacheContent articles, IPager pager, IBikeModelsCacheRepository<int> models, IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming, IPWACMSCacheRepository renderedArticles)
        {
            _articles = articles;
            _pager = pager;
            _models = models;
            _bikeModels = bikeModels;
            _upcoming = upcoming;
            _renderedArticles = renderedArticles;
            ProcessQueryString();
        }
        #endregion

        /// <summary>
        /// Created by : Snehal Dange on 17th Aug 2017
        /// Summary    : Process query string for  scooters news page
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
        /// Created by  :  Snehal Dange on 17th Aug 2017
        /// Summary     :  Processes model masking name for scooters
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
        /// Created by  : Snehal Dange on 17th Aug 2017
        /// Summary     :  Processes Make masking name for scooters
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
        /// Created By : Snehal Dange on 17 August 2017
        /// Summary    : Bind link pager
        /// </summary>
        private void BindLinkPager(NewsScootersPageVM objData)
        {
            try
            {
                objData.PagerEntity = new PagerEntity();
                objData.PagerEntity.BaseUrl = string.Format("{0}{1}", (IsMobile ? "/m" : ""), UrlFormatter.FormatScootersNewsUrl(make, model));
                objData.PagerEntity.PageNo = curPageNo;
                objData.PagerEntity.PagerSlotSize = pagerSlotSize;
                objData.PagerEntity.PageUrlType = "page/";
                objData.PagerEntity.TotalResults = (int)objData.Articles.RecordCount;
                objData.PagerEntity.PageSize = pageSize;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Exception : Bikewale.Models.News.NewsScootersPage.BindLinkPager");
            }
        }


        /// <summary>
        /// Created by  :  Snehal Dange on 17 August 2017
        /// Summary     :  Set page metas and headings
        /// </summary>
        private void SetPageMetas(NewsScootersPageVM objData)
        {
            objData.PageMetaTags.CanonicalUrl = string.Format("{0}{1}{2}", BWConfiguration.Instance.BwHostUrlForJs, UrlFormatter.FormatNewsUrl(make, model), (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
            objData.PageMetaTags.AlternateUrl = string.Format("{0}/m{1}{2}", BWConfiguration.Instance.BwHostUrlForJs, UrlFormatter.FormatNewsUrl(make, model), (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
            if (ModelId > 0)
            {
                objData.PageMetaTags.Title = string.Format("{0} {1} Scooter News | Latest news about {0} {1} scooters - BikeWale", objMake.MakeName, objModel.ModelName);
                objData.PageMetaTags.Description = String.Format("Read the latest news about scooters. Know more about {0} {1} scooter new launch updates, and much more from two wheeler industry.", objMake.MakeName, objModel.ModelName);
                objData.PageH1 = string.Format("{0} {1} Scooter News", objMake.MakeName, objModel.ModelName);
                objData.PageH2 = string.Format("Latest news and views about {0} {1} scooters", objMake.MakeName, objModel.ModelName);
                objData.AdTags.TargetedMakes = objMake.MakeName;
                objData.AdTags.TargetedModel = objModel.ModelName;
            }
            else if (MakeId > 0)
            {
                objData.PageMetaTags.Title = string.Format("{0} Scooter News | Latest news about {0} scooters - BikeWale", objMake.MakeName);
                objData.PageMetaTags.Description = String.Format("Read the latest news about scooters. Know more about {0} scooter new launch updates, and much more from two wheeler industry.", objMake.MakeName);
                objData.PageMetaTags.Keywords = string.Format("{0} Scooter news, {0} scooter updates,{0} scooty news, {0} scooty updates", objMake.MakeName);
                objData.PageH1 = string.Format("{0} Scooter News", objMake.MakeName);
                objData.PageH2 = string.Format("Latest news and views about {0} scooters", objMake.MakeName);
                objData.AdTags.TargetedMakes = objMake.MakeName;
            }
            else
            {
                objData.PageMetaTags.Title = "Scooter News | Latest news about scooters - BikeWale";
                objData.PageMetaTags.Description = "Read the latest news about scooters. Know more about scooter new launch updates, and much more from two wheeler industry.";
                objData.PageMetaTags.Keywords = "scooter news, scooty news, auto news, scooter launch, Indian scooter news";
                objData.PageH1 = string.Format("Scooter News");
                objData.PageH2 = string.Format(" Latest News and Views about Scooters");
            }
        }


        /// <summary>
        /// Created By : Snehal Dange on 17 August 2017
        /// Summary    : Create previous and next page urls
        /// </summary>
        /// <param name="objData"></param>
        private void CreatePrevNextUrl(NewsScootersPageVM objData)
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


        /// <summary>
        /// Created by : Snehal Dange on 17 August 2017
        /// Summary    : Get view model for page widgets
        /// </summary>
        private void GetWidgetData(NewsScootersPageVM objData, int topCount)
        {
            try
            {
                currentCityArea = GlobalCityArea.GetGlobalCityArea();
                if (currentCityArea != null)
                    CityId = currentCityArea.CityId;

                MostPopularBikesWidget objPopularBikes = new MostPopularBikesWidget(_bikeModels, bikeType, showCheckOnRoadCTA, false, pqSource, pageCatId, MakeId);
                objPopularBikes.TopCount = topCount;
                objPopularBikes.CityId = CityId;
                objData.MostPopularBikes = objPopularBikes.GetData();
                if (MakeId > 0 && objMake != null)
                {
                    objData.MostPopularBikes.WidgetHeading = string.Format("Popular {0} Scooters", objMake.MakeName);
                    objData.MostPopularBikes.WidgetHref = string.Format("/{0}-scooters/", objMake.MaskingName);
                    objData.MostPopularBikes.WidgetLinkTitle = string.Format("{0} Scooters", objMake.MakeName);
                }
                else
                {
                    objData.MostPopularBikes.WidgetHeading = "Popular Scooters";
                    objData.MostPopularBikes.WidgetHref = "/best-scooters-in-india/";
                    objData.MostPopularBikes.WidgetLinkTitle = "Best Scooters in India";
                }

                if (ModelId > 0)
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
                else
                {
                    UpcomingBikesWidget objUpcomingBikes = new UpcomingBikesWidget(_upcoming);
                    objUpcomingBikes.Filters = new UpcomingBikesListInputEntity();
                    objUpcomingBikes.Filters.BodyStyleId = 5;
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
                        objData.UpcomingBikes.WidgetHeading = string.Format("Upcoming {0} Scooters", objMake.MakeName);
                        objData.UpcomingBikes.WidgetHref = string.Format("/{0}-bikes/upcoming/", objMake.MaskingName);
                    }
                    else
                    {
                        objData.UpcomingBikes.WidgetHeading = "Upcoming Scooters";
                        objData.UpcomingBikes.WidgetHref = "/upcoming-scooters/";
                    }
                    objData.UpcomingBikes.WidgetLinkTitle = "Upcoming Scooters in India";
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Exception : Bikewale.Models.News.NewsScootersPage.GetWidgetData");
            }
        }


        /// <summary>
        /// Created By : Snehal Dange on 17 August 2017
        /// Summary    : Get news data for scooters
        /// </summary>
        /// <returns></returns>
        public NewsScootersPageVM GetData(int widgetTopCount)
        {
            NewsScootersPageVM objData = new NewsScootersPageVM();

            try
            {
                int _startIndex = 0, _endIndex = 0;
                _pager.GetStartEndIndex(pageSize, curPageNo, out _startIndex, out _endIndex);

                List<EnumCMSContentType> categorList = new List<EnumCMSContentType>();
                categorList.Add(EnumCMSContentType.News);
                if (MakeId == 0 && ModelId == 0)
                {
                    categorList.Add(EnumCMSContentType.AutoExpo2016);
                    categorList.Add(EnumCMSContentType.Features);
                    categorList.Add(EnumCMSContentType.RoadTest);
                    categorList.Add(EnumCMSContentType.ComparisonTests);
                    categorList.Add(EnumCMSContentType.SpecialFeature);
                    categorList.Add(EnumCMSContentType.TipsAndAdvices);
                }
                string contentTypeList = CommonApiOpn.GetContentTypesString(categorList);

                categorList.Clear();
                categorList = null;
                if (objMake != null)
                    objData.Make = objMake;
                if (objModel != null)
                    objData.Model = objModel;


                objData.Articles = _articles.GetArticlesByCategoryList(contentTypeList, _startIndex, _endIndex, Convert.ToString((int)EnumBikeBodyStyles.Scooter), (int)MakeId, (int)ModelId);

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
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.Models.News.NewsScootersPage.GetData");
            }
            return objData;
        }

    }
}