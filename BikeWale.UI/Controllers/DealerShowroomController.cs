using Bikewale.CoreDAL;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.Used;
using Bikewale.Models;
using Bikewale.Models.DealerShowroom;
using Bikewale.Notifications;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    public class DealerShowroomController : Controller
    {
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IBikeMakesCacheRepository<int> _bikeMakesCache = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IUsedBikeDetailsCacheRepository _objUsedCache = null;
        private readonly IStateCacheRepository _objStateCache = null;

        public DealerShowroomController(IDealerCacheRepository objDealerCache, IBikeMakesCacheRepository<int> bikeMakesCache, IUpcoming upcoming, IUsedBikeDetailsCacheRepository objUsedCache, IStateCacheRepository objStateCache)
        {
            _objDealerCache = objDealerCache;
            _bikeMakesCache = bikeMakesCache;
            _upcoming = upcoming;
            _objUsedCache = objUsedCache;
            _objStateCache = objStateCache;

        }

        [Route("dealerdetails/make/{makeMaskingName}/dealerid/{dealerId}")]
        public ActionResult DealerDetails(string makeMaskingName, uint dealerId)
        {
            try
            {
                MakeMaskingResponse objResponse = _bikeMakesCache.GetMakeMaskingResponse(makeMaskingName);

                if (objResponse != null && objResponse.StatusCode == 200 && dealerId > 0)
                {
                    DealerShowroomDealerDetailsVM objDealerDetailsVM = null;
                    DealerShowroomDealerDetail objDealerDetails = new DealerShowroomDealerDetail(_objDealerCache);
                    if (objDealerDetails != null)
                    {
                        objDealerDetails.makeId = objResponse.MakeId;
                        objDealerDetailsVM = objDealerDetails.GetData();
                    }

                    return View("~/Views/DealerShowroom/DealerDetail.cshtml", objDealerDetailsVM);
                }
                else if (objResponse.StatusCode == 301)
                {
                    return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objResponse.MaskingName));
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomController.DealerDetails");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }

        }

        [Route("m/dealerdetails/make/{makeMaskingName}/dealerid/{dealerId}")]
        public ActionResult DealerDetails_Mobile(string makeMaskingName, uint dealerId)
        {
            try
            {
                MakeMaskingResponse objResponse = _bikeMakesCache.GetMakeMaskingResponse(makeMaskingName);

                if (objResponse != null && objResponse.StatusCode == 200 && dealerId > 0)
                {
                    DealerShowroomDealerDetailsVM objDealerDetailsVM = null;
                    DealerShowroomDealerDetail objDealerDetails = new DealerShowroomDealerDetail(_objDealerCache);
                    if (objDealerDetails != null)
                    {
                        objDealerDetails.makeId = objResponse.MakeId;
                        objDealerDetailsVM = objDealerDetails.GetData();
                    }

                    return View("~/Views/DealerShowroom/DealerDetail_Mobile.cshtml", objDealerDetailsVM);
                }
                else if (objResponse.StatusCode == 301)
                {
                    return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objResponse.MaskingName));
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomController.DealerDetails");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }

        }

        [Route("dealersinindia/make/{makeMaskingName}")]
        public ActionResult DealersInIndia(string makeMaskingName)
        {
            try
            {
                DealerShowroomIndiaPage objDealer = new DealerShowroomIndiaPage(_objDealerCache, _upcoming, _objUsedCache, _objStateCache, _bikeMakesCache, makeMaskingName);
                if (objDealer != null)
                {

                    if (objDealer.status == Entities.StatusCodes.ContentFound)
                    {
                        DealerShowroomIndiaPageVM objDealerVM = null;
                        objDealer.topCount = 9;
                        objDealerVM = objDealer.GetData();
                        return View(objDealerVM);
                    }
                    else if (objDealer.status == Entities.StatusCodes.RedirectPermanent)
                    {
                        return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objDealer.objResponse.MaskingName));
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
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomController.DealersInIndia");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

        [Route("m/dealersinindia/make/{makeMaskingName}")]
        public ActionResult DealersInIndia_Mobile(string makeMaskingName)
        {
            try
            {


                DealerShowroomIndiaPage objDealer = new DealerShowroomIndiaPage(_objDealerCache, _upcoming, _objUsedCache, _objStateCache, _bikeMakesCache, makeMaskingName);
                if (objDealer != null)
                {

                    if (objDealer.status == Entities.StatusCodes.ContentFound)
                    {
                        DealerShowroomIndiaPageVM objDealerVM = null;
                        objDealer.topCount = 9;
                        objDealerVM = objDealer.GetData();
                        return View(objDealerVM);
                    }
                    else if (objDealer.status == Entities.StatusCodes.RedirectPermanent)
                    {
                        return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objDealer.objResponse.MaskingName));
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
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomController.DealersInIndia_Mobile");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

        [Route("dealerincity/make/{makeMaskingName}/city/{cityMaskingName}")]
        public ActionResult DealerInCity(string makeMaskingName, string cityMaskingName)
        {
            try
            {
                DealerShowroomCityPage objDealer = new DealerShowroomCityPage(_objDealerCache, _objUsedCache, _bikeMakesCache, makeMaskingName, cityMaskingName);
                if (objDealer != null)
                {
                    if (objDealer.status == Entities.StatusCodes.ContentFound)
                    {
                        DealerShowroomCityPageVM objDealerVM = null;

                        if (objDealer != null)
                        {
                            objDealer.topCount = 9;
                            objDealerVM = objDealer.GetData();
                        }

                        return View(objDealerVM);
                    }
                    else if (objDealer.status == Entities.StatusCodes.RedirectPermanent)
                    {
                        return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objDealer.objResponse.MaskingName));
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
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomController.DealersInCity");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

        [Route("m/dealerincity/make/{makeMaskingName}/city/{cityMaskingName}")]
        public ActionResult DealerInCity_Mobile(string makeMaskingName, string cityMaskingName)
        {
            try
            {
                DealerShowroomCityPage objDealer = new DealerShowroomCityPage(_objDealerCache, _objUsedCache, _bikeMakesCache, makeMaskingName, cityMaskingName);
                if (objDealer != null)
                {
                    if (objDealer.status == Entities.StatusCodes.ContentFound)
                    {
                        DealerShowroomCityPageVM objDealerVM = null;

                        if (objDealer != null)
                        {
                            objDealer.topCount = 9;
                            objDealerVM = objDealer.GetData();
                        }

                        return View(objDealerVM);
                    }
                    else if (objDealer.status == Entities.StatusCodes.RedirectPermanent)
                    {
                        return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objDealer.objResponse.MaskingName));
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
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomController.DealersInCity");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }
        }

    }
}