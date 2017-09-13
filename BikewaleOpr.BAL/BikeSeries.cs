using Bikewale.Notifications;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using System;
using System.Collections.Generic;
namespace BikewaleOpr.BAL
{
    public class BikeSeries: IBikeSeries
    {
        private readonly IBikeSeriesRepository _seriesRepo;
        public BikeSeries(IBikeSeriesRepository seriesRepo)
        {
            _seriesRepo = seriesRepo;
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
                objBikeSeriesList = _seriesRepo.GetSeries();
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
        public void AddSeries(BikeSeriesEntity bikeSeries, uint UpdatedBy)
        {
            try
            {
                if (bikeSeries != null && bikeSeries.BikeMake != null && UpdatedBy > 0)
                {
                    _seriesRepo.AddSeries(bikeSeries, UpdatedBy);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.BAL.BikeSeries: AddSeries");
            }
        }

        public bool EditSeries(BikeSeriesEntity bikeSeries, int updatedBy)
        {
            bool IsEdited = false;
            try
            {
                if (bikeSeries != null && bikeSeries.SeriesId > 0 && updatedBy > 0)
                {
                    IsEdited = _seriesRepo.EditSeries(bikeSeries, updatedBy);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.BAL.BikeSeries: EditSeries_{0}_{1}", bikeSeries, updatedBy));
            }
            return IsEdited;
        }

        public bool DeleteSeries(uint bikeSeriesId)
        {
            bool IsDeleted = false;
            try
            {
                if (bikeSeriesId > 0)
                {
                    IsDeleted = _seriesRepo.DeleteSeries(bikeSeriesId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.BAL.BikeSeries: DeleteSeries_{0}", bikeSeriesId));
            }
            return IsDeleted;
        }


    }
}
