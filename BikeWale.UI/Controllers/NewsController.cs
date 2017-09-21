using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Pager;
using Bikewale.Models;
using Bikewale.Utility;
using log4net;
using System;
using System.Diagnostics;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    using Common;
    using Entities.CMS.Articles;
    using Interfaces.PWA.CMS;
    using Models.News;
    using System.Linq;


    public class NewsController : Controller
    {
        static bool _logPWAStats = BWConfiguration.Instance.EnablePWALogging;
        static bool _disablePWA = BWConfiguration.Instance.DisablePWA;
        static ILog _logger = LogManager.GetLogger("Pwa-Logger-NewsController");
        private readonly bool _logNewsUrl = BWConfiguration.Instance.LogNewsUrl;

        #region Variables fro dependency injection
        private readonly ICMSCacheContent _articles = null;
        private readonly IPager _pager = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private readonly IBikeMakesCacheRepository<int> _makes = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeInfo _bikeInfo = null;
        private readonly ICityCacheRepository _cityCache = null;
        private readonly IPWACMSCacheRepository _renderedArticles = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objBikeVersionsCache = null;

        #endregion

        #region Constructor
        public NewsController(ICMSCacheContent articles, IPager pager, IBikeModelsCacheRepository<int> models, IBikeMakesCacheRepository<int> makes, IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming, IBikeInfo bikeInfo, ICityCacheRepository cityCache, IPWACMSCacheRepository renderedArticles, IBikeVersionCacheRepository<BikeVersionEntity, uint> objBikeVersionsCache)
        {
            _articles = articles;
            _pager = pager;
            _models = models;
            _bikeModels = bikeModels;
            _upcoming = upcoming;
            _bikeInfo = bikeInfo;
            _cityCache = cityCache;
            _renderedArticles = renderedArticles;
            _makes = makes;
            _objBikeVersionsCache = objBikeVersionsCache;

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
            NewsIndexPage obj = new NewsIndexPage(_articles, _pager, _makes, _models, _bikeModels, _upcoming, _renderedArticles, _objBikeVersionsCache);
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
            NewsIndexPage obj = new NewsIndexPage(_articles, _pager, _makes, _models, _bikeModels, _upcoming, _renderedArticles, _objBikeVersionsCache);
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
                if (obj.status == Entities.StatusCodes.ContentNotFound)
                    return Redirect("/m/pagenotfound.aspx");
                else
                    return View(objData);
            }
        }

        /// <summary>
        /// Created by : Prasad Gawde on 18 May 2017
        /// Summmary   : Action method to render news listing page -mobile
        /// </summary>
        [Route("m/news/index_pwa/")]
        public ActionResult Index_Mobile_Pwa()
        {
            NewsIndexPage obj = new NewsIndexPage(_articles, _pager, _makes, _models, _bikeModels, _upcoming, _renderedArticles, _objBikeVersionsCache);
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
                Stopwatch sw = null;
                if (_logPWAStats)
                    sw = Stopwatch.StartNew();

                NewsIndexPageVM objData = obj.GetPwaData(9);

                if (_logPWAStats)
                {
                    sw.Stop();
                    ThreadContext.Properties["TimeTaken"] = sw.ElapsedMilliseconds;
                    ThreadContext.Properties["PageName"] = "NewsController - List";
                    _logger.Error(sw.ElapsedMilliseconds);
                }
                if (obj.status == Entities.StatusCodes.ContentNotFound)
                    return Redirect("/m/pagenotfound.aspx");
                else
                {
                    if (_disablePWA)
                        return View("~/Views/News/Index_Mobile.cshtml", objData);
                    else
                    {
                        return View(objData);
                    }
                }
            }
        }

        /// <summary>
        /// Created by : Aditi srivastava on 29 Mar 2017
        /// Summmary   : Action method to render news detail page-desktop
        /// </summary>
        [Route("news/detail/{basicid}/")]
        [Filters.DeviceDetection()]
        public ActionResult Detail(string basicid)
        {
            NewsDetailPage obj = new NewsDetailPage(_articles, _makes, _models, _bikeModels, _upcoming, _bikeInfo, _cityCache, basicid, _renderedArticles, _objBikeVersionsCache);
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
            NewsDetailPage obj = new NewsDetailPage(_articles, _makes, _models, _bikeModels, _upcoming, _bikeInfo, _cityCache, basicid, _renderedArticles, _objBikeVersionsCache);
            obj.IsMobile = true;
            obj.LogNewsUrl = _logNewsUrl;
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

                Stopwatch sw = null;
                if (_logPWAStats)
                    sw = Stopwatch.StartNew();

                NewsDetailPageVM objData = obj.GetPwaData(9);

                if (_logPWAStats)
                {
                    sw.Stop();
                    ThreadContext.Properties["TimeTaken"] = sw.ElapsedMilliseconds;
                    ThreadContext.Properties["PageName"] = "NewsController - Detail";
                    _logger.Error(sw.ElapsedMilliseconds);
                }

                if (_logNewsUrl && !string.IsNullOrEmpty(objData.ArticleDetails.ArticleUrl) && objData.ArticleDetails.ArticleUrl.EndsWith(@".html"))
                {
                    ThreadContext.Properties["NewsUrl"] = objData.ArticleDetails.ArticleUrl;
                    _logger.Error(String.Format("m/news/detail/{0}/", basicid));
                }

                if (obj.status == Entities.StatusCodes.ContentNotFound)
                    return Redirect("/m/pagenotfound.aspx");
                else
                {
                    if (_disablePWA)
                    {
                        return View("~/Views/News/Detail_Mobile_nopwa.cshtml", objData);
                    }
                    else
                    {
                        return View(objData);
                    }
                }
            }
        }

        /// <summary>
        /// Action to get the map news details page
        /// </summary>
        /// <param name="basicid"></param>
        /// <returns></returns>
        [Route("m/news/details/{basicid}/amp/")]
        public ActionResult DetailsAMP(int basicid)
        {
            try
            {
                ViewBag.TaggedModelId = 0;

                ArticleDetails objNews = _articles.GetNewsDetails(Convert.ToUInt32(basicid));

                if (objNews != null && objNews.Content != null)
                {
                    ViewBag.Title = objNews.Title;
                    ViewBag.ArticleSectionTitle = "- BikeWale News";
                    ViewBag.Author = objNews.AuthorName;
                    ViewBag.PublishedDate = objNews.DisplayDate.ToString();
                    ViewBag.LastModified = objNews.DisplayDate.ToString();
                    ViewBag.PageViews = objNews.Views;
                    ViewBag.Canonical = String.Format("{0}/news/{1}-{2}.html", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, basicid, objNews.ArticleUrl);
                    ViewBag.MobilePageUrl = String.Format("{0}/m/news/{1}-{2}.html", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, basicid, objNews.ArticleUrl);
                    ViewBag.MainImageUrl = Bikewale.Utility.Image.GetPathToShowImages(objNews.OriginalImgUrl, objNews.HostUrl, Bikewale.Utility.ImageSize._640x348);
                    ViewBag.Description = objNews.Description.StripHtml();

                    //Convert article content to the amp content
                    ViewBag.NewsContent = objNews.Content.ConvertToAmpContent();
                    ViewBag.SanitizedNewsContent = objNews.Content.StripHtml();

                    if (!String.IsNullOrEmpty(objNews.NextArticle.ArticleUrl))
                    {
                        ViewBag.NextPageTitle = objNews.NextArticle.Title;
                        ViewBag.NextPageUrl = String.Format("{0}/m/news/{1}-{2}.html", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, objNews.NextArticle.BasicId, objNews.NextArticle.ArticleUrl);
                    }

                    if (!String.IsNullOrEmpty(objNews.PrevArticle.ArticleUrl))
                    {
                        ViewBag.PrevPageTitle = objNews.PrevArticle.Title;
                        ViewBag.PrevPageUrl = String.Format("{0}/m/news/{1}-{2}.html", Bikewale.Utility.BWConfiguration.Instance.BwHostUrl, objNews.PrevArticle.BasicId, objNews.PrevArticle.ArticleUrl);
                    }

                    if (objNews.VehiclTagsList != null && objNews.VehiclTagsList.Count > 0)
                    {

                        VehicleTag objTag = objNews.VehiclTagsList.FirstOrDefault(m => m.ModelBase != null && m.ModelBase.ModelId > 0);

                        if (objTag != null)
                            ViewBag.TaggedModelId = objTag.ModelBase.ModelId;
                    }
                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "m/news/details/{basicid}/amp/" + basicid);
            }
            return View("~/views/m/content/news/details_amp.cshtml");
        }



        /// <summary>
        /// Created by : Snehal Dange on 17th August , 2017
        /// Summmary   : Action method to render Scooter news - Desktop
        /// </summary>
        [Route("scooters/news/")]
        [Filters.DeviceDetection()]
        public ActionResult Scooters()
        {
            NewsScootersPageVM objData = null;

            try
            {
                ScooterNewsPage obj = new ScooterNewsPage(_articles, _pager, _models, _makes, _bikeModels, _upcoming, _renderedArticles);
                if (obj.Status == Entities.StatusCodes.ContentNotFound)
                {
                    return Redirect("/pagenotfound.aspx");
                }
                else if (obj.Status == Entities.StatusCodes.RedirectPermanent)
                {
                    return RedirectPermanent(obj.RedirectUrl);
                }
                else
                {
                    obj.WidgetTopCount = 4;
                    objData = obj.GetData();
                    if (obj.Status == Entities.StatusCodes.ContentNotFound)
                        return Redirect("/pagenotfound.aspx");


                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Controllers.Scooters");
            }
            return View(objData);
        }


        /// <summary>
        /// Created by : Snehal Dange on 18th August , 2017
        /// Summmary   : Action method to render scooter news listing page -mobile
        /// </summary>
        [Route("m/scooters/news/")]
        public ActionResult Scooters_Mobile()
        {
            NewsScootersPageVM objData = null;

            try
            {
                ScooterNewsPage obj = new ScooterNewsPage(_articles, _pager, _models, _makes, _bikeModels, _upcoming, _renderedArticles);
                obj.IsMobile = true;
                if (obj.Status == Entities.StatusCodes.ContentNotFound)
                {
                    return Redirect("/m/pagenotfound.aspx");
                }
                else if (obj.Status == Entities.StatusCodes.RedirectPermanent)
                {
                    return RedirectPermanent(string.Format("/m{0}", obj.RedirectUrl));
                }
                else
                {
                    obj.WidgetTopCount = 9;
                    objData = obj.GetData();
                    if (obj.Status == Entities.StatusCodes.ContentNotFound)
                        return Redirect("/m/pagenotfound.aspx");

                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Controllers.Scooters_Mobile");
            }
            return View(objData);
        }

        #endregion
    }
}