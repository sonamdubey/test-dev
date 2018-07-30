using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Filters;
using Bikewale.Interfaces.AdSlot;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.HomePage;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Used;
using Bikewale.Interfaces.UserReviews;
using Bikewale.Interfaces.Videos;
using Bikewale.Models;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created by : Sangram Nandkhile on 24 March 2017
    /// Summary: Controller to hold homepage related actions
    /// Modified by : Aditi Srivastava on 5 June 2017
    /// Summary     : Added BL instance for comparison list
    /// Modified by : Vivek Singh Tomar on 31st July 2017
    /// Summary : Added IUpcoming for filling upcoming bike list
    /// Modified by : Ashutosh Sharma on 31 Oct 2017
    /// Description : Added IAdSlot.
    /// </summary>
    public class HomePageController : Controller
    {
        private readonly IBikeMakesCacheRepository _bikeMakes = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly ICityCacheRepository _usedBikeCities = null;
        private readonly IHomePageBannerCacheRepository _cachedBanner = null;
        private readonly IBikeModelsCacheRepository<int> _cachedModels = null;
        private readonly IBikeCompare _compare = null;
        private readonly IUsedBikeDetailsCacheRepository _cachedBikeDetails = null;
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;
        private readonly IUserReviewsCache _userReviewsCache = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IAdSlot _adSlot = null;
        private readonly IApiGatewayCaller _apiGatewayCaller;

        /// <summary>
        /// Modified by : Ashutosh Sharma on 31 Oct 2017
        /// Description : Added IAdSlot.
        /// </summary>
        public HomePageController(IBikeMakesCacheRepository bikeMakes, INewBikeLaunchesBL newLaunches, IBikeModels<BikeModelEntity, int> bikeModels, ICityCacheRepository usedBikeCities, IHomePageBannerCacheRepository cachedBanner, IBikeModelsCacheRepository<int> cachedModels, IBikeCompare compare, IUsedBikeDetailsCacheRepository cachedBikeDetails, IVideos videos, ICMSCacheContent articles, IUpcoming upcoming, IUserReviewsCache userReviewsCache, IAdSlot adSlot, IApiGatewayCaller apiGatewayCaller)
        {
            _bikeMakes = bikeMakes;
            _bikeModels = bikeModels;
            _newLaunches = newLaunches;
            _usedBikeCities = usedBikeCities;
            _cachedBanner = cachedBanner;
            _cachedModels = cachedModels;
            _compare = compare;
            _cachedBikeDetails = cachedBikeDetails;
            _videos = videos;
            _articles = articles;
            _userReviewsCache = userReviewsCache;
            _upcoming = upcoming;
            _adSlot = adSlot;
            _apiGatewayCaller = apiGatewayCaller;
        }
        // GET: HomePage
        //[Route("homepage/")]
        /// <summary>
        /// Modified by : Aditi Srivastava on 6 June 2017
        /// Summary     : Added compare source
        /// Modified by : Ashutosh Sharma on 31 Oct 2017
        /// Description : Added _adSlot in HomePageModel object creation.
        /// </summary>
        [DeviceDetection]
        public ActionResult Index()
        {
            HomePageVM objData = null;
            HomePageModel obj = new HomePageModel(10, 9, 9, _bikeMakes, _newLaunches, _bikeModels, _usedBikeCities, _cachedBanner, _cachedModels, _compare, _cachedBikeDetails, _videos, _articles, _upcoming, _userReviewsCache, _adSlot, _apiGatewayCaller);
            obj.CompareSource = CompareSources.Desktop_Featured_Compare_Widget;
            objData = obj.GetData();
            return View(objData);

        }

        // GET: HomePage
        /// <summary>
        /// Modified by : Ashutosh Sharma on 31 Oct 2017
        /// Description : Added _adSlot in HomePageModel object creation.
        /// </summary>
        /// <returns></returns>
        [Route("m/homepage/")]
        public ActionResult Index_Mobile()
        {
            HomePageVM objData = null; HomePageModel obj = new HomePageModel(6, 9, 9, _bikeMakes, _newLaunches, _bikeModels, _usedBikeCities, _cachedBanner, _cachedModels, _compare, _cachedBikeDetails, _videos, _articles, _upcoming, _userReviewsCache, _adSlot, _apiGatewayCaller);
            obj.IsMobile = true;
            obj.CompareSource = CompareSources.Mobile_Featured_Compare_Widget;
            objData = obj.GetData();
            return View(objData);
        }
    }
}
