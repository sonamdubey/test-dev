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
        #region Version Details
        /// <summary>
        /// To get versions Details for DropDowns
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns>Version Minimum Details</returns>
        [ResponseType(typeof(VersionDetails))]
        public HttpResponseMessage Get(uint versionId)
        {
            BikeVersionEntity objVersion = null;
            VersionDetails objDTOVersionList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    IBikeVersions<BikeVersionEntity, uint> versionRepository = null;

                    container.RegisterType<IBikeVersions<BikeVersionEntity, uint>, BikeVersionsRepository<BikeVersionEntity, uint>>();
                    versionRepository = container.Resolve<IBikeVersions<BikeVersionEntity, uint>>();

                    objVersion = versionRepository.GetById(versionId);

                    if (objVersion != null)
                    {
                        // Auto map the properties
                        objDTOVersionList = new VersionDetails();  
                        objDTOVersionList = VersionEntityToDTO.ConvertVersionEntity(objVersion);

                        return Request.CreateResponse(HttpStatusCode.OK, objVersion);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Version.VersionController");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "OOps ! Some error occured.");
            }
        }   // Get 
        #endregion

        #region Versions Specifications
        /// <summary>
        ///  To Get Version's Specifications  adn 
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [ResponseType(typeof(VersionSpecifications))]
        public HttpResponseMessage Get(uint versionId, bool? specs)
        {
            BikeSpecificationEntity objSpecs = null;
            VersionSpecifications objDTOVersionList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    IBikeVersions<BikeVersionEntity, uint> versionRepository = null;

                    container.RegisterType<IBikeVersions<BikeVersionEntity, uint>, BikeVersionsRepository<BikeVersionEntity, uint>>();
                    versionRepository = container.Resolve<IBikeVersions<BikeVersionEntity, uint>>();

                    objSpecs = versionRepository.GetSpecifications(versionId);

                    if (objSpecs != null)
                    {
                        // Auto map the properties
                        objDTOVersionList = new VersionSpecifications();
                        objDTOVersionList = VersionEntityToDTO.ConvertSpecificationEntity(objSpecs);

                        return Request.CreateResponse(HttpStatusCode.OK, objSpecs);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Version.VersionController");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "OOps ! Some error occured.");
            }
        }   // Get  Versions Specifications
        #endregion

    }
}
