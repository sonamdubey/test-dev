using Bikewale.Notifications;
using BikewaleOpr.DTO.BikeData;
using BikewaleOpr.Entities.BikeData;
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
        public IHttpActionResult Add(uint makeId, string seriesName, string seriesMaskingName, uint updatedBy, bool isSeriesPageUrl)
        {
            BikeSeriesDTO objBikeSeriesDTO = null;
            BikeSeriesEntity objBikeSeries = null;
            objBikeSeries = _series.AddSeries(makeId, seriesName, seriesMaskingName, updatedBy, isSeriesPageUrl);
            if(objBikeSeries != null)
            {
                try
                {
                    if (objBikeSeries.SeriesId == 0)
                    {
                        return BadRequest("Bike series already exist");
                    }
                    objBikeSeriesDTO = BikeSeriesMapper.Convert(objBikeSeries);
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "BikewalwOpr.Service.Controllers.SeriesController: Add");
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
        public IHttpActionResult Edit(uint seriesId, string seriesName, string seriesMaskingName, int updatedBy, bool isSeriesPageUrl)
        {
            bool IsEdited = false;
            try
            {
                IsEdited = _series.EditSeries(seriesId, seriesName, seriesMaskingName, updatedBy, isSeriesPageUrl);
                if (IsEdited)
                {
                    return Ok(IsEdited);
                }
                else
                {
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("SeriesController.Edit_{0}_{1}_{2}", seriesId, seriesName, seriesMaskingName, updatedBy));
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
        public IHttpActionResult Delete(uint seriesId)
        {
            bool isDeleted = false;
            try
            {
                if (seriesId > 0)
                {
                    isDeleted = _series.DeleteSeries(seriesId);

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
                ErrorClass objErr = new ErrorClass(ex, string.Format("SeriesController.Delete_{0}", seriesId));
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
                ErrorClass objErr = new ErrorClass(ex, string.Format("SeriesController.Delete_{0}", modelId));
                return InternalServerError();
            }
        }

    }
}
