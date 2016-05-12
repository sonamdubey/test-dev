using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Model;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Description;

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
        private readonly IBikeModelsRepository<BikeModelEntity, int> _modelRepository = null;
        private readonly IBikeModels<BikeModelEntity, int> _modelsContent = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelRepository"></param>
        /// <param name="modelContent"></param>
        public ModelController(IBikeModelsRepository<BikeModelEntity, int> modelRepository, IBikeModels<BikeModelEntity, int> modelContent)
        {
            _modelRepository = modelRepository;
            _modelsContent = modelContent;

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

        #region Model Content Data
        /// <summary>
        /// Author : Vivek Gupta
        /// Date : 5-5-2016
        /// Desc : wrapping data in a single api for - User Reviews - News- Expert Reviews- Videos
        /// </summary>
        /// <param name="modelId"></param>        
        /// <returns>List of User Reviews - News- Expert Reviews- Videos of the model</returns>
        [ResponseType(typeof(BikeModelContentDTO)), Route("api/model/articles/")]
        public IHttpActionResult GetModelContent(int modelId)
        {
            BikeModelContent bkModelContent = null;
            BikeModelContentDTO bkContent = null;

            try
            {
                if (modelId > 0)
                {
                    bkModelContent = _modelsContent.GetRecentModelArticles(modelId);
                }

                else
                {
                    return BadRequest();
                }

                if (bkModelContent != null)
                {
                    bkContent = new BikeModelContentDTO();
                    bkContent = ModelMapper.Convert(bkModelContent);
                    
                    bkModelContent = null;
                    bkContent.News = new CMSShareUrl().GetShareUrl(bkContent.News);
                    bkContent.ExpertReviews = new CMSShareUrl().GetShareUrl(bkContent.ExpertReviews);

                    bkModelContent = null;

                    return Ok(bkContent);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Model.ModelController.GetModelContent");
                objErr.SendMail();
                return InternalServerError();
            }
        }
        #endregion  Model Content Data

    }

}
