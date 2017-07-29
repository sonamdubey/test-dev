using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Models.ManagePrices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BikewaleOpr.Controllers
{
    public class PriceMonitoringController : Controller
    {
        private readonly IBikeMakes makesRepo;
        private readonly IBikeModelsRepository modelsRepo;

        public PriceMonitoringController(IBikeMakes _makesRepo, IBikeModelsRepository _modelsRepo)
        {
            makesRepo = _makesRepo;
            modelsRepo = _modelsRepo;
        }
        // GET: PriceMonitoring
        public ActionResult Index()
        {
            PriceMonitoringModel priceMonitoringModel = new PriceMonitoringModel(makesRepo);
            
            return View(priceMonitoringModel.GetMakes("NOW"));
        }
    }
}