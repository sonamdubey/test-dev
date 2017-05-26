using Bikewale.Entities.BikeData;
using Bikewale.Entities.PWA.Articles;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Pager;
using Bikewale.Models;
using Bikewale.PWA.Utils;
using Bikewale.Utility;
using log4net;
using Newtonsoft.Json;
using React.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Web;
using System.Web.Mvc;
using React;

namespace Bikewale.Controllers
{
    using Interfaces.PWA.CMS;
    using AssemblyRegistration = React.AssemblyRegistration;


    public class NewsController : Controller
    {
        static bool _logPWAStats = BWConfiguration.Instance.EnablePWALogging;
        static bool _disablePWA = BWConfiguration.Instance.DisablePWA;
        static ILog _logger = LogManager.GetLogger("Pwa-Logger-NewsController");    
           

        #region Variables fro dependency injection
        private readonly ICMSCacheContent _articles = null;
        private readonly IPager _pager = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeInfo _bikeInfo = null;
        private readonly ICityCacheRepository _cityCache = null;
        private readonly IPWACMSCacheRepository _renderedArticles = null;
        #endregion

        #region Constructor
        public NewsController(ICMSCacheContent articles, IPager pager, IBikeModelsCacheRepository<int> models, IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming, IBikeInfo bikeInfo, ICityCacheRepository cityCache, IPWACMSCacheRepository renderedArticles)
        {
            _articles = articles;
            _pager = pager;
            _models = models;
            _bikeModels = bikeModels;
            _upcoming = upcoming;
            _bikeInfo = bikeInfo;
            _cityCache = cityCache;
            _renderedArticles = renderedArticles;

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
            NewsIndexPage obj = new NewsIndexPage(_articles, _pager, _models, _bikeModels, _upcoming,_renderedArticles);
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
            NewsIndexPage obj = new NewsIndexPage(_articles, _pager, _models, _bikeModels, _upcoming,_renderedArticles);
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
            NewsIndexPage obj = new NewsIndexPage(_articles, _pager, _models, _bikeModels, _upcoming,_renderedArticles);
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

                if (_disablePWA)
                    return View("~/Views/News/Index_Mobile.cshtml", objData);
                else
                {                    
                    if (obj.status == Entities.StatusCodes.ContentNotFound)
                        return Redirect("/m/pagenotfound.aspx");
                    else
                        return View(objData);
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
            NewsDetailPage obj = new NewsDetailPage(_articles, _models, _bikeModels, _upcoming, _bikeInfo, _cityCache, basicid, _renderedArticles);
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
            NewsDetailPage obj = new NewsDetailPage(_articles, _models, _bikeModels, _upcoming, _bikeInfo, _cityCache, basicid,_renderedArticles);
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

                NewsDetailPageVM objData = obj.GetPwaData(9);

                if (_logPWAStats)
                {
                    sw.Stop();
                    ThreadContext.Properties["TimeTaken"] = sw.ElapsedMilliseconds;
                    ThreadContext.Properties["PageName"] = "NewsController - Detail";
                    _logger.Error(sw.ElapsedMilliseconds);
                }

                if (_disablePWA)
                {
                    return View("~/Views/News/Detail_Mobile_nopwa.cshtml", objData);
                }
                else
                {
                    if (obj.status == Entities.StatusCodes.ContentNotFound)
                        return Redirect("/m/pagenotfound.aspx");
                    else
                        return View(objData);
                }
            }
        }
        #endregion
    }
}