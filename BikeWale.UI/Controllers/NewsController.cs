using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Pager;
using Bikewale.Models;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class NewsController : Controller
    {
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
                return RedirectPermanent(string.Format("/m{0}",obj.redirectUrl));
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
                if (obj.status == Entities.StatusCodes.ContentNotFound)
                    return Redirect("/m/pagenotfound.aspx");
                else
                    return View(objData);
            }
        }
        #endregion
    }
}