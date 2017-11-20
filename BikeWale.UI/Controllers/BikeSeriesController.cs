using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Models.BikeSeries;
using System.Web.Mvc;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 15 Nov 2017
    /// Description : UI controller for bike series page.
    /// </summary>
    public class BikeSeriesController : Controller
    {
        private readonly IBikeSeriesCacheRepository _seriesCache;
        private readonly IUsedBikesCache _usedBikesCache;
        public BikeSeriesController(IBikeSeriesCacheRepository seriesCache, IUsedBikesCache usedBikesCache)
        {
            _seriesCache = seriesCache;
            _usedBikesCache = usedBikesCache;
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 15 Nov 2017
        /// Description : Action method for desktop.
        /// </summary>
        /// <returns></returns>
        [Route("model/series/"), Filters.DeviceDetection]
        public ActionResult Index()
        {
            SeriesPageVM obj;
            SeriesPage seriesPage = new SeriesPage(_seriesCache, _usedBikesCache);
            obj = seriesPage.GetData();
            return View(obj);
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 15 Nov 2017
        /// Description : Action method for mobile.
        /// </summary>
        /// <returns></returns>
        [Route("m/model/series/")]
        public ActionResult Index_List_Mobile()
        {
            SeriesPageVM obj;
            SeriesPage seriesPage = new SeriesPage(_seriesCache, _usedBikesCache);
            obj = seriesPage.GetData();
            return View(obj);
        }
    }
}