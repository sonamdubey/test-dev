using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Models;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class NewsController : Controller
    {
        private readonly ICMSCacheContent _articles = null;
        private readonly IPager _pager = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private int _topCount;
        public NewsController(ICMSCacheContent articles, IPager pager, IBikeModelsCacheRepository<int> models, IBikeModels<BikeModelEntity, int> bikeModels)
        {
            _articles = articles;
            _pager = pager;
            _models = models;
            _bikeModels = bikeModels;
        }

        /// <summary>
        /// Created by : Aditi srivastava on 27 Mar 2017
        /// Summmary   : Action method to render news listing page
        /// </summary>
        /// <returns></returns>
        [Route("newslanding/")]
        public ActionResult Index()
        {
            _topCount = 4;
            NewsIndexPage obj = new NewsIndexPage(_articles, _pager,_models,_bikeModels,_topCount);

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
                NewsIndexPageVM objData = obj.GetData();
                if (obj.status == Entities.StatusCodes.ContentNotFound)
                    return Redirect("/pagenotfound.aspx");
                else
                return View(objData);
            }
        }

        [Route("m/newslanding/")]
        public ActionResult Index_Mobile()
        {
            _topCount = 9;
            NewsIndexPage obj = new NewsIndexPage(_articles, _pager, _models, _bikeModels, _topCount);

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
                NewsIndexPageVM objData = obj.GetData();
                if (obj.status == Entities.StatusCodes.ContentNotFound)
                    return Redirect("/pagenotfound.aspx");
                else
                return View(objData);
            }
        }

        [Route("news/{basicid}/")]
        public ActionResult Details(string basicid)
        {
            return View();
        }

        [Route("m/news/{basicid}/")]
        public ActionResult Details_Mobile(string basicid)
        {
            return View();
        }
    }
}