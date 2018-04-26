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
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IBikeInfo _objGenericBike = null;
        private readonly IArticles _articles;
        private readonly IPWACMSCacheRepository _renderedArticles;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _modelMaskingCache = null;

        #endregion

        #region Page level variables
        private string _pageName = "Editorial List";
        private const int pageSize = 10, pagerSlotSize = 5;
        private int curPageNo = 1;
        private uint _totalPagesCount;
        private string makeMaskingName = string.Empty, model = string.Empty, series = string.Empty;
        private uint MakeId, ModelId, CityId, SeriesId;
        public string redirectUrl, CityName;
        public StatusCodes status;
        private GlobalCityAreaEntity currentCityArea;
        private BikeModelEntity objModel = null;
        private BikeMakeEntityBase objMake = null;
        private BikeSeriesEntityBase objSeries;
        private SeriesMaskingResponse objResponse;
        private EditorialPageType currentPageType = EditorialPageType.Default;
        public EnumBikeBodyStyles BodyStyle = EnumBikeBodyStyles.AllBikes;
        private EnumBikeType bikeType = EnumBikeType.All;
        private string ModelIds = string.Empty;
        private readonly uint _totalTabCount = 3;
        private BikeInfoTabType _pageId = BikeInfoTabType.ExpertReview;
        private EnumEditorialPageType widgetType;
        private bool isMakeLive, isModelTagged, isSeriesAvailable;
        #endregion

        #region Public properties
        public bool IsMobile { get; set; }

        #endregion

        #region Constructor
        public ExpertReviewsIndexPage(ICMSCacheContent cmsCache, IPager pager, IBikeModelsCacheRepository<int> models,
            IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming, IBikeMakesCacheRepository objMakeCache, IBikeVersionCacheRepository<BikeVersionEntity, uint> objBikeVersionsCache,
            IBikeSeriesCacheRepository seriesCache, IBikeSeries series, ICityCacheRepository objCityCache, IBikeInfo objGenericBike, IBikeMaskingCacheRepository<BikeModelEntity, int> modelMaskingCache)
            : base(objMakeCache, models, bikeModels, upcoming, series)
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
            _modelMaskingCache = modelMaskingCache;
            ProcessQueryString();
            ProcessCityArea();
        }

        public ExpertReviewsIndexPage(ICMSCacheContent cmsCache, IPager pager, IBikeModelsCacheRepository<int> models,
            IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming, IBikeMakesCacheRepository objMakeCache, IBikeVersionCacheRepository<BikeVersionEntity, uint> objBikeVersionsCache,
            IBikeSeriesCacheRepository seriesCache, IBikeSeries series, ICityCacheRepository objCityCache, IBikeInfo objGenericBike, IArticles articles, IPWACMSCacheRepository renderedArticles, IBikeMaskingCacheRepository<BikeModelEntity, int> modelMaskingCache)
            : base(objMakeCache, models, bikeModels, upcoming, series)
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
            _modelMaskingCache = modelMaskingCache;
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

                    #region Bind Editorial Widgets (Maintain order)

                    SetAdditionalVariables(objData);
                    objData.PageWidgets = base.GetEditorialWidgetData(widgetType);
                    
                    #endregion

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
                EditorialWidgetEntity editorialWidgetData = null;

                switch (currentPageType)
                {
                    case EditorialPageType.Default:
                        widgetType = EnumEditorialPageType.Listing;
                        editorialWidgetData = new EditorialWidgetEntity
                        {
                            IsMobile = IsMobile,
                            CityId = CityId
                        };
                        break;
                    
                    case EditorialPageType.ModelPage:
                        widgetType = EnumEditorialPageType.Detail;

                        editorialWidgetData = new EditorialWidgetEntity
                        {
                            IsMobile = IsMobile,
                            IsMakeLive = isMakeLive,
                            IsModelTagged = isModelTagged,
                            IsSeriesAvailable = isSeriesAvailable,
                            IsScooterOnlyMake = objMake.IsScooterOnly,
                            BodyStyle = BodyStyle,
                            CityId = CityId,
                            Make = objMake,
                            Series = isSeriesAvailable ? objModel.ModelSeries : null
                        };

                        break;

                    case EditorialPageType.MakePage:
                        widgetType = EnumEditorialPageType.MakeListing;

                        editorialWidgetData = new EditorialWidgetEntity
                        {
                            IsMobile = IsMobile,
                            IsMakeLive = isMakeLive,
                            IsScooterOnlyMake = objMake.IsScooterOnly,
                            BodyStyle = BodyStyle,
                            CityId = CityId,
                            Make = objMake
                        };

                        break;

                    case EditorialPageType.SeriesPage:
                        widgetType = EnumEditorialPageType.Detail;

                        editorialWidgetData = new EditorialWidgetEntity
                        {
                            IsMobile = IsMobile,
                            IsMakeLive = isMakeLive,
                            IsModelTagged = true,
                            IsSeriesAvailable = isSeriesAvailable,
                            IsScooterOnlyMake = objMake.IsScooterOnly,
                            BodyStyle = BodyStyle,
                            CityId = CityId,
                            Make = objMake,
                            Series = objSeries
                        };

                        break;
                    default:
                        break;
                }

                base.SetAdditionalData(editorialWidgetData);
               
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Models.News.ExpertReviewsIndexPage.SetAdditionalVariables");
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
                makeMaskingName = queryString["make"];
                maskingName = queryString["model"];

                if (!string.IsNullOrEmpty(maskingName))
                    currentPageType = EditorialPageType.ModelPage;
                else if (!string.IsNullOrEmpty(makeMaskingName))
                    currentPageType = EditorialPageType.MakePage;

                if (!string.IsNullOrEmpty(makeMaskingName))
                {
                    ProcessMakeMaskingName(request, makeMaskingName);
                    ProcessModelSeriesMaskingName(request, String.Format("{0}_{1}", makeMaskingName, maskingName));
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
                        isSeriesAvailable = true;
                    }
                    else
                    {
                        model = objResponse.MaskingName;
                        ModelId = objResponse.ModelId;
                        objModel = _modelMaskingCache.GetById((int) objResponse.ModelId);
                        isModelTagged = true;
                        isSeriesAvailable = objModel.ModelSeries.IsSeriesPageUrl;

                        List<BikeVersionMinSpecs> objVersionsList = _objBikeVersionsCache.GetVersionMinSpecs(ModelId, false);
                        if (objVersionsList != null && objVersionsList.Count > 0)
                        {
                            BodyStyle = objVersionsList.FirstOrDefault().BodyStyle;
                        }
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
                makeResponse = _objMakeCache.GetMakeMaskingResponse(make);
            }
            if (makeResponse != null)
            {
                if (makeResponse.StatusCode == 200)
                {
                    MakeId = makeResponse.MakeId;
                    objMake = _objMakeCache.GetMakeDetails(MakeId);
                    isMakeLive = objMake.IsNew && !objMake.IsFuturistic;
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
            objData.PageMetaTags.CanonicalUrl = string.Format("{0}{1}{2}", BWConfiguration.Instance.BwHostUrlForJs, UrlFormatter.FormatExpertReviewUrl(makeMaskingName, series, model), (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
            objData.PageMetaTags.AlternateUrl = string.Format("{0}/m{1}{2}", BWConfiguration.Instance.BwHostUrlForJs, UrlFormatter.FormatExpertReviewUrl(makeMaskingName, series, model), (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
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
                objData.PagerEntity.BaseUrl = string.Format("{0}{1}", (IsMobile ? "/m" : ""), UrlFormatter.FormatExpertReviewUrl(makeMaskingName, series, model));
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