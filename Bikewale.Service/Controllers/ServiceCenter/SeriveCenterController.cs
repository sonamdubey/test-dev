using Bikewale.DTO.City;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.ServiceCenter;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.ServiceCenter;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
namespace Bikewale.Service.Controllers.ServiceCenter
{
    /// <summary>
    /// Created By:- Subodh Jain 08 Nov 2016
    /// Summary :- For service center locator
    /// </summary>
    public class SeriveCenterController : ApiController
    {
        private readonly IServiceCenter _objServiceCenter = null;
        public SeriveCenterController(IServiceCenter ObjServiceCenter)
        {
            _objServiceCenter = ObjServiceCenter;
        }
        [HttpGet, Route("api/servicecenter/cities/make/{makeId}/"), ResponseType(typeof(IEnumerable<CityBase>))]
        public IHttpActionResult Get(uint makeId)
        {
            IEnumerable<CityEntityBase> objcityList = null;
            if (makeId > 0)
            {
                try
                {
                    objcityList = _objServiceCenter.GetServiceCenterCities(makeId);
                    if (objcityList != null)
                    {
                        IEnumerable<CityBase> cityList = ServiceCenterMapper.Convert(objcityList);
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
