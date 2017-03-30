using Bikewale.CoreDAL;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Used;
using Bikewale.Models;
using Bikewale.Models.DealerShowroom;
using Bikewale.Notifications;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created By :- Subodh Jain 29 March 2017
    /// Sumary :- Controller for Dealer Showroom
    /// </summary>
    public class DealerShowroomController : Controller
    {
        private readonly IDealerCacheRepository _objDealerCache = null;
        private readonly IBikeMakesCacheRepository<int> _bikeMakesCache = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IUsedBikeDetailsCacheRepository _objUsedCache = null;
        private readonly IStateCacheRepository _objStateCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IServiceCenter _objSC = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IBikeMakes<BikeMakeEntity, int> _bikeMakes = null;

        //Constructor for dealer locator
        public DealerShowroomController(IBikeMakes<BikeMakeEntity, int> bikeMakes, INewBikeLaunchesBL newLaunches, IServiceCenter objSC, IDealerCacheRepository objDealerCache, IBikeMakesCacheRepository<int> bikeMakesCache, IUpcoming upcoming, IBikeModels<BikeModelEntity, int> bikeModels, IUsedBikeDetailsCacheRepository objUsedCache, IStateCacheRepository objStateCache)
        {
            _objDealerCache = objDealerCache;
            _bikeMakesCache = bikeMakesCache;
            _upcoming = upcoming;
            _objUsedCache = objUsedCache;
            _objStateCache = objStateCache;
            _bikeModels = bikeModels;
            _newLaunches = newLaunches;
            _objSC = objSC;
            _bikeMakes = bikeMakes;

        }

        /// <summary>
        /// Created By:- Subodh Jain 29 March 2017
        /// Summary :- Action method For Landing Page Desktop
        /// </summary>
        /// <returns></returns>
        [Route("dealershowroom/Index/")]
        public ActionResult Index()
        {
            DealerShowroomIndexPage objDealerIndex = new DealerShowroomIndexPage(_bikeMakes, _objDealerCache, _bikeMakesCache, _upcoming, _newLaunches, 10);
            try
            {
                if (objDealerIndex != null)
                {
                    IndexVM objDealerIndexVM = new IndexVM();
                    objDealerIndexVM = objDealerIndex.GetData();
                    return View(objDealerIndexVM);
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomController.Index");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }

        }
        /// <summary>
        /// Created By:- Subodh Jain 29 March 2017
        /// Summary :- Action method For Landing Page Mobile
        /// </summary>
        /// <returns></returns>
        [Route("m/dealershowroom/Index/")]
        public ActionResult Index_Mobile()
        {
            DealerShowroomIndexPage objDealerIndex = new DealerShowroomIndexPage(_bikeMakes, _objDealerCache, _bikeMakesCache, _upcoming, _newLaunches, 6);
            try
            {
                if (objDealerIndex != null)
                {
                    IndexVM objDealerIndexVM = new IndexVM();
                    objDealerIndexVM = objDealerIndex.GetData();
                    return View(objDealerIndexVM);
                }
                else
                {
                    return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
                }
            }
            catch (System.Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomController.Index");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }

        }
        /// <summary>
        /// Created By:- Subodh Jain 29 March 2017
        /// Summary :- Action method For Dealers In India Desktop
        /// </summary>
        /// <returns></returns>
        [Route("dealersinindia/make/{makeMaskingName}")]
        public ActionResult DealersInIndia(string makeMaskingName)
        {
            try
            {
                DealerShowroomIndiaPage objDealer = new DealerShowroomIndiaPage(_newLaunches, _objDealerCache, _upcoming, _objUsedCache, _objStateCache, _bikeMakesCache, makeMaskingName);

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
                    else if (objDealer.status == Entities.StatusCodes.RedirectTemporary)
                    {
                        return Redirect(objDealer.redirectUrl);
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

        /// <summary>
        /// Created By:- Subodh Jain 29 March 2017
        /// Summary :- Action method For Dealers In India Mobile
        /// </summary>
        /// <returns></returns>
        [Route("m/dealersinindia/make/{makeMaskingName}")]
        public ActionResult DealersInIndia_Mobile(string makeMaskingName)
        {
            try
            {


                DealerShowroomIndiaPage objDealer = new DealerShowroomIndiaPage(_newLaunches, _objDealerCache, _upcoming, _objUsedCache, _objStateCache, _bikeMakesCache, makeMaskingName);
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
                    else if (objDealer.status == Entities.StatusCodes.RedirectTemporary)
                    {
                        return Redirect(objDealer.redirectUrl);
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

        /// <summary>
        /// Created By:- Subodh Jain 29 March 2017
        /// Summary :- Action method For Dealers In City Desktop
        /// </summary>
        /// <returns></returns>
        [Route("dealerincity/make/{makeMaskingName}/city/{cityMaskingName}")]
        public ActionResult DealerInCity(string makeMaskingName, string cityMaskingName)
        {
            try
            {
                DealerShowroomCityPage objDealer = new DealerShowroomCityPage(_bikeModels, _objSC, _objDealerCache, _objUsedCache, _bikeMakesCache, makeMaskingName, cityMaskingName, 3);
                if (objDealer != null)
                {
                    if (objDealer.status == Entities.StatusCodes.ContentFound)
                    {
                        DealerShowroomCityPageVM objDealerVM = null;

                        if (objDealer != null)
                        {
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

        /// <summary>
        /// Created By:- Subodh Jain 29 March 2017
        /// Summary :- Action method For Dealers In City Mobile
        /// </summary>
        /// <returns></returns>
        [Route("m/dealerincity/make/{makeMaskingName}/city/{cityMaskingName}")]
        public ActionResult DealerInCity_Mobile(string makeMaskingName, string cityMaskingName)
        {
            try
            {
                DealerShowroomCityPage objDealer = new DealerShowroomCityPage(_bikeModels, _objSC, _objDealerCache, _objUsedCache, _bikeMakesCache, makeMaskingName, cityMaskingName, 9);
                if (objDealer != null)
                {
                    if (objDealer.status == Entities.StatusCodes.ContentFound)
                    {
                        DealerShowroomCityPageVM objDealerVM = null;

                        if (objDealer != null)
                        {
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

        /// <summary>
        /// Created By:- Subodh Jain 29 March 2017
        /// Summary :- Action method For Dealer Details Page Desktop
        /// </summary>
        /// <returns></returns>
        [Route("dealerdetails/make/{makeMaskingName}/dealerid/{dealerId}")]
        public ActionResult DealerDetail(string makeMaskingName, uint dealerId)
        {
            try
            {

                DealerShowroomDealerDetail objDealerDetails = new DealerShowroomDealerDetail(_objSC, _objDealerCache, _bikeMakesCache, _bikeModels, makeMaskingName, dealerId, 3);
                if (objDealerDetails != null && dealerId > 0)
                {
                    DealerShowroomDealerDetailsVM objDealerDetailsVM = null;

                    if (objDealerDetails.status == Entities.StatusCodes.ContentFound)
                    {

                        objDealerDetailsVM = objDealerDetails.GetData();
                        return View(objDealerDetailsVM);
                    }

                    else if (objDealerDetails.status == Entities.StatusCodes.RedirectPermanent)
                    {
                        return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objDealerDetails.objResponse.MaskingName));
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

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomController.DealerDetails");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }

        }
        /// <summary>
        /// Created By:- Subodh Jain 29 March 2017
        /// Summary :- Action method For Dealer Details Page Mobile
        /// </summary>
        /// <returns></returns>
        [Route("m/dealerdetails/make/{makeMaskingName}/dealerid/{dealerId}")]
        public ActionResult DealerDetail_Mobile(string makeMaskingName, uint dealerId)
        {
            try
            {

                DealerShowroomDealerDetail objDealerDetails = new DealerShowroomDealerDetail(_objSC, _objDealerCache, _bikeMakesCache, _bikeModels, makeMaskingName, dealerId, 9);
                if (objDealerDetails != null && dealerId > 0)
                {
                    DealerShowroomDealerDetailsVM objDealerDetailsVM = null;

                    if (objDealerDetails.status == Entities.StatusCodes.ContentFound)
                    {

                        objDealerDetailsVM = objDealerDetails.GetData();
                        return View(objDealerDetailsVM);
                    }

                    else if (objDealerDetails.status == Entities.StatusCodes.RedirectPermanent)
                    {
                        return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objDealerDetails.objResponse.MaskingName));
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

                ErrorClass objErr = new ErrorClass(ex, "DealerShowroomController.DealerDetail_Mobile");
                return Redirect(CommonOpn.AppPath + "pageNotFound.aspx");
            }

        }


    }
}