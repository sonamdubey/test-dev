using Bikewale.Comparison.Interface;
using Bikewale.CoreDAL;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Models;
using System;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created By :- Subodh Jain 10 May 2017
    /// Summary :- Compare Bike controller
    /// </summary>
    public class CompareBikesController : Controller
    {

        private readonly IBikeCompareCacheRepository _cachedCompare = null;
        private readonly ICMSCacheContent _compareTest = null;
        private readonly IBikeMakesCacheRepository<int> _objMakeCache = null;
        private readonly IBikeMaskingCacheRepository<BikeModelEntity, int> _objModelMaskingCache = null;
        private readonly IBikeCompare _objCompare = null;
        private readonly ISponsoredCampaignRepository _objSponsored = null;
        public CompareBikesController(IBikeCompareCacheRepository cachedCompare, ICMSCacheContent compareTest, IBikeMaskingCacheRepository<BikeModelEntity, int> objModelMaskingCache, IBikeCompare objCompare, IBikeMakesCacheRepository<int> objMakeCache, ISponsoredCampaignRepository objSponsored)
        {
            _cachedCompare = cachedCompare;
            _compareTest = compareTest;
            _objModelMaskingCache = objModelMaskingCache;
            _objCompare = objCompare;
            _objMakeCache = objMakeCache;
            _objSponsored = objSponsored;
        }

        // GET: CompareBikes
        [Route("compare/")]
        [Filters.DeviceDetection()]
        public ActionResult Index()
        {
            CompareIndex objCompare = new CompareIndex(_objCompare, _compareTest);

            if (objCompare != null)
            {
                CompareVM CompareVM = null;

                CompareVM = objCompare.GetData();
                return View(CompareVM);
            }
            else
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }
        // GET: CompareBikes
        [Route("m/compare/")]
        public ActionResult Index_Mobile()
        {
            CompareIndex objCompare = new CompareIndex(_objCompare, _compareTest);

            if (objCompare != null)
            {
                CompareVM CompareVM = null;

                CompareVM = objCompare.GetData();
                return View(CompareVM);
            }
            else
            {
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

        // GET: CompareBikes Details
        [Route("compare/details/")]
        [Filters.DeviceDetection()]
        public ActionResult CompareBikeDetails()
        {
            string originalUrl = Request.ServerVariables["HTTP_X_ORIGINAL_URL"];
            if (String.IsNullOrEmpty(originalUrl))
                originalUrl = Request.ServerVariables["URL"];
            CompareDetails objDetails = new CompareDetails(_compareTest, _objModelMaskingCache, _cachedCompare, _objCompare, _objMakeCache, _objSponsored, originalUrl);
            if (objDetails != null)
            {
                if (objDetails.status == Entities.StatusCodes.ContentFound)
                {
                    CompareDetailsVM objVM = null;
                    objVM = objDetails.GetData();
                    if (objDetails.status == Entities.StatusCodes.RedirectPermanent)
                    {
                        return RedirectPermanent(objDetails.redirectionUrl);
                    }
                    if (objVM != null && objVM.Compare != null)
                    {
                        return View(objVM);
                    }
                    else
                    {
                        return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                    }
                }
                else if (objDetails.status == Entities.StatusCodes.RedirectPermanent)
                {
                    return RedirectPermanent(objDetails.redirectionUrl);
                }
                else if ((objDetails.status == Entities.StatusCodes.RedirectTemporary))
                {
                    return Redirect(CommonOpn.AppPath + "comparebikes/");

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