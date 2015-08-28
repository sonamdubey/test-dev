using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Bikewale.Interfaces.Location;
using Bikewale.DAL.Location;
using Bikewale.DTO.Area;
using AutoMapper;
using System.Web.Http.Description;
using Bikewale.Entities.Location;
using Bikewale.Entities.BikeData;
using Bikewale.Service.AutoMappers.Area;
using Bikewale.Notifications;

namespace Bikewale.Service.Controllers.Area
{
    /// <summary>
    /// To Get List of Areas
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// </summary>
    public class AreaListController : ApiController
    {
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
                using (IUnityContainer container = new UnityContainer())
                {
                    IArea citysRepository = null;

                    container.RegisterType<IArea, AreaRepository>();
                    citysRepository = container.Resolve<IArea>();

                    objAreaList = citysRepository.GetAreas(cityId);

                    if (objAreaList != null && objAreaList.Count > 0)
                    {
                        objDTOAreaList = new AreaList();
                        objDTOAreaList.Area = AreaListMapper.Convert(objAreaList);

                        return Ok(objDTOAreaList);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Area.AreaListController");
                objErr.SendMail();
                return InternalServerError();
            }
        }   // Get Areas

    }
}
