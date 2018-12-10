using AutoMapper;
using Carwale.BL.Dealers;
using AEPLCore.Cache;
using Carwale.Cache.Dealers;
using Carwale.DAL.Dealers;
using Carwale.DTOs.Dealer;
using Carwale.DTOs.Dealers;
using Carwale.DTOs.NewCars;
using Carwale.Entity.Dealers;
using Carwale.Entity.Enum;
using Carwale.Interfaces;
using Carwale.Interfaces.Dealers;
using Carwale.Notifications;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
//using Microsoft.AspNet.WebApi.WebHost;

namespace Carwale.Service.Controllers
{
    public class NewCarDealersController : ApiController
    {

        private readonly IUnityContainer _container;
        private readonly INewCarDealers _newCarDealers;
        private readonly INewCarDealersCache _newCarDealersCacheRepo;
        private readonly IDealers _dealersBL;
        public NewCarDealersController(IUnityContainer container, INewCarDealers newCarDealers, INewCarDealersCache newCarDealersCacheRepo, IDealers dealersBL)
        {
            _container = container;
            _newCarDealers = newCarDealers;
            _newCarDealersCacheRepo = newCarDealersCacheRepo;
            _dealersBL = dealersBL;
        }
        /// <summary>
        /// Populates the list of dealers based on cityid and makeid passed in querystring
        /// Written By : Supriya on 29/5/2014
        /// </summary>
        /// <returns></returns>
        
        [HttpGet]
        [ActionName("showrooms")]
        public HttpResponseMessage GetNewCarDealers(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage();
            try
            {
                NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                if (!ValidateDealerRequest(ref nvc))
                {
                    response.Content = new StringContent("Bad Request");
                    return response;
                }

                int stateId = GetStateId(nvc);
                int cityId = Convert.ToInt32(nvc["cityId"]);
                int makeId = Convert.ToInt32(nvc["makeId"]);

                NewCarDealerEntiy dealersList = _newCarDealers.GetDealersList(stateId, cityId, makeId);

                if (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && request.Headers.Contains("appversion") == true)
                {
                    var source = request.Headers.GetValues("SourceId");
                    string platform = string.Join(",", source);
                    int sourceId = Convert.ToInt32(platform.Split(',')[0]);

                    var app = request.Headers.GetValues("appVersion");
                    string appV = string.Join(",", app);
                    int appVersion = Convert.ToInt32(appV.Split(',')[0]);

                    if ((sourceId.Equals(Convert.ToInt16(Platform.CarwaleiOS)) && appVersion <= 19) || (sourceId.Equals(Convert.ToInt16(Platform.CarwaleAndroid)) && appVersion <= 96))
                    {
                        dealersList.NewCarDealers.ForEach(x => x.ShowroomImage = x.ShowroomImage.Replace("https://", ""));
                    }
                }

                response.Content = new StringContent(JsonConvert.SerializeObject(dealersList));
                response.Content.Headers.Expires = DateTimeOffset.Now.AddMinutes(10).ToUniversalTime();
                response.Headers.CacheControl = new System.Net.Http.Headers.CacheControlHeaderValue() { Private = true, MaxAge = new TimeSpan(0, 0, 6000) };
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealersController.GetNewCarDealers()");
                objErr.LogException();
            }
            return response;
        }

        /// <summary>
        /// To provide Dealer API to BCG client
        /// Written By : Chetan on 04/09/2018
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("showroomdetails")]
        public HttpResponseMessage GetNewCarDealerDetails(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage();
            try
            {
                NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                if (!ValidateDealerRequest(ref nvc))
                {
                    response.Content = new StringContent("Bad Request");
                    return response;
                }

                int stateId = GetStateId(nvc);
                int cityId = Convert.ToInt32(nvc["cityId"]);
                int makeId = Convert.ToInt32(nvc["makeId"]);

                NewCarDealerEntiy dealersList = _newCarDealers.GetDealersList(stateId, cityId, makeId);
                var showroomsdetails = Mapper.Map<List<NewCarDealer>, List<DealerDetailsV1Dto>>(dealersList.NewCarDealers);

                response.Content = new StringContent(JsonConvert.SerializeObject(showroomsdetails));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealersController.GetNewCarDealerDetails()");
                objErr.LogException();
            }
            return response;
        }


        /// <summary>
        /// Populates the list of cities & popularcities based on makeidId paased in querystring
        /// Written By : Supriya on 29/5/2014
        /// Modification: Removed '[AthunticateBasic]' attribute
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("cities")]
        public HttpResponseMessage GetCitiesByMake()
        {
            var response = new HttpResponseMessage();
            try
            {
                NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                if (!RegExValidations.IsPositiveNumber(nvc["makeId"].ToString()))
                {
                    response.Content = new StringContent("Bad Request");
                    return response;
                }

                int makeId = Convert.ToInt32(nvc["makeId"]);

                response.Content = new StringContent(JsonConvert.SerializeObject(_newCarDealers.GetCitiesByMake(makeId)));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealersController.GetCitiesByMake()");
                objErr.LogException();
            }
            return response;
        }

        /// <summary>
        /// Populates the list of about us images based on dealer id passed
        /// Written By : Supriya on 29/5/2014
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ActionName("images")]
        public HttpResponseMessage GetAboutUsImages(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage();
            try
            {
                NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                if (!RegExValidations.IsPositiveNumber(nvc["dealerId"].ToString()))
                {
                    response.Content = new StringContent("Bad Request");
                    return response;
                }

                int dealerId = Convert.ToInt32(nvc["dealerId"]);

                INewCarDealers imgList = _container.Resolve<INewCarDealers>();
                int sourceId = 0, appVersion = 0;
                if (request.Headers.Contains("CWK") == true && request.Headers.Contains("SourceId") == true && request.Headers.Contains("appversion") == true && !string.IsNullOrEmpty(request.Headers.GetValues("SourceId").ToString()) && !string.IsNullOrEmpty(request.Headers.GetValues("appVersion").ToString()))
                {
                    var source = request.Headers.GetValues("SourceId");

                    if (!string.IsNullOrEmpty(source.ToString()))
                    {
                        string platform = string.Join(",", source);
                        sourceId = Convert.ToInt32(platform.Split(',')[0]);
                    }

                    var app = request.Headers.GetValues("appVersion");
                    if (!string.IsNullOrEmpty(app.ToString()))
                    {
                        string appV = string.Join(",", app);
                        appVersion = Convert.ToInt32(appV.Split(',')[0]);
                    }
                }

                if (sourceId.Equals(Convert.ToInt16(Platform.CarwaleiOS)) && appVersion <= 13)
                {
                    Mapper.CreateMap<AboutUsImageEntity, NewCarDealerImages>()
                                                                       .ForMember(src => src.ImgThumbUrl, dest => dest.MapFrom(y => y.ImgThumbUrl))
                                                                       .ForMember(src => src.ImgLargeUrl, dest => dest.MapFrom(y => y.ImgLargeUrl));

                    List<AboutUsImageEntity> content = (List<AboutUsImageEntity>)_dealersBL.GetDealerImages(dealerId);

                    List<NewCarDealerImages> objDealerShowroomImages = Mapper.Map<List<AboutUsImageEntity>, List<NewCarDealerImages>>(content);
                    response.Content = new StringContent(JsonConvert.SerializeObject(objDealerShowroomImages));
                }
                else
                {

                    response.Content = new StringContent(JsonConvert.SerializeObject(_dealersBL.GetDealerImages(dealerId)));
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealersController.Images()");
                objErr.LogException();
            }
            return response;
        }


        /// <summary>
        /// Populates the list of makes based on cityId paased in querystring
        /// Written By : Supriya on 29/5/2014
        /// Modified By : Rohan S on 22-08-2014
        /// Modification: Removed '[AthunticateBasic]' attribute
        /// Modifications : Commented method 
        /// </summary>
        /// <returns></returns>
        public HttpResponseMessage GetMakesByCity()
        {
            var response = new HttpResponseMessage();
            try
            {
                NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                if (!RegExValidations.IsPositiveNumber(nvc["cityId"].ToString()))
                {
                    response.Content = new StringContent("Bad Request");
                    return response;
                }

                int cityId = Convert.ToInt32(nvc["cityId"]);

                response.Content = new StringContent(JsonConvert.SerializeObject(_newCarDealers.GetMakesByCity(cityId)));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealersController.GetMakesByCity()");
                objErr.LogException();
            }
            return response;
        }

        /// <summary>
        /// Populates the list of dealers on make and count
        /// Written By : Vinayak on 23/2/2015
        /// </summary>
        [HttpGet]
        [ActionName("ncdmakecount")]
        public HttpResponseMessage GetMakesAndCount()
        {
            var response = new HttpResponseMessage();
            try
            {
                NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

                if (string.IsNullOrEmpty(nvc["type"].ToString()))
                {
                    response.Content = new StringContent("bad Request");
                    return response;
                }

                string type = Convert.ToString(nvc["type"]);

                response.Content = new StringContent(JsonConvert.SerializeObject(_newCarDealers.GetMakesAndCount(type)));
            }
            catch (Exception ex)
            {
                ExceptionHandler err = new ExceptionHandler(ex, "NewCarDealersController.GetMakesAndCount()");
                err.LogException();
            }
            return response;
        }

        /// <summary>
        /// Populates the dealer details
        /// Written By : Vinayak on 23/2/2015
        /// Modified By : Shalini on 10/03/2015
        /// </summary>
        [HttpGet]
        [ActionName("ncddetails")]
        public HttpResponseMessage GetDealerDetail()
        {
            DealerShowroomDetails content = null;
            var response = new HttpResponseMessage();
            try
            {
                NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                int makeId = 0;
                if (!RegExValidations.IsPositiveNumber(nvc["dealerId"].ToString()))
                {
                    response.Content = new StringContent("bad Request");
                    return response;
                }
                if (RegExValidations.IsPositiveNumber(CustomParser.parseStringObject(nvc["makeId"])))
                    makeId = CustomParser.parseIntObject((nvc["makeId"]));

                int dealerId = Convert.ToInt32(nvc["dealerId"]);
                content = _newCarDealers.GetDealerDetails(dealerId, null, null, makeId);
            }
            catch (Exception ex)
            {
                ExceptionHandler err = new ExceptionHandler(ex, "NewCarDealersController.GetDealerDetail()");
                err.LogException();
            }

            return Request.CreateResponse<DealerShowroomDetails>(HttpStatusCode.OK, content);
        }

        /// <summary>
        /// Populates the dealer details
        /// Written By : Vicky Lund on 18/2/2016
        /// Created new version without Serializable class
        /// </summary>
        [HttpGet, Route("api/NewCarDealers/GetDealerDetails/")]
        public IHttpActionResult GetDealerDetails(int dealerId, int campaignId, int cityId, int makeId)
        {
            DealerShowroomDetailsDTO content = null;
            var response = new HttpResponseMessage();
            try
            {
                if (!RegExValidations.IsPositiveNumber(dealerId.ToString()))
                    return BadRequest();

                if (!RegExValidations.IsPositiveNumber(makeId.ToString()))
                    makeId = 0;

                if (!RegExValidations.IsPositiveNumber(campaignId.ToString()))
                    campaignId = 0;

                if (!RegExValidations.IsPositiveNumber(cityId.ToString()))
                    cityId = 0;

                DealerShowroomDetails dealerShowroomDetails = _newCarDealers.GetDealerDetails(dealerId, campaignId, cityId, makeId);
                if (dealerShowroomDetails == null)
                    return NotFound();

                content = Mapper.Map<DealerShowroomDetails, DealerShowroomDetailsDTO>(dealerShowroomDetails);
            }
            catch (Exception ex)
            {
                ExceptionHandler err = new ExceptionHandler(ex, "NewCarDealersController.GetDealerDetail()_v2");
                err.LogException();
            }
            return Ok(content);
        }

        /// <summary>
        /// For getting Models related to dealer
        /// Created: Chetan Thambad, 03/02/2016
        /// </summary>
        /// <returns></returns>
        [EnableCors(origins: "http://opr.carwale.com,http://oprst.carwale.com,http://webserver:8082,,http://localhost:8082", headers: "*", methods: "GET")]
        [HttpGet, Route("api/NewCarDealers/GetDealerModels/")]
        public IHttpActionResult GetDealerModels(int dealerId)
        {
            var dealerSpecificModels = _newCarDealers.DealerModelListBl(dealerId);
            return Ok(dealerSpecificModels);
        }

        /// <summary>
        /// This api returns the list of NCS dealers of particular model and city
        /// </summary>
        /// <param name="modelId"></param>
        /// <param name="cityId"></param>
        /// <returns></returns>
        [EnableCors(origins: "http://localhost,https://localhost,http://test.cartrade.com,https://test.cartrade.com,http://testm.cartrade.com,"
            + "https://testm.cartrade.com,http://testapi.cartrade.com,https://testapi.cartrade.com,http://www.cartrade.com,https://www.cartrade.com,"
            + "http://m.cartrade.com,https://m.cartrade.com,,http://api.cartrade.com,https://api.cartrade.com", headers: "*", methods: "GET")]
        [HttpGet,Route("api/dealers/ncs/")]
        public IHttpActionResult GetNCSDealers(int modelId = -1, int cityId = -1, int campaignId = -1)
        {
            if (modelId < 1 || cityId < 1 ) 
                return BadRequest();
            try
            {
                return Json(Mapper.Map<IEnumerable<NewCarDealersList>, IEnumerable<DealerDTO>>(_newCarDealers.GetNcsDealers(modelId, cityId, campaignId)));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "NewCarDealersController.GetNCSDealers");
                objErr.LogException();
                return InternalServerError();
            }
        }

        private bool ValidateDealerRequest(ref NameValueCollection nvc)
        {
            if (!(
                        RegExValidations.IsPositiveNumber(nvc["makeId"].ToString()) &&
                        (
                            (
                                RegExValidations.IsPositiveNumber(nvc["cityId"].ToString()) ||
                                (
                                    nvc["stateId"] != null &&
                                    RegExValidations.IsPositiveNumber(nvc["stateId"].ToString())

                                )
                            ) ||
                            (
                                nvc["allStates"] != null &&
                                nvc["allStates"].ToString() == "1"
                            )
                        )
                    ))
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        private int GetStateId(NameValueCollection nvc)
        {
            int stateId = -1;
            if (!String.IsNullOrWhiteSpace(nvc["stateId"]))
                stateId = Convert.ToInt32(nvc["stateId"]);

            if (nvc["allStates"] != null && nvc["allStates"].ToString() == "1")
            {
                stateId = -1;
            }

            return stateId;
        }
    }
}
