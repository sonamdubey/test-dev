using Bikewale.Notifications;
using BikewaleOpr.Cache;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using System;
using System.Collections.Generic;
using System.Linq;
namespace BikewaleOpr.BAL
{
    public class BikeSeries : IBikeSeries
    {
        private readonly IBikeSeriesRepository _seriesRepo;
        private readonly IBikeModelsRepository _modelRepo;
        private readonly IBikeBodyStylesRepository _bodystyles;
        public BikeSeries(IBikeSeriesRepository seriesRepo, IBikeModelsRepository modelRepo, IBikeBodyStylesRepository bodystyles)
        {
            _seriesRepo = seriesRepo;
            _modelRepo = modelRepo;
            _bodystyles = bodystyles;
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 12th Sep 2017
        /// Summary : Get all bike series
        /// Modified by : Rajan Chauhan on 12th Dec 2017
        /// Summary : Added bike bodystyles list
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
                ErrorClass.LogError(ex, "BikewaleOpr.BAL.BikeSeries: GetSeries");
            }
            return objBikeSeriesList;
        }

		/// <summary>
		/// Created by : Vivek Singh Tomar on 12th Sep 2017
		/// Summary : Add new bike series
		/// Modified by : Ashutosh Sharma on 27 Nov 2017
        /// Description : Added call to ClearSeriesCache.
        /// Modified by : Rajan Chauhan on 12 Dec 2017
        /// Description : Added bodyStyleId in AddSeries.
		/// </summary>
		/// <param name="bikeSeries"></param>
		/// <param name="UpdatedBy"></param>
		/// <param name="seriesId"></param>
		/// <param name="isSeriesExist"></param>
		public Tuple<bool, string, BikeSeriesEntity> AddSeries(uint makeId, string seriesName, string seriesMaskingName, uint updatedBy, bool isSeriesPageUrl, uint? bodyStyleId)
        {   
           
            Tuple<bool, string, BikeSeriesEntity> respObj = null;
            if (!isSeriesPageUrl)
            {
                bodyStyleId = 0;
            }
            try
            {
                if (makeId > 0 && updatedBy > 0 && !string.IsNullOrEmpty(seriesName) && !string.IsNullOrEmpty(seriesMaskingName) && ((isSeriesPageUrl && (bodyStyleId != null && bodyStyleId != 0)) || !isSeriesPageUrl))
                {
                    BikeSeriesEntity objBikeSeries = new BikeSeriesEntity()
                    {
                        SeriesName = seriesName,
                        SeriesMaskingName = seriesMaskingName,
                        IsSeriesPageUrl = isSeriesPageUrl,
                        BikeMake = new BikeMakeEntityBase()
                        {
                            MakeId = Convert.ToInt32(makeId)
                        },
                        BodyStyle = new BikeBodyStyleEntity()
                        {
                            BodyStyleId = Convert.ToUInt32(bodyStyleId)
                        }
                    };
                    if (_seriesRepo.IsSeriesMaskingNameExists(makeId, seriesMaskingName) && isSeriesPageUrl)
                    {
                        respObj = new Tuple<bool, string, BikeSeriesEntity>(false, "Cannot create duplicate series page url", objBikeSeries);
                    }
                    else if (_modelRepo.IsModelMaskingNameExists(makeId, seriesMaskingName) && isSeriesPageUrl)
                    {
                        respObj = new Tuple<bool, string, BikeSeriesEntity>(false, "Given Series Masking Name already exists as Model Masking Name.", objBikeSeries);
                    }
                    else
                    {

                        _seriesRepo.AddSeries(objBikeSeries, updatedBy);
                        if (objBikeSeries.SeriesId > 0)
                        {
                            BikewaleOpr.Cache.BwMemCache.ClearMaskingMappingCache();
							BwMemCache.ClearSeriesCache(objBikeSeries.SeriesId, makeId);
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
                ErrorClass.LogError(ex, "BikewaleOpr.BAL.BikeSeries: AddSeries");
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
                ErrorClass.LogError(ex, "BikewaleOpr.BAL.BikeSeries: GetSeriesByMake");
            }
            return objBikeSeriesList;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 12-Sep-2017
        /// Description : BAL Method to edit bike series
        /// Modified by : Ashutosh Sharma on 30 Nov 2017
        /// Description : Added logic to model page cache clear.
        /// Modified by : Rajan Chauhan on 13 Dec 2017
        /// Description : Added bodyStyleId in AddSeries.
        /// </summary>
        /// <param name="bikeSeries"></param>
        /// <param name="updatedBy"></param>
        /// <returns></returns>
        public Tuple<bool, string> EditSeries(uint makeId, uint seriesId, string seriesName, string seriesMaskingName, int updatedBy, bool isSeriesPageUrl, uint? bodyStyleId)
        {
            Tuple<bool, string> respObj = null;
            string oldMaskingName = "";
            if (!isSeriesPageUrl)
            {
                bodyStyleId = 0;
            }
            try
            {
                if (seriesId > 0 && !string.IsNullOrEmpty(seriesName) && !string.IsNullOrEmpty(seriesMaskingName) && updatedBy > 0 && ((isSeriesPageUrl && (bodyStyleId != null && bodyStyleId != 0)) || !isSeriesPageUrl))
                {
                    BikeSeriesEntity bikeSeries = new BikeSeriesEntity()
                    {
                        SeriesId = seriesId,
                        SeriesName = seriesName,
                        SeriesMaskingName = seriesMaskingName,
                        IsSeriesPageUrl = isSeriesPageUrl,
                        UpdatedBy = Convert.ToString(updatedBy),
                        BodyStyle = new BikeBodyStyleEntity()
                        {
                            BodyStyleId = Convert.ToUInt32(bodyStyleId)
                        }
                    };

                    var series = _seriesRepo.GetSeriesByMake((int)makeId);
                    if (series != null)
                    {
                        var objSeries = series.FirstOrDefault(m => m.SeriesId == seriesId);
                        if (objSeries != null)
                        {
                            oldMaskingName = objSeries.SeriesMaskingName;
                        }
                    }

                    if (!oldMaskingName.Equals(seriesMaskingName) && _seriesRepo.IsSeriesMaskingNameExists(makeId, seriesMaskingName) && isSeriesPageUrl)
                    {
                        respObj = new Tuple<bool, string>(false, "Cannot create duplicate series page url");
                    }
                    else if (_modelRepo.IsModelMaskingNameExists(makeId, seriesMaskingName) && isSeriesPageUrl)
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
                            string modelIds = _seriesRepo.GetModelIdsBySeries(seriesId);
                            if (!string.IsNullOrEmpty(modelIds))
                            {
                                string[] arrModelIds = modelIds.Split(',');
                                foreach (var modelId in arrModelIds)
                                {
                                    BwMemCache.ClearVersionDetails(Convert.ToUInt32(modelId));
                                }
                            }
                            
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.BAL.BikeSeries: EditSeries_{0}_{1}", seriesId, updatedBy));
            }
            return respObj;
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 12-Sep-2017
        /// Description : BAL Method to delete bike series
        /// Modified by : Ashutosh Sharma on 30 Nov 2017
        /// Description : Added logic to model page cache clear.
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
                        string modelIds = _seriesRepo.GetModelIdsBySeries(bikeSeriesId);
                        if (!string.IsNullOrEmpty(modelIds))
                        {
                            string[] arrModelIds = modelIds.Split(',');
                            foreach (var modelId in arrModelIds)
                            {
                                BwMemCache.ClearVersionDetails(Convert.ToUInt32(modelId));
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.BAL.BikeSeries: DeleteSeries_{0}", bikeSeriesId));
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
                ErrorClass.LogError(ex, string.Format("BikewaleOpr.BAL.BikeSeries: DeleteMappingOfModelSeries_{0}", modelId));
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
                Bikewale.Notifications.ErrorClass.LogError(ex, "BikewaleOpr.BALs.BikeSeries.Getsynopsis");
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
                Bikewale.Notifications.ErrorClass.LogError(ex, "BikewaleOpr.BALs.BikeSeries.UpdateSynopsis");
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
