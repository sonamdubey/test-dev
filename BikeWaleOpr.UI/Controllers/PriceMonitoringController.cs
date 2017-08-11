using Bikewale.Notifications;
using BikewaleOpr.Entity.BikePricing;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.BikePricing;
using BikewaleOpr.Interface.Location;
using BikewaleOpr.Models.ManagePrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    /// <summary>
    /// Created By : Ashutosh Sharma on 31-07-2017
    /// Discription : Price Monitoring Report Controller.
    /// </summary>
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

        [Route("pricemonitoring/")]
        public ActionResult Index()
        {
            PriceMonitoringVM priceMonitoringVM = null;
            try
            {
                PriceMonitoringModel priceMonitoringModel = new PriceMonitoringModel(_makesRepo, _pricesRepo, _locationRepo);
                priceMonitoringVM = priceMonitoringModel.getData();
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, string.Format("PriceMonitoringController.Index"));

            }

            return View(priceMonitoringVM);
        }

        /// <summary>
        /// Created By : Ashutosh Sharma on 31-07-2017
        /// Discription: Action method for default view for price monitoring page.
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>

        [Route("pricemonitoring/make/{makeId}/state/{stateId}")]
        public ActionResult IndexWithoutBikeModel(uint makeId, uint stateId)
        {
            PriceMonitoringVM priceMonitoringVM = null;

            try
            {
                PriceMonitoringModel priceMonitoringModel = new PriceMonitoringModel(_makesRepo, _pricesRepo, _locationRepo);
                priceMonitoringVM = priceMonitoringModel.getData(makeId, stateId);

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("PriceMonitoringController.Index_makeId:{0}_stateId:{1}", makeId, stateId));
            }

            return View("Index", priceMonitoringVM);
        }


        [Route("pricemonitoring/make/{makeId}/model/{modelId}/state/{stateId}")]
        public ActionResult IndexWithBikeModel(uint makeId, uint modelId, uint stateId)
        {
            PriceMonitoringVM priceMonitoringVM = null;

            try
            {
                PriceMonitoringModel priceMonitoringModel = new PriceMonitoringModel(_makesRepo, _pricesRepo, _locationRepo);
                priceMonitoringVM = priceMonitoringModel.getData(makeId, modelId, stateId);

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("PriceMonitoringController.Index_makeId:{0}_modelId:{1}_stateId:{2}", makeId, modelId, stateId));
            }

            return View("Index", priceMonitoringVM);
        }


        
        /// <summary>
        /// 
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