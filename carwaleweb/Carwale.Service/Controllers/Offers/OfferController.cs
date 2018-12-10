using AutoMapper;
using Carwale.BL.PriceQuote;
using Carwale.DTOs.Offers;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Offers;
using Carwale.Notifications;
using Carwale.Service.Filters;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace Carwale.Service.Offers
{
    public class OffersController : ApiController
    {
        protected JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore, ContractResolver = new CamelCasePropertyNamesContractResolver() };
            

        /// <summary>
        /// Controller for post Offer inquiry 
        /// Written By : Ashish Verma on 20/11/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        
        [HttpPost]
        [ActionName("inquiry")]
        public HttpResponseMessage PostOfferInquiry([FromBody] OfferInputs offerInputs)
        {
            var response = new HttpResponseMessage();

            try
            {
                offerInputs.Ltsrc = CommonLTS.CookieLTS != "-1" ? CommonLTS.CookieLTS.Split(':')[0] : "-1";
                response.Content = new StringContent(JsonConvert.SerializeObject(new OfferCoupon()));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "OfferController.PostOfferInquiry()");
                objErr.LogException();
            }
            return response;

        }

        /// <summary>
        /// Controller for Getting All Offers
        /// Written By : Ashish Verma on 20/11/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("all")]
        public HttpResponseMessage GetAllOffers(HttpRequestMessage request)
        {
            var response = new HttpResponseMessage();
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            int makeId = (nvc["makeid"] != null && !String.IsNullOrEmpty(nvc["makeid"].ToString())) ? Convert.ToInt32(nvc["makeid"]) : -1 ;
            int modelId = (nvc["modelid"] != null && !String.IsNullOrEmpty(nvc["modelid"].ToString())) ?Convert.ToInt32(nvc["modelid"]) : -1;
            int versionId = (nvc["versionid"] != null && !String.IsNullOrEmpty(nvc["versionid"].ToString())) ? Convert.ToInt32(nvc["versionid"]) : -1;
            int cityId = (nvc["cityid"] != null && !String.IsNullOrEmpty(nvc["cityid"].ToString())) ? Convert.ToInt32(nvc["cityid"]) : -1;
            int platformId = (request.Headers.Contains("SourceId") == true ? Convert.ToInt16(request.Headers.GetValues("SourceId").First().ToString()) : ((!string.IsNullOrEmpty(nvc["platformId"])) ? Convert.ToInt16(nvc["platformId"]) : -1));
            try
            {
                var offers = new List<OffersEntity>(); // container.Resolve<IOffersRepository>().GetOffer(makeId, modelId, versionId, cityId, platformId);
                response.Content = new StringContent(JsonConvert.SerializeObject(offers));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "OfferController.GetAllOffers()");
                objErr.LogException();
            }
            return response;

        }

        /// <summary>
        /// Controller for Getting All Offers
        /// Written By : Ashish Verma on 20/11/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("dealerdetails")]
        public HttpResponseMessage GetOfferDealerDetails()
        {
            var response = new HttpResponseMessage();
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            int dealerId = (nvc["dealerid"] != null && !String.IsNullOrEmpty(nvc["dealerid"].ToString())) ? Convert.ToInt32(nvc["dealerid"]) : 0;
            int offerId = (nvc["offerid"] != null && !String.IsNullOrEmpty(nvc["offerid"].ToString())) ? Convert.ToInt32(nvc["offerid"]) : -1;


            try
            {
                if (dealerId == 0 || offerId == -1)
                {
                    response.Content = null;
                    return response;
                }
                response.Content = new StringContent(JsonConvert.SerializeObject(new OfferDealerDetails()));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "OfferController.GetOfferDealerDetails()");
                objErr.LogException();
            }
            return response;

        }

        /// <summary>
        /// Controller for getting all cities for offers
        /// Written By : Ashish Verma on 20/11/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("cities")]
        public HttpResponseMessage GetOffersCities()
        {
            var response = new HttpResponseMessage();

            try
            {
                IUnityContainer container = new UnityContainer();

                var objCity = new List<City>();
                response.Content = new StringContent(JsonConvert.SerializeObject(objCity));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "OfferController.GetOffersCities()");
                objErr.LogException();
            }
            return response;

        }

        /// <summary>
        /// Controller for getting all cities for offers
        /// Written By : Ashish Verma on 20/11/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("models")]
        public HttpResponseMessage GetOfferModelsByMake()
        {
            var response = new HttpResponseMessage();
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            int makeId = (nvc["makeid"] != null && !String.IsNullOrEmpty(nvc["makeid"].ToString())) ? Convert.ToInt32(nvc["makeid"]) : 0;
            int cityId = (nvc["cityid"] != null && !String.IsNullOrEmpty(nvc["cityid"].ToString())) ? Convert.ToInt32(nvc["cityid"]) : 0;
            int offerId = (nvc["offerid"] != null && !String.IsNullOrEmpty(nvc["offerid"].ToString())) ? Convert.ToInt32(nvc["offerid"]) : 0;
            try
            {
                var objModels = new List<OfferModels>();
                response.Content = new StringContent(JsonConvert.SerializeObject(objModels));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "OfferController.GetOfferModelsByMake()");
                objErr.LogException();
            }
            return response;

        }

        /// <summary>
        /// Controller for getting all cities for offers
        /// Written By : Ashish Verma on 20/11/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("makes")]
        public HttpResponseMessage GetOfferMakesByCity()
        {
            var response = new HttpResponseMessage();
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

            int cityId = (nvc["cityid"] != null && !String.IsNullOrEmpty(nvc["cityid"].ToString())) ? Convert.ToInt32(nvc["cityid"]) : 0;
            try
            {
                var objModels = new List<OfferMakes>(); // container.Resolve<IOffersRepository>().GetOfferMakesByCity(cityId);
                response.Content = new StringContent(JsonConvert.SerializeObject(objModels));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "OfferController.GetOfferModelsByMake()");
                objErr.LogException();
            }
            return response;

        }

        /// <summary>
        /// Controller for getting all cities for offers
        /// Written By : Ashish Verma on 20/11/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("tnc")]
        public HttpResponseMessage GetOfferTnc()
        {
            var response = new HttpResponseMessage();
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

            int offerId = (nvc["offer"] != null && !String.IsNullOrEmpty(nvc["offer"].ToString())) ? Convert.ToInt32(nvc["offer"]) : -1;
            if (offerId == -1)
            {
                response.Content = null;
                return response;
            }
            try
            {
                var objModels = new OfferTnc(); // container.Resolve<IOffersRepository>().GetOfferTnc(offerId);
                response.Content = new StringContent(JsonConvert.SerializeObject(objModels));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "OfferController.GetOfferTnc()");
                objErr.LogException();
            }
            return response;

        }

        /// <summary>
        /// Controller for getting all cities for offers
        /// Written By : Ashish Verma on 20/11/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("paymentSuccess")]
        [AuthenticateBasic]
        public HttpResponseMessage GetPaymentSuccess()
        {
            var response = new HttpResponseMessage();
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

            int couponId = (nvc["couponid"] != null && !String.IsNullOrEmpty(nvc["couponid"].ToString())) ? Convert.ToInt32(nvc["couponid"]) : -1;
            int responseId = (nvc["responseid"] != null && !String.IsNullOrEmpty(nvc["responseid"].ToString())) ? Convert.ToInt32(nvc["responseid"]) : -1;
            int versionId = (nvc["versionid"] != null && !String.IsNullOrEmpty(nvc["versionid"].ToString())) ? Convert.ToInt32(nvc["versionid"]) : -1;
            int bookRefId = (nvc["bookno"] != null && !String.IsNullOrEmpty(nvc["bookno"].ToString())) ? Convert.ToInt32(nvc["bookno"]) : -1;
            if (couponId == -1 || responseId == -1)
            {
                response.Content = null;
            }
            try
            {
                var objOffer = string.Empty; //container.Resolve<IOfferPaymentAdapter<OfferPaymentEntity>>().ProcessObject(responseId, couponId,versionId,bookRefId);
                response.Content = new StringContent(JsonConvert.SerializeObject(objOffer));
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "OfferController.PostPaymentSuccess()");
                objErr.LogException();
            }
            return response;
        }

        [HttpGet,Route("api/version/offers")]
        public IHttpActionResult GetOfferByVersionAndCity(int versionId, int cityId, int platformId, int offerCount = 2)
        {
            if (versionId < 1 || cityId < 1 || platformId < 1 || offerCount < 1)
                return BadRequest("Missing or Wrong Parameters");

            try
            {
                var offerList = new List<OffersEntity>(); // _offersBL.GetOfferByVersionAndCity(versionId, cityId, platformId, offerCount);
                Mapper.CreateMap<OffersEntity, OffersDTO>();

                var offersDTO = Mapper.Map<List<OffersEntity>, List<OffersDTO>>(offerList);
                return Ok(offersDTO);
            }
            catch (Exception ex)
            {
                ExceptionHandler objErr = new ExceptionHandler(ex, "OffersController.GetOfferByVersionAndCity()");
                objErr.LogException();
            }
            return InternalServerError();
        }
    }
}
