using Bikewale.Entities.BikeData.NewLaunched;
using Bikewale.Filters;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Models;
using Bikewale.Utility;
using System;
using System.Linq;
using System.Web.Mvc;
namespace Bikewale.Controllers
{
    public class NewLaunchesController : Controller
    {
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IBikeMakesCacheRepository _objMakeCache = null;
        private readonly IUpcoming _upcoming = null;

        private readonly ICMSCacheContent _objArticles;

        public NewLaunchesController(INewBikeLaunchesBL newLaunches, IBikeMakesCacheRepository objMakeCache, IUpcoming upcoming, ICMSCacheContent objArticles)
        {
            _newLaunches = newLaunches;
            _objMakeCache = objMakeCache;
            _upcoming = upcoming;
            _objArticles = objArticles;
        }
        /// <summary>
        /// Modified By :- Subodh Jain 15 Feb 2017
        /// Summary:- Added make widget changes for landing page
        /// Modified By :- Subodh Jain 20 March 2017
        /// Summary :- Changed title and description for page
        /// </summary>
        /// <param name="pageNumber"></param>
        [Route("newlaunches/")]
        [DeviceDetection]
        public ActionResult Index(ushort? pageNumber)
        {
            var objFilters = new InputFilter()
            {
                PageNo = (int)(pageNumber.HasValue ? pageNumber : 1),
                CityId = GlobalCityArea.GetGlobalCityArea().CityId,
                PageSize = 15
            };
            NewLaunchedIndexModel model = new NewLaunchedIndexModel(_newLaunches, _objMakeCache, _upcoming, objFilters,
                                          Entities.PriceQuote.PQSourceEnum.Desktop_NewLaunchLanding, pageNumber, _objArticles);
            model.BaseUrl = "/new-bike-launches/";
            model.PageSize = 15;
            model.MakeTopCount = 10;
            return View(model.GetData());
        }

        /// <summary>
        /// Modified By :- Subodh Jain 15 Feb 2017
        /// Summary:- Added make widget changes for landing page
        /// Modified By :- Subodh Jain 20 March 2017
        /// Summary :- Changed title and description for page
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [Route("m/newlaunches/")]
        public ActionResult Index_Mobile(ushort? pageNumber)
        {
            var objFilters = new InputFilter()
            {
                PageNo = (int)(pageNumber.HasValue ? pageNumber : 1),
                CityId = GlobalCityArea.GetGlobalCityArea().CityId,
                PageSize = 10
            };
            NewLaunchedIndexModel model = new NewLaunchedIndexModel(_newLaunches, _objMakeCache, _upcoming, objFilters,
                                          Entities.PriceQuote.PQSourceEnum.Mobile_NewLaunchLanding, pageNumber, _objArticles);
            model.BaseUrl = "/m/new-bike-launches/";
            model.PageSize = 10;
            model.MakeTopCount = 6;
            model.IsMobile = true;
            return View(model.GetData());
        }

        /// <summary>
        /// Modified By :- Subodh Jain 17 Feb 2017
        /// Summary ;- Added upcoming widget
        /// </summary>
        /// <param name="launchYear"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [Route("newlaunches/make/{maskingName}/")]
        [DeviceDetection]
        public ActionResult BikeByMake(string maskingName, ushort? pageNumber)
        {
            NewLaunchedMakePageModel model = new NewLaunchedMakePageModel(maskingName, _newLaunches, _objMakeCache, _upcoming, Entities.PriceQuote.PQSourceEnum.Desktop_NewLaunchLanding, pageNumber);

            if (model.Status == Entities.StatusCodes.ContentFound)
            {
                model.BaseUrl = String.Format("/new-{0}-bike-launches/", maskingName);
                model.PageSize = 15;
                model.MakeTopCount = 9;
                var vmPage = model.GetData();
                if (model.Status == Entities.StatusCodes.ContentNotFound)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(vmPage);
                }
            }
            else if (model.Status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(model.RedirectUrl);
            }
            else
            {
                return HttpNotFound();
            }
        }



        [Route("newlaunches/makes/")]
        [DeviceDetection]
        public ActionResult Makes(uint? makeId)
        {
            if (makeId != null)
            {
                ViewBag.BrandCountList = (_newLaunches.GetMakeList(makeId.Value).Take(9));
            }
            else
            {
                ViewBag.BrandCountList = (_newLaunches.GetMakeList().Take(9));
            }
            return PartialView("~/Views/Shared/_NewLaunchedByMakeWithCount.cshtml", ViewBag.BrandCountList);
        }


        /// <summary>
        /// Modified By :- Subodh Jain 17 Feb 2017
        /// Summary ;- Added upcoming widget
        /// </summary>
        /// <param name="launchYear"></param>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [Route("m/newlaunches/make/{maskingName}/")]
        public ActionResult BikeByMake_Mobile(string maskingName, ushort? pageNumber)
        {
            NewLaunchedMakePageModel model = new NewLaunchedMakePageModel(maskingName, _newLaunches, _objMakeCache, _upcoming, Entities.PriceQuote.PQSourceEnum.Desktop_NewLaunchLanding, pageNumber);

            if (model.Status == Entities.StatusCodes.ContentFound)
            {
                model.BaseUrl = String.Format("/m/new-{0}-bike-launches/", maskingName);
                model.PageSize = 10;
                model.MakeTopCount = 9;
                model.IsMobile = true;
                var vmPage = model.GetData();
                if (model.Status == Entities.StatusCodes.ContentNotFound)
                {
                    return HttpNotFound();
                }
                else
                {
                    return View(vmPage);
                }
            }
            else if (model.Status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(model.RedirectUrl);
            }
            else
            {
                return HttpNotFound();
            }
        }



        /// <summary>
        /// Route for new bike launches models
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <returns></returns>
        [Route("m/newlaunches/models/")]
        public ActionResult Models_Mobile()
        {
            return View("~/views/m/shared/_newlaunchedbymakewithoutcount.cshtml");
        }

        [Route("m/newlaunches/makes/")]
        public ActionResult Makes_Mobile(uint? makeId)
        {
            if (makeId != null)
            {
                ViewBag.BrandCountList = _newLaunches.GetMakeList(makeId.Value).Take(9);
            }
            else
            {
                ViewBag.BrandCountList = _newLaunches.GetMakeList().Take(9);
            }
            return PartialView("~/Views/m/Shared/_NewLaunchedByMakeWithCount.cshtml", ViewBag.BrandCountList);
        }

    }
}
