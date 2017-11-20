using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System;
using System.Linq;
using System.Collections.Generic;
using Bikewale.Entities.BikeSeries;

namespace Bikewale.BAL.BikeData
{
    public class BikeSeries : IBikeSeries
    {
        private readonly IBikeSeriesCacheRepository _bikeSeriesCacheRepository = null;

        public BikeSeries(IBikeSeriesCacheRepository bikeSeriesCacheRepository)
        {
            _bikeSeriesCacheRepository = bikeSeriesCacheRepository;
        }

		public IEnumerable<NewBikeEntityBase> GetNewModels(uint seriesId, uint cityId)
		{
			IEnumerable<NewBikeEntityBase> objModels = null;
			try
			{
				objModels = _bikeSeriesCacheRepository.GetNewModels(seriesId, cityId);
			}
			catch (Exception ex)
			{
				ErrorClass objErr = new ErrorClass(ex, string.Format("BAL.BikeData.BikeSeries.GetNewModels_SeriesId = {1}", seriesId));
			}
			return objModels;
		}

		public IEnumerable<UpcomingBikeEntityBase> GetUpcomingModels(uint seriesId)
		{
			IEnumerable<UpcomingBikeEntityBase> objModels = null;
			try
			{
				objModels = _bikeSeriesCacheRepository.GetUpcomingModels(seriesId);
			}
			catch (Exception ex)
			{
				ErrorClass objErr = new ErrorClass(ex, string.Format("BAL.BikeData.BikeSeries.GetUpcomingModels_SeriesId = {1}", seriesId));
			}
			return objModels;
		}

		/// <summary>
		/// Created by : Vivek Singh Tomar on 28th Sep 2017
		/// Summary : Get models by series id
		/// </summary>
		/// <param name="modelId"></param>
		/// <param name="seriesId"></param>
		/// <returns></returns>
		public BikeSeriesModels GetModelsListBySeriesId(uint modelId, uint seriesId)
        {
            BikeSeriesModels objModels = null;
            try
            {
                if (modelId > 0 && seriesId > 0)
                {
                    objModels = _bikeSeriesCacheRepository.GetModelsListBySeriesId(seriesId);
                    if (objModels != null)
                    {
                        if (objModels.NewBikes != null)
                        {
                            objModels.NewBikes = objModels.NewBikes.Where(bike => bike.BikeModel.ModelId != modelId);
                        }

                        if (objModels.UpcomingBikes != null)
                        {
                            objModels.UpcomingBikes = objModels.UpcomingBikes.Where(bike => bike.BikeModel.ModelId != modelId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BAL.BikeData.BikeSeries.GetModelsListBySeries ModelId = {0} and SeriesId = {1}", modelId, seriesId));
            }
            return objModels;
        }

		public BikeDescriptionEntity GetSynopsis(uint seriesId)
		{
			BikeDescriptionEntity synopsis = null;
			try
			{
				synopsis = _bikeSeriesCacheRepository.GetSynopsis(seriesId);
			}
			catch (Exception ex)
			{
				ErrorClass objErr = new ErrorClass(ex, string.Format("BAL.BikeData.BikeSeries.GetSynopsis_SeriesId = {0}", seriesId));
			}
			return synopsis;
		}

		public IEnumerable<BikeSeriesEntity> GetOtherSeriesFromMake(int makeId)
		{
			IEnumerable<BikeSeriesEntity> bikeSeriesEntityList = null;
			try
			{
				bikeSeriesEntityList = _bikeSeriesCacheRepository.GetOtherSeriesFromMake(makeId);
			}
			catch (Exception ex)
			{
				ErrorClass objErr = new ErrorClass(ex, string.Format("BAL.BikeData.BikeSeries.GetOtherSeriesFromMake_makeId = {0}", makeId));
			}
			return bikeSeriesEntityList;
		}

		public IEnumerable<BikeSeriesCompareBikes> GetBikesToCompare(uint seriesId)
		{
			IEnumerable<BikeSeriesCompareBikes> bikeSeriesCompareBikes = null;
			try
			{
				bikeSeriesCompareBikes = _bikeSeriesCacheRepository.GetBikesToCompare(seriesId);
			}
			catch (Exception ex)
			{
				ErrorClass objErr = new ErrorClass(ex, string.Format("BAL.BikeData.BikeSeries.GetBikesToCompare_seriesId = {0}", seriesId));
			}
			return bikeSeriesCompareBikes;
		}
	}   // class
}   // namespace
