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
using Bikewale.Entities.CMS;
using Bikewale.Utility;
using System.Configuration;
using Bikewale.DTO.CMS.Photos;
using Bikewale.Service.AutoMappers.CMS;
using Bikewale.Entities.CMS.Photos;
using System.Web;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.BikeData;

namespace Bikewale.Service.Controllers.Model
{
    public class ModelPageController : ApiController
    {
        private string _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
        private string _applicationid = ConfigurationManager.AppSettings["applicationId"];
        private string _requestType = "application/json";
        private readonly IBikeModelsRepository<BikeModelEntity, int> _modelRepository = null;
        private readonly IBikeModelsCacheRepository<int> _cache;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelRepository"></param>
        public ModelPageController(IBikeModelsRepository<BikeModelEntity, int> modelRepository, IBikeModelsCacheRepository<int> cache)
        {
            _modelRepository = modelRepository;
            _cache = cache;
        }

        #region Model Page Complete
        /// <summary>
        /// To get complete Model Page with Specs,Features,description and Details
        /// For the Specs and Features Default version selected is the one with maximum pricequotes
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Complete Model Page</returns>
        [ResponseType(typeof(ModelPage)), Route("api/model/details/")]
        public IHttpActionResult Get(int modelId)
        {
            BikeModelPageEntity objModelPage = null;
            ModelPage objDTOModelPage = null;
            List<EnumCMSContentType> categorList = null;
            
            try
            {
                objModelPage = _cache.GetModelPageDetails(modelId);
                
                if (objModelPage != null)
                {
                    // If android, IOS client sanitize the article content 
                    string platformId = string.Empty;

                    if (Request.Headers.Contains("platformId"))
                    {
                        platformId = Request.Headers.GetValues("platformId").First().ToString();
                    }

                    if (!string.IsNullOrEmpty(platformId) && (platformId == "3" || platformId == "4"))
                    {
                        objModelPage.ModelVersionSpecs = null;
                    }
                    else {
                        objModelPage.objFeatures = null;
                        objModelPage.objOverview = null;
                        objModelPage.objSpecs = null;
                    }

                    // Auto map the properties
                    objDTOModelPage = new ModelPage();
                    objDTOModelPage = ModelMapper.Convert(objModelPage);

                    categorList = new List<EnumCMSContentType>();
                    categorList.Add(EnumCMSContentType.PhotoGalleries);
                    categorList.Add(EnumCMSContentType.RoadTest);

                    string contentTypeList = CommonApiOpn.GetContentTypesString(categorList);
                    string _apiUrl = String.Format("/webapi/image/modelphotolist/?applicationid={0}&modelid={1}&categoryidlist={2}", _applicationid, modelId, contentTypeList);

                    objDTOModelPage.Photos = BWHttpClient.GetApiResponseSync<List<CMSModelImageBase>>(_cwHostUrl, _requestType, _apiUrl, objDTOModelPage.Photos);
                    
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
