using Bikewale.Entities.BikeData;
using Bikewale.Entities.BikeSeries;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
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
        /// Created by : Ashutosh Sharma on 17 Nov 2017
        /// Description : BAL method to get new models of a series with city price.
        /// </summary>
        /// <param name="seriesId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<NewBikeEntityBase> GetNewModels(uint seriesId, uint cityId)
        {
            IEnumerable<NewBikeEntityBase> objModels = null;
            try
            {
                objModels = _bikeSeriesCacheRepository.GetNewModels(seriesId, cityId);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BAL.BikeData.BikeSeries.GetNewModels_SeriesId_{0}_CityId_{1}", seriesId, cityId));
            }
            return objModels;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 17 Nov 2017
        /// Description : BAL method to get upcoming models of a series.
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public IEnumerable<UpcomingBikeEntityBase> GetUpcomingModels(uint seriesId)
        {
            IEnumerable<UpcomingBikeEntityBase> objModels = null;
            try
            {
                objModels = _bikeSeriesCacheRepository.GetUpcomingModels(seriesId);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BAL.BikeData.BikeSeries.GetUpcomingModels_SeriesId_{0}", seriesId));
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
                ErrorClass.LogError(ex, string.Format("BAL.BikeData.BikeSeries.GetModelsListBySeries ModelId = {0} and SeriesId = {1}", modelId, seriesId));
            }
            return objModels;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 17 Nov 2017
        /// Description : BAL method to get synopsis of a series.
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public BikeDescriptionEntity GetSynopsis(uint seriesId)
        {
            BikeDescriptionEntity synopsis = null;
            try
            {
                synopsis = _bikeSeriesCacheRepository.GetSynopsis(seriesId);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BAL.BikeData.BikeSeries.GetSynopsis_SeriesId_{0}", seriesId));
            }
            return synopsis;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 17 Nov 2017
        /// Description : BAL method to get all series of a make.
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public IEnumerable<BikeSeriesEntity> GetOtherSeriesFromMake(int makeId, uint seriesId)
        {
            IEnumerable<BikeSeriesEntity> bikeSeriesEntityList = null;
            try
            {
                bikeSeriesEntityList = _bikeSeriesCacheRepository.GetOtherSeriesFromMake(makeId);
                if (bikeSeriesEntityList != null && bikeSeriesEntityList.Any())
                {
                    bikeSeriesEntityList = bikeSeriesEntityList.Where(m => m.SeriesId != seriesId);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BAL.BikeData.BikeSeries.GetOtherSeriesFromMake_makeId_{0}_SeriesId_{1}", makeId, seriesId));
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
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BAL.BikeData.BikeSeries.GetBikesToCompare_seriesId = {0}", seriesId));
            }
            return bikeSeriesCompareBikes;
        }

        /// <summary>
        /// created by : vivek singh tomar on 24th nov 2017
        /// summary : get modelids as comma separated string by series id
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public string GetModelIdsBySeries(uint seriesId)
        {
            string modelIds = string.Empty;
            try
            {
                if (seriesId > 0)
                {
                    modelIds = _bikeSeriesCacheRepository.GetModelIdsBySeries(seriesId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.BAL.BikeData.BikeSeries.GetMaskingNames seriesId {0}", seriesId));
            }
            return modelIds;
        }


        /// <summary>
        /// Created By : Deepak Israni on 16 April 2018
        /// Description: To get the list of series by make with minimum price by city.
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        public IEnumerable<BikeSeriesEntity> GetMakeSeries(int makeId, int cityId)
        {
            IEnumerable<BikeSeriesEntity> makeSeriesEntityList = null;
            try
            {
                makeSeriesEntityList = _bikeSeriesCacheRepository.GetMakeSeries(makeId, cityId);
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, string.Format("BAL.BikeData.BikeSeries.GetMakeSeries_makeId_{0}_cityId_{1}", makeId, cityId));
            }
            return makeSeriesEntityList;
        }
    }   // class
}   // namespace
