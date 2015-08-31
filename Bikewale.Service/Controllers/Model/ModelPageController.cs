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
using Bikewale.Notifications;

namespace Bikewale.Service.Controllers.Model
{
    public class ModelPageController : ApiController
    {
        private readonly IBikeModelsRepository<BikeModelEntity, int> _modelRepository = null;
        public ModelPageController(IBikeModelsRepository<BikeModelEntity, int> modelRepository)
        {
            _modelRepository = modelRepository;
        }

        #region Model Details
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [ResponseType(typeof(ModelDetails))]
        public IHttpActionResult Get(int modelId)
        {
            BikeModelEntity objModel = null;
            ModelDetails objDTOModel = null;
            try
            {
                objModel = _modelRepository.GetById(modelId);

                if (objModel != null)
                {
                    // Auto map the properties
                    objDTOModel = new ModelDetails();

                    return Ok(objDTOModel);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Make.MakeController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }   // Get 
        #endregion
    }
}
