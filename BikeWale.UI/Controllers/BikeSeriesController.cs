using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Bikewale.Models.BikeModels;
using Bikewale.Interfaces.BikeData;
using Bikewale.Models.BikeSeries;

namespace Bikewale.Controllers
{
	/// <summary>
	/// Created by : Ashutosh Sharma on 15 Nov 2017
	/// Description : UI controller for bike series page.
	/// </summary>
    public class BikeSeriesController : Controller
    {
		private readonly IBikeSeriesCacheRepository _seriesCache;
		public BikeSeriesController(IBikeSeriesCacheRepository seriesCache)
		{
			_seriesCache = seriesCache;
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
			SeriesPage seriesPage = new SeriesPage(_seriesCache);
			obj = seriesPage.GetData();
			return View(obj);
		}

		/// <summary>
		/// Created by : Ashutosh Sharma on 15 Nov 2017
		/// Description : Action method for mobile.
		/// </summary>
		/// <returns></returns>
		[Route("m/model/series/"), Filters.DeviceDetection]
        public ActionResult Index_List_Mobile()
        {
			SeriesPageVM obj;
			SeriesPage seriesPage = new SeriesPage(_seriesCache);
			obj = seriesPage.GetData();
			return View(obj);
        }
    }
}