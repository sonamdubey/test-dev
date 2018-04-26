using Bikewale.Entities;
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
    public class ComparisonTestsController : Controller
    {
        #region Variables for dependency injection
        private readonly ICMSCacheContent _cmsCache = null;
        private readonly IPager _pager = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IUpcoming _upcoming = null;
        private readonly ICityCacheRepository _cityCache = null;
        private readonly IBikeMakesCacheRepository _objMakeCache = null;
        private readonly IBikeModelsCacheRepository<int> _models = null;
        private readonly IBikeSeries _series;
        #endregion

        #region Constructor
        public ComparisonTestsController(ICMSCacheContent cmsCache, IPager pager, IBikeModels<BikeModelEntity, int> bikeModels, IUpcoming upcoming, ICityCacheRepository cityCache, IBikeMakesCacheRepository objMakeCache, IBikeModelsCacheRepository<int> models, IBikeSeries series)
        {
            _cmsCache = cmsCache;
            _pager = pager;
            _bikeModels = bikeModels;
            _upcoming = upcoming;
            _cityCache = cityCache;
            _objMakeCache = objMakeCache;
            _models = models;
            _series = series;
        }
        #endregion
        #region Action Methods
        /// <summary>
        /// Created by : Aditi srivastava on 8 May 2017
        /// Summmary   : Action method to render comparison test listing page- Desktop
        /// </summary>
        [Route("comparisontests/index/")]
        [Filters.DeviceDetection()]
        public ActionResult Index()
        {
            ComparisonTestsIndexPage obj = new ComparisonTestsIndexPage(_cmsCache, _pager, _bikeModels, _upcoming, _objMakeCache, _models, _series);
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
                ComparisonTestsIndexPageVM objData = obj.GetData(4);
                if (obj.status == StatusCodes.ContentNotFound)
                    return Redirect("/pagenotfound.aspx");
                else
                    return View(objData);
            }
        }

        /// <summary>
        /// Created by : Aditi srivastava on 8 May 2017
        /// Summmary   : Action method to render comparison tests listing page - mobile
        /// </summary>
        [Route("m/comparisontests/index/")]
        public ActionResult Index_Mobile()
        {
            ComparisonTestsIndexPage obj = new ComparisonTestsIndexPage(_cmsCache, _pager, _bikeModels, _upcoming, _objMakeCache, _models, _series);
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
                ComparisonTestsIndexPageVM objData = obj.GetData(9);
                if (obj.status == StatusCodes.ContentNotFound)
                    return Redirect("/m/pagenotfound.aspx");
                else
                    return View(objData);
            }
        }
        #endregion
    }
}