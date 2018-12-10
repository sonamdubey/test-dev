using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Carwale.Notifications;
using Newtonsoft.Json;
using Carwale.DAL.PriceQuote;
using Carwale.Interfaces.PriceQuote;
using Carwale.BL.PriceQuote;
using Carwale.Interfaces.Dealers;
using Carwale.DAL.Dealers;
using AEPLCore.Cache;
using Carwale.Interfaces;
using System.Collections.Specialized;
using System.Web;
using System.Web.Http;
using Carwale.Interfaces.CarData;
using Carwale.DAL.CarData;
using Carwale.BL.Dealers;
using Carwale.Entity.PriceQuote;
using Carwale.Interfaces.Geolocation;
using Carwale.DAL.Geolocation;
using Carwale.DTOs.PriceQuote;
using Carwale.Cache.PriceQuote;
using Carwale.DTOs.CarData;
using Carwale.Entity.Enum;
using Carwale.Service.Filters;
using Carwale.Cache.Geolocation;
using Carwale.Cache.CarData;
using Carwale.Cache.Dealers;
using System.Linq;
using Carwale.Utility;
using AutoMapper;
using Carwale.Interfaces.Campaigns;
using Carwale.Entity;
using Carwale.DTOs;
using Carwale.DTOs.Geolocation;
using Carwale.Notifications.Logs;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Price;
using Carwale.Interfaces.Validations;
using System.Net;

namespace Carwale.Service.Controllers
{
    public class PQController : ApiController
    {
        private readonly IPrices _prices;
        private readonly IUnityContainer _container;
        private readonly ICampaignRecommendationsBL _campaignRecoBl;
        private readonly IValidateMmv _validateMmv;
        private readonly INearbyCitiesSearch _nearByCitiesSearchLogic;
        private readonly ICarVersionCacheRepository _carVersionCacheRepo;
        private readonly ICarModelCacheRepository _carModelCacheRepo;
        private readonly List<int> _vwfsMakeIds = Array.ConvertAll(System.Configuration.ConfigurationManager.AppSettings["VWFSMakeIds"].Split(','), int.Parse).ToList<int>();
        private readonly int _abTestMaxValForNewPq = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AbTestMaxValForNewPqDesktop"]);
        private readonly int _abTestMinValForNewPq = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["AbTestMinValForNewPqDesktop"]);

        public PQController(IUnityContainer unityContainer, IPrices prices, ICampaignRecommendationsBL campaignRecoBl,
            IValidateMmv validateMmv, INearbyCitiesSearch nearByCitiesSearchLogic, ICarVersionCacheRepository carVersionCacheRepository,
            ICarModelCacheRepository carModelCacheRepo)
        {
            _container = unityContainer;
            _prices = prices;
            _campaignRecoBl = campaignRecoBl;
            _validateMmv = validateMmv;
            _nearByCitiesSearchLogic = nearByCitiesSearchLogic;
            _carVersionCacheRepo = carVersionCacheRepository;
            _carModelCacheRepo = carModelCacheRepo;
        }

        /// <summary>
        /// Controller for Getting price quote and car,city info on refresh based on PQIds
        /// Written By : Ashish Verma on 2/6/2014
        /// </summary>
        /// <param name="type"></param>/// <returns></returns>
        public HttpResponseMessage GetById()
        {
            var response = new HttpResponseMessage();

            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            string enIds = (nvc["pqids"] != null && !String.IsNullOrEmpty(nvc["pqids"].ToString())) ? nvc["pqids"].ToString() : "";
            int sourceId = (nvc["sourceid"] != null && !String.IsNullOrEmpty(nvc["sourceid"])) ? Convert.ToInt16(nvc["sourceid"]) : 0;
            string userIdentifier = (sourceId == (int)Platform.CarwaleiOS || sourceId == (int)Platform.CarwaleAndroid) ? HttpContext.Current.Request.Headers["IMEI"] : UserTracker.GetSessionCookie();
            if (sourceId <= 0)
            {
                response.Content = null;
                return response;
            }

            else
            {
                try
                {
                    if (sourceId == (int)Platform.CarwaleDesktop)
                    {
                        bool IsValidPqIds = Utility.RegExValidations.ValidateCommaSeperatedNumbers(enIds);
                        if (!IsValidPqIds)
                        {
                            response.Content = null;
                            return response;
                        }
                        List<ulong> pqIdsList = enIds.Split(',').Select(ulong.Parse).ToList();

                        var pq = _container.Resolve<IPQAdapter>("pqDesktopAdapterv1");

                        var pqList = pq.GetPQByIds<PQDesktop>(pqIdsList);

                        response.Content = new StringContent(JsonConvert.SerializeObject(pqList));
                    }

                }
                catch (Exception ex)
                {
                    ExceptionHandler objErr = new ExceptionHandler(ex, "PriceQuoteController.GetById()");
                    objErr.LogException();
                }
            }

            return response;
        }

        /// <summary>
        /// Controller for Getting price quote and car,city info  based on CustInfo
        /// Written By : Ashish Verma on 2/6/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public HttpResponseMessage PostPQ([FromBody] PQInput pqInput)
        {
            var response = new HttpResponseMessage();

            try
            {
                string userIdentifier = (pqInput.SourceId == (int)Platform.CarwaleiOS || pqInput.SourceId == (int)Platform.CarwaleAndroid) ?
                                        HttpContext.Current.Request.Headers["IMEI"] : UserTracker.GetSessionCookie();
                if (string.IsNullOrWhiteSpace(pqInput.Ltsrc))
                {
                    pqInput.Ltsrc = CommonLTS.CookieLTS != "-1" ? CommonLTS.CookieLTS.Split(':')[0] : "-1";
                }

                IUnityContainer container = new UnityContainer();
                if (pqInput.CityId > 0 && (pqInput.CarVersionId > 0 || pqInput.CarModelId > 0))
                {
                    if (pqInput.SourceId == (int)Platform.CarwaleAndroid || pqInput.SourceId == (int)Platform.CarwaleiOS)
                    {
                        var pq = _container.Resolve<IPQAdapter>("pqAndroidAdapter");

                        var pqList = pq.GetPQ<PQAndroid>(pqInput, userIdentifier);

                        response.Content = new StringContent(JsonConvert.SerializeObject(pqList));
                    }
                    else if (pqInput.SourceId == (int)Platform.CarwaleDesktop)
                    {
                        var pq = _container.Resolve<IPQAdapter>("pqDesktopAdapterv1");

                        var pqList = pq.GetPQ<PQDesktop>(pqInput, userIdentifier);

                        response.Content = new StringContent(JsonConvert.SerializeObject(pqList));
                    }
                }
                else
                {
                    response.Content = null;
                    return response;
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PriceQuoteController.PostPQ() for PQInputs of cityId :" + pqInput.CityId + " and VersionId : " + pqInput.CarVersionId);
                objErr.LogException();
            }
            return response;

        }

        /// <summary>
        /// Controller for Getting price quote and car,city info  based on CityId and versionId(this controller not inserts
        /// new entry into NewcarPurchageInquries table )
        /// Written By : Ashish Verma on 23/9/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public HttpResponseMessage GetPQ()
        {
            var response = new HttpResponseMessage();

            int versionId, cityId;
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

            versionId = string.IsNullOrEmpty(nvc["versionid"].ToString()) ? -1 : Convert.ToInt32(nvc["versionid"]);
            cityId = string.IsNullOrEmpty(nvc["cityid"].ToString()) ? -1 : Convert.ToInt32(nvc["cityid"]);

            if (versionId == -1 || cityId == -1 || string.IsNullOrEmpty(cityId.ToString()) || string.IsNullOrEmpty(versionId.ToString()))
            {
                response.Content = null;
                return response;
            }

            try
            {

                var pq = _container.Resolve<IPQAdapter>("pqDesktopAdapterv1");

                var pqList = pq.GetPQ(cityId, versionId);

                response.Content = new StringContent(JsonConvert.SerializeObject(pqList));

            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PriceQuoteController.GetPQ()");
                objErr.LogException();
            }
            return response;

        }

        [Route("api/v1/pq/version/{versionId}/prices/")]
        public IHttpActionResult GetVersionPQ(int versionId, int cityId)
        {
            Prices response = null;
            try
            {
                if (versionId == -1 || cityId == -1 || string.IsNullOrEmpty(cityId.ToString()) || string.IsNullOrEmpty(versionId.ToString()))
                    return BadRequest();
                response = _prices.GetVersionPQ(cityId, versionId);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PriceQuoteController.GetPQ()");
                objErr.LogException();
                return InternalServerError();
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("api/v1/pq/prices/")]
        public IHttpActionResult GetModelPrices(int modelId, int cityId)
        {
            var modelPricesDTO = _prices.GetOnRoadPrice(modelId, cityId);

            if (modelPricesDTO == null)
                return NotFound();

            return Ok(modelPricesDTO);
        }

        ///<summary>
        ///Controller for showing recommended models on PQ popup which comes after clicking of add another car
        ///Written By: Chetan T on 29/07/2016
        ///</summary>
        ///
        [HttpGet]
        [Route("api/pq/recommendedCars")]
        public IHttpActionResult GetRecoModelsForPQPopup(string userHistory, int refModelId, int noOfReco, int cityId, int platformId, int? zoneId)
        {
            string cwc = (platformId == (int)Platform.CarwaleiOS || platformId == (int)Platform.CarwaleAndroid) ? HttpContext.Current.Request.Headers["IMEI"] : UserTracker.GetSessionCookie();

            zoneId = zoneId == null ? 0 : zoneId;
            var loc = new Location { CityId = cityId, ZoneId = CustomParser.parseIntObject(zoneId) };

            var pqPopupRecoModels = _campaignRecoBl.GetPQRecommendations(cwc, userHistory, refModelId, noOfReco, loc, platformId);

            return Ok(Mapper.Map<List<MakeModelEntity>, List<CarEntityDTO>>(pqPopupRecoModels));
        }



        /// <summary>
        /// Controller for Getting price quote and car,city info  based on CustInfo
        /// Written By : vinayak
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [Route("v1/api/pq/")]
        public HttpResponseMessage PostPQV1([FromBody] PQInput pqInput)
        {
            var response = new HttpResponseMessage();

            try
            {
                string userIdentifier = (pqInput.SourceId == (int)Platform.CarwaleiOS || pqInput.SourceId == (int)Platform.CarwaleAndroid) ?
                                        HttpContext.Current.Request.Headers["IMEI"] : UserTracker.GetSessionCookie();
                int abTestCookieValue = CustomerCookie.AbTest;

                if (string.IsNullOrWhiteSpace(pqInput.Ltsrc))
                {
                    pqInput.Ltsrc = CommonLTS.CookieLTS != "-1" ? CommonLTS.CookieLTS.Split(':')[0] : "-1";
                }

                if (pqInput.CityId <= 0 || (pqInput.CarVersionId <= 0 && pqInput.CarModelId <= 0)
                   || (pqInput.CarModelId > 0 && !_validateMmv.IsModelValid(pqInput.CarModelId))
                   || (pqInput.CarVersionId > 0 && !_validateMmv.IsModelVersionValid(pqInput.CarVersionId)))
                {
                    response.Content = null;
                    return response;
                }

                if (pqInput.SourceId == (int)Platform.CarwaleAndroid || pqInput.SourceId == (int)Platform.CarwaleiOS)
                {
                    var pq = _container.Resolve<IPQAdapter>("pqAndroidAdapterv1");

                    var pqList = pq.GetPQ<PQAndroid>(pqInput, userIdentifier);

                    response.Content = new StringContent(JsonConvert.SerializeObject(pqList));
                }
                else if (pqInput.SourceId == (int)Platform.CarwaleDesktop)
                {
                    int makeId = 0;
                    if (pqInput.CarVersionId > 0)
                    {
                        var carVersionDetails = _carVersionCacheRepo.GetVersionDetailsById(pqInput.CarVersionId);
                        makeId = carVersionDetails != null ? carVersionDetails.MakeId : 0;
                    }
                    else
                    {
                        var carModelDetails = _carModelCacheRepo.GetModelDetailsById(pqInput.CarModelId);
                        makeId = carModelDetails != null ? carModelDetails.MakeId : 0;
                    }

                    if (_vwfsMakeIds.Contains(makeId) || (abTestCookieValue <= _abTestMaxValForNewPq && abTestCookieValue >= _abTestMinValForNewPq))
                    {
                        string newPqPageUri = string.Format("/quotation/?m={0}&v={1}&c={2}&a={3}&p={4}", pqInput.CarModelId, pqInput.CarVersionId, pqInput.CityId,
                                                                                                    pqInput.AreaId, pqInput.PageId);
                        response.StatusCode = HttpStatusCode.PartialContent;
                        response.Content = new StringContent(newPqPageUri);
                        return response;
                    }

                    var pq = _container.Resolve<IPQAdapter>("pqDesktopAdapterv1");

                    var pqList = pq.GetPQ<PQDesktop>(pqInput, userIdentifier);

                    response.Content = new StringContent(JsonConvert.SerializeObject(pqList));
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PriceQuoteController.PostPQ_V1() for PQInputs of cityId :" + pqInput.CityId + " and VersionId : " + pqInput.CarVersionId);
                objErr.LogException();
            }
            return response;

        }

        /// <summary>
        /// Written By: Shalini Nair on 05/05/2016
        /// </summary>
        /// <param name="pqInput"></param>
        /// <returns></returns>
        [Route("v2/api/pq/")]
        public IHttpActionResult PostPQV2([FromBody] PQInput pqInput)
        {
            try
            {
                string userIdentifier = (pqInput.SourceId == (int)Platform.CarwaleiOS || pqInput.SourceId == (int)Platform.CarwaleAndroid) ?
                                        HttpContext.Current.Request.Headers["IMEI"] : UserTracker.GetSessionCookie();
                if (string.IsNullOrWhiteSpace(pqInput.Ltsrc))
                    pqInput.Ltsrc = CommonLTS.CookieLTS != "-1" ? CommonLTS.CookieLTS.Split(':')[0] : "-1";

                if (pqInput.CityId <= 0 || (pqInput.CarVersionId <= 0 && pqInput.CarModelId <= 0)
                    || (pqInput.CarModelId > 0 && !_validateMmv.IsModelValid(pqInput.CarModelId))
                    || (pqInput.CarVersionId > 0 && !_validateMmv.IsModelVersionValid(pqInput.CarVersionId)))
                {
                    return BadRequest();
                }

                var pq = _container.Resolve<IPQAdapter>("pqAndroidAdapterv2");
                var pqList = pq.GetPQ<PQAndroidV2>(pqInput, userIdentifier);

                if (pqList == null)
                {
                    return InternalServerError();
                }

                return Ok(pqList);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PriceQuoteController.PostPQ_V2() for PQInputs of cityId :" + pqInput.CityId + " and VersionId : " + pqInput.CarVersionId);
                objErr.LogException();
                return InternalServerError();
            }
        }

        [Route("v1/api/pq/")]
        public HttpResponseMessage GetByIdV1()
        {
            var response = new HttpResponseMessage();

            try
            {
                NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
                string enIds = (nvc["pqids"] != null && !String.IsNullOrEmpty(nvc["pqids"].ToString())) ? nvc["pqids"].ToString() : "";
                int sourceId = (nvc["sourceid"] != null && !String.IsNullOrEmpty(nvc["sourceid"].ToString())) ? Convert.ToInt16(nvc["sourceid"]) : 0;
                string userIdentifier = (sourceId == (int)Platform.CarwaleiOS || sourceId == (int)Platform.CarwaleAndroid) ? HttpContext.Current.Request.Headers["IMEI"] : UserTracker.GetSessionCookie();
                if (sourceId <= 0)
                {
                    response.Content = null;
                    return response;
                }
                if (sourceId == (int)Platform.CarwaleDesktop)
                {
                    List<string> enIdsList = enIds.Split(',').ToList();
                    var pq = _container.Resolve<IPQAdapter>("pqDesktopAdapterv1");
                    var pqList = pq.GetPQByIds<PQDesktop>(enIdsList, userIdentifier);
                    response.Content = new StringContent(JsonConvert.SerializeObject(pqList));
                }
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "PriceQuoteController.GetByIdV1()");
                objErr.LogException();
            }

            return response;
        }


        [HttpPost]
        [Route("api/pq/clearPricesCache")]
        public IHttpActionResult ClearPricesCache([FromBody] List<string> keys)
        {
            _prices.UpdateCache(keys);
            return Ok();
        }

        [HttpGet]
        [Route("api/pq/nearbycities")]
        public IHttpActionResult GetNearbyCities(int versionId, int cityId, int count)
        {
            return Ok(_prices.GetNearbyCitieswithPrices(versionId, cityId, count));
        }

        [HttpGet]
        [Route("api/v1/pq/nearbycities")]
        public IHttpActionResult GetNearbyCitiesV1(int versionId, int cityId, int count)
        {
            try
            {
                if (versionId <= 0 || cityId <= 0 || count <= 0)
                {
                    return BadRequest();
                }

                var nearByCities = _prices.GetNearbyCitiesDto(versionId, cityId, count);
                if (nearByCities.Cities.Count > 0)
                {
                    return Ok(nearByCities);
                }
                else
                {
                    return Ok();
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError();
            }
        }

        /// <summary>
        /// This api updates prices elastic search index for nearbycities. 
        /// This function will be removed once logic is moved to prices microservice.
        /// </summary>
        /// <param name="vehiclePriceList"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/prices/ElasticSearchIndex/")]
        public IHttpActionResult NearByCitiesElasticSearchIndex(List<VehiclePriceDto> vehiclePriceList)
        {
            try
            {
                if (!vehiclePriceList.IsNotNullOrEmpty())
                {
                    return BadRequest();
                }

                var vehicleCityList = Mapper.Map<List<VehiclePriceDto>, List<VehiclePrice>>(vehiclePriceList);
                _nearByCitiesSearchLogic.AddToIndex(vehicleCityList);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex);
                return InternalServerError();
            }
        }
    }

}
