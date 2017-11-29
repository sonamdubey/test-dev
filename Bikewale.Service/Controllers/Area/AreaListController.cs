using Bikewale.DTO.Area;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Area;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Area
{
    /// <summary>
    /// To Get List of Areas
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// </summary>
    public class AreaListController : ApiController
    {
        
        private readonly IArea _area = null;
        public AreaListController(IArea area)
        {
            _area = area;
        }

        /// <summary>
        /// Get List of Areas based on city
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns>Areas List</returns>
        [ResponseType(typeof(AreaList))]
        public IHttpActionResult Get(string cityId)
        {
            List<AreaEntityBase> objAreaList = null;
            AreaList objDTOAreaList = null;
            try
            {
                objAreaList = _area.GetAreas(cityId);

                if (objAreaList != null && objAreaList.Count > 0)
                {
                    objDTOAreaList = new AreaList();
                    objDTOAreaList.Area = AreaListMapper.Convert(objAreaList);
                                        
                    objAreaList.Clear();
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Area.AreaListController");
               
                return InternalServerError();
            }
        }   // Get Areas

    }
}
