using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Version;
using Bikewale.Service.Utilities;
using System;
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
    /// </summary>
    public class VersionController : CompressionApiController//ApiController
    {

        private readonly IBikeVersions<BikeVersionEntity, uint> _versionRepository = null;
        public VersionController(IBikeVersions<BikeVersionEntity, uint> versionRepository)
        {
            _versionRepository = versionRepository;
        }

        #region Version Details
        /// <summary>
        /// To get versions Details for Dropdowns
        /// Modified by :   Sumit Kate on 12 Apr 2016
        /// Description :   Send BadRequest if versionid <= 0
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
                    objVersion = _versionRepository.GetById(versionId);

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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Version.VersionController");
                objErr.SendMail();
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
        [ResponseType(typeof(VersionDetails)), Route("api/v2/Version/")]
        public IHttpActionResult GetV2(uint versionId)
        {
            BikeVersionEntity objVersion = null;
            Bikewale.DTO.Version.v2.VersionDetails objDTOVersionList = null;
            try
            {
                if (versionId > 0)
                {
                    objVersion = _versionRepository.GetById(versionId);

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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Version.VersionController");
                objErr.SendMail();
                return InternalServerError();
            }

            return NotFound();
        }   // Get 
        #endregion

        #region Versions Specifications
        /// <summary>
        ///  To Get Version's Specifications  and Features 
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [ResponseType(typeof(VersionSpecifications))]
        public IHttpActionResult Get(uint versionId, bool? specs)
        {
            BikeSpecificationEntity objSpecs = null;
            VersionSpecifications objDTOVersionList = null;
            try
            {
                objSpecs = _versionRepository.GetSpecifications(versionId);

                if (objSpecs != null)
                {
                    // Auto map the properties
                    objDTOVersionList = new VersionSpecifications();
                    objDTOVersionList = VersionListMapper.Convert(objSpecs);

                    return Ok(objSpecs);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Version.VersionController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }   // Get  Versions Specifications
        #endregion

    }
}
