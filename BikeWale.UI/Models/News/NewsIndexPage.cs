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
using Bikewale.Entities.PWA.Articles;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.EditCMS;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Pager;
using Bikewale.Interfaces.PWA.CMS;
using Bikewale.Models.BestBikes;
using Bikewale.Models.BikeModels;
using Bikewale.Models.Scooters;
using Bikewale.PWA.Utils;
using Bikewale.Utility;
using Newtonsoft.Json;
using Bikewale.Models.EditorialPages;
using Bikewale.Entities.EditorialWidgets;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 27 Mar 2017
    /// Summary    : Model to get data for news default page
    /// Modified by:Snehal Dange on 24 August,2017
    /// Description: Added _bikeMakesCacheRepository,_objVersion.
    ///              Added PopularScooterBrandsWidget
    /// Modified by : Ashutosh Sharma on 27 Nov 2017
    /// Description : Added IBikeSeriesCacheRepository and IBikeSeries for series news page.
    /// Modified by : Rajan Chauhan on 27 Feb 2017
    /// Description : changed CityName from public to private
    /// Modified by: Dhruv Joshi
    /// Dated: 16th April 2018
    /// Description: Added _pageId and _totalTabCount to page variables for generic info widget
    /// </summary>
    public class NewsIndexPage : EditorialBasePage
    {
        #region Variables for dependency injection and constructor
        private readonly ICMSCacheContent _cacheContent = null;
        private readonly IArticles _articles = null;
        private readonly IPWACMSCacheRepository _renderedArticles = null;
        private readonly IPager _pager = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private readonly IBikeMakesCacheRepository _objMakeCache = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IBikeVersions<BikeVersionEntity, uint> _objVersion;
        private readonly IBikeSeriesCacheRepository _seriesCache;
        private readonly IBikeSeries _series;
        private readonly ICityCacheRepository _objCityCache = null;
        private readonly IBikeInfo _objGenericBike = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _modelMaskingCache = null;
        #endregion

        #region Page level variables
        private uint MakeId, ModelId, pageCatId = 0, CityId;
        private const int pageSize = 10, pagerSlotSize = 5;
        private int curPageNo = 1;
        private uint _totalPagesCount;
        private string make = string.Empty, model = string.Empty, series = string.Empty;
        private GlobalCityAreaEntity currentCityArea = null;
        private string CityName;
        private BikeModelEntity objModel = null;
        private BikeMakeEntityBase objMake = null;
        private BikeSeriesEntityBase objSeries;
        private readonly uint _totalTabCount = 3;
        private BikeInfoTabType _pageId = BikeInfoTabType.News;
        private static string pageName = "Editorial List";
        private EditorialPageType currentPageType = EditorialPageType.Default;
        private EnumBikeBodyStyles BodyStyle = EnumBikeBodyStyles.AllBikes;
        private EnumEditorialPageType widgetType;
        private bool isMakeLive, isModelTagged, isSeriesAvailable;
        #endregion

        #region Public properties
        public bool IsMobile { get; set; }
        public string RedirectUrl { get; set; }
        public StatusCodes Status { get; set; }

        #endregion

        #region Constructor

        static string _newsContentType, _allContentTypes;
        static NewsIndexPage()
        {
            List<EnumCMSContentType> categoryList = new List<EnumCMSContentType>();
            categoryList.Add(EnumCMSContentType.News);
            categoryList.Add(EnumCMSContentType.AutoExpo2018);
            _newsContentType = CommonApiOpn.GetContentTypesString(categoryList);

            categoryList.Add(EnumCMSContentType.AutoExpo2016);
            categoryList.Add(EnumCMSContentType.Features);
            categoryList.Add(EnumCMSContentType.RoadTest);
            categoryList.Add(EnumCMSContentType.ComparisonTests);
            categoryList.Add(EnumCMSContentType.SpecialFeature);
            categoryList.Add(EnumCMSContentType.TipsAndAdvices);

            _allContentTypes = CommonApiOpn.GetContentTypesString(categoryList);
        }

        /// <summary>
        /// Modified by : Ashutosh Sharma on 27 Nov 2017
        /// Description : Added IBikeSeriesCacheRepository and IBikeSeries for series news page.
        /// Modified by : Ashutosh Sharma on 27 Dec 2017
        /// Description : Added call to ProcessCityArea.
        /// </summary>
        public NewsIndexPage(ICMSCacheContent cacheContent, IPager pager, IBikeMakesCacheRepository objMakeCache, IBikeModelsCacheRepository<int> models, IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming, IPWACMSCacheRepository renderedArticles,
            IBikeVersions<BikeVersionEntity, uint> objBikeVersions, IArticles articles, IBikeSeriesCacheRepository seriesCache,
            IBikeSeries series, ICityCacheRepository objCityCache, IBikeInfo objGenericBike, IBikeMaskingCacheRepository<BikeModelEntity, int> modelMaskingCache)
            : base(objMakeCache, models, bikeModels, upcoming, series)
        {
            _articles = articles;
            _cacheContent = cacheContent;
            _pager = pager;
            _models = models;
            _bikeModels = bikeModels;
            _upcoming = upcoming;
            _renderedArticles = renderedArticles;
            _objMakeCache = objMakeCache;
            _objVersion = objBikeVersions;
            _seriesCache = seriesCache;
            _series = series;
            _objCityCache = objCityCache;
            _objGenericBike = objGenericBike;
            _modelMaskingCache = modelMaskingCache;
            ProcessQueryString();
            ProcessCityArea();
        }

        #endregion

        #region Functions
        /// <summary>
        /// Created by : Ashutosh Sharma on 27 Dec 2017
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
                ErrorClass.LogError(ex, "Bikewale.Models.News.NewsIndexPage.ProcessCityArea");
            }
        }

        /// <summary>
        /// Created By : Aditi Srivastava on 27 Mar 2017
        /// Summary    : Get page data
        /// Modified by : Ashutosh Sharma on 27 Nov 2017
        /// Description : Added call to GetArticlesByCategoryList for news page of series.
        /// Modified by : Snehal Dange on 29th Nov 2017
        /// Descritpion : Added ga for page
        /// Modified by : Pratibha Verma on 25the January
        /// Description : Added AutoExpo2018 in news category
        /// Modified By : Deepak Israni on 20 April 2018
        /// Description : Bind data for page widgets.
        /// </summary>
        /// <returns></returns>
        public NewsIndexPageVM GetData(int widgetTopCount)
        {
            NewsIndexPageVM objData = new NewsIndexPageVM();
            objData.BodyStyle = this.BodyStyle;
            objData.EditorialPageType = this.currentPageType;

            try
            {
                string modelIds = string.Empty;
                int _startIndex = 0, _endIndex = 0;
                _pager.GetStartEndIndex(pageSize, curPageNo, out _startIndex, out _endIndex);

                string contentTypeList = (MakeId == 0 && ModelId == 0) ? _allContentTypes : _newsContentType;

                if (objMake != null)
                    objData.Make = objMake;
                if (objModel != null)
                    objData.Model = objModel;
                if (objSeries != null)
                    objData.Series = objSeries;
                if (objData.Series != null)
                {
                    modelIds = _series.GetModelIdsBySeries(objData.Series.SeriesId);
                    objData.Articles = _cacheContent.GetArticlesByCategoryList(contentTypeList, _startIndex, _endIndex, (int)MakeId, modelIds);
                }
                else
                {
                    objData.Articles = _cacheContent.GetArticlesByCategoryList(contentTypeList, _startIndex, _endIndex, (int)MakeId, (int)ModelId);
                }

                if (objData.Articles != null && objData.Articles.RecordCount > 0)
                {
                    _totalPagesCount = (uint)_pager.GetTotalPages((int)objData.Articles.RecordCount, pageSize);
                    int recordCount = (int)objData.Articles.RecordCount;
                    Status = StatusCodes.ContentFound;
                    objData.StartIndex = _startIndex;
                    objData.EndIndex = _endIndex > recordCount ? recordCount : _endIndex;
                    BindLinkPager(objData, recordCount);
                    CreatePrevNextUrl(objData, recordCount);

                    if (ModelId > 0)
                    {
                        BindBikeInfoWidget(objData);
                    }

                    SetPageMetas(objData);

                    #region Bind Editorial Widgets (Maintain order)
                    SetAdditionalVariables(objData);
                    objData.PageWidgets = base.GetEditorialWidgetData(widgetType);
                    #endregion
                    
                    objData.Page = Entities.Pages.GAPages.Editorial_List_Page;

                }
                else
                {
                    Status = StatusCodes.ContentNotFound;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.News.NewsIndexPage.GetData");
            }
            return objData;
        }

        /// <summary>
        /// Created By : Prasad Gawde on 25 May 2017
        /// Summary    : Get page data for PWA
        /// </summary>
        /// <returns></returns>
        public NewsIndexPageVM GetPwaData(int widgetTopCount)
        {
            NewsIndexPageVM objData = new NewsIndexPageVM();

            try
            {
				
                int _startIndex = 0, _endIndex = 0;
                _pager.GetStartEndIndex(pageSize, curPageNo, out _startIndex, out _endIndex);

                string contentTypeList = (MakeId == 0 && ModelId == 0) ? _allContentTypes : _newsContentType;

                if (objMake != null)
                    objData.Make = objMake;
                if (objModel != null)
                    objData.Model = objModel;

                //objData.Articles
                PwaContentBase pwaCmsContent = _articles.GetArticlesByCategoryListPwa(contentTypeList, _startIndex, _endIndex, (int)MakeId, (int)ModelId);


                if (pwaCmsContent != null && pwaCmsContent.RecordCount > 0)
                {
                    _totalPagesCount = (uint)_pager.GetTotalPages((int)pwaCmsContent.RecordCount, pageSize);
                    Status = StatusCodes.ContentFound;
                    int recordCount = (int)pwaCmsContent.RecordCount;

                    pwaCmsContent.StartIndex = (uint)_startIndex;
                    pwaCmsContent.EndIndex = (uint)(_endIndex > recordCount ? recordCount : _endIndex);
                    pwaCmsContent.PageTitle = "Bike News";
                    BindLinkPager(objData, recordCount); //needs the record count
                    SetPageMetas(objData); //needs nothing
                    CreatePrevNextUrl(objData, recordCount); // needs record count


                    SetAdditionalVariables(objData);
                    objData.PageWidgets = base.GetEditorialWidgetData(EnumEditorialPageType.Listing);

                    try
                    {
                        if ((objData.Model == null || string.IsNullOrEmpty(objData.Model.ModelName)) &&
                            (objData.Make == null || string.IsNullOrEmpty(objData.Make.MakeName)))
                        {
                            //setting the store for Redux
                            objData.ReduxStore = new PwaReduxStore();
                            objData.ReduxStore.News.NewsArticleListReducer.ArticleListData.ArticleList = pwaCmsContent;
                            if (objData.PageWidgets != null)
                            {
                                PopulateStoreForWidgetData(objData);
                            }

                            var storeJson = JsonConvert.SerializeObject(objData.ReduxStore);

                            objData.ServerRouterWrapper = _renderedArticles.GetNewsListDetails(PwaCmsHelper.GetSha256Hash(storeJson), objData.ReduxStore.News.NewsArticleListReducer,
                                "/m/news/", "root", "ServerRouterWrapper");
                            objData.WindowState = storeJson;
                            objData.Page = Entities.Pages.GAPages.Editorial_List_Page;
                        }
                    }
                    catch
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
                ErrorClass.LogError(ex, "Bikewale.Models.News.NewsIndexPage.GetPwaData");
            }
            return objData;
        }

        /// <summary>
        /// Modified by : Sanskar Gupta om 23 April 2018
        /// Description : Changed the widget population methodology, simplified it by calling a generic function and used the new `PageWidgets` (Dictionary) logic.
        /// </summary>
        /// <param name="objData"></param>
        private void PopulateStoreForWidgetData(NewsIndexPageVM objData)
        {
            objData.ReduxStore.News.NewsArticleListReducer.NewBikesListData.NewBikesList = ConverterUtility.MapPopularAndUpcomingWidgetDataToPwa(objData.PageWidgets);
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 27 Mar 2017
        /// Summary    : Process query string for news page
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
                string maskingName = queryString["model"];

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
        /// Created by  :  Aditi Srivasava on 27 Mar 2017
        /// Summary     :  Processes model masking name
        /// Modifies by : Ashutosh Sharma on 27 Nov 2017
        /// Description : Process series and model masking name, get news data of series if series page is created otherwise	model news. Changed method name from 'ProcessModelMaskingName' to 'ProcessModelSeriesMaskingName'
        /// </summary>
        private void ProcessModelSeriesMaskingName(HttpRequest request, string maskingName)
        {
            try
            {
                SeriesMaskingResponse objResponse = null;
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
                            objSeries = new BikeSeriesEntityBase
                            {
                                SeriesId = objResponse.SeriesId,
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
                            objModel = _modelMaskingCache.GetById((int)objResponse.ModelId);
                            isModelTagged = true;
                            isSeriesAvailable = objModel.ModelSeries.IsSeriesPageUrl;

                            IEnumerable<BikeVersionMinSpecs> objVersionsList = _objVersion.GetVersionMinSpecs(ModelId, false);
                            if (objVersionsList != null && objVersionsList.Any())
                            {
                                BodyStyle = objVersionsList.FirstOrDefault().BodyStyle;
                            }
                        }
                    }
                    else if (objResponse.StatusCode == 301)
                    {
                        Status = StatusCodes.RedirectPermanent;
                        RedirectUrl = request.RawUrl.Replace(objResponse.MaskingName, objResponse.NewMaskingName);
                    }
                    else
                    {
                        Status = StatusCodes.ContentNotFound;
                    }
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Models.News.NewsIndexPage.ProcessModelSeriesMaskingName");
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
                    Status = StatusCodes.RedirectPermanent;
                    RedirectUrl = request.RawUrl.Replace(make, makeResponse.MaskingName);
                }
                else
                {
                    Status = StatusCodes.ContentNotFound;
                }
            }
        }

        /// <summary>
        /// Created by  :  Aditi Srivasava on 28 Mar 2017
        /// Summary     :  Set page metas and headings
        /// /// Modified by :- Subodh Jain 19 june 2017
        /// Summary :- Added TargetModels and Target Make
        /// Modified by :- Snehal Dange 24 August 2017
        /// Summary :- Added code for BodyStyle.Scooters
        /// Modifies by : Ashutosh Sharma on 27 Nov 2017
        /// Description : Added logic for series news page.
        /// Modified by : Rajan Chauhan on 28 Dec 2017
        /// Description : Change in PageMetaTags Title and Description
        /// </summary>
        private void SetPageMetas(NewsIndexPageVM objData)
        {
            objData.PageMetaTags.CanonicalUrl = string.Format("{0}{1}{2}", BWConfiguration.Instance.BwHostUrl, UrlFormatter.FormatNewsUrl(make, series, model), (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));
            objData.PageMetaTags.AlternateUrl = string.Format("{0}/m{1}{2}", BWConfiguration.Instance.BwHostUrl, UrlFormatter.FormatNewsUrl(make, series, model), (curPageNo > 1 ? string.Format("page/{0}/", curPageNo) : ""));

            EnumBikeBodyStyles bodyStyle = EnumBikeBodyStyles.AllBikes;

            if (this.currentPageType == EditorialPageType.SeriesPage && objMake != null)
            {
                bodyStyle = objData.Series.BodyStyle;
                string bodyStyleText = Bikewale.Utility.BodyStyleLinks.BodyStyleHeadingText(bodyStyle);
                objData.PageMetaTags.Title = string.Format("Latest news about all {0} {1} {2} | {0} {1} news - BikeWale", objMake.MakeName, objData.Series.SeriesName, objData.Series.BodyStyle == EnumBikeBodyStyles.Scooter ? "Scooters" : "Bikes");
                objData.PageMetaTags.Description = String.Format("Read the latest news about all {0} {1} {2} on BikeWale. Catch up on the latest buzz around {0} {1}", objMake.MakeName, objData.Series.SeriesName, objData.Series.BodyStyle == EnumBikeBodyStyles.Scooter ? "Scooters" : "Bikes");
                objData.PageMetaTags.Keywords = string.Format("News about {0} {1}, {0} {1} News", objMake.MakeName, objData.Series.SeriesName);
                objData.PageH1 = string.Format("{0} {1} News", objMake.MakeName, objData.Series.SeriesName);
                objData.PageH2 = string.Format("Latest {0} {1} {2} News and Views", objMake.MakeName, objData.Series.SeriesName, bodyStyleText);
                objData.AdTags.TargetedSeries = objData.Series.SeriesName;
            }
            else if (ModelId > 0)
            {
                IEnumerable<BikeVersionMinSpecs> objVersionsList = _objVersion.GetVersionMinSpecs(ModelId, false);

                if (objVersionsList != null && objVersionsList.Any())
                    bodyStyle = objVersionsList.FirstOrDefault().BodyStyle;

                if (bodyStyle.Equals(EnumBikeBodyStyles.Scooter))
                {
                    if (BWConfiguration.Instance.MetasMakeId.Split(',').Contains(MakeId.ToString()))
                    {
                        objData.PageMetaTags.Title = string.Format("News Updates on {0} {1} | News About {0} {1}- BikeWale", objMake.MakeName, objModel.ModelName);
                        objData.PageH1 = string.Format("Latest News about {0} {1}", objMake.MakeName, objModel.ModelName);
                    }
                    else
                    {
                        objData.PageMetaTags.Title = string.Format("Latest News about {0} {1} | {0} {1} News - BikeWale", objMake.MakeName, objModel.ModelName);
                        objData.PageH1 = string.Format("{0} {1} Scooters News", objMake.MakeName, objModel.ModelName);
                        objData.PageH2 = string.Format("Latest {0} {1} Scooters News and Views", objMake.MakeName, objModel.ModelName);
                    }


                    objData.PageMetaTags.Description = String.Format("Read the latest news about {0} {1} scooters exclusively on BikeWale. Know more about {1}.", objMake.MakeName, objModel.ModelName);


                    objData.AdTags.TargetedMakes = objMake.MakeName;
                    objData.AdTags.TargetedModel = objModel.ModelName;
                }
                else
                {
                    if (BWConfiguration.Instance.MetasMakeId.Split(',').Contains(MakeId.ToString()))
                    {
                        objData.PageMetaTags.Title = string.Format("News Updates on {0} {1} | News About {0} {1}- BikeWale", objMake.MakeName, objModel.ModelName);
                        objData.PageH1 = string.Format("Latest News about {0} {1}", objMake.MakeName, objModel.ModelName);
                    }
                    else
                    {
                        objData.PageMetaTags.Title = string.Format("Latest News about {0} {1} | {0} {1} News - BikeWale", objMake.MakeName, objModel.ModelName);
                        objData.PageH1 = string.Format("{0} {1} Bikes News", objMake.MakeName, objModel.ModelName);
                        objData.PageH2 = string.Format("Latest {0} {1} Bikes News and Views", objMake.MakeName, objModel.ModelName);
                    }

                    objData.PageMetaTags.Description = String.Format("Read the latest news about {0} {1} bikes exclusively on BikeWale. Know more about {1}.", objMake.MakeName, objModel.ModelName);


                    objData.AdTags.TargetedMakes = objMake.MakeName;
                    objData.AdTags.TargetedModel = objModel.ModelName;
                }


            }
            else if (MakeId > 0 && objMake != null)
            {
                if (BWConfiguration.Instance.MetasMakeId.Split(',').Contains(MakeId.ToString()))
                {
                    objData.PageMetaTags.Title = string.Format("News Updates on {0} Bikes | News About {0} Models- BikeWale", objMake.MakeName);
                    objData.PageH1 = string.Format("Latest News about {0} Bikes", objMake.MakeName);
                }
                else
                {
                    objData.PageMetaTags.Title = string.Format("Latest News about {0} Bikes | {0} Bikes News - BikeWale", objMake.MakeName);
                    objData.PageH1 = string.Format("{0} Bikes News", objMake.MakeName);
                    objData.PageH2 = string.Format("Latest {0} Bikes News and Views", objMake.MakeName);
                }

                objData.PageMetaTags.Description = String.Format("Read the latest news about popular and upcoming {0} bikes exclusively on BikeWale. Know more about {0} bikes.", objMake.MakeName);


                objData.AdTags.TargetedMakes = objMake.MakeName;
            }
            else
            {
                objData.PageMetaTags.Title = "Bike News - Latest Indian Bike News & Views | BikeWale";
                objData.PageMetaTags.Description = "Latest news updates on Indian bikes industry, expert views and interviews exclusively on BikeWale.";
                objData.PageMetaTags.Keywords = "news, bike news, auto news, latest bike news, indian bike news, bike news of india";
                objData.PageH1 = "Bike News";
                objData.PageH2 = "Latest Indian Bikes News and Views";
            }

            if (curPageNo > 1)
            {
                objData.PageMetaTags.Description = string.Format("Page {0} of {1} - {2}", curPageNo, _totalPagesCount, objData.PageMetaTags.Description);
                objData.PageMetaTags.Title = string.Format("Page {0} of {1} - {2}", curPageNo, _totalPagesCount, objData.PageMetaTags.Title);
            }

            if (objData.Model != null)
            {
                objData.Series = _models.GetSeriesByModelId(ModelId);
            }

            SetBreadcrumList(objData, bodyStyle);
            if (bodyStyle.Equals(EnumBikeBodyStyles.Scooter))
            {
                BindMoreAboutScootersWidget(objData);
            }
        }

        /// <summary>
        /// Created By : Aditi Srivastava on 27 Mar 2017
        /// Summary    : Bind link pager
        /// Modifies by : Ashutosh Sharma on 27 Nov 2017
        /// Description : Added logic for series news page.
        /// </summary>
        private void BindLinkPager(NewsIndexPageVM objData, int recordCount)
        {
            try
            {
                objData.PagerEntity = new PagerEntity();
                if (objData.Series != null && objData.Series.IsSeriesPageUrl)
                {
                    objData.PagerEntity.BaseUrl = string.Format("{0}{1}", (IsMobile ? "/m" : ""), UrlFormatter.FormatNewsUrl(make, objData.Series.MaskingName));
                }
                else
                {
                    objData.PagerEntity.BaseUrl = string.Format("{0}{1}", (IsMobile ? "/m" : ""), UrlFormatter.FormatNewsUrl(make, model));
                }
                objData.PagerEntity.PageNo = curPageNo;
                objData.PagerEntity.PagerSlotSize = pagerSlotSize;
                objData.PagerEntity.PageUrlType = "page/";
                objData.PagerEntity.TotalResults = recordCount;
                objData.PagerEntity.PageSize = pageSize;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Models.News.NewsIndexPage.BindLinkPager");
            }
        }

        /// <summary>
        /// Created By : Aditi Srivastava on 29 Mar 2017
        /// Summary    : Create previous and next page urls
        /// </summary>
        /// <param name="objData"></param>
        private void CreatePrevNextUrl(NewsIndexPageVM objData, int recordCount)
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

        /// <summary>
        /// Created By : Snehal Dange on 12th Oct 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList(NewsIndexPageVM objData, EnumBikeBodyStyles bodyStyle)
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

                if (objData.Make != null && bodyStyle.Equals(EnumBikeBodyStyles.Scooter) && !objData.Make.IsScooterOnly)
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

                BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, "News"));

                objData.BreadcrumbList.BreadcrumListItem = BreadCrumbs;

            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "Exception : Bikewale.Models.News.NewsIndexPage.SetBreadcrumList");
            }

        }

        /// <summary>
        /// Created By: Snehal Dange on 21th Dec 2017
        /// Summary : Bind more about scooter widget
        /// </summary>
        /// <param name="objData"></param>
        private void BindMoreAboutScootersWidget(NewsIndexPageVM objData)
        {
            try
            {
                MoreAboutScootersWidget obj = new MoreAboutScootersWidget(_models, _objCityCache, _objVersion, _objGenericBike, Entities.GenericBikes.BikeInfoTabType.News);
                obj.modelId = ModelId;
                objData.ObjMoreAboutScooter = obj.GetData();
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("Bikewale.Models.News.NewsIndexPage..BindMoreAboutScootersWidget : ModelId {0}", ModelId));
            }
        }

        /// <summary>
        /// Created By  : Sanskar Gupta on 12 April 2018
        /// Description : Function to set additional Page level variables.
        /// </summary>
        /// <param name="objData">VM of the page.</param>
        private void SetAdditionalVariables(NewsIndexPageVM objData)
        {
            try
            {
                objData.PageName = pageName;
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

                        widgetType = isMakeLive ? EnumEditorialPageType.MakeListing : EnumEditorialPageType.Listing;

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
                Bikewale.Notifications.ErrorClass.LogError(ex, "Bikewale.Models.News.NewsIndexPage.SetAdditionalVariables");
            }
        }

        private void BindBikeInfoWidget(NewsIndexPageVM objData)
        {
            BikeInfoWidget genericInfoWidget = new BikeInfoWidget(_objGenericBike, _objCityCache, ModelId, CityId, _totalTabCount, _pageId);
            objData.GenericBikeInfoWidget = genericInfoWidget.GetData();
        }
        #endregion
    }
}