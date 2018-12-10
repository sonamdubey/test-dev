using Carwale.Entity.Dealers;
using Carwale.Interfaces;
using Carwale.Interfaces.Dealers;
using Carwale.Utility;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Carwale.BL.PriceQuote;
using Carwale.Service.Filters.Dealer;
using Carwale.Entity.Dealers.URI;
using Carwale.Interfaces.Campaigns;
using Carwale.Interfaces.Customer;
using Carwale.Entity.CarData;
using Carwale.DTOs.Dealer;
using AutoMapper;
using Carwale.Service.Filters.Campaigns;
using System.Web.Http.Cors;
using Carwale.Entity.Enum;
using Carwale.Entity.Geolocation;
using Carwale.Entity.Campaigns;
using Carwale.Entity;
using Carwale.DTOs.Campaigns;
using System.Net;
using Carwale.Notifications.Logs;
using Carwale.Interfaces.Leads;

namespace Carwale.Service.Controllers
{
    public class DealerSponsoredAdController : ApiController
    {
        private readonly IUnityContainer _container;

        public DealerSponsoredAdController(IUnityContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Controller for Post dealer inquiry 
        /// Written By : Ashish Verma on 2/6/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public HttpResponseMessage PostDealerInquiry([FromBody] DealerInquiryDetails dealerInquiryDetails)
        {
            var response = new HttpResponseMessage();
            try
            {
                if(dealerInquiryDetails.ApplicationId <= 0)
                {
                    dealerInquiryDetails.ApplicationId = 1;
                }
                if (dealerInquiryDetails.AreaId < 0)
                {
                    dealerInquiryDetails.AreaId = 0;
                }
                if (string.IsNullOrWhiteSpace(dealerInquiryDetails.Ltsrc))
                {
                    dealerInquiryDetails.Ltsrc = CommonLTS.CookieLTS != "-1" ? CommonLTS.CookieLTS.Split(':')[0] : "-1";
                } 
                              
                var _pqDealerAdLeadId = _container.Resolve<IDealerInquiry>().ProcessRequest(dealerInquiryDetails);
                response.Content = new StringContent(CarwaleSecurity.Encrypt(HttpUtility.UrlEncode(_pqDealerAdLeadId.ToString())));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "DealerSponsoredAdController.PostDealerInquiry()");
            }
            return response;
        }

        /// <summary>
        /// For Getting Dealers List on clicking PQDealerAd Based on makeid and cityId
        /// Written By : Ashish Verma on 18/9/2014
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public HttpResponseMessage GetPQDealersList()
        {
            var response = new HttpResponseMessage();

            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

            if (!RegExValidations.IsPositiveNumber(nvc["cityid"].ToString()) || !RegExValidations.IsPositiveNumber(nvc["makeid"].ToString()))
            {
                response.Content = new StringContent("Bad Request");
                return response;
            }

            int cityId = Convert.ToInt32(nvc["cityId"]);
            int makeId = Convert.ToInt32(nvc["makeId"]);
            try
            {

                response.Content = new StringContent(JsonConvert.SerializeObject(_container.Resolve<IDealerSponsoredAdCache>().GetNewCarDealersByMakeAndCityId(makeId, cityId)));

            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "DealerSponsoredAdController.GetNewCarDealers()");
            }
            return response;
        }

        /// <summary>
        /// For Getting SponsoredDealerEntity by (campaignid) or (modelid,cityid,zoneid)
        /// Written By : Rohan Sapkal on 13/05/2015
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        [HttpGet]
        [ActionName("getdealercampaign")]
        [DealerCampaignValidation]
        public HttpResponseMessage GetDealerCampaign([FromUri] CampaignInputURI input)
        {
            var response = new HttpResponseMessage();
            try
            {
                var _campaignBL = _container.Resolve<ICampaign>();
                var _trackingBL = _container.Resolve<ICustomerTracking>();

                SponsoredDealer dealerSponsoredDetails = _campaignBL.GetSponsorDealerAd(input.ModelId, input.PlatformId,
                    new Location
                    {
                        CityId = input.CityId,
                        ZoneId = CustomParser.parseIntObject(input.ZoneId)
                    });

                if (input.PlatformId == (int)Platform.CarwaleAndroid || input.PlatformId == (int)Platform.CarwaleiOS)
                {
                    var carDataTrackingEntity = new CarDataTrackingEntity
                    {
                        ModelId = input.ModelId,
                        Platform = input.PlatformId,
                        VersionId = 0,
                        Category = "ModelPage",
                        Action = "ModelImpression"
                    };
                    carDataTrackingEntity.Location.CityId = input.CityId;

                    _trackingBL.AppsTrackModelVersionImpression(carDataTrackingEntity, dealerSponsoredDetails);
                }

                if (dealerSponsoredDetails.DealerId < 1 || string.IsNullOrWhiteSpace(dealerSponsoredDetails.DealerName))
                {
                    response.Content = new StringContent("No results found");
                    return response;
                }

                response.Content = new StringContent(JsonConvert.SerializeObject(dealerSponsoredDetails));

            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "DealerSponsoredAdController.GetSponsoredDealer()");
            }
            return response;
        }

        [EnableCors(origins: "http://localhost,https://localhost,http://test.cartrade.com,https://test.cartrade.com,http://testm.cartrade.com,https://testm.cartrade.com,http://testapi.cartrade.com,https://testapi.cartrade.com,http://www.cartrade.com,https://www.cartrade.com,,http://m.cartrade.com,https://m.cartrade.com,,http://api.cartrade.com,https://api.cartrade.com", headers: "*", methods: "POST")]
        [HttpPost, Route("api/dealerinquiry/")]
        [LeadSubmissionValidation]
        public IHttpActionResult SubmitDealerInquiry([FromBody] DealerInquiryDetailsDTO dealerInquiryDetails)
        {
            var response = "";
            try
            {
                DealerInquiryDetails inquiryDetails = Mapper.Map<DealerInquiryDetailsDTO, DealerInquiryDetails>(dealerInquiryDetails);
                if (string.IsNullOrWhiteSpace(dealerInquiryDetails.Ltsrc)) inquiryDetails.Ltsrc = CommonLTS.CookieLTS != "-1" ? CommonLTS.CookieLTS.Split(':')[0] : "-1";
                inquiryDetails.BuyTimeText = "1 week";
                inquiryDetails.BuyTimeValue = 7;
                inquiryDetails.RequestType = 1;
                var _pqDealerAdLeadId = _container.Resolve<IDealerInquiry>().ProcessRequest(inquiryDetails);
                response = (CarwaleSecurity.Encrypt(HttpUtility.UrlEncode(_pqDealerAdLeadId.ToString())));
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "DealerSponsoredAdController.SubmitDealerInquiry()");
            }
            return Ok(response);
        }

        [HttpGet]
        [Route("api/dealerAd/")]
        [DealerAdValidation]
        public IHttpActionResult GetDealerAd([FromUri] CampaignInputv2 input)
        {
            try
            {
                Location locationObj = Mapper.Map<CampaignInputv2, Location>(input);
                CarIdEntity carEntity = Mapper.Map<CampaignInputv2, CarIdEntity>(input);
                int platformId = Request.Headers.GetValueFromHttpHeader<int>("sourceid");

                IDealerAdProvider _dealerAdProvider = _container.Resolve<IDealerAdProvider>();
                DealerAd dealerAd = _dealerAdProvider.GetDealerAd(carEntity, locationObj, platformId, (int)CampaignAdType.Pq, input.CampaignId, input.PageId);
                DealerAdDTO dealerAdDto = Mapper.Map<DealerAd, DealerAdDTO>(dealerAd);

                if (platformId == (int)Platform.CarwaleAndroid || platformId == (int)Platform.CarwaleiOS)
                {
                    CarDataTrackingEntity carDataTrackingEntity = new CarDataTrackingEntity
                    {
                        ModelId = input.ModelId,
                        Platform = platformId,
                        VersionId = 0,
                        Category = "ModelPage",
                        Action = "ModelImpression"
                    };
                    carDataTrackingEntity.Location.CityId = locationObj != null ? locationObj.CityId : 0;

                    var _trackingBL = _container.Resolve<ICustomerTracking>();

                    if (dealerAd != null)
                    {
                        _trackingBL.AppsTrackModelVersionImpressionV1(carDataTrackingEntity, dealerAd.Campaign);
                    }
                    else
                    {
                        _trackingBL.AppsTrackModelVersionImpressionV1(carDataTrackingEntity, null);
                    }
                }

                if (dealerAdDto != null)
                {
                    return Json(dealerAdDto);
                }
                else
                {
                    return StatusCode(HttpStatusCode.NoContent);
                }
            }
            catch (Exception ex)
            {
                Logger.LogException(ex, "DealerSponsoredAdController.GetDealerAd()");
                return InternalServerError();
            }
        }
    }
}
