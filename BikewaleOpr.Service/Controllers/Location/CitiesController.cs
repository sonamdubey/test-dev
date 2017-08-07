using Bikewale.Notifications;
using BikewaleOpr.DALs.Location;
using BikewaleOpr.DTO.Location;
using BikewaleOpr.Entity;
using BikewaleOpr.Interface.Location;
using BikewaleOpr.Service.AutoMappers.Location;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace BikewaleOpr.Service.Controllers.Location
{
    /// <summary>
    /// Created By  :   Vishnu Teja Yalakuntla on 01 Aug 2017
    /// Description :   API for managing  all operations related to cities.
    /// </summary>
    public class CitiesController : ApiController
    {
        private readonly ILocation location = null;
        public CitiesController()
        {
            location = new LocationRepository();
        }
        /// <summary>
        /// Created By  :   Vishnu Teja Yalakuntla on 01 Aug 2017
        /// Description :   Fetches all cities which belong to the given state.
        /// </summary>
        /// <param name="stateId"></param>
        /// <returns></returns>
        [HttpGet, ResponseType(typeof(CityNameDTO)), Route("api/cities/state/{stateid}/")]
        public IHttpActionResult GetCitiesByState(UInt32 stateId)
        {
            if (stateId > 0)
            {
                IEnumerable<CityNameEntity> cities = null;
                IEnumerable<CityNameDTO> cityDtos = null;

                try
                {
                    cities = location.GetCitiesByState(stateId);
                    cityDtos = CityMapper.Convert(cities);
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, string.Format("GetCitiesByState stateId={0}", stateId));
                }
                if (cityDtos != null)
                    return Ok(cityDtos);
                else
                    return NotFound();
            }
            else
                return BadRequest();
        }

    }
}
