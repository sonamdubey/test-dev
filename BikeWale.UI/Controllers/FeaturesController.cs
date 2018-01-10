using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Pager;
using Bikewale.Models;
using Bikewale.Models.Features;
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
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objBikeVersionsCache = null;
        private readonly IBikeInfo _bikeInfo = null;       
        private readonly ICityCacheRepository _cityCacheRepo;
        private readonly IBikeMakesCacheRepository _bikeMakesCacheRepository = null;

        public FeaturesController(ICMSCacheContent Cache, IPager objPager, IUpcoming upcoming, IBikeModels<BikeModelEntity, int> bikeModels, IBikeModelsCacheRepository<int> models, IBikeVersionCacheRepository<BikeVersionEntity, uint> objBikeVersionsCache, IBikeInfo bikeInfo, ICityCacheRepository cityCacheRepo, IBikeMakesCacheRepository bikeMakesCacheRepository)
        {
            _Cache = Cache;
            _objPager = objPager;
            _upcoming = upcoming;
            _bikeModels = bikeModels;
            _models = models;
            _objBikeVersionsCache = objBikeVersionsCache;
            _bikeMakesCacheRepository = bikeMakesCacheRepository;
            _cityCacheRepo = cityCacheRepo;
            _bikeInfo = bikeInfo;
        }

        /// <summary>
        /// Created by :- Subodh Jain on 31 March 2017
        /// Summary :- Index Method for Features news section
        /// </summary>
        /// <returns></returns>
        [Route("features/")]
        [Filters.DeviceDetection()]
        public ActionResult Index()
        {
            IndexPage obj = new IndexPage(_Cache, _objPager, _upcoming, _bikeModels);
            if (obj.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/pagenotfound.aspx");
            }
            else
            {
                IndexFeatureVM objData = new IndexFeatureVM();
                objData = obj.GetData(4);
                if (obj.status == Entities.StatusCodes.ContentFound)
                    return View(objData);
                else
                    return Redirect("/pageNotFound.aspx");
            }
        }
        /// <summary>
        /// Created by :- Subodh Jain on 31 March 2017
        /// Summary :- Index Method for Features news section Mobile
        /// </summary>
        /// <returns></returns>
        [Route("m/features/")]
        public ActionResult Index_Mobile()
        {
            IndexPage obj = new IndexPage(_Cache, _objPager, _upcoming, _bikeModels);
            obj.IsMobile = true;
            if (obj.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/m/pagenotfound.aspx");
            }
            else
            {
                IndexFeatureVM objData = new IndexFeatureVM();
                objData = obj.GetData(9);
                if (obj.status == Entities.StatusCodes.ContentFound)
                    return View(objData);
                else
                    return Redirect("/m/pageNotFound.aspx");
            }
        }

        /// <summary>
        ///Created By:- Subodh Jain 31 March 2017
        ///Summary :- Detail Page Feature Desktop
        /// </summary>
        /// <returns></returns>
        [Route("features/detail/{basicid}/")]
        [Filters.DeviceDetection()]
        public ActionResult Detail(string basicId)
        {
            DetailPage objDetail = new DetailPage(_Cache, _upcoming, _bikeModels, _models, basicId, _objBikeVersionsCache, _bikeInfo, _cityCacheRepo, _bikeMakesCacheRepository);
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
                DetailFeatureVM objData = objDetail.GetData(4);
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
        [Route("m/features/detail/{basicid}/")]
        public ActionResult Detail_Mobile(string basicId)
        {
            DetailPage objDetail = new DetailPage(_Cache, _upcoming, _bikeModels, _models, basicId, _objBikeVersionsCache, _bikeInfo, _cityCacheRepo, _bikeMakesCacheRepository);
            objDetail.IsMobile = true;
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
                DetailFeatureVM objData = objDetail.GetData(9);
                if (objDetail.status == Entities.StatusCodes.ContentNotFound)
                    return Redirect("/m/pageNotFound.aspx");
                else
                    return View(objData);
            }
        }

        /// <summary>
        /// Action to get the map features details page
        /// Modifid by: Vivek Singh Tomar on 31st Aug 2017
        /// Summary: Removed use of viewbags by VM
        /// Modified by : Ashutosh Sharma on 27 Oct 2017
        /// Description : Setting property 'IsAMPPage'
        /// </summary>
        /// <param name="basicid"></param>
        /// <returns></returns>
        [Route("m/features/details/{basicid}/amp/")]
        public ActionResult DetailsAMP(string basicid)
        {
            DetailPage objDetail = new DetailPage(_Cache, _upcoming, _bikeModels, _models, basicid, _objBikeVersionsCache, _bikeInfo, _cityCacheRepo, _bikeMakesCacheRepository);
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
                objDetail.IsAMPPage = true;
                DetailFeatureVM objData = objDetail.GetData(9);
                if (objDetail.status == Entities.StatusCodes.ContentNotFound)
                    return Redirect("/m/pageNotFound.aspx");
                else
                    return View("~/views/m/content/features/details_amp.cshtml", objData);
            }
        }
    }
}