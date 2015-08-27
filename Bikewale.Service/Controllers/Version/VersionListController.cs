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
    /// To Get List of Versions
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// </summary>
    public class VersionListController : ApiController
    {                                 
        #region List of Models Version with MinSpecs
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="isNew"></param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<VersionMinSpecs>))]
        public HttpResponseMessage Get(uint modelId, bool isNew)
        {
            List<BikeVersionMinSpecs> objMVSpecsList = null;
            List<VersionMinSpecs> objDTOMVSpecsList = null;
            using (IUnityContainer container = new UnityContainer())
            {
                IBikeVersions<BikeVersionEntity, int> versionRepository = null;

                container.RegisterType<IBikeVersions<BikeVersionEntity, int>, BikeVersionsRepository<BikeVersionEntity, int>>();
                versionRepository = container.Resolve<IBikeVersions<BikeVersionEntity, int>>();

                objMVSpecsList = versionRepository.GetVersionMinSpecs(modelId, isNew);

                if (objMVSpecsList != null && objMVSpecsList.Count > 0)
                {
                    objDTOMVSpecsList = new   List<VersionMinSpecs>();  
                    objDTOMVSpecsList = VersionEntityToDTO.ConvertVersionMinSpecsList(objMVSpecsList);
                    return Request.CreateResponse(HttpStatusCode.OK, objDTOMVSpecsList);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                }
            }
        }   // Get 
        #endregion
        
        #region List of Models Version
        /// <summary>
        /// 
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="makeId"></param>
        /// <returns></returns>
        [ResponseType(typeof(VersionList))]
        public HttpResponseMessage Get(EnumBikeType requestType, int modelId, int? cityId = null)
        {
            List<BikeVersionsListEntity> objVersionList = null;
            VersionList objDTOVersionList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    IBikeVersions<BikeVersionEntity, int> versionRepository = null;

                    container.RegisterType<IBikeVersions<BikeVersionEntity, int>, BikeVersionsRepository<BikeVersionEntity, int>>();
                    versionRepository = container.Resolve<IBikeVersions<BikeVersionEntity, int>>();

                    objVersionList = versionRepository.GetVersionsByType(requestType, modelId, cityId);

                    if (objVersionList != null && objVersionList.Count > 0)
                    {
                        objDTOVersionList = new VersionList();
                        objDTOVersionList.Version = VersionEntityToDTO.ConvertVersionsListEntityList(objVersionList);
                        return Request.CreateResponse(HttpStatusCode.OK, objDTOVersionList);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Version.VersionListController");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "OOps ! Some error occured.");
            }
        }   // Get 
        #endregion                 

    }
}
