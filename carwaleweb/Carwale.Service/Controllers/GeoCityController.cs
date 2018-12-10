using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;
using Carwale.Interfaces.Geolocation;
using Carwale.Entity.Geolocation;
using Carwale.Interfaces;
using System.Collections.Specialized;
using System.Web;
using Newtonsoft.Json;
using Carwale.Notifications;
using AutoMapper;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.IPToLocation;
using Carwale.Notifications.Logs;
using Carwale.Entity.Common;
using Carwale.Utility.Serialization;
using System.Web.Http.Cors;
using AEPLCore.Utils.GeoMaps;
using Carwale.Interfaces.AutoComplete;
using System.Net.Http.Formatting;
using Newtonsoft.Json.Serialization;
using Carwale.Service.Filters;
using Carwale.Entity.Enum;
using System.Net;
using Carwale.Utility;

namespace Carwale.Service.Controllers
{
    public class GeoCityController : ApiController
    {
        private readonly IPQGeoLocationBL _geoLocationBL;
        private readonly IGeoCitiesCacheRepository _geoCitiesCacheRepo;
        private readonly IRepository<Cities> _geoCityCache;
        private readonly IAutoComplete_v1 _autoComplete_v1;
        private readonly IGeoCityLogic _geoCityLogic;
        private static readonly JsonMediaTypeFormatter _jsonFormatter = new JsonMediaTypeFormatter
        {
            SerializerSettings =
            {   DefaultValueHandling = DefaultValueHandling.Ignore,
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
            }
        };
        public GeoCityController
            (
                IPQGeoLocationBL geoLocationBL,
                IGeoCitiesCacheRepository geoCitiesCacheRepo,
                IRepository<Cities> geoCityCache,
                IAutoComplete_v1 autoComplete_v1,
                IGeoCityLogic geoCityLogic
            )
        {
            _geoLocationBL = geoLocationBL;
            _geoCitiesCacheRepo = geoCitiesCacheRepo;
            _geoCityCache = geoCityCache;
            _autoComplete_v1 = autoComplete_v1;
            _geoCityLogic = geoCityLogic;
        }
        /// <summary>
        /// Created by  : Ashish Verma
        /// Description :Gets the PQ city name and CityId where price exists for that model by model id
        /// Date        : August 1, 2014
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetPQCitiesByModelId()
        {
            var response = new HttpResponseMessage();
            try
            {
                int modelId;
                NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

                modelId = string.IsNullOrEmpty(nvc["modelid"]) ? -1 : Convert.ToInt32(nvc["modelid"]);
                if (modelId == -1)
                {
                    response.Content = null;
                    return response;
                }

                List<PriceQuoteCityDTO> cityList = Mapper.Map<List<City>, List<PriceQuoteCityDTO>>(_geoCitiesCacheRepo.GetPQCitiesByModelId(modelId));

                response.Content = new StringContent(JsonConvert.SerializeObject(cityList));

            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GeoCityController.GetPqCitiesByModelId()");
                objErr.LogException();
            }
            return response;
        }

        /// <summary>
        /// Created by  : Ashish Verma
        /// Description Gets the PQ State name and StateId where price exists for that model by model id
        /// Date        : August 1, 2014
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetGeoPQStatesByModelId()
        {
            var response = new HttpResponseMessage();

            int modelId;
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

            modelId = string.IsNullOrEmpty(nvc["modelid"]) ? -1 : Convert.ToInt32(nvc["modelid"]);
            if (modelId == -1)
            {
                response.Content = null;
                return response;
            }
            try
            {
                StatesAndPopularCities obj = _geoLocationBL.GetPQStatesAndPopularCities(modelId);

                response.Content = new StringContent(JsonConvert.SerializeObject(obj));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GeoCityController.GetGeoPqStatesByModelId()");
                objErr.LogException();
            }
            return response;
        }

        /// <summary>
        /// Created by  : Ashish Verma
        /// Description Gets the PQ city name and city id where price exists for that model by model id and state id
        /// Date        : August 1, 2014
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetPQCitiesByModelAndStateId()
        {
            var response = new HttpResponseMessage();

            int modelId;
            int stateId;
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

            modelId = string.IsNullOrEmpty(nvc["modelid"]) ? -1 : Convert.ToInt32(nvc["modelid"]);
            stateId = string.IsNullOrEmpty(nvc["stateid"]) ? -1 : Convert.ToInt32(nvc["stateid"]);
            if (modelId == -1 && stateId == -1)
            {
                response.Content = null;
                return response;
            }
            try
            {
                List<PriceQuoteCityDTO> cityList = Mapper.Map<List<City>, List<PriceQuoteCityDTO>>(_geoCitiesCacheRepo.GetPQCitiesByStateIdAndModelId(modelId, stateId));

                response.Content = new StringContent(JsonConvert.SerializeObject(cityList));

            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GeoCityController.GetPqCitiesByModelAndStateId()");
                objErr.LogException();
            }
            return response;
        }

        /// <summary>
        /// Created by  : Ashish Verma
        /// Description Gets the PQ zone name and zone id where price exists for that model by model id and cityid
        /// Date        : August 1, 2014
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetPQZonesByModelAndCityId()
        {
            var response = new HttpResponseMessage();

            int modelId;
            int cityId;
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

            modelId = string.IsNullOrEmpty(nvc["modelid"]) ? -1 : Convert.ToInt32(nvc["modelid"]);
            cityId = string.IsNullOrEmpty(nvc["cityid"]) ? -1 : Convert.ToInt32(nvc["cityid"]);
            if (modelId == -1 && cityId == -1)
            {
                response.Content = null;
                return response;
            }
            try
            {
                List<Zones> zoneList = _geoCitiesCacheRepo.GetPQCityZones(cityId, modelId);

                response.Content = new StringContent(JsonConvert.SerializeObject(zoneList));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GeoCityController.GetPqZonesByModelAndCityId()");
                objErr.LogException();
            }
            return response;
        }

        /// <summary>
        /// Created by  : Ashish Verma
        /// Description Gets the PQ popular city name and city id where price exists for that model by model id
        /// Date        : sept 22, 2014
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetPQPopularCities()
        {
            var response = new HttpResponseMessage();

            int modelId;
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

            modelId = string.IsNullOrEmpty(nvc["modelid"]) ? -1 : Convert.ToInt32(nvc["modelid"]);
            if (modelId == -1)
            {
                response.Content = null;
                return response;
            }
            try
            {
                List<PopularCity> cityList = _geoCitiesCacheRepo.GetPQPopularCities(modelId);

                response.Content = new StringContent(JsonConvert.SerializeObject(cityList));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GeoCityController.GetpqPopularCities()");
                objErr.LogException();
            }
            return response;
        }

        /// <summary>
        /// Created by  : Kirtan Shetty
        /// Description :Gets the city name for the given city id
        /// Date        : August 1, 2014
        /// </summary>
        /// <returns></returns>
        [EnableCors(origins: "http://opr.carwale.com,http://oprst.carwale.com,http://webserver:8082", headers: "*", methods: "GET")]
        public HttpResponseMessage GetCityNameById()
        {
            var response = new HttpResponseMessage();
            string cityId;

            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

            cityId = nvc["cityid"];

            if (cityId == "")
            {
                response.Content = null;
                return response;
            }

            try
            {
                string CityName = _geoCitiesCacheRepo.GetCityNameById(cityId);

                response.Content = new StringContent(JsonConvert.SerializeObject(CityName));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GeoCityController.GetCityNameById()");
                objErr.LogException();
            }
            return response;
        }

        [EnableCors(origins: "http://opr.carwale.com,http://oprst.carwale.com,http://webserver:8082,http://localhost:8081,https://cwoprst.carwale.com,https://operations.carwale.com", headers: "*", methods: "GET")]
        public HttpResponseMessage GetStates()
        {
            var response = new HttpResponseMessage();

            try
            {
                List<States> stateList = _geoCitiesCacheRepo.GetStates();

                response.Content = new StringContent(JsonConvert.SerializeObject(stateList));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GeoCityController.GetStates()");
                objErr.LogException();
            }
            return response;
        }

        [EnableCors(origins: "http://opr.carwale.com,http://oprst.carwale.com,http://webserver:8082,http://localhost:8081,https://cwoprst.carwale.com,https://operations.carwale.com", headers: "*", methods: "GET")]
        public HttpResponseMessage GetCitiesByState()
        {
            var response = new HttpResponseMessage();
            Int32 stateId;

            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            stateId = string.IsNullOrEmpty(nvc["stateid"]) ? -1 : Convert.ToInt32(nvc["stateid"]);
            if (stateId == -1)
            {
                response.Content = null;
                return response;
            }

            try
            {
                List<PriceQuoteCityDTO> cityList = Mapper.Map<List<City>, List<PriceQuoteCityDTO>>(_geoCitiesCacheRepo.GetCitiesByStateId(stateId));

                response.Content = new StringContent(JsonConvert.SerializeObject(cityList));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GeoCityController.GetStates()");
                objErr.LogException();
            }
            return response;
        }

        /// <summary>
        /// Created by  : Ashish Verma
        /// Description :Gets the PQ city name and CityId ,zone Name zoneId based on modelid
        /// Date        : Nov 29, 2014
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetPQCityZonesByModelId()
        {
            var response = new HttpResponseMessage();

            int modelId;
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

            modelId = string.IsNullOrEmpty(nvc["modelid"]) ? -1 : Convert.ToInt32(nvc["modelid"]);
            if (modelId == -1)
            {
                response.Content = null;
                return response;
            }
            try
            {
                List<CityZones> cityList = _geoLocationBL.GetPQCityZonesList(modelId);

                response.Content = new StringContent(JsonConvert.SerializeObject(cityList));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GeoCityController.GetPqCitiesByModelId()");
                objErr.LogException();
            }
            return response;
        }

        [HttpGet]
        public IHttpActionResult StateAndAllCities(int cityId)
        {
            if (cityId < 1)
            {
                return Ok<Carwale.Entity.Geolocation.StateAndAllCities>(null);
            }

            try
            {
                Carwale.Entity.Geolocation.StateAndAllCities stateAndAllCities = _geoCitiesCacheRepo.GetStateAndAllCities(cityId);

                return Json(Mapper.Map<Carwale.Entity.Geolocation.StateAndAllCities, DTOs.Geolocation.StateAndAllCities>(stateAndAllCities));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GeoCityController.StateAndAllCities(int cityId)");
                objErr.LogException();
            }
            return InternalServerError();
        }

        [Route("api/pq/cities")]
        [HttpGet]
        public IHttpActionResult GetPQCityZonesGroupsByModelId(int modelId, int cityId = 0, int stateId = 0)
        {
            if (modelId < 1)
            {
                return Ok<PQCityDTO>(null);
            }
            try
            {
                PQCityDTO citiesZones = _geoLocationBL.GetPQCitiesZones(modelId, cityId, stateId);
                return Json(citiesZones);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GeoCityController.GetPqCitiesByModelId()");
                objErr.LogException();
            }
            return InternalServerError();
        }


        [Route("api/prices/exist")]
        [HttpGet]
        public IHttpActionResult GetPriceAvailability(int modelId, int cityId)
        {
            if (modelId < 1 || cityId < 1)
            {
                return BadRequest();
            }

            return Ok(new { response = _geoLocationBL.GetPriceAvailability(modelId, cityId) });
        }

        [Route("api/cities")]
        [HttpGet, EnableCors("https://www-carwale-com.cdn.ampproject.org, https://cdn.ampproject.org, https://www-carwale-com.amp.cloudflare.com", "*", "GET")]
        public IHttpActionResult GetAllCities(string fields = null, int? count = null, Modules module = Modules.Default, bool? isPopular = null)
        {
            try
            {
                if (fields != null || module != Modules.Default || isPopular != null || count != null)
                {
                    if (count <= 0)
                    {
                        ModelState.AddModelError("count", "Count should be greater than 0");
                    }
                    if (!ModelState.IsValid)
                    {
                        return BadRequest(ModelState);
                    }

                    var cities = _geoCitiesCacheRepo.GetCities(module, isPopular);

                    if (cities != null && cities.Any())
                    {
                        if (count > 0)
                        {
                            cities = cities.Take(count.Value);
                        }

                        var settings = new JsonSerializerSettings
                        {
                            ContractResolver = new CustomPropertiesContractResolver(fields),
                            NullValueHandling = NullValueHandling.Ignore
                        };
                        return Json(cities, settings);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return Ok(Mapper.Map<IEnumerable<Cities>, IEnumerable<CitiesDTO>>(_geoCityCache.GetAll()));
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }

        [Route("api/v2/pq/cities")]
        [HttpGet]
        public IHttpActionResult GetPQCitiesByModelId(int modelId, int groupId = 0, int stateId = 0)
        {
            if (modelId < 1)
            {
                return NotFound();
            }
            try
            {
                var pqCitiesZones = _geoLocationBL.GetPQCitiesZonesV2(modelId, groupId, stateId);
                var cities = JsonConvert.SerializeObject(pqCitiesZones, Formatting.Indented,
                    new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });

                return Json(JsonConvert.DeserializeObject(cities));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GeoCityController.GetPqCitiesByModelId()");
                objErr.LogException();
            }
            return InternalServerError();
        }

        [HttpGet]
        public IHttpActionResult GetStateByCityId(int cityId)
        {
            if (cityId < 1)
            {
                return Ok<States>(null);
            }

            try
            {
                var stateDetails = _geoCitiesCacheRepo.GetStateByCityId(cityId);

                return Json(stateDetails);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GeoCityController.GetStateByCityId(int cityId)");
                objErr.LogException();
            }
            return InternalServerError();
        }

        [Route("api/nearby/cities")]
        [HttpGet]
        public IHttpActionResult GetNearByCities(int cityId, short count)
        {
            if (cityId < 1 || count < 1)
            {
                return NotFound();
            }
            try
            {
                var cities = _geoCitiesCacheRepo.GetNearestCities(cityId, count);
                return Ok(Mapper.Map<IEnumerable<City>, IEnumerable<CitiesDTO>>(cities));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "GeoCityController.GetNearByCities()");
                objErr.LogException();
            }
            return InternalServerError();
        }

        [Route("api/city/{Id:min(1)}")]
        [HttpGet]
        public IHttpActionResult GetCityNameById(int Id)
        {
            IPToLocationDTO cityDetails = new IPToLocationDTO();
            try
            {
                cityDetails = AutoMapper.Mapper.Map<Cities, IPToLocationDTO>(_geoCitiesCacheRepo.GetCityDetailsById(Id));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "GeoCityController.GetCityNameById()");
            }
            return Ok(cityDetails);
        }

        [Route("api/cities/{Id:min(1)}/zones/")]
        [EnableCors(origins: "http://opr.carwale.com,http://oprst.carwale.com,http://webserver:8082", headers: "*", methods: "GET")]
        [HttpGet]
        public IHttpActionResult GetZonesByCity(int id)
        {
            List<DTOs.Geolocation.Zone> zoneList = new List<DTOs.Geolocation.Zone>();
            try
            {
                zoneList = Mapper.Map<IEnumerable<Entity.Geolocation.Zone>, List<DTOs.Geolocation.Zone>>(_geoCitiesCacheRepo.GetZonesByCity(id));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, System.Reflection.MethodBase.GetCurrentMethod().Name);
            }
            return Ok(zoneList);
        }

        [Route("api/state/{Id}/groupcities/")]
        [EnableCors(origins: "http://opr.carwale.com,http://oprst.carwale.com,http://webserver:8082", headers: "*", methods: "GET")]
        [HttpGet]
        public IHttpActionResult GetGroupCities(int id)
        {
            if (id < -1)
            {
                return BadRequest();
            }
            try
            {
                var groupCities = _geoLocationBL.GetGroupCities(id == 0 ? -1 : id);
                return Json(groupCities);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }

        [Route("api/state/{Id}/zones/")]
        [EnableCors(origins: "http://opr.carwale.com,http://oprst.carwale.com,http://webserver:8082", headers: "*", methods: "GET")]
        [HttpGet]
        public IHttpActionResult GetZonesByState(int id)
        {
            if (id < -1)
            {
                return BadRequest();
            }
            try
            {
                var zonesbyState = _geoLocationBL.GetZonesbyState(id == 0 ? -1 : id);
                return Json(zonesbyState);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }

        [HandleException]
        public IHttpActionResult GetCityFromLatLong(string latitude, string longitude)
        {
            var citySuggestion = _geoCityLogic.GetCityFromLatLong(latitude, longitude)?.Payload as DTOs.CityResultsDTO;
            if (citySuggestion == null)
            {
                return NotFound();
            }
            return Content(HttpStatusCode.OK, new { cityId = citySuggestion.Id, cityName = citySuggestion.Name });
        }

        [HandleException]
        public IHttpActionResult GetCityAndArea(string latitude, string longitude)
        {
            var objFromAPI = LocationFinder.GetAddressByLatLong(latitude, longitude);
            if (objFromAPI == null || (string.IsNullOrWhiteSpace(objFromAPI.CityName) && string.IsNullOrWhiteSpace(objFromAPI.StateName)))
            {
                return NotFound();
            }
            NameValueCollection nvcforCitySuggesstion = new NameValueCollection
            {
                ["term"] = objFromAPI.CityName
            };
            var citySuggestion = _autoComplete_v1.GetCitySuggestion(nvcforCitySuggesstion);
            if (!citySuggestion.IsNotNullOrEmpty())
            {
               return NotFound();
            }
            DTOs.CityResultsDTO dto;
            dto = citySuggestion.Count > 1
                ? Mapper.Map<DTOs.CityResultsDTO>(
                     citySuggestion
                         .Where(stateName => stateName.Result.Split(',').Last().Trim().Equals(objFromAPI.StateName, StringComparison.OrdinalIgnoreCase))
                         .Select(payload => payload.Payload).First())
                : Mapper.Map<DTOs.CityResultsDTO>(citySuggestion[0].Payload);
            if (dto == null)
            {
                return NotFound();
            }
            Platform platform;
            Enum.TryParse(Request.Headers.GetValues("sourceId").First(), out platform);
            NameValueCollection nvcforAreaSuggesstion = new NameValueCollection
            {
                ["term"] = objFromAPI.Pincode,
                ["cityId"] = dto.Id.ToString(),
                ["record"] = "5",
                ["size"] = "5",
                ["SourceId"] = ((int)platform).ToString(),
                ["showFeaturedCar"] = "false"
            };
            var areaSuggestion = _autoComplete_v1.GetAreaSuggestion(nvcforAreaSuggesstion);
            var areaDTO = areaSuggestion.Select(x => x.Payload as DTOs.AreaResultsDTO);
            return Content(HttpStatusCode.OK, new { cityId = dto.Id, cityName = dto.Name, areas = areaDTO }, _jsonFormatter);
        }
    }
}
