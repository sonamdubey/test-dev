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
using AutoMapper;
using System.Web.Http.Description;
using Bikewale.DTO.Model;
using Bikewale.DTO.Series;
using Bikewale.DTO.Make;
using Bikewale.DTO.Version;
using Bikewale.Service.Controllers.Version;
using Bikewale.Service.AutoMappers.Model;

namespace Bikewale.Service.Controllers.Model
{
    public class ModelPageController : ApiController
    {
        #region Model Details
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [ResponseType(typeof(ModelDetails))]
        public HttpResponseMessage Get(int modelId)
        {
            BikeModelEntity objModel = null;
            ModelDetails objDTOModel = null;
            using (IUnityContainer container = new UnityContainer())
            {
                IBikeModelsRepository<BikeModelEntity, int> modelRepository = null;

                container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                modelRepository = container.Resolve<IBikeModelsRepository<BikeModelEntity, int>>();

                objModel = modelRepository.GetById(modelId);

                if (objModel != null)
                {
                    // Auto map the properties
                    objDTOModel = new ModelDetails();
                    //objDTOModel = ModelEntityToDTO.ConvertModelEntity(objModel);

                    return Request.CreateResponse(HttpStatusCode.OK, objDTOModel);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                }
            }
        }   // Get 
        #endregion
    }
}
