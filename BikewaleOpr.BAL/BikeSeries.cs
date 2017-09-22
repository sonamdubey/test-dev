using Bikewale.Notifications;
using BikewaleOpr.Entities.BikeData;
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
        public BikeSeriesEntity AddSeries(uint makeId, string seriesName, string seriesMaskingName, uint updatedBy, bool isSeriesPageUrl)
        {
            try
            {
                if (makeId > 0 && updatedBy > 0 && !string.IsNullOrEmpty(seriesName) && !string.IsNullOrEmpty(seriesMaskingName))
                {
                    BikeSeriesEntity objBikeSeries = new BikeSeriesEntity()
                    {
                        SeriesName = seriesName,
                        SeriesMaskingName = seriesMaskingName,
                        IsSeriesPageUrl = isSeriesPageUrl,
                        BikeMake = new BikeMakeEntityBase()
                        {
                            MakeId = Convert.ToInt32(makeId)
                        }
                    };
                    _seriesRepo.AddSeries(objBikeSeries, updatedBy);
                    return objBikeSeries;
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.BAL.BikeSeries: AddSeries");
            }
            return null;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 12th Sep 2017
        /// Summary : Fetch list of series according to make
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
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
        public bool EditSeries(uint seriesId, string seriesName, string seriesMaskingName, int updatedBy, bool isSeriesPageUrl)
        {
            bool IsEdited = false;
            try
            {
                if (seriesId > 0 && !string.IsNullOrEmpty(seriesName) && !string.IsNullOrEmpty(seriesMaskingName) && updatedBy > 0)
                {
                    BikeSeriesEntity bikeSeries = new BikeSeriesEntity()
                    {
                        SeriesId = seriesId,
                        SeriesName = seriesName,
                        SeriesMaskingName = seriesMaskingName,
                        IsSeriesPageUrl = isSeriesPageUrl,
                        UpdatedBy = Convert.ToString(updatedBy)
                    };
                    IsEdited = _seriesRepo.EditSeries(bikeSeries, updatedBy);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.BAL.BikeSeries: EditSeries_{0}_{1}", seriesId, updatedBy));
            }
            return IsEdited;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 12-Sep-2017
        /// Description : BAL Method to delete bike series
        /// </summary>
        /// <param name="bikeSeriesId"></param>
        /// <returns></returns>
        public bool DeleteSeries(uint bikeSeriesId, uint deletedBy)
        {
            bool IsDeleted = false;
            try
            {
                if (bikeSeriesId > 0)
                {
                    IsDeleted = _seriesRepo.DeleteSeries(bikeSeriesId, deletedBy);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.BAL.BikeSeries: DeleteSeries_{0}", bikeSeriesId));
            }
            return IsDeleted;
        }

        

        /// <summary>
        /// Created by : Ashutosh Sharma on 12-Sep-2017
        /// Description : BAL Method to delete bike series mapping with model
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public bool DeleteMappingOfModelSeries(uint modelId)
        {
            bool IsDeleted = false;
            try
            {
                if (modelId > 0)
                {
                    IsDeleted = _seriesRepo.DeleteMappingOfModelSeries(modelId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.BAL.BikeSeries: DeleteMappingOfModelSeries_{0}", modelId));
            }
            return IsDeleted;
        }
    }
}
