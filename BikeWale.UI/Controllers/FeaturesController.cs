using Bikewale.CoreDAL;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Models;
using Bikewale.Models.Features;
using Bikewale.Notifications;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class FeaturesController : Controller
    {
        private readonly ICMSCacheContent _Cache = null;
        private readonly IPager _objPager = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;

        public FeaturesController(ICMSCacheContent Cache, IPager objPager, IUpcoming upcoming, IBikeModels<BikeModelEntity, int> bikeModels, IBikeModelsCacheRepository<int> models)
        {
            _Cache = Cache;
            _objPager = objPager;
            _upcoming = upcoming;
            _bikeModels = bikeModels;
            _models = models;
        }

        /// <summary>
        /// Created by :- Subodh Jain on 31 March 2017
        /// Summary :- Index Method for Features news section
        /// </summary>
        /// <returns></returns>
        [Route("features/")]
        [Filters.DeviceDetection()]
        public ActionResult Index(ushort? pageNumber)
        {
            IndexPage objIndexPage = new IndexPage(_Cache, _objPager, _upcoming, _bikeModels);
            IndexFeatureVM objFeatureIndex = new IndexFeatureVM();
            objIndexPage.TopCount = 4;
            if (pageNumber.HasValue && pageNumber.Value > 0)
                objIndexPage.CurPageNo = pageNumber.Value;
            objFeatureIndex = objIndexPage.GetData();
            if (objIndexPage.status == Entities.StatusCodes.ContentFound)
                return View(objFeatureIndex);
            else
                return Redirect("/pageNotFound.aspx");
        }
        /// <summary>
        /// Created by :- Subodh Jain on 31 March 2017
        /// Summary :- Index Method for Features news section Mobile
        /// </summary>
        /// <returns></returns>
        [Route("m/features/")]
        public ActionResult Index_Mobile(ushort? pageNumber)
        {

            IndexPage objIndexPage = new IndexPage(_Cache, _objPager, _upcoming, _bikeModels);
            IndexFeatureVM objFeatureIndex = new IndexFeatureVM();
            objIndexPage.TopCount = 9;
            if (pageNumber.HasValue && pageNumber.Value > 0)
                objIndexPage.CurPageNo = pageNumber.Value;
            objFeatureIndex = objIndexPage.GetData();
            if (objIndexPage.status == Entities.StatusCodes.ContentFound)
                return View(objFeatureIndex);
            else
                return Redirect("/m/pageNotFound.aspx");

        }

        /// <summary>
        ///Created By:- Subodh Jain 31 March 2017
        ///Summary :- Detail Page Feature Desktop
        /// </summary>
        /// <returns></returns>
        [Route("content/features/details/")]
        [Filters.DeviceDetection()]
        public ActionResult Detail(string basicId)
        {
            DetailPage objDetail = new DetailPage(_Cache, _upcoming, _bikeModels, _models, basicId);
            objDetail.TopCount = 4;

            if (objDetail.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/pagenotfound.aspx");
            }
            else if (objDetail.status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(objDetail.redirectUrl);
            }
            else
            {
                DetailFeatureVM objData = objDetail.GetData();
                if (objDetail.status == Entities.StatusCodes.ContentNotFound)
                    return Redirect("/pagenotfound.aspx");
                else
                    return View(objData);
            }

        }
        /// <summary>
        ///Created By:- Subodh Jain 31 March 2017
        ///Summary :- Detail Page Feature Mobile
        /// </summary>
        /// <returns></returns>
        [Route("m/content/features/details/")]
        public ActionResult Detail_Mobile(string basicId)
        {
            DetailPage objDetail = new DetailPage(_Cache, _upcoming, _bikeModels, _models, basicId);
            objDetail.TopCount = 9;

            if (objDetail.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/pagenotfound.aspx");
            }
            else if (objDetail.status == Entities.StatusCodes.RedirectPermanent)
            {
                return Redirect(string.Format("/m{0}", objDetail.redirectUrl));
            }
            else
            {
                DetailFeatureVM objData = objDetail.GetData();
                if (objDetail.status == Entities.StatusCodes.ContentNotFound)
                    return Redirect("/m/pageNotFound.aspx");
                else
                    return View(objData);
            }
        }
    }
}