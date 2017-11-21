using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.CMS;
using Bikewale.Interfaces.Compare;
using Bikewale.Interfaces.UsedBikes;
using Bikewale.Interfaces.Videos;
using Bikewale.Models.BikeSeries;
using System.Web.Mvc;
using Bikewale.Entities.BikeData;

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
        [Route("series/{maskingName}/"), Filters.DeviceDetection]
        public ActionResult Index(uint seriesId)
        {
            SeriesPageVM obj;
            SeriesPage seriesPage = new SeriesPage(_seriesCache, _usedBikesCache, _bikeSeries, _articles, _videos, _compareScooters);
            obj = seriesPage.GetData(seriesId);
            return View(obj);
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 15 Nov 2017
        /// Description : Action method for mobile.
        /// </summary>
        /// <returns></returns>
        [Route("m/make/{makeMaskingName}/series/{maskingName}/")]
        public ActionResult Index_Mobile(string makeMaskingName, string maskingName)
        {
            ActionResult objResult = null;

            SeriesMaskingResponse objResponse = _seriesCache.ProcessMaskingName(maskingName);

            if (objResponse != null)
            {
                if (!objResponse.IsSeriesPageCreated)
                {
                    objResult = RedirectToAction("Index_Mobile", "Model", new { makeMasking = makeMaskingName, modelMasking = maskingName });
                }
                else if (objResponse.StatusCode == 301)
                {
                    string url = string.Format("/{0}-bikes/{1}/", makeMaskingName, objResponse.NewMaskingName);
                    objResult = RedirectPermanent(url);
                }
                else
                {
                    SeriesPageVM obj;

                    SeriesPage seriesPage = new SeriesPage(_seriesCache, _usedBikesCache, _bikeSeries, _articles, _videos, _compareScooters);
                    seriesPage.IsMobile = true;
                    obj = seriesPage.GetData(objResponse.Id);
                    objResult = View(obj);
                }
            }
            else {
                objResult = Redirect("/m/pagenotfound.aspx");
            }

            return objResult;
        }
    }

}