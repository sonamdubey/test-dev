using Bikewale.Notifications;
using BikewaleOpr.Entity.BikePricing;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Interface.BikePricing;
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

        public PriceMonitoringController(IBikeMakes makesRepo, IShowroomPricesRepository pricesRepo)
        {
            _makesRepo = makesRepo;
            _pricesRepo = pricesRepo;
        }

        /// <summary>
        /// Created By : Ashutosh Sharma on 31-07-2017
        /// Discription: Action method for default view for price monitoring page.
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        public ActionResult Index(uint? makeId, uint? modelId)
        {
            PriceMonitoringModel priceMonitoringModel = new PriceMonitoringModel(_makesRepo, _pricesRepo);
            PriceMonitoringVM priceMonitoringVM = new PriceMonitoringVM();

            try
            {
                priceMonitoringVM.BikeMakes = priceMonitoringModel.GetMakes("NEW");

                if (makeId.HasValue && modelId.HasValue)
                {
                    priceMonitoringVM.MakeId = makeId ?? 0;
                    priceMonitoringVM.ModelId = modelId ?? 0;

                    priceMonitoringVM.PriceMonitoringEntity = priceMonitoringModel.GetPriceMonitoringDetails(Convert.ToUInt32(makeId), Convert.ToUInt32(modelId));
                }
            }
            catch (Exception ex)
            {

                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.Controllers.PriceMonitoringController.Index_makeid:{0}_modelid:{1}", makeId, modelId));

            }

            return View(priceMonitoringVM);
        }

        /// <summary>
        /// Created By : Ashutosh Sharma on 31-07-2017
        /// Discription : Action method to be called when form posted with make and model.
        /// </summary> 
        /// <param name="makeId"></param>
        /// <param name="modelId"></param>
        public ActionResult GetReport(uint makeId, uint modelId)
        {
            return RedirectToAction("Index", "PriceMonitoring", new {@makeId = makeId, @modelId = modelId });
        }
    }
}