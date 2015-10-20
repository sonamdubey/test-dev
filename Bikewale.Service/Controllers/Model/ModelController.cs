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
using Bikewale.DTO.Model;
using Bikewale.DTO.Series;
using Bikewale.DTO.Make;
using Bikewale.DTO.Version;
using Bikewale.Service.Controllers.Version;
using Bikewale.Service.AutoMappers.Model;
using Bikewale.Notifications;
using Bikewale.Entities.CMS.Photos;
using Bikewale.Entities.CMS;
using Bikewale.Utility;
using System.Configuration;
using Bikewale.DTO.CMS.Photos;
using Bikewale.Service.AutoMappers.CMS;

namespace Bikewale.Service.Controllers.Model
{
    /// <summary>
    /// To Get Model Details and other required Information related to the Models
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// </summary>
    public class ModelController : ApiController
    {
        private string _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
        private string _applicationid = ConfigurationManager.AppSettings["applicationId"];
        private string _requestType = "application/json";
        private readonly IBikeModelsRepository<BikeModelEntity, int> _modelRepository = null;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelRepository"></param>
        public ModelController(IBikeModelsRepository<BikeModelEntity, int> modelRepository)
        {
            _modelRepository = modelRepository;
        }

        #region Model Details
        /// <summary>
        ///  To get Required Model Details 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Required Model Details </returns>
        [ResponseType(typeof(ModelDetails))]
        public IHttpActionResult Get(int modelId)
        {
            BikeModelEntity objModel = null;
            ModelDetails objDTOModel = null;
            try
            {
                objModel = _modelRepository.GetById(modelId);

                if (objModel != null)
                {
                    // Auto map the properties
                    objDTOModel = new ModelDetails();
                    objDTOModel = ModelMapper.Convert(objModel);
                    return Ok(objDTOModel);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Model.ModelController");
                objErr.SendMail();
                return InternalServerError();
            }

            return NotFound();
        }   // Get 
        #endregion

        #region Model Versions List        
        /// <summary>
        /// To get all new versions based on Models 
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="isNew"></param>
        /// <returns>List of Model's New Versions</returns>
        [ResponseType(typeof(List<ModelVersionList>)), Route("api/model/versions/")]
        public IHttpActionResult Get(int modelId, bool isNew)
        {
            List<BikeVersionsListEntity> mvEntityList = null;
            List<ModelVersionList> mvList = null;
            try
            {
                mvEntityList = _modelRepository.GetVersionsList(modelId, isNew);

                if (mvEntityList != null && mvEntityList.Count > 0)
                {
                    mvList = new List<ModelVersionList>();
                    mvList = ModelMapper.Convert(mvEntityList);

                    mvEntityList.Clear();
                    mvEntityList = null;

                    return Ok(mvList);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Model.ModelController");
                objErr.SendMail();
                return InternalServerError();
            }
        }   // Get 
        #endregion               
    }

}
