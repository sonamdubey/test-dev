using Bikewale.Entities.Compare;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
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
        private readonly IBikeCompare _compareScooters = null;
        public BikeSeriesController(IBikeSeriesCacheRepository seriesCache, IUsedBikesCache usedBikesCache, IBikeSeries bikeSeries, ICMSCacheContent articles, IVideos videos, IBikeCompare compareScooters)
        {
            _bikeSeries = bikeSeries;
            _usedBikesCache = usedBikesCache;
            _articles = articles;
            _videos = videos;
            _seriesCache = seriesCache;
            _compareScooters = compareScooters;
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
            SeriesPage seriesPage = new SeriesPage(_seriesCache, _usedBikesCache, _bikeSeries, _articles, _videos, _compareScooters);
            seriesPage.CompareSource = CompareSources.Desktop_SeriesPage;
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

            SeriesPage seriesPage = new SeriesPage(_seriesCache, _usedBikesCache, _bikeSeries, _articles, _videos, _compareScooters);
            seriesPage.IsMobile = true;
            seriesPage.CompareSource = CompareSources.Mobile_SeriesPage;
            obj = seriesPage.GetData(seriesId);
            return View(obj);
        }
    }

}