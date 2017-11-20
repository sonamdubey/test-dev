using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Interfaces.Videos;
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
        private readonly IBikeSeries _bikeSeries = null;
        private readonly IUsedBikesCache _usedBikesCache;
        private readonly ICMSCacheContent _articles = null;
        private readonly IVideos _videos = null;
        private readonly IBikeSeriesCacheRepository _seriesCache = null;
        public BikeSeriesController(IBikeSeriesCacheRepository seriesCache, IUsedBikesCache usedBikesCache, IBikeSeries bikeSeries, ICMSCacheContent articles, IVideos videos)
        {
            _bikeSeries = bikeSeries;
            _usedBikesCache = usedBikesCache;
            _articles = articles;
            _videos = videos;
            _seriesCache = seriesCache;
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 15 Nov 2017
        /// Description : Action method for desktop.
        /// </summary>
        /// <returns></returns>
        [Route("model/series/{seriesId}/"), Filters.DeviceDetection]
        public ActionResult Index(uint seriesId)
        {
            SeriesPageVM obj;
            SeriesPage seriesPage = new SeriesPage(_seriesCache, _usedBikesCache, _bikeSeries, _articles, _videos);
            obj = seriesPage.GetData(seriesId);
            return View(obj);
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 15 Nov 2017
        /// Description : Action method for mobile.
        /// </summary>
        /// <returns></returns>
        [Route("m/model/series/{seriesId}/")]
        public ActionResult Index_Mobile(uint seriesId)
        {
            SeriesPageVM obj;

            SeriesPage seriesPage = new SeriesPage(_seriesCache, _usedBikesCache, _bikeSeries, _articles, _videos);
            seriesPage.IsMobile = true;
            obj = seriesPage.GetData(seriesId);
            return View(obj);
        }
    }

}