using Bikewale.DAL.Location;
using Bikewale.DTO.BikeBooking.Area;
using Bikewale.Entities.Location;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Bikebooking.Area;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.BikeBooking.Area
{
    /// <summary>
    /// BikeBooking Area List Controller
    /// Author  : Sumit Kate
    /// Created on  : 20 Aug 2015
    /// </summary>
    public class BBAreaListController : ApiController
    {
        /// <summary>
        /// Gets the list of Areas of a City
        /// </summary>
        /// <param name="cityId">City Id</param>
        /// <returns></returns>
        [ResponseType(typeof(BBAreaList))]
        public HttpResponseMessage Get(UInt16 cityId)
        {
            IEnumerable<AreaEntityBase> objAreaList = null;
            BBAreaList objDTOAreaList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IArea, AreaRepository>();
                    IArea repository = container.Resolve<IArea>();
                    objAreaList = repository.GetAreasByCity(cityId);

                    if (objAreaList != null)
                    {
                        objDTOAreaList = new BBAreaList();
                        objDTOAreaList.Areas = AreaEntityToDTO.Convert(objAreaList);
                        return Request.CreateResponse(HttpStatusCode.OK, objDTOAreaList);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.BikeBooking.Area.BBAreaListController.Get");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }
        }
    }
}
