using Bikewale.BAL.BikeBooking;
using Bikewale.BAL.PriceQuote;
using Bikewale.Cache.Core;
using Bikewale.Cache.Location;
using Bikewale.DAL.Location;
using Bikewale.DTO.Model.v3;
using Bikewale.Entities.BikeData;
using Bikewale.Entities.Location;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.BikeData;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.Location;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Model;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;



namespace Bikewale.Service.Controllers.Model
{
    public class ModelPageController : ApiController
    {
        //private string _cwHostUrl = ConfigurationManager.AppSettings["cwApiHostUrl"];
        //private string _applicationid = ConfigurationManager.AppSettings["applicationId"];
        //private string _requestType = "application/json";
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

                    //categorList = new List<EnumCMSContentType>();
                    //categorList.Add(EnumCMSContentType.PhotoGalleries);
                    //categorList.Add(EnumCMSContentType.RoadTest);
                    //categorList.Add(EnumCMSContentType.ComparisonTests);
                    //string contentTypeList = CommonApiOpn.GetContentTypesString(categorList);

                    //categorList.Clear();
                    //categorList = null;

                    //string _apiUrl = String.Format("/webapi/image/modelphotolist/?applicationid={0}&modelid={1}&categoryidlist={2}", _applicationid, modelId, contentTypeList);

                    //objDTOModelPage.Photos = BWHttpClient.GetApiResponseSync<List<CMSModelImageBase>>(_cwHostUrl, _requestType, _apiUrl, objDTOModelPage.Photos);
                    //if (!string.IsNullOrEmpty(platformId) && (platformId == "3" || platformId == "4"))
                    //{
                    //    if (objDTOModelPage.Photos != null)
                    //    {
                    //        objDTOModelPage.Photos.Insert(0,
                    //            new CMSModelImageBase()
                    //            {
                    //                HostUrl = objDTOModelPage.ModelDetails.HostUrl,
                    //                OriginalImgPath = objDTOModelPage.ModelDetails.OriginalImagePath,
                    //                Caption = objDTOModelPage.ModelDetails.ModelName,
                    //                ImageCategory = "Model Image"
                    //            });
                    //    }
                    //    else
                    //    {
                    //        objDTOModelPage.Photos = new List<CMSModelImageBase>();
                    //        objDTOModelPage.Photos.Add(new CMSModelImageBase()
                    //        {
                    //            HostUrl = objDTOModelPage.ModelDetails.HostUrl,
                    //            OriginalImgPath = objDTOModelPage.ModelDetails.OriginalImagePath,
                    //            Caption = objDTOModelPage.ModelDetails.ModelName,
                    //            ImageCategory = "Model Image"
                    //        });
                    //    }
                    //}
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
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(Bikewale.DTO.Model.v3.ModelPage)), Route("api/v3/model/details/")]
        public IHttpActionResult GetV3(uint modelId, int? cityId, int? areaId)
        {
            int _modelId = Convert.ToInt32(modelId);
            Bikewale.DTO.Model.v3.ModelPage objDTOModelPage = null;
            try
            {
                if (modelId == 0 || cityId <= 0 || areaId <= 0)
                {
                    return BadRequest();
                }
                BikeModelPageEntity objModelPage = null;
                objModelPage = _cache.GetModelPageDetails(_modelId);
                if (objModelPage != null)
                {
                    if (Request.Headers.Contains("platformId"))
                    {
                        string platformId = Request.Headers.GetValues("platformId").First().ToString();
                        if (platformId == "3")
                        {
                            objDTOModelPage = ModelMapper.ConvertV3(objModelPage);

                            #region On road pricing for versions
                            PQOnRoadPrice pqOnRoad = new PQOnRoadPrice();
                            if (cityId > 0)
                            {
                                bool isAreaSelected = false;
                                var cityList = FetchCityByModelId(_modelId);
                                objDTOModelPage.IsCityExists = cityList != null && cityList.Any(p => p.CityId == cityId);
                                if (objDTOModelPage.IsCityExists)
                                {
                                    var areaList = GetAreaForCityAndModel(_modelId, Convert.ToInt16(cityId));
                                    objDTOModelPage.IsAreaExists = (areaList != null && areaList.Count() > 0);

                                    // If area is provided, check if area exists in list
                                    if (areaId > 0)
                                    {
                                        isAreaSelected = areaList != null && areaList.Any(p => p.AreaId == areaId);
                                    }
                                }
                                PQByCityArea getPQ = new PQByCityArea();
                                pqOnRoad = getPQ.GetOnRoadPrice(_modelId, cityId, areaId);

                                if (pqOnRoad != null)
                                {
                                    objDTOModelPage.PQId = pqOnRoad.PriceQuote.PQId;
                                    objDTOModelPage.DealerId = pqOnRoad.PriceQuote.DealerId;
                                    objDTOModelPage.IsExShowroomPrice = pqOnRoad.DPQOutput == null && pqOnRoad.BPQOutput == null;

                                    // When City has areas and area is not selected then show ex-showrrom price so user can select it
                                    bool isAreaExistAndSelected = objDTOModelPage.IsAreaExists && isAreaSelected;
                                    // when DPQ OR Only city level pricing exists
                                    if (isAreaExistAndSelected || (!objDTOModelPage.IsAreaExists))
                                    {
                                        #region  Iterate over version to fetch Dealer PQ or BikeWalePQ

                                        foreach (var version in objDTOModelPage.ModelVersions)
                                        {
                                            if (pqOnRoad.DPQOutput != null)
                                            {
                                                var selected = pqOnRoad.DPQOutput.Varients.Where(p => p.objVersion.VersionId == version.VersionId).FirstOrDefault();
                                                if (selected != null)
                                                {
                                                    version.Price = selected.OnRoadPrice;
                                                    version.IsDealerPriceQuote = true;
                                                }
                                                else if (pqOnRoad.BPQOutput != null && pqOnRoad.BPQOutput.Varients != null)
                                                {
                                                    var selectedBPQ = pqOnRoad.BPQOutput.Varients.Where(p => p.VersionId == version.VersionId).FirstOrDefault();
                                                    if (selectedBPQ != null)
                                                    {
                                                        version.Price = selectedBPQ.OnRoadPrice;
                                                        version.IsDealerPriceQuote = false;
                                                    }
                                                }
                                            }
                                            else if (pqOnRoad.BPQOutput != null && pqOnRoad.BPQOutput.Varients != null)
                                            {
                                                var selectedBPQ = pqOnRoad.BPQOutput.Varients.Where(p => p.VersionId == version.VersionId).FirstOrDefault();
                                                if (selectedBPQ != null)
                                                {
                                                    version.Price = selectedBPQ.OnRoadPrice;
                                                    version.IsDealerPriceQuote = false;
                                                }
                                            }
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        objDTOModelPage.IsExShowroomPrice = true;
                                    }
                                }
                            }
                            else if (cityId == null)
                            {
                                objDTOModelPage.IsExShowroomPrice = true;
                            }
                            #endregion
                            // Check if bike has more than 1 version and send base version as the first version in VersionList
                            if (objDTOModelPage.ModelVersions != null && objDTOModelPage.ModelVersions.Count > 1 && pqOnRoad != null)
                            {
                                List<VersionDetail> swappedVersions = SwapVersionList(objDTOModelPage, pqOnRoad);
                            }
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
        /// Created by: Sangram Nandkhile on 14 Apr 2016
        /// Put BaseVersion in the list as Zero'th element
        /// </summary>
        /// <param name="objDTOModelPage"></param>
        /// <param name="pqOnRoad"></param>
        /// <returns></returns>
        private List<VersionDetail> SwapVersionList(ModelPage objDTOModelPage, PQOnRoadPrice pqOnRoad)
        {
            try
            {
                List<VersionDetail> swappedList = new List<VersionDetail>();
                var baseModelVersion = objDTOModelPage.ModelVersions.FindAll(p => p.VersionId == pqOnRoad.BaseVersion).FirstOrDefault();
                swappedList.Add(baseModelVersion);
                objDTOModelPage.ModelVersions.RemoveAll(p => p.VersionId == pqOnRoad.BaseVersion);
                swappedList.AddRange(objDTOModelPage.ModelVersions);
                objDTOModelPage.ModelVersions = swappedList;
                return swappedList;
            }
            catch
            {

            }
        }


        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   24 Nov 2015
        /// Description     :   Gets City Details by ModelId
        /// </summary>
        /// <param name="modelId">Model Id</param>
        private IEnumerable<CityEntityBase> FetchCityByModelId(int modelId)
        {
            IEnumerable<CityEntityBase> cityList = null;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<ICity, CityRepository>()
                                 .RegisterType<ICacheManager, MemcacheManager>()
                                 .RegisterType<ICityCacheRepository, CityCacheRepository>();
                    ICityCacheRepository objcity = container.Resolve<ICityCacheRepository>();
                    cityList = objcity.GetPriceQuoteCities(Convert.ToUInt16(modelId));
                    return cityList;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ModelPageController" + "FetchCityByModelId");
                objErr.SendMail();
            }
            return cityList;
        }

        /// <summary>
        /// Author          :   Sangram Nandkhile
        /// Created Date    :   24 Nov 2015
        /// Description     :   Get List of Area depending on City and Model Id
        /// </summary>
        private IEnumerable<Bikewale.Entities.Location.AreaEntityBase> GetAreaForCityAndModel(int modelId, int cityId)
        {
            IEnumerable<Bikewale.Entities.Location.AreaEntityBase> areaList = null;
            try
            {

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, DealerPriceQuote>()
                        .RegisterType<ICacheManager, MemcacheManager>()
                        .RegisterType<IAreaCacheRepository, AreaCacheRepository>();

                    IAreaCacheRepository objArea = container.Resolve<IAreaCacheRepository>();
                    areaList = objArea.GetAreaList(Convert.ToUInt32(modelId), Convert.ToUInt32(cityId));
                    return areaList;
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass objErr = new Bikewale.Notifications.ErrorClass(ex, "ModelPageController" + "GetAreaForCityAndModel");
                objErr.SendMail();
            }

            return areaList;
        }
        #endregion
    }
}
