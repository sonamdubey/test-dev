using Carwale.Cache.CarData;
using AEPLCore.Cache;
using Carwale.DAL.CarData;
using Carwale.DTOs;
using Carwale.DTOs.CarData;
using Carwale.Entity;
using Carwale.Entity.CarData;
using Carwale.Entity.Common;
using Carwale.Entity.URIs.CarData;
using Carwale.Interfaces;
using Carwale.Interfaces.CarData;
using Carwale.Interfaces.CompareCars;
using Carwale.Interfaces.Offers;
using Carwale.Notifications;
using Carwale.Service.Filters;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Linq;
using AutoMapper;
using Carwale.Interfaces.PriceQuote;
using Carwale.Entity.PriceQuote;
using Carwale.DTOs.PriceQuote;
using Carwale.DTOs.Geolocation;
using System.Configuration;
using Carwale.Entity.Geolocation;
using Carwale.DTOs.Autocomplete;
using Carwale.Utility;
using System.Web.Http.Cors;
using Carwale.DTOs.CMS.ThreeSixtyView;
using Carwale.Notifications.Logs;
using Carwale.Interfaces.NewCars;
using Carwale.Entity.AdapterModels;
using Carwale.Entity.CompareCars;

namespace Carwale.Service.Controllers.CarData
{
    public class CarVersionsDataController : ApiController
    {
        private readonly ICarVersionRepository _carVersionRepo;
        private readonly ICarVersionCacheRepository _carVersionCacheRepo;
        private readonly ICarVersions _carVersion;
        private readonly IUnityContainer _container;
        private readonly ICarModelCacheRepository _modelCacheRepository;
        public CarVersionsDataController(
                                        ICarVersionRepository carVersionRepo, ICarVersionCacheRepository carVersionCacheRepo,
                                        ICarVersions carVersion, IUnityContainer container, ICarModelCacheRepository modelCacheRepository
                                        )
        {
            _carVersionCacheRepo = carVersionCacheRepo;
            _carVersionRepo = carVersionRepo;
            _carVersion = carVersion;
            _container = container;
            _modelCacheRepository = modelCacheRepository;
        }

        /// Modified by: rakesh yadav on 1 Sep 2015
        /// Desc:Resolving dependency injection using UnityBootstraper and UnityResolver
        //[AthunticateBasic]
        public IHttpActionResult GetValuationVersion(int year, int model)
        {
            ValuationVersionsDTO carVersions = new ValuationVersionsDTO()
            {
                Versions = _carVersionRepo.GetValuationVersion(year, model)
            };

            if (carVersions.Versions.Count == 0)
                return NotFound();

            return Ok(carVersions);
        }

        /// <summary>
        /// Modified by: rakesh yadav on 1 Sep 2015
        /// Desc:Resolving dependency injection using UnityBootstraper and UnityResolver
        /// </summary>
        /// <param name="qp"></param>
        /// <returns></returns>
        [EnableCors(origins: "https://cwoprst.carwale.com,https://operations.carwale.com", headers: "*", methods: "GET")]
        public IHttpActionResult GetCarVersions([FromUri]CarVersionsURI uri)
        {
            List<CarVersionEntity> carVersionsList = _carVersion.GetCarVersionsByType(uri.type, uri.modelId, uri.cityId);

            if (carVersionsList.Count == 0)
                return NotFound();

            return ResponseMessage(new HttpResponseMessage() { Content = new StringContent(JsonConvert.SerializeObject(carVersionsList)) });

        }
        /// <summary>
        /// Populates the Car Versions list based on the ModelId passed 
        /// Written By : Shalini on 16/07/14
        /// Modified by: rakesh yadav on 1 Sep 2015
        /// Desc:Resolving dependency injection using UnityBootstraper and UnityResolver
        /// </summary>
        /// <returns></returns>
        [EnableCors(origins: "http://opr.carwale.com,http://oprst.carwale.com", headers: "*", methods: "GET")]
        public IHttpActionResult GetVersionSummaryByModel(int modelId = -1)
        {
            List<CarVersionsV1> carVersion = null;

            carVersion = AutoMapper.Mapper.Map<List<CarVersionsV1>>(_carVersion.GetCarVersions(modelId, Status.All) ?? new List<CarVersions>());

            if (carVersion.Count == 0)
                return NotFound();

            return Ok(carVersion);
        }

        //added by ashish verma
        //modified by ashish verma on 29/09/2014 (List<CarVersionDetails> changed to CarVersiondetails Entity)
        /// Modified by: rakesh yadav on 1 Sep 2015
        /// Desc:Resolving dependency injection using UnityBootstraper and UnityResolver
        public IHttpActionResult GetCarDetailsByVersionId(int versionid)
        {
            var carVersionDetails = _carVersionCacheRepo.GetVersionDetailsById(versionid);

            if (carVersionDetails == null)
                return NotFound();
            return Json(carVersionDetails);
        }

        /// <summary>
        /// Author:Rakesh Yadav On 09 Oct 2015
        /// desc: get fuels and alternative fuels for version
        /// </summary>
        /// <param name="versionId"></param>
        /// <returns></returns>

        [HttpGet, Route("api/v1/version/fuels/")]
        public IHttpActionResult VersionFuelType(int versionId)
        {
            var carFuel = _carVersion.GetCarFuel(versionId);

            if (carFuel.Id == 0 || String.IsNullOrEmpty(carFuel.Name))
            {
                return NotFound();
            }

            return Ok(carFuel);
        }

        /// <summary>
        /// Gets all versions corresponding to a model Id with a flag representing existence of offers.
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="platformId"></param>
        /// <returns>IHttpActionResult</returns>

        [HttpGet, Route("api/versions/versionWithOffer/")]
        public IHttpActionResult GetVersionsWithOffers(int modelId, int cityId, int platformId)
        {
            if (modelId <= 0 || cityId <= 0 || platformId <= 0)
            {
                return BadRequest("Bad parameters");
            }

            try
            {
                return Ok(AutoMapper.Mapper.Map<List<Carwale.Entity.CarData.CarVersionEntity>, List<Carwale.DTOs.CarData.CarVersionsDTO>>
                    (_carVersionCacheRepo.GetCarVersionsByType("New", modelId)));
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "OfferController.GetVersionsWithOffers()");
                objErr.SendMail();
            }
            return InternalServerError();
        }

        /// <summary>
        /// Specs and Features
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <param name="platformId"></param>
        /// <returns>IHttpActionResult</returns>

        [HttpGet, Route("api/versions/specs/")]
        public IHttpActionResult GetSpecsFeatures(int versiondId)
        {
            if (versiondId <= 0)
                return BadRequest("Bad parameters");

            try
            {
                List<int> versionIds = new List<int>();
                versionIds.Add(versiondId);
                ICarDataLogic _carDataLogic = _container.Resolve<ICarDataLogic>();
                var versions = Mapper.Map<CarDataPresentation, CCarData>(_carDataLogic.GetCombinedCarDataOldApp(versionIds));

                SpecsFeatures response = new SpecsFeatures()
                {
                    Specs = AutoMapper.Mapper.Map<List<Carwale.Entity.CompareCars.SubCategory>, List<Carwale.DTOs.CarData.SubCategory>>(versions.Specs),
                    Features = AutoMapper.Mapper.Map<List<Carwale.Entity.CompareCars.SubCategory>, List<Carwale.DTOs.CarData.SubCategory>>(versions.Features),
                    OverView = AutoMapper.Mapper.Map<List<Carwale.Entity.CompareCars.Item>, List<Carwale.DTOs.CarData.Item>>(versions.OverView)
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "OfferController.GetVersionsWithOffers()");
                objErr.SendMail();
            }
            return InternalServerError();
        }
        [HttpGet, Route("api/versions/components/")]
        public IHttpActionResult GetVersionComponents(string vids = null, string type = null)
        {
            if (string.IsNullOrEmpty(vids) || string.IsNullOrEmpty(type))
                return BadRequest("vids and type parameters cannot be null or empty");
            try
            {
                SpecsFeatures response = new SpecsFeatures();
                List<int> versionList = null;
                List<int> types = null;
                try
                {

                    versionList = new List<int>(Array.ConvertAll(vids.Split(','), int.Parse));
                    if (!(versionList.All(item => item > 0)))
                        return BadRequest("VersionId should be greater than zero");
                    types = new List<int>(Array.ConvertAll(type.Split(','), int.Parse));
                }
                catch (Exception ex)
                {
                    ErrorClass objErr = new ErrorClass(ex, "CarVersionsDataController.GetVersionComponents()");
                    return BadRequest("Invalid parameters");
                }
                ICarDataLogic carDataLogic = _container.Resolve<ICarDataLogic>();
                var versions = Mapper.Map<CarDataPresentation, CCarData>(carDataLogic.GetCombinedCarDataOldApp(versionList));
                if (versions == null)
                    return Ok(response);
                foreach (var component in types)
                {
                    switch (component)
                    {
                        case (int)ComponentType.OverView:
                            response.OverView = AutoMapper.Mapper.Map<List<Carwale.Entity.CompareCars.Item>, List<Carwale.DTOs.CarData.Item>>(versions.OverView);
                            if (response.OverView != null)
                            {
                                foreach (var ov in response.OverView)
                                {
                                    if (ov.UnitType == "CUSTOM" || ov.UnitType == "BIT")
                                    {
                                        ov.UnitType = "";
                                    }
                                }
                            }
                            break;
                        case (int)ComponentType.Specs:
                            response.Specs = AutoMapper.Mapper.Map<List<Carwale.Entity.CompareCars.SubCategory>, List<Carwale.DTOs.CarData.SubCategory>>(versions.Specs);
                            if (response.Specs != null)
                            {
                                foreach (var sp in response.Specs)
                                {
                                    foreach (var item in sp.Items)
                                    {
                                        if (item.UnitType == "CUSTOM" || item.UnitType == "BIT")
                                        {
                                            item.UnitType = "";
                                        }
                                    }

                                }
                            }
                            break;
                        case (int)ComponentType.Features:
                            response.Features = AutoMapper.Mapper.Map<List<Carwale.Entity.CompareCars.SubCategory>, List<Carwale.DTOs.CarData.SubCategory>>(versions.Features);
                            if (response.Features != null)
                            {
                                foreach (var fs in response.Features)
                                {
                                    foreach (var item in fs.Items)
                                    {
                                        if (item.UnitType == "CUSTOM" || item.UnitType == "BIT")
                                        {
                                            item.UnitType = "";
                                        }
                                    }

                                }
                            }
                            break;
                        default:
                            return BadRequest("Type Parameter should be 1/2/3");
                    }
                }
                return Ok(response);


            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "CarVersionsDataController.GetVersionComponents()");
                objErr.SendMail();
            }
            return InternalServerError();
        }

        [AuthenticateBasic, HttpGet, Route("api/versions/")]
        public IHttpActionResult GetVersionDetailsList(int cityId, string modelIds = null, string versionIds = null, string type = "CompareAll")
        {
            bool ismodelsNull = string.IsNullOrWhiteSpace(modelIds);
            bool isversionsNull = string.IsNullOrWhiteSpace(versionIds);

            if ((ismodelsNull && isversionsNull) || (!ismodelsNull && !isversionsNull))
            {
                return BadRequest("Bad parameters");
            }

            try
            {
                var cardetailsList = new CarDetailsListDTO();
                cardetailsList.Models = new List<VersionDetailsDTO>();

                List<int> carList = null;
                Dictionary<int, List<CarVersionDetails>> versionListDetails = null;
                try
                {
                    if (!ismodelsNull)
                    {
                        carList = modelIds.Split(',').Select(Int32.Parse).ToList();
                        versionListDetails = _carVersion.GetVersionDetailsList(carList, null, cityId, type: type);
                    }
                    else
                    {
                        carList = versionIds.Split(',').Select(Int32.Parse).ToList();
                        versionListDetails = _carVersion.GetVersionDetailsList(null, carList, cityId, type: type);
                    }
                    return Ok(GetCarModelList(versionListDetails, carList));
                }
                catch (Exception ex)
                {
                    Logger.LogException(ex, "CarVersionsDataController.GetVersionDetailsList()");
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "CarVersionsDataController.GetVersionDetailsList()");
            }
            return InternalServerError();
        }
        [AuthenticateBasic, HttpGet, Route("api/v2/versions/")]
        public IHttpActionResult GetVersionDetailsListV2(int cityId, string modelIds = null, string versionIds = null, string type = "new")
        {
            try
            {
                bool isModelsNull = string.IsNullOrWhiteSpace(modelIds);
                bool isVersionsNull = string.IsNullOrWhiteSpace(versionIds);
                if ((isModelsNull && isVersionsNull) || (!isModelsNull && !isVersionsNull))
                {
                    return BadRequest("Bad parameters");
                }
                List<int> carList = new List<int>();
                if (modelIds != null)
                {
                    carList = modelIds.Split(',').Select(Int32.Parse).ToList();
                }
                else if (versionIds != null)
                {
                    carList = versionIds.Split(',').Select(Int32.Parse).ToList();
                }

                if (!carList.IsValidNumberList())
                {
                    return BadRequest("Bad parameters");
                }

                CarDataAdapterInputs inputParam = new CarDataAdapterInputs
                {
                    CustLocation = new Location
                    {
                        CityId = cityId
                    },
                    ModelIds = !isModelsNull ? carList : null,
                    VersionIds = isModelsNull ? carList : null,
                    Type = type
                };
                IServiceAdapterV2 versionPageAdapter = _container.Resolve<IServiceAdapterV2>("VersionPageAppAdapter");
                var carDetailsList = versionPageAdapter.Get<CarDetailsListDTOV2, CarDataAdapterInputs>(inputParam);
                if (carDetailsList != null && carDetailsList.Models != null && carDetailsList.Models.Count == 0)
                {
                    return NotFound();
                }
                else
                {
                    return Ok(carDetailsList);
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "CarVersionDataController.GetVersionDetailsListV2");
                return InternalServerError();
            }


        }
        [HttpGet, Route("api/versions/colors/")]
        public IHttpActionResult GetVersionsColor(string vids = null)
        {
            if (string.IsNullOrEmpty(vids))
                return BadRequest("version ids cannot be null or empty");
            try
            {
                List<List<Carwale.DTOs.CarData.Color>> color = new List<List<Carwale.DTOs.CarData.Color>>();
                try
                {
                    List<int> versionList = new List<int>(Array.ConvertAll(vids.Split(','), int.Parse));
                    if (!(versionList.All(item => item > 0)))
                        return BadRequest("Version Id must be greater than zero");
                    var versionColors = _carVersion.GetVersionsColors(versionList);
                    if (versionColors == null)
                        return Ok(color);
                    color = AutoMapper.Mapper.Map<List<List<Carwale.Entity.CompareCars.Color>>, List<List<Carwale.DTOs.CarData.Color>>>(versionColors);
                    var colorObj = new ColorDTO()
                    {
                        CarColors = color
                    };
                    return Ok(colorObj);
                }
                catch
                {
                    return BadRequest("Invalid Parameter");
                }

            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "CarVersionsDataController.GetVersionsColor()");
                objErr.SendMail();
            }
            return InternalServerError();
        }

        [AuthenticateBasicAttribute, HttpGet, Route("api/v1/versions/colors/")]
        public IHttpActionResult GetVersionsColor_V1(string versionids = null)
        {
            if (string.IsNullOrEmpty(versionids))
                return BadRequest("version ids cannot be null or empty");
            try
            {
                List<List<VersionColorDto>> color = null;
                ColorDto_V1 colorObj = new ColorDto_V1();
                try
                {
                    List<int> versionList = versionids.ConvertStringToList<int>();
                    if (versionList == null || versionList.Count <= 0 || versionList.Count > 10)
                    {
                        return BadRequest("The number of version ids should not be more than 10");
                    }
                    if (!(versionList.All(item => item > 0)))
                    {
                        return BadRequest("Version Id must be greater than zero");
                    }
                    var versionColors = _carVersion.GetVersionsColors(versionList);
                    if (versionColors == null)
                    {
                        return Ok(new List<List<VersionColorDto>>());
                    }
                    color = AutoMapper.Mapper.Map<List<List<Carwale.Entity.CompareCars.Color>>, List<List<VersionColorDto>>>(versionColors);

                    List<CarColorsDto> result = new List<CarColorsDto>();
                    for (int i = 0; i < versionColors.Count; i++)
                    {
                        result.Add(new CarColorsDto
                        {
                            VersionId = versionList[i],
                            Colors = color[i]
                        });
                    }
                    if (result.IsNotNullOrEmpty())
                    {
                        colorObj.CarColors = result;
                    }
                    return Ok(colorObj);
                }
                catch
                {
                    return BadRequest("Invalid Parameter");
                }

            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "CarVersionsDataController.GetVersionsColor()");
            }
            return InternalServerError();
        }

        [HttpGet, Route("api/PqVersionsAndCities/"), AuthenticateBasic]
        public IHttpActionResult PqVersionAndCities(int modelId)
        {
            try
            {
                var pqVersionCities = _carVersion.PqVersionsAndCities(modelId);

                return Ok(Mapper.Map<PqVersionCitiesEntity, PqVersionCitiesDTO>(pqVersionCities));
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "CarVersionsDataController.PqVersionAndCities()");
                objErr.SendMail();
                return InternalServerError(new Exception("Something went wrong on the server"));
            }
        }

        [HttpGet, Route("api/versiondetails/")]
        public IHttpActionResult VersionDetails(int versionId)
        {
            try
            {
                IServiceAdapterV2 versionPageAdapter = _container.Resolve<IServiceAdapterV2>("VersionPageAdapterAndroidV1");
                var versionDetails = versionPageAdapter.Get<VersionDetailsDTO_AndroidV1, VersionAndroidRequest>(new VersionAndroidRequest
                {
                    VersionId = versionId
                });

                return Ok(versionDetails);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "VersionDetails");
                return InternalServerError(new Exception("Something went wrong on the server"));
            }
        }

        public CarDetailsListDTO GetCarModelList(Dictionary<int, List<CarVersionDetails>> versionListDetails, List<int> carList)
        {
            var cardetailsList = new CarDetailsListDTO() { Models = new List<VersionDetailsDTO>() };
            Dictionary<int, CarModelDetails> carModelDetailsDict = new Dictionary<int, CarModelDetails>();
            foreach (var car in carList)
            {
                List<CarVersionDetails> versionDetails = null;
                versionListDetails.TryGetValue(car, out versionDetails);
                if (versionDetails != null && versionDetails.Count > 0)
                {
                    var versioninfo = versionDetails.First();
                    if (!carModelDetailsDict.ContainsKey(versioninfo.ModelId))
                    {
                        carModelDetailsDict.Add(versioninfo.ModelId, _modelCacheRepository.GetModelDetailsById(versioninfo.ModelId));
                    }
                    var results = Mapper.Map<List<CarVersionDetails>, List<VersionListDTO>>(versionDetails);
                    cardetailsList.Models.Add(new VersionDetailsDTO()
                    {
                        VersionDetails = results,
                        ModelId = versioninfo.ModelId,
                        MakeId = versioninfo.MakeId,
                        ModelName = versioninfo.ModelName,
                        MakeName = versioninfo.MakeName,
                        ThreeSixtyAvailability = Mapper.Map<CarModelDetails, ThreeSixtyAvailabilityDTO>(carModelDetailsDict[versioninfo.ModelId])
                    });
                }
                else
                {
                    cardetailsList.Models.Add(new VersionDetailsDTO());
                }
            }
            return cardetailsList;
        }
    }
}
