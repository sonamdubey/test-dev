using Bikewale.DTO.Make;
using Bikewale.DTO.Upcoming;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Make;
using System;
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

        private readonly IBikeMakesCacheRepository _bikeMakes = null;

        public MakeController(IBikeMakesCacheRepository bikeMakes)
        {
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
                    objMake = _bikeMakes.GetMakeDetails(makeId);

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
        
        [Route("api/notifyuser/")]
        public IHttpActionResult PostNotification([FromBody]UpcomingNotification notifObj)
        {

        }

    }
}
