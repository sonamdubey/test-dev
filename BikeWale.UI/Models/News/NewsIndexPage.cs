using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pager;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Models.BestBikes;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Web;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 27 Mar 2017
    /// Summary    : Model to get data for news default page
    /// </summary>
    public class NewsIndexPage
    {
        #region Variables for dependency injection and constructor
        private readonly ICMSCacheContent _articles = null;
        private readonly IPager _pager = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private int _topCount;
        #endregion

        #region Page level variables
        private uint MakeId, ModelId, pageCatId = 0;
        private const int pageSize = 10, pagerSlotSize = 5;
        private int curPageNo = 1;
        private string make = string.Empty, model = string.Empty;
        private MakeHelper makeHelper = null;
        private ModelHelper modelHelper = null;
        private GlobalCityAreaEntity currentCityArea;
        public string redirectUrl;
        public StatusCodes status;
        protected BikeModelEntity objModel = null;
        protected BikeMakeEntityBase objMake = null;
        private IBikeModels<BikeModelEntity, int> _bikeModels;
        private EnumBikeType bikeType = EnumBikeType.All;
        private bool showCheckOnRoadCTA = false;
        private PQSourceEnum pqSource = 0;
        #endregion

        #region Constructor
        public NewsIndexPage(ICMSCacheContent articles, IPager pager, IBikeModelsCacheRepository<int> models, IBikeModels<BikeModelEntity, int> bikeModels, int topCount)
        {
            _articles = articles;
            _pager = pager;
            _models = models;
            _bikeModels = bikeModels;
            _topCount = topCount;
            ProcessQueryString();
        }
        #endregion

        #region Functions

        /// <summary>
        /// Created By : Aditi Srivastava on 27 Mar 2017
        /// Summary    : Get page data
        /// </summary>
        /// <returns></returns>
        public NewsIndexPageVM GetData()
        {
            NewsIndexPageVM objData = new NewsIndexPageVM();

            try
            {
                int _startIndex = 0, _endIndex = 0;
                _pager.GetStartEndIndex(pageSize, curPageNo, out _startIndex, out _endIndex);

                objData.StartIndex = _startIndex;
                objData.EndIndex = _endIndex;

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

                objData.Articles = _articles.GetArticlesByCategoryList(contentTypeList, _startIndex, _endIndex, (int)MakeId, (int)ModelId);

                if (objData.Articles != null && objData.Articles.RecordCount > 0)
                {
                    status = StatusCodes.ContentFound;
                    BindLinkPager(objData);
                    SetPageMetas(objData);
                    GetWidgetData(objData);
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Bikewale.Models.News.NewsIndexPage.GetData");
            }
            return objData;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 27 Mar 2017
        /// Summary    : Process query string for news page
        /// </summary>
        public void ProcessQueryString()
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
        /// Created by  :  Aditi Srivasava on 27 Mar 2017
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
        /// Created by  :  Aditi Srivasava on 27 Mar 2017
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
        /// </summary>
        private void SetPageMetas(NewsIndexPageVM objData)
        {
            objData.PageMetaTags.CanonicalUrl = string.Format("{0}{1}{2}", BWConfiguration.Instance.BwHostUrlForJs, UrlFormatter.FormatNewsUrl(make, model), (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
            objData.PageMetaTags.AlternateUrl = string.Format("{0}/m{1}{2}", BWConfiguration.Instance.BwHostUrlForJs, UrlFormatter.FormatNewsUrl(make, model), (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
            if (ModelId > 0)
            {
                objData.PageMetaTags.Title = string.Format("Latest News about {0} {1} | {0} {1} News - BikeWale", objMake.MakeName, objModel.ModelName);
                objData.PageMetaTags.Description = String.Format("Read the latest news about {0} {1} bikes exclusively on BikeWale. Know more about {1}.", objMake.MakeName, objModel.ModelName);
                objData.PageH1 = string.Format("{0} {1} Bikes News", objMake.MakeName, objModel.ModelName);
                objData.PageH2 = string.Format("Latest {0} {1} Bikes News and Views", objMake.MakeName, objModel.ModelName);
            }
            else if (MakeId > 0)
            {
                objData.PageMetaTags.Title = string.Format("Latest News about {0} Bikes | {0} Bikes News - BikeWale", objMake.MakeName);
                objData.PageMetaTags.Description = String.Format("Read the latest news about popular and upcoming {0} bikes exclusively on BikeWale. Know more about {0} bikes.", objMake.MakeName);
                objData.PageH1 = string.Format("{0} Bikes News", objMake.MakeName);
                objData.PageH2 = string.Format("Latest {0} Bikes News and Views", objMake.MakeName);
            }
            else
            {
                objData.PageMetaTags.Title = "Bike News - Latest Indian Bike News & Views | BikeWale";
                objData.PageMetaTags.Description = "Latest news updates on Indian bikes industry, expert views and interviews exclusively on BikeWale.";
                objData.PageMetaTags.Keywords = "news, bike news, auto news, latest bike news, indian bike news, bike news of india";
                objData.PageH1 = string.Format("Bike News");
                objData.PageH2 = string.Format("Latest Indian Bikes News and Views");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 28 Mar 2017
        /// Summary    : Get view model for page widgets
        /// </summary>
        private void GetWidgetData(NewsIndexPageVM objData)
        {
            try
            {
                currentCityArea = GlobalCityArea.GetGlobalCityArea();
                uint cityId = 0;
                if (currentCityArea != null)
                    cityId = currentCityArea.CityId;

                MostPopularBikesWidget objPopularBikes = new MostPopularBikesWidget(_bikeModels, bikeType, showCheckOnRoadCTA, pqSource, pageCatId, MakeId);
                objPopularBikes.TopCount = _topCount;
                objPopularBikes.CityId = cityId;
                objData.MostPopularBikes = objPopularBikes.GetData();
                if (MakeId > 0 && objMake != null)
                {
                    objData.MostPopularBikes.WidgetHeading = string.Format("Popular {0} bikes", objMake.MakeName);
                    objData.MostPopularBikes.WidgetHref = string.Format("/{0}-bikes/", objMake.MaskingName);
                    objData.MostPopularBikes.WidgetLinkTitle = string.Format("{0} Bikes", objMake.MakeName);
                }
                else
                {
                    objData.MostPopularBikes.WidgetHeading = "Popular bikes";
                    objData.MostPopularBikes.WidgetHref = "/best-bikes-in-india/";
                    objData.MostPopularBikes.WidgetLinkTitle = "Best Bikes in India";
                }

                if (ModelId > 0)
                {
                    PopularBikesByBodyStyle objPopularStyle = new PopularBikesByBodyStyle(_models);
                    objPopularStyle.ModelId = ModelId;
                    objPopularStyle.CityId = cityId;
                    objPopularStyle.TopCount = _topCount;
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
                    UpcomingBikesWidget objUpcomingBikes = new UpcomingBikesWidget(_models);
                    objUpcomingBikes.TopCount = _topCount;
                    if (MakeId > 0)
                    {
                        objUpcomingBikes.MakeId = MakeId;
                    }
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
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Exception : Bikewale.Models.News.NewsIndexPage.GetWidgetData");
            }
        }

        /// <summary>
        /// Created By : Aditi Srivastava on 27 Mar 2017
        /// Summary    : Bind link pager
        /// </summary>
        private void BindLinkPager(NewsIndexPageVM objData)
        {
            try
            {
                objData.PagerEntity = new PagerEntity();
                objData.PagerEntity.BaseUrl = string.Format("{0}{1}", objData.PagerEntity.BaseUrl, UrlFormatter.FormatNewsUrl(make, model));
                objData.PagerEntity.PageNo = curPageNo;
                objData.PagerEntity.PagerSlotSize = pagerSlotSize;
                objData.PagerEntity.PageUrlType = "page/";
                objData.PagerEntity.TotalResults = (int)objData.Articles.RecordCount;
                objData.PagerEntity.PageSize = pageSize;
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "Exception : Bikewale.Models.News.NewsIndexPage.BindLinkPager");
            }
        }
        #endregion

    }
}