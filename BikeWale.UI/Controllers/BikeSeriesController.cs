using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bikewale.Models.BikeModels;

namespace Bikewale.Controllers
{
    public class BikeSeriesController : Controller
    {
        // GET: BikeSeries
        [Route("m/model/series/"), Filters.DeviceDetection]
        public ActionResult Index_List_Mobile()
        {
            ModelPageVM obj = new ModelPageVM();
            return View(obj);
        }

        // GET: BikeSeries
        [Route("model/series/"), Filters.DeviceDetection]
        public ActionResult Index_List()
        {
            ModelPageVM obj = new ModelPageVM();
            return View(obj);
        }
    }

}