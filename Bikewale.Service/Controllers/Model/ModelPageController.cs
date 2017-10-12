﻿using Bikewale.BAL.PriceQuote;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.manufacturecampaign;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Interfaces.UserReviews;
using Bikewale.ManufacturerCampaign.Entities;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Model;
using Bikewale.Service.Utilities;
using Bikewale.Utility;
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
   
        private readonly IDealerPriceQuoteDetail _dealers;
        private readonly IBikeModels<Bikewale.Entities.BikeData.BikeModelEntity, int> _modelBL = null;
        private readonly IUserReviews _userReviews = null;
        private readonly IManufacturerCampaign _objManufacturerCampaign = null;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelRepository"></param>
        /// <param name="cache"></param>
        /// <param name="dealers"></param>
        /// <param name="modelBL"></param>
        /// <param name="userReviews"></param>
        public ModelPageController(IManufacturerCampaign objManufacturerCampaign, IBikeModelsRepository<Bikewale.Entities.BikeData.BikeModelEntity, int> modelRepository, IBikeModelsCacheRepository<int> cache, IDealerPriceQuoteDetail dealers, IBikeModels<Bikewale.Entities.BikeData.BikeModelEntity, int> modelBL, IUserReviews userReviews)
        {
            
            _dealers = dealers;
            _modelBL = modelBL;
            _userReviews = userReviews;
            _objManufacturerCampaign = objManufacturerCampaign;
        }

        #region Model Page Complete
        /// <summary>
        /// To get complete Model Page with Specs,Features,description and Details
        /// For the Specs and Features Default version selected is the one with maximum pricequotes
        /// Modified by Sajal Gupta on 28-02-2017
        /// Descrioption : Call BAL function instead of cache function to fetch model details.
        /// Modified by :   Sumit Kate on 26 Apr 2017
        /// Description :   For App, review count is fetched from old user reviews
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Complete Model Page</returns>
        [ResponseType(typeof(Bikewale.DTO.Model.ModelPage)), Route("api/model/details/")]
        public IHttpActionResult Get(int modelId)
        {
            BikeModelPageEntity objModelPage = null;
            Bikewale.DTO.Model.ModelPage objDTOModelPage = null;

            try
            {
                objModelPage = _modelBL.GetModelPageDetails(Convert.ToInt32(modelId));

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
                        objModelPage.ModelDetails.ReviewCount = (int)_userReviews.GetUserReviews(0, 0, (uint)modelId, 0, Entities.UserReviews.FilterBy.MostHelpful).TotalReviews;
                    }
                    else
                    {
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
        /// Modified by Sajal Gupta on 28-02-2017
        /// Descrioption : Call BAL function instead of cache function to fetch model details.
        /// Modified by :   Sumit Kate on 26 Apr 2017
        /// Description :   For App, review count is fetched from old user reviews
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns>Complete Model Page</returns>
        [ResponseType(typeof(Bikewale.DTO.Model.v2.ModelPage)), Route("api/v2/model/details/")]
        public IHttpActionResult GetV2(int modelId)
        {
            BikeModelPageEntity objModelPage = null;
            Bikewale.DTO.Model.v2.ModelPage objDTOModelPage = null;


            try
            {
                objModelPage = _modelBL.GetModelPageDetails(modelId);

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
                        objModelPage.ModelDetails.ReviewCount = (int)_userReviews.GetUserReviews(0, 0, (uint)modelId, 0, Entities.UserReviews.FilterBy.MostHelpful).TotalReviews;
                    }
                    else
                    {
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
        /// Modified by Sajal Gupta on 28-02-2017
        /// Descrioption : Call BAL function instead of cache function to fetch model details.
        /// Modified by :   Sumit Kate on 26 Apr 2017
        /// Description :   For App, review count is fetched from old user reviews
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(Bikewale.DTO.Model.v3.ModelPage)), Route("api/v3/model/details/")]
        public IHttpActionResult GetV3(uint modelId, int? cityId, int? areaId, string deviceId = null)
        {
            int modelID = Convert.ToInt32(modelId);
            Bikewale.DTO.Model.v3.ModelPage objDTOModelPage = null;
            try
            {
                if (modelId <= 0 || cityId <= 0)
                {
                    return BadRequest();
                }
                BikeModelPageEntity objModelPage = null;
                objModelPage = _modelBL.GetModelPageDetails(Convert.ToInt32(modelId));
                if (objModelPage != null)
                {
                    if (Request.Headers.Contains("platformId"))
                    {
                        string platformId = Request.Headers.GetValues("platformId").First().ToString();
                        if (platformId == "3")
                        {
                            objModelPage.ModelDetails.ReviewCount = (int)_userReviews.GetUserReviews(0, 0, (uint)modelId, 0, Entities.UserReviews.FilterBy.MostHelpful).TotalReviews;
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
        /// Modified by Sajal Gupta on 28-02-2017
        /// Descrioption : Call BAL function instead of cache function to fetch model details.
        /// Modified by :   Sumit Kate on 26 Apr 2017
        /// Description :   For App, review count is fetched from old user reviews
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(Bikewale.DTO.Model.v4.ModelPage)), Route("api/v4/model/details/")]
        public IHttpActionResult GetV4(uint modelId, int? cityId, int? areaId, string deviceId = null)
        {
            int modelID = Convert.ToInt32(modelId);
            Bikewale.DTO.Model.v4.ModelPage objDTOModelPage = null;
            try
            {
                if (modelId <= 0 || cityId <= 0)
                {
                    return BadRequest();
                }
                BikeModelPageEntity objModelPage = null;
                objModelPage = _modelBL.GetModelPageDetails(modelID);
                if (objModelPage != null)
                {
                    if (Request.Headers.Contains("platformId"))
                    {
                        string platformId = Request.Headers.GetValues("platformId").First().ToString();
                        if (platformId == "3")
                        {
                            objModelPage.ModelDetails.ReviewCount = (int)_userReviews.GetUserReviews(0, 0, modelId, 0, Entities.UserReviews.FilterBy.MostHelpful).TotalReviews;
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

                            if (cityId != null && cityId.Value > 0 && !objModelPage.ModelDetails.Futuristic)
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

                                        if (areaId.HasValue && areaId.Value > 0)
                                            versionId = (int)dealerPQRepository.GetDefaultPriceQuoteVersion((uint)modelID, (uint)cityId.Value, (uint)areaId.Value);
                                        else
                                            versionId = (int)dealerPQRepository.GetDefaultPriceQuoteVersion(Convert.ToUInt32(modelID), Convert.ToUInt32(cityId));

                                    }
                                }
                                if (pqEntity.IsExShowroomPrice)
                                    objDTOModelPage = ModelMapper.ConvertV4(objModelPage, pqEntity, null);
                                else
                                    objDTOModelPage = ModelMapper.ConvertV4(objModelPage, pqEntity,
                                    _dealers.GetDealerQuotationV2(Convert.ToUInt32(cityId), Convert.ToUInt32(versionId), pqEntity.DealerId, Convert.ToUInt32(areaId.HasValue ? areaId.Value : 0)));
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


        [ResponseType(typeof(DTO.Model.v5.ModelPage)), Route("api/v5/model/{modelId}/details/")]
        public IHttpActionResult GetV5(uint modelId, uint? cityId, int? areaId, string deviceId = null)
        {

            DTO.Model.v5.ModelPage objDTOModelPage = null;
            try
            {
                if (modelId <= 0 || cityId <= 0)
                {
                    return BadRequest();
                }
                BikeModelPageEntity objModelPage = null;
                ManufacturerCampaignEntity campaigns = null;
                objModelPage = _modelBL.GetModelPageDetails((int)modelId);
                if (objModelPage != null)
                {
                    if (Request.Headers.Contains("platformId"))
                    {
                        string platformId = Request.Headers.GetValues("platformId").First().ToString();
                        if (platformId == "3")
                        {
                            
                            #region On road pricing for versions
                            PQByCityArea getPQ;
                            PQByCityAreaEntity pqEntity = null;
                            if (!objModelPage.ModelDetails.Futuristic)
                            {

                                getPQ = new PQByCityArea();
                                pqEntity = getPQ.GetVersionListV2((int)modelId, objModelPage.ModelVersions, (int)cityId, areaId, Convert.ToUInt16(Bikewale.DTO.PriceQuote.PQSources.Android), null, null, deviceId);
                            }

                            if (cityId != null && cityId.Value > 0 && !objModelPage.ModelDetails.Futuristic)
                            {
                                int versionId = 0;
                                if (pqEntity != null && pqEntity.VersionList != null && pqEntity.VersionList.Any())
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

                                        if (areaId.HasValue && areaId.Value > 0)
                                            versionId = (int)dealerPQRepository.GetDefaultPriceQuoteVersion(modelId, (uint)cityId.Value, (uint)areaId.Value);
                                        else
                                            versionId = (int)dealerPQRepository.GetDefaultPriceQuoteVersion(modelId, Convert.ToUInt32(cityId));

                                    }
                                }
                                
                                if (pqEntity != null && pqEntity.DealerId == 0)
                                {
                                     campaigns = _objManufacturerCampaign.GetCampaigns(modelId, cityId.Value, ManufacturerCampaignServingPages.Mobile_Model_Page);
                                   
                                    
                                }



                                if (pqEntity != null && pqEntity.IsExShowroomPrice)
                                    objDTOModelPage = ModelMapper.ConvertV5(objModelPage, pqEntity, null, campaigns);
                                else
                                    objDTOModelPage = ModelMapper.ConvertV5(objModelPage, pqEntity,
                                    _dealers.GetDealerQuotationV2(Convert.ToUInt32(cityId), Convert.ToUInt32(versionId), pqEntity.DealerId, Convert.ToUInt32(areaId.HasValue ? areaId.Value : 0)),  campaigns);                                
                            }
                            else
                            {
                                objDTOModelPage = ModelMapper.ConvertV5(objModelPage, pqEntity, null, campaigns);
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
