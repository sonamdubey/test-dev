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

        #region Model Description
        /// <summary>
        ///  To get Long/Short Model's Description (Synopsis) 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Model Synopsis</returns>
        [ResponseType(typeof(ModelDescription))]
        public IHttpActionResult Get(int modelId, bool? description)
        {
            BikeDescriptionEntity objModelDesc = null;
            ModelDescription objDTOModelDesc = null;
            try
            {

                objModelDesc = _modelRepository.GetModelSynopsis(modelId);

                if (objModelDesc != null)
                {
                    // Auto map the properties
                    objDTOModelDesc = new ModelDescription();
                    objDTOModelDesc = ModelMapper.Convert(objModelDesc);
                    return Ok(objDTOModelDesc);
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

        #region Model Versions List
        /// <summary>
        /// To get all new versions based on Models 
        /// </summary>
        /// <param name="requestType"></param>
        /// <param name="modelId"></param>
        /// <returns>List of Model's New Versions </returns>
        [ResponseType(typeof(List<ModelVersionList>))]
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

        #region Model Specifications and Features
        /// <summary>
        ///  To get Model's Specifications and Features based on VersionId 
        /// </summary>
        /// <param name="versionId">Model's Version ID</param>
        /// <param name="specs"></param>
        /// <param name="features"></param>
        /// <returns>Model's Specs and Features</returns>
        [ResponseType(typeof(VersionSpecifications))]
        public IHttpActionResult Get(int versionId, bool? specs, bool? features)
        {
            BikeSpecificationEntity objVersionSpecs = null;
            VersionSpecifications objDTOVersionSpecs = null;
            try
            {
                objVersionSpecs = _modelRepository.MVSpecsFeatures(versionId);

                if (objVersionSpecs != null)
                {
                    // Auto map the properties
                    objDTOVersionSpecs = new VersionSpecifications();
                    objDTOVersionSpecs = ModelMapper.Convert(objVersionSpecs);
                    return Ok(objDTOVersionSpecs);
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

        }   // Get  Specs and Features
        #endregion

        #region Model Page Complete
        /// <summary>
        /// To get complete Model Page with Specs,Features,description and Details
        /// For the Specs and Features Default version selected is the one with maximum pricequotes
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Complete Model Page</returns>
        [ResponseType(typeof(ModelPage))]
        public IHttpActionResult Get(int modelId, bool isNew, bool? specs, bool? features)
        {
            BikeModelPageEntity objModelPage = null;
            ModelPage objDTOModelPage = null;
            List<EnumCMSContentType> categorList = null;
            List<ModelImage> objImageList = null;
            try
            {
                objModelPage = _modelRepository.GetModelPage(modelId, isNew);

                if (objModelPage != null)
                {
                    // Auto map the properties
                    objDTOModelPage = new ModelPage();
                    objDTOModelPage = ModelMapper.Convert(objModelPage);

                    categorList = new List<EnumCMSContentType>();
                    categorList.Add(EnumCMSContentType.PhotoGalleries);                    
                    string contentTypeList = CommonApiOpn.GetContentTypesString(categorList);
                    string _apiUrl = String.Format("webapi/image/modelphotolist/?applicationid={0}&modelid={1}&categoryidlist={2}", _applicationid, modelId, contentTypeList);
                    
                    objImageList = BWHttpClient.GetApiResponseSync<List<ModelImage>>(_cwHostUrl, _requestType, _apiUrl, objImageList);
                    if (objImageList != null && objImageList.Count > 0)
                    {
                        // Auto map the properties
                        List<CMSModelImageBase> objCMSModels = new List<CMSModelImageBase>();
                        objCMSModels = CMSMapper.Convert(objImageList);
                        objDTOModelPage.Photos = objCMSModels;
                    }
                    return Ok(objDTOModelPage);
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
        }   // Get  Model Page
        #endregion


    }

}
