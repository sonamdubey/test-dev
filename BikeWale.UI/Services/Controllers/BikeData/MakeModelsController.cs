﻿using System.Collections.Generic;
using System.Web.Http;
using Bikewale.DTO.BikeData;
using Bikewale.DTO.Widgets;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.BikeData;
using Bikewale.Service.Utilities;
using Bikewale.BAL.BikeData;
using Microsoft.Practices.Unity;
using System.Web.Http.Cors;
using System.Configuration;
using Bikewale.Utility;
using System.Web;
using System.Linq;

namespace Bikewale.Service.Controllers.BikeData
{
    /// <summary>
    /// Created by  :   Sumit Kate on 13 Sep 2016
    /// Description :   Make Models Controller
    /// </summary>
    public class MakeModelsController : CompressionApiController
    {
        private readonly IBikeMakesCacheRepository _makesCacheRepository;
        private readonly IBikeModels<BikeModelEntity, int> _bikeModels;
        private static HashSet<string> _ampCors = new HashSet<string> { "https://www-bikewale-com.cdn.ampproject.org", "https://www-bikewale-com.amp.cloudflare.com", "https://cdn.ampproject.org","https://www.bikewale.com" };
        
        /// <summary>
        /// Constructor to Initialize cache layer
        /// </summary>
        /// <param name="makesCacheRepository"></param>
        /// <param name="bikeModels"></param>
        public MakeModelsController(IBikeMakesCacheRepository makesCacheRepository, IBikeModels<BikeModelEntity, int> bikeModels)
        {
            _makesCacheRepository = makesCacheRepository;
            _bikeModels = bikeModels;
            
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 13 Sep 2016
        /// Description :   Gets All the makes and their models by calling cache layer
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult Get()
        {
            IEnumerable<MakeModelBase> makeModels = null;
            try
            {
                makeModels = MakeModelEntityMapper.Convert(_makesCacheRepository.GetAllMakeModels());
            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, "MakeModelsController.Get");
               
                InternalServerError();
            }
            return Ok(makeModels);
        }
         
        [HttpGet, Route("api/popularbikesbybodystyle/{modelId}/{topCount}")]
        public IHttpActionResult GetPopularBikesByBodyStyle(uint modelId, uint topCount, uint cityId)
        {
            IEnumerable<MostPopularBikes> makeModels = null;
            try
            {
                IEnumerable<MostPopularBikesBase> bikeModelEntity = _bikeModels.GetMostPopularBikesByModelBodyStyle((int)modelId, (int)topCount, cityId, false);
                makeModels = MakeModelEntityMapper.ConvertWithoutMinSpec(bikeModelEntity);
            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, "MakeModelsController.GetPopularBikesByBodyStyle");               
                InternalServerError();
            }
            return Ok(makeModels);
        }


        [HttpGet, Route("api/popularbikesbymake/{makeId}/{topCount}")]
        public IHttpActionResult GetPopularBikesByMake(uint makeId, uint topCount, uint cityId)
        {
            IEnumerable<MostPopularBikes> makeModels = null;
            try
            {
                IEnumerable<MostPopularBikesBase> bikeModelEntity = _bikeModels.GetMostPopularBikesbyMakeCity(topCount, makeId, cityId);
                makeModels = MakeModelEntityMapper.Convert(bikeModelEntity);
            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, "MakeModelsController.GetPopularBikesByMake");                
                InternalServerError();
            }
            return Ok(makeModels);
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 2nd Jan 2018
        /// Summary    : Get popular bikes 
        /// </summary>
        /// <returns></returns>

        [HttpGet, Route("api/popularbikes/"), EnableCors("https://www-bikewale-com.cdn.ampproject.org, https://www-bikewale-com.amp.cloudflare.com, https://cdn.ampproject.org,https://www.bikewale.com", "*", "GET")]
        public IHttpActionResult GetMostPopularBikes(int topCount)
        {
            string ampSourceOrigin = HttpUtility.ParseQueryString(Request.RequestUri.Query)["__amp_source_origin"];
            string ampOrigin = Request.Headers.Contains("Origin") ? Request.Headers.GetValues("Origin").FirstOrDefault() : string.Empty;

            if (!string.IsNullOrEmpty(ampSourceOrigin) && _ampCors.Contains(ampOrigin))
            {
                BWCookies.AddAmpHeaders(ampSourceOrigin,ampOrigin, true);
            }
            IEnumerable<MostPopularBikes> makeModels = null;
            try
            {
                IEnumerable<MostPopularBikesBase> bikeModelEntity = _bikeModels.GetMostPopularBikes(topCount);
                makeModels = MakeModelEntityMapper.Convert(bikeModelEntity);
            }
            catch (System.Exception ex)
            {
                ErrorClass.LogError(ex, "MakeModelsController.GetPopularBikes");
                InternalServerError();
            }
            return Ok(makeModels);
        }
    }
}
