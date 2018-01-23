using Bikewale.DTO.Model;
using Bikewale.DTO.Version;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Model;
using Bikewale.Service.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;
namespace Bikewale.Service.Controllers.Model
{
    /// <summary>
    /// To Get Model Details and other required Information related to the Models
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class ModelController : CompressionApiController//ApiController
    {
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Model.ModelController");

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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Model.ModelController");

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
                    bkContent = ModelMapper.Convert(bkModelContent);

                    return Ok(bkContent);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Model.ModelController.GetModelContent");
                return InternalServerError();
            }
        }
        #endregion  Model Content Data


        #region Model Content Data
        /// <summary>
        /// Created By : Sushil Kumar on 6th September 2017
        /// Desc : Bring data in a single api for - User Reviews - News- Expert Reviews- Videos as per new model screen
        /// </summary>
        /// <param name="modelId"></param>        
        /// <returns>List of User Reviews - News- Expert Reviews- Videos of the model</returns>
        [ResponseType(typeof(Bikewale.DTO.Model.v2.BikeModelContentDTO)), Route("api/v2/model/{modelId}/articles/")]
        public IHttpActionResult GetModelContentV2(int modelId)
        {


            try
            {
                if (modelId > 0)
                {
                    Bikewale.DTO.Model.v2.BikeModelContentDTO bkContent = null;
                    Bikewale.Entities.BikeData.v2.BikeModelContent bkModelContent = _modelsContent.GetRecentModelArticlesv2(modelId);

                    if (bkModelContent != null)
                    {
                        bkContent = ModelMapper.ConvertV2(bkModelContent);

                        return Ok(bkContent);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Exception : Bikewale.Service.Model.ModelController.GetModelContent({0})", modelId));
                return InternalServerError();
            }
        }
        #endregion  Model Content Data

        /// <summary>
        /// Created By: Sangram Nandkhile on 31 Jan 2017
        /// Summary: To return Model Images, Other model Images and color wise images
        /// Modified By : Sajal Gupta on 28-02-2017
        /// Description : Call function of BAL Layer instead of Cache layer.       
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [ResponseType(typeof(IList<ColorImageBaseDTO>)), Route("api/model/{modelId}/photos/")]
        public IHttpActionResult GetModelColorPhotos(int modelId)
        {
            IEnumerable<ColorImageBaseDTO> allPhotos = null;
            IEnumerable<Bikewale.Entities.CMS.Photos.ColorImageBaseEntity> imageList = null;
            try
            {
                if (modelId > 0)
                {
                    imageList = _modelsContent.CreateAllPhotoList(modelId);
                    allPhotos = ModelMapper.Convert(imageList);
                }
                else
                {
                    return BadRequest();
                }

                if (allPhotos != null)
                {
                    return Ok(allPhotos);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Model.ModelController.GetModelColorPhotos");
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 05th Oct 2017
        /// Summary : Get available gallery components details
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [ResponseType(typeof(ModelGallery)), Route("api/model/{modelId}/gallery/")]
        public IHttpActionResult GetGalleryByModelId(int modelId)
        {
            ModelGallery objGallery = null;
            try
            {
                if (modelId <= 0)
                {
                    return BadRequest();
                }
                if (Request.Headers.Contains("platformId"))
                {
                    var platformId = Request.Headers.GetValues("platformId").First();
                    if (platformId != null && platformId.ToString().Equals("3"))
                    {
                        BikeModelPageEntity objModelPage = null;
                        objModelPage = _modelsContent.GetModelPageDetails(modelId);
                        objGallery = ModelMapper.ConvertToModelGallery(objModelPage, modelId);
                        return Ok(objGallery);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Service.Controllers.GalleryController: GalleryComponents, modelid = {0}", modelId));

                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by : Vivek Singh Tomar on 5th Oct 2017
        /// Summary : Get color photos for modelid
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<ModelColorPhoto>)), Route("api/model/{modelId}/colorphotos/")]
        public IHttpActionResult GetColorPhotosByModelId(int modelId)
        {
            IEnumerable<ModelColorPhoto> objAllPhotos = null;
            try
            {
                if (modelId <= 0)
                {
                    return BadRequest();
                }
                if (Request.Headers.Contains("platformId"))
                {
                    var platformId = Request.Headers.GetValues("platformId").First();
                    if (platformId != null && platformId.ToString().Equals("3"))
                    {
                        IEnumerable<ModelColorImage> objAllPhotosEntity = _modelsContent.GetModelColorPhotos(modelId).Where( modelColorPhoto=> modelColorPhoto.IsImageExists == true);
                        objAllPhotos = ModelMapper.Convert(objAllPhotosEntity);
                        return Ok(objAllPhotos);
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Service.Controllers.GalleryController: GetColorPhotosByModelId, modelid = {0}", modelId));

                return InternalServerError();
            }
        }

        /// <summary>
        /// Gets the colors by model identifier.
        /// </summary>
        /// <param name="modelId">The model identifier.</param>
        /// <returns></returns>
        [ResponseType(typeof(IEnumerable<ModelColorPhoto>)), Route("api/model/{modelId}/colors/")]
        public IHttpActionResult GetColorsByModelId(int modelId)
        {
            IEnumerable<ModelColorPhoto> objAllPhotos = null;
            try
            {
                if (modelId <= 0)
                {
                    return BadRequest();
                }

                IEnumerable<ModelColorImage> objAllPhotosEntity = _modelsContent.GetModelColorPhotos(modelId);
                if (objAllPhotosEntity != null)
                {
                    objAllPhotos = ModelMapper.Convert(objAllPhotosEntity);
                    return Ok(objAllPhotos);
                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("Bikewale.Service.Controllers.GetColorsByModelId:  modelid = {0}", modelId));

                return InternalServerError();
            }
        }
    }
}
