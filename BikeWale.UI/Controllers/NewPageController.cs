using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Filters;
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
    /// </summary>
    /// <seealso cref="System.Web.Mvc.Controller" />
    /// <author>
    /// Created by: Sangram Nandkhile on 31-Mar-2017
    /// Summary: Controller which holds actions for New page
    /// Modified by : Aditi Srivastava on 5 June 2017
    /// Summary     : Added BL instance for comparison list
    /// Modified by : Vivek Singh Tomar on 31st July 2017
    /// Summary : Added IUpcoming for filling upcoming bike list
    /// </author>
    public class NewPageController : Controller
    {
        private readonly IBikeMakesCacheRepository _bikeMakes = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly ICityCacheRepository _usedBikeCities = null;
        private readonly IHomePageBannerCacheRepository _cachedBanner = null;
        private readonly IBikeModelsCacheRepository<int> _cachedModels = null;
        private readonly IBikeCompare _compare = null;
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;
        private readonly ICMSCacheContent _expertReviews = null;
        private readonly IUserReviewsCache _userReviewsCache = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IApiGatewayCaller _apiGatewayCaller;

        NewPageVM objData = null;

        public NewPageController(IBikeMakesCacheRepository bikeMakes, INewBikeLaunchesBL newLaunches, IBikeModels<BikeModelEntity, int> bikeModels, ICityCacheRepository usedBikeCities, IHomePageBannerCacheRepository cachedBanner, IBikeModelsCacheRepository<int> cachedModels, IBikeCompare compare, IUsedBikeDetailsCacheRepository cachedBikeDetails, IVideos videos, ICMSCacheContent articles, ICMSCacheContent expertReviews, IUpcoming upcoming, IUserReviewsCache userReviewsCache, IApiGatewayCaller apiGatewayCaller)

        {
            _bikeMakes = bikeMakes;
            _bikeModels = bikeModels;
            _newLaunches = newLaunches;
            _usedBikeCities = usedBikeCities;
            _cachedBanner = cachedBanner;
            _cachedModels = cachedModels;
            _compare = compare;
            _videos = videos;
            _articles = articles;
            _expertReviews = expertReviews;
            _userReviewsCache = userReviewsCache;
            _upcoming = upcoming;
            _apiGatewayCaller = apiGatewayCaller;
        }
        // GET: HomePage
        /// <summary>
        /// Modified by : Aditi Srivastava on 6 June 2017
        /// Summary     : Added compare source
        /// </summary>
        [Route("newpage/")]
        [DeviceDetection]
        public ActionResult Index()
        {
            NewPageModel obj = new NewPageModel(10, 9, 9, _bikeMakes, _newLaunches, _bikeModels, _cachedModels, _compare, _videos, _articles, _expertReviews, _upcoming, _userReviewsCache, _apiGatewayCaller);
            obj.CompareSource = CompareSources.Desktop_Featured_Compare_Widget;
            objData = obj.GetData();
            return View(objData);
        }

        // GET: HomePage
        [Route("m/newpage/")]
        public ActionResult Index_Mobile()
        {
            NewPageModel obj = new NewPageModel(6, 9, 9, _bikeMakes, _newLaunches, _bikeModels, _cachedModels, _compare, _videos, _articles, _expertReviews, _upcoming, _userReviewsCache, _apiGatewayCaller);

            obj.IsMobile = true;
            obj.CompareSource = CompareSources.Mobile_Featured_Compare_Widget;
            objData = obj.GetData();
            return View(objData);
        }
    }
}