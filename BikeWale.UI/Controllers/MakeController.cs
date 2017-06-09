﻿using Bikewale.Common;
using Bikewale.Entities;
using Bikewale.Entities.Compare;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.ServiceCenter;
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
    /// Created by: Sangram Nandkhile on 27-Mar-2017
    /// Summary: Controller which holds actions for Make
    /// Modified by : Aditi Srivastava on 5 June 2017
    /// Summary     : Added BL instance for comparison list
    /// </author>
    public class MakeController : Controller
    {
        private readonly IDealerCacheRepository _dealerServiceCenters = null;
        private readonly IBikeModelsCacheRepository<int> _bikeModelsCache;
        private readonly IBikeMakesCacheRepository<int> _bikeMakesCache;
        private readonly ICMSCacheContent _articles = null;
        private readonly ICMSCacheContent _expertReviews = null;
        private readonly IVideos _videos = null;
        private readonly IUsedBikeDetailsCacheRepository _cachedBikeDetails;
        private readonly IDealerCacheRepository _cacheDealers;
        private readonly IUpcoming _upcoming = null;
        private readonly IBikeCompare _compareBikes;
        private readonly IServiceCenter _objService;

        public MakeController(IDealerCacheRepository dealerServiceCenters, IBikeModelsCacheRepository<int> bikeModelsCache, IBikeMakesCacheRepository<int> bikeMakesCache, ICMSCacheContent articles, ICMSCacheContent expertReviews, IVideos videos, IUsedBikeDetailsCacheRepository cachedBikeDetails, IDealerCacheRepository cacheDealers, IUpcoming upcoming, IBikeCompare compareBikes, IServiceCenter objService)
        {
            _dealerServiceCenters = dealerServiceCenters;
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
        }
        // GET: Makes
        [Route("makepage/{makeMaskingName}/")]
        [Bikewale.Filters.DeviceDetection]
        public ActionResult Index(string makeMaskingName)
        {
            MakePageModel obj = new MakePageModel(makeMaskingName, 9, _dealerServiceCenters, _bikeModelsCache, _bikeMakesCache, _articles, _expertReviews, _videos, _cachedBikeDetails, _cacheDealers, _upcoming,_compareBikes, _objService);
            obj.CompareSource = CompareSources.Desktop_Make_Similar_Compare_Widget;
            MakePageVM objData = new MakePageVM();
            if (obj != null)
            {
                if (obj.status == StatusCodes.ContentFound)
                {
                    objData = obj.GetData();
                    return View(objData);
                }
                else if (obj.status == StatusCodes.RedirectPermanent)
                {
                    return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, obj.objResponse.MaskingName));
                }
                else if (obj.status == StatusCodes.RedirectTemporary)
                {
                    return Redirect(obj.redirectUrl);
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            else
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

        // GET: Makes
        [Route("m/makepage/{makeMaskingName}/")]
        public ActionResult Index_Mobile(string makeMaskingName)
        {
            MakePageModel obj = new MakePageModel(makeMaskingName, 9, _dealerServiceCenters, _bikeModelsCache, _bikeMakesCache, _articles, _expertReviews, _videos, _cachedBikeDetails, _cacheDealers, _upcoming, _compareBikes, _objService);
            obj.CompareSource = CompareSources.Mobile_Make_Similar_Compare_Widget;
            MakePageVM objData = new MakePageVM();
            if (obj != null)
            {
                if (obj.status == StatusCodes.ContentFound)
                {
                    objData = obj.GetData();
                    return View(objData);
                }
                else if (obj.status == StatusCodes.RedirectPermanent)
                {
                    return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, obj.objResponse.MaskingName));
                }
                else if (obj.status == StatusCodes.RedirectTemporary)
                {
                    return Redirect(obj.redirectUrl);
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            else
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }
    }
}