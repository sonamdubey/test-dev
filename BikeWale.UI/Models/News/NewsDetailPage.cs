using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
using Bikewale.Entities.Pages;
using Bikewale.Entities.PriceQuote;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Entities.Schema;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PWA.CMS;
using Bikewale.Memcache;
using Bikewale.Models.BestBikes;
using Bikewale.Models.BikeMakes;
using Bikewale.Models.BikeModels;
using Bikewale.Models.Scooters;
using Bikewale.Models.Shared;
using Bikewale.Notifications;
using Bikewale.PWA.Utils;
using Bikewale.Utility;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Bikewale.Models.EditorialPages;
using Bikewale.Entities.EditorialWidgets;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 29 Mar 2017
    /// Summary    : Model for news details page
    /// Modified by : Snehal Dange on 24 August,2017
    /// Description : Added _bikeMakesCacheRepository,_objBikeVersionsCache.
    ///               Added PopularScooterBrandsWidget
    /// </summary>
    [System.Runtime.InteropServices.Guid("3C5A3C0F-8084-46E7-BE75-F7FDFA9BBB60")]
    public class NewsDetailPage : EditorialBasePage
    {
        #region Variables for dependency injection
        private readonly ICMSCacheContent _cmsCache = null;
        private IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private IBikeModelsCacheRepository<int> _models = null;
        private readonly IBikeInfo _bikeInfo;
        private readonly ICityCacheRepository _cityCacheRepo;
        private IUpcoming _upcoming = null;
        private string _basicId;
        private readonly IPWACMSCacheRepository _renderedArticles = null;
        private readonly IBikeMakesCacheRepository _bikeMakesCacheRepository = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objBikeVersionsCache = null;
        private readonly IBikeSeriesCacheRepository _seriesCache = null;
        private readonly IBikeSeries _series;


        #endregion

        #region Page level variables
        public StatusCodes status;
        public string mappedCWId;
        public string redirectUrl;
        private string CityName;
        private GlobalCityAreaEntity currentCityArea;
        private uint CityId, MakeId, ModelId, pageCatId = 0;
        private readonly uint _totalTabCount = 3;
        private BikeInfoTabType _pageId = BikeInfoTabType.News;
        //private EnumBikeType bikeType = EnumBikeType.All;
        private readonly bool showCheckOnRoadCTA = false;
        private uint basicId, _versionId;
        private PQSourceEnum pqSource = 0;
        public BikeSeriesEntityBase bikeSeriesEntityBase;
        private static string _widgetCruiserSports;
        private static string pageName = "Editorial Details";

        private bool isModelTagged;
        private bool isMakeTagged;
        private bool isMakeLive;
        private string MakeName, MakeMaskingName;
        private EnumBikeBodyStyles bodyStyle;
        private bool isSeriesAvailable;
        private bool isScooterOnlyMake;
        #endregion

        #region Public properties
        public bool IsMobile { get; set; }

        static ILog _logger = LogManager.GetLogger("NewsDetailPage");

        public bool IsAMPPage { get; set; }
        #endregion
        
        #region Constructor
        public NewsDetailPage(ICMSCacheContent cmsCache, IBikeMakesCacheRepository bikeMakesCacheRepository, IBikeModelsCacheRepository<int> models, IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming, IBikeInfo bikeInfo, ICityCacheRepository cityCacheRepo, string basicId, IPWACMSCacheRepository renderedArticles, IBikeVersionCacheRepository<BikeVersionEntity, uint> objBikeVersionsCache,
            IBikeSeriesCacheRepository seriesCache, IBikeSeries series):base(bikeMakesCacheRepository, models, bikeModels, upcoming, series)
        {
            _cmsCache = cmsCache;
            _models = models;
            _bikeModels = bikeModels;
            _upcoming = upcoming;
            _bikeInfo = bikeInfo;
            _cityCacheRepo = cityCacheRepo;
            _basicId = basicId;
            _renderedArticles = renderedArticles;
            _bikeMakesCacheRepository = bikeMakesCacheRepository;
            _objBikeVersionsCache = objBikeVersionsCache;
            _seriesCache = seriesCache;
            _series = series;
            ProcessCityArea();
            ProcessQueryString();

        }

        static NewsDetailPage()
        {

            IList<EnumBikeBodyStyles> bodyStyles = new List<EnumBikeBodyStyles>();
            bodyStyles.Add(EnumBikeBodyStyles.Cruiser);
            bodyStyles.Add(EnumBikeBodyStyles.Sports);

            _widgetCruiserSports = CommonApiOpn.GetContentTypesString(bodyStyles);
        }

        /// <summary>
        /// Created by  : Rajan Chauhan on 26 Feb 2018
        /// Description : To set currentCityArea, CityName and      
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
                ErrorClass.LogError(ex, "Bikewale.Models.News.NewsDetailPage.ProcessCityArea");
            }
        }

        private void CheckSeriesData(NewsDetailPageVM objdata)
        {
            try
            {
                bikeSeriesEntityBase = _models.GetSeriesByModelId(ModelId);
                if (null != bikeSeriesEntityBase && bikeSeriesEntityBase.IsSeriesPageUrl)
                {
                    objdata.IsSeriesAvailable = true;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.NewsDetailPage.CheckSeriesData");
            }
        }
        #endregion

        #region Functions
        /// <summary>
        /// Created by : Aditi Srivastava on 29 Mar 2017
        /// Summary    : Process query string
        /// </summary>
        private void ProcessQueryString()
        {
            var request = HttpContext.Current.Request;
            string qsBasicId = _basicId;
            try
            {
                qsBasicId = BasicIdMapping.GetCWBasicId(qsBasicId);
                if (!qsBasicId.Equals(_basicId) && !String.IsNullOrEmpty(request["t"]))
                {
                    status = StatusCodes.RedirectPermanent;
                    mappedCWId = qsBasicId;
                    string url = request["t"];

                    if (url.Contains(string.Format("/{0}-", mappedCWId)) ||
                        url.StartsWith(@"/news/") ||
                        url.StartsWith(@"/m/news/") ||
                        url.EndsWith(@".html"))
                    {
                        redirectUrl = url;
                        ThreadContext.Properties["RedirectUrl"] = request.UrlReferrer != null ? request.UrlReferrer.ToString() : "Unknown";
                        ThreadContext.Properties["request(t)"] = request["t"];
                        ThreadContext.Properties["ReceivedURL"] = url;
                        _logger.Error("NewsDetailPage.ProcessQueryString()");
                    }
                    else
                        redirectUrl = string.Format("{0}/news/{1}-{2}.html", BWConfiguration.Instance.BwHostUrl, mappedCWId, request["t"]);
                }
                else if (uint.TryParse(qsBasicId, out basicId) && basicId > 0)
                    status = StatusCodes.ContentFound;
                else
                    status = StatusCodes.ContentNotFound;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.NewsDetailPage.ProcessQueryString");
            }

        }

        /// <summary>
        /// Created by : Aditi Srivastava on 29 Mar 2017
        /// Summary    : Get entire data for news details page
        /// Modified by : Ashutosh Sharma on 27 Oct 2017
        /// Description : Added call to BindAmpJsTags.
        /// Modified by : snehal Dange on 29th Nov 2017
        /// Descritpion : Added ga for page
        /// </summary>
        public NewsDetailPageVM GetData(int widgetTopCount)
        {
            NewsDetailPageVM objData = new NewsDetailPageVM();
            try
            {
                objData.ArticleDetails = _cmsCache.GetNewsDetails(basicId);

                if (objData.ArticleDetails != null)
                {
                    status = StatusCodes.ContentFound;
                    GetTaggedBikeListByMake(objData);
                    GetTaggedBikeListByModel(objData);
                    CheckSeriesData(objData);

                    BindBikeInfoWidget(objData);
                    SetAdditionalVariables(objData);

                    objData.PageWidgets = base.GetEditorialWidgetData(EnumEditorialPageType.Detail);
                    
                    BindSimilarBikes(objData);
                    SetPageMetas(objData);

                    if (objData.Model != null && ModelId != 0 && objData.Model.ModelId != ModelId)
                        objData.Model.ModelId = (int)ModelId;
                    if (IsAMPPage)
                    {
                        BindAmpJsTags(objData);
                    }
                    objData.Page = GAPages.Editorial_Details_Page;
                }
                else
                    status = StatusCodes.ContentNotFound;
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.Models.NewsDetailPage.GetData");
            }
            return objData;
        }

        /// <summary>
        /// Created By  : Sanskar Gupta on 12 April 2018
        /// Description : Function to set additional Page level variables.
        /// </summary>
        /// <param name="objData">VM of the page.</param>
        private void SetAdditionalVariables(NewsDetailPageVM objData)
        {
            objData.PageName = pageName;
            isMakeLive = !(objData.BikeInfo != null && (objData.BikeInfo.IsUpcoming || objData.BikeInfo.IsDiscontinued));

            bodyStyle = EnumBikeBodyStyles.AllBikes;

            List<BikeVersionMinSpecs> objVersionsList = _objBikeVersionsCache.GetVersionMinSpecs(ModelId, false);

            if (objVersionsList != null && objVersionsList.Count > 0)
                bodyStyle = objVersionsList.FirstOrDefault().BodyStyle;

            objData.BodyStyle = bodyStyle;

            isSeriesAvailable = objData.IsSeriesAvailable;
            if (objData.Make != null)
            {
                isScooterOnlyMake = objData.Make.IsScooterOnly;
            }
            EditorialWidgetEntity editorialWidgetData = new EditorialWidgetEntity
            {
                IsMobile = IsMobile,
                IsMakeLive = isMakeLive,
                IsModelTagged = isModelTagged,
                IsSeriesAvailable = isSeriesAvailable,
                IsScooterOnlyMake = isScooterOnlyMake,
                BodyStyle = bodyStyle,
                CityId = CityId,
                Make = objData.Make,
                Series = bikeSeriesEntityBase
            };
            base.SetAdditionalData(editorialWidgetData);
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 27 Oct 2017
        /// Description : Method to bind required JS for AMP page.
        /// </summary>
        /// <param name="objData"></param>
        private void BindAmpJsTags(NewsDetailPageVM objData)
        {
            try
            {
                objData.AmpJsTags = new Entities.Models.AmpJsTags();
                objData.AmpJsTags.IsAccordion = true;
                objData.AmpJsTags.IsAd = true;
                objData.AmpJsTags.IsBind = true;
                objData.AmpJsTags.IsCarousel = true;
                objData.AmpJsTags.IsSidebar = true;
                objData.AmpJsTags.IsSocialShare = true;
                objData.AmpJsTags.IsIframe = objData.ArticleDetails != null && objData.ArticleDetails.Content.Contains("<iframe");
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("BindAmpJsTags_{0}", objData));
            }
        }


        /// <summary>
        /// Created By : Prasad Gawde on 25 May 2017
        /// Summary    : Get page data for PWA
        /// Modified by : Snehal Dange on 29th Nov 2017
        /// Descritpion : Added ga for page
        /// Modified by : Sanskar Gupta on 20 April 2018
        /// Description : Added call for `SetAddtionalVariables()`, populate `objData.PageWidgets` and changed method call to populate `newsDetailReducer.NewBikesListData.NewBikesList` and `newsDetailReducer.NewBikesListData.BikeMakeList`
        /// </summary>
        /// <returns></returns>
        public NewsDetailPageVM GetPwaData(int widgetTopCount)
        {
            NewsDetailPageVM objData = new NewsDetailPageVM();
            try
            {
                objData.ArticleDetails = _cmsCache.GetNewsDetails(basicId);

                if (objData.ArticleDetails != null)
                {
                    status = StatusCodes.ContentFound;
                    GetTaggedBikeListByMake(objData);
                    GetTaggedBikeListByModel(objData);
                    CheckSeriesData(objData);

                    SetAdditionalVariables(objData);
                    objData.PageWidgets = base.GetEditorialWidgetData(EnumEditorialPageType.Detail);
                    
                    SetPageMetas(objData);

                    if (objData.Model != null && ModelId != 0 && objData.Model.ModelId != ModelId)
                        objData.Model.ModelId = (int)ModelId;

                    objData.ReduxStore = new PwaReduxStore();

                    var newsDetailReducer = objData.ReduxStore.News.NewsDetailReducer;
                    newsDetailReducer.ArticleDetailData.ArticleDetail = ConverterUtility.MapArticleDetailsToPwaArticleDetails(objData.ArticleDetails);
                    newsDetailReducer.RelatedModelObject.ModelObject = ConverterUtility.MapGenericBikeInfoToPwaBikeInfo(objData.BikeInfo);

                    if (objData.PageWidgets != null)
                    {
                        newsDetailReducer.NewBikesListData.NewBikesList = ConverterUtility.MapPopularAndUpcomingWidgetDataToPwa(objData.PageWidgets);
                        newsDetailReducer.NewBikesListData.BikeMakeList = ConverterUtility.MapOtherBrandsWidgetDataToPWA(objData.PageWidgets); 
                    }
                    var storeJson = JsonConvert.SerializeObject(objData.ReduxStore);

                    objData.ServerRouterWrapper = _renderedArticles.GetNewsDetails(PwaCmsHelper.GetSha256Hash(storeJson), objData.ReduxStore.News.NewsDetailReducer,
                                newsDetailReducer.ArticleDetailData.ArticleDetail.ArticleUrl, "root", "ServerRouterWrapper", "News");
                    objData.WindowState = storeJson;
                    objData.Page = GAPages.Editorial_Details_Page;
                }
                else
                    status = StatusCodes.ContentNotFound;
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.Models.NewsDetailPage.GetData");
            }
            return objData;
        }
        /// <summary>
        /// Created By :- Subodh Jain 13-12-2017
        /// Description :- Bind Similar Bikes Only for desktop
        /// </summary>
        /// <param name="objData"></param>
        private void BindSimilarBikes(NewsDetailPageVM objData)
        {
            try
            {
                if (objData.Model != null && objData.Model.ModelId > 0)
                {
                    var objSimilarBikes = new SimilarBikesWidget(_objBikeVersionsCache, (uint)objData.Model.ModelId, true, PQSourceEnum.Desktop_NewsDetailsPage);

                    objSimilarBikes.TopCount = 9;
                    objSimilarBikes.CityId = CityId;
                    objSimilarBikes.IsNew = objData.BikeInfo != null && (objData.BikeInfo.IsUpcoming || objData.BikeInfo.IsDiscontinued) ? false : true;
                    objSimilarBikes.IsUpcoming = objData.BikeInfo != null ? objData.BikeInfo.IsUpcoming : false;
                    objSimilarBikes.IsDiscontinued = objData.BikeInfo != null ? objData.BikeInfo.IsDiscontinued : false;
                    objData.SimilarBikes = objSimilarBikes.GetData();
                    if (objData.SimilarBikes != null && objData.SimilarBikes.Bikes != null && objData.SimilarBikes.Bikes.Any())
                    {
                        objData.SimilarBikes.Make = objData.Make;
                        objData.SimilarBikes.Model = objData.Model;
                        objData.SimilarBikes.VersionId = _versionId;
                        objData.SimilarBikes.Page = GAPages.Editorial_Details_Page;
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Bikewale.Models.NewsDetailPage.BindSimilarBikes({0})", objData.Model.ModelId));
            }
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 29 Mar 2017
        /// Summary    : Set news details page metas
        /// Modified by :- Subodh Jain 19 june 2017
        /// Summary :- Added TargetModels and Target Make
        /// </summary>
        private void SetPageMetas(NewsDetailPageVM objData)
        {
            try
            {
                objData.BaseUrl = IsMobile ? "/m" : "";
                objData.PageMetaTags.Title = string.Format("{0} - BikeWale", objData.ArticleDetails.Title);
                if (objData.Make != null)
                    objData.AdTags.TargetedMakes = objData.Make.MakeName;
                if (objData.Model != null)
                    objData.AdTags.TargetedModel = objData.Model.ModelName;
                objData.PageMetaTags.ShareImage = Image.GetPathToShowImages(objData.ArticleDetails.OriginalImgUrl, objData.ArticleDetails.HostUrl, ImageSize._640x348);
                objData.PageMetaTags.Description = string.Format("BikeWale coverage on {0}. Get the latest reviews and photos for {0} on BikeWale coverage.", objData.ArticleDetails.Title);
                objData.PageMetaTags.CanonicalUrl = string.Format("https://www.bikewale.com/news/{0}-{1}.html", objData.ArticleDetails.BasicId, objData.ArticleDetails.ArticleUrl);
                objData.PageMetaTags.AlternateUrl = string.Format("https://www.bikewale.com/m/news/{0}-{1}.html", objData.ArticleDetails.BasicId, objData.ArticleDetails.ArticleUrl);
                objData.PageMetaTags.AmpUrl = string.Format("{0}/m/news/{1}-{2}/amp/", BWConfiguration.Instance.BwHostUrl, objData.ArticleDetails.ArticleUrl, objData.ArticleDetails.BasicId);
                if (objData.ArticleDetails.PrevArticle != null && objData.ArticleDetails.PrevArticle.ArticleUrl != null)
                    objData.PageMetaTags.PreviousPageUrl = string.Format("{0}{1}/news/{2}-{3}.html", BWConfiguration.Instance.BwHostUrl, objData.BaseUrl, objData.ArticleDetails.PrevArticle.BasicId, objData.ArticleDetails.PrevArticle.ArticleUrl);
                if (objData.ArticleDetails.NextArticle != null && objData.ArticleDetails.NextArticle.ArticleUrl != null)
                    objData.PageMetaTags.NextPageUrl = string.Format("{0}{1}/news/{2}-{3}.html", BWConfiguration.Instance.BwHostUrl, objData.BaseUrl, objData.ArticleDetails.NextArticle.BasicId, objData.ArticleDetails.NextArticle.ArticleUrl);
                objData.Page_H1 = objData.ArticleDetails.Title;

                SetBreadcrumList(objData);
                SetPageJSONSchema(objData);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.NewsDetailPage.SetPageMetas");
            }
        }


        /// <summary>
        /// Created By : Sushil Kumar on 12th Sep 2017
        /// Description : Function to create page level schema for breadcrum
        /// </summary>
        private void SetBreadcrumList(NewsDetailPageVM objData)
        {
            IList<BreadcrumbListItem> BreadCrumbs = new List<BreadcrumbListItem>();
            string url = string.Format("{0}/", Utility.BWConfiguration.Instance.BwHostUrl);
            ushort position = 1;
            if (IsMobile)
            {
                url += "m/";
            }

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position++, url, "Home"));
            url += "news/";
            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, url, "Bike News"));

            BreadCrumbs.Add(SchemaHelper.SetBreadcrumbItem(position, null, objData.Page_H1));

            objData.BreadcrumbList.BreadcrumListItem = BreadCrumbs;

        }


        /// <summary>
        /// Created By  : Sushil Kumar on 25th Aug 2017
        /// Description : To load json schema for the news articles
        /// </summary>
        /// <param name="objData"></param>
        private void SetPageJSONSchema(NewsDetailPageVM objData)
        {
            var objSchema = new NewsArticle();
            objSchema.HeadLine = objData.ArticleDetails.Title;
            objSchema.DateModified = objData.ArticleDetails.DisplayDate.ToString();
            objSchema.DatePublished = objSchema.DateModified;
            objSchema.Description = FormatDescription.SanitizeHtml(objData.ArticleDetails.Description);
            objSchema.ArticleBody = Bikewale.Utility.FormatDescription.SanitizeHtml(objData.ArticleDetails.Content);
            objSchema.ArticleImage = new ImageObject()
            {
                ImageUrl = objData.PageMetaTags.ShareImage,
                Height = "348",
                Width = "640"
            };
            objSchema.Author = new Author()
            {
                Name = objData.ArticleDetails.AuthorName
            };
            objSchema.MainEntityOfPage = new MainEntityOfPage() { PageUrlId = objData.PageMetaTags.CanonicalUrl };
            objSchema.Url = objData.PageMetaTags.CanonicalUrl;
            objData.PageMetaTags.PageSchemaJSON = Newtonsoft.Json.JsonConvert.SerializeObject(objSchema);

            //set webpage schema for the model page
            WebPage webpage = SchemaHelper.GetWebpageSchema(objData.PageMetaTags, objData.BreadcrumbList);

            if (webpage != null)
            {
                objData.PageMetaTags.SchemaJSON = SchemaHelper.JsonSerialize(webpage);
            }

        }

        /// <summary>
        /// Created by : Aditi Srivastava on 29 Mar 2017
        /// Summary    : Get tagged make in article
        /// </summary>
        private void GetTaggedBikeListByMake(NewsDetailPageVM objData)
        {
            try
            {
                if (objData.ArticleDetails.VehiclTagsList != null && objData.ArticleDetails.VehiclTagsList.Any())
                {

                    var taggedMakeObj = objData.ArticleDetails.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.MakeBase.MaskingName));

                    if (taggedMakeObj != null)
                    {
                        objData.Make = taggedMakeObj.MakeBase;
                        _versionId = taggedMakeObj.VersionBase != null ? (uint)taggedMakeObj.VersionBase.VersionId : 0;
                    }
                    else
                    {
                        objData.Make = objData.ArticleDetails.VehiclTagsList.FirstOrDefault().MakeBase;
                        _versionId = objData.ArticleDetails.VehiclTagsList.FirstOrDefault().VersionBase != null ? (uint)objData.ArticleDetails.VehiclTagsList.FirstOrDefault().VersionBase.VersionId : 0;
                        if (objData.Make != null)
                            objData.Make = new Common.MakeHelper().GetMakeNameByMakeId((uint)objData.Make.MakeId);
                    }
                    BikeMakeEntityBase Make = objData.Make;
                    MakeId = (uint)Make.MakeId;
                    isMakeTagged = MakeId > 0;
                    MakeName = Make.MakeName;
                    MakeMaskingName = Make.MaskingName;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.NewsDetailPage.GetTaggedBikeListByMake");
            }
        }


        /// <summary>
        /// Created by : Aditi Srivastava on 29 Mar 2017
        /// Summary    : Get tagged model in article
        /// </summary>
        private void GetTaggedBikeListByModel(NewsDetailPageVM objData)
        {
            try
            {
                if (objData.ArticleDetails.VehiclTagsList != null && objData.ArticleDetails.VehiclTagsList.Any())
                {

                    var taggedModelObj = objData.ArticleDetails.VehiclTagsList.FirstOrDefault(m => !string.IsNullOrEmpty(m.ModelBase.MaskingName));
                    if (taggedModelObj != null)
                    {
                        objData.Model = taggedModelObj.ModelBase;
                    }
                    else
                    {
                        objData.Model = objData.ArticleDetails.VehiclTagsList.FirstOrDefault().ModelBase;
                        if (objData.Model != null)
                            objData.Model = new Bikewale.Common.ModelHelper().GetModelDataById((uint)objData.Model.ModelId);
                    }
                    ModelId = (uint)objData.Model.ModelId;
                    isModelTagged = ModelId > 0;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.NewsDetailPage.GetTaggedBikeListByModel");
            }
        }

        /// <summary>
        /// Fetches the bikes by body style.
        /// Modified by : Ashutosh Sharma on 27 Dec 2017
        /// Description : Added cityId to fetch bikes with city price.
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="bodyStyle">The body style.</param>
        private void FetchBikesByBodyStyle(NewsDetailPageVM objData, EnumBikeBodyStyles bodyStyle)
        {
            objData.SeriesWidget = new EditorialSeriesWidgetVM();
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
                IEnumerable<BestBikeEntityBase> bestBikesByBodyStyle = _models.GetBestBikesByCategory(bodyStyle, CityId);
                if (bestBikesByBodyStyle != null && bestBikesByBodyStyle.Any())
                {
                    objData.SeriesWidget.PopularBikesByBodyStyle = bestBikesByBodyStyle.Take(6);
                }

                objData.SeriesWidget.PopularSeriesBikes = FetchPopularSeriesBikes(bikeSeriesEntityBase.SeriesId);
                objData.SeriesWidget.SeriesName = bikeSeriesEntityBase.SeriesName;
                objData.SeriesWidget.WidgetLink = string.Format("/{0}-bikes/{1}/", objData.Make.MaskingName, bikeSeriesEntityBase.MaskingName);
                objData.SeriesWidget.WidgetLinkTitle = string.Format("View all {0} {1}", bikeSeriesEntityBase.SeriesName, "bikes");
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Models.News.NewsIndexPage.FetchBikesByBodyStyle");
            }
        }
        

        /// <summary>
        /// Binds the series bikes and model body style bikes.
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="bikeSeriesEntityBase">The bike series entity base.</param>
        /// <param name="bodyStyles">The body styles.</param>
        private void BindSeriesBikesAndModelBodyStyleBikes(NewsDetailPageVM objData, BikeSeriesEntityBase bikeSeriesEntityBase, EnumBikeBodyStyles bodyStyles)
        {
            try
            {
                if (objData.SeriesWidget != null && objData.Make != null && objData.Make.MaskingName != null && bikeSeriesEntityBase != null)
                {
                    objData.SeriesBikesAndModelBodyStyleBikes = new MultiTabsWidgetVM()
                    {
                        TabHeading1 = string.Format("{0} {1}", bikeSeriesEntityBase.SeriesName, bodyStyles == EnumBikeBodyStyles.Scooter ? "Scooters" : "Bikes"),
                        ViewPath1 = "~/Views/Shared/_EditorialSeriesBikesWidget.cshtml",
                        TabId1 = "SeriesBikes",
                        ViewAllHref1 = string.Format("/{0}-bikes/{1}/", objData.Make.MaskingName, bikeSeriesEntityBase.MaskingName),
                        ViewAllTitle1 = string.Format("View all {0} {1}", bikeSeriesEntityBase.SeriesName, bodyStyles == EnumBikeBodyStyles.Scooter ? "Scooters" : "Bikes"),
                        ViewAllText1 = string.Format("View all {0} {1}", bikeSeriesEntityBase.SeriesName, bodyStyles == EnumBikeBodyStyles.Scooter ? "Scooters" : "Bikes"),
                        ShowViewAllLink1 = true,
                        PopularSeriesBikes = objData.SeriesWidget.PopularSeriesBikes,
                        //FetchPopularSeriesBikes(bikeSeriesEntityBase.SeriesId),


                        TabHeading2 = string.Format("Popular {0}", Bikewale.Utility.BodyStyleLinks.BodyStyleText(bodyStyles)),
                        ViewPath2 = "~/Views/BestBikes/_PopularBodyStyle_Vertical.cshtml",
                        TabId2 = "PopularBodyStyle",
                        ViewAllHref2 = bodyStyles == EnumBikeBodyStyles.Scooter ? "/best-scooters-in-india/" : (bodyStyles == EnumBikeBodyStyles.Sports ? "/best-sports-bikes-in-india/" : (bodyStyles == EnumBikeBodyStyles.Cruiser ? "/best-cruiser-bikes-in-india/" : "/best-bikes-in-india/")),
                        ViewAllTitle2 = string.Format("View all popular {0}", Bikewale.Utility.BodyStyleLinks.BodyStyleText(bodyStyles).ToLower()),
                        ViewAllText2 = string.Format("View all popular {0}", Bikewale.Utility.BodyStyleLinks.BodyStyleText(bodyStyles).ToLower()),
                        ShowViewAllLink2 = true,
                        PopularBodyStyle = new PopularBodyStyleVM() { PopularBikes = objData.PopularBodyStyle.PopularBikes.Take(6) },
                        Pages = MultiTabWidgetPagesEnum.SeriesBikesAndModelBodyStyleBike,
                        PageName = "News"
                    };
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.News.NewsIndexPage.BindSeriesBikesAndModelBodyStyleBikes");
            }
        }

        /// <summary>
        /// Fetches the popular series bikes.
        /// Modified by : Sanskar Gupta on 16 April 2018
        /// Description : Added null check for `popularSeriesBikes`
        /// </summary>
        /// <param name="seriesId">The series identifier.</param>
        /// <returns></returns>
        private IEnumerable<MostPopularBikesBase> FetchPopularSeriesBikes(uint seriesId)
        {
            IEnumerable<MostPopularBikesBase> popularSeriesBikes = null;
            try
            {
                popularSeriesBikes = _models.GetMostPopularBikesByMakeWithCityPrice((int)MakeId, CityId);
                if(popularSeriesBikes == null)
                {
                    return null;
                }
                string modelIds = string.Empty;
                if (seriesId > 0)
                {
                    modelIds = _series.GetModelIdsBySeries(seriesId);
                }
                string[] modelArray = modelIds.Split(',');
                if (modelArray.Length > 0)
                {
                    popularSeriesBikes = (from bike in popularSeriesBikes
                                          where modelArray.Contains(bike.objModel.ModelId.ToString())
                                          select bike
                                         );
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Models.News.NewsIndexPage.FetchPopularBikes");
            }
            return popularSeriesBikes;
        }
        
        #endregion

        /// <summary>
        /// Created By  : Deepak Israni on 11 April 2018
        /// Description : Function to Bind Bike Info Widget. 
        /// </summary>
        /// <param name="objData"></param>
        private void BindBikeInfoWidget(NewsDetailPageVM objData)
        {
            BikeInfoWidget objBikeInfo = new BikeInfoWidget(_bikeInfo, _cityCacheRepo, ModelId, CityId, _totalTabCount, _pageId);
            objData.BikeInfo = objBikeInfo.GetData();
            objData.BikeInfo.IsSmallSlug = true;
        }

    }

}