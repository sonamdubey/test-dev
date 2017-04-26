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
    /// Created by : Sangram Nandkhile on 24 March 2017
    /// Summary: Controller to hold homepage related actions
    /// </summary>
    public class HomePageController : Controller
    {
        private readonly IBikeMakes<BikeMakeEntity, int> _bikeMakes = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly ICityCacheRepository _usedBikeCities = null;
        private readonly IHomePageBannerCacheRepository _cachedBanner = null;
        private readonly IBikeModelsCacheRepository<int> _cachedModels = null;
        private readonly IBikeCompareCacheRepository _cachedCompare = null;
        private readonly IUsedBikeDetailsCacheRepository _cachedBikeDetails = null;
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;
        private readonly ICMSCacheContent _expertReviews = null;


        public HomePageController(IBikeMakes<BikeMakeEntity, int> bikeMakes, INewBikeLaunchesBL newLaunches, IBikeModels<BikeModelEntity, int> bikeModels, ICityCacheRepository usedBikeCities, IHomePageBannerCacheRepository cachedBanner, IBikeModelsCacheRepository<int> cachedModels, IBikeCompareCacheRepository cachedCompare, IUsedBikeDetailsCacheRepository cachedBikeDetails, IVideos videos, ICMSCacheContent articles, ICMSCacheContent expertReviews)
        {
            _bikeMakes = bikeMakes;
            _bikeModels = bikeModels;
            _newLaunches = newLaunches;
            _usedBikeCities = usedBikeCities;
            _cachedBanner = cachedBanner;
            _cachedModels = cachedModels;
            _cachedCompare = cachedCompare;
            _cachedBikeDetails = cachedBikeDetails;
            _videos = videos;
            _articles = articles;
            _expertReviews = expertReviews;
        }
        // GET: HomePage
        //[Route("homepage/")]
        [DeviceDetection]
        public ActionResult Index()
        {
            HomePageVM objData = null;
            HomePageModel obj = new HomePageModel(10, 9, _bikeMakes, _newLaunches, _bikeModels, _usedBikeCities, _cachedBanner, _cachedModels, _cachedCompare, _cachedBikeDetails, _videos, _articles, _expertReviews);
            objData = obj.GetData();
            return View(objData);

        }

        // GET: HomePage
        [Route("m/homepage/")]
        public ActionResult Index_Mobile()
        {
            HomePageVM objData = null;
            HomePageModel obj = new HomePageModel(6, 9, _bikeMakes, _newLaunches, _bikeModels, _usedBikeCities, _cachedBanner, _cachedModels, _cachedCompare, _cachedBikeDetails, _videos, _articles, _expertReviews);
            obj.IsMobile = true;
            objData = obj.GetData();
            return View(objData);
        }
    }
}
