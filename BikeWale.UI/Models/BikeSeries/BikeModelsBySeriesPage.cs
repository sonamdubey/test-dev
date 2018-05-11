using Bikewale.Entities.BikeData;
using Bikewale.Entities.Pages;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bikewale.Models.BikeSeries
{
    public class BikeModelsBySeriesPage
    {
        private readonly IBikeSeries _bikeSeries = null;
        public BikeModelsBySeriesPage(IBikeSeries bikeSeries)
        {
            _bikeSeries = bikeSeries;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 28th Sep 2017
        /// Summary : Model page for Bike Models By SeriesId
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public BikeSeriesModelsVM GetData(uint modelId, uint seriesId)
        {
            BikeSeriesModelsVM objModelsVM = new BikeSeriesModelsVM();
            try
            {
                objModelsVM.SeriesModels = _bikeSeries.GetModelsListBySeriesId(modelId, seriesId);
                
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Models.BikeSeries.BikeModelsBySeriesPage.GetModelsBySeries ModelId = {0} and SeriesId = {1}", modelId, seriesId));
            }
            return objModelsVM;
        }
    }
}