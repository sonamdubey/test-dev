using Bikewale.DTO.BikeBooking.Area;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Bikebooking.Area;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.BikeBooking.Area
{
    /// <summary>
    /// BikeBooking Area List Controller
    /// Author  : Sumit Kate
    /// Created on  : 20 Aug 2015
    /// </summary>
    public class BBAreaListController : CompressionApiController//ApiController
    {
        private readonly IArea _repository = null;
        public BBAreaListController(IArea repository)
        {
            _repository = repository;
        }
        /// <summary>
        /// Gets the list of Areas of a City
        /// </summary>
        /// <param name="cityId">City Id</param>
        /// <returns></returns>
        [ResponseType(typeof(BBAreaList))]
        public IHttpActionResult Get(UInt16 cityId)
        {
            IEnumerable<AreaEntityBase> objAreaList = null;
            BBAreaList objDTOAreaList = null;
            try
            {
                objAreaList = _repository.GetAreasByCity(cityId);

                if (objAreaList != null)
                {
                    objDTOAreaList = new BBAreaList();
                    objDTOAreaList.Areas = BBAreaListMapper.Convert(objAreaList);

                    objAreaList = null;

                    return Ok(objDTOAreaList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.BikeBooking.Area.BBAreaListController.Get");
               
                return InternalServerError();
            }
        }
    }
}
