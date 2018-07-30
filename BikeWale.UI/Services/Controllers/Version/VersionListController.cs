using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Version;
using Bikewale.Service.Utilities;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Version
{
    /// <summary>
    /// To Get List of Versions
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class VersionListController : CompressionApiController//ApiController
    {

        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _versionCacheRepo;

        public VersionListController(IBikeVersionCacheRepository<BikeVersionEntity,uint> versionRepo)
        {
            _versionCacheRepo = versionRepo;
        }

        #region List of Models Version with MinSpecs
        /// <summary>
        /// Versions List with minimum specs details
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="isNew"></param>
        /// <returns>Versions List</returns>
        [ResponseType(typeof(IEnumerable<VersionMinSpecs>))]
        public IHttpActionResult Get(uint modelId, bool isNew)
        {
            IEnumerable<VersionMinSpecs> objDTOMVSpecsList = null;
            try
            {
                IEnumerable<BikeVersionMinSpecs> objMVSpecsList = _versionCacheRepo.GetVersionMinSpecs(modelId, isNew);

                if (objMVSpecsList != null && objMVSpecsList.Any())
                {
                    objDTOMVSpecsList = VersionListMapper.Convert(objMVSpecsList);

                    objMVSpecsList = null;

                    return Ok(objDTOMVSpecsList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Version.VersionListController");
               
                return InternalServerError();
            }
            return NotFound();
        }   // Get 
        #endregion

        #region List of Models Version
        /// <summary>
        /// List of Versions based on models and requesttype
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns>Versions List</returns>
        [ResponseType(typeof(VersionList))]
        public IHttpActionResult Get(EnumBikeType requestType, int modelId, int? cityId = null)
        {
            List<BikeVersionsListEntity> objVersionList = null;
            VersionList objDTOVersionList = null;
            try
            {
                objVersionList = _versionCacheRepo.GetVersionsByType(requestType, modelId, cityId);

                if (objVersionList != null && objVersionList.Count > 0)
                {
                    objDTOVersionList = new VersionList();
                    objDTOVersionList.Version = VersionListMapper.Convert(objVersionList);

                    objVersionList.Clear();
                    objVersionList = null;

                    return Ok(objDTOVersionList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Version.VersionListController");
               
                return InternalServerError();
            }
            return NotFound();
        }   // Get 
        #endregion

    }
}
