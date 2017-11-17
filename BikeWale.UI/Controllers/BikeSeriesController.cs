using Bikewale.Models.BikeSeries;
using Bikewale.Interfaces.BikeData;
using Bikewale.Models.BikeSeries;
using Bikewale.Interfaces.CMS;
using System.Web.Mvc;
using Bikewale.Interfaces.Videos;

namespace Bikewale.Controllers
{
    /// <summary>
    /// Created by : Ashutosh Sharma on 15 Nov 2017
    /// Description : UI controller for bike series page.
    /// </summary>
    public class BikeSeriesController : Controller
    {
		private readonly IBikeSeries _bikeSeries = null;
		private readonly ICMSCacheContent _articles = null;
		private readonly IVideos _videos = null;
		public BikeSeriesController(IBikeSeries bikeSeries, ICMSCacheContent articles, IVideos videos)
        {
			_bikeSeries = bikeSeries;
			_articles = articles;
			_videos = videos;
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
			SeriesPage seriesPage = new SeriesPage(_bikeSeries, _articles, _videos);
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
			SeriesPage seriesPage = new SeriesPage(_bikeSeries, _articles, _videos);
			seriesPage.IsMobile = true;
			obj = seriesPage.GetData(seriesId);
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