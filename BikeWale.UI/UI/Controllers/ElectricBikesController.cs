using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Videos;
using Bikewale.Models;
using System.Web.Mvc;

namespace Bikewale.Controllers
{

    /// <summary>
    /// Created By :- Subodh Jain 07-12-2017
    /// Summary :- Controller for Electric Bikes
    /// </summary>
    /// <returns></returns>
    public class ElectricBikesController : Controller
    {
        private readonly IBikeMakesCacheRepository _objMakeCache = null;
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;
        private readonly IBikeMakesCacheRepository _bikeMakes = null;
        private readonly IBikeModelsCacheRepository<int> _modelCacheRepository = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels;

        public ElectricBikesController(IBikeModelsCacheRepository<int> modelCacheRepository, IBikeMakesCacheRepository objMakeCache, ICMSCacheContent articles, IVideos videos, IBikeMakesCacheRepository bikeMakes, IBikeModels<BikeModelEntity, int> bikeModels)
        {
            _objMakeCache = objMakeCache;
            _articles = articles;
            _videos = videos;
            _bikeMakes = bikeMakes;
            _modelCacheRepository = modelCacheRepository;
            _bikeModels = bikeModels;
        }
        // GET: ElectricBikes
        /// <summary>
        /// Created By :- Subodh Jain 07-12-2017
        /// Summary :- Method for Index Desktop
        /// </summary>
        /// <returns></returns>
        [Route("electric/index/")]
        public ActionResult Index()
        {
            ElectricBikesPageVM objData = null;
            ElectricBikesPage objElectricBike = new ElectricBikesPage(_modelCacheRepository, _objMakeCache, _articles, _videos, _bikeMakes, _bikeModels);

            objElectricBike.TopCountBrand = 10;
            objElectricBike.EditorialTopCount = 3;
            objData = objElectricBike.GetData();
            objData.PageCatId = 0;
            return View(objData);
        }

        /// <summary>
        /// Created By :- Subodh Jain 07-12-2017
        /// Summary :- Method for Index Mobile
        /// </summary>
        /// <returns></returns>
        [Route("m/electric/index/")]
        public ActionResult Index_Mobile()
        {
            ElectricBikesPageVM objData = null;
            ElectricBikesPage objElectricBike = new ElectricBikesPage(_modelCacheRepository, _objMakeCache, _articles, _videos, _bikeMakes, _bikeModels);
            objElectricBike.TopCountBrand = 6;
            objElectricBike.IsMobile = true;
            objElectricBike.EditorialTopCount = 3;
            objData = objElectricBike.GetData();
            objData.PageCatId = 0;
            return View(objData);
        }

    }
}