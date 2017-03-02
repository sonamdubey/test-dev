using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created by : Aditi Srivastava on 28 Feb 2017
    /// Summary    : Controller for popular bikes/body style bikes
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
            ViewBag.MakeName = objPopularBikes.FirstOrDefault().objMake.MakeName;
            return View("~/Views/Shared/_PopularBikes.cshtml", objPopularBikes);
        }

        [Route("m/popularbikes/")]
        public ActionResult PopularBikesMobile(uint topCount, uint makeId, uint cityId)
        {
            if (cityId > 0)
                objPopularBikes = _modelCache.GetMostPopularBikesbyMakeCity(topCount, makeId, cityId);
            else
                objPopularBikes = _modelCache.GetMostPopularBikes((int)topCount, (int)makeId);
            ViewBag.MakeName = objPopularBikes.FirstOrDefault().objMake.MakeName;
            return View("~/Views/m/Shared/_PopularBikes.cshtml", objPopularBikes);
        }

        [Route("popularbodystyle/")]
        public ActionResult PopularBodyStyleDesktop(int modelId,int topCount, uint cityId)
        {
            IEnumerable<MostPopularBikesBase> objPopularBodyStyle = _modelCache.GetPopularBikesByBodyStyle(modelId, topCount, cityId);
            if (objPopularBodyStyle != null && objPopularBodyStyle.FirstOrDefault()!=null)
            {
                ViewBag.BodyStyle = objPopularBodyStyle.FirstOrDefault().BodyStyle;  
            }
            return View("~/Views/Shared/_PopularBodyStyle.cshtml", objPopularBodyStyle);
        }

        [Route("m/popularbodystyle/")]
        public ActionResult PopularBodyStyleMobile(int modelId, int topCount, uint cityId)
        {
            IEnumerable<MostPopularBikesBase> objPopularBodyStyle = _modelCache.GetPopularBikesByBodyStyle(modelId, topCount, cityId);
            if (objPopularBodyStyle != null && objPopularBodyStyle.FirstOrDefault() != null)
            {
                ViewBag.BodyStyle = objPopularBodyStyle.FirstOrDefault().BodyStyle;
            }
            return View("~/Views/m/Shared/_PopularBodyStyle.cshtml", objPopularBodyStyle);
        }
    }
}