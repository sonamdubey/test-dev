using Bikewale.Entities.BikeData;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Interfaces.UserReviews;
using Bikewale.ManufacturerCampaign.Interface;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Model;
using Bikewale.Service.Utilities;
using Bikewale.Utility;
using log4net;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
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
        private readonly IPQByCityArea _objPQByCityArea = null;
        private readonly IPriceQuoteCache _objPqCache = null;
        private readonly IDealerPriceQuote _dealerPriceQuote;
        private static readonly ILog _logger = LogManager.GetLogger(typeof(ModelPageController));
        private static HashSet<string> _ampCors = new HashSet<string> { "https://www-bikewale-com.cdn.ampproject.org", "https://www-bikewale-com.amp.cloudflare.com", "https://cdn.ampproject.org" ,"https://www.bikewale.com" };

        /// <summary>
        /// Modified by :   Sumit Kate on 29 Mar 2018
        /// Description :   Added Price Quote cache layer interface
        /// Modified by :   Monika Korrapati on 27 Sept 2018
        /// Description :   Added DealerPriceQuote BAL layer interface
        /// </summary>
        /// <param name="objManufacturerCampaign"></param>
        /// <param name="modelRepository"></param>
        /// <param name="cache"></param>
        /// <param name="dealers"></param>
        /// <param name="modelBL"></param>
        /// <param name="userReviews"></param>
        /// <param name="objPQByCityArea"></param>
        /// <param name="objPqCache"></param>
        public ModelPageController(IManufacturerCampaign objManufacturerCampaign, IBikeModelsRepository<Bikewale.Entities.BikeData.BikeModelEntity, int> modelRepository, IBikeModelsCacheRepository<int> cache, IDealerPriceQuoteDetail dealers, IBikeModels<Bikewale.Entities.BikeData.BikeModelEntity, int> modelBL, IUserReviews userReviews, IPQByCityArea objPQByCityArea,
            IPriceQuoteCache objPqCache, IDealerPriceQuote dealerPriceQuote)
        {

            _dealers = dealers;
            _modelBL = modelBL;
            _userReviews = userReviews;
            _objManufacturerCampaign = objManufacturerCampaign;
            _objPQByCityArea = objPQByCityArea;
            _objPqCache = objPqCache;
            _dealerPriceQuote = dealerPriceQuote;
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
		[Obsolete("model detail API verison obsolete. Check current version V5")]
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
                        objModelPage.ModelVersionMinSpecs = null;
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Model.ModelController");

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
		[Obsolete("V2 version is obsolete. Check current version V5")]
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
                        objModelPage.ModelVersionMinSpecs = null;
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Model.ModelController");

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
                            PQByCityAreaEntity pqEntity = null;
                            if (!objModelPage.ModelDetails.Futuristic)
                            {
                                pqEntity = _objPQByCityArea.GetVersionList(modelID, objModelPage.ModelVersions, cityId, areaId, Convert.ToUInt16(Bikewale.DTO.PriceQuote.PQSources.Android), null, null, deviceId);
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Model.ModelController");

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
        /// Modified by :   Monika Korrapati on 27 Sept 2018
        /// Description :   versionId will be fetched from BAL instead of DAL.
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
                            PQByCityAreaEntity pqEntity = null;
                            if (!objModelPage.ModelDetails.Futuristic)
                            {
                                pqEntity = _objPQByCityArea.GetVersionList(modelID, objModelPage.ModelVersions, cityId, areaId, Convert.ToUInt16(Bikewale.DTO.PriceQuote.PQSources.Android), null, null, deviceId);
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
                                            versionId = (int)dealerPQRepository.GetDefaultPriceQuoteVersion((uint)modelID, (uint)cityId.Value, (uint)areaId.Value);
                                        else
                                            versionId = (int)_dealerPriceQuote.GetDefaultPriceQuoteVersion(Convert.ToUInt32(modelID), Convert.ToUInt32(cityId));

                                    }
                                }
                                if (pqEntity != null && pqEntity.IsExShowroomPrice)
                                    objDTOModelPage = ModelMapper.ConvertV4(objModelPage, pqEntity, null);
                                else
                                    objDTOModelPage = ModelMapper.ConvertV4(objModelPage, pqEntity,
                                    pqEntity.DealerEntity);
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Model.ModelController");

                return InternalServerError();
            }
        }


        [ResponseType(typeof(DTO.Model.v5.ModelPage)), Route("api/v5/model/{modelId}/details/")]
        public IHttpActionResult GetV5(uint modelId, uint? cityId = null, int? areaId = null, string deviceId = null)
        {

            DTO.Model.v5.ModelPage objDTOModelPage = null;
            try
            {
                if (modelId <= 0 || !Request.Headers.Contains("platformId"))
                {
                    return BadRequest();
                }

                BikeModelPageEntity objModelPage = null;
                objModelPage = _modelBL.GetModelPageDetails((int)modelId);


                if (objModelPage != null)
                {

                    PQByCityAreaEntity pqEntity = null;
                    ushort platformId;

                    if (!objModelPage.ModelDetails.Futuristic)
                    {
                        pqEntity = _objPQByCityArea.GetVersionListV2((int)modelId, objModelPage.ModelVersions, (int)(cityId.HasValue ? cityId.Value : 0), areaId, Convert.ToUInt16(Bikewale.DTO.PriceQuote.PQSources.Android), null, null, deviceId);
                    }

                    if (ushort.TryParse(Request.Headers.GetValues("platformId").First().ToString(), out platformId) && platformId == 3 && cityId.HasValue && cityId.Value > 0)
                    {
                        #region On road pricing for versions
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

                            if (pqEntity != null && pqEntity.IsExShowroomPrice)
                                objDTOModelPage = ModelMapper.ConvertV5(_objPqCache, objModelPage, pqEntity, null, platformId);
                            else

                                objDTOModelPage = ModelMapper.ConvertV5(_objPqCache, objModelPage, pqEntity,
                                pqEntity.DealerEntity, platformId);

                        }
                        else
                        {
                            objDTOModelPage = ModelMapper.ConvertV5(_objPqCache, objModelPage, pqEntity, null, platformId);
                        }
                        #endregion
                    }
                    else
                    {
                        objDTOModelPage = ModelMapper.ConvertV5(_objPqCache, objModelPage, pqEntity, null, platformId);
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
                ErrorClass.LogError(ex, String.Format("Exception : Bikewale.Service.Model.ModelController.GetV5({0},{1},{2})", modelId, cityId, areaId));
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 11 October 2018
        /// Description : new version for PQId related changes
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="areaId"></param>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        [ResponseType(typeof(DTO.Model.v6.ModelPage)), Route("api/v6/model/{modelId}/details/")]
        public IHttpActionResult GetV6(uint modelId, uint? cityId = null, int? areaId = null, string deviceId = null)
        {

            DTO.Model.v6.ModelPage objDTOModelPage = null;
            try
            {
                if (modelId <= 0 || !Request.Headers.Contains("platformId"))
                {
                    return BadRequest();
                }

                BikeModelPageEntity objModelPage = null;
                objModelPage = _modelBL.GetModelPageDetails((int)modelId);


                if (objModelPage != null)
                {

                    Bikewale.Entities.PriceQuote.v3.PQByCityAreaEntity pqEntity = null;
                    ushort platformId;

                    if (!objModelPage.ModelDetails.Futuristic)
                    {
                        pqEntity = _objPQByCityArea.GetVersionListV3((int)modelId, objModelPage.ModelVersions, (int)(cityId.HasValue ? cityId.Value : 0), areaId, Convert.ToUInt16(Bikewale.DTO.PriceQuote.PQSources.Android), null, null, deviceId);
                    }

                    if (ushort.TryParse(Request.Headers.GetValues("platformId").First().ToString(), out platformId) && platformId == 3 && cityId.HasValue && cityId.Value > 0)
                    {
                        #region On road pricing for versions
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

                            if (pqEntity != null && pqEntity.IsExShowroomPrice)
                                objDTOModelPage = ModelMapper.ConvertV6(_objPqCache, objModelPage, pqEntity, null, platformId);
                            else

                                objDTOModelPage = ModelMapper.ConvertV6(_objPqCache, objModelPage, pqEntity,
                                pqEntity.DealerEntity, platformId);

                        }
                        else
                        {
                            objDTOModelPage = ModelMapper.ConvertV6(_objPqCache, objModelPage, pqEntity, null, platformId);
                        }
                        #endregion
                    }
                    else
                    {
                        objDTOModelPage = ModelMapper.ConvertV6(_objPqCache, objModelPage, pqEntity, null, platformId);
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
                ErrorClass.LogError(ex, String.Format("Exception : Bikewale.Service.Model.ModelController.GetV6({0},{1},{2})", modelId, cityId, areaId));
                return InternalServerError();
            }
        }

        [Route("api/model/{modelId}/details/amp/"), EnableCors("https://www-bikewale-com.cdn.ampproject.org, https://www-bikewale-com.amp.cloudflare.com, https://cdn.ampproject.org,https://www.bikewale.com", "*", "GET")]
        public IHttpActionResult GetModelDetails(uint modelId)
        {
            string ampSourceOrigin = HttpUtility.ParseQueryString(Request.RequestUri.Query)["__amp_source_origin"];
            string ampOrigin = Request.Headers.Contains("Origin") ? Request.Headers.GetValues("Origin").FirstOrDefault() : string.Empty;

            if (!string.IsNullOrEmpty(ampSourceOrigin) && _ampCors.Contains(ampOrigin))
            {
                BWCookies.AddAmpHeaders(ampSourceOrigin, ampOrigin, true);
            }
            
            return Ok();
        }
        #endregion
    }

}
