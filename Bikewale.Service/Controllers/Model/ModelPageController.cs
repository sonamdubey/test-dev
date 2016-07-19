using Bikewale.BAL.PriceQuote;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Model;
using Bikewale.Service.Utilities;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;



namespace Bikewale.Service.Controllers.Model
{
    /// <summary>
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class ModelPageController : CompressionApiController//ApiController
    {
        private readonly IBikeModelsRepository<BikeModelEntity, int> _modelRepository = null;
        private readonly IBikeModelsCacheRepository<int> _cache;
        private readonly IDealerPriceQuoteDetail _dealers;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelRepository"></param>
        public ModelPageController(IBikeModelsRepository<BikeModelEntity, int> modelRepository, IBikeModelsCacheRepository<int> cache, IDealerPriceQuoteDetail dealers)
        {
            _modelRepository = modelRepository;
            _cache = cache;
            _dealers = dealers;
        }

        #region Model Page Complete
        /// <summary>
        /// To get complete Model Page with Specs,Features,description and Details
        /// For the Specs and Features Default version selected is the one with maximum pricequotes
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Complete Model Page</returns>
        [ResponseType(typeof(Bikewale.DTO.Model.ModelPage)), Route("api/model/details/")]
        public IHttpActionResult Get(int modelId)
        {
            BikeModelPageEntity objModelPage = null;
            Bikewale.DTO.Model.ModelPage objDTOModelPage = null;
            //List<EnumCMSContentType> categorList = null;

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
                    else
                    {
                        //objModelPage.objFeatures = null;
                        //objModelPage.objOverview = null;
                        //objModelPage.objSpecs = null;
                        if (objModelPage.objFeatures != null && objModelPage.objFeatures.FeaturesList != null)
                        {
                            objModelPage.objFeatures.FeaturesList.Clear();
                            objModelPage.objFeatures.FeaturesList = null;
                            objModelPage.objFeatures = null;
                        }
                        if (objModelPage.objOverview != null && objModelPage.objOverview.OverviewList != null)
                        {
                            objModelPage.objOverview.OverviewList.Clear();
                            objModelPage.objOverview.OverviewList = null;
                            objModelPage.objOverview = null;
                        }
                        if (objModelPage.objSpecs != null && objModelPage.objSpecs.SpecsCategory != null)
                        {
                            objModelPage.objSpecs.SpecsCategory.Clear();
                            objModelPage.objSpecs.SpecsCategory = null;
                            objModelPage.objSpecs = null;
                        }
                    }

                    // Auto map the properties
                    objDTOModelPage = new Bikewale.DTO.Model.ModelPage();
                    objDTOModelPage = ModelMapper.Convert(objModelPage);

                    if (objModelPage != null)
                    {
                        if (objModelPage.ModelColors != null)
                        {
                            objModelPage.ModelColors = null;
                        }
                        if (objModelPage.ModelVersions != null)
                        {
                            objModelPage.ModelVersions.Clear();
                            objModelPage.ModelVersions = null;
                        }
                        if (objModelPage.objFeatures != null && objModelPage.objFeatures.FeaturesList != null)
                        {
                            objModelPage.objFeatures.FeaturesList.Clear();
                            objModelPage.objFeatures.FeaturesList = null;
                        }
                        if (objModelPage.objOverview != null && objModelPage.objOverview.OverviewList != null)
                        {
                            objModelPage.objOverview.OverviewList.Clear();
                            objModelPage.objOverview.OverviewList = null;
                        }
                        if (objModelPage.objSpecs != null && objModelPage.objSpecs.SpecsCategory != null)
                        {
                            objModelPage.objSpecs.SpecsCategory.Clear();
                            objModelPage.objSpecs.SpecsCategory = null;
                        }
                        if (objModelPage.Photos != null)
                        {
                            objModelPage.Photos.Clear();
                            objModelPage.Photos = null;
                        }
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

        /// <summary>
        /// Created by  :   Sumit Kate on 29 Jan 2016
        /// Description :   This the new version of existing API.        
        /// This returns the new dual tone colour with hexcodes(as list) 
        /// along with complete Model Page with Specs,Features,description and Details
        /// For the Specs and Features Default version selected is the one with maximum pricequotes
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Complete Model Page</returns>
        [ResponseType(typeof(Bikewale.DTO.Model.v2.ModelPage)), Route("api/v2/model/details/")]
        public IHttpActionResult GetV2(int modelId)
        {
            BikeModelPageEntity objModelPage = null;
            Bikewale.DTO.Model.v2.ModelPage objDTOModelPage = null;
            //List<EnumCMSContentType> categorList = null;

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
                    else
                    {
                        //objModelPage.objFeatures = null;
                        //objModelPage.objOverview = null;
                        //objModelPage.objSpecs = null;
                        if (objModelPage.objFeatures != null && objModelPage.objFeatures.FeaturesList != null)
                        {
                            objModelPage.objFeatures.FeaturesList.Clear();
                            objModelPage.objFeatures.FeaturesList = null;
                            objModelPage.objFeatures = null;
                        }
                        if (objModelPage.objOverview != null && objModelPage.objOverview.OverviewList != null)
                        {
                            objModelPage.objOverview.OverviewList.Clear();
                            objModelPage.objOverview.OverviewList = null;
                            objModelPage.objOverview = null;
                        }
                        if (objModelPage.objSpecs != null && objModelPage.objSpecs.SpecsCategory != null)
                        {
                            objModelPage.objSpecs.SpecsCategory.Clear();
                            objModelPage.objSpecs.SpecsCategory = null;
                            objModelPage.objSpecs = null;
                        }
                    }

                    // Auto map the properties
                    objDTOModelPage = new Bikewale.DTO.Model.v2.ModelPage();
                    objDTOModelPage = ModelMapper.ConvertV2(objModelPage);

                    if (objModelPage != null)
                    {
                        if (objModelPage.ModelColors != null)
                        {
                            objModelPage.ModelColors = null;
                        }
                        if (objModelPage.ModelVersions != null)
                        {
                            objModelPage.ModelVersions.Clear();
                            objModelPage.ModelVersions = null;
                        }
                        if (objModelPage.objFeatures != null && objModelPage.objFeatures.FeaturesList != null)
                        {
                            objModelPage.objFeatures.FeaturesList.Clear();
                            objModelPage.objFeatures.FeaturesList = null;
                        }
                        if (objModelPage.objOverview != null && objModelPage.objOverview.OverviewList != null)
                        {
                            objModelPage.objOverview.OverviewList.Clear();
                            objModelPage.objOverview.OverviewList = null;
                        }
                        if (objModelPage.objSpecs != null && objModelPage.objSpecs.SpecsCategory != null)
                        {
                            objModelPage.objSpecs.SpecsCategory.Clear();
                            objModelPage.objSpecs.SpecsCategory = null;
                        }
                        if (objModelPage.Photos != null)
                        {
                            objModelPage.Photos.Clear();
                            objModelPage.Photos = null;
                        }
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
        /// <summary>
        /// Created by  :   Sangram Nandkhile on 13 Apr 2016
        /// Description :   This the new version v3 of existing API.        
        /// Removed specs, colors, features and unnecessary properties
        /// Modified by :   Sumit Kate on 23 May 2016
        /// Description :   Get the Device Id from deviceId parameter
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(Bikewale.DTO.Model.v3.ModelPage)), Route("api/v3/model/details/")]
        public IHttpActionResult GetV3(uint modelId, int? cityId, int? areaId, string deviceId = null)
        {
            int modelID = Convert.ToInt32(modelId);
            Bikewale.DTO.Model.v3.ModelPage objDTOModelPage = null;
            try
            {
                if (modelId <= 0 || cityId <= 0 || areaId <= 0)
                {
                    return BadRequest();
                }
                BikeModelPageEntity objModelPage = null;
                objModelPage = _cache.GetModelPageDetails(modelID);
                if (objModelPage != null)
                {
                    if (Request.Headers.Contains("platformId"))
                    {
                        string platformId = Request.Headers.GetValues("platformId").First().ToString();
                        if (platformId == "3")
                        {
                            #region On road pricing for versions
                            PQOnRoadPrice pqOnRoad; PQByCityArea getPQ;
                            PQByCityAreaEntity pqEntity = null;
                            if (!objModelPage.ModelDetails.Futuristic)
                            {
                                pqOnRoad = new PQOnRoadPrice();
                                getPQ = new PQByCityArea();
                                pqEntity = getPQ.GetVersionList(modelID, objModelPage.ModelVersions, cityId, areaId, Convert.ToUInt16(Bikewale.DTO.PriceQuote.PQSources.Android), null, null, deviceId);
                            }
                            objDTOModelPage = ModelMapper.ConvertV3(objModelPage, pqEntity);
                            #endregion
                        }
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
        }


        /// <summary>
        /// Created by  :   Lucky Rathore on 16 Jun 2016
        /// Description :   This the new version v4 of existing API update include PrimatyDealer, IsPrimary, SecondaryDealerCount and AltPrimarySectionText.       
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(Bikewale.DTO.Model.v4.ModelPage)), Route("api/v4/model/details/")]
        public IHttpActionResult GetV4(uint modelId, int? cityId, int? areaId, string deviceId = null)
        {
            int modelID = Convert.ToInt32(modelId);
            Bikewale.DTO.Model.v4.ModelPage objDTOModelPage = null;
            try
            {
                if (modelId <= 0 || cityId <= 0 || areaId <= 0)
                {
                    return BadRequest();
                }
                BikeModelPageEntity objModelPage = null;
                objModelPage = _cache.GetModelPageDetails(modelID);
                if (objModelPage != null)
                {
                    if (Request.Headers.Contains("platformId"))
                    {
                        string platformId = Request.Headers.GetValues("platformId").First().ToString();
                        if (platformId == "3")
                        {
                            #region On road pricing for versions
                            PQOnRoadPrice pqOnRoad;
                            PQByCityArea getPQ;
                            PQByCityAreaEntity pqEntity = null;
                            if (!objModelPage.ModelDetails.Futuristic)
                            {
                                pqOnRoad = new PQOnRoadPrice();
                                getPQ = new PQByCityArea();
                                pqEntity = getPQ.GetVersionList(modelID, objModelPage.ModelVersions, cityId, areaId, Convert.ToUInt16(Bikewale.DTO.PriceQuote.PQSources.Android), null, null, deviceId);
                            }

                            if (areaId != null && !objModelPage.ModelDetails.Futuristic)
                            {
                                int versionId = 0;
                                if (pqEntity != null && pqEntity.VersionList != null && pqEntity.VersionList.Count() > 0)
                                {
                                    var deafultVersion = pqEntity.VersionList.FirstOrDefault(i => i.IsDealerPriceQuote);
                                    if (deafultVersion != null)
                                    {
                                        versionId = deafultVersion.VersionId;
                                    }
                                }

                                if (versionId <= 0)
                                {
                                    using (IUnityContainer container = new UnityContainer())
                                    {
                                        container.RegisterType<IDealerPriceQuote, Bikewale.DAL.BikeBooking.DealerPriceQuoteRepository>();
                                        IDealerPriceQuote dealerPQRepository = container.Resolve<IDealerPriceQuote>();
                                        versionId = (int)dealerPQRepository.GetDefaultPriceQuoteVersion(Convert.ToUInt32(modelID), Convert.ToUInt32(cityId));
                                    }
                                }
                                objDTOModelPage = ModelMapper.ConvertV4(objModelPage, pqEntity,
                                    _dealers.GetDealerQuotation(Convert.ToUInt32(cityId), Convert.ToUInt32(versionId), pqEntity.DealerId));
                            }
                            else
                            {
                                objDTOModelPage = ModelMapper.ConvertV4(objModelPage, pqEntity, null);
                            }

                            #endregion
                        }
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
        }

        #endregion
    }
}
