using Bikewale.BAL.ApiGateway.Adapters.BikeData;
using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.BAL.ApiGateway.Entities.BikeData;
using Bikewale.BAL.PriceQuote;
using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.PriceQuote;
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
		private string _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
		private string _applicationid = ConfigurationManager.AppSettings["applicationId"];
		private readonly IBikeModelsRepository<BikeModelEntity, int> _modelRepository = null;
		private readonly IBikeModelsCacheRepository<int> _cache;
		private readonly IBikeModels<BikeModelEntity, int> _bikeModelEntity = null;
		private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _versionCacheRepository = null;
		private readonly IPQByCityArea _objPQByCityArea = null;
		private readonly IApiGatewayCaller _apiGatewayCaller;
		/// <summary>
		/// 
		/// </summary>
		/// <param name="modelRepository"></param>
		/// <param name="cache"></param>
		/// <param name="bikeModelEntity"></param>
		/// <param name="versionCacheRepository"></param>
		/// <param name="objPQByCityArea"></param>
		/// <param name="apiGatewayCaller"></param>
		public ModelSpecsController(IBikeModelsRepository<BikeModelEntity, int> modelRepository, IBikeModelsCacheRepository<int> cache, IBikeModels<BikeModelEntity, int> bikeModelEntity, IBikeVersionCacheRepository<BikeVersionEntity, uint> versionCacheRepository, IPQByCityArea objPQByCityArea, IApiGatewayCaller apiGatewayCaller)
		{
			_modelRepository = modelRepository;
			_cache = cache;
			_bikeModelEntity = bikeModelEntity;
			_versionCacheRepository = versionCacheRepository;
			_objPQByCityArea = objPQByCityArea;
			_apiGatewayCaller = apiGatewayCaller;
		}

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
				objModelPage = _bikeModelEntity.GetModelPageDetails(modelId);
				if (objModelPage.ModelVersionMinSpecs != null)
				{
					int versionId = objModelPage.ModelVersionMinSpecs.VersionId;
					if (versionId > 0)
					{
						objModelPage.VersionSpecsFeatures = _bikeModelEntity.GetFullSpecsFeatures(versionId);
					}
				}
				if (objModelPage != null)
				{
					objPQ = _objPQByCityArea.GetVersionList(modelId, objModelPage.ModelVersions, cityId, areaId, Convert.ToUInt16(platformId), null, null, deviceId);
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
				GetVersionSpecsSummaryByItemIdAdapter adapt = new GetVersionSpecsSummaryByItemIdAdapter();
				VersionsDataByItemIds_Input specItemInput = new VersionsDataByItemIds_Input
				{
					Versions = new List<int> { Convert.ToInt32(versionId) },
					Items = new List<EnumSpecsFeaturesItems>() {
                            EnumSpecsFeaturesItems.Displacement,
                            EnumSpecsFeaturesItems.MaxPower,
                            EnumSpecsFeaturesItems.MaximumTorque,
                            EnumSpecsFeaturesItems.NoOfGears,
                            EnumSpecsFeaturesItems.FuelEfficiencyOverall,
                            EnumSpecsFeaturesItems.FrontBrakeType,
                            EnumSpecsFeaturesItems.RearBrakeType,
                            EnumSpecsFeaturesItems.WheelType,
                            EnumSpecsFeaturesItems.KerbWeight,
                            EnumSpecsFeaturesItems.ChassisType,
                            EnumSpecsFeaturesItems.TopSpeed,
                            EnumSpecsFeaturesItems.TyreType,
                            EnumSpecsFeaturesItems.FuelTankCapacity
                        }
				};
				adapt.AddApiGatewayCall(_apiGatewayCaller, specItemInput);
				GetVersionSpecsByIdAdapter adapt1 = new GetVersionSpecsByIdAdapter();
				adapt1.AddApiGatewayCall(_apiGatewayCaller, new List<int> { (int)versionId });
				_apiGatewayCaller.Call();
				SpecsFeaturesEntity versionSpecsFeatures = adapt1.Output;
				if (versionSpecsFeatures != null)
				{
					IEnumerable<SpecsItem> summarySpecsList = null;
					if (adapt.Output != null && adapt.Output.Any())
					{
						summarySpecsList = adapt.Output.FirstOrDefault().MinSpecsList;
					}
					VersionSpecs versionSpecs = VersionListMapper.Convert(versionSpecsFeatures, summarySpecsList);
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