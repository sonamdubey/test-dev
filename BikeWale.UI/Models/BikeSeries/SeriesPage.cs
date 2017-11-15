using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.Models.BikeSeries
{
	public class SeriesPage
	{
		private readonly IBikeSeriesCacheRepository _seriesCache;
		public SeriesPage(IBikeSeriesCacheRepository seriesCache)
		{
			_seriesCache = seriesCache;
		}

		public SeriesPageVM GetData()
		{
			SeriesPageVM objSeriesPage = null;
			try
			{
				objSeriesPage = new SeriesPageVM();
				objSeriesPage.SeriesModels =  _seriesCache.GetModelsListBySeriesId(2);
			}
			catch (Exception ex)
			{
				ErrorClass objErr = new ErrorClass(ex, "Bikewale.Models.BikeSeries.SeriesPage.GetData");
			}
			return objSeriesPage;
		}
	}
}