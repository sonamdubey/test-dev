using Bikewale.DTO.Make;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Make;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;


namespace Bikewale.Service.Controllers.Make
{
    /// <summary>
    /// To Get Make Details 
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// </summary>
    public class MakeController : ApiController
    {

        private readonly IBikeMakesCacheRepository _bikeMakesCache = null;

        private readonly IBikeMakes<BikeMakeEntity, int> _bikeMakes = null;
        public MakeController(IBikeMakesCacheRepository bikeMakesCache, IBikeMakes<BikeMakeEntity, int> bikeMakes)
        {
            _bikeMakesCache = bikeMakesCache;
            _bikeMakes = bikeMakes;
        }

        /// <summary>
        ///  To get make Details based on MakeId  for DropDown
        /// </summary>
        /// <param name="makeId"></param>
        /// <returns>Make Details </returns>
        [ResponseType(typeof(MakeBase))]
        public IHttpActionResult Get(uint makeId)
        {
            BikeMakeEntityBase objMake = null;
            MakeBase objDTOMakeBase = null;
            try
            {
                if (makeId > 0)
                {
                    objMake = _bikeMakesCache.GetMakeDetails(makeId);

                    if (objMake != null)
                    {
                        objDTOMakeBase = MakeListMapper.Convert(objMake);
                        return Ok(objDTOMakeBase);
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Make.MakeController");

                return InternalServerError();
            }
            return NotFound();
        }//get make details

        /// <summary>
        /// Created By  : Deepak Israni on 20 November 2018
        /// Description : API to get all scooter makes data (excluding scooter only makes).
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("api/scootermakes/")]
        public IHttpActionResult GetScooterMakes(bool includeScooterOnly = false)
        {
            try
            {
                IEnumerable<BikeMakeEntityBase> scooterMakes = _bikeMakesCache.GetScooterMakes();

                if (scooterMakes != null)
                {
                    scooterMakes = includeScooterOnly ? scooterMakes : scooterMakes.Where(s => s.IsScooterOnly == false); 
                }
                else
                {
                    return NotFound();
                }
                IEnumerable<MakeBase> scooterOutput = MakeListMapper.Convert(scooterMakes);

                return Ok(scooterOutput);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Make.MakeController.GetScooterMakes()");
                return InternalServerError();
            }
            
        }

    }
}
