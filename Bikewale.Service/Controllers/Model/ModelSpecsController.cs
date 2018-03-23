using Bikewale.BAL.GrpcFiles.Specs_Features;
using Bikewale.BAL.PriceQuote;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Model;
using Bikewale.Service.AutoMappers.Version;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Model
{
    /// <summary>
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class ModelSpecsController : CompressionApiController//ApiController
    {
        private  string _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
        private string _applicationid = ConfigurationManager.AppSettings["applicationId"];
        private readonly IBikeModelsRepository<BikeModelEntity, int> _modelRepository = null;
        private readonly IBikeModelsCacheRepository<int> _cache;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModelEntity = null;
		private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _versionCacheRepository = null;

		/// <summary>
		/// 
		/// </summary>
		/// <param name="modelRepository"></param>
		/// <param name="cache"></param>
		/// <param name="bikeModelEntity"></param>
		/// <param name="versionCacheRepository"></param>
		public ModelSpecsController(IBikeModelsRepository<BikeModelEntity, int> modelRepository, IBikeModelsCacheRepository<int> cache, IBikeModels<BikeModelEntity, int> bikeModelEntity, IBikeVersionCacheRepository<BikeVersionEntity, uint> versionCacheRepository)
        {
            _modelRepository = modelRepository;
            _cache = cache;
            _bikeModelEntity = bikeModelEntity;
			_versionCacheRepository = versionCacheRepository;

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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Model.ModelController");
               
                return InternalServerError();
            }

        }   // Get  Specs and Features
        #endregion

        /// <summary>
        /// Created By : Lucky Rathore on 14 Apr 2016
        /// Description : API to give Model Specification, Feature, Versions and Colors.
        /// Modified by :   Sumit Kate on 23 May 2016
        /// Description :   Get the Device Id from deviceId parameter
        /// Modified by Sajal Gupta on 28-02-2017
        /// Descrioption : Call BAL function instead of cache function to fetch model details.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [ResponseType(typeof(BikeSpecs)), Route("api/model/bikespecs/")]
        public IHttpActionResult GetBikeSpecs(int modelId, int? cityId, int? areaId, string deviceId = null)
        {
            if (modelId <= 0 || cityId <= 0 || areaId <= 0)
            {
                return BadRequest();
            }
            PQByCityArea getPQ = null;
            BikeModelPageEntity objModelPage = null;
            BikeSpecs specs = null;
            PQByCityAreaEntity objPQ = null;
            try
            {
                string platformId = string.Empty;

                if (Request.Headers.Contains("platformId"))
                {
                    platformId = Request.Headers.GetValues("platformId").First().ToString();
                    if (string.IsNullOrEmpty(platformId) || (platformId != "3" && platformId != "4"))
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return BadRequest();
                }

                getPQ = new PQByCityArea();
                objModelPage = _bikeModelEntity.GetModelPageDetails(modelId);
                objModelPage.VersionSpecsFeatures = SpecsFeaturesServiceGateway.GetVersionsSpecsFeatures(new List<uint>{ 1 });

                if (objModelPage != null)
                {
                    objPQ = getPQ.GetVersionList(modelId, objModelPage.ModelVersions, cityId, areaId, Convert.ToUInt16(platformId), null, null, deviceId);
                    if (objPQ != null)
                    {
                        specs = ModelMapper.ConvertToBikeSpecs(objModelPage, objPQ);
                        return Ok(specs);
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Model.ModelController.GetBikeSpecs");
               
                return InternalServerError();
            }
        }
        /// <summary>
        /// Created by : Ashutosh Sharma on 26 Dec 2017
        /// Description : API to get specs and features of a version.
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        [HttpGet, ResponseType(typeof(VersionSpecs)), Route("api/version/{versionId}/specs/")]
        public IHttpActionResult GetBikeVersionSpecs(uint versionId)
        {
            try
            {
                if (versionId <= 0)
                {
                    return BadRequest();
                }
                SpecsFeaturesEntity versionSpecsFeatures = SpecsFeaturesServiceGateway.GetVersionsSpecsFeatures(new List<uint> { versionId});
        
                if (versionSpecsFeatures != null)
                {
                    VersionSpecs versionSpecs = VersionListMapper.Convert(versionSpecsFeatures);
                    if (versionSpecs != null)
                    {
                        return Ok(versionSpecs);
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.Model.ModelSpecsController.GetBikeVersionSpecs");
                return InternalServerError();
            }
        }
	}
}
