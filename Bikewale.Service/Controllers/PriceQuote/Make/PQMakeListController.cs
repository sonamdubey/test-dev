using Bikewale.DAL.BikeData;
using Bikewale.DTO.PriceQuote.Make;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote.Make;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote.Make
{
    /// <summary>
    /// Price Quote Make List
    /// Author  : Sumit Kate
    /// Created on : 20 Aug 2015
    /// </summary>
    public class PQMakeListController : ApiController
    {
        /// <summary>
        /// Gets Makes List
        /// </summary>
        /// <returns>Make List</returns>
        [ResponseType(typeof(PQMakeList))]
        public HttpResponseMessage Get()
        {
            List<BikeMakeEntityBase> objMakeList = null;
            PQMakeList objDTOMakeList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    IBikeMakes<BikeMakeEntity, int> makesRepository = null;

                    container.RegisterType<IBikeMakes<BikeMakeEntity, int>, BikeMakesRepository<BikeMakeEntity, int>>();
                    makesRepository = container.Resolve<IBikeMakes<BikeMakeEntity, int>>();

                    objMakeList = makesRepository.GetMakesByType(EnumBikeType.PriceQuote);

                    if (objMakeList != null && objMakeList.Count > 0)
                    {
                        // Auto map the properties
                        objDTOMakeList = new PQMakeList();
                        objDTOMakeList.Makes = PQMakeEntityToDTO.ConvertMakeList(objMakeList);
                        return Request.CreateResponse(HttpStatusCode.OK, objDTOMakeList);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.Make.PQMakeListController.Get");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }
        }
    }
}
