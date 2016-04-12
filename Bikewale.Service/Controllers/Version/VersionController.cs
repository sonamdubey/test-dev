using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.DAL.BikeData;
using AutoMapper;
using System.Web.Http.Description;
using Bikewale.DTO.Version;
using Bikewale.Service.AutoMappers.Version;
using Bikewale.Notifications;

namespace Bikewale.Service.Controllers.Version
{
    /// <summary>
    /// To Get Version Details
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// </summary>
    public class VersionController : ApiController
    {
        
        private readonly  IBikeVersions<BikeVersionEntity, uint> _versionRepository = null;
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
