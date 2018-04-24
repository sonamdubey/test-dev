using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.CMS;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pager;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Pager;
using Bikewale.Models.BestBikes;
using Bikewale.Models.BikeModels;
using Bikewale.Models.Scooters;
using Bikewale.Utility;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Interfaces.EditCMS;
using Bikewale.PWA.Utils;
using Bikewale.Interfaces.PWA.CMS;
using Newtonsoft.Json;
using Bikewale.Models.EditorialPages;
using Bikewale.Entities.EditorialWidgets;

namespace Bikewale.Models
{
    /// <summary>
    /// Created By : Ashish G. Kamble on 21 Mar 2017
    /// Summary : Model for the expert reviews landing page
    /// Modified by: Dhruv Joshi
    /// Dated: 16th April 2018
    /// Description: Added _pageId and _totalTabCount to page variables for generic info widget
    /// </summary>
    public class ExpertReviewsIndexPage : EditorialBasePage
    {
        #region Variables for dependency injection
        private readonly ICMSCacheContent _cmsCache = null;
        private readonly IPager _pager;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IBikeMakesCacheRepository _objMakeCache = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objBikeVersionsCache = null;
        private readonly IBikeSeriesCacheRepository _seriesCache;
        private readonly IBikeSeries _series;
        private EditorialPageType currentPageType = EditorialPageType.Default;
        public EnumBikeBodyStyles BodyStyle = EnumBikeBodyStyles.AllBikes;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IBikeInfo _objGenericBike = null;
        private readonly IArticles _articles;
        private readonly IPWACMSCacheRepository _renderedArticles;

        #endregion

        #region Page level variables
        private string _pageName = "Editorial List";
        private const int pageSize = 10, pagerSlotSize = 5;
        private int curPageNo = 1;
        private uint _totalPagesCount;
        string make = string.Empty, model = string.Empty, series = string.Empty;
        private uint MakeId, ModelId, CityId, SeriesId, pageCatId = 0;
        public string redirectUrl, CityName;
        public StatusCodes status;
        private MakeHelper makeHelper = null;
        private ModelHelper modelHelper = null;
        private GlobalCityAreaEntity currentCityArea;
        private BikeModelEntity objModel = null;
        private BikeMakeEntityBase objMake = null;
        private BikeSeriesEntityBase objSeries;
        private SeriesMaskingResponse objResponse;

        private readonly bool showCheckOnRoadCTA = false;
        private readonly PQSourceEnum pqSource = 0;
        private EnumBikeType bikeType = EnumBikeType.All;
        private string ModelIds = string.Empty;
        private readonly uint _totalTabCount = 3;
        private BikeInfoTabType _pageId = BikeInfoTabType.ExpertReview;
        #endregion

        #region Public properties
        public bool IsMobile { get; set; }

        #endregion

        #region Constructor
        public ExpertReviewsIndexPage(ICMSCacheContent cmsCache, IPager pager, IBikeModelsCacheRepository<int> models,
            IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming, IBikeMakesCacheRepository objMakeCache, IBikeVersionCacheRepository<BikeVersionEntity, uint> objBikeVersionsCache,
            IBikeSeriesCacheRepository seriesCache, IBikeSeries series, ICityCacheRepository objCityCache, IBikeInfo objGenericBike) : base(objMakeCache, models, bikeModels, upcoming, series)
        {
            _cmsCache = cmsCache;
            _pager = pager;
            _models = models;
            _bikeModels = bikeModels;
            _upcoming = upcoming;
            _objMakeCache = objMakeCache;
            _objBikeVersionsCache = objBikeVersionsCache;
            _seriesCache = seriesCache;
            _series = series;
            _objCityCache = objCityCache;
            _objGenericBike = objGenericBike;
            ProcessQueryString();
            ProcessCityArea();
        }

        public ExpertReviewsIndexPage(ICMSCacheContent cmsCache, IPager pager, IBikeModelsCacheRepository<int> models,
            IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming, IBikeMakesCacheRepository objMakeCache, IBikeVersionCacheRepository<BikeVersionEntity, uint> objBikeVersionsCache,
            IBikeSeriesCacheRepository seriesCache, IBikeSeries series, ICityCacheRepository objCityCache, IBikeInfo objGenericBike, IArticles articles, IPWACMSCacheRepository renderedArticles) : base(objMakeCache, models, bikeModels, upcoming, series)
        {
            _cmsCache = cmsCache;
            _pager = pager;
            _models = models;
            _bikeModels = bikeModels;
            _upcoming = upcoming;
            _objMakeCache = objMakeCache;
            _objBikeVersionsCache = objBikeVersionsCache;
            _seriesCache = seriesCache;
            _series = series;
            _objCityCache = objCityCache;
            _objGenericBike = objGenericBike;
            _articles = articles;
            _renderedArticles = renderedArticles;
            ProcessQueryString();
            ProcessCityArea();
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 24 Feb 2018
        /// Description : Method to get global city Id and Name from cookie.
        /// </summary>
        private void ProcessCityArea()
        {
            try
            {
                currentCityArea = GlobalCityArea.GetGlobalCityArea();
                if (currentCityArea != null)
                {
                    CityId = currentCityArea.CityId;
                    CityName = currentCityArea.City;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.News.ExpertReviewsIndexPage.ProcessCityArea");
            }
        }
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Aditi Srivastava on 30 Mar 2017
        /// Summary    : Function to get the expert reviews landing page data
        /// Modified by : snehal Dange on 28th Nov 2017
        /// Descritpion : Added ga for page
        /// Modified by : Ashutosh Sharma on 26 Feb 2018
        /// Description : Added recordCount in call to method 'BindLinkPager'.
        /// Modified By : Deepak Israni on 24 April 2018
        /// Description : Set page widgets through new method.
        /// </summary>
        public ExpertReviewsIndexPageVM GetData(int widgetTopCount)
        {
            ExpertReviewsIndexPageVM objData = new ExpertReviewsIndexPageVM();
            objData.BodyStyle = this.BodyStyle;
            objData.EditorialPageType = this.currentPageType;

            try
            {
                if (objMake != null)
                {
                    objData.Make = objMake;
                }

                if (objModel != null)
                {
                    objData.Model = objModel;
                }

                if (objSeries != null)
                {
                    objData.Series = objSeries;
                }

                int _startIndex = 0, _endIndex = 0;
                _pager.GetStartEndIndex(pageSize, curPageNo, out _startIndex, out _endIndex);

                objData.StartIndex = _startIndex;
                objData.EndIndex = _endIndex;

                // Added by Vivek Singh Tomar to get list of model ids for given series
                if (objData.Series != null)
                {
                    ModelIds = _series.GetModelIdsBySeries(objData.Series.SeriesId);
                }
                else
                {
                    ModelIds = Convert.ToString(ModelId);
                }

                objData.Articles = _cmsCache.GetArticlesByCategoryList(Convert.ToString((int)EnumCMSContentType.RoadTest), _startIndex, _endIndex, (int)MakeId, ModelIds);



                if (objData.Articles != null && objData.Articles.RecordCount > 0)
                {
                    _totalPagesCount = (uint)_pager.GetTotalPages((int)objData.Articles.RecordCount, pageSize);
                    status = StatusCodes.ContentFound;
                    objData.StartIndex = _startIndex;
                    objData.EndIndex = _endIndex > objData.Articles.RecordCount ? Convert.ToInt32(objData.Articles.RecordCount) : _endIndex;
                    BindLinkPager(objData, (int) objData.Articles.RecordCount);
                    SetPageMetas(objData);
                    CreatePrevNextUrl(objData, (int)objData.Articles.RecordCount);

                    if (ModelId > 0)
                    {
                        BindBikeInfoWidget(objData);
                    }

                    #region Bind Editorial Widgets (Maintain order, set page metas before this)

                    SetAdditionalVariables(objData);

                    if (objData.Make != null)
                    {
                        if (objData.Model != null)
                        {
                            objData.PageWidgets = base.GetEditorialWidgetData(EnumEditorialPageType.Detail);
                        }
                        else
                        {
                            objData.PageWidgets = base.GetEditorialWidgetData(EnumEditorialPageType.MakeListing);
                        }
                    }
                    else
                    {
                        objData.PageWidgets = base.GetEditorialWidgetData(EnumEditorialPageType.Listing);
                    }
                    #endregion

                    if (IsMobile)
                    {
                        GetWidgetData(objData, widgetTopCount); 
                    }

                    if (objData.Model != null)
                    {
                        objData.Series = _models.GetSeriesByModelId(ModelId);
                    }

                    SetBreadcrumList(objData);
                    objData.Page = Entities.Pages.GAPages.Editorial_List_Page;
                    if (bikeType.Equals(EnumBikeType.Scooters))
                    {
                        BindMoreAboutScootersWidget(objData);
                    }
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviewsIndexPage.GetData");
            }

            return objData;
        }

        /// <summary>
        /// Created By  : Sanskar Gupta on 12 April 2018
        /// Description : Function to set additional Page level variables.
        /// Modified By : Deepak Israni on 24 April 2018
        /// Description : Changed function to handle tagged makes, models and series.
        /// </summary>
        /// <param name="objData">VM of the page.</param>
        private void SetAdditionalVariables(ExpertReviewsIndexPageVM objData)
        {
            try
            {
                objData.PageName = _pageName;

                if (objData.Make != null)
                {
                    BikeMakeEntityBase taggedMake = _objMakeCache.GetMakeDetails((uint)objData.Make.MakeId);
                    bool isMakeLive = taggedMake.IsNew && !taggedMake.IsFuturistic;

                    EnumBikeBodyStyles bodyStyle = EnumBikeBodyStyles.AllBikes;

                    if (objData.Series != null && objData.Series.IsSeriesPageUrl == true && objData.Model == null)
                    {
                        objData.Model = _series.GetNewModels(objData.Series.SeriesId, CityId).FirstOrDefault().BikeModel;
                        ModelId = (uint)objData.Model.ModelId;
                    }

                    if (objData.Model != null)
                    {
                        List<BikeVersionMinSpecs> objVersionsList = _objBikeVersionsCache.GetVersionMinSpecs(ModelId, false);

                        if (objVersionsList != null && objVersionsList.Count > 0)
                        {
                            bodyStyle = objVersionsList.FirstOrDefault().BodyStyle;
                        }

                        BikeSeriesEntityBase bikeSeriesEntityBase = _models.GetSeriesByModelId(ModelId);
                        if (bikeSeriesEntityBase != null && bikeSeriesEntityBase.IsSeriesPageUrl)
                        {
                            objData.Series = bikeSeriesEntityBase;
                        }

                    }

                    objData.BodyStyle = bodyStyle;

                    EditorialWidgetEntity editorialWidgetData = new EditorialWidgetEntity
                    {
                        IsMobile = IsMobile,
                        IsMakeLive = isMakeLive,
                        IsModelTagged = objData.Model != null,
                        IsSeriesAvailable = objData.Series != null ? objData.Series.IsSeriesPageUrl : false,
                        IsScooterOnlyMake = objData.Make.IsScooterOnly,
                        BodyStyle = bodyStyle,
                        CityId = CityId,
                        Make = objData.Make,
                        Series = objData.Series
                    };
                    base.SetAdditionalData(editorialWidgetData);
                }
                else
                {
                    EditorialWidgetEntity editorialWidgetData = new EditorialWidgetEntity
                    {
                        IsMobile = IsMobile,
                        CityId = CityId
                    };
                    base.SetAdditionalData(editorialWidgetData);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Bikewale.Models.News.NewsIndexPage.SetAdditionalVariables"));
            }

        }


        /// <summary>
        /// Created by : Ashutosh Sharma on 23 Feb 2018
        /// Description : Method to get expert reviews index page data and bind to redux store with server rendered html for pwa.
        /// </summary>
        /// <param name="widgetTopCount"></param>
        /// <returns></returns>
        public ExpertReviewsIndexPageVM GetPwaData(int widgetTopCount)
        {
            ExpertReviewsIndexPageVM objData = new ExpertReviewsIndexPageVM();
            objData.BodyStyle = this.BodyStyle;
            objData.EditorialPageType = this.currentPageType;

            try
            {
                if (objMake != null)
                    objData.Make = objMake;

                if (objModel != null)
                    objData.Model = objModel;

                if (objSeries != null)
                    objData.Series = objSeries;

                int _startIndex = 0, _endIndex = 0;
                _pager.GetStartEndIndex(pageSize, curPageNo, out _startIndex, out _endIndex);

                objData.StartIndex = _startIndex;
                objData.EndIndex = _endIndex;

                // Added by Vivek Singh Tomar to get list of model ids for given series
                if (objData.Series != null)
                    ModelIds = _series.GetModelIdsBySeries(objData.Series.SeriesId);
                else
                    ModelIds = Convert.ToString(ModelId);

                PwaContentBase pwaCmsContent = _articles.GetArticlesByCategoryListPwa(Convert.ToString((int)EnumCMSContentType.RoadTest), _startIndex, _endIndex, (int)MakeId, (int)ModelId);



                if (pwaCmsContent != null && pwaCmsContent.RecordCount > 0)
                {
                    pwaCmsContent.PageTitle = "Expert Reviews";
                    _totalPagesCount = (uint)_pager.GetTotalPages((int)pwaCmsContent.RecordCount, pageSize);
                    status = StatusCodes.ContentFound;
                    pwaCmsContent.StartIndex = (uint)_startIndex;
                    pwaCmsContent.EndIndex = (uint)(_endIndex > pwaCmsContent.RecordCount ? Convert.ToInt32(pwaCmsContent.RecordCount) : _endIndex);
                    BindLinkPager(objData, (int)pwaCmsContent.RecordCount);
                    SetPageMetas(objData);
                    CreatePrevNextUrl(objData, (int)pwaCmsContent.RecordCount);

                    SetAdditionalVariables(objData);
                    objData.PageWidgets = base.GetEditorialWidgetData(EnumEditorialPageType.Listing);

                    if (objData.Model != null)
                        objData.Series = _models.GetSeriesByModelId(ModelId);
                    SetBreadcrumList(objData);

                    objData.Page = Entities.Pages.GAPages.Editorial_List_Page;
                    if (MakeId > 0 && ModelId > 0 && bikeType.Equals(EnumBikeType.Scooters))
                    {
                        BindMoreAboutScootersWidget(objData);
                    }

                    //Populate ReduxStore and Server rendered Html
                    objData.ReduxStore = new PwaReduxStore();
                    objData.ReduxStore.News.NewsArticleListReducer.ArticleListData.ArticleList = pwaCmsContent;
                    if (objData.PageWidgets != null)
                    {
                        PopulateStoreForWidgetData(objData); 
                    }
                    string storeJson = JsonConvert.SerializeObject(objData.ReduxStore);

                    objData.ServerRouterWrapper = _renderedArticles.GetNewsListDetails(PwaCmsHelper.GetSha256Hash(storeJson), objData.ReduxStore.News.NewsArticleListReducer,
                        "/m/expert-reviews/", "root", "ServerRouterWrapper");
                    objData.WindowState = storeJson;
                    objData.Page = Entities.Pages.GAPages.Editorial_List_Page;
                }
                else
                {
                    status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviewsIndexPage.GetData");
            }

            return objData;
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 23 Feb 2018
        /// Description : Method to populate widget data.
        /// Modified by : Sanskar Gupta om 23 April 2018
        /// Description : Changed the widget population methodology, simplified it by calling a generic function and used the new `PageWidgets` (Dictionary) logic.
        /// </summary>
        /// <param name="objData"></param>
        private void PopulateStoreForWidgetData(ExpertReviewsIndexPageVM objData)
        {
            objData.ReduxStore.News.NewsArticleListReducer.NewBikesListData.NewBikesList = ConverterUtility.MapPopularAndUpcomingWidgetDataToPwa(objData.PageWidgets);
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
                string maskingName = string.Empty;
                if (!string.IsNullOrEmpty(queryString["pn"]))
                {
                    string _pageNo = queryString["pn"];
                    if (!string.IsNullOrEmpty(_pageNo))
                    {
                        int.TryParse(_pageNo, out curPageNo);
                    }
                }
                make = queryString["make"];
                maskingName = queryString["model"];

                if (!string.IsNullOrEmpty(maskingName))
                    currentPageType = EditorialPageType.ModelPage;
                else if (!string.IsNullOrEmpty(make))
                    currentPageType = EditorialPageType.MakePage;

                if (!string.IsNullOrEmpty(make))
                {
                    ProcessMakeMaskingName(request, make);
                    ProcessModelSeriesMaskingName(request, String.Format("{0}_{1}", make, maskingName));
                }
            }
        }

        /// <summary>
        /// Created by  :  Aditi Srivasava on 30 Mar 2017
        /// Summary     :  Processes model masking name
        /// Created by  :  Vivek Singh Tomar on 24th nov 2017
        /// Summary     :  Name changed to ProcessModelSeriesMaskigName and changes made to check if given masking name is model or series
        /// </summary>
        private void ProcessModelSeriesMaskingName(HttpRequest request, string maskingName)
        {
            if (!string.IsNullOrEmpty(maskingName))
            {
                objResponse = _seriesCache.ProcessMaskingName(maskingName);
            }
            if (objResponse != null)
            {
                if (objResponse.StatusCode == 200)
                {
                    if (objResponse.IsSeriesPageCreated)
                    {
                        series = objResponse.MaskingName;
                        SeriesId = objResponse.SeriesId;
                        objSeries = new BikeSeriesEntityBase
                        {
                            SeriesId = SeriesId,
                            BodyStyle = objResponse.BodyStyle,
                            SeriesName = objResponse.Name,
                            MaskingName = series,
                            IsSeriesPageUrl = true
                        };
                        currentPageType = EditorialPageType.SeriesPage;
                        this.BodyStyle = objSeries.BodyStyle;
                    }
                    else
                    {
                        modelHelper = new ModelHelper();
                        model = objResponse.MaskingName;
                        ModelId = objResponse.ModelId;
                        objModel = modelHelper.GetModelDataById(objResponse.ModelId);
                    }
                }
                else if (objResponse.StatusCode == 301)
                {
                    status = StatusCodes.RedirectPermanent;
                    redirectUrl = request.RawUrl.Replace(objResponse.MaskingName, objResponse.NewMaskingName);
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
        /// Modified by : Rajan Chauhan on 28 Dec 2017
        /// Description : Change in PageMetaTags Title and Description
        /// </summary>
        private void SetPageMetas(ExpertReviewsIndexPageVM objData)
        {
            objData.PageMetaTags.CanonicalUrl = string.Format("{0}{1}{2}", BWConfiguration.Instance.BwHostUrlForJs, UrlFormatter.FormatExpertReviewUrl(make, series, model), (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
            objData.PageMetaTags.AlternateUrl = string.Format("{0}/m{1}{2}", BWConfiguration.Instance.BwHostUrlForJs, UrlFormatter.FormatExpertReviewUrl(make, series, model), (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
            EnumBikeBodyStyles bodyStyle = EnumBikeBodyStyles.AllBikes;

            if (this.currentPageType == EditorialPageType.SeriesPage && objMake != null)
            {
                bodyStyle = objData.Series.BodyStyle;
                string bodyStyleText = Bikewale.Utility.BodyStyleLinks.BodyStyleHeadingText(bodyStyle);
                objData.PageMetaTags.Title = string.Format("Expert Reviews about {0} {1} {2} in India | {1} {2} Comparison & Road Tests - BikeWale", objMake.MakeName, objSeries.SeriesName, objData.Series.BodyStyle == EnumBikeBodyStyles.Scooter ? "Scooters" : "Bikes");
                objData.PageMetaTags.Description = string.Format("Read the latest expert reviews on all {0} {1} {2} on BikeWale. Read about {0} {1} comparison tests and road tests exclusively on BikeWale", objMake.MakeName, objSeries.SeriesName, objData.Series.BodyStyle == EnumBikeBodyStyles.Scooter ? "Scooters" : "Bikes");
                objData.PageMetaTags.Keywords = string.Format("Expert Reviews about {0} {1}, {0} {1} expert reviews, {0} {1} first ride review, {0} {1} Long Term Report", objMake.MakeName, objSeries.SeriesName);
                objData.PageH1 = string.Format("{0} {1} Expert Reviews", objMake.MakeName, objSeries.SeriesName);
                objData.AdTags.TargetedSeries = objData.Series.SeriesName;
            }
            else if (ModelId > 0)
            {
                if (BWConfiguration.Instance.MetasMakeId.Split(',').Contains(MakeId.ToString()))
                {
                    objData.PageMetaTags.Title = string.Format("Expert Reviews on {0} {1} | First Ride & Comparison Test- BikeWale", objMake.MakeName, objModel.ModelName);
                    objData.PageH1 = string.Format(" Expert Reviews on {0} {1}", objMake.MakeName, objModel.ModelName);
                }
                else
                {
                    objData.PageMetaTags.Title = string.Format("{0} {1} Expert Reviews India - Bike Comparison & Road Tests - BikeWale", objMake.MakeName, objModel.ModelName);
                    objData.PageH1 = string.Format("{0} {1} Expert Reviews", objMake.MakeName, objModel.ModelName);
                }


                objData.PageMetaTags.Description = string.Format("Latest expert reviews on {0} {1} in India. Read {0} {1} comparison tests and road tests exclusively on BikeWale", objMake.MakeName, objModel.ModelName);
                objData.PageMetaTags.Keywords = string.Format("{0} {1} expert reviews, {0} {1} road tests, {0} {1} comparison tests, {0} {1} reviews, {0}{1} bike comparison", objMake.MakeName, objModel.ModelName);

                objData.AdTags.TargetedModel = objModel.ModelName;
                objData.AdTags.TargetedMakes = objMake.MakeName;
            }
            else if (MakeId > 0)
            {
                if (BWConfiguration.Instance.MetasMakeId.Split(',').Contains(MakeId.ToString()))
                {
                    objData.PageMetaTags.Title = string.Format("Expert Reviews on {0} Bikes | First Ride & Comparison Tests- BikeWale", objMake.MakeName);
                    objData.PageH1 = string.Format("Expert Reviews on {0} Bikes", objMake.MakeName);
                }
                else
                {
                    objData.PageMetaTags.Title = string.Format("{0} Bikes Expert Reviews India - Bike Comparison & Road Tests - BikeWale", objMake.MakeName);
                    objData.PageH1 = string.Format("{0} Bikes Expert Reviews", objMake.MakeName);
                }

                objData.PageMetaTags.Description = string.Format("Latest expert reviews on upcoming and new {0} bikes in India. Read {0} bike comparison tests and road tests exclusively on BikeWale", objMake.MakeName);
                objData.PageMetaTags.Keywords = string.Format("{0} bike expert reviews, {0} bike road tests, {0} bike comparison tests, {0} bike reviews, {0} road tests, {0} expert reviews, {0} bike comparison, {0} comparison tests.", objMake.MakeName);

                objData.AdTags.TargetedMakes = objMake.MakeName;
            }
            else
            {
                objData.PageMetaTags.Title = "Expert Bike Reviews India - Bike Comparison & Road Tests - BikeWale";
                objData.PageMetaTags.Description = "Latest expert reviews on upcoming and new bikes in India. Read bike comparison tests and road tests exclusively on BikeWale";
                objData.PageMetaTags.Keywords = "Expert bike reviews, bike road tests, bike comparison tests, bike reviews, road tests, expert reviews, bike comparison, comparison tests";
                objData.PageH1 = "Expert Reviews";
            }

            if (curPageNo > 1)
            {
                objData.PageMetaTags.Description = string.Format("Page {0} of {1} - {2}", curPageNo, _totalPagesCount, objData.PageMetaTags.Description);
                objData.PageMetaTags.Title = string.Format("Page {0} of {1} - {2}", curPageNo, _totalPagesCount, objData.PageMetaTags.Title);
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 30 Mar 2017
        /// Summary    : Get view model for page widgets
        /// Modified by Sajal Gupta on 24-08-2017
        /// description : Added Popular Scooter Brands widget
        /// Modified by Sajal Gupta on 05-12-2017
        /// description : AddedMultiTab widget
        /// Modified by : Snehal Dange on 21st dec 2017
        /// Summary : added
        /// Modified by : Sanskar Gupta on 22 Jan 2018
        /// Description : Added Newly Launched feature
        /// Modified by: Dhruv Joshi
        /// Dated: 16th April 2018
        /// Description: Getting data for generic bike widget for Model listing page
        /// </summary>
        private void GetWidgetData(ExpertReviewsIndexPageVM objData, int topCount)
        {
            if (currentPageType == EditorialPageType.SeriesPage)
            {
                FetchPopularBikes(objData);
                FetchBikesByBodyStyle(objData, objSeries.BodyStyle);
                SetSeriesWidgetProperties(objData);
            }
            else
            {
                MostPopularBikeWidgetVM MostPopularBikes = null;
                MostPopularBikeWidgetVM MostPopularMakeBikes = null;
                MostPopularBikeWidgetVM MostPopularScooters = null;
                MostPopularBikeWidgetVM MostPopularMakeScooters = null;
                UpcomingBikesWidgetVM UpcomingBikes = null;
                UpcomingBikesWidgetVM UpcomingScooters = null;
                IEnumerable<BikeMakeEntityBase> PopularScooterMakes = null;
                PopularBodyStyleVM BodyStyleVM = null;

                EnumBikeBodyStyles bodyStyle = EnumBikeBodyStyles.AllBikes;
                try
                {
                    List<BikeVersionMinSpecs> objVersionsList = _objBikeVersionsCache.GetVersionMinSpecs(ModelId, false);

                    if (objVersionsList != null && objVersionsList.Count > 0)
                        bodyStyle = objVersionsList.FirstOrDefault().BodyStyle;

                    MostPopularBikesWidget objPopularBikes = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, showCheckOnRoadCTA, false, pqSource, pageCatId, MakeId);
                    objPopularBikes.TopCount = 9;
                    objPopularBikes.CityId = CityId;

                    

                    if (MakeId > 0)
                    {
                        MostPopularMakeBikes = objPopularBikes.GetData();

                        objPopularBikes = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, showCheckOnRoadCTA, false, pqSource, pageCatId);
                        objPopularBikes.TopCount = 9;
                        objPopularBikes.CityId = CityId;
                        MostPopularBikes = objPopularBikes.GetData();
                        objData.MostPopularMakeBikes = new MostPopularBikeWidgetVM() { Bikes = MostPopularMakeBikes.Bikes.Take(6), WidgetHref = string.Format("/{0}-bikes/", objData.Make.MaskingName), WidgetLinkTitle = "View all Bikes" };
                    }
                    else
                        MostPopularBikes = objPopularBikes.GetData();

                    MostPopularBikesWidget objPopularScooters = new MostPopularBikesWidget(_bikeModels, EnumBikeType.Scooters, showCheckOnRoadCTA, false, pqSource, pageCatId, MakeId);
                    objPopularScooters.TopCount = 9;
                    objPopularScooters.CityId = CityId;

                    if (MakeId > 0)
                    {
                        MostPopularMakeScooters = objPopularScooters.GetData();

                        objPopularScooters = new MostPopularBikesWidget(_bikeModels, EnumBikeType.Scooters, showCheckOnRoadCTA, false, pqSource, pageCatId);
                        objPopularScooters.TopCount = 9;
                        objPopularScooters.CityId = CityId;
                        MostPopularScooters = objPopularScooters.GetData();
                    }
                    else
                        MostPopularScooters = objPopularScooters.GetData();

                    UpcomingBikesWidget objUpcomingBikes = new UpcomingBikesWidget(_upcoming);
                    objUpcomingBikes.Filters = new UpcomingBikesListInputEntity();
                    objUpcomingBikes.Filters.PageNo = 1;
                    objUpcomingBikes.Filters.PageSize = 6;

                    objUpcomingBikes.SortBy = EnumUpcomingBikesFilter.Default;
                    UpcomingBikes = objUpcomingBikes.GetData();

                    objData.UpcomingBikes = new UpcomingBikesWidgetVM
                    {
                        UpcomingBikes = UpcomingBikes.UpcomingBikes.Take(topCount)
                    };
                    objUpcomingBikes.Filters.BodyStyleId = (uint)EnumBikeBodyStyles.Scooter;

                    UpcomingScooters = objUpcomingBikes.GetData();


                    BikeFilters obj = new BikeFilters();
                    obj.CityId = CityId;
                    obj.TopCount = 2;
                    IEnumerable<MostPopularBikesBase> promotedBikes = _bikeModels.GetAdPromotedBike(obj, true);

                    if (bodyStyle.Equals(EnumBikeBodyStyles.Scooter))
                    {
                        PopularScooterBrandsWidget objPopularScooterBrands = new PopularScooterBrandsWidget(_objMakeCache);
                        objPopularScooterBrands.TopCount = 6;
                        PopularScooterMakes = objPopularScooterBrands.GetData();
                        objData.PopularScooterMakesWidget = PopularScooterMakes.Take(6);
                        bikeType = EnumBikeType.Scooters;

                    }
                    else
                    {
                        PopularBikesByBodyStyle BodyStyleBikes = new PopularBikesByBodyStyle(_models);
                        BodyStyleBikes.ModelId = ModelId;
                        BodyStyleBikes.CityId = CityId;
                        BodyStyleBikes.TopCount = topCount > 6 ? topCount : 6;
                        BodyStyleVM = BodyStyleBikes.GetData();

                        objData.PopularBodyStyle = BodyStyleVM;

                        if (objData.PopularBodyStyle != null)
                        {
                            objData.PopularBodyStyle.WidgetHeading = string.Format("Popular {0}", objData.PopularBodyStyle.BodyStyleText);
                            objData.PopularBodyStyle.WidgetLinkTitle = string.Format("Best {0} in India", objData.PopularBodyStyle.BodyStyleLinkTitle);
                            objData.PopularBodyStyle.WidgetHref = UrlFormatter.FormatGenericPageUrl(objData.PopularBodyStyle.BodyStyle);

                            if (objData.PopularBodyStyle.BodyStyle == EnumBikeBodyStyles.AllBikes)
                            {
                                objData.PopularBodyStyle.PopularBikes = _bikeModels.GetAdPromoteBikeFilters(promotedBikes, objData.PopularBodyStyle.PopularBikes);
                            }

                        }
                    }


                    if (IsMobile || (MakeId > 0 && ModelId == 0))
                    {
                        objData.UpcomingBikes.WidgetHeading = "Upcoming bikes";
                        objData.UpcomingBikes.WidgetHref = "/upcoming-bikes/";
                        objData.UpcomingBikes.WidgetLinkTitle = "Upcoming Bikes in India";
                        BikeSeriesEntityBase bikeSeriesEntityBase = _models.GetSeriesByModelId(ModelId);
                        if (bikeSeriesEntityBase != null && bikeSeriesEntityBase.IsSeriesPageUrl)
                        {
                            FetchPopularBikes(objData, bikeSeriesEntityBase.SeriesId);
                            objData.MostPopularBikes = new MostPopularBikeWidgetVM()
                            {
                                Bikes = objData.SeriesWidget.PopularSeriesBikes,
                                WidgetHeading = string.Format("Popular {0} Bikes", bikeSeriesEntityBase.SeriesName),
                                WidgetHref = UrlFormatter.BikeSeriesUrl(objData.Make.MaskingName, bikeSeriesEntityBase.MaskingName),
                                WidgetLinkTitle = string.Format("View all {0} bikes", bikeSeriesEntityBase.SeriesName)
                            };

                            objData.MostPopularBikes.Bikes = objData.MostPopularBikes.Bikes.Take(topCount);
                        }
                        else if (bodyStyle.Equals(EnumBikeBodyStyles.Scooter))
                        {
                            objData.MostPopularBikes = MostPopularMakeScooters;
                            objData.MostPopularBikes.WidgetHeading = string.Format("Popular {0} Scooters", objMake.MakeName);
                            objData.MostPopularBikes.WidgetHref = string.Format("/{0}-scooters/", objMake.MaskingName);
                            objData.MostPopularBikes.WidgetLinkTitle = string.Format("{0} Scooters", objMake.MakeName);

                           objData.MostPopularBikes.Bikes = objData.MostPopularBikes.Bikes.Take(topCount);
                        }
                        else if (MakeId > 0 && objMake != null)
                        {
                            objData.MostPopularBikes = MostPopularMakeBikes;
                            objData.MostPopularBikes.WidgetHeading = string.Format("Popular {0} bikes", objMake.MakeName);
                            objData.MostPopularBikes.WidgetHref = string.Format("/{0}-bikes/", objMake.MaskingName);
                            objData.MostPopularBikes.WidgetLinkTitle = string.Format("{0} Bikes", objMake.MakeName);

                            objData.MostPopularBikes.Bikes = objData.MostPopularBikes.Bikes.Take(topCount);
                        }
                        else
                        {
                            objData.MostPopularBikes = MostPopularBikes;
                            objData.MostPopularBikes.WidgetHeading = "Popular Bikes";
                            objData.MostPopularBikes.WidgetHref = "/best-bikes-in-india/";
                            objData.MostPopularBikes.WidgetLinkTitle = "Best Bikes in India";

                            objData.MostPopularBikes.Bikes = objData.MostPopularBikes.Bikes.Take(topCount);
                            objData.MostPopularBikes.Bikes = _bikeModels.GetAdPromoteBikeFilters(promotedBikes, objData.MostPopularBikes.Bikes);
                        }


                    }
                    else
                    {
                        if (MakeId == 0)
                        {
                            objData.UpcomingBikesAndUpcomingScootersWidget = new MultiTabsWidgetVM();

                            objData.UpcomingBikesAndUpcomingScootersWidget.TabHeading1 = "Upcoming bikes";
                            objData.UpcomingBikesAndUpcomingScootersWidget.TabHeading2 = "Upcoming scooters";
                            objData.UpcomingBikesAndUpcomingScootersWidget.ViewPath1 = "~/Views/Upcoming/_UpcomingBikes_Vertical.cshtml";
                            objData.UpcomingBikesAndUpcomingScootersWidget.ViewPath2 = "~/Views/Upcoming/_UpcomingBikes_Vertical.cshtml";
                            objData.UpcomingBikesAndUpcomingScootersWidget.TabId1 = "UpcomingBikes";
                            objData.UpcomingBikesAndUpcomingScootersWidget.TabId2 = "UpcomingScooters";
                            objData.UpcomingBikesAndUpcomingScootersWidget.UpcomingBikes = UpcomingBikes;

                            if (UpcomingBikes != null)
                                objData.UpcomingBikesAndUpcomingScootersWidget.UpcomingBikes.UpcomingBikes = objData.UpcomingBikesAndUpcomingScootersWidget.UpcomingBikes.UpcomingBikes.Take(6);

                            objData.UpcomingBikesAndUpcomingScootersWidget.UpcomingScooters = UpcomingScooters;

                            if (UpcomingScooters != null)
                                objData.UpcomingBikesAndUpcomingScootersWidget.UpcomingScooters.UpcomingBikes = objData.UpcomingBikesAndUpcomingScootersWidget.UpcomingScooters.UpcomingBikes.Take(6);

                            objData.UpcomingBikesAndUpcomingScootersWidget.ViewAllHref1 = "/upcoming-bikes/";
                            objData.UpcomingBikesAndUpcomingScootersWidget.ViewAllTitle1 = "View all upcoming bikes";
                            objData.UpcomingBikesAndUpcomingScootersWidget.ViewAllText1 = "View all upcoming bikes";
                            objData.UpcomingBikesAndUpcomingScootersWidget.ShowViewAllLink1 = true;
                            objData.UpcomingBikesAndUpcomingScootersWidget.ShowViewAllLink2 = false;
                            objData.UpcomingBikesAndUpcomingScootersWidget.Pages = MultiTabWidgetPagesEnum.UpcomingBikesAndUpcomingScooters;
                            objData.UpcomingBikesAndUpcomingScootersWidget.PageName = "ExpertReviews";

                            objData.PopularBikesAndPopularScootersWidget = new MultiTabsWidgetVM();

                            objData.PopularBikesAndPopularScootersWidget.TabHeading1 = "Popular Bikes";
                            objData.PopularBikesAndPopularScootersWidget.TabHeading2 = "Popular Scooters";
                            objData.PopularBikesAndPopularScootersWidget.ViewPath1 = "~/Views/BikeModels/_MostPopularBikesSideBar.cshtml";
                            objData.PopularBikesAndPopularScootersWidget.ViewPath2 = "~/Views/BikeModels/_MostPopularBikesSideBar.cshtml";
                            objData.PopularBikesAndPopularScootersWidget.TabId1 = "PopularBikes";
                            objData.PopularBikesAndPopularScootersWidget.TabId2 = "PopularScooters";
                            objData.PopularBikesAndPopularScootersWidget.MostPopularBikes = MostPopularBikes;

                            if (MostPopularBikes != null)
                                objData.PopularBikesAndPopularScootersWidget.MostPopularBikes.Bikes = _bikeModels.GetAdPromoteBikeFilters(promotedBikes, objData.PopularBikesAndPopularScootersWidget.MostPopularBikes.Bikes.Take(6));


                            objData.PopularBikesAndPopularScootersWidget.MostPopularScooters = MostPopularScooters;

                            if (MostPopularScooters != null)
                                objData.PopularBikesAndPopularScootersWidget.MostPopularScooters.Bikes = objData.PopularBikesAndPopularScootersWidget.MostPopularScooters.Bikes.Take(6);

                            objData.PopularBikesAndPopularScootersWidget.ViewAllHref2 = "/best-scooters-in-india/";
                            objData.PopularBikesAndPopularScootersWidget.ViewAllHref1 = "/best-bikes-in-india/";
                            objData.PopularBikesAndPopularScootersWidget.ViewAllTitle1 = "View all bikes";
                            objData.PopularBikesAndPopularScootersWidget.ViewAllTitle2 = "View all scooters";
                            objData.PopularBikesAndPopularScootersWidget.ViewAllText1 = "View all bikes";
                            objData.PopularBikesAndPopularScootersWidget.ViewAllText2 = "View all scooters";
                            objData.PopularBikesAndPopularScootersWidget.ShowViewAllLink1 = true;
                            objData.PopularBikesAndPopularScootersWidget.ShowViewAllLink2 = true;
                            objData.PopularBikesAndPopularScootersWidget.Pages = MultiTabWidgetPagesEnum.PopularBikesAndPopularScooters;
                            objData.PopularBikesAndPopularScootersWidget.PageName = "ExpertReviews";
                        }
                        else
                        {
                            BikeSeriesEntityBase bikeSeriesEntityBase = _models.GetSeriesByModelId(ModelId);
                            if (bikeSeriesEntityBase != null && bikeSeriesEntityBase.IsSeriesPageUrl)
                            {
                                FetchPopularBikes(objData, bikeSeriesEntityBase.SeriesId);
                                if (bodyStyle.Equals(EnumBikeBodyStyles.Scooter))
                                {
                                    BindSeriesBikesAndOtherBrands(objData, bikeSeriesEntityBase, PopularScooterMakes);
                                    BindPopularScootersAndUpcomingScootersWidget(objData, MostPopularScooters, UpcomingScooters);
                                }
                                else if (bodyStyle.Equals(EnumBikeBodyStyles.Cruiser) || bodyStyle.Equals(EnumBikeBodyStyles.Sports))
                                {
                                    BindSeriesBikesAndModelBodyStyleBikes(objData, bikeSeriesEntityBase, bodyStyle);
                                    BindPopularBikesAndUpcomingBikesWidget(objData, MostPopularBikes, UpcomingBikes);
                                }
                                else
                                {
                                    objData.MostPopularBikes = new MostPopularBikeWidgetVM()
                                    {
                                        Bikes = objData.SeriesWidget.PopularSeriesBikes,
                                        WidgetHeading = string.Format("{0} Bikes", bikeSeriesEntityBase.SeriesName),
                                        WidgetHref = UrlFormatter.BikeSeriesUrl(objData.Make.MaskingName, bikeSeriesEntityBase.MaskingName),
                                        WidgetLinkTitle = string.Format("View all {0} bikes", bikeSeriesEntityBase.SeriesName)
                                    };
                                    BindPopularBikesAndUpcomingBikesWidget(objData, MostPopularBikes, UpcomingBikes);
                                }
                            }
                            else if (bodyStyle.Equals(EnumBikeBodyStyles.Scooter))
                            {
                                objData.PopularMakeScootersAndOtherBrandsWidget = new MultiTabsWidgetVM();

                                objData.PopularMakeScootersAndOtherBrandsWidget.TabHeading1 = string.Format("{0} Scooters", objData.Make.MakeName);
                                objData.PopularMakeScootersAndOtherBrandsWidget.TabHeading2 = "Other Brands";
                                objData.PopularMakeScootersAndOtherBrandsWidget.ViewPath1 = "~/Views/BikeModels/_MostPopularBikesSideBar.cshtml";
                                objData.PopularMakeScootersAndOtherBrandsWidget.ViewPath2 = "~/Views/Scooters/_PopularScooterBrandsVerticalWidget.cshtml";
                                objData.PopularMakeScootersAndOtherBrandsWidget.TabId1 = "PopularMakeScooters";
                                objData.PopularMakeScootersAndOtherBrandsWidget.TabId2 = "OtherBrands";
                                objData.PopularMakeScootersAndOtherBrandsWidget.MostPopularMakeScooters = MostPopularMakeScooters;

                                if (MostPopularMakeScooters != null)
                                    objData.PopularMakeScootersAndOtherBrandsWidget.MostPopularMakeScooters.Bikes = objData.PopularMakeScootersAndOtherBrandsWidget.MostPopularMakeScooters.Bikes.Take(6);

                                if (PopularScooterMakes != null)
                                    objData.PopularMakeScootersAndOtherBrandsWidget.PopularScooterMakes = PopularScooterMakes.Take(6);

                                objData.PopularMakeScootersAndOtherBrandsWidget.ViewAllHref1 = string.Format("/{0}-scooters/", objData.Make.MaskingName);
                                objData.PopularMakeScootersAndOtherBrandsWidget.ViewAllHref2 = "/scooters/";
                                objData.PopularMakeScootersAndOtherBrandsWidget.ViewAllTitle1 = "View all scooters";
                                objData.PopularMakeScootersAndOtherBrandsWidget.ViewAllTitle2 = "View other brands";
                                objData.PopularMakeScootersAndOtherBrandsWidget.ViewAllText1 = "View all scooters";
                                objData.PopularMakeScootersAndOtherBrandsWidget.ViewAllText2 = "View other brands";
                                objData.PopularMakeScootersAndOtherBrandsWidget.ShowViewAllLink1 = true;
                                objData.PopularMakeScootersAndOtherBrandsWidget.ShowViewAllLink2 = true;
                                objData.PopularMakeScootersAndOtherBrandsWidget.Pages = MultiTabWidgetPagesEnum.PopularMakeScootersAndOtherBrands;
                                objData.PopularMakeScootersAndOtherBrandsWidget.PageName = "ExpertReviews";
                                BindPopularScootersAndUpcomingScootersWidget(objData, MostPopularScooters, UpcomingScooters);
                            }
                            else if (bodyStyle.Equals(EnumBikeBodyStyles.Sports) || bodyStyle.Equals(EnumBikeBodyStyles.Cruiser))
                            {
                                objData.PopularMakeBikesAndBodyStyleBikesWidget = new MultiTabsWidgetVM();

                                objData.PopularMakeBikesAndBodyStyleBikesWidget.TabHeading1 = string.Format("{0} bikes", objData.Make.MakeName);
                                objData.PopularMakeBikesAndBodyStyleBikesWidget.TabHeading2 = bodyStyle.Equals(EnumBikeBodyStyles.Sports) ? "Sports bikes" : "Cruisers";
                                objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewPath1 = "~/Views/BikeModels/_MostPopularBikesSideBar.cshtml";
                                objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewPath2 = "~/Views/BestBikes/_PopularBodyStyle_Vertical.cshtml";
                                objData.PopularMakeBikesAndBodyStyleBikesWidget.TabId1 = "PopularMakeBikes";
                                objData.PopularMakeBikesAndBodyStyleBikesWidget.TabId2 = bodyStyle.Equals(EnumBikeBodyStyles.Sports) ? "PopularSportsBikes" : "PopularCruisers";
                                objData.PopularMakeBikesAndBodyStyleBikesWidget.MostPopularMakeBikes = MostPopularMakeBikes;

                                if (MostPopularMakeBikes != null)
                                    objData.PopularMakeBikesAndBodyStyleBikesWidget.MostPopularMakeBikes.Bikes = objData.PopularMakeBikesAndBodyStyleBikesWidget.MostPopularMakeBikes.Bikes.Take(6);

                                objData.PopularMakeBikesAndBodyStyleBikesWidget.PopularBodyStyle = new PopularBodyStyleVM() { PopularBikes = objData.PopularBodyStyle.PopularBikes.Take(6) };
                                objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllHref1 = string.Format("/{0}-bikes/", objData.Make.MaskingName);
                                objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllHref2 = !bodyStyle.Equals(EnumBikeBodyStyles.Sports) ? "/best-cruiser-bikes-in-india/" : "/best-sports-bikes-in-india/";
                                objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllTitle1 = "View all bikes";
                                objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllTitle2 = bodyStyle.Equals(EnumBikeBodyStyles.Sports) ? "View all Sports bikes" : "View all Cruisers";
                                objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllText1 = "View all bikes";
                                objData.PopularMakeBikesAndBodyStyleBikesWidget.ViewAllText2 = bodyStyle.Equals(EnumBikeBodyStyles.Sports) ? "View all Sports bikes" : "View all Cruisers";
                                objData.PopularMakeBikesAndBodyStyleBikesWidget.ShowViewAllLink1 = true;
                                objData.PopularMakeBikesAndBodyStyleBikesWidget.ShowViewAllLink2 = true;
                                objData.PopularMakeBikesAndBodyStyleBikesWidget.Pages = MultiTabWidgetPagesEnum.PopularMakeBikesAndBodyStyleWidget;
                                objData.PopularMakeBikesAndBodyStyleBikesWidget.PageName = "ExpertReviews";
                                BindPopularBikesAndUpcomingBikesWidget(objData, MostPopularBikes, UpcomingBikes);
                            }
                            else
                            {
                                objData.PopularBikesAndUpcomingBikesWidget = new MultiTabsWidgetVM();

                                objData.PopularBikesAndUpcomingBikesWidget.TabHeading1 = "Popular Bikes";
                                objData.PopularBikesAndUpcomingBikesWidget.TabHeading2 = "Upcoming Bikes";
                                objData.PopularBikesAndUpcomingBikesWidget.ViewPath1 = "~/Views/BikeModels/_MostPopularBikesSideBar.cshtml";
                                objData.PopularBikesAndUpcomingBikesWidget.ViewPath2 = "~/Views/Upcoming/_UpcomingBikes_Vertical.cshtml";
                                objData.PopularBikesAndUpcomingBikesWidget.TabId1 = "PopularBikes";
                                objData.PopularBikesAndUpcomingBikesWidget.TabId2 = "UpcomingBikes";
                                objData.PopularBikesAndUpcomingBikesWidget.MostPopularBikes = MostPopularBikes;

                                if (MostPopularBikes != null)
                                    objData.PopularBikesAndUpcomingBikesWidget.MostPopularBikes.Bikes = objData.PopularBikesAndUpcomingBikesWidget.MostPopularBikes.Bikes.Take(6);

                                objData.PopularBikesAndUpcomingBikesWidget.UpcomingBikes = UpcomingBikes;

                                if (UpcomingBikes != null)
                                    objData.PopularBikesAndUpcomingBikesWidget.UpcomingBikes.UpcomingBikes = objData.PopularBikesAndUpcomingBikesWidget.UpcomingBikes.UpcomingBikes.Take(6);

                                objData.PopularBikesAndUpcomingBikesWidget.ViewAllHref1 = "/best-bikes-in-india/";
                                objData.PopularBikesAndUpcomingBikesWidget.ViewAllHref2 = "/upcoming-bikes/";
                                objData.PopularBikesAndUpcomingBikesWidget.ViewAllTitle1 = "View all bikes";
                                objData.PopularBikesAndUpcomingBikesWidget.ViewAllTitle2 = "View all upcoming bikes";
                                objData.PopularBikesAndUpcomingBikesWidget.ViewAllText1 = "View all bikes";
                                objData.PopularBikesAndUpcomingBikesWidget.ViewAllText2 = "View all upcoming bikes";
                                objData.PopularBikesAndUpcomingBikesWidget.ShowViewAllLink1 = true;
                                objData.PopularBikesAndUpcomingBikesWidget.ShowViewAllLink2 = true;
                                objData.PopularBikesAndUpcomingBikesWidget.Pages = MultiTabWidgetPagesEnum.PopularBikesAndUpcomingBikes;
                                objData.PopularBikesAndUpcomingBikesWidget.PageName = "ExpertReviews";

                                objData.PopularBikesAndUpcomingBikesWidget.MostPopularBikes.Bikes = _bikeModels.GetAdPromoteBikeFilters(promotedBikes, objData.PopularBikesAndUpcomingBikesWidget.MostPopularBikes.Bikes);

                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviewsIndexPage.GetWidgetData");
                }
            }
        }
        private void BindPopularBikesAndUpcomingBikesWidget(ExpertReviewsIndexPageVM objData, MostPopularBikeWidgetVM mostPopularBikes, UpcomingBikesWidgetVM upcomingBikes)
        {
            try
            {
                objData.PopularBikesAndUpcomingBikesWidget = new MultiTabsWidgetVM();

                objData.PopularBikesAndUpcomingBikesWidget.TabHeading1 = "Popular Bikes";
                objData.PopularBikesAndUpcomingBikesWidget.TabHeading2 = "Upcoming Bikes";
                objData.PopularBikesAndUpcomingBikesWidget.ViewPath1 = "~/Views/BikeModels/_MostPopularBikesSideBar.cshtml";
                objData.PopularBikesAndUpcomingBikesWidget.ViewPath2 = "~/Views/Upcoming/_UpcomingBikes_Vertical.cshtml";
                objData.PopularBikesAndUpcomingBikesWidget.TabId1 = "PopularBikes";
                objData.PopularBikesAndUpcomingBikesWidget.TabId2 = "UpcomingBikes";
                objData.PopularBikesAndUpcomingBikesWidget.MostPopularBikes = mostPopularBikes;

                if (mostPopularBikes != null)
                    objData.PopularBikesAndUpcomingBikesWidget.MostPopularBikes.Bikes = objData.PopularBikesAndUpcomingBikesWidget.MostPopularBikes.Bikes.Take(6);

                objData.PopularBikesAndUpcomingBikesWidget.UpcomingBikes = upcomingBikes;

                if (upcomingBikes != null)
                    objData.PopularBikesAndUpcomingBikesWidget.UpcomingBikes.UpcomingBikes = objData.PopularBikesAndUpcomingBikesWidget.UpcomingBikes.UpcomingBikes.Take(6);

                objData.PopularBikesAndUpcomingBikesWidget.ViewAllHref1 = "/best-bikes-in-india/";
                objData.PopularBikesAndUpcomingBikesWidget.ViewAllHref2 = "/upcoming-bikes/";
                objData.PopularBikesAndUpcomingBikesWidget.ViewAllTitle1 = "View all bikes";
                objData.PopularBikesAndUpcomingBikesWidget.ViewAllTitle2 = "View all upcoming bikes";
                objData.PopularBikesAndUpcomingBikesWidget.ViewAllText1 = "View all bikes";
                objData.PopularBikesAndUpcomingBikesWidget.ViewAllText2 = "View all upcoming bikes";
                objData.PopularBikesAndUpcomingBikesWidget.ShowViewAllLink1 = true;
                objData.PopularBikesAndUpcomingBikesWidget.ShowViewAllLink2 = true;
                objData.PopularBikesAndUpcomingBikesWidget.Pages = MultiTabWidgetPagesEnum.PopularBikesAndUpcomingBikes;
                objData.PopularBikesAndUpcomingBikesWidget.PageName = "ExpertReviews";
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Models.News.ExpertReviewsIndexPage.BindPopularBikesAndUpcomingBikesWidget");
            }
        }

        private void BindPopularScootersAndUpcomingScootersWidget(ExpertReviewsIndexPageVM objData, MostPopularBikeWidgetVM mostPopularBikes, UpcomingBikesWidgetVM upcomingScooters)
        {
            try
            {
                objData.PopularScootersAndUpcomingScootersWidget = new MultiTabsWidgetVM();

                objData.PopularScootersAndUpcomingScootersWidget.TabHeading1 = "Popular Scooters";
                objData.PopularScootersAndUpcomingScootersWidget.TabHeading2 = "Upcoming Scooters";
                objData.PopularScootersAndUpcomingScootersWidget.ViewPath1 = "~/Views/BikeModels/_MostPopularBikesSideBar.cshtml";
                objData.PopularScootersAndUpcomingScootersWidget.ViewPath2 = "~/Views/Upcoming/_UpcomingBikes_Vertical.cshtml"; ;
                objData.PopularScootersAndUpcomingScootersWidget.TabId1 = "PopularScooters";
                objData.PopularScootersAndUpcomingScootersWidget.TabId2 = "UpcomingScooters";
                objData.PopularScootersAndUpcomingScootersWidget.MostPopularScooters = mostPopularBikes;

                if (mostPopularBikes != null)
                    objData.PopularScootersAndUpcomingScootersWidget.MostPopularScooters.Bikes = objData.PopularScootersAndUpcomingScootersWidget.MostPopularScooters.Bikes.Take(6);

                objData.PopularScootersAndUpcomingScootersWidget.UpcomingScooters = upcomingScooters;

                if (upcomingScooters != null)
                    objData.PopularScootersAndUpcomingScootersWidget.UpcomingScooters.UpcomingBikes = objData.PopularScootersAndUpcomingScootersWidget.UpcomingScooters.UpcomingBikes.Take(6);

                objData.PopularScootersAndUpcomingScootersWidget.ViewAllHref1 = "/best-scooters-in-india/";
                objData.PopularScootersAndUpcomingScootersWidget.ViewAllTitle1 = "View all scooters";
                objData.PopularScootersAndUpcomingScootersWidget.ViewAllText1 = "View all scooters";
                objData.PopularScootersAndUpcomingScootersWidget.ShowViewAllLink1 = true;
                objData.PopularScootersAndUpcomingScootersWidget.ShowViewAllLink2 = false;
                objData.PopularScootersAndUpcomingScootersWidget.Pages = MultiTabWidgetPagesEnum.PopularScootersAndUpcomingScooters;
                objData.PopularScootersAndUpcomingScootersWidget.PageName = "ExpertReviews";
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Models.News.ExpertReviewsIndexPage.BindSeriesBikesAndOtherBrands");
            }
        }

        private void BindSeriesBikesAndOtherBrands(ExpertReviewsIndexPageVM objData, BikeSeriesEntityBase bikeSeriesEntityBase, IEnumerable<BikeMakeEntityBase> popularScooterMakes)
        {
            try
            {
                objData.SeriesBikesAndOtherBrands = new MultiTabsWidgetVM();

                objData.SeriesBikesAndOtherBrands.TabHeading1 = string.Format("{0} Scooters", bikeSeriesEntityBase.SeriesName);
                objData.SeriesBikesAndOtherBrands.TabHeading2 = "Other Brands";
                objData.SeriesBikesAndOtherBrands.ViewPath1 = "~/Views/Shared/_EditorialSeriesBikesWidget.cshtml";
                objData.SeriesBikesAndOtherBrands.ViewPath2 = "~/Views/Scooters/_PopularScooterBrandsVerticalWidget.cshtml";
                objData.SeriesBikesAndOtherBrands.TabId1 = "SeriesScooters";
                objData.SeriesBikesAndOtherBrands.TabId2 = "OtherBrands";
                objData.SeriesBikesAndOtherBrands.PopularSeriesBikes = objData.SeriesWidget.PopularSeriesBikes;

                if (popularScooterMakes != null)
                    objData.SeriesBikesAndOtherBrands.PopularScooterMakes = popularScooterMakes.Take(6);

                objData.SeriesBikesAndOtherBrands.ViewAllHref1 = UrlFormatter.BikeSeriesUrl(objData.Make.MaskingName, bikeSeriesEntityBase.MaskingName);
                objData.SeriesBikesAndOtherBrands.ViewAllHref2 = "/scooters/";
                objData.SeriesBikesAndOtherBrands.ViewAllTitle1 = string.Format("View all {0} scooters", bikeSeriesEntityBase.SeriesName);
                objData.SeriesBikesAndOtherBrands.ViewAllTitle2 = "View other brands";
                objData.SeriesBikesAndOtherBrands.ViewAllText1 = string.Format("View all {0} scooters", bikeSeriesEntityBase.SeriesName);
                objData.SeriesBikesAndOtherBrands.ViewAllText2 = "View other brands";
                objData.SeriesBikesAndOtherBrands.ShowViewAllLink1 = true;
                objData.SeriesBikesAndOtherBrands.ShowViewAllLink2 = true;
                objData.SeriesBikesAndOtherBrands.Pages = MultiTabWidgetPagesEnum.SeriesBikesAndOtherBrands;
                objData.SeriesBikesAndOtherBrands.PageName = "ExpertReviews";
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Models.News.ExpertReviewsIndexPage.BindSeriesBikesAndOtherBrands");
            }
        }

        private void BindSeriesBikesAndModelBodyStyleBikes(ExpertReviewsIndexPageVM objData, BikeSeriesEntityBase bikeSeriesEntityBase, EnumBikeBodyStyles bodyStyles)
        {
            try
            {
                if (objData.SeriesWidget != null && objMake != null && bikeSeriesEntityBase != null)
                {
                    objData.SeriesBikesAndModelBodyStyleBikes = new MultiTabsWidgetVM()
                    {
                        TabHeading1 = string.Format("{0} {1}", bikeSeriesEntityBase.SeriesName, bodyStyles == EnumBikeBodyStyles.Scooter ? "Scooters" : "Bikes"),
                        ViewPath1 = "~/Views/Shared/_EditorialSeriesBikesWidget.cshtml",
                        TabId1 = "SeriesBikes",
                        ViewAllHref1 = string.Format("/{0}-bikes/{1}/", objMake.MaskingName, bikeSeriesEntityBase.MaskingName),
                        ViewAllTitle1 = string.Format("View all {0} {1}", bikeSeriesEntityBase.SeriesName, bodyStyles == EnumBikeBodyStyles.Scooter ? "scooters" : "bikes"),
                        ViewAllText1 = string.Format("View all {0} {1}", bikeSeriesEntityBase.SeriesName, bodyStyles == EnumBikeBodyStyles.Scooter ? "scooters" : "bikes"),
                        ShowViewAllLink1 = true,
                        PopularSeriesBikes = objData.SeriesWidget.PopularSeriesBikes,
                        TabHeading2 = string.Format("{0}", Bikewale.Utility.BodyStyleLinks.BodyStyleText(bodyStyles)),
                        ViewPath2 = "~/Views/BestBikes/_PopularBodyStyle_Vertical.cshtml",
                        TabId2 = "PopularBodyStyle",
                        ViewAllHref2 = bodyStyles == EnumBikeBodyStyles.Scooter ? "/best-scooters-in-india/" : (bodyStyles == EnumBikeBodyStyles.Sports ? "/best-sports-bikes-in-india/" : (bodyStyles == EnumBikeBodyStyles.Cruiser ? "/best-cruiser-bikes-in-india/" : "/best-bikes-in-india/")),
                        ViewAllTitle2 = string.Format("View all popular {0}", Bikewale.Utility.BodyStyleLinks.BodyStyleText(bodyStyles).ToLower()),
                        ViewAllText2 = string.Format("View all popular {0}", Bikewale.Utility.BodyStyleLinks.BodyStyleText(bodyStyles).ToLower()),
                        ShowViewAllLink2 = true,
                        PopularBodyStyle = new PopularBodyStyleVM() { PopularBikes = objData.PopularBodyStyle.PopularBikes.Take(6) },
                        Pages = MultiTabWidgetPagesEnum.SeriesBikesAndModelBodyStyleBike,
                        PageName = "ExpertReviews"
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.News.ExpertReviewsIndexPage.BindSeriesBikesAndModelBodyStyleBikes");
            }
        }
        /// <summary>
        /// Sets the series widget properties.
        /// Modified by : Rajan Chauhan on 27 Dec 2017
        /// Description : Change of Widget on series page to PopularSeriesAndBodyStyleWidget and bottom widget to UpcomingBikesByBodyStyleWidget
        /// </summary>
        /// <param name="objData">The object data.</param>
        private void SetSeriesWidgetProperties(ExpertReviewsIndexPageVM objData)
        {
            string bodyStyleText = Bikewale.Utility.BodyStyleLinks.BodyStyleText(this.BodyStyle);
			if (IsMobile)
			{
				objData.SeriesMobileWidget = new EditorialSeriesMobileWidgetVM();
				if (objData.SeriesWidget.PopularSeriesBikes.Any())
				{
                    objData.SeriesMobileWidget.PopularSeriesBikesVM = new PopularBodyStyleVM() {
                        PopularBikes = objData.SeriesWidget.PopularSeriesBikes.Take(6),
					    WidgetHeading = string.Format("Popular {0} {1}", objData.Series.SeriesName, this.BodyStyle == EnumBikeBodyStyles.Scooter ? "Scooters" : "Bikes"),
                        WidgetLinkTitle = string.Format("View all {0} {1}", objSeries.SeriesName, this.BodyStyle == EnumBikeBodyStyles.Scooter ? "scooters" : "bikes"),
                        WidgetHref = string.Format("/m/{0}-bikes/{1}/", objMake.MaskingName, objData.Series.MaskingName)
                    };
					
                }

                if (objData.SeriesWidget.PopularBikesByBodyStyle.Any())
                {
                    objData.SeriesMobileWidget.PopularBikesByBodyStyleVM = new BestBikesEditorialWidgetVM() {
                        BestBikes = objData.SeriesWidget.PopularBikesByBodyStyle.Take(6),
                        WidgetHeading = string.Format("Popular {0}", bodyStyleText),
                        WidgetLinkTitle = string.Format("View all popular {0}", this.BodyStyle == EnumBikeBodyStyles.Scooter ? "scooters" : "bikes"),
                        WidgetHref = string.Format("/m{0}", Bikewale.Utility.UrlFormatter.FormatGenericPageUrl(this.BodyStyle))
                    
                    };
                }
            }
            else
            {
				objData.PopularSeriesAndBodyStyleWidget = new MultiTabsWidgetVM() {
                    TabHeading1 = string.Format("{0} {1}", objSeries.SeriesName, this.BodyStyle == EnumBikeBodyStyles.Scooter ? "Scooters" : "Bikes"),
				    ViewPath1 = "~/Views/Shared/_EditorialSeriesBikesWidget.cshtml",
				    TabId1 = "SeriesBikes",
				    ViewAllHref1 = string.Format("/{0}-bikes/{1}/", objMake.MaskingName, objSeries.MaskingName),
				    ViewAllTitle1 = string.Format("View all {0} {1}", objSeries.SeriesName, this.BodyStyle == EnumBikeBodyStyles.Scooter ? "scooters" : "bikes"),
				    ViewAllText1 = string.Format("View all {0} {1}", objSeries.SeriesName, this.BodyStyle == EnumBikeBodyStyles.Scooter ? "scooters" : "bikes"),
				    ShowViewAllLink1 = true,
				    PopularSeriesBikes = objData.SeriesWidget.PopularSeriesBikes,
                    TabHeading2 = string.Format("Popular {0}", Bikewale.Utility.BodyStyleLinks.BodyStyleText(this.BodyStyle)),
                    ViewPath2 = "~/Views/Shared/_EditorialBestBikesSideBar.cshtml",
                    TabId2 = "PopularBodyStyle",
                    ViewAllHref2 = this.BodyStyle == EnumBikeBodyStyles.Scooter ? "/best-scooters-in-india/" : (this.BodyStyle == EnumBikeBodyStyles.Sports ? "/best-sports-bikes-in-india/" : (this.BodyStyle == EnumBikeBodyStyles.Cruiser ? "/best-cruiser-bikes-in-india/" : "/best-bikes-in-india/")),
                    ViewAllTitle2 = string.Format("View all popular {0}", Bikewale.Utility.BodyStyleLinks.BodyStyleText(this.BodyStyle).ToLower()),
                    ViewAllText2 = string.Format("View all popular {0}", Bikewale.Utility.BodyStyleLinks.BodyStyleText(this.BodyStyle).ToLower()),
                    ShowViewAllLink2 = true,
                    PopularBikesByBodyStyle = objData.SeriesWidget.PopularBikesByBodyStyle,
                    Pages = MultiTabWidgetPagesEnum.PopularSeriesAndBodyStyleWidget,
                    PageName = "ExpertReviews"
                };


                // Bottom widget

                bool IsUpcomingViewAllLinkShown = !(this.BodyStyle == EnumBikeBodyStyles.Scooter || this.BodyStyle == EnumBikeBodyStyles.Sports || this.BodyStyle == EnumBikeBodyStyles.Cruiser);

                objData.UpcomingBikesByBodyStyleWidget = new UpcomingBikesWidgetVM()
                {
                    WidgetHeading = string.Format("Upcoming {0}", Bikewale.Utility.BodyStyleLinks.BodyStyleText(this.BodyStyle)),
                    WidgetHref = IsUpcomingViewAllLinkShown ? "/upcoming-bikes/" : "",
                    WidgetLinkTitle = string.Format("View all upcoming {0}", this.BodyStyle == EnumBikeBodyStyles.Scooter ? "scooters" : "bikes"),
                    UpcomingBikes = objData.SeriesWidget.UpcomingBikesByBodyStyle,
                    ShowViewAllLink = IsUpcomingViewAllLinkShown
                };
            }
        }
        /// <summary>
        /// Fetches the bikes by body style.
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="bodyStyle">The body style.</param>
        private void FetchBikesByBodyStyle(ExpertReviewsIndexPageVM objData, EnumBikeBodyStyles bodyStyle)
        {
            try
            {
                if (!IsMobile)
                {
                    // Fetch Upcoming bikes
                    UpcomingBikesListInputEntity filters = new UpcomingBikesListInputEntity()
                    {
                        PageNo = 1,
                        PageSize = 6,
                        BodyStyleId = (uint)bodyStyle
                    };

                    objData.SeriesWidget.UpcomingBikesByBodyStyle = _upcoming.GetModels(filters, EnumUpcomingBikesFilter.Default);
                }
                // Popular BodyStyles
                IEnumerable<BestBikeEntityBase> bestBikesByBodyStyle = _models.GetBestBikesByCategory(bodyStyle);
                if (bestBikesByBodyStyle != null && bestBikesByBodyStyle.Any())
                {
                    objData.SeriesWidget.PopularBikesByBodyStyle = bestBikesByBodyStyle.Take(6);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Models.News.NewsIndexPage.FetchBikesByBodyStyle");
            }

        }
        /// <summary>
        /// Fetches the popular bikes.
        /// Modified by : Rajan Chauhan on 28 Dec 2017
        /// Description : Removed the PopularMakeSeriesBikes from objData SeriesWidget
        /// </summary>
        private void FetchPopularBikes(ExpertReviewsIndexPageVM objData, uint seriesId = 0)
        {
            try
            {
                objData.SeriesWidget = new EditorialSeriesWidgetVM();
                IEnumerable<MostPopularBikesBase> makePopularBikes = _models.GetMostPopularBikesByMake((int)MakeId);
                string modelIds = string.Empty;
                if (seriesId > 0)
                    modelIds = _series.GetModelIdsBySeries(seriesId);
                else
                    modelIds = _series.GetModelIdsBySeries(objData.Series.SeriesId);
                string[] modelArray = modelIds.Split(',');
                if (modelArray.Length > 0)
                {
                    var popularSeries = (from bike in makePopularBikes
                                         where modelArray.Contains(bike.objModel.ModelId.ToString())
                                         select bike
                                         ).ToList<MostPopularBikesBase>();
                    if (popularSeries != null && popularSeries.Any())
                        objData.SeriesWidget.PopularSeriesBikes = popularSeries.Take(6);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Models.News.NewsIndexPage.FetchPopularBikes");
            }

        }
        /// <summary>
        /// Created By : Aditi Srivastava on 30 Mar 2017
        /// Summary    : Bind link pager
        /// Modified by : Ashutosh Sharma on 26 Feb 2018
        /// Description : Added recordCount in arguments.
        /// </summary>
        private void BindLinkPager(ExpertReviewsIndexPageVM objData, int recordCount)
        {
            try
            {
                objData.PagerEntity = new PagerEntity();
                objData.PagerEntity.BaseUrl = string.Format("{0}{1}", (IsMobile ? "/m" : ""), UrlFormatter.FormatExpertReviewUrl(make, series, model));
                objData.PagerEntity.PageNo = curPageNo;
                objData.PagerEntity.PagerSlotSize = pagerSlotSize;
                objData.PagerEntity.PageUrlType = "page/";
                objData.PagerEntity.TotalResults = recordCount;
                objData.PagerEntity.PageSize = pageSize;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.ExpertReviewsIndexPage.BindLinkPager");
            }
        }
        /// <summary>
        /// Created By : Aditi Srivastava on 30 Mar 2017
        /// Summary    : Create previous and next page urls
        /// Modified by : Ashutosh Sharma on 23 Feb 2018
        /// Description : Added argument recordCount and removed use of objData.Articles.RecordCount in method.
        /// </summary>
        /// <param name="objData"></param>
        private void CreatePrevNextUrl(ExpertReviewsIndexPageVM objData, int recordCount)
        {
            string _mainUrl = String.Format("{0}{1}page/", BWConfiguration.Instance.BwHostUrl, objData.PagerEntity.BaseUrl);
            string prevPageNumber = string.Empty, nextPageNumber = string.Empty;
            int totalPages = _pager.GetTotalPages(recordCount, pageSize);
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
        private void SetBreadcrumList(ExpertReviewsIndexPageVM objData)
        {
            try
            {
                IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
                string bikeUrl, scooterUrl, seriesUrl;
                bikeUrl = scooterUrl = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
                ushort position = 1;
                if (IsMobile)
                {
                    bikeUrl += "m/";
                }

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, "Home"));


                if (objData.Make != null)
                {
                    bikeUrl = string.Format("{0}{1}-bikes/", bikeUrl, objData.Make.MaskingName);

                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, string.Format("{0} Bikes", objData.Make.MakeName)));
                }

                if ((objData.Model != null || (objData.Series != null && objData.Series.IsSeriesPageUrl)) && objData.BodyStyle.Equals(EnumBikeBodyStyles.Scooter) && !(objData.Make.IsScooterOnly))
                {
                    if (IsMobile)
                    {
                        scooterUrl += "m/";
                    }
                    scooterUrl = string.Format("{0}{1}-scooters/", scooterUrl, objData.Make.MaskingName);

                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, scooterUrl, string.Format("{0} Scooters", objData.Make.MakeName)));
                }

                if (objData.Series != null && objData.Series.IsSeriesPageUrl)
                {
                    seriesUrl = string.Format("{0}{1}/", bikeUrl, objData.Series.MaskingName);
                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, seriesUrl, objData.Series.SeriesName));
                }

                if (objData.Model != null)
                {
                    bikeUrl = string.Format("{0}{1}/", bikeUrl, objData.Model.MaskingName);

                    BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, bikeUrl, string.Format("{0} {1}", objData.Make.MakeName, objData.Model.ModelName)));
                }

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, "Reviews"));

                objData.BreadcrumbList.BreadcrumListItem = BreadCrumbs;

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Exception : Bikewale.Models.ExpertReviewsIndexPage.SetBreadcrumList");
            }

        }

        /// <summary>
        /// Created By: Snehal Dange on 21th Dec 2017
        /// Summary : Bind more about scooter widget
        /// </summary>
        /// <param name="objData"></param>
        private void BindMoreAboutScootersWidget(ExpertReviewsIndexPageVM objData)
        {
            try
            {
                MoreAboutScootersWidget obj = new MoreAboutScootersWidget(_models, _objCityCache, _objBikeVersionsCache, _objGenericBike, BikeInfoTabType.ExpertReview);
                obj.modelId = ModelId;
                objData.ObjMoreAboutScooter = obj.GetData();
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Bikewale.Models.ExpertReviewsIndexPage.BindMoreAboutScootersWidget : ModelId {0}", ModelId));
            }
        }

        private void BindBikeInfoWidget(ExpertReviewsIndexPageVM objData)
        {
            BikeInfoWidget genericInfoWidget = new BikeInfoWidget(_objGenericBike, _objCityCache, ModelId, CityId, _totalTabCount, _pageId);
            objData.GenericBikeInfoWidget = genericInfoWidget.GetData();
        }
        #endregion
    }
}