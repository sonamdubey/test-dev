using Bikewale.Entities.BikeData;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Pager;
using Bikewale.Models;
using Bikewale.PWA.Utils;
using log4net;
using Newtonsoft.Json;
using React.Web.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class NewsController : Controller
    {
        static CustomConcurrentArray _preProcessedNewslistHtmls = new CustomConcurrentArray();
        static CustomConcurrentArray _preProcessedNewsDetailHtmls = new CustomConcurrentArray();
        static HtmlHelper _htmlHelper=null;
        object _lockObject=new object();
        //static string _hashOfRenderedStore = string.Empty;
        //static IHtmlString _renderedStoreString = null;
      //  static string _storeJsonString = string.Empty;
        static ILog _logger = LogManager.GetLogger("Pwa-Logger-Renderengine");
        private HtmlHelper NewsHtmlHelper
        {
            get
            {
                if (_htmlHelper == null)
                {
                    lock (_lockObject)
                    {
                        if (_htmlHelper == null)
                        {
                            var vdd = new ViewDataDictionary();
                            var tdd = new TempDataDictionary();
                            var controllerContext = this.ControllerContext;
                            var view = new RazorView(controllerContext, "/", "/", false, null);
                            _htmlHelper = new HtmlHelper(new ViewContext(controllerContext, view, vdd, tdd, new StringWriter()),
                                 new ViewDataContainer(vdd));
                        }
                    }
                }
                return _htmlHelper;
            }
        }

        private class ViewDataContainer : IViewDataContainer
        {
            public ViewDataContainer(ViewDataDictionary viewData)
            {
                ViewData = viewData;
            }

            public ViewDataDictionary ViewData { get; set; }
        }

        #region Variables fro dependency injection
        private readonly ICMSCacheContent _articles = null;
        private readonly IPager _pager = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeInfo _bikeInfo = null;
        private readonly ICityCacheRepository _cityCache = null;
        #endregion

        #region Constructor
        public NewsController(ICMSCacheContent articles, IPager pager, IBikeModelsCacheRepository<int> models, IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming,IBikeInfo bikeInfo,ICityCacheRepository cityCache)
        {
            _articles = articles;
            _pager = pager;
            _models = models;
            _bikeModels = bikeModels;
            _upcoming = upcoming;
            _bikeInfo = bikeInfo;
            _cityCache=cityCache;
            
        }
        #endregion

        #region Action Methods
        /// <summary>
        /// Created by : Aditi srivastava on 27 Mar 2017
        /// Summmary   : Action method to render news listing page- Desktop
        /// </summary>
        [Route("news/index/")]
        [Filters.DeviceDetection()]
        public ActionResult Index()
        {
            NewsIndexPage obj = new NewsIndexPage(_articles, _pager,_models,_bikeModels,_upcoming);
            if (obj.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/pagenotfound.aspx");
            }
            else if (obj.status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(obj.redirectUrl);
            }
            else
            {
                NewsIndexPageVM objData = obj.GetData(4);
                if (obj.status == Entities.StatusCodes.ContentNotFound)
                    return Redirect("/pagenotfound.aspx");
                else
                return View(objData);
            }
        }
        /// <summary>
        /// Created by : Aditi srivastava on 27 Mar 2017
        /// Summmary   : Action method to render news listing page -mobile
        /// </summary>
        [Route("m/news/index/")]
        public ActionResult Index_Mobile()
        {
            NewsIndexPage obj = new NewsIndexPage(_articles, _pager, _models, _bikeModels,_upcoming);
            obj.IsMobile = true;
            if (obj.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/m/pagenotfound.aspx");
            }
            else if (obj.status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(string.Format("/m{0}", obj.redirectUrl));
            }
            else
            {
                NewsIndexPageVM objData = obj.GetData(9);

                if (objData != null)
                {
                    //setting the store for Redux
                    objData.ReduxStore = new PwaReduxStore();
                    var tempStoreArticleList = objData.ReduxStore.NewsReducer.NewsArticleListReducer.ArticleListData.ArticleList;
                    tempStoreArticleList.Articles = ConverterUtility.MapArticleSummaryListToPwaArticleSummaryList(objData.Articles.Articles);
                    tempStoreArticleList.StartIndex = (uint)objData.StartIndex;
                    tempStoreArticleList.EndIndex = (uint)objData.EndIndex;
                    tempStoreArticleList.RecordCount = (uint)objData.Articles.RecordCount;
                    PopulateStoreForWidgetData(objData, obj.CityName);


                    var tempStoreJson = JsonConvert.SerializeObject(objData.ReduxStore);
                    var tempHashOfStoreJson = ConverterUtility.GetSha256Hash(tempStoreJson);
                    var processedHtml=_preProcessedNewslistHtmls.Get(tempHashOfStoreJson);
                    string jsonStr;
                    IHtmlString processedString;

                    if (processedHtml==null)                  
                    {//rerender                        
                        var sw = Stopwatch.StartNew();
                        var articleReducer = objData.ReduxStore.NewsReducer.NewsArticleListReducer;
                        processedString = NewsHtmlHelper.React("ServerRouterWrapper", new
                        {
                            Url = "/m/news/",
                            ArticleListData = articleReducer.ArticleListData,
                            NewBikesListData = articleReducer.NewBikesListData

                        }, containerId: "root");
                        jsonStr = tempStoreJson;
                        _preProcessedNewslistHtmls.Add(tempHashOfStoreJson, processedString, jsonStr);
                        sw.Stop();
                        ThreadContext.Properties["TimeTaken"] = sw.ElapsedMilliseconds;
                        _logger.Error(sw.ElapsedMilliseconds);
                    }else
                    {
                        processedString = processedHtml.HtmlString;
                        jsonStr = processedHtml.Json;
                    }
                    objData.ServerRouterWrapper = processedString;
                    objData.WindowState = jsonStr;
                }                

                if (obj.status == Entities.StatusCodes.ContentNotFound)
                    return Redirect("/m/pagenotfound.aspx");
                else
                return View(objData);
            }
        }

        private void PopulateStoreForWidgetData(NewsIndexPageVM objData,string cityName)
        {
            List<PwaBikeNews> objPwaBikeNews = new List<PwaBikeNews>();
            if (objData.MostPopularBikes != null && objData.MostPopularBikes.Bikes != null)
            {
                PwaBikeNews popularBikes = new PwaBikeNews();
                popularBikes.Heading = "Popular bikes";
                popularBikes.CompleteListUrl = "/m/best-bikes-in-india/";
                popularBikes.CompleteListUrlAlternateLabel = "Best Bikes in India";
                popularBikes.CompleteListUrlLabel = "View all";
                popularBikes.BikesList = ConverterUtility.MapMostPopularBikesBaseToPwaBikeDetails(objData.MostPopularBikes.Bikes,
                    cityName);

                objPwaBikeNews.Add(popularBikes);
            }

            if (objData.UpcomingBikes != null && objData.UpcomingBikes.UpcomingBikes != null)
            {
                PwaBikeNews upcomingBikes = new PwaBikeNews();
                upcomingBikes.Heading = "Upcoming bikes";
                upcomingBikes.CompleteListUrl = "/m/upcoming-bikes/";
                upcomingBikes.CompleteListUrlAlternateLabel = "Upcoming Bikes in India";
                upcomingBikes.CompleteListUrlLabel = "View all";
                upcomingBikes.BikesList = ConverterUtility.MapUpcomingBikeEntityToPwaBikeDetails(objData.UpcomingBikes.UpcomingBikes
                    , cityName);
                objPwaBikeNews.Add(upcomingBikes);
            }

            objData.ReduxStore.NewsReducer.NewsArticleListReducer.NewBikesListData.NewBikesList = objPwaBikeNews;
        }

        /// <summary>
        /// Created by : Aditi srivastava on 29 Mar 2017
        /// Summmary   : Action method to render news detail page-desktop
        /// </summary>
        [Route("news/detail/{basicid}/")]
        [Filters.DeviceDetection()]
        public ActionResult Detail(string basicid)
        {
            NewsDetailPage obj = new NewsDetailPage(_articles, _models, _bikeModels, _upcoming, _bikeInfo, _cityCache, basicid);
            if (obj.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/pagenotfound.aspx");
            }
            else if (obj.status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(obj.redirectUrl);
            }
            else
            {
                NewsDetailPageVM objData = obj.GetData(3);
                if (obj.status == Entities.StatusCodes.ContentNotFound)
                    return Redirect("/pagenotfound.aspx");
                else
                    return View(objData);
            }
        }
        /// <summary>
        /// Created by : Aditi srivastava on 29 Mar 2017
        /// Summmary   : Action method to render news detail page- mobile
        /// </summary>
        [Route("m/news/detail/{basicid}/")]
        public ActionResult Detail_Mobile(string basicid)
        {
           NewsDetailPage obj = new NewsDetailPage(_articles, _models, _bikeModels, _upcoming, _bikeInfo, _cityCache, basicid);
           obj.IsMobile = true;
            if (obj.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/m/pagenotfound.aspx");
            }
            else if (obj.status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(string.Format("/m{0}",obj.redirectUrl));
            }
            else
            {
                NewsDetailPageVM objData = obj.GetData(9);

                if (objData != null)
                {
                    //setting the store for Redux
                    objData.ReduxStore = new PwaReduxStore();

                   
                    var newsDetailReducer = objData.ReduxStore.NewsReducer.NewsDetailReducer;
                    newsDetailReducer.ArticleDetailData.ArticleDetail = ConverterUtility.MapArticleDetailsToPwaArticleDetails(objData.ArticleDetails);
                    newsDetailReducer.NewBikesListData.NewBikesList = ConverterUtility.MapNewBikeListToPwaNewBikeList(objData, obj.CityName);
                    newsDetailReducer.RelatedModelObject.ModelObject = ConverterUtility.MapGenericBikeInfoToPwaBikeInfo(objData.BikeInfo);

                    var tempStoreJson = JsonConvert.SerializeObject(objData.ReduxStore);
                    var tempHashOfStoreJson = ConverterUtility.GetSha256Hash(tempStoreJson);
                    var processedHtml = _preProcessedNewsDetailHtmls.Get(tempHashOfStoreJson);
                    string jsonStr;
                    IHtmlString processedString;

                    if (processedHtml == null)
                    {//rerender                        
                        var sw = Stopwatch.StartNew();
                        var articleReducer = objData.ReduxStore.NewsReducer.NewsDetailReducer;
                        processedString = NewsHtmlHelper.React("ServerRouterWrapper", new
                        {
                            Url = articleReducer.ArticleDetailData.ArticleDetail.ArticleUrl,
                            ArticleDetailData = articleReducer.ArticleDetailData,
                            RelatedModelObject = articleReducer.RelatedModelObject,
                            NewBikesListData = articleReducer.NewBikesListData

                        }, containerId: "root");
                        jsonStr = tempStoreJson;
                        _preProcessedNewsDetailHtmls.Add(tempHashOfStoreJson, processedString, jsonStr);
                        sw.Stop();
                        ThreadContext.Properties["TimeTaken"] = sw.ElapsedMilliseconds;
                        _logger.Error(sw.ElapsedMilliseconds);
                    }
                    else
                    {
                        processedString = processedHtml.HtmlString;
                        jsonStr = processedHtml.Json;
                    }
                    objData.ServerRouterWrapper = processedString;
                    objData.WindowState = jsonStr;
                }

                if (obj.status == Entities.StatusCodes.ContentNotFound)
                    return Redirect("/m/pagenotfound.aspx");
                else
                    return View(objData);
            }
        }
        #endregion
    }
}