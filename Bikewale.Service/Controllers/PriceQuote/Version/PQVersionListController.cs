﻿using Bikewale.DTO.PriceQuote.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote.Version;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote.Version
{
    /// <summary>
    /// Price Quote Version List controller
    /// Author  :   Sumit Kate
    /// Created On  :   20 Aug 2015
    /// </summary>
    public class PQVersionListController : ApiController
    {
        private readonly IBikeVersionCacheRepository<BikeVersionEntity,uint> _objVersion = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objVersion"></param>
        public PQVersionListController(IBikeVersionCacheRepository<BikeVersionEntity,uint> objVersion)
        {
            _objVersion = objVersion;
        }
        /// <summary>
        /// Gets the Version list for given model and city
        /// </summary>
        /// <param name="modelId">model Id</param>
        /// <param name="cityId">city id</param>
        /// <returns></returns>
        [ResponseType(typeof(PQVersionList))]
        public IHttpActionResult Get(uint modelId, int? cityId = null)
        {
            List<BikeVersionsListEntity> objVersionList = null;
            PQVersionList objDTOVersionList = null;
            try
            {
                objVersionList = _objVersion.GetVersionsByType(EnumBikeType.PriceQuote, Convert.ToInt32(modelId), cityId);

                if (objVersionList != null && objVersionList.Count > 0)
                {
                    // Auto map the properties
                    objDTOVersionList = new PQVersionList();
                    objDTOVersionList.Versions = PQVersionListMapper.Convert(objVersionList);

                    objVersionList.Clear();
                    objVersionList = null;

                    return Ok(objDTOVersionList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.Version.PQVersionListController.Get");
               
                return InternalServerError();
            }
        }
    }
}
