using AutoMapper;
using Bikewale.BAL.BikeData;
using Bikewale.DAL.BikeBooking;
using Bikewale.DTO.PriceQuote.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote.Version;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote.Version
{
    /// <summary>
    /// Price Quote Version List controller
    /// Author  :   Sumit Kate
    /// Created On  :   20 Aug 2015
    /// </summary>
    public class PQVersionListController : ApiController
    {
        /// <summary>
        /// Gets the Version list for given model and city
        /// </summary>
        /// <param name="modelId">model Id</param>
        /// <param name="cityId">city id</param>
        /// <returns></returns>
        [ResponseType(typeof(PQVersionList))]
        public HttpResponseMessage Get(uint modelId, int? cityId = null)
        {
            List<BikeVersionsListEntity> objVersionList = null;
            PQVersionList objDTOVersionList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IBikeVersions<BikeVersionEntity, uint>, BikeVersions<BikeVersionEntity, uint>>();
                    IBikeVersions<BikeVersionEntity, uint> objVersion = container.Resolve<IBikeVersions<BikeVersionEntity, uint>>();
                    objVersionList = objVersion.GetVersionsByType(EnumBikeType.PriceQuote, Convert.ToInt32(modelId), cityId);

                    if (objVersionList != null && objVersionList.Count > 0)
                    {
                        // Auto map the properties
                        objDTOVersionList = new PQVersionList();
                        objDTOVersionList.Versions = PQVersionEntityToDTO.VersionList(objVersionList);

                        return Request.CreateResponse(HttpStatusCode.OK, objDTOVersionList);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.Version.PQVersionListController.Get");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }
        }
    }
}
