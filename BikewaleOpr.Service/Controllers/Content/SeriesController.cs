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

    }
}
