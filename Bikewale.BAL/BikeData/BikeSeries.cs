using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System;
using System.Linq;

namespace Bikewale.BAL.BikeData
{
    public class BikeSeries : IBikeSeries
    { 
        private readonly IBikeSeriesCacheRepository _bikeSeriesCacheRepository = null;

        public BikeSeries(IBikeSeriesCacheRepository bikeSeriesCacheRepository)
        {
            _bikeSeriesCacheRepository = bikeSeriesCacheRepository;
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
                if(modelId > 0 && seriesId > 0)
                {
                    objModels = _bikeSeriesCacheRepository.GetModelsListBySeriesId(seriesId);
                    if(objModels != null)
                    {
                        if(objModels.NewBikes != null)
                        {
                            objModels.NewBikes = objModels.NewBikes.Where(bike => bike.BikeModel.ModelId != modelId);
                        }

                        if(objModels.UpcomingBikes != null)
                        {
                            objModels.UpcomingBikes = objModels.UpcomingBikes.Where(bike => bike.BikeModel.ModelId != modelId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BAL.BikeData.BikeSeries.GetModelsListBySeries ModelId = {0} and SeriesId = {1}", modelId, seriesId));
            }
            return objModels;
        }
    }   // class
}   // namespace
