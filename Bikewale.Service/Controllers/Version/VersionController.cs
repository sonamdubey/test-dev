using Bikewale.DTO.BikeData;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Version;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.Version
{
    /// <summary>
    /// To Get Version Details
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// Modified by :   Aditi Srivastava on 20 Oct 2016
    /// Description :   New api to get version colors by version id
    /// </summary>
    public class VersionController : CompressionApiController//ApiController
    {

        private readonly IBikeVersions<BikeVersionEntity, uint> _versionRepository = null;
        private readonly IBikeVersionCacheRepository<BikeVersionEntity, uint> _objVersionColorCache = null;
        public VersionController(IBikeVersions<BikeVersionEntity, uint> versionRepository, IBikeVersionCacheRepository<BikeVersionEntity, uint> objVersionColorCache)
        {
            _versionRepository = versionRepository;
            _objVersionColorCache = objVersionColorCache;
        }

        #region Version Details
        /// <summary>
        /// To get versions Details for Dropdowns
        /// Modified by :   Sumit Kate on 12 Apr 2016
        /// Description :   Send BadRequest if versionid less than 0
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns>Version Minimum Details</returns>
        [ResponseType(typeof(VersionDetails))]
        public IHttpActionResult Get(uint versionId)
        {
            BikeVersionEntity objVersion = null;
            VersionDetails objDTOVersionList = null;
            try
            {
                if (versionId > 0)
                {
                    objVersion = _objVersionColorCache.GetById(versionId);

                    if (objVersion != null)
                    {
                        // Auto map the properties
                        objDTOVersionList = new VersionDetails();
                        objDTOVersionList = VersionListMapper.Convert(objVersion);

                        return Ok(objDTOVersionList);
                    }
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Version.VersionController");
               
                return InternalServerError();
            }

            return NotFound();
        }   // Get 


        /// <summary>
        /// To get versions Details for Dropdowns
        /// Modified by :   Sumit Kate on 12 Apr 2016
        /// Description :   Send BadRequest if versionid <= 0
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns>Version Minimum Details</returns>
        [ResponseType(typeof(Bikewale.DTO.Version.v2.VersionDetails)), Route("api/v2/Version/")]
        public IHttpActionResult GetV2(uint versionId)
        {
            BikeVersionEntity objVersion = null;
            Bikewale.DTO.Version.v2.VersionDetails objDTOVersionList = null;
            try
            {
                if (versionId > 0)
                {
                    objVersion = _objVersionColorCache.GetById(versionId);

                    if (objVersion != null)
                    {
                        // Auto map the properties
                        objDTOVersionList = new Bikewale.DTO.Version.v2.VersionDetails();
                        objDTOVersionList = VersionListMapper.ConvertV2(objVersion);

                        return Ok(objDTOVersionList);
                    }
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Version.VersionController");
               
                return InternalServerError();
            }

            return NotFound();
        }   // Get 
        #endregion

        /// <summary>
        /// Created By: Aditi Srivastava on 17 Oct 2016
        /// Summary: Get version colors by version id
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>
        [ResponseType(typeof(BikeColorsbyVersionDTO)), Route("api/version/{versionId}/color/")]
        public IHttpActionResult GetVersionColor(uint versionId)
        {
            IEnumerable<Bikewale.Entities.BikeData.BikeColorsbyVersion> objVersionColors = null;
            BikeColorsbyVersionDTO objDTOVersionColors = new BikeColorsbyVersionDTO();
            try
            {
                objVersionColors = _objVersionColorCache.GetColorsbyVersionId(versionId);

                if (objVersionColors != null)
                {
                    objDTOVersionColors.VersionColors = VersionListMapper.Convert(objVersionColors);

                    return Ok(objDTOVersionColors);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Exception : Bikewale.Service.Version.VersionController.GetVersionColor(VersionId={0}", versionId));
               
                return InternalServerError();
            }
            return NotFound();
        }

    }
}
