﻿using Bikewale.Notifications;
using BikewaleOpr.Cache;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using System;
using System.Collections.Generic;
namespace BikewaleOpr.BAL
{
    public class BikeSeries : IBikeSeries
    {
        private readonly IBikeSeriesRepository _seriesRepo;
        private readonly IBikeModelsRepository _modelRepo;
        public BikeSeries(IBikeSeriesRepository seriesRepo, IBikeModelsRepository modelRepo)
        {
            _seriesRepo = seriesRepo;
            _modelRepo = modelRepo;
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
        public Tuple<bool, string, BikeSeriesEntity> AddSeries(uint makeId, string seriesName, string seriesMaskingName, uint updatedBy, bool isSeriesPageUrl)
        {
            Tuple<bool, string, BikeSeriesEntity> respObj = null;
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
                    if (_seriesRepo.IsSeriesMaskingNameExists(makeId, seriesMaskingName) && isSeriesPageUrl)
                    {
                        respObj = new Tuple<bool, string, BikeSeriesEntity>(false, "Cannot create duplicate series page url", objBikeSeries);
                    }
                    else if (_modelRepo.IsModelMaskingNameExists(seriesMaskingName) && isSeriesPageUrl)
                    {
                        respObj = new Tuple<bool, string, BikeSeriesEntity>(false, "Given Series Masking Name already exists as Model Masking Name.", objBikeSeries);
                    }
                    else
                    {

                        _seriesRepo.AddSeries(objBikeSeries, updatedBy);
                        if (objBikeSeries.SeriesId > 0)
                        {
                            BikewaleOpr.Cache.BwMemCache.ClearMaskingMappingCache();
                            respObj = new Tuple<bool, string, BikeSeriesEntity>(true, "Bike series has been updated successfully.", objBikeSeries);
                        }
                        else
                        {
                            respObj = new Tuple<bool, string, BikeSeriesEntity>(false, "Bike series already exist", objBikeSeries);
                        }
                    }
                }
                else
                {
                    respObj = new Tuple<bool, string, BikeSeriesEntity>(false, "Input data is not correct", null);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.BAL.BikeSeries: AddSeries");
                respObj = new Tuple<bool, string, BikeSeriesEntity>(false, "Something went wrong, could't update.", null);
            }
            return respObj;
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
                if (makeId > 0)
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
        public Tuple<bool, string> EditSeries(uint makeId, uint seriesId, string seriesName, string seriesMaskingName, int updatedBy, bool isSeriesPageUrl)
        {
            Tuple<bool, string> respObj = null;
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
                    if (_seriesRepo.IsSeriesMaskingNameExists(makeId, seriesMaskingName) && isSeriesPageUrl)
                    {
                        respObj = new Tuple<bool, string>(false, "Cannot create duplicate series page url");
                    }
                    else if (_modelRepo.IsModelMaskingNameExists(seriesMaskingName) && isSeriesPageUrl)
                    {
                        respObj = new Tuple<bool, string>(false, "Given Series Masking Name already exists as Model Masking Name.");
                    }
                    else
                    {
                        bool isEdited = _seriesRepo.EditSeries(bikeSeries, updatedBy);
                        respObj = new Tuple<bool, string>(isEdited, isEdited ? "Updated Successfully" : "Failed to update");
                        if (isEdited)
                        {
                            BwMemCache.ClearModelsBySeriesId(seriesId);
                            BwMemCache.ClearMaskingMappingCache();
							BwMemCache.ClearSeriesCache(seriesId, makeId);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.BAL.BikeSeries: EditSeries_{0}_{1}", seriesId, updatedBy));
            }
            return respObj;
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
                    if (IsDeleted)
                    {
                        BwMemCache.ClearModelsBySeriesId(bikeSeriesId);
                        BwMemCache.ClearMaskingMappingCache();
						BwMemCache.ClearSeriesCache(bikeSeriesId, 0);
                    }
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
        /// Modified by : Ashutosh Sharma on 18 Oct 2017
        /// Description : Added call to ClearVersionDetails
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        public bool DeleteMappingOfModelSeries(uint modelId)
        {
            int seriesId = 0;
            try
            {
                if (modelId > 0)
                {
                    seriesId = _seriesRepo.DeleteMappingOfModelSeries(modelId);
                    if (seriesId != 0)
                    {
                        BwMemCache.ClearModelsBySeriesId(Convert.ToUInt32(seriesId));
                        BwMemCache.ClearVersionDetails(modelId);
                        BwMemCache.ClearMaskingMappingCache();
						BwMemCache.ClearSeriesCache(Convert.ToUInt32(seriesId), 0);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.BAL.BikeSeries: DeleteMappingOfModelSeries_{0}", modelId));
            }
            return seriesId > 0;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 7th Nov 2017
        /// Summary : BAL function for get synopsis
        /// </summary>
        /// <param name="seriesId"></param>
        /// <returns></returns>
        public SynopsisData Getsynopsis(int seriesId)
        {
            SynopsisData objSynopsis = null;
            try
            {
                if (seriesId > 0)
                {
                    objSynopsis = _seriesRepo.Getsynopsis(seriesId);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.BALs.BikeSeries.Getsynopsis");
            }

            return objSynopsis;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 7th Nov 2017
        /// Summary : BAL function for update synopsis
        /// </summary>
        /// <param name="seriesId"></param>
        /// <param name="updatedBy"></param>
        /// <param name="objSynopsis"></param>
        /// <returns></returns>
        public bool UpdateSynopsis(int seriesId, int updatedBy, SynopsisData objSynopsis)
        {
            bool isUpdated = false;
            try
            {
                if (seriesId > 0 && updatedBy > 0 && objSynopsis != null)
                {
                    isUpdated = _seriesRepo.UpdateSynopsis(seriesId, updatedBy, objSynopsis);
					BwMemCache.ClearSeriesCache(Convert.ToUInt32(seriesId), 0);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.BALs.BikeSeries.UpdateSynopsis");
            }
            return isUpdated;
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 21 Nov 2017
        /// Description :   Calls DAL function
        /// </summary>
        /// <param name="seriesMaskingName"></param>
        /// <returns></returns>
        public bool IsSeriesMaskingNameExists(uint makeId, string seriesMaskingName)
        {
            return _seriesRepo.IsSeriesMaskingNameExists(makeId, seriesMaskingName);
        }
    }
}
