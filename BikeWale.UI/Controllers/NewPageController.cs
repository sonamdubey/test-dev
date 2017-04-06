using Bikewale.Entities.BikeData;
using Bikewale.Filters;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.HomePage;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.Videos;
using Bikewale.Models;
using System.Web.Mvc;


namespace Bikewale.Controllers
{
    /// <summary>
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    /// <author>
    /// Created by: Sangram Nandkhile on 31-Mar-2017
    /// Summary: Controller which holds actions for New page
    /// </author>
    public class NewPageController : Controller
    {
        private readonly IBikeMakes<BikeMakeEntity, int> _bikeMakes = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly ICityCacheRepository _usedBikeCities = null;
        private readonly IHomePageBannerCacheRepository _cachedBanner = null;
        private readonly IBikeModelsCacheRepository<int> _cachedModels = null;
        private readonly IBikeCompareCacheRepository _cachedCompare = null;
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;
        private readonly ICMSCacheContent _expertReviews = null;

        NewPageVM objData = null;

        public NewPageController(IBikeMakes<BikeMakeEntity, int> bikeMakes, INewBikeLaunchesBL newLaunches, IBikeModels<BikeModelEntity, int> bikeModels, ICityCacheRepository usedBikeCities, IHomePageBannerCacheRepository cachedBanner, IBikeModelsCacheRepository<int> cachedModels, IBikeCompareCacheRepository cachedCompare, IUsedBikeDetailsCacheRepository cachedBikeDetails, IVideos videos, ICMSCacheContent articles, ICMSCacheContent expertReviews)
        {
            _bikeMakes = bikeMakes;
            _bikeModels = bikeModels;
            _newLaunches = newLaunches;
            _usedBikeCities = usedBikeCities;
            _cachedBanner = cachedBanner;
            _cachedModels = cachedModels;
            _cachedCompare = cachedCompare;
            _videos = videos;
            _articles = articles;
            _expertReviews = expertReviews;
        }
        // GET: HomePage
        [Route("newpage/")]
        [DeviceDetection]
        public ActionResult Index()
        {
            NewPageModel obj = new NewPageModel(10, 9, _bikeMakes, _newLaunches, _bikeModels, _usedBikeCities, _cachedModels, _cachedCompare, _videos, _articles, _expertReviews);
            objData = obj.GetData();
            return View(objData);
        }

        // GET: HomePage
        [Route("m/newpage/")]
        public ActionResult Index_Mobile()
        {
            NewPageModel obj = new NewPageModel(6, 9, _bikeMakes, _newLaunches, _bikeModels, _usedBikeCities, _cachedModels, _cachedCompare, _videos, _articles, _expertReviews);
            objData = obj.GetData();
            return View(objData);
        }
    }
}