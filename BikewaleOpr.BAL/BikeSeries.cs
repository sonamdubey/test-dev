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
        public void AddSeries(BikeSeriesEntity bikeSeries, uint updatedBy)
        {
            try
            {
                if (bikeSeries != null && bikeSeries.BikeMake != null && updatedBy > 0)
                {
                    _seriesRepo.AddSeries(bikeSeries, updatedBy);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.BAL.BikeSeries: AddSeries");
            }
        }

        public IEnumerable<BikeSeriesEntityBase> GetSeriesByMake(int makeId)
        {
            IEnumerable<BikeSeriesEntityBase> objBikeSeriesList = null;
            try
            {
                if(makeId > 0)
                {
                    objBikeSeriesList = _seriesRepo.GetSeriesByMake(makeId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.BAL.BikeSeries: GetSeriesByMake");
            }
            return objBikeSeriesList;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 12-Sep-2017
        /// Description : BAL Method to edit bike series
        /// </summary>
        /// <param name="bikeSeries"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Created by : Ashutosh Sharma on 12-Sep-2017
        /// Description : BAL Method to delete bike series
        /// </summary>
        /// <param name="bikeSeriesId"></param>
        /// <returns></returns>
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
