using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Bikewale.Entities.BikeData;
using Bikewale.Interfaces.BikeData;
using System.Web.Http.Description;
using Bikewale.DTO.Model;
using Bikewale.Service.AutoMappers.Model;
using Bikewale.Notifications;
using Bikewale.BAL.PriceQuote;
using Bikewale.Entities.PriceQuote;
using Bikewale.DTO.Model.v3;



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
        /// Created by  :   Sangram Nandkhile on 16 Apr 2016
        /// Description :   This the new version v3 of existing API.        
        /// Removed specs, colors, features and unnecessary properties
        /// </summary>
        /// <param name="modelId"></param>
        /// <returns></returns>
        [ResponseType(typeof(Bikewale.DTO.Model.v3.ModelPage)), Route("api/v3/model/details/")]
        public IHttpActionResult GetV3(int modelId, int? cityId, int? areaId)
        {
            Bikewale.DTO.Model.v3.ModelPage objDTOModelPage = null;
            try
            {
                BikeModelPageEntity objModelPage = null;
                objModelPage = _cache.GetModelPageDetails(modelId);
                if (objModelPage != null)
                {
                    // If android, IOS client sanitize the article content 
                    string platformId = string.Empty;
                    if (Request.Headers.Contains("platformId"))
                    {
                        platformId = Request.Headers.GetValues("platformId").First().ToString();
                        if(platformId == "3")
                        {
                            #region Mapping from Entities to DTO

                            objDTOModelPage = new DTO.Model.v3.ModelPage();
                            objDTOModelPage.SmallDescription = objModelPage.ModelDesc.SmallDescription;
                            objDTOModelPage.MakeId = objModelPage.ModelDetails.MakeBase.MakeId;
                            objDTOModelPage.MakeName = objModelPage.ModelDetails.MakeBase.MakeName;
                            objDTOModelPage.ModelId = objModelPage.ModelDetails.ModelId;
                            objDTOModelPage.ModelName = objModelPage.ModelDetails.ModelName;
                            objDTOModelPage.ReviewCount = objModelPage.ModelDetails.ReviewCount;
                            objDTOModelPage.ReviewRate = objModelPage.ModelDetails.ReviewRate;
                            objDTOModelPage.IsDiscontinued = !objModelPage.ModelDetails.New;

                            if (objModelPage.objOverview != null)
                            {
                                var overViewList = new List<Bikewale.Entities.BikeData.Specs>();
                                foreach(var spec in objModelPage.objOverview.OverviewList)
                                {
                                    var ov = new Entities.BikeData.Specs()
                                    {
                                        DisplayText  = spec.DisplayText,
                                        DisplayValue = spec.DisplayValue
                                    };
                                    overViewList.Add(ov);
                                }
                                objDTOModelPage.overviewList = null;
                            }
                            if (objModelPage.Photos != null)
                            {
                                var photos = new List<DTO.Model.v3.CMSModelImageBase>();
                                foreach (var photo in objModelPage.Photos)
                                {
                                    var addPhoto = new DTO.Model.v3.CMSModelImageBase()
                                    {
                                        HostUrl = photo.HostUrl,
                                        OriginalImgPath = photo.OriginalImgPath
                                    };
                                    photos.Add(addPhoto);
                                }
                                objDTOModelPage.Photos = photos;
                            }
                            if (objModelPage.ModelVersions != null)
                            {
                                List<VersionDetail> modelSpecs = new List<VersionDetail>();
                                foreach (var version in objModelPage.ModelVersions)
                                {
                                    VersionDetail ver = new VersionDetail();
                                    ver.AlloyWheels = version.AlloyWheels;
                                    ver.AntilockBrakingSystem = version.AntilockBrakingSystem;
                                    ver.BrakeType = version.BrakeType;
                                    ver.ElectricStart = version.ElectricStart;
                                    ver.VersionId = version.VersionId;
                                    ver.VersionName = version.VersionName;
                                    ver.Price = version.Price;
                                    modelSpecs.Add(ver);
                                }
                                objDTOModelPage.ModelVersions = modelSpecs;
                            }
                            #endregion

                            #region On road pricing for versions
                            PQOnRoadPrice pqOnRoad = new PQOnRoadPrice();
                            if (cityId > 0)
                            {
                                PQByCityArea getPQ = new PQByCityArea();
                                pqOnRoad = getPQ.GetOnRoadPrice(modelId, cityId, areaId, 0);
                                if (pqOnRoad != null)
                                {
                                    if (pqOnRoad.DPQOutput!=null)
                                    {
                                        foreach (var version in objDTOModelPage.ModelVersions)
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
                                    }
                                }
                            }

                            #endregion
                            // Check if bike has more than 1 version and send base version as the first version in VersionList
                            if (objDTOModelPage.ModelVersions != null && objDTOModelPage.ModelVersions.Count > 1 && pqOnRoad!=null)
                            {
                                List<VersionDetail> swappedVersions = new List<VersionDetail>();
                                var baseModelVersion = objDTOModelPage.ModelVersions.FindAll(p => p.VersionId == pqOnRoad.BaseVersion).FirstOrDefault();
                                swappedVersions.Add(baseModelVersion);
                                objDTOModelPage.ModelVersions.RemoveAll(p => p.VersionId == pqOnRoad.BaseVersion);
                                swappedVersions.AddRange(objDTOModelPage.ModelVersions);
                                objDTOModelPage.ModelVersions = swappedVersions;
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
        #endregion
    }
}
