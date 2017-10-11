﻿using Bikewale.BAL.PriceQuote;
using Bikewale.DTO.PriceQuote.Version;
using Bikewale.DTO.PriceQuote.Version.v2;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Model;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote.Version
{
    /// <summary>
    /// Created by: Sangram Nandkhile on 20 Apr 2016
    /// Summary: API to return PriceQuote for model by city and area
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class PQVersionListByCityAreaController : CompressionApiController//ApiController
    {
        private readonly IBikeVersions<BikeVersionEntity, uint> _objVersion = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objVersionCache = null;
        private readonly IBikeModelsRepository<BikeModelEntity, int> _objModel = null;

        /// <summary>
        /// To Fetch PQ versionList, PQID and dealerId
        /// </summary>
        /// <param name="objVersion"></param>
        /// <param name="objModel"></param>
        public PQVersionListByCityAreaController(IBikeVersions<BikeVersionEntity, uint> objVersion, IBikeModelsRepository<BikeModelEntity, int> objModel, IBikeVersionCacheRepository<BikeVersionEntity, uint> objVersionCache)
        {
            _objVersion = objVersion;
            _objModel = objModel;
            _objVersionCache = objVersionCache;
        }

        /// <summary>
        /// Gets the Version list for given model and city
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <returns></returns>
        [ResponseType(typeof(PQByCityAreaEntity)), Route("api/model/versionlistprice/")]
        public IHttpActionResult Get(int modelId, int? cityId = null, int? areaId = null, string deviceId = null)
        {
            if (cityId < 0 || modelId < 0)
            {
                return BadRequest();
            }

            IEnumerable<BikeVersionMinSpecs> objVersionsList = null;
            PQByCityAreaDTO objPQDTO = null;
            PQByCityAreaEntity pqEntity = null;

            try
            {
                objVersionsList = _objVersionCache.GetVersionMinSpecs(Convert.ToUInt32(modelId), true);

                if (objVersionsList != null && objVersionsList.Count() > 0)
                {
                    PQByCityArea pqByCityArea = new PQByCityArea();
                    string platformId = string.Empty;
                    UInt16 platform = default(UInt16);
                    if (Request.Headers.Contains("platformId"))
                    {
                        platformId = Request.Headers.GetValues("platformId").First().ToString();
                    }
                    UInt16.TryParse(platformId, out platform);
                    pqEntity = pqByCityArea.GetVersionList(modelId, objVersionsList, cityId, areaId, platform, null, null, deviceId);
                    objPQDTO = ModelMapper.Convert(pqEntity);
                    objVersionsList = null;
                    return Ok(objPQDTO);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.Version.PQVersionListByCityAreaController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created By : Vivek Gupta 
        /// Date : 17-06-2016
        /// Desc : adding dealerpackage type, secondary dealer count and primary dealer offers
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [ResponseType(typeof(PQByCityAreaDTOV2)), Route("api/v2/model/versionlistprice/")]
        public IHttpActionResult GetV2(int modelId, int? cityId = null, int? areaId = null, string deviceId = null)
        {
            if (cityId < 0 || modelId < 0)
            {
                return BadRequest();
            }
            IEnumerable<BikeVersionMinSpecs> objVersionsList = null;
            PQByCityAreaDTOV2 objPQDTO = null;
            PQByCityAreaEntity pqEntity = null;

            objVersionsList = _objVersionCache.GetVersionMinSpecs(Convert.ToUInt32(modelId), true);

            try
            {
                if (objVersionsList != null && objVersionsList.Count() > 0)
                {
                    PQByCityArea pqByCityArea = new PQByCityArea();
                    string platformId = string.Empty;
                    UInt16 platform = default(UInt16);
                    if (Request.Headers.Contains("platformId"))
                    {
                        platformId = Request.Headers.GetValues("platformId").First().ToString();
                    }
                    UInt16.TryParse(platformId, out platform);
                    pqEntity = pqByCityArea.GetVersionListV2(modelId, objVersionsList, cityId, areaId, platform, null, null, deviceId);
                    objPQDTO = ModelMapper.ConvertV2(pqEntity);
                    objVersionsList = null;
                    return Ok(objPQDTO);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.Version.PQVersionListByCityAreaController.GetV2");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created By : Sangram Nandkhile 
        /// Date : 11 Oct 2017
        /// Desc : adding dealerpackage type, secondary dealer count and primary dealer offers
        /// </summary>
        /// <param name="modelId">The model identifier.</param>
        /// <param name="cityId">The city identifier.</param>
        /// <param name="areaId">The area identifier.</param>
        /// <param name="deviceId">The device identifier.</param>
        /// <returns></returns>
        [ResponseType(typeof(Bikewale.DTO.PriceQuote.Version.v3.PQByCityAreaDTO)), Route("api/v3/model/versionlistprice/")]
        public IHttpActionResult GetV3(int modelId, int? cityId = null, int? areaId = null, string deviceId = null)
        {
            if (cityId < 0 || modelId < 0)
            {
                return BadRequest();
            }
            IEnumerable<BikeVersionMinSpecs> objVersionsList = null;
            Bikewale.DTO.PriceQuote.Version.v3.PQByCityAreaDTO objPQDTO = null;
            PQByCityAreaEntity pqEntity = null;

            objVersionsList = _objVersionCache.GetVersionMinSpecs(Convert.ToUInt32(modelId), true);

            try
            {
                if (objVersionsList != null && objVersionsList.Count() > 0)
                {
                    PQByCityArea pqByCityArea = new PQByCityArea();
                    string platformId = string.Empty;
                    UInt16 platform = default(UInt16);
                    if (Request.Headers.Contains("platformId"))
                    {
                        platformId = Request.Headers.GetValues("platformId").First().ToString();
                    }
                    UInt16.TryParse(platformId, out platform);
                    pqEntity = pqByCityArea.GetVersionListV2(modelId, objVersionsList, cityId, areaId, platform, null, null, deviceId);
                    objPQDTO = ModelMapper.ConvertV3(pqEntity);
                    objVersionsList = null;
                    return Ok(objPQDTO);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.Version.PQVersionListByCityAreaController.GetV3");
                return InternalServerError();
            }
        }


    }
}
