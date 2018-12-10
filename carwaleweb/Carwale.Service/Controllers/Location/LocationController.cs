using AutoMapper;
using Carwale.BL.IPToLocation;
using Carwale.DTOs;
using Carwale.DTOs.Elastic.Autocomplete.Area;
using Carwale.DTOs.Geolocation;
using Carwale.DTOs.IPToLocation;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Geolocation.LatLongURI;
using Carwale.Entity.IPToLocation;
using Carwale.Interfaces.Geolocation;
using Carwale.Notifications;
using Carwale.Notifications.Logs;
using Carwale.Service.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Carwale.Service.Controllers
{
    public class LocationController : ApiController
    {
        protected readonly IPToLocation _ipToLocation;
        private readonly IGeoCitiesCacheRepository _geoCitiesCacheRepo;
        private readonly IGeoCityLogic _geoCityLogic;
        private static readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver(),
            NullValueHandling = NullValueHandling.Ignore
        };

        public LocationController(IPToLocation ipToLocation, IGeoCitiesCacheRepository geoCitiesCacheRepo, IGeoCityLogic geoCityLogic)
        {
            _ipToLocation = ipToLocation;
            _geoCitiesCacheRepo = geoCitiesCacheRepo;
            _geoCityLogic = geoCityLogic;
        }
        /// <summary>
        /// citybylatlong api 
        /// written by Natesh kumar on 5/11/14
        /// Modified By : Shalini Nair on 14/05/2015
        /// </summary>
        [HttpGet]
        [LatLongApiValidator]
        public HttpResponseMessage Area([FromUri]LatLongURI queryString, [FromUri] string desc)
        {
            var response = new HttpResponseMessage();

            try
            {
                Carwale.BL.GeoLocation.ElasticLocation BL = new Carwale.BL.GeoLocation.ElasticLocation();

                Area area = BL.GetLocation(Convert.ToDouble(queryString.Latitude), Convert.ToDouble(queryString.Longitude));

                //if no results are found 
                if (area.cityid < 1 || string.IsNullOrEmpty(area.cityname))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "No results found");
                }

                response.Content = new StringContent(JsonConvert.SerializeObject(area));

            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "LocationController.Area()");
                objErr.LogException();

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
            }

            return response;
        }

        /// <summary>
        /// citybylatlong api for get data based on areaid 
        /// written by Jitendra singh on 03/30/16
        /// </summary>
        [HttpGet]
        public HttpResponseMessage Area([FromUri] int id)
        {
            var response = new HttpResponseMessage();

            try
            {
                if (id > 0)
                {
                    Carwale.BL.GeoLocation.ElasticLocation BL = new Carwale.BL.GeoLocation.ElasticLocation();

                    Area area = BL.GetLocation(id);

                    //if no results are found 
                    if (area == null || area.cityid < 1 || string.IsNullOrEmpty(area.cityname))
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NoContent, "No results found");
                    }

                    response.Content = new StringContent(JsonConvert.SerializeObject(area));
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Invalid Request");
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "LocationController.Area()");
                objErr.LogException();

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
            return response;
        }

        /// <summary>
        /// get city based on IPToLocation
        /// written by Sachin Bharti on 02/05/16
        /// </summary>
        [HttpGet]
        public HttpResponseMessage GetIPToLocationCity()
        {
            var response = new HttpResponseMessage();
            try
            {
                IPToLocationEntity location = _ipToLocation.GetCity();
                //if no results are found 
                if (location == null || location.CityId < 1 || string.IsNullOrEmpty(location.CityName))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "No results found");
                }

                response.Content = new StringContent(JsonConvert.SerializeObject(location));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "LocationController.GetIPToLocationCity()");
                objErr.LogException();

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
            return response;
        }

        /// <summary>
        /// get city based on IPToLocation
        /// written by jitendra singh on 02/05/16
        /// </summary>
        [HttpGet, Route("api/v1/locations/ipcity/")]
        public HttpResponseMessage GetIPToLocationCity_v1()
        {
            var response = new HttpResponseMessage();
            try
            {
                IPToLocationDTO location = _ipToLocation.GetCity_v1();
                //if no results are found 
                if (location == null || location.CityId < 1 || string.IsNullOrEmpty(location.CityName))
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NoContent, "No results found");
                }

                response.Content = new StringContent(JsonConvert.SerializeObject(location));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "LocationController.GetIPToLocationCity_v1()");
                objErr.LogException();

                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Internal Server Error");
            }
            return response;
        }
        /// <summary>
        /// get areacode based on cityid
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>

        [HttpGet, Route("api/locations/areacode/")]
        public IHttpActionResult GetAreaCodeByCity(int cityId)
        {
            try
            {
                if(cityId <= 0)
                {
                    return BadRequest();
                }

                List<AreaCode> areaCodes = _geoCitiesCacheRepo.GetAreaCodeByCity(cityId);
                if (areaCodes == null || areaCodes.Count == 0)
                    return NotFound();

                return Json(areaCodes);
            }
            catch (Exception ex)
            {
                Logger.LogException(ex);
                return InternalServerError();
            }
        }

        [HttpGet, Route("api/locations/getnearestareas/"), HandleException, LatLongApiValidator]
        public IHttpActionResult GetNearestAreas(double latitude, double longitude, int noOfRecords = 1)
        {
            var result = BL.GeoLocation.ElasticLocation.GetNearestAreas(latitude, longitude, noOfRecords);
            List<AreaPayLoad> listOfAreas = new List<AreaPayLoad>();
            foreach (var item in result)
            {
                listOfAreas.Add(item.payload);
            }
            return Ok(listOfAreas);
        }

        [HttpGet, Route("api/locations/nearest/"), HandleException, LatLongApiValidator]
        public IHttpActionResult GetNearestLocations(double latitude, double longitude, int noOfRecords = 1)
        {
            var citySuggestions = _geoCityLogic.GetCityFromLatLong(latitude.ToString(), longitude.ToString())?.Payload as CityResultsDTO;
            if (citySuggestions != null)
            {
                List<LocationsDTO> listOfLocations;
                if (citySuggestions.IsAreaAvailable)
                {
                    var result = BL.GeoLocation.ElasticLocation.GetNearestAreas(latitude, longitude, noOfRecords);
                    listOfLocations = Mapper.Map<List<LocationsDTO>>(result.Select(x => x.payload));
                }
                else
                {
                    listOfLocations = new List<LocationsDTO> {
                                            Mapper.Map<LocationsDTO>(citySuggestions)
                                        };
                }
                return Json(listOfLocations, _serializerSettings);
            }
            return Content(HttpStatusCode.NotFound, "Your location was not found!");
        }
    }
}
