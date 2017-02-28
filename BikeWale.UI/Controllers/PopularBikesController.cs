using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created by : Aditi Srivastava on 28 Feb 2017
    /// Summary    : Controller for popular bikes
    /// </summary>
    public class PopularBikesController : Controller
    {
        private readonly IBikeModelsCacheRepository<int> _modelCache = null;
        IEnumerable<MostPopularBikesBase> objPopularBikes = null;

        public PopularBikesController(IBikeModelsCacheRepository<int> modelCache)
        {
            _modelCache = modelCache;
        }
        [Route("popularbikes/")]
        public ActionResult PopularBikesDesktop(uint topCount, uint makeId, uint cityId)
        {
            if (cityId > 0)
                objPopularBikes = _modelCache.GetMostPopularBikesbyMakeCity(topCount, makeId, cityId);
            else
                objPopularBikes = _modelCache.GetMostPopularBikes((int)topCount, (int)makeId);
            return View("~/Views/Shared/_PopularBikes.cshtml", objPopularBikes);
        }

        [Route("m/popularbikes/")]
        public ActionResult PopularBikesMobile(uint topCount, uint makeId, uint cityId)
        {
            if (cityId > 0)
                objPopularBikes = _modelCache.GetMostPopularBikesbyMakeCity(topCount, makeId, cityId);
            else
                objPopularBikes = _modelCache.GetMostPopularBikes((int)topCount, (int)makeId);
            return View("~/Views/m/Shared/_PopularBikes.cshtml", objPopularBikes);
        }
    }
}