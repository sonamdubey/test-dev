using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Pager;
using Bikewale.Models;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class BikeCareController : Controller
    {
        #region Variables for dependency injection
        private readonly ICMSCacheContent _cmsCache = null;
        private readonly IPager _objPager = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private readonly IBikeMakesCacheRepository _bikeMakesCacheRepository = null;
        private readonly IBikeSeriesCacheRepository _seriesCache = null;
        private readonly IBikeSeries _series = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _modelMaskingCacheRepo = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objBikeVersionsCache = null;
        #endregion

        #region Constructor
        public BikeCareController(ICMSCacheContent cmsCache, IPager objPager, IUpcoming upcoming, IBikeModels<BikeModelEntity, int> bikeModels, IBikeModelsCacheRepository<int> models)
        {
            _cmsCache = cmsCache;
            _objPager = objPager;
            _upcoming = upcoming;
            _bikeModels = bikeModels;
            _models = models;
        }
        #endregion

        #region Action Methods
        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Action method for bike care listing -desktop
        /// </summary>
        [Route("bikecare/index/")]
        [Filters.DeviceDetection()]
        public ActionResult Index()
        {
            BikeCareIndexPage obj = new BikeCareIndexPage(_cmsCache, _objPager, _upcoming, _bikeModels, _bikeMakesCacheRepository, _models, _series);
            if (obj.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/pagenotfound.aspx");
            }
            else
            {
                BikeCareIndexPageVM objData = new BikeCareIndexPageVM();
                objData = obj.GetData(4);
                if (obj.status == Entities.StatusCodes.ContentFound)
                    return View(objData);
                else
                    return Redirect("/pageNotFound.aspx");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Action method for bike care listing - Mobile
        /// </summary>
        [Route("m/bikecare/index/")]
        public ActionResult Index_Mobile()
        {
            BikeCareIndexPage obj = new BikeCareIndexPage(_cmsCache, _objPager, _upcoming, _bikeModels, _bikeMakesCacheRepository, _models, _series);
            obj.IsMobile = true;
            if (obj.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/m/pagenotfound.aspx");
            }
            else
            {
                BikeCareIndexPageVM objData = new BikeCareIndexPageVM();
                objData = obj.GetData(9);
                if (obj.status == Entities.StatusCodes.ContentFound)
                    return View(objData);
                else
                    return Redirect("/m/pageNotFound.aspx");
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Action method for bike care deatils - desktop
        /// </summary>
        [Route("bikecare/details/{basicid}/")]
        [Filters.DeviceDetection()]
        public ActionResult Detail(string basicid)
        {
            BikeCareDetailPage obj = new BikeCareDetailPage(_cmsCache, _upcoming, _bikeModels, _models, basicid, _bikeMakesCacheRepository, _seriesCache, _series, _modelMaskingCacheRepo, _objBikeVersionsCache);
            if (obj.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/pagenotfound.aspx");
            }
            else
            {
                BikeCareDetailPageVM objData = obj.GetData(3);
                if (obj.status == Entities.StatusCodes.ContentNotFound)
                    return Redirect("/pagenotfound.aspx");
                else
                    return View(objData);
            }
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 1 Apr 2017
        /// Summary    : Action method for bike care details - Mobile
        /// </summary>
        [Route("m/bikecare/details/{basicid}/")]
        public ActionResult Detail_Mobile(string basicid)
        {
            BikeCareDetailPage obj = new BikeCareDetailPage(_cmsCache, _upcoming, _bikeModels, _models, basicid, _bikeMakesCacheRepository, _seriesCache, _series, _modelMaskingCacheRepo, _objBikeVersionsCache);
            if (obj.status == Entities.StatusCodes.ContentNotFound)
            {
                return Redirect("/m/pagenotfound.aspx");
            }
            else
            {
                obj.IsMobile = true;
                BikeCareDetailPageVM objData = obj.GetData(9);
                if (obj.status == Entities.StatusCodes.ContentNotFound)
                    return Redirect("/m/pagenotfound.aspx");
                else
                    return View(objData);
            }
        }
        #endregion
    }
}