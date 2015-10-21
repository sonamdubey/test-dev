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
        
        private readonly  IBikeVersions<BikeVersionEntity, uint> _versionRepository = null;
        public VersionListController(IBikeVersions<BikeVersionEntity, uint> versionRepository)
        {
            _versionRepository = versionRepository;
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
            List<BikeVersionMinSpecs> objMVSpecsList = null;
            List<VersionMinSpecs> objDTOMVSpecsList = null;
            try
            {
                objMVSpecsList = _versionRepository.GetVersionMinSpecs(modelId, isNew);

                if (objMVSpecsList != null && objMVSpecsList.Count > 0)
                {
                    objDTOMVSpecsList = new List<VersionMinSpecs>();
                    objDTOMVSpecsList = VersionListMapper.Convert(objMVSpecsList);

                    objMVSpecsList.Clear();
                    objMVSpecsList = null;

                    return Ok(objDTOMVSpecsList);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Version.VersionListController");
                objErr.SendMail();
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
                objVersionList = _versionRepository.GetVersionsByType(requestType, modelId, cityId);

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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Version.VersionListController");
                objErr.SendMail();
                return InternalServerError();
            }
            return NotFound();
        }   // Get 
        #endregion                 

    }
}
