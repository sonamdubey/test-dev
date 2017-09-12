using Bikewale.Notifications;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Entity.BikeData;
using BikewaleOpr.Interface.BikeData;
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
        public IHttpActionResult Add(uint MakeId, string SeriesName, string SeriesMaskingName)
        {
            if (MakeId > 0 && !string.IsNullOrEmpty(SeriesName) && !string.IsNullOrEmpty(SeriesMaskingName))
            {
                BikeSeriesEntity objBikeSeries = new BikeSeriesEntity()
                {
                    SeriesName = SeriesName,
                    SeriesMaskingName = SeriesMaskingName,
                    BikeMake = new BikeMakeEntityBase()
                    {
                        MakeId = Convert.ToInt32(MakeId)
                    }
                };
                //_series.AddSeries(objBikeSeries, );

            }
            return Ok();
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
        public IHttpActionResult Edit(uint makeId, string seriesName, string seriesMaskingName, long updatedBy)
        {
            bool IsEdited = false;
            try
            {
                if (makeId > 0 && !string.IsNullOrEmpty(seriesName) && !string.IsNullOrEmpty(seriesMaskingName))
                {
                    BikeSeriesEntity objBikeSeries = new BikeSeriesEntity()
                    {
                        SeriesName = seriesName,
                        SeriesMaskingName = seriesMaskingName,
                        BikeMake = new BikeMakeEntityBase()
                        {
                            MakeId = Convert.ToInt32(makeId)
                        }
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
                    return InternalServerError();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("SeriesController.Edit_{0}_{1}_{2}", makeId, seriesName, seriesMaskingName, updatedBy));
                return InternalServerError();
            }
        }
        /// <summary>
        /// 
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
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("SeriesController.Delete_{0}", bikeSeriesId));
                return InternalServerError();
            }
        }

    }
}
