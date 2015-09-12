using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Bikewale.DTO.Model;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Model;

namespace Bikewale.Service.Controllers.Model
{
    public class ModelDescriptionController : ApiController
    {
        private string _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
        private string _applicationid = ConfigurationManager.AppSettings["applicationId"];
        private string _requestType = "application/json";
        private readonly IBikeModelsRepository<BikeModelEntity, int> _modelRepository = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelRepository"></param>
        public ModelDescriptionController(IBikeModelsRepository<BikeModelEntity, int> modelRepository)
        {
            _modelRepository = modelRepository;
        }

        #region Model Description
        /// <summary>
        ///  To get Long/Short Model's Description (Synopsis) 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Model Synopsis</returns>
        [ResponseType(typeof(ModelDescription)), Route("api/model/description/")]
        public IHttpActionResult Get(int modelId)
        {
            BikeDescriptionEntity objModelDesc = null;
            ModelDescription objDTOModelDesc = null;
            try
            {

                objModelDesc = _modelRepository.GetModelSynopsis(modelId);

                if (objModelDesc != null)
                {
                    // Auto map the properties
                    objDTOModelDesc = new ModelDescription();
                    objDTOModelDesc = ModelMapper.Convert(objModelDesc);
                    return Ok(objDTOModelDesc);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Model.ModelController");
                objErr.SendMail();
                return InternalServerError();
            }
        }   // Get 
        #endregion
    }
}
