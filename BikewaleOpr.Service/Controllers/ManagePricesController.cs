using Bikewale.Notifications;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Interface.BikePricing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BikewaleOpr.Service.Controllers
{
    /// <summary>
    /// Created by: Ashutosh Sharma on 29-07-2017
    /// Discription: Manage Price for bikes API controller
    /// </summary>
    public class ManagePricesController : ApiController
    {
        private readonly IShowroomPricesRepository _pricesRepo;
        public ManagePricesController(IShowroomPricesRepository pricesRepo)
        {
            _pricesRepo = pricesRepo;
        }

        /// <summary>
        /// Created by: Ashutosh Sharma on 29-07-2017
        /// Discription: Get Bike Models of a Bike Make
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns>List of Bike Models</returns>
        [HttpGet, Route("api/manageprices/getmodels/{makeId}")]
        public IHttpActionResult GetModelsByMake(uint makeId)
        {
            IEnumerable<BikeModelEntityBase> modelList = null;
            try
            {
                modelList = _pricesRepo.GetModelsByMake(makeId);
                if(modelList != null){
                    return Ok(modelList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("BikewaleOpr.Models.ManagePrices.PriceMonitoringModel.GetModelsByMake_{0}",makeId));
                return InternalServerError();
            }
        }
    }
}
