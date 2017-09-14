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
        [HttpPost, Route("api/bikeseries/add/")]
        public IHttpActionResult Add(uint MakeId, string SeriesName, string SeriesMaskingName, uint UpdatedBy)
        {
            BikeSeriesDTO objBikeSeriesDTO = null;
            if (MakeId > 0 && !string.IsNullOrEmpty(SeriesName) && !string.IsNullOrEmpty(SeriesMaskingName))
            {
                try
                { 
                    BikeSeriesEntity objBikeSeries = new BikeSeriesEntity()
                    {
                        SeriesName = SeriesName,
                        SeriesMaskingName = SeriesMaskingName,
                        CreatedOn = DateTime.Now,
                        UpdatedOn = DateTime.Now,
                        BikeMake = new BikeMakeEntityBase()
                        {
                            MakeId = Convert.ToInt32(MakeId)
                        }
                    };
                    _series.AddSeries(objBikeSeries, UpdatedBy);
                    if(objBikeSeries.SeriesId == 0)
                    {
                        return BadRequest("Input bike series is redundant");
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
        [HttpPost, Route("api/bikeseries/edit/")]
        public IHttpActionResult Edit(uint seriesId, string seriesName, string seriesMaskingName, int updatedBy)
        {
            bool IsEdited = false;
            try
            {
                if (seriesId > 0 && !string.IsNullOrEmpty(seriesName) && !string.IsNullOrEmpty(seriesMaskingName) && updatedBy > 0)
                {
                    BikeSeriesEntity objBikeSeries = new BikeSeriesEntity()
                    {
                        SeriesId = seriesId,
                        SeriesName = seriesName,
                        SeriesMaskingName = seriesMaskingName,
                        UpdatedBy = Convert.ToString(updatedBy)
                    };
                    IsEdited = _series.EditSeries(objBikeSeries, updatedBy);
                    if (IsEdited)
                    {
                        return Ok(IsEdited);
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
        [HttpPost, Route("api/bikeseries/delete/")]
        public IHttpActionResult Delete(uint bikeSeriesId)
        {
            bool isDeleted = false;
            try
            {
                if (bikeSeriesId > 0)
                {
                    isDeleted = _series.DeleteSeries(bikeSeriesId);

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
                ErrorClass objErr = new ErrorClass(ex, string.Format("SeriesController.Delete_{0}", bikeSeriesId));
                return InternalServerError();
            }
        }

    }
}
