using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Models.Shared;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bikewale.Controllers.Mobile
{
    public class ScootersController : Controller
    {
        private readonly IBikeModels<BikeModelEntity, int> _models = null;
        public ScootersController(IBikeModels<BikeModelEntity, int> models)
        {
            _models = models;
        }

        // GET: Scooters
        [Route("m/scooters/")]
        public ActionResult Index()
        {
            PopulatePopularScooters();
            return View("~/views/m/scooters/index.cshtml");
        }
        /// <summary>
        /// Created by : Aditi Srivastava on 9 Mar 2017
        /// Summary    : get list of popular scooters
        /// </summary>
        private void PopulatePopularScooters()
        {
            uint cityId = GlobalCityArea.GetGlobalCityArea().CityId;
            uint topCount = 9;
            PopularScootersList objScooters = new PopularScootersList();
            objScooters.PopularScooters = _models.GetMostPopularScooters(topCount, cityId);

            objScooters.PQSourceId = 87;
            objScooters.PageCatId = 63;
            ViewBag.popularScooters = objScooters;
        }
        [Route("m/scooters/make/")]
        public ActionResult BikesByMake()
        {
            return View("~/views/m/scooters/bikesbymake.cshtml");
        }

    }
}