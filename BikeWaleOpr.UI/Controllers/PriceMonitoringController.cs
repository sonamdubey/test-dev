using Bikewale.Notifications;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.Dealers;
using BikewaleOpr.Interface.Location;
using BikewaleOpr.Models.ManagePrices;
using System;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    /// <summary>
    /// Created By : Ashutosh Sharma on 31-07-2017
    /// Description : Price Monitoring Report Controller.
    /// </summary>

    [Authorize]
    public class PriceMonitoringController : Controller
    {
        private readonly IBikeMakes _makesRepo = null;
        private readonly IShowroomPricesRepository _pricesRepo = null;
        private readonly ILocation _locationRepo = null;

        public PriceMonitoringController(IBikeMakes makesRepo, IShowroomPricesRepository pricesRepo, ILocation locationRepo)
        {
            _makesRepo = makesRepo;
            _pricesRepo = pricesRepo;
            _locationRepo = locationRepo;
        }

        /// <summary>
        /// Created By : Ashutosh Sharma on 31-Jul-2017
        /// Description: Action method for default view for price monitoring page.
        /// </summary>
        /// <returns></returns>
        [Route("content/pricemonitoring/")]
        public ActionResult Index()
        {
            PriceMonitoringVM priceMonitoringVM = null;
            try
            {
                PriceMonitoringModel priceMonitoringModel = new PriceMonitoringModel(_makesRepo, _pricesRepo, _locationRepo);
                priceMonitoringVM = priceMonitoringModel.GetData();
            }
            catch (Exception ex)
            {

                ErrorClass.LogError(ex, string.Format("PriceMonitoringController.Index"));

            }

            return View(priceMonitoringVM);
        }

        /// <summary>
        /// Created By : Ashutosh Sharma on 09-08-2017
        /// Description: Action method for price monitoring page when make and state is selected.
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>

        [Route("content/pricemonitoring/make/{makeId}/state/{stateId}/")]
        public ActionResult IndexWithoutBikeModel(uint makeId, uint stateId)
        {
            PriceMonitoringVM priceMonitoringVM = null;

            try
            {
                PriceMonitoringModel priceMonitoringModel = new PriceMonitoringModel(_makesRepo, _pricesRepo, _locationRepo);
                priceMonitoringVM = priceMonitoringModel.GetData(makeId, stateId);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("PriceMonitoringController.Index_makeId:{0}_stateId:{1}", makeId, stateId));
            }

            return View("Index", priceMonitoringVM);
        }

        /// <summary>
        /// Created By : Ashutosh Sharma on 09-08-2017
        /// Description: Action method for price monitoring page when make, model and state is selected.
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [Route("content/pricemonitoring/make/{makeId}/model/{modelId}/state/{stateId}/")]
        public ActionResult IndexWithBikeModel(uint makeId, uint modelId, uint stateId)
        {
            PriceMonitoringVM priceMonitoringVM = null;

            try
            {
                PriceMonitoringModel priceMonitoringModel = new PriceMonitoringModel(_makesRepo, _pricesRepo, _locationRepo);
                priceMonitoringVM = priceMonitoringModel.GetData(makeId, modelId, stateId);

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("PriceMonitoringController.Index_makeId:{0}_modelId:{1}_stateId:{2}", makeId, modelId, stateId));
            }

            return View("Index", priceMonitoringVM);
        }



        /// <summary>
        /// Created By : Ashutosh Sharma on 09-08-2017
        /// Description: Action method for form submit.
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="stateId"></param>
        /// <returns></returns>

        [HttpPost]
        public ActionResult Submit(uint makeId = 0, uint modelId = 0, uint stateId = 0)
        {
            if (modelId == 0)
            {
                return RedirectToAction("IndexWithoutBikeModel", "PriceMonitoring", new { @makeId = makeId, @stateId = stateId });
            }
            else
            {
                return RedirectToAction("IndexWithBikeModel", "PriceMonitoring", new { @makeId = makeId, @modelId = modelId, @stateId = stateId });
            }
        }
    }
}