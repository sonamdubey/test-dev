﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Bikewale.Notifications;
using BikewaleOpr.DTO.BikeData;
using BikewaleOpr.Entities.BikeData;
using BikewaleOpr.Interface.BikeData;
using BikewaleOpr.Service.AutoMappers.BikeData;
using BikewaleOpr.Entity.BikeData;

namespace BikewaleOpr.Service.Controllers.Content
{
    public class ModelsController : ApiController
    {
        private readonly IBikeModelsRepository _modelsRepo = null;

        public ModelsController(IBikeModelsRepository modelsRepo)
        {
            _modelsRepo = modelsRepo;
        }

        /// <summary>
        /// Written By : Ashish G. Kamble on 18n Apr 2017
        /// Summary : API to get the bikemodels list for the given make id and request type
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="requestType">
        /// 1 PRICEQUOTE
        /// 2 NEW
        /// 3 USED
        /// 4 UPCOMING
        /// 5 ROADTEST
        /// 6 COMPARISONTEST
        /// 7 ALL
        /// 8 user reviews
        /// </param>
        /// <returns>API return list of bikemodels</returns>
        [HttpGet, Route("api/models/makeid/{makeId}/requestType/{requestType}")]
        public IHttpActionResult GetModels(uint makeId, ushort requestType)
        {
            try
            {
                if (makeId > 0 && requestType > 0)
                {
                    IEnumerable<BikeModelEntityBase> objModels = _modelsRepo.GetModels(makeId, requestType);

                    if (objModels != null && objModels.Any())
                    {
                        IEnumerable<ModelBase> objModelsDTO = BikeModelsMapper.Convert(objModels);

                        return Ok(objModelsDTO);
                    }
                    else {
                        return Ok();
                    }                    
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Service.Controllers.Content.GetModels");
                return InternalServerError();
            }
        }   // End of GetModels

        /// <summary>
        /// Created by: Vivek Singh Tomar on 7th Aug 2017
        /// Summary: Api to versions for given model id.
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [HttpGet, Route("api/models/{modelid}/versions/{requesttype}/")]
        public IHttpActionResult GetVersions(EnumBikeType requestType, uint modelId)
        {
            IEnumerable<VersionBase> objBikeVersionBaseList = null;
            if(modelId > 0)
            {
                try
                {
                    IEnumerable<BikeVersionEntityBase> objBikeVersionEntityBaseList = _modelsRepo.GetVersionsByModel(requestType, modelId);
                    objBikeVersionBaseList = BikeVersionsMapper.Convert(objBikeVersionEntityBaseList);
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "BikewaleOpr.Service.Controllers.Content.ModelsController.GetVersions");
                    return InternalServerError();
                }
            }
            else
            {
                return BadRequest("Invalid Model Id");
            }
            return Ok(objBikeVersionBaseList);
        }
    }   // class
}   // namespace
