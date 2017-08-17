using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.DAL.BikeData;
using Bikewale.DTO.Make;
using AutoMapper;
using System.Web.Http.Description;
using Bikewale.Service.AutoMappers.Make;
using Bikewale.Notifications;

namespace Bikewale.Service.Controllers.Make
{
     /// <summary>
    /// To Get Make Details 
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// </summary>
    public class MakeController : ApiController
    {

        private readonly IBikeMakesCacheRepository<int> _bikeMakes = null;

        public MakeController(IBikeMakesCacheRepository<int> bikeMakes)
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
                        objDTOMakeBase = new MakeBase();
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Make.MakeController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }//get make details
        
    }
}
