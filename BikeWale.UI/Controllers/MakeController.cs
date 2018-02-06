using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Compare;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.ServiceCenter;
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
    /// Created by: Sangram Nandkhile on 27-Mar-2017
    /// Summary: Controller which holds actions for Make
    /// Modified by : Aditi Srivastava on 5 June 2017
    /// Summary     : Added BL instance for comparison list
    /// Modified By : Snehal Dange on 21st Nov 2017
    /// Description : Added IUserReviewsCache
    /// </author>
    public class MakeController : Controller
    {
        private readonly IBikeModelsCacheRepository<int> _bikeModelsCache;
        private readonly IBikeMakesCacheRepository _bikeMakesCache;
        private readonly ICMSCacheContent _articles = null;
        private readonly ICMSCacheContent _expertReviews = null;
        private readonly IVideos _videos = null;
        private readonly IUsedBikeDetailsCacheRepository _cachedBikeDetails;
        private readonly IBikeModels<BikeModelEntity, int> _objModelEntity = null;
        private readonly IDealerCacheRepository _cacheDealers;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeCompare _compareBikes;
        private readonly IServiceCenter _objService;
        private readonly IUserReviewsCache _cacheUserReviews;

        public MakeController(IBikeModelsCacheRepository<int> bikeModelsCache, IBikeModels<BikeModelEntity, int> objModelEntity, IBikeMakesCacheRepository bikeMakesCache, ICMSCacheContent articles, ICMSCacheContent expertReviews, IVideos videos, IUsedBikeDetailsCacheRepository cachedBikeDetails, IDealerCacheRepository cacheDealers, IUpcoming upcoming, IBikeCompare compareBikes, IServiceCenter objService, IUserReviewsCache cacheUserReviews)
        {
            _bikeModelsCache = bikeModelsCache;
            _bikeMakesCache = bikeMakesCache;
            _articles = articles;
            _expertReviews = expertReviews;
            _videos = videos;
            _cachedBikeDetails = cachedBikeDetails;
            _cacheDealers = cacheDealers;
            _objService = objService;
            _upcoming = upcoming;
            _compareBikes = compareBikes;
            _cacheUserReviews = cacheUserReviews;
            _objModelEntity = objModelEntity;
        }
        // GET: Makes

        [Route("makepage/{makeMaskingName}/")]
        [Bikewale.Filters.DeviceDetection]
        public ActionResult Index(string makeMaskingName)
        {
            MakePageModel obj = new MakePageModel(makeMaskingName, _objModelEntity, _bikeModelsCache, _bikeMakesCache, _articles, _expertReviews, _videos, _cachedBikeDetails, _cacheDealers, _upcoming, _compareBikes, _objService, _cacheUserReviews);
            obj.CompareSource = CompareSources.Desktop_Featured_Compare_Widget;
            MakePageVM objData = null;

            if (obj.Status == StatusCodes.ContentFound)
            {
                objData = obj.GetData();
                return View(objData);
            }
            else if (obj.Status == StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(obj.RedirectUrl);
            }
            else if (obj.Status == StatusCodes.RedirectTemporary)
            {
                return Redirect(obj.RedirectUrl);
            }
            else
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }

        }

        //// GET: Makes
        //[Route("m/makepage/{makeMaskingName}/")]
        //public ActionResult Index_Mobile(string makeMaskingName)
        //{
        //    MakePageModel obj = new MakePageModel(makeMaskingName, _bikeModelsCache, _bikeMakesCache, _articles, _expertReviews, _videos, _cachedBikeDetails, _cacheDealers, _upcoming, _compareBikes, _objService, _cacheUserReviews);
        //    obj.CompareSource = CompareSources.Mobile_Featured_Compare_Widget;
        //    MakePageVM objData = null;

        //    if (obj.Status == StatusCodes.ContentFound)
        //    {
        //        obj.IsMobile = true;
        //        objData = obj.GetData();
        //        return View(objData);
        //    }
        //    else if (obj.Status == StatusCodes.RedirectPermanent)
        //    {
        //        return RedirectPermanent(obj.RedirectUrl);
        //    }
        //    else if (obj.Status == StatusCodes.RedirectTemporary)
        //    {
        //        return Redirect(obj.RedirectUrl);
        //    }
        //    else
        //    {
        //        return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
        //    }

        //}

        // GET: Makes
        [Route("m/makepage/{makeMaskingName}/")]
        public ActionResult Index_Mobile_New(string makeMaskingName)
        {
            MakePageModel obj = new MakePageModel(makeMaskingName, _objModelEntity, _bikeModelsCache, _bikeMakesCache, _articles, _expertReviews, _videos, _cachedBikeDetails, _cacheDealers, _upcoming, _compareBikes, _objService, _cacheUserReviews);
            obj.CompareSource = CompareSources.Mobile_Featured_Compare_Widget;
            MakePageVM objData = null;

            if (obj.Status == StatusCodes.ContentFound)
            {
                obj.IsMobile = true;
                objData = obj.GetData();
                return View(objData);
            }
            else if (obj.Status == StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(obj.RedirectUrl);
            }
            else if (obj.Status == StatusCodes.RedirectTemporary)
            {
                return Redirect(obj.RedirectUrl);
            }
            else
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }


        // GET: Makes

        /// <summary>
        /// Created by : Ashutosh Sharma on 25 Oct 2017
        /// Description : Action method for Make AMP page.
        /// </summary>
        /// <param name="makeMaskingName"></param>
        /// <returns></returns>
        [Route("m/makepage/{makeMaskingName}/amp/")]
        public ActionResult Index_Mobile_AMP(string makeMaskingName)
        {
            MakePageModel obj = new MakePageModel(makeMaskingName, _objModelEntity, _bikeModelsCache, _bikeMakesCache, _articles, _expertReviews, _videos, _cachedBikeDetails, _cacheDealers, _upcoming, _compareBikes, _objService, _cacheUserReviews);
            obj.CompareSource = CompareSources.Mobile_Featured_Compare_Widget;
            MakePageVM objData = null;

            if (obj.Status == StatusCodes.ContentFound)
            {
                obj.IsAmpPage = true;
                obj.IsMobile = true;
                objData = obj.GetData();
                return View(objData);
            }
            else if (obj.Status == StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(obj.RedirectUrl);
            }
            else if (obj.Status == StatusCodes.RedirectTemporary)
            {
                return Redirect(obj.RedirectUrl);
            }
            else
            {
                return Redirect("/pageNotFound.aspx");
            }
        }

    }


}