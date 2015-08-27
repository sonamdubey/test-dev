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

namespace Bikewale.Service.Controllers.Model
{
    /// <summary>
    /// To Get Model Details and other required Information related to the Models
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// </summary>
    public class ModelController : ApiController
    {

        #region Model Details 
        /// <summary>
        ///  To get Required Model Details 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Required Model Details </returns>
        [ResponseType(typeof(ModelDetails))]
        public HttpResponseMessage Get(int modelId)
        {
            BikeModelEntity objModel = null;
            ModelDetails objDTOModel = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    IBikeModelsRepository<BikeModelEntity, int> modelRepository = null;

                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                    modelRepository = container.Resolve<IBikeModelsRepository<BikeModelEntity, int>>();

                    objModel = modelRepository.GetById(modelId);

                    if (objModel != null)
                    {
                        // Auto map the properties
                        objDTOModel = new ModelDetails();

                        //Mapper.CreateMap<BikeModelEntityBase, ModelBase>();
                        //Mapper.CreateMap<BikeMakeEntityBase, MakeBase>();
                        //Mapper.CreateMap<BikeModelEntity, ModelDetails>();
                        //Mapper.CreateMap<BikeSeriesEntityBase, SeriesBase>();
                        //objDTOModel = Mapper.Map<BikeModelEntity, ModelDetails>(objModel);
                        objDTOModel = ModelEntityToDTO.ConvertModelEntity(objModel);
                        return Request.CreateResponse(HttpStatusCode.OK, objDTOModel);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Model.ModelController");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "OOps ! Some error occured.");
            }
        }   // Get 
        #endregion

        #region Model Description
        /// <summary>
        ///  To get Long/Short Model's Description (Synopsis) 
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Model Synopsis</returns>
        [ResponseType(typeof(ModelDescription))]
        public HttpResponseMessage Get(int modelId,bool? description)
        {
            BikeDescriptionEntity objModelDesc = null;
            ModelDescription objDTOModelDesc = null;
               try
                {
                   using (IUnityContainer container = new UnityContainer())
                    {
                        IBikeModelsRepository<BikeModelEntity, int> modelRepository = null;

                        container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                        modelRepository = container.Resolve<IBikeModelsRepository<BikeModelEntity, int>>();

                        objModelDesc = modelRepository.GetModelSynopsis(modelId);

                        if (objModelDesc != null)
                        {
                            // Auto map the properties
                            objDTOModelDesc = new ModelDescription();
                            objDTOModelDesc = ModelEntityToDTO.ConvertModelDescription(objModelDesc);
                            return Request.CreateResponse(HttpStatusCode.OK, objDTOModelDesc);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Model.ModelController");
                    objErr.SendMail();
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "OOps ! Some error occured.");
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
        public HttpResponseMessage Get(int modelId, bool isNew)
        {
            List<BikeVersionsListEntity> mvEntityList = null;
            List<ModelVersionList> mvList = null;
           try
                {
                    using (IUnityContainer container = new UnityContainer())
                    {
                        IBikeModelsRepository<BikeModelEntity, int> modelRepository = null; 
                        
                        container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                        modelRepository = container.Resolve<IBikeModelsRepository<BikeModelEntity, int>>();
                       
                        mvEntityList = modelRepository.GetVersionsList(modelId, isNew);    
                        
                        if (mvEntityList != null && mvEntityList.Count > 0)
                        {
                            mvList = new List<ModelVersionList>();
                            mvList = ModelEntityToDTO.ConvertVersionList(mvEntityList);
                            return Request.CreateResponse(HttpStatusCode.OK, mvList);
                        }
                        else
                        {
                            return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                        }
                    }
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Model.ModelController");
                    objErr.SendMail();
                    return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "OOps ! Some error occured.");
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
        public HttpResponseMessage Get(int versionId,bool? specs,bool? features)
        {
            BikeSpecificationEntity objVersionSpecs = null;
            VersionSpecifications objDTOVersionSpecs = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    IBikeModelsRepository<BikeModelEntity, int> modelRepository = null;

                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, int>, BikeModelsRepository<BikeModelEntity, int>>();
                    modelRepository = container.Resolve<IBikeModelsRepository<BikeModelEntity, int>>();

                    objVersionSpecs = modelRepository.MVSpecsFeatures(versionId);

                    if (objVersionSpecs != null)
                    {
                        // Auto map the properties
                        objDTOVersionSpecs = new VersionSpecifications();

                        //Mapper.CreateMap<BikeSpecificationEntity, VersionSpecifications>();
                        //objDTOVersionSpecs = Mapper.Map<BikeSpecificationEntity, VersionSpecifications>(objVersionSpecs);
                        objDTOVersionSpecs = ModelEntityToDTO.ConvertSpecificationEntity(objVersionSpecs);
                        return Request.CreateResponse(HttpStatusCode.OK, objDTOVersionSpecs);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Model.ModelController");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "OOps ! Some error occured.");
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
        public HttpResponseMessage Get(uint modelId,bool isNew,bool?specs,bool?features)
        {
            BikeModelPageEntity objModelPage = null;
            ModelPage objDTOModelPage = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    IBikeModelsRepository<BikeModelEntity, uint> modelRepository = null;

                    container.RegisterType<IBikeModelsRepository<BikeModelEntity, uint>, BikeModelsRepository<BikeModelEntity, uint>>();
                    modelRepository = container.Resolve<IBikeModelsRepository<BikeModelEntity, uint>>();

                    objModelPage = modelRepository.GetModelPage(modelId, isNew);

                    if (objModelPage != null)
                    {
                        // Auto map the properties
                        objDTOModelPage = new ModelPage();
                        objDTOModelPage = ModelEntityToDTO.ConvertModelPage(objModelPage);
                        return Request.CreateResponse(HttpStatusCode.OK, objDTOModelPage);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NoContent, "No Data Found");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Model.ModelController");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "OOps ! Some error occured.");
            }
        }   // Get  Model Page
        #endregion   
        

    }     

}
