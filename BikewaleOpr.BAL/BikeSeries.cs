using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BikewaleOpr.BAL
{
    public class BikeSeries: IBikeSeries
    {
        private IBikeSeriesRepository _series;
        public BikeSeries(IBikeSeriesRepository series)
        {
            _series = series;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 12th Sep 2017
        /// Summary : Get all bike series
        /// </summary>
        /// <returns></returns>
        public IEnumerable<BikeSeriesEntity> GetSeries()
        {
            IEnumerable<BikeSeriesEntity> objBikeSeriesList = null;
            try
            {
                objBikeSeriesList = _series.GetSeries();
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.BAL.BikeSeries: GetSeries");
            }
            return objBikeSeriesList;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 12th Sep 2017
        /// Summary : Add new bike series
        /// </summary>
        /// <param name="bikeSeries"></param>
        /// <param name="UpdatedBy"></param>
        /// <param name="seriesId"></param>
        /// <param name="isSeriesExist"></param>
        public uint AddSeries(BikeSeriesEntity bikeSeries, uint UpdatedBy)
        {
            uint seriesId = 0;
            try
            {
                if(bikeSeries != null && bikeSeries.BikeMake != null && UpdatedBy > 0)
                {
                    seriesId = _series.AddSeries(bikeSeries, UpdatedBy);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.BAL.BikeSeries: AddSeries");
            }
            return seriesId;
        }
    }
}
