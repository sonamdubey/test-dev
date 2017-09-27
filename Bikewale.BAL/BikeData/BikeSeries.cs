using System.Collections.Generic;
using Microsoft.Practices.Unity;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using System;
using Bikewale.Notifications;
using System.Linq;

namespace Bikewale.BAL.BikeData
{
    public class BikeSeries : IBikeSeries
    { 
        private readonly IBikeSeriesRepository _bikeSeriesRepository = null;

        public BikeSeries(IBikeSeriesRepository bikeSeriesRepository)
        {
            _bikeSeriesRepository = bikeSeriesRepository;
        }

        public BikeSeriesModels GetModelsListBySeriesId(int modelId, uint seriesId)
        {
            BikeSeriesModels objModels = null;
            try
            {
                if(seriesId > 0)
                {
                    objModels = _bikeSeriesRepository.GetModelsListBySeriesId(seriesId);
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("BAL.BikeData.BikeSeries.GetModelsListBySeries SeriesId = {0}", seriesId));
            }
            return objModels;
        }
    }   // class
}   // namespace
