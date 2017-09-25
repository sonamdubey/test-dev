﻿using Bikewale.Comparison.Interface;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Models;
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
        private readonly ISponsoredComparison _objSponsored = null;
        public CompareBikesController(IBikeCompareCacheRepository cachedCompare, ICMSCacheContent compareTest, IBikeMaskingCacheRepository<BikeModelEntity, int> objModelMaskingCache, IBikeCompare objCompare, IBikeMakesCacheRepository<int> objMakeCache, ISponsoredComparison objSponsored)
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


            CompareVM CompareVM = null;
            CompareVM = objCompare.GetData();
            if (CompareVM != null)
            {
                return View(CompareVM);
            }
            else
            {
                return Redirect("/pageNotFound.aspx");
            }
        }
        // GET: CompareBikes
        [Route("m/compare/")]
        public ActionResult Index_Mobile()
        {
            CompareIndex objCompare = new CompareIndex(_objCompare, _compareTest);

            CompareVM CompareVM = null;
            CompareVM = objCompare.GetData();
            if (CompareVM != null)
            {
                return View(CompareVM);
            }
            else
            {
                return Redirect("/m/pageNotFound.aspx");
            }

        }

        // GET: CompareBikes Details
        [Route("compare/details/")]
        [Filters.DeviceDetection()]
        public ActionResult CompareBikeDetails()
        {
            CompareDetails objDetails = new CompareDetails(_compareTest, _objModelMaskingCache, _cachedCompare, _objCompare, _objMakeCache, _objSponsored, 4);
            if (objDetails.status == Entities.StatusCodes.ContentFound)
            {
                CompareDetailsVM objVM = null;
                objVM = objDetails.GetData();
                if (objDetails.status == Entities.StatusCodes.RedirectPermanent)
                {
                    return RedirectPermanent(objDetails.redirectionUrl);
                }
                else
                {
                    if (objVM != null && objVM.Compare != null)
                    {
                        return View(objVM);
                    }
                    else
                    {
                        return Redirect("/pageNotFound.aspx");
                    }
                }
            }
            else if (objDetails.status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(objDetails.redirectionUrl);
            }
            else if ((objDetails.status == Entities.StatusCodes.RedirectTemporary))
            {
                return Redirect("/comparebikes/");

            }
            else
            {
                return Redirect("/pageNotFound.aspx");
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Route("m/compare/details/")]
        public ActionResult CompareBikeDetails_Mobile()
        {
            CompareDetails objDetails = new CompareDetails(_compareTest, _objModelMaskingCache, _cachedCompare, _objCompare, _objMakeCache, _objSponsored, 2);

            if (objDetails.status == Entities.StatusCodes.ContentFound)
            {
                CompareDetailsVM objVM = null;
                objDetails.IsMobile = true;
                objVM = objDetails.GetData();
                if (objDetails.status == Entities.StatusCodes.RedirectPermanent)
                {
                    return RedirectPermanent(objDetails.redirectionUrl);
                }
                else
                {
                    if (objVM != null && objVM.Compare != null)
                    {
                        return View(objVM);
                    }
                    else
                    {
                        return Redirect("/m/pageNotFound.aspx");
                    }
                }

            }
            else if (objDetails.status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(objDetails.redirectionUrl);
            }
            else if ((objDetails.status == Entities.StatusCodes.RedirectTemporary))
            {
                return Redirect("/m/comparebikes/");

            }
            else
            {
                return Redirect("/m/pageNotFound.aspx");
            }

        }
    }
}