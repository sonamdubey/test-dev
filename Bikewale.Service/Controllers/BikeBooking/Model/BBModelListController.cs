using AutoMapper;
using Bikewale.DAL.BikeBooking;
using Bikewale.DTO.BikeBooking.Model;
using Bikewale.Entities.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Bikebooking.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.BikeBooking.Model
{
    /// <summary>
    /// BikeBooking Model List controller
    /// Author  :   Sumit Kate
    /// Created On  :   20 Aug 2015
    /// </summary>
    public class BBModelListController : ApiController
    {
        /// <summary>
        /// Returns the Model List for a make
        /// </summary>
        /// <param name="makeId">Make Id</param>
        /// <returns></returns>
        [ResponseType(typeof(BBModelList))]
        public HttpResponseMessage Get(UInt16 makeId)
        {
            IEnumerable<BikeModelEntityBase> objModelList = null;
            BBModelList objDTOModelList = null;

            ModelRepository repository = new ModelRepository();
            try
            {
                objModelList = repository.GetModelByMake("PRICEQUOTE", makeId);

                if (objModelList != null)
                {
                    objDTOModelList = new BBModelList();
                    objDTOModelList.Models = ModelEntityToDTO.Convert(objModelList);

                    return Request.CreateResponse(HttpStatusCode.OK, objDTOModelList);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.BikeBooking.Model.BBModelListController.Get");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }
        }
    }
}
