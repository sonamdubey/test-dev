using Bikewale.DTO.Model;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Model;
using System;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Model
{
    public class ModelDescriptionController : ApiController
    {
        private readonly IBikeModelsCacheRepository<int> _modelRepository = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelRepository"></param>
        public ModelDescriptionController(IBikeModelsCacheRepository<int> modelRepository)
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Model.ModelController");
               
                return InternalServerError();
            }
        }   // Get 
        #endregion
    }
}
