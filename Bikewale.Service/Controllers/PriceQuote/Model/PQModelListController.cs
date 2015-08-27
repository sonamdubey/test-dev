using AutoMapper;
using Bikewale.BAL.BikeData;
using Bikewale.DAL.BikeData;
using Bikewale.DTO.PriceQuote.Make;
using Bikewale.DTO.PriceQuote.Model;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote.Model;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote.Model
{
    /// <summary>
    /// Price Quote Model List Controller
    /// Author  :   Sumit Kate
    /// Created on  :   20 Aug 2015
    /// </summary>
    public class PQModelListController : ApiController
    {
        /// <summary>
        /// Returns the List of Models of a Model
        /// </summary>
        /// <param name="makeId">Bike Make</param>
        /// <returns></returns>
        [ResponseType(typeof(PQModelList))]
        public HttpResponseMessage Get(int makeId)
        {
            List<BikeModelEntityBase> objModelList = null;
            PQModelList objDTOModelList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    IBikeModels<BikeModelEntity, int> ModelsRepository = null;

                    container.RegisterType<IBikeModels<BikeModelEntity, int>, BikeModels<BikeModelEntity, int>>();
                    ModelsRepository = container.Resolve<IBikeModels<BikeModelEntity, int>>();

                    objModelList = ModelsRepository.GetModelsByType(EnumBikeType.PriceQuote, makeId);

                    if (objModelList != null && objModelList.Count > 0)
                    {
                        // Auto map the properties
                        objDTOModelList = new PQModelList();
                        objDTOModelList.Models = PQModelEntityToDTO.ConvertModelList(objModelList);
                        return Request.CreateResponse(HttpStatusCode.OK, objDTOModelList);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.Model.PQModelListController.Get");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "some error occured.");
            }
        }
    }
}
