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

    }
}
