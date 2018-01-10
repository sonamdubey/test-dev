using Bikewale.Notifications;
using BikewaleOpr.DTO.BikeData;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Service.AutoMappers.BikeData;
using System;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers.Content
{
    /// <summary>
    /// Created by: Vivek Singh Tomar on 12th Sep 2017
    /// Summary: API to perform crud operatin on bike series
    /// </summary>
    public class SeriesController : ApiController
    {
        private readonly IBikeSeries _series;
        public SeriesController(IBikeSeries series)
        {
            _series = series;
        }

        /// <summary>
        /// Created by: Vivek Singh Tomar on 12th Sep 2017
        /// Summary: Add Bike Series
        /// </summary>
        /// <returns></returns>
        [HttpPost, Route("api/make/{makeId}/series/add/")]
        public IHttpActionResult Add(uint makeId, string seriesName, string seriesMaskingName, uint updatedBy, bool isSeriesPageUrl, uint? bodyStyleId)
        {
            BikeSeriesDTO objBikeSeriesDTO = null;

            Tuple<bool, string, BikeSeriesEntity> balResp = null;

            balResp = _series.AddSeries(makeId, seriesName, seriesMaskingName, updatedBy, isSeriesPageUrl, bodyStyleId);
            if (balResp != null)
            {
                try
                {
                    objBikeSeriesDTO = BikeSeriesMapper.Convert(balResp.Item3);
                    objBikeSeriesDTO.Message = balResp.Item2;
                }
                catch (Exception ex)
                {
                    ErrorClass.LogError(ex, "BikewalwOpr.Service.Controllers.SeriesController: Add");
                }
                return Ok(objBikeSeriesDTO);
            }
            else
            {
                return BadRequest("Input data is not correct");
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 12-Sep-2017
        /// Description : API to edit Bike Series
        /// </summary>
        /// <param name="MakeId"></param>
        /// <param name="SeriesName"></param>
        /// <param name="seriesMaskingName"></param>
        /// <returns></returns>
        [HttpPost, Route("api/series/{seriesId}/edit/")]
        public IHttpActionResult Edit(uint makeId, uint seriesId, string seriesName, string seriesMaskingName, int updatedBy, bool isSeriesPageUrl, uint? bodyStyleId)
        {
            Tuple<bool, string> balResp = null;
            try
            {
                balResp = _series.EditSeries(makeId, seriesId, seriesName, seriesMaskingName, updatedBy, isSeriesPageUrl, bodyStyleId);
                if (balResp != null)
                {
                    if (balResp.Item1)
                        return Ok(balResp.Item2);
                    else
                        return BadRequest(balResp.Item2);
                }
                else
                {
                    return BadRequest("Input data is not correct");
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("SeriesController.Edit_{0}_{1}_{2}", seriesId, seriesName, seriesMaskingName, updatedBy));
                return InternalServerError();
            }
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 12-Sep-2017
        /// Description : API to delete Bike Series
        /// </summary>
        /// <param name="MakeId"></param>
        /// <param name="SeriesName"></param>
        /// <param name="SeriesMaskingName"></param>
        /// <returns></returns>
        [HttpPost, Route("api/make/series/{seriesId}/delete/")]
        public IHttpActionResult Delete(uint seriesId, uint deletedBy)
        {
            bool isDeleted = false;
            try
            {
                if (seriesId > 0)
                {
                    isDeleted = _series.DeleteSeries(seriesId, deletedBy);

                    if (isDeleted)
                    {
                        return Ok();
                    }
                    else
                    {
                        return InternalServerError();
                    }
                }
                else
                {
                    return BadRequest("Input data is not correct");
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("SeriesController.Delete_{0}", seriesId));
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by : Ashutosh Sharma on 12-Sep-2017
        /// Description : API to delete Bike Series
        /// </summary>
        /// <param name="MakeId"></param>
        /// <param name="SeriesName"></param>
        /// <param name="SeriesMaskingName"></param>
        /// <returns></returns>
        [HttpPost, Route("api/model/{modelId}/series/delete/")]
        public IHttpActionResult DeleteMappingOfModelSeries(uint modelId)
        {
            bool isDeleted = false;
            try
            {
                if (modelId > 0)
                {
                    isDeleted = _series.DeleteMappingOfModelSeries(modelId);

                    if (isDeleted)
                    {
                        return Ok();
                    }
                    else
                    {
                        return InternalServerError();
                    }
                }
                else
                {
                    return BadRequest("Input data is not correct");
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("SeriesController.Delete_{0}", modelId));
                return InternalServerError();
            }
        }


        /// <summary>
        /// Created by : Vivek Singh Tomar on 7th Nov 2017
        ///  Summary : API for get synopsis 
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [HttpGet, Route("api/series/{seriesid}/synopsis/")]
        public IHttpActionResult GetSynopsis(int seriesId)
        {
            if (seriesId > 0)
            {
                SynopsisData objSynopsis = null;

                try
                {
                    objSynopsis = _series.Getsynopsis(seriesId);
                    if (objSynopsis != null)
                        return Ok(BikeDataMapper.Convert(objSynopsis));
                    else
                        return NotFound();
                }
                catch (Exception ex)
                {
                    Bikewale.Notifications.ErrorClass.LogError(ex, "GetSynopsis : Series");
                    return InternalServerError();
                }
            }
            else
                return BadRequest();
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 7th Nov 2017
        /// Summary : API for updating synopsis
        /// </summary>
        /// <param name="seriesId"></param>
        /// <param name="objSynopsisDto"></param>
        /// <returns></returns>
        [HttpPost, Route("api/series/{seriesid}/synopsis/")]
        public IHttpActionResult SaveSynopsis(int seriesId, [FromBody] SynopsisDataDto objSynopsisDto)
        {
            SynopsisData objSynopsis = BikeDataMapper.Convert(objSynopsisDto);
            bool isUpdated = false;
            if (seriesId > 0)
            {
                try
                {
                    int userId = 0;
                    int.TryParse(Bikewale.Utility.OprUser.Id, out userId);

                    isUpdated = _series.UpdateSynopsis(seriesId, userId, objSynopsis);
                    if (!isUpdated)
                    {
                        return Ok(false);
                    }
                }
                catch (Exception ex)
                {
                    Bikewale.Notifications.ErrorClass.LogError(ex, "SaveSynopsis");
                    return InternalServerError();
                }
            }
            else
                return BadRequest("Invalid inputs");

            return Ok(true);
        }

    }
}
