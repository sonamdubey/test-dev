using System;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Model;
using Bikewale.DTO.Model;

namespace Bikewale.Service.Controllers.Model
{
    public class ModelSpecsController : ApiController
    {
        private string _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
        private string _applicationid = ConfigurationManager.AppSettings["applicationId"];
        private readonly IBikeModelsRepository<BikeModelEntity, int> _modelRepository = null;
        private readonly IBikeModelsCacheRepository<int> _cache;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelRepository"></param>
        public ModelSpecsController(IBikeModelsRepository<BikeModelEntity, int> modelRepository,  IBikeModelsCacheRepository<int> cache)
        {
            _modelRepository = modelRepository;
            _cache = cache;
        }

        #region Model Specifications and Features
        /// <summary>
        ///  To get Model's Specifications and Features based on VersionId 
        /// </summary>
        /// <param name="versionId">Model's Version ID</param>        
        /// <returns>Model's Specs and Features</returns>
        [ResponseType(typeof(VersionSpecifications)), Route("api/version/specs/")]
        public IHttpActionResult Get(int versionId)
        {
            BikeSpecificationEntity objVersionSpecs = null;
            VersionSpecifications objDTOVersionSpecs = null;
            try
            {
                objVersionSpecs = _modelRepository.MVSpecsFeatures(versionId);

                if (objVersionSpecs != null)
                {
                    // Auto map the properties
                    objDTOVersionSpecs = new VersionSpecifications();
                    objDTOVersionSpecs = ModelMapper.Convert(objVersionSpecs);
                    return Ok(objDTOVersionSpecs);
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

        }   // Get  Specs and Features
        #endregion

        /// <summary>
        /// Created By : Lucky Rathore on 14 Apr 2016
        /// Description : API to give Model Specification, Feature, Versions and Colors.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [ResponseType(typeof(BikeSpecs)), Route("api/model/bikespecs/")]
        public IHttpActionResult Get(int modelId, UInt16? cityId, UInt16? areaId)
        {
            BikeModelPageEntity objModelPage = null;
            BikeSpecs specs = null;
            try
            {
                objModelPage = _cache.GetModelPageDetails(modelId);

                if (objModelPage != null)
                {
                    specs = new BikeSpecs();
                    specs = ModelMapper.ConvertToBikeSpecs(objModelPage);
                    return Ok(specs);
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
        } 
    }
}
