using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.BikeData.NewLaunched;
using Bikewale.Interfaces.BikeData.UpComing;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Location;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Interfaces.Used;
using Bikewale.Models;
using Bikewale.Models.DealerShowroom;
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
        private readonly IBikeMakesCacheRepository _bikeMakesCache = null;
        private readonly IUpcoming _upcoming = null;
        private readonly IUsedBikeDetailsCacheRepository _objUsedCache = null;
        private readonly IStateCacheRepository _objStateCache = null;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels = null;
        private readonly IServiceCenter _objSC = null;
        private readonly INewBikeLaunchesBL _newLaunches = null;
        private readonly IBikeModelsCacheRepository<int> _objBestBikes = null;
        private readonly IDealer _objDealer;
        private readonly IApiGatewayCaller _apiGatewayCaller;
		private readonly IPriceQuote _objPQ = null;
        private readonly ICityMaskingCacheRepository _cityMaskingCache = null;

        //Constructor for dealer locator
        public DealerShowroomController(IBikeModelsCacheRepository<int> objBestBikes, IBikeMakes<BikeMakeEntity, int> bikeMakes, INewBikeLaunchesBL newLaunches, IServiceCenter objSC, IDealerCacheRepository objDealerCache, IBikeMakesCacheRepository bikeMakesCache,  ICityMaskingCacheRepository cityMaskingCache, IUpcoming upcoming, IBikeModels<BikeModelEntity, int> bikeModels, IUsedBikeDetailsCacheRepository objUsedCache, IStateCacheRepository objStateCache, IDealer objDealer, IApiGatewayCaller apiGatewayCaller, IPriceQuote objPQ)
        {
            _objDealerCache = objDealerCache;
            _bikeMakesCache = bikeMakesCache;
            _cityMaskingCache = cityMaskingCache;
            _upcoming = upcoming;
            _objUsedCache = objUsedCache;
            _objStateCache = objStateCache;
            _bikeModels = bikeModels;
            _newLaunches = newLaunches;
            _objSC = objSC;
            _objBestBikes = objBestBikes;
            _objDealer = objDealer;
            _apiGatewayCaller = apiGatewayCaller;
			_objPQ = objPQ;
        }


        /// <summary>
        /// Created By:- Subodh Jain 29 March 2017
        /// Summary :- Action method For Landing Page Desktop
        /// </summary>
        /// <returns></returns>
        [Filters.DeviceDetection()]
        [Route("dealershowroom/Index/")]

        public ActionResult Index()
        {
            DealerShowroomIndexPage objDealerIndex = new DealerShowroomIndexPage(_objBestBikes, _objDealerCache, _bikeMakesCache, _upcoming, _newLaunches, 10);

            IndexVM objDealerIndexVM = objDealerIndex.GetData();
            if (objDealerIndexVM != null)
                return View(objDealerIndexVM);
            else
                return HttpNotFound();


        }
        /// <summary>
        /// Created By:- Subodh Jain 29 March 2017
        /// Summary :- Action method For Landing Page Mobile
        /// </summary>
        /// <returns></returns>
        [Route("m/dealershowroom/Index/")]
        public ActionResult Index_Mobile()
        {
            DealerShowroomIndexPage objDealerIndex = new DealerShowroomIndexPage(_objBestBikes, _objDealerCache, _bikeMakesCache, _upcoming, _newLaunches, 6);

            objDealerIndex.IsMobile = true;
            IndexVM objDealerIndexVM = objDealerIndex.GetData();
            if (objDealerIndexVM != null)
                return View(objDealerIndexVM);
            else
                return HttpNotFound();


        }
        /// <summary>
        /// Created By:- Subodh Jain 29 March 2017
        /// Summary :- Action method For Dealers In India Desktop
        /// </summary>
        /// <returns></returns>
        [Filters.DeviceDetection()]
        [Route("dealersinindia/make/{makeMaskingName}")]
        public ActionResult DealersInIndia(string makeMaskingName)
        {

            DealerShowroomIndiaPage objDealer = new DealerShowroomIndiaPage(_newLaunches, _objDealerCache, _upcoming, _objUsedCache, _objStateCache, _bikeMakesCache, makeMaskingName);


            if (objDealer.status == Entities.StatusCodes.ContentFound)
            {
                DealerShowroomIndiaPageVM objDealerVM = null;
                objDealer.TopCount = 9;
                objDealerVM = objDealer.GetData();
                return View(objDealerVM);
            }
            else if (objDealer.status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objDealer.objResponse.MaskingName));
            }
            else if (objDealer.status == Entities.StatusCodes.RedirectTemporary)
            {
                return Redirect(objDealer.RedirectUrl);
            }
            else
            {
                return HttpNotFound();
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


            DealerShowroomIndiaPage objDealer = new DealerShowroomIndiaPage(_newLaunches, _objDealerCache, _upcoming, _objUsedCache, _objStateCache, _bikeMakesCache, makeMaskingName);

            if (objDealer.status == Entities.StatusCodes.ContentFound)
            {
                DealerShowroomIndiaPageVM objDealerVM = null;
                objDealer.TopCount = 9;
                objDealer.IsMobile = true;
                objDealerVM = objDealer.GetData();
                return View(objDealerVM);
            }
            else if (objDealer.status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(Request.RawUrl.Replace(makeMaskingName, objDealer.objResponse.MaskingName));
            }
            else if (objDealer.status == Entities.StatusCodes.RedirectTemporary)
            {
                objDealer.RedirectUrl = string.Format("/m{0}", objDealer.RedirectUrl);
                return Redirect(objDealer.RedirectUrl);
            }
            else
            {
                return HttpNotFound();
            }

        }

        /// <summary>
        /// Created By:- Subodh Jain 29 March 2017
        /// Summary :- Action method For Dealers In City Desktop
        /// </summary>
        /// <returns></returns>
        [Filters.DeviceDetection()]
        [Route("dealerincity/make/{makeMaskingName}/city/{cityMaskingName}")]
        public ActionResult DealerInCity(string makeMaskingName, string cityMaskingName)
        {

			DealerShowroomCityPage objDealer = new DealerShowroomCityPage(_bikeModels, _objSC, _objDealerCache, _objUsedCache, _bikeMakesCache, _cityMaskingCache, makeMaskingName, cityMaskingName, 3, _objPQ);
            objDealer.PQSource = PQSourceEnum.Desktop_DealerLocator_Listing;

            if (objDealer.status.Equals(Entities.StatusCodes.ContentFound))
            {
                DealerShowroomCityPageVM objDealerVM = null;
                objDealerVM = objDealer.GetData();
                return View(objDealerVM);
            }
            else if (objDealer.status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(objDealer.RedirectUrl);
            }
            else
            {
                return HttpNotFound();
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

			DealerShowroomCityPage objDealer = new DealerShowroomCityPage(_bikeModels, _objSC, _objDealerCache, _objUsedCache, _bikeMakesCache, _cityMaskingCache, makeMaskingName, cityMaskingName, 9, _objPQ);
            objDealer.PQSource = PQSourceEnum.Mobile_DealerLocator_Listing;

            if (objDealer.status == Entities.StatusCodes.ContentFound)
            {
                DealerShowroomCityPageVM objDealerVM = null;
                objDealer.IsMobile = true;
                objDealerVM = objDealer.GetData();
                return View(objDealerVM);
            }
            else if (objDealer.status == Entities.StatusCodes.RedirectPermanent)
            {
                return RedirectPermanent(objDealer.RedirectUrl);
            }
            else
            {
                return HttpNotFound();
            }

        }

        /// <summary>
        /// Created By:- Subodh Jain 29 March 2017
        /// Summary :- Action method For Dealer Details Page Desktop
        /// </summary>
        /// <returns></returns>
        [Filters.DeviceDetection()]
        [Route("dealerdetails/make/{makeMaskingName}/city/{cityMaskingName}/dealerid/{dealerId}")]
        public ActionResult DealerDetail(string makeMaskingName, string cityMaskingName, uint dealerId)
        {

			DealerShowroomDealerDetail objDealerDetails = new DealerShowroomDealerDetail(_objSC, _objDealerCache, _bikeMakesCache, _bikeModels, makeMaskingName, cityMaskingName, dealerId, 3, false, _apiGatewayCaller, _objDealer, _objPQ);

            if (dealerId > 0)
            {
                DealerShowroomDealerDetailsVM objDealerDetailsVM = null;

                if (objDealerDetails.status == Entities.StatusCodes.ContentFound)
                {
                    objDealerDetails.Source = DTO.PriceQuote.PQSources.Desktop;
                    objDealerDetails.PQSource = PQSourceEnum.Desktop_DealerLocator_Detail;
                    objDealerDetailsVM = objDealerDetails.GetData();
                    if (objDealerDetailsVM != null && objDealerDetailsVM.DealerDetails != null && objDealerDetailsVM.DealerDetails.DealerDetails != null)
                        return View(objDealerDetailsVM);
                    else
                        return HttpNotFound();
                }

                else if (objDealerDetails.status == Entities.StatusCodes.RedirectPermanent)
                {
                    return RedirectPermanent(objDealerDetails.objDealerDetails.RedirectUrl);
                }
                else
                {
                    return HttpNotFound();
                }
            }
            else
            {
                return HttpNotFound();
            }

        }
        /// <summary>
        /// Created By:- Subodh Jain 29 March 2017
        /// Summary :- Action method For Dealer Details Page Mobile
        /// </summary>
        /// <returns></returns>
        [Route("m/dealerdetails/make/{makeMaskingName}/city/{cityMaskingName}/dealerid/{dealerId}")]
        public ActionResult DealerDetail_Mobile(string makeMaskingName, string cityMaskingName, uint dealerId)
        {


			DealerShowroomDealerDetail objDealerDetails = new DealerShowroomDealerDetail(_objSC, _objDealerCache, _bikeMakesCache, _bikeModels, makeMaskingName, cityMaskingName, dealerId, 9, true, _apiGatewayCaller, _objDealer, _objPQ);
            if (dealerId > 0)
            {
                DealerShowroomDealerDetailsVM objDealerDetailsVM = null;

                if (objDealerDetails.status == Entities.StatusCodes.ContentFound)
                {
                    objDealerDetails.Source = DTO.PriceQuote.PQSources.Mobile;
                    objDealerDetails.PQSource = PQSourceEnum.Mobile_DealerLocator_Detail;
                    objDealerDetailsVM = objDealerDetails.GetData();
                    if (objDealerDetailsVM != null && objDealerDetailsVM.DealerDetails != null && objDealerDetailsVM.DealerDetails.DealerDetails != null)
                        return View(objDealerDetailsVM);
                    else
                        return HttpNotFound();
                }

                else if (objDealerDetails.status == Entities.StatusCodes.RedirectPermanent)
                {
                    return RedirectPermanent(objDealerDetails.objDealerDetails.RedirectUrl);
                }
                else
                {
                    return HttpNotFound();
                }

            }
            else
            {
                return HttpNotFound();
            }


        }


    }
}