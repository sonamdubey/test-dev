﻿using Bikewale.Entities;
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
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;

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
    public class NewsDetailPage
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
            IBikeSeriesCacheRepository seriesCache, IBikeSeries series)
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
                    GetWidgetData(objData, widgetTopCount, false);

                    BindBikeInfoWidget(objData);
                    SetAdditionalVariables(objData);
                    SetEditorialWidgetData(objData);

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
            isMakeLive = !(objData.BikeInfo != null && (objData.BikeInfo.IsUpcoming || objData.BikeInfo.IsDiscontinued));
            bodyStyle = objData.BodyStyle;
            isSeriesAvailable = objData.IsSeriesAvailable;
            if (objData.Make != null)
            {
                isScooterOnlyMake = objData.Make.IsScooterOnly;
            }
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
                    GetWidgetData(objData, widgetTopCount, true);
                    SetPageMetas(objData);

                    if (objData.Model != null && ModelId != 0 && objData.Model.ModelId != ModelId)
                        objData.Model.ModelId = (int)ModelId;

                    objData.ReduxStore = new PwaReduxStore();

                    var newsDetailReducer = objData.ReduxStore.News.NewsDetailReducer;
                    newsDetailReducer.ArticleDetailData.ArticleDetail = ConverterUtility.MapArticleDetailsToPwaArticleDetails(objData.ArticleDetails);
                    newsDetailReducer.RelatedModelObject.ModelObject = ConverterUtility.MapGenericBikeInfoToPwaBikeInfo(objData.BikeInfo);
                    newsDetailReducer.NewBikesListData.NewBikesList = ConverterUtility.MapNewBikeListToPwaNewBikeList(objData, CityName);
                    newsDetailReducer.NewBikesListData.BikeMakeList = ConverterUtility.MapBikeMakeEntityBaseListToPwaMakeBikeBaseList(objData);
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
        /// Created by : Aditi Srivastava on 29 Mar 2017
        /// Summary    : Get data for the page widgets
        /// Modified By Sajal Gupta on 25-04-2017
        /// Descrition : Call most popular bike widget by body type
        /// </summary>
        private void GetWidgetData(NewsDetailPageVM objData, int topCount, bool isPWA)
        {
            try
            {
                objData.BodyStyle = EnumBikeBodyStyles.AllBikes;

                List<BikeVersionMinSpecs> objVersionsList = _objBikeVersionsCache.GetVersionMinSpecs(ModelId, false);

                if (objVersionsList != null && objVersionsList.Count > 0)
                    objData.BodyStyle = objVersionsList.FirstOrDefault().BodyStyle;

                objData.IsScooter = objData.BodyStyle.Equals(EnumBikeBodyStyles.Scooter);
                MostPopularBikesWidget objPopularBikes = new MostPopularBikesWidget(_bikeModels, objData.IsScooter ? EnumBikeType.Scooters : EnumBikeType.All, showCheckOnRoadCTA, false, pqSource, pageCatId, MakeId);
                objPopularBikes.TopCount = topCount;
                objPopularBikes.CityId = CityId;
                objData.MostPopularBikes = objPopularBikes.GetData();
                if (objData.MostPopularBikes != null)
                {
                    objData.MostPopularBikes.CityId = CityId;
                    objData.MostPopularBikes.ReturnUrlForAmpPages = string.Format("{0}/m/news/{1}-{2}.html", BWConfiguration.Instance.BwHostUrl, objData.ArticleDetails.BasicId, objData.ArticleDetails.ArticleUrl);
                }
                MostPopularBikeWidgetVM PopularBikesWidget = objData.MostPopularBikes;

                if (ModelId > 0)
                {
                    #region When Model is tagged

                    MostPopularBikeWidgetVM MostPopularBikes = null;
                    MostPopularBikeWidgetVM MostPopularMakeBikes = null;
                    MostPopularBikeWidgetVM MostPopularScooters = null;
                    MostPopularBikeWidgetVM MostPopularMakeScooters = null;
                    UpcomingBikesWidgetVM UpcomingBikes = null;
                    UpcomingBikesWidgetVM UpcomingScooters = null;
                    IEnumerable<BikeMakeEntityBase> PopularScooterMakes = null;
                    PopularBodyStyleVM BodyStyleVM = null;

                    BikeInfoWidget objBikeInfo = new BikeInfoWidget(_bikeInfo, _cityCacheRepo, ModelId, CityId, _totalTabCount, _pageId);
                    objData.BikeInfo = objBikeInfo.GetData();
                    objData.BikeInfo.IsSmallSlug = true;


                    if (objData.IsSeriesAvailable)
                    {
                        objData.SeriesBikes = new MostPopularBikeWidgetVM()
                        {
                            Bikes = FetchPopularSeriesBikes(bikeSeriesEntityBase.SeriesId),
                            CityId = CityId,
                            WidgetHeading = string.Format("Popular {0} {1}", bikeSeriesEntityBase.SeriesName, objData.IsScooter ? "Scooters" : "Bikes"),
                            WidgetLinkTitle = string.Format("View all {0} {1}", bikeSeriesEntityBase.SeriesName, objData.IsScooter ? "Scooters" : "Bikes"),
                            WidgetHref = string.Format("/{0}{1}-bikes/{2}/", IsMobile ? "m/":"",objData.Make.MaskingName, bikeSeriesEntityBase.MaskingName)

                        };
                    }


                    if (IsMobile)  // Mobile
                    {
                        if (objData.IsScooter)
                        {
                            PopularScooterBrandsWidget objPopularScooterBrands = new PopularScooterBrandsWidget(_bikeMakesCacheRepository);
                            objPopularScooterBrands.TopCount = 6;
                            objData.PopularScooterMakesWidget = objPopularScooterBrands.GetData();
                        }
                        else
                        {
                            SetPopularBikeByBodyStyleId(objData, topCount);
                        }

                        if (bikeSeriesEntityBase != null)
                        {
                            FetchPopularBikes(objData, bikeSeriesEntityBase.SeriesId);
                            if (objData.IsSeriesAvailable && objData.SeriesWidget != null && objData.Make != null)
                            {
                                objData.MostPopularBikes = new MostPopularBikeWidgetVM()
                                {
                                    Bikes = objData.SeriesWidget.PopularSeriesBikes,
                                    WidgetHeading = string.Format("Popular {0} {1}", bikeSeriesEntityBase.SeriesName, objData.IsScooter ? "Scooters" : "Bikes"),
                                    WidgetHref = "/m" + UrlFormatter.BikeSeriesUrl(objData.Make.MaskingName, bikeSeriesEntityBase.MaskingName),
                                    WidgetLinkTitle = string.Format("View all {0} {1}", bikeSeriesEntityBase.MaskingName, objData.IsScooter ? "Scooters" : "Bikes")
                                };
                            }

                        }
                        else if (objData.BodyStyle.Equals(EnumBikeBodyStyles.Scooter) && objData.Make != null)
                        {
                            objData.MostPopularBikes = MostPopularMakeScooters;
                            objData.MostPopularBikes.WidgetHeading = string.Format("Popular {0} Scooters", objData.Make.MakeName);
                            objData.MostPopularBikes.WidgetHref = string.Format("/m/{0}-scooters/", objData.Make.MaskingName);
                            objData.MostPopularBikes.WidgetLinkTitle = string.Format("{0} Scooters", objData.Make.MakeName);
                        }
                    }
                    else  // Desktop
                    {
                        #region Desktop

                        // Fetch overall popular bikes 
                        objPopularBikes = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, showCheckOnRoadCTA, false, pqSource, pageCatId, MakeId);
                        objPopularBikes.TopCount = 9;
                        objPopularBikes.CityId = CityId;
                        MostPopularMakeBikes = objPopularBikes.GetData();


                        // Fetch popular bikes for make
                        objPopularBikes = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, showCheckOnRoadCTA, false, pqSource, pageCatId);
                        objPopularBikes.TopCount = 9;
                        objPopularBikes.CityId = CityId;
                        MostPopularBikes = objPopularBikes.GetData();

                        objData.MostPopularMakeBikes = new MostPopularBikeWidgetVM() { Bikes = MostPopularMakeBikes.Bikes.Take(6), WidgetHref = string.Format("/{0}-bikes/", objData.Make.MaskingName), WidgetLinkTitle = "View all Bikes" };

                        MostPopularBikesWidget objPopularScooters = new MostPopularBikesWidget(_bikeModels, EnumBikeType.Scooters, showCheckOnRoadCTA, false, pqSource, pageCatId, MakeId);
                        objPopularScooters.TopCount = 9;
                        objPopularScooters.CityId = CityId;


                        MostPopularMakeScooters = objPopularScooters.GetData();

                        objPopularScooters = new MostPopularBikesWidget(_bikeModels, EnumBikeType.Scooters, showCheckOnRoadCTA, false, pqSource, pageCatId);
                        objPopularScooters.TopCount = 9;
                        objPopularScooters.CityId = CityId;
                        MostPopularScooters = objPopularScooters.GetData();

                        objData.MostPopularMakeBikes = new MostPopularBikeWidgetVM()
                        {
                            Bikes = MostPopularMakeBikes.Bikes.Take(6),
                            WidgetHref = string.Format("/{0}-bikes/", objData.Make.MaskingName),
                            WidgetLinkTitle = "View all Bikes"
                        };



                        // Fetch upcoming bikes
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

                        // Fetch upcoming scooters
                        objUpcomingBikes.Filters.BodyStyleId = (uint)EnumBikeBodyStyles.Scooter;
                        UpcomingScooters = objUpcomingBikes.GetData();

                        PopularScooterBrandsWidget objPopularScooterBrands = null;
                        // If model is a scooter
                        if (objData.IsScooter)
                        {

                            // Fetch popular makes for scooters
                            objPopularScooterBrands = new PopularScooterBrandsWidget(_bikeMakesCacheRepository);
                            objPopularScooterBrands.TopCount = 6;
                            PopularScooterMakes = objPopularScooterBrands.GetData();
                            objData.PopularScooterMakesWidget = PopularScooterMakes.Take(6);

                            // fetch popular scooters for make
                            objPopularScooters = new MostPopularBikesWidget(_bikeModels, EnumBikeType.Scooters, showCheckOnRoadCTA, false, pqSource, pageCatId, MakeId);
                            objPopularScooters.TopCount = 9;
                            objPopularScooters.CityId = CityId;
                            MostPopularMakeScooters = objPopularScooters.GetData();

                            // fetch overall popular scooters
                            objPopularScooters = new MostPopularBikesWidget(_bikeModels, EnumBikeType.Scooters, showCheckOnRoadCTA, false, pqSource, pageCatId);
                            objPopularScooters.TopCount = 9;
                            objPopularScooters.CityId = CityId;
                            MostPopularScooters = objPopularScooters.GetData();
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
                            }
                        }

                        if (objData.IsScooter)
                        {
                            if (objData.IsSeriesAvailable)
                            {
                                BindSeriesBikesAndOtherBrands(objData, bikeSeriesEntityBase, PopularScooterMakes);
                            }
                            objData.PopularMakeScootersAndOtherBrandsWidget = new MultiTabsWidgetVM();

                            // If series is not available
                            if (!objData.IsSeriesAvailable)
                            {
                                BindPopularMakeScootersAndOtherBrandsWidget(objData, MostPopularMakeScooters, PopularScooterMakes);
                            }
                            BindPopularAndUpcomingScooters(objData, MostPopularScooters, UpcomingScooters);

                        }
                        else //if (bodyStyle.Equals(EnumBikeBodyStyles.Sports) || bodyStyle.Equals(EnumBikeBodyStyles.Cruiser))
                        {
                            // Bind everything except scooters
                            BindSportsAndCruisers(objData, MostPopularMakeBikes, objData.BodyStyle);
                            BindPopularAndUpcomingBikes(objData, MostPopularBikes, UpcomingBikes);
                        }
                        //else // When Body style is other Than Scooter, sports or Cruisers
                        //{
                        //    BindOtherBodyStyle(objData, MostPopularBikes, UpcomingBikes);
                        //}
                        #endregion

                    }
                    #endregion
                }
                else  // When model is not tagged
                {
                    BindMakeUpcomingBikes(objData, topCount);
                }

                // if make is available
                if (MakeId > 0 && objData.Make != null)
                {
                    BindPopularBikesOrScooters(objData, PopularBikesWidget, objData.BodyStyle);
                }
                else
                {
                    PopularBikesWidget.WidgetHeading = "Popular Bikes";
                    PopularBikesWidget.WidgetHref = string.Format("/{0}best-bikes-in-india/", IsMobile ? "m/" : "");
                    PopularBikesWidget.WidgetLinkTitle = "Best Bikes in India";
                    PopularBikesWidget.CtaText = "View all Bikes";
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.NewsDetailPage.GetWidgetData");
            }
        }

        /// <summary>
        /// Binds the make upcoming bikes.
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="topCount">The top count.</param>
        private void BindMakeUpcomingBikes(NewsDetailPageVM objData, int topCount)
        {
            string urlPrefix = IsMobile ? "m/" : "";
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

            if (objData.Make != null)
            {
                objData.UpcomingBikes.WidgetHeading = string.Format("Upcoming {0} bikes", objData.Make.MakeName);
                objData.UpcomingBikes.WidgetHref = string.Format("/{0}{1}-bikes/upcoming/", urlPrefix, objData.Make.MaskingName);
            }
            else
            {
                objData.UpcomingBikes.WidgetHeading = "Upcoming bikes";
                objData.UpcomingBikes.WidgetHref = string.Format("/{0}upcoming-bikes/", urlPrefix);
            }
            objData.UpcomingBikes.WidgetLinkTitle = "Upcoming Bikes in India";
        }

        /// <summary>
        /// Binds the popular bikes or scooters.
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="PopularBikesWidget">The popular bikes widget.</param>
        /// <param name="bodyStyle">The body style.</param>
        private void BindPopularBikesOrScooters(NewsDetailPageVM objData, MostPopularBikeWidgetVM PopularBikesWidget, EnumBikeBodyStyles bodyStyle)
        {
            string urlPrefix = IsMobile ? "m/" : "";
            if (bodyStyle.Equals(EnumBikeBodyStyles.Scooter))
            {
                PopularBikesWidget.WidgetHeading = string.Format("Popular {0} Scooters", objData.Make.MakeName);
                if (objData.Make.IsScooterOnly)
                    PopularBikesWidget.WidgetHref = string.Format("/{0}{1}-bikes/", urlPrefix, objData.Make.MaskingName);
                else
                    PopularBikesWidget.WidgetHref = string.Format("/{0}{1}-scooters/", urlPrefix, objData.Make.MaskingName);
                PopularBikesWidget.WidgetLinkTitle = string.Format("{0} Scooters", objData.Make.MakeName);
                PopularBikesWidget.CtaText = "View all Scooters";
            }
            else
            {
                PopularBikesWidget.WidgetHeading = string.Format("Popular {0} Bikes", objData.Make.MakeName);
                PopularBikesWidget.WidgetHref = string.Format("/{0}{1}-bikes/", urlPrefix, objData.Make.MaskingName);
                PopularBikesWidget.WidgetLinkTitle = string.Format("{0} Bikes", objData.Make.MakeName);
                PopularBikesWidget.CtaText = "View all Bikes";
            }
        }

        private void BindPopularMakeScootersAndOtherBrandsWidget(NewsDetailPageVM objData, MostPopularBikeWidgetVM MostPopularMakeScooters, IEnumerable<BikeMakeEntityBase> PopularScooterMakes)
        {
            objData.PopularMakeScootersAndOtherBrandsWidget.TabHeading1 = string.Format("{0} scooters", objData.Make.MakeName);
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
            objData.PopularMakeScootersAndOtherBrandsWidget.PageName = "News";
        }

        /// <summary>
        /// Binds the popular and upcoming scooters.
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="MostPopularScooters">The most popular scooters.</param>
        /// <param name="UpcomingScooters">The upcoming scooters.</param>
        private void BindPopularAndUpcomingScooters(NewsDetailPageVM objData, MostPopularBikeWidgetVM MostPopularScooters, UpcomingBikesWidgetVM UpcomingScooters)
        {
            objData.PopularScootersAndUpcomingScootersWidget = new MultiTabsWidgetVM();

            objData.PopularScootersAndUpcomingScootersWidget.TabHeading1 = "Popular scooters";
            objData.PopularScootersAndUpcomingScootersWidget.TabHeading2 = "Upcoming Scooters";
            objData.PopularScootersAndUpcomingScootersWidget.ViewPath1 = "~/Views/BikeModels/_MostPopularBikesSideBar.cshtml";
            objData.PopularScootersAndUpcomingScootersWidget.ViewPath2 = "~/Views/Upcoming/_UpcomingBikes_Vertical.cshtml"; ;
            objData.PopularScootersAndUpcomingScootersWidget.TabId1 = "PopularScooters";
            objData.PopularScootersAndUpcomingScootersWidget.TabId2 = "UpcomingScooters";
            objData.PopularScootersAndUpcomingScootersWidget.MostPopularScooters = MostPopularScooters;

            if (MostPopularScooters != null)
                objData.PopularScootersAndUpcomingScootersWidget.MostPopularScooters.Bikes = objData.PopularScootersAndUpcomingScootersWidget.MostPopularScooters.Bikes.Take(6);

            objData.PopularScootersAndUpcomingScootersWidget.UpcomingScooters = UpcomingScooters;

            if (UpcomingScooters != null)
                objData.PopularScootersAndUpcomingScootersWidget.UpcomingScooters.UpcomingBikes = objData.PopularScootersAndUpcomingScootersWidget.UpcomingScooters.UpcomingBikes.Take(6);
            objData.PopularScootersAndUpcomingScootersWidget.ViewAllHref1 = "/best-scooters-in-india/";
            objData.PopularScootersAndUpcomingScootersWidget.ViewAllTitle1 = "View all scooters";
            objData.PopularScootersAndUpcomingScootersWidget.ViewAllText1 = "View all scooters";
            objData.PopularScootersAndUpcomingScootersWidget.ShowViewAllLink1 = true;
            objData.PopularScootersAndUpcomingScootersWidget.ShowViewAllLink2 = false;
            objData.PopularScootersAndUpcomingScootersWidget.Pages = MultiTabWidgetPagesEnum.PopularScootersAndUpcomingScooters;
            objData.PopularScootersAndUpcomingScootersWidget.PageName = "News";
        }

        /// <summary>
        /// Binds the popular and upcoming bikes.
        /// Modified by : Sanskar Gupta on 22 Jan 2018
        /// Description : Added Newly Launched feature
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="MostPopularBikes">The most popular bikes.</param>
        /// <param name="UpcomingBikes">The upcoming bikes.</param>
        private void BindPopularAndUpcomingBikes(NewsDetailPageVM objData, MostPopularBikeWidgetVM MostPopularBikes, UpcomingBikesWidgetVM UpcomingBikes)
        {
            objData.PopularBikesAndUpcomingBikesWidget = new MultiTabsWidgetVM();

            objData.PopularBikesAndUpcomingBikesWidget.TabHeading1 = "Popular bikes";
            objData.PopularBikesAndUpcomingBikesWidget.TabHeading2 = "Upcoming bikes";
            objData.PopularBikesAndUpcomingBikesWidget.ViewPath1 = "~/Views/BikeModels/_MostPopularBikesSideBar.cshtml";
            objData.PopularBikesAndUpcomingBikesWidget.ViewPath2 = "~/Views/Upcoming/_UpcomingBikes_Vertical.cshtml";
            objData.PopularBikesAndUpcomingBikesWidget.TabId1 = "PopularBikes";
            objData.PopularBikesAndUpcomingBikesWidget.TabId2 = "UpcomingBikes";
            objData.PopularBikesAndUpcomingBikesWidget.MostPopularBikes = MostPopularBikes;

            if (MostPopularBikes != null)
                objData.PopularBikesAndUpcomingBikesWidget.MostPopularBikes.Bikes = objData.PopularBikesAndUpcomingBikesWidget.MostPopularBikes.Bikes.Take(6);

            BikeFilters obj = new BikeFilters();
            obj.CityId = CityId;
            IEnumerable<MostPopularBikesBase> promotedBikes = _bikeModels.GetAdPromotedBike(obj, true);
            objData.PopularBikesAndUpcomingBikesWidget.MostPopularBikes.Bikes = _bikeModels.GetAdPromoteBikeFilters(promotedBikes, objData.PopularBikesAndUpcomingBikesWidget.MostPopularBikes.Bikes);

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
            objData.PopularBikesAndUpcomingBikesWidget.PageName = "News";
        }

        /// <summary>
        /// Binds the sports and cruisers.
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="MostPopularMakeBikes">The most popular make bikes.</param>
        /// <param name="bodyStyle">The body style.</param>
        private void BindSportsAndCruisers(NewsDetailPageVM objData, MostPopularBikeWidgetVM MostPopularMakeBikes, EnumBikeBodyStyles bodyStyle)
        {
            if (objData.IsSeriesAvailable)
            {
                FetchBikesByBodyStyle(objData, bodyStyle);
                BindSeriesBikesAndModelBodyStyleBikes(objData, bikeSeriesEntityBase, bodyStyle);
            }
            else
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
                objData.PopularMakeBikesAndBodyStyleBikesWidget.PageName = "News";
            }
        }

        /// <summary>
        /// Binds the other body style.
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="MostPopularBikes">The most popular bikes.</param>
        /// <param name="UpcomingBikes">The upcoming bikes.</param>
        private void BindOtherBodyStyle(NewsDetailPageVM objData, MostPopularBikeWidgetVM MostPopularBikes, UpcomingBikesWidgetVM UpcomingBikes)
        {
            objData.PopularBikesAndUpcomingBikesWidget = new MultiTabsWidgetVM();

            objData.PopularBikesAndUpcomingBikesWidget.TabHeading1 = "Popular bikes";
            objData.PopularBikesAndUpcomingBikesWidget.TabHeading2 = "Upcoming bikes";
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
            objData.PopularBikesAndUpcomingBikesWidget.PageName = "News";
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
        /// Sets the popular bike by body style identifier.
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="topCount">The top count.</param>
        private void SetPopularBikeByBodyStyleId(NewsDetailPageVM objData, int topCount)
        {
            if (objData != null && topCount > 0)
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
                    objData.PopularBodyStyle.WidgetHref = string.Format("{0}{1}",IsMobile ? "/m":"",UrlFormatter.FormatGenericPageUrl(objData.PopularBodyStyle.BodyStyle));
                    objData.PopularBodyStyle.CityId = CityId;
                    objData.PopularBodyStyle.ReturnUrlForAmpPages = string.Format("{0}/m/news/{1}-{2}.html", BWConfiguration.Instance.BwHostUrl, objData.ArticleDetails.BasicId, objData.ArticleDetails.ArticleUrl);

                    if (objData.PopularBodyStyle.BodyStyle == EnumBikeBodyStyles.AllBikes) {
                        BikeFilters obj = new BikeFilters();
                        obj.CityId = CityId;
                        IEnumerable<MostPopularBikesBase> promotedBikes = _bikeModels.GetAdPromotedBike(obj, true);
                        objData.PopularBodyStyle.PopularBikes = _bikeModels.GetAdPromoteBikeFilters(promotedBikes, objData.PopularBodyStyle.PopularBikes);
                    }

                }
            }
        }

        /// <summary>
        /// Binds the series bikes and other brands.
        /// </summary>
        /// <param name="objData">The object data.</param>
        /// <param name="bikeSeriesEntityBase">The bike series entity base.</param>
        /// <param name="popularScooterMakes">The popular scooter makes.</param>
        private void BindSeriesBikesAndOtherBrands(NewsDetailPageVM objData, BikeSeriesEntityBase bikeSeriesEntityBase, IEnumerable<BikeMakeEntityBase> popularScooterMakes)
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
                objData.SeriesBikesAndOtherBrands.PopularSeriesBikes = FetchPopularSeriesBikes(bikeSeriesEntityBase.SeriesId);
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
                objData.SeriesBikesAndOtherBrands.PageName = "News";
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Models.News.NewsIndexPage.BindSeriesBikesAndOtherBrands");
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
        /// </summary>
        /// <param name="seriesId">The series identifier.</param>
        /// <returns></returns>
        private IEnumerable<MostPopularBikesBase> FetchPopularSeriesBikes(uint seriesId)
        {
            IEnumerable<MostPopularBikesBase> popularSeriesBikes = null;
            try
            {
                popularSeriesBikes = _models.GetMostPopularBikesByMakeWithCityPrice((int)MakeId, CityId);
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

        /// <summary>
        /// Fetches the popular bikes.
        /// </summary>
        private void FetchPopularBikes(NewsDetailPageVM objData, uint seriesId = 0)
        {
            try
            {
                objData.SeriesWidget = new EditorialSeriesWidgetVM();
                IEnumerable<MostPopularBikesBase> makePopularBikes = _models.GetMostPopularBikesByMake((int)MakeId);
                string modelIds = string.Empty;
                modelIds = _series.GetModelIdsBySeries(seriesId);
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Models.News.NewsIndexPage.FetchPopularBikes - BasicId : " + _basicId);
            }

        }
        #endregion

        /// <summary>
        /// Created By  : Deepak Israni, Sanskar Gupta on 09 April 2018
        /// Description : Function to Set Editorial Widget Data. Added the conditionals to Bind the widgets using this function.
        /// </summary>
        /// <param name="objData">VM of the page.</param>
        private void SetEditorialWidgetData(NewsDetailPageVM objData)
        {
            if (isModelTagged)
            {
                if (bodyStyle == EnumBikeBodyStyles.Cruiser || bodyStyle == EnumBikeBodyStyles.Sports)
                {
                    // Model is Tagged, Body Style is either Cruiser or Sports
                    objData.PageWidgets = SetWidgetDataForModelCruiserSports();
                }
                else if (bodyStyle == EnumBikeBodyStyles.Scooter)
                {
                    // Model is Tagged, Body Style is Scooter
                    objData.PageWidgets = SetWidgetDataForModelScooter();
                }
                else
                {
                    // Model is Tagged, Body Style is something other than Cruiser/Sports/Scooter
                    objData.PageWidgets = SetWidgetDataForModelExceptCruiserSportsScooter();
                }
            }
            else
            {
                // Make is Tagged
                objData.PageWidgets = SetWidgetDataForMakeNews();
            }
        }

        /// <summary>
        /// Created By  : Sanskar Gupta on 12 April 2018
        /// Description : Function to Set Editorial widget data for Model-News and Body Styles except Cruiser/Sports/Scooter.
        /// </summary>
        private IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> SetWidgetDataForModelExceptCruiserSportsScooter()
        {
            EditorialWidgetVM FirstWidget = new EditorialWidgetVM();
            FirstWidget.WidgetColumns = new Dictionary<EditorialWidgetColumnPosition, EditorialWidgetInfo>();
            EditorialWidgetVM SecondWidget = new EditorialWidgetVM();
            SecondWidget.WidgetColumns = new Dictionary<EditorialWidgetColumnPosition, EditorialWidgetInfo>();
            try
            {
                FirstWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = isSeriesAvailable ? BindWidget(EditorialWidgetCategory.Popular_Series) : BindWidget(EditorialWidgetCategory.Popular_Make);
                SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = BindWidget(EditorialWidgetCategory.Popular_All);
                if (!IsMobile)
                {
                    SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Right] = BindWidget(EditorialWidgetCategory.Upcoming_All);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeWale.UI.Models.News.NewsDetailPage.SetWidgetDataForModelExceptCruiserSportsScooter"));
            }
            IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> PageWidgets = new Dictionary<EditorialPageWidgetPosition, EditorialWidgetVM>();
            PageWidgets.Add(EditorialPageWidgetPosition.First, FirstWidget);
            PageWidgets.Add(EditorialPageWidgetPosition.Second, SecondWidget);
            return PageWidgets;
        }

        /// <summary>
        /// Created By  : Sanskar Gupta on 12 April 2018
        /// Description : Function to Set Editorial widget data for Model-News and Body Style Scooter.
        /// </summary>
        /// <param name="objData">VM of the page.</param>
        /// <param name="widgets">Array of widgets to be populated (4 items for now)</param>
        private IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> SetWidgetDataForModelScooter()
        {
            EditorialWidgetVM FirstWidget = new EditorialWidgetVM();
            FirstWidget.WidgetColumns = new Dictionary<EditorialWidgetColumnPosition, EditorialWidgetInfo>();
            EditorialWidgetVM SecondWidget = new EditorialWidgetVM();
            SecondWidget.WidgetColumns = new Dictionary<EditorialWidgetColumnPosition, EditorialWidgetInfo>();
            try
            {
                FirstWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = isSeriesAvailable ? BindWidget(EditorialWidgetCategory.Series_Scooters) : BindWidget(EditorialWidgetCategory.Popular_Make_Scooters);
                if (!IsMobile)
                {
                    FirstWidget.WidgetColumns[EditorialWidgetColumnPosition.Right] = BindWidget(EditorialWidgetCategory.OtherBrands_All);
                    SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = BindWidget(EditorialWidgetCategory.Popular_Scooters);
                    SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Right] = BindWidget(EditorialWidgetCategory.Upcoming_Scooters);
                }
                else
                {
                    SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = BindWidget(EditorialWidgetCategory.OtherBrands_All);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeWale.UI.Models.News.NewsDetailPage.SetWidgetDataForModelScooter"));
            }
            IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> PageWidgets = new Dictionary<EditorialPageWidgetPosition, EditorialWidgetVM>();
            PageWidgets.Add(EditorialPageWidgetPosition.First, FirstWidget);
            PageWidgets.Add(EditorialPageWidgetPosition.Second, SecondWidget);
            return PageWidgets;
        }

        /// <summary>
        /// Created By  : Sanskar Gupta on 12 April 2018
        /// Description : Function to Set Editorial widget data for Model-News and Body Styles Cruiser/Sports.
        /// </summary>
        private IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> SetWidgetDataForModelCruiserSports()
        {
            EditorialWidgetVM FirstWidget = new EditorialWidgetVM();
            FirstWidget.WidgetColumns = new Dictionary<EditorialWidgetColumnPosition, EditorialWidgetInfo>();
            EditorialWidgetVM SecondWidget = new EditorialWidgetVM();
            SecondWidget.WidgetColumns = new Dictionary<EditorialWidgetColumnPosition, EditorialWidgetInfo>();
            try
            {
                FirstWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = isSeriesAvailable ? BindWidget(EditorialWidgetCategory.Popular_Series) : BindWidget(EditorialWidgetCategory.Popular_Make);
                if (!IsMobile)
                {
                    FirstWidget.WidgetColumns[EditorialWidgetColumnPosition.Right] = BindWidget(EditorialWidgetCategory.Popular_BodyStyle);
                    SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = BindWidget(EditorialWidgetCategory.Popular_All);
                    SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Right] = BindWidget(EditorialWidgetCategory.Upcoming_All);
                }
                else
                {
                    SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = BindWidget(EditorialWidgetCategory.Popular_BodyStyle);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeWale.UI.Models.News.NewsDetailPage.SetWidgetDataForModelCruiserSports"));
            }
            IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> PageWidgets = new Dictionary<EditorialPageWidgetPosition, EditorialWidgetVM>();
            PageWidgets.Add(EditorialPageWidgetPosition.First, FirstWidget);
            PageWidgets.Add(EditorialPageWidgetPosition.Second, SecondWidget);
            return PageWidgets;
        }

        /// <summary>
        /// Created By  : Sanskar Gupta on 12 April 2018
        /// Description : Function to Set Editorial widget data for Make-News page.
        /// </summary>
        private IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> SetWidgetDataForMakeNews()
        {
            EditorialWidgetVM FirstWidget = new EditorialWidgetVM();
            FirstWidget.WidgetColumns = new Dictionary<EditorialWidgetColumnPosition, EditorialWidgetInfo>();
            EditorialWidgetVM SecondWidget = new EditorialWidgetVM();
            SecondWidget.WidgetColumns = new Dictionary<EditorialWidgetColumnPosition, EditorialWidgetInfo>();
            try
            {
                if (isMakeTagged && isMakeLive)
                {
                    FirstWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = BindWidget(EditorialWidgetCategory.Popular_Make);
                    SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = BindWidget(EditorialWidgetCategory.Popular_All);
                    if (!IsMobile)
                    {
                        SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Right] = BindWidget(EditorialWidgetCategory.Upcoming_All);
                    }
                }
                else
                {
                    FirstWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = BindWidget(EditorialWidgetCategory.Popular_All);
                    SecondWidget.WidgetColumns[EditorialWidgetColumnPosition.Left] = BindWidget(EditorialWidgetCategory.Upcoming_All);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeWale.UI.Models.News.NewsDetailPage.SetWidgetDataForMakeNews"));
            }
            IDictionary<EditorialPageWidgetPosition, EditorialWidgetVM> PageWidgets = new Dictionary<EditorialPageWidgetPosition, EditorialWidgetVM>();
            PageWidgets.Add(EditorialPageWidgetPosition.First, FirstWidget);
            PageWidgets.Add(EditorialPageWidgetPosition.Second, SecondWidget);
            return PageWidgets;
        }

        /// <summary>
        /// Created By  : Sanskar Gupta, Deepak Israni on 09 April 2018
        /// Description : Function to Bind Editorial Widget. Pass the `category` of the bike data to be bound and the VM object.
        /// </summary>
        /// <param name="category">The category Enum of the bike list to be bound.</param>
        /// <returns></returns>
        private EditorialWidgetInfo BindWidget(EditorialWidgetCategory category)
        {
            EditorialWidgetInfo widget = null;
            try
            {
                int editorialWidgetTopCount = IsMobile ? 9 : 6;
                widget = InitializeWidgetCategory(category);
                string viewAllTitle = string.Empty;

                switch (category)
                {
                    case EditorialWidgetCategory.Popular_All:
                        SetWidgetStructureData(widget, "Popular Bikes", "PopularBikes", true, UrlFormatter.FormatGenericPageUrl(EnumBikeBodyStyles.AllBikes), "View all bikes", "View all bikes");
                        ((EditorialPopularBikesWidget)widget).MostPopularBikeList = SetAdPromotedBikes(_models.GetMostPopularBikesbyMakeCity((uint)editorialWidgetTopCount, 0, CityId), editorialWidgetTopCount);
                        break;

                    case EditorialWidgetCategory.Popular_Make:
                        SetWidgetStructureData(widget, string.Format("{0} Bikes", MakeName), "PopularMakeBikes", true, UrlFormatter.BikeMakeUrl(MakeMaskingName), "View all bikes", "View all bikes");
                        ((EditorialPopularBikesWidget)widget).MostPopularBikeList = _models.GetMostPopularBikesbyMakeCity((uint)editorialWidgetTopCount, MakeId, CityId);
                        break;

                    case EditorialWidgetCategory.Popular_BodyStyle:
                        string title = null, tabId = null;
                        if (bodyStyle == EnumBikeBodyStyles.Sports)
                        {
                            title = "Sports Bikes";
                            viewAllTitle = "View all Sports bikes";
                            tabId = "PopularSportsBikes";
                        }
                        else if (bodyStyle == EnumBikeBodyStyles.Cruiser)
                        {
                            title = "Cruisers";
                            viewAllTitle = "View all Cruisers";
                            tabId = "PopularCruisers";
                        }
                        SetWidgetStructureData(widget, title, tabId, true, UrlFormatter.FormatGenericPageUrl(bodyStyle), "View all bikes", viewAllTitle);
                        ((EditorialPopularBikesWidget)widget).MostPopularBikeList = _models.GetPopularBikesByBodyStyle((ushort)bodyStyle, (uint)editorialWidgetTopCount, CityId);
                        break;

                    case EditorialWidgetCategory.Popular_Scooters:
                        SetWidgetStructureData(widget, "Popular Scooters", "PopularScooters", true, UrlFormatter.FormatGenericPageUrl(bodyStyle), "View all scooters", "View all scooters");
                        ((EditorialPopularBikesWidget)widget).MostPopularBikeList = _models.GetMostPopularScooters((uint)editorialWidgetTopCount, CityId);
                        break;

                    case EditorialWidgetCategory.Popular_Make_Scooters:
                        SetWidgetStructureData(widget, string.Format("{0} Scooters", MakeName), "PopularMakeScooters", true, UrlFormatter.ScooterMakeUrl(MakeMaskingName, isScooterOnlyMake), "View all scooters", "View all scooters");
                        ((EditorialPopularBikesWidget)widget).MostPopularBikeList = _models.GetMostPopularScooters((uint)editorialWidgetTopCount, MakeId, CityId);
                        break;

                    case EditorialWidgetCategory.Popular_Series:
                        viewAllTitle = string.Format("View all {0} bikes", bikeSeriesEntityBase.SeriesName);
                        SetWidgetStructureData(widget, string.Format("{0} Bikes", bikeSeriesEntityBase.SeriesName), "PopularSeriesBikes", true, UrlFormatter.BikeSeriesUrl(MakeMaskingName, bikeSeriesEntityBase.MaskingName), viewAllTitle, viewAllTitle);
                        ((EditorialPopularBikesWidget)widget).MostPopularBikeList = FetchPopularSeriesBikes(bikeSeriesEntityBase.SeriesId).Take(editorialWidgetTopCount);
                        break;

                    case EditorialWidgetCategory.Series_Scooters:
                        viewAllTitle = string.Format("View all {0} Scooters", bikeSeriesEntityBase.SeriesName);
                        SetWidgetStructureData(widget, string.Format("{0} Scooters", bikeSeriesEntityBase.SeriesName), "PopularSeriesScooters", true, UrlFormatter.BikeSeriesUrl(MakeMaskingName, bikeSeriesEntityBase.MaskingName), viewAllTitle, viewAllTitle);
                        ((EditorialPopularBikesWidget)widget).MostPopularBikeList = FetchPopularSeriesBikes(bikeSeriesEntityBase.SeriesId).Take(editorialWidgetTopCount);
                        break;

                    case EditorialWidgetCategory.Upcoming_All:
                        viewAllTitle = "View all upcoming bikes";
                        SetWidgetStructureData(widget, "Upcoming Bikes", "UpcomingBikes", true, UrlFormatter.UpcomingBikesUrl(), viewAllTitle, viewAllTitle);
                        ((EditorialUpcomingBikesWidget)widget).UpcomingBikeList = _models.GetUpcomingBikesList(EnumUpcomingBikesFilter.Default, editorialWidgetTopCount);
                        break;

                    case EditorialWidgetCategory.Upcoming_Scooters:
                        SetWidgetStructureData(widget, "Upcoming Scooters", "UpcomingScooters", false);
                        ((EditorialUpcomingBikesWidget)widget).UpcomingBikeList = GetUpcomingScooters(editorialWidgetTopCount);
                        break;

                    case EditorialWidgetCategory.OtherBrands_All:
                        viewAllTitle = "View other brands";
                        SetWidgetStructureData(widget, "Other Brands", "OtherBrands", true, UrlFormatter.AllScootersUrl(), viewAllTitle, viewAllTitle);
                        ((EditorialOtherBrandsWidget)widget).OtherBrandsList = GetOtherScooterBrands((int)MakeId, editorialWidgetTopCount);
                        break;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeWale.UI.Models.News.NewsDetailPage.BindWidget__Category: {0}", category));
            }
            return widget;
        }

        /// <summary>
        /// Created By  : Sanskar Gupta on 12 April 2018
        /// Description : Function to Initialize the Editorial Widget categories. 
        /// </summary>
        private EditorialWidgetInfo InitializeWidgetCategory(EditorialWidgetCategory category)
        {
            EditorialWidgetInfo widget = null;
            switch (category)
            {
                case EditorialWidgetCategory.Popular_All:
                case EditorialWidgetCategory.Popular_Make:
                case EditorialWidgetCategory.Popular_Make_Scooters:
                case EditorialWidgetCategory.Popular_BodyStyle:
                case EditorialWidgetCategory.Popular_Scooters:
                case EditorialWidgetCategory.Popular_Series:
                case EditorialWidgetCategory.Series_Scooters:
                    widget = new EditorialPopularBikesWidget();
                    break;
                case EditorialWidgetCategory.Upcoming_All:
                case EditorialWidgetCategory.Upcoming_Make:
                case EditorialWidgetCategory.Upcoming_Scooters:
                    widget = new EditorialUpcomingBikesWidget();
                    break;
                case EditorialWidgetCategory.OtherBrands_All:
                    widget = new EditorialOtherBrandsWidget();
                    break;
                default:
                    return widget;
            }           
            return widget;
        }

        /// <summary>
        /// Created By  : Deepak Israni on 12 April 2018
        /// Description : Function to Get Scooters of other brands.
        /// </summary>
        /// <param name="skipMakeId">MakeId of the scooter to be skipped</param>
        /// <param name="topCount"></param>
        /// <returns></returns>
        private IEnumerable<BikeMakeEntityBase> GetOtherScooterBrands(int skipMakeId, int topCount)
        {
            IEnumerable<BikeMakeEntityBase> bikeList = null;
            try
            {
                bikeList = _bikeMakesCacheRepository.GetScooterMakes();

                if (bikeList != null && skipMakeId > 0 && bikeList.Any())
                {
                    bikeList = bikeList.Where(x => x.MakeId != skipMakeId);
                }

                if (bikeList != null && topCount > 0 && bikeList.Count() > topCount)
                {
                    bikeList = bikeList.Take(topCount);
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "NewsDetailPage.GetOtherScooterBrands()");
            }
            return bikeList;
        }

        /// <summary>
        /// Created By : Deepak Israni on 9 April 2018
        /// Description: Function to get list of Upcoming scooters.
        /// </summary>
        /// <param name="topCount"></param>
        /// <returns></returns>
        private IEnumerable<UpcomingBikeEntity> GetUpcomingScooters(int topCount)
        {
            IEnumerable<UpcomingBikeEntity> UpcomingScooters = null;

            UpcomingBikesListInputEntity Filters = new UpcomingBikesListInputEntity()
            {
                PageNo = 1,
                PageSize = topCount,
                BodyStyleId = (uint)EnumBikeBodyStyles.Scooter
            };
            
                UpcomingScooters = _upcoming.GetModels(Filters, EnumUpcomingBikesFilter.Default);

            return UpcomingScooters;
        }

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

        /// <summary>
        /// Created By  : Sanskar Gupta on 11 April 2018
        /// Description : Function to set the status and position of a `Newly Launched` bike 
        /// </summary>
        private IEnumerable<MostPopularBikesBase> SetAdPromotedBikes(IEnumerable<MostPopularBikesBase> PopularBikesList, int topCount)
        {
            BikeFilters obj = new BikeFilters();
            obj.CityId = CityId;
            IEnumerable<MostPopularBikesBase> promotedBikes = _bikeModels.GetAdPromotedBike(obj, true);
            return _bikeModels.GetAdPromoteBikeFilters(promotedBikes, PopularBikesList).Take(topCount);
        }


        /// <summary>
        /// Created By  : Sanskar Gupta on 11 April 2018
        /// Description : Function to set Miscellaneous data for a widget.
        /// </summary>
        /// <param name="widget">The object containing data for that widget</param>
        /// <param name="title">String for Title of the Widget</param>
        /// <param name="tabId">String for TabId of the Widget</param>
        /// <param name="showViewAll">Boolean denoting whether `View All` link to be shown at the bottom of the widget.</param>
        /// <param name="viewAllUrl">Url where the user is redirected when the `View All` button is clicked</param>
        /// <param name="viewAllText">String for Text of `View All` button</param>
        /// <param name="viewAllTitle">Link title as well as the button text of the view all link/button</param>
        private void SetWidgetStructureData(EditorialWidgetInfo widget, string title, string tabId, bool showViewAll, string viewAllUrl=null, string viewAllText=null, string viewAllTitle=null)
        {
            try
            {
                widget.Title = title;
                widget.TabId = tabId;
                widget.ShowViewAll = showViewAll;

                if (widget.ShowViewAll)
                {
                    widget.ViewAllUrl = viewAllUrl;
                    widget.ViewAllText = viewAllText;
                    widget.ViewAllTitle = viewAllTitle;
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikeWale.UI.Models.News.NewsDetailPage.SetWidgetMiscData__WidgetTitle: {0}", title));
            }
        }
        

    }

}