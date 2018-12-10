using AutoMapper;
using Carwale.DTOs.CarData;
using Carwale.DTOs.NewCars;
using Carwale.Entity.CarData;
using Carwale.Entity.CMS;
using Carwale.Entity.CMS.URIs;
using Carwale.Entity.Common;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CMS;
using Carwale.Interfaces.CMS.Photos;
using Carwale.Interfaces.NewCars;
using Carwale.Service.Filters;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Web.Http;
using System.Configuration;
using System.Linq;
using Carwale.Notifications;
using System.Web.Http.Cors;
using System.Collections.Specialized;
using Carwale.DTOs.CMS.ThreeSixtyView;
using System.Web;
using Carwale.Entity;
using Newtonsoft.Json.Serialization;
using System.Text;
using Carwale.BL.CMS;
using Carwale.Entity.AdapterModels;
using Carwale.BL.NewCars;
using Carwale.Service.Filters.CMS;

namespace Carwale.Service.Controllers.CarData
{
    public class CarModelDataController : ApiController
    {
        private readonly ICarModels _carModelBL;
        private readonly ICarVersions _carVersionBL;
        private readonly ICarModelRepository _carModelRepos;
        private readonly ICarModelCacheRepository _carModelCacheRepos;
        private readonly ICarMileage _carMileage;
        private readonly IPhotos _photosCacheRepo;
        private readonly IVideosBL _videosRepository;
        private readonly IUnityContainer _container;

        public CarModelDataController(IUnityContainer container, ICarModels carModelBL, ICarModelRepository carModelRepos, ICarModelCacheRepository carModelCacheRepos, ICarMileage carMileage, IPhotos photosCacheRepo, IVideosBL videosRepository, ICarVersions carVersionBL)
        {
            _carModelBL = carModelBL;
            _carVersionBL = carVersionBL;
            _carModelRepos = carModelRepos;
            _carModelCacheRepos = carModelCacheRepos;
            _carMileage = carMileage;
            _photosCacheRepo = photosCacheRepo;
            _videosRepository = videosRepository;
            _container = container;
        }

        /// <summary>
        /// Modified by: rakesh yadav on 28 Aug 2015
        /// Desc:Resolving dependency injection using UnityBootstraper and UnityResolver, send status code 404(if makes not found) and 200(makes are available)
        /// </summary>
        /// <returns></returns>
        //[AthunticateBasic]
        public IHttpActionResult GetValuationModels(int year, int make)
        {
            var valuationModel = new ValuationModelsDTO()
            {
                Models = _carModelRepos.GetValuationModels(year, make)
            };

            if (valuationModel.Models.Count == 0)
                return NotFound();

            return Ok(valuationModel);
        }

        /// <summary>
        /// Populates the model list based on type passed
        /// Written By : Supriya on 2/6/2014
        /// Modified by: rakesh yadav on 31 Aug 2015
        /// Desc:Resolving dependency injection using UnityBootstraper and UnityResolver, send status code 404(if makes not found) and 200(makes are available)
        /// </summary>
        /// <param name="qp"></param>
        /// <returns></returns>


        public IHttpActionResult GetAllModels(string type = "all")
        {
            List<CarMakeModelEntityBase> carModels = new List<CarMakeModelEntityBase>();
            carModels = _carModelCacheRepos.GetAllModels(type);

            if (carModels.Count == 0)
                return NotFound();

            return Ok(carModels);

        }

        /// <summary>
        /// Populates the model list based on type passed and makeid passed
        /// Written By : Supriya on 2/6/2014
        /// Modified by: rakesh yadav on 28 Aug 2015
        /// Desc:Resolving dependency injection using UnityBootstraper and UnityResolver
        /// </summary>
        /// <param name="qp"></param>
        /// <returns></returns>

        [EnableCors(origins: "http://opr.carwale.com,http://oprst.carwale.com,http://webserver:8082,http://localhost:8081,https://cwoprst.carwale.com,https://operations.carwale.com", headers: "*", methods: "GET"), Route("api/models/"), Route("api/CarModelData/GetCarModelsByType/"), Route("webapi/CarModelData/GetCarModelsByType/")]
        public IHttpActionResult GetCarModelsByType(string type = "all", int makeId = -1, int? year = null)
        {
            List<CarModelEntityBase> carModels = new List<CarModelEntityBase>();
            ThreeSixtyViewCategory threeSixtyType;
            Enum.TryParse(HttpContext.Current.Request.QueryString["threeSixtyType"], true, out threeSixtyType);
            carModels = _carModelCacheRepos.GetCarModelsByType(type, makeId, year, (int)threeSixtyType);

            if (carModels.Count == 0)
                return NotFound();

            return Ok(carModels);
        }

        [EnableCors(origins: "http://opr.carwale.com,http://oprst.carwale.com,http://webserver:8082,http://localhost:8081", headers: "*", methods: "GET"), Route("api/v1/models/")]
        public IHttpActionResult GetCarModelsByTypeV1(string type = "all", int makeId = -1, int? year = null)
        {
            List<CarModelEntityBase> carModels = new List<CarModelEntityBase>();
            ThreeSixtyViewCategory threeSixtyType;
            Enum.TryParse(HttpContext.Current.Request.QueryString["threeSixtyType"], true, out threeSixtyType);
            carModels = _carModelCacheRepos.GetCarModelsByType(type, makeId, year, (int)threeSixtyType);

            if (carModels.Count == 0)
                return NotFound();

            return ResponseMessage(new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(Mapper.Map<List<CarModelEntityBase>, List<CarModelsDTO>>(carModels), new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore }), Encoding.UTF8, "text/json") });
        }

        /// <summary>
        /// Populates the model list based on makeId passed
        /// Written By : Shalini on 15/07/14
        /// Modified by: rakesh yadav on 28 Aug 2015
        /// Desc:Resolving dependency injection using UnityBootstraper and UnityResolver
        /// </summary>
        /// <returns></returns> 
        [EnableCors(origins: "http://opr.carwale.com,http://oprst.carwale.com,http://webserver:8082,http://localhost:8082", headers: "*", methods: "GET")]
        public IHttpActionResult GetModelsByMake(int makeId = -1)
        {
            List<CarModelSummary> carModels = new List<CarModelSummary>();
            carModels = _carModelCacheRepos.GetModelsByMake(makeId);

            if (carModels.Count == 0)
                return NotFound();

            return ResponseMessage(new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(carModels)) });
        }

        /// <summary>
        /// Written by Rohan 06-11-2015
        /// Get New Models ,for a specific dealer if dealerid is passed
        /// </summary>
        /// <param name="makeId"></param>
        /// <param name="dealerId"></param>
        /// <returns></returns>
        [EnableCors(origins: "http://opr.carwale.com,http://oprst.carwale.com,http://webserver:8082,http://localhost:8082", headers: "*", methods: "GET")]
        public IHttpActionResult GetNewModelsByMake(int makeId = -1, int dealerId = 0)
        {
            List<CarModelSummary> carModels = new List<CarModelSummary>();
            carModels = _carModelBL.GetNewModelsByMake(makeId, dealerId);

            if (carModels.Count == 0)
                return NotFound();

            return ResponseMessage(new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(carModels)) });
        }

        /// <summary>
        /// Populates the upcoming car models list based on makeId passed 
        /// Written By : Shalini on 15/07/14
        /// Modified by: rakesh yadav on 31 Aug 2015
        /// Desc:Resolving dependency injection using UnityBootstraper and UnityResolver, send status code 404(if makes not found) and 200(makes are available)
        /// </summary>
        /// <returns></returns>

        public IHttpActionResult GetUpcomingCarModelsByMake(int makeId)
        {

            List<UpcomingCarModel> carModels = new List<UpcomingCarModel>();
            carModels = _carModelCacheRepos.GetUpcomingCarModelsByMake(makeId);

            if (carModels.Count == 0)
                return NotFound();

            return ResponseMessage(new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(carModels)) });
        }
        /// <summary>
        /// Populates the Similar Car Models list based on the modelId passed 
        /// Written By : Shalini on 15/07/14
        /// Modified by: rakesh yadav on 28 Aug 2015
        /// Desc:Resolving dependency injection using UnityBootstraper and UnityResolver
        /// </summary>
        /// <returns></returns>

        public IHttpActionResult GetSimilarCarModelsByModel(int modelid)
        {
            var request = HttpContext.Current.Request;
            var cid = request.Headers["IMEI"] ?? string.Empty;
            List<SimilarCarModels> similarCarModels = _carModelBL.GetSimilarCarsByModel(modelid, cid);

            if (similarCarModels.Count == 0)
                return NotFound();
            return ResponseMessage(new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(similarCarModels)) });
        }

        /// <summary>
        /// Populates the Model Colors list based on the modelId passed 
        /// Written By : Shalini on 15/07/14
        /// Modified by: rakesh yadav on 28 Aug 2015
        /// Desc:Resolving dependency injection using UnityBootstraper and UnityResolver
        /// </summary>
        /// <returns></returns>

        public IHttpActionResult GetModelColorsByModel(int modelId)
        {
            List<ModelColors> modelColors = new List<ModelColors>();

            modelColors = _carModelCacheRepos.GetModelColorsByModel(modelId);
            modelColors.ForEach(x =>
            {
                x.HexCode = x.HexCode.Split(',')[0].Trim();
                x.Color = x.Color.Split(new Char[] { '/', '\\', ',' })[0];
            });
            if (modelColors.Count == 0)
                return NotFound();

            return Ok(modelColors);
        }

        //Added by Ashish verma
        //fetching data from cache instead of directly call repository
        /// Modified by: rakesh yadav on 28 Aug 2015
        /// Desc:Resolving dependency injection using UnityBootstraper and UnityResolver
        public IHttpActionResult GetCarDetailsByModelId(int modelid)
        {
            return Json(_carModelCacheRepos.GetModelDetailsById(modelid));
        }

        /// <summary>
        /// Created By : Supriya K
        /// Modified by: rakesh yadav on 28 Aug 2015
        /// Desc:Resolving dependency injection using UnityBootstraper and UnityResolver
        /// </summary>
        /// <returns></returns>
        [AuthenticateBasic]
        public IHttpActionResult GetMileageByModelId(int modelid)
        {
            var versionList = _carVersionBL.GetCarVersions(modelid, Status.New);
            List<MileageDataDTO> mileage = null;
            if (versionList != null)
            {
                mileage = Mapper.Map<List<MileageDataEntity>, List<MileageDataDTO>>(_carMileage.GetMileageData(versionList));
            }
            if (mileage == null || mileage.Count == 0)
            {
                return NotFound();
            }

            return Ok(mileage);
        }

        /// Modified by: rakesh yadav on 28 Aug 2015
        /// Desc:Resolving dependency injection using UnityBootstraper and UnityResolver
        [HttpGet]
        //[CMSImageApiValidator]
        public IHttpActionResult Gallery([FromUri] ModelPhotosBycountURI galleryUri, HttpRequestMessage request)
        {
            //Default values used for android app
            if (string.IsNullOrWhiteSpace(galleryUri.CategoryIdList)) galleryUri.CategoryIdList = "8,10";

            var galleryDTO = new PhotoGalleryDTO();

            galleryDTO.modelImages = _photosCacheRepo.GetModelPhotosByCount(galleryUri);
            galleryDTO.modelVideos = _videosRepository.GetVideosByModelId(galleryUri.ModelId, CMSAppId.Carwale, 1, (int)galleryUri.TotalRecords);
            CarModelDetails carModelDetails = _carModelCacheRepos.GetModelDetailsById(galleryUri.ModelId);
            galleryDTO.ThreeSixtyAvailability = Mapper.Map<CarModelDetails, ThreeSixtyAvailabilityDTO>(carModelDetails);
            if (CMSCommon.IsThreeSixtyViewAvailable(carModelDetails))
            {
                galleryDTO.ThreeSixtyImage = new ThreeSixtyImageDto()
                {
                    ExteriorImagePath = CMSCommon.Get360ModelCarouselLinkageImageUrl(carModelDetails),
                    InteriorImagePath = CMSCommon.Get360ModelCarouselLinkageImageUrl(carModelDetails, true),
                    ImageHost = ConfigurationManager.AppSettings["CDNHostURL"].ToString()
                };
            }
            if (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && request.Headers.Contains("appversion") == true)
            {
                var source = request.Headers.GetValues("SourceId");
                string platform = string.Join(",", source);
                int sourceId = Convert.ToInt32(platform.Split(',')[0]);

                var app = request.Headers.GetValues("appVersion");
                string appV = string.Join(",", app);
                int appVersion = Convert.ToInt32(appV.Split(',')[0]);

                if ((sourceId.Equals(Convert.ToInt16(Platform.CarwaleiOS)) && appVersion <= 13) || (sourceId.Equals(Convert.ToInt16(Platform.CarwaleAndroid)) && appVersion <= 37))
                {
                    galleryDTO.modelImages.ForEach(x => x.HostUrl = x.HostUrl.Replace("https://", ""));
                }
            }

            if (galleryDTO == null)
                return NotFound();

            return Ok(galleryDTO);
        }

        [HttpGet]
        [Route("api/v2/model/{modelId:int:min(1)}/gallery/"), Route("webapi/CarModeldata/Gallery_V2/")]
        public IHttpActionResult Gallery_V2([FromUri] ModelPhotosBycountURI galleryUri, HttpRequestMessage request)
        {
            if (galleryUri.ModelId <= 0 || galleryUri.ApplicationId <= 0)
            {
                return BadRequest();
            }
            //Default values used for android app
            if (string.IsNullOrWhiteSpace(galleryUri.CategoryIdList)) galleryUri.CategoryIdList = "8,10";

            var galleryDTO = new PhotoGalleryDTO();

            galleryDTO.modelImages = _photosCacheRepo.GetModelPhotosByCount(galleryUri);
            galleryDTO.modelVideos = _videosRepository.GetVideosByModelId(galleryUri.ModelId, CMSAppId.Carwale, 1, (int)galleryUri.TotalRecords);
            CarModelDetails carModelDetails = _carModelCacheRepos.GetModelDetailsById(galleryUri.ModelId);
            List<ModelColors> modelColors = _carModelCacheRepos.GetModelColorsByModel(galleryUri.ModelId);
            galleryDTO.IsColorsLinkPresent = CMSCommon.IsModelColorPhotosPresent(modelColors);
            galleryDTO.ModelColors = Mapper.Map<List<ModelColors>, List<Carwale.DTOs.CMS.Photos.ModelColorsDTO>>(modelColors);

            galleryDTO.ThreeSixtyAvailability = Mapper.Map<CarModelDetails, ThreeSixtyAvailabilityDTO>(carModelDetails);
            if (CMSCommon.IsThreeSixtyViewAvailable(carModelDetails))
            {
                galleryDTO.ThreeSixtyImage = new ThreeSixtyImageDto()
                {
                    ExteriorImagePath = CMSCommon.Get360ModelCarouselLinkageImageUrl(carModelDetails),
                    InteriorImagePath = CMSCommon.Get360ModelCarouselLinkageImageUrl(carModelDetails, true),
                    ImageHost = ConfigurationManager.AppSettings["CDNHostURL"].ToString()
                };
            }
            if (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && request.Headers.Contains("appversion") == true)
            {
                var source = request.Headers.GetValues("SourceId");
                string platform = string.Join(",", source);
                int sourceId = Convert.ToInt32(platform.Split(',')[0]);

                var app = request.Headers.GetValues("appVersion");
                string appV = string.Join(",", app);
                int appVersion = Convert.ToInt32(appV.Split(',')[0]);

                if ((sourceId.Equals(Convert.ToInt16(Platform.CarwaleiOS)) && appVersion <= 13) || (sourceId.Equals(Convert.ToInt16(Platform.CarwaleAndroid)) && appVersion <= 37))
                {
                    galleryDTO.modelImages.ForEach(x => x.HostUrl = x.HostUrl.Replace("https://", ""));
                }
            }

            if (galleryDTO == null)
                return NotFound();

            return Ok(galleryDTO);
        }
        /// <summary>
        /// Created By : Chetan Thambad <14/12/2015>
        /// Desc: Showing Similar Car Models for Buying assistence System
        /// </summary>
        /// <returns></returns>
        [Route("api/model/similar")]
        public IHttpActionResult GetSimilarModels(int modelId, int cityId, int noOfRecommendation)
        {
            if (!RegExValidations.IsNumeric(modelId.ToString()) || noOfRecommendation < 1 || !RegExValidations.IsNumeric(cityId.ToString()))
            {
                return BadRequest();
            }

            var campaignRecommendations = _carModelBL.GetModelRecommendations(modelId, cityId, noOfRecommendation);
            return Ok(campaignRecommendations);
        }
        /// <summary>
        /// Created By : Ajay Singh <1/3/2016>
        /// Desc: Get ModelDetails on the basis of ModelId
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("api/model/{id:int:min(1)}")]
        public IHttpActionResult Get(int id)
        {

            var modelDto = new ModelPageDTO_Android();

            _container.RegisterInstance<int>(Convert.ToInt16(id));

            IServiceAdapter modelPageAdapter = _container.Resolve<IServiceAdapter>("ModelPageAndroid");

            modelDto = modelPageAdapter.Get<ModelPageDTO_Android>();


            if (modelDto != null)

                return Ok(modelDto);
            else
                return BadRequest();

        }
        /// <summary>
        /// Created By : jitendra Singh <8/11/2016>
        /// Desc: Get ModelDetails on the basis of ModelId
        /// </summary>
        /// <returns></returns>

        [HttpGet]
        [Route("api/v1/model/{id:int:min(1)}")]
        public IHttpActionResult Get_V1(int id, int cityid)
        {

            var modelDto = new ModelPageDTO_Android_V1();
            CustLocation custLocation = new CustLocation();
            custLocation.CityId = cityid;

            _container.RegisterInstance<int>(Convert.ToInt16(id));
            _container.RegisterInstance<CustLocation>(custLocation);

            IServiceAdapter modelPageAdapter = _container.Resolve<IServiceAdapter>("ModelPageAndroid_V1");

            modelDto = modelPageAdapter.Get<ModelPageDTO_Android_V1>();


            if (modelDto != null)

                return Ok(modelDto);
            else
                return BadRequest();
        }

        //added new parameter isAmp to support trending search on amp page
        [HttpGet, Route("api/trending/models/"), EnableCors("https://www-carwale-com.cdn.ampproject.org,https://cdn.ampproject.org,https://www-carwale-com.amp.cloudflare.com", "*", "GET")]
        [AmpFilter]
        public IHttpActionResult GetTrendingModels()
        {
            try
            {
                int platformId, count;
                bool isAmp;
                NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                if (String.IsNullOrEmpty(nvc["platformid"]) || String.IsNullOrEmpty(nvc["count"]))
                {
                    return BadRequest();
                }
                Int32.TryParse(nvc["platformid"], out platformId);
                Int32.TryParse(nvc["count"], out count);
                Boolean.TryParse(nvc["isamp"], out isAmp);

                var modelDto = _carModelBL.GetTrendingModelDetails(count, platformId, isAmp);
                return Ok(modelDto);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarModelDataController.GetTrendingModels()");
                objErr.LogException();
                return InternalServerError();
            }
        }

        [HttpOptions, Route("api/trending/models/"), EnableCors("*", "*", "OPTIONS")]
        public IHttpActionResult OptionsTrendingModels()
        {
            HttpContext.Current.Response.AppendHeader("Allow", "GET,OPTIONS");
            return Ok();
        }

        [HttpGet, Route("api/userhistory/models/")]
        public IHttpActionResult GetModelDetails(string modelIdList, int platformId)
        {
            try
            {
                List<CarMakeModelAdEntityBase> modelDto = _carModelBL.GetHistoryModelDetails(modelIdList, platformId);
                return Ok(modelDto);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarModelDataController.GetModelDetails()");
                objErr.LogException();
                return InternalServerError();
            }
        }

        [HttpGet, Route("api/model/")]
        public IHttpActionResult GetModelWithPriceDetails(int makeId, int cityId)
        {
            try
            {
                List<CarModelSummaryDTO> modelDto = new List<CarModelSummaryDTO>();

                modelDto = _carModelBL.GetModelDetails(makeId, cityId);
                return Ok(modelDto);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "CarModelDataController.GetModelDetails()");
                objErr.LogException();
                return InternalServerError();
            }
        }

        [HttpGet]
        [Route("api/v2/model/{id:int:min(1)}")]
        public IHttpActionResult Get_V2(int id, int cityId)
        {
            CarDataAdapterInputs modelInput = new CarDataAdapterInputs
            {
                CustLocation = new Location
                {
                    CityId = cityId
                },
                ModelDetails = new CarEntity
                {
                    ModelId = id
                }
            };
            IServiceAdapterV2 modelPageAdapter = _container.Resolve<IServiceAdapterV2>("ModelPageApp_V2");
            ModelPageDTOApp_V2 modelDto = modelPageAdapter.Get<ModelPageDTOApp_V2, CarDataAdapterInputs>(modelInput);
            if (modelDto != null)
                return Ok(modelDto);
            else
                return BadRequest();
        }
    }
}
