using Bikewale.Entities.Location;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Notifications;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.ServiceCenter
{
    public class SeriveCenterController : ApiController
    {
        private readonly IServiceCenter _objServiceCenter = null;
        public SeriveCenterController(IServiceCenter ObjServiceCenter)
        {
            _objServiceCenter = ObjServiceCenter;
        }
        [HttpGet, Route("api/servicecenter/cities/make/{makeId}/"), ResponseType(typeof(IEnumerable<CityEntityBase>))]
        public IHttpActionResult Get(uint makeId)
        {
            IEnumerable<CityEntityBase> cityList = null;
            if (makeId > 0)
            {
                try
                {
                    cityList = _objServiceCenter.GetServiceCenterCities(makeId);
                    if (cityList != null)
                    {
                        return Ok(cityList);
                    }

                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "Bikewale.Service.Controllers.City.SeriveCenterController.Get");
                    objErr.SendMail();
                    return InternalServerError();
                }
            }
            else
            {
                return BadRequest();

            }
            return NotFound();
        }
    }
}
