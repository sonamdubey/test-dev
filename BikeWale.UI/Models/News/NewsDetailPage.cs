using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.GenericBikes;
using Bikewale.Entities.Location;
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
using Bikewale.Models.BikeModels;
using Bikewale.Models.Scooters;
using Bikewale.Notifications;
using Bikewale.PWA.Utils;
using Bikewale.Utility;
using log4net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models
{
    /// <summary>
    /// Created by : Aditi Srivastava on 29 Mar 2017
    /// Summary    : Model for news details page
    /// Modified by:Snehal Dange on 24 August,2017
    /// Description: Added _bikeMakesCacheRepository,_objBikeVersionsCache.
    ///              Added PopularScooterBrandsWidget
    /// </summary>
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
        #endregion

        #region Page level variables
        public StatusCodes status;
        public string mappedCWId;
        public string redirectUrl;
        private GlobalCityAreaEntity currentCityArea;
        private uint CityId, MakeId, ModelId, pageCatId = 0;
        private readonly uint _totalTabCount = 3;
        private BikeInfoTabType _pageId = BikeInfoTabType.News;
        private EnumBikeType bikeType = EnumBikeType.All;
        private readonly bool showCheckOnRoadCTA = false;
        private uint basicId;
        private PQSourceEnum pqSource = 0;
        #endregion

        #region Public properties
        public bool IsMobile { get; set; }

        static ILog _logger = LogManager.GetLogger("NewsDetailPage");

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

        public bool LogNewsUrl { get; set; }
        public bool IsAMPPage { get; set; }
        #endregion

        #region Constructor
        public NewsDetailPage(ICMSCacheContent cmsCache, IBikeMakesCacheRepository bikeMakesCacheRepository, IBikeModelsCacheRepository<int> models, IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming, IBikeInfo bikeInfo, ICityCacheRepository cityCacheRepo, string basicId, IPWACMSCacheRepository renderedArticles, IBikeVersionCacheRepository<BikeVersionEntity, uint> objBikeVersionsCache)
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
            LogNewsUrl = BWConfiguration.Instance.LogNewsUrl;
            ProcessQueryString();
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
                    GetWidgetData(objData, widgetTopCount, false);
                    SetPageMetas(objData);

                    if (objData.Model != null && ModelId != 0 && objData.Model.ModelId != ModelId)
                        objData.Model.ModelId = (int)ModelId;
                    if (IsAMPPage)
                    {
                        BindAmpJsTags(objData);
                    }
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
                    GetWidgetData(objData, widgetTopCount, true);
                    SetPageMetas(objData);

                    if (objData.Model != null && ModelId != 0 && objData.Model.ModelId != ModelId)
                        objData.Model.ModelId = (int)ModelId;

                    objData.ReduxStore = new PwaReduxStore();

                    var newsDetailReducer = objData.ReduxStore.News.NewsDetailReducer;
                    newsDetailReducer.ArticleDetailData.ArticleDetail = ConverterUtility.MapArticleDetailsToPwaArticleDetails(objData.ArticleDetails);
                    newsDetailReducer.RelatedModelObject.ModelObject = ConverterUtility.MapGenericBikeInfoToPwaBikeInfo(objData.BikeInfo);

                    if (objData.PopularBodyStyle == null)
                    {
                        SetPopularBikeByBodyStyleId(objData, widgetTopCount);
                    }

                    newsDetailReducer.NewBikesListData.NewBikesList = ConverterUtility.MapNewBikeListToPwaNewBikeList(objData, CityName);

                    var storeJson = JsonConvert.SerializeObject(objData.ReduxStore);

                    objData.ServerRouterWrapper = _renderedArticles.GetNewsDetails(PwaCmsHelper.GetSha256Hash(storeJson), objData.ReduxStore.News.NewsDetailReducer,
                                newsDetailReducer.ArticleDetailData.ArticleDetail.ArticleUrl, "root", "ServerRouterWrapper");
                    objData.WindowState = storeJson;
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
                objData.PageMetaTags.Title = string.Format("{0} - BikeWale News", objData.ArticleDetails.Title);
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
                    }
                    else
                    {
                        objData.Make = objData.ArticleDetails.VehiclTagsList.FirstOrDefault().MakeBase;
                        if (objData.Make != null)
                            objData.Make = new Common.MakeHelper().GetMakeNameByMakeId((uint)objData.Make.MakeId);
                    }
                    MakeId = (uint)objData.Make.MakeId;
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
        /// Modified By Sajal Gupta on 25-04-20187
        /// Descrition : Call most popular bike widget by body type
        /// </summary>
        private void GetWidgetData(NewsDetailPageVM objData, int topCount, bool isPWA)
        {
            try
            {
                currentCityArea = GlobalCityArea.GetGlobalCityArea();
                if (currentCityArea != null)
                    CityId = currentCityArea.CityId;

                EnumBikeBodyStyles bodyStyle = EnumBikeBodyStyles.AllBikes;

                List<BikeVersionMinSpecs> objVersionsList = _objBikeVersionsCache.GetVersionMinSpecs(ModelId, false);

                if (objVersionsList != null && objVersionsList.Count > 0)
                    bodyStyle = objVersionsList.FirstOrDefault().BodyStyle;

                MostPopularBikesWidget objPopularBikes = new MostPopularBikesWidget(_bikeModels, bodyStyle.Equals(EnumBikeBodyStyles.Scooter) ? EnumBikeType.Scooters : EnumBikeType.All, showCheckOnRoadCTA, false, pqSource, pageCatId, MakeId);
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

                    if (IsMobile)
                    {
                       
                        if (bodyStyle.Equals(EnumBikeBodyStyles.Scooter) && !isPWA)
                        {
                            PopularScooterBrandsWidget objPopularScooterBrands = new PopularScooterBrandsWidget(_bikeMakesCacheRepository);
                            objPopularScooterBrands.TopCount = 6;
                            objData.PopularScooterMakesWidget = objPopularScooterBrands.GetData();
                            bikeType = EnumBikeType.Scooters;
                        }
                        else
                        {
                            SetPopularBikeByBodyStyleId(objData, topCount);
                        }
                    }
                    else
                    {

                        objPopularBikes = new MostPopularBikesWidget(_bikeModels, EnumBikeType.All, showCheckOnRoadCTA, false, pqSource, pageCatId, MakeId);
                        objPopularBikes.TopCount = 9;
                        objPopularBikes.CityId = CityId;


                        MostPopularMakeBikes = objPopularBikes.GetData();

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

                        if (bodyStyle.Equals(EnumBikeBodyStyles.Scooter))
                        {
                            PopularScooterBrandsWidget objPopularScooterBrands = new PopularScooterBrandsWidget(_bikeMakesCacheRepository);
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
                            }
                        }



                        if (bodyStyle.Equals(EnumBikeBodyStyles.Scooter))
                        {
                            objData.PopularMakeScootersAndOtherBrandsWidget = new MultiTabsWidgetVM();

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
                            objData.PopularMakeBikesAndBodyStyleBikesWidget.PageName = "News";

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
                        else
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

                    if (objData.Make != null)
                    {
                        objData.UpcomingBikes.WidgetHeading = string.Format("Upcoming {0} bikes", objData.Make.MakeName);
                        objData.UpcomingBikes.WidgetHref = string.Format("/{0}-bikes/upcoming/", objData.Make.MaskingName);
                    }
                    else
                    {
                        objData.UpcomingBikes.WidgetHeading = "Upcoming bikes";
                        objData.UpcomingBikes.WidgetHref = "/upcoming-bikes/";
                    }
                    objData.UpcomingBikes.WidgetLinkTitle = "Upcoming Bikes in India";
                }



                if (MakeId > 0 && objData.Make != null)
                {
                    if (bikeType.Equals(EnumBikeType.Scooters))
                    {
                        PopularBikesWidget.WidgetHeading = string.Format("Popular {0} scooters", objData.Make.MakeName);
                        if (objData.Make.IsScooterOnly)
                            PopularBikesWidget.WidgetHref = string.Format("/{0}-bikes/", objData.Make.MaskingName);
                        else
                            PopularBikesWidget.WidgetHref = string.Format("/{0}-scooters/", objData.Make.MaskingName);
                        PopularBikesWidget.WidgetLinkTitle = string.Format("{0} Scooters", objData.Make.MakeName);
                        PopularBikesWidget.CtaText = "View all scooters";
                    }
                    else
                    {
                        PopularBikesWidget.WidgetHeading = string.Format("Popular {0} bikes", objData.Make.MakeName);
                        PopularBikesWidget.WidgetHref = string.Format("/{0}-bikes/", objData.Make.MaskingName);
                        PopularBikesWidget.WidgetLinkTitle = string.Format("{0} Bikes", objData.Make.MakeName);
                        PopularBikesWidget.CtaText = "View all bikes";
                    }
                }
                else
                {
                    PopularBikesWidget.WidgetHeading = "Popular bikes";
                    PopularBikesWidget.WidgetHref = "/best-bikes-in-india/";
                    PopularBikesWidget.WidgetLinkTitle = "Best Bikes in India";
                    PopularBikesWidget.CtaText = "View all bikes";
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Bikewale.Models.NewsDetailPage.GetWidgetData");
            }
        }

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
                    objData.PopularBodyStyle.WidgetHref = UrlFormatter.FormatGenericPageUrl(objData.PopularBodyStyle.BodyStyle);
                    objData.PopularBodyStyle.CityId = CityId;
                    objData.PopularBodyStyle.ReturnUrlForAmpPages = string.Format("{0}/m/news/{1}-{2}.html", BWConfiguration.Instance.BwHostUrl, objData.ArticleDetails.BasicId, objData.ArticleDetails.ArticleUrl);
                    bikeType = objData.PopularBodyStyle.BodyStyle == EnumBikeBodyStyles.Scooter ? EnumBikeType.Scooters : EnumBikeType.All;
                }
            }
        }

        #endregion
    }
}