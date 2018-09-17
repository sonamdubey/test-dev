using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.DTO.PriceQuote;
using Bikewale.DTO.PriceQuote.CustomerDetails;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Customer;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.Lead;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.PriceQuote;
using Bikewale.Service.Utilities;
using Bikewale.Utility;
using RabbitMqPublishing;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
namespace Bikewale.Service.Controllers.PriceQuote
{
    /// <summary>
    /// Price Quote Customer Detail Controller
    /// Author      :   Sumit Kate
    /// Created On  :   21 Aug 2015
    /// Modified by :   Sumit Kate on 20 May 2016
    /// Description :   Serialize the input to error message for more details
    /// Modified by :   Aditi Srivastava on 14 Sep 2016
    /// Description :   Changed dealer masking no to dealer phone no in DPQSmsEntity
    /// </summary>
    public class PQCustomerDetailController : CompressionApiController//ApiController
    {
        private readonly ICustomerAuthentication<CustomerEntity, UInt32> _objAuthCustomer = null;
        private readonly ICustomer<CustomerEntity, UInt32> _objCustomer = null;
        private readonly IDealerPriceQuote _objDealerPriceQuote = null;
        private readonly IMobileVerificationRepository _mobileVerRespo = null;
        private readonly IMobileVerificationCache _mobileVerCacheRepo = null;
        private readonly IMobileVerification _mobileVerification = null;
        private readonly IDealer _objDealer = null;
        private readonly IPriceQuote _objPriceQuote = null;
        private readonly ILeadNofitication _objLeadNofitication = null;
        private readonly ILead _objLeadProcess = null;
        private readonly IApiGatewayCaller _apiGatewayCaller;

        public PQCustomerDetailController(
            ICustomerAuthentication<CustomerEntity, UInt32> objAuthCustomer,
            ICustomer<CustomerEntity, UInt32> objCustomer,
            IDealerPriceQuote objDealerPriceQuote,
            IMobileVerificationRepository mobileVerRespo,
            IMobileVerification mobileVerificetion,
            IDealer objDealer,
            IPriceQuote objPriceQuote, ILeadNofitication objLeadNofitication, IMobileVerificationCache mobileVerCacheRepo, ILead objLeadProcess, IApiGatewayCaller apiGatewayCaller)
        {
            _objAuthCustomer = objAuthCustomer;
            _objCustomer = objCustomer;
            _objDealerPriceQuote = objDealerPriceQuote;
            _mobileVerRespo = mobileVerRespo;
            _mobileVerification = mobileVerificetion;
            _objDealer = objDealer;
            _objPriceQuote = objPriceQuote;
            _objLeadNofitication = objLeadNofitication;
            _mobileVerCacheRepo = mobileVerCacheRepo;
            _objLeadProcess = objLeadProcess;
            _apiGatewayCaller = apiGatewayCaller;
        }

        /// <summary>
        /// Created by : Snehal Dange on 3rd May 2018
        /// Desc :  Method created to set request headers into nvc.
        /// Modified by : Sanskar Gupta on 09 May 2018
        /// Description : Add logging when exception is caught.
        /// Modified by : Monika Korrapati on 17 Aug 2018
        /// Description : Added appVersion to requestHeaders nvc for bhrigu tracking
        /// </summary>
        /// <param name="requestHeadersInput"></param>
        /// <returns></returns>
        private NameValueCollection SetRequestHeaders(HttpRequestHeaders requestHeadersInput)
        {
            NameValueCollection requestHeaders = null;
            try
            {
                requestHeaders = new NameValueCollection();
                requestHeaders["utma"] = requestHeadersInput.Contains("utma") ? requestHeadersInput.GetValues("utma").FirstOrDefault() : String.Empty;
                requestHeaders["utmz"] = requestHeadersInput.Contains("utmz") ? requestHeadersInput.GetValues("utmz").FirstOrDefault() : String.Empty;
                requestHeaders["platformId"] = requestHeadersInput.Contains("platformId") ? requestHeadersInput.GetValues("platformId").FirstOrDefault().ToString() : String.Empty;
                requestHeaders["appVersion"] = requestHeadersInput.Contains("version_code") && requestHeaders["platformId"] == ((byte)PQSources.Android).ToString()? requestHeadersInput.GetValues("version_code").First().ToString() : String.Empty;
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Exception : Bikewale.Service.Controllers.PriceQuote.PQCustomerDetailController.SetRequestHeaders()"));
            }
            return requestHeaders;
        }

        /// <summary>
        /// Saves the Customer details if it is a new customer.
        /// generated the OTP for the non verified customer
        /// Modified By :   Sumit Kate on 18 Nov 2015
        /// Description :   Save the State of the Booking Journey as Described in Task# 107795062 
        /// Modified By :   Sumit Kate on 08 Dec 2015
        /// Description :   Update the Bike Version. Fixed the APP functionality.
        /// Modified By : Sadhana Upadhyay on 29 Dec 2015
        /// Summary : To capture device id, utma, utmz, Pq lead id etc.
        /// Modified By : Lucky Rathore on 20/04/2016
        /// Summary : Masking No. (mobile no.) of dealer is changed to dealer phone no. for sms to customer.
        /// Modified by :   Sumit Kate on 02 May 2016
        /// Description :   Send the notification immediately
        /// Modified by :   Lucky Rathore on 13 May 2016
        /// Description :   var versionName declare, Intialized and NotifyCustomer() singature Updated.
        /// Modified by :   Sumit Kate on 01 June 2016
        /// Description :   Commented Mobile verification process
        ///  noOfAttempts = -1 and isVerified = true to by pass the Mobile Verification Process
        /// Modified By : Lucky Rathore on 11 July 2016.
        /// Description : parameter dealerArea added in NotifyDealer(); 
        /// MOdified by : Pratibha Verma on 27 April 2018
        /// Description : added parameters additionalnumbers and additionalemails in NotifyDealer()
        /// Modifed by : Snehal Dange on 3rd May 2018
        /// Desc :  Restuctured the code to process all details in Bal
        /// </summary>
        /// <param name="input">Customer details with price quote details</param>
        /// <returns></returns>
        [ResponseType(typeof(PQCustomerDetailOutput)), Route("api/PQCustomerDetail/"), HttpPost]
        public IHttpActionResult Post([FromBody]Bikewale.DTO.PriceQuote.PQCustomerDetailInput input)
        {

            PQCustomerDetailOutput output = null;

            try
            {
                PQCustomerDetailOutputEntity outEntity = null;
                Bikewale.Entities.PriceQuote.PQCustomerDetailInput pqInput = null;
                if (input != null
                    && !String.IsNullOrEmpty(input.CustomerMobile)
                    && input.PQId > 0
                    && input.DealerId > 0
                    && Convert.ToInt32(input.VersionId) > 0
                    && Convert.ToInt32(input.CityId) > 0)
                {
                    pqInput = PQCustomerMapper.Convert(input);
                    NameValueCollection requestHeaders = SetRequestHeaders(Request.Headers);
                    outEntity = _objLeadProcess.ProcessPQCustomerDetailInputWithPQ(pqInput, requestHeaders);
                    output = PQCustomerMapper.Convert(outEntity);

                    return Ok(output);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Exception : Bikewale.Service.Controllers.PriceQuote.PQCustomerDetailController.Post({0})", Newtonsoft.Json.JsonConvert.SerializeObject(input)));

                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 26 June 2018
        /// Description : added leadId in input and output entity to remove PQId dependency
        /// Modified by : Rajan Chauhan on 27 July 2018
        /// Description : Added AreaId
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ResponseType(typeof(Bikewale.DTO.PriceQuote.v3.PQCustomerDetailOutput)), Route("api/v1/PQCustomerDetail/"), HttpPost]
        public IHttpActionResult Post([FromBody]Bikewale.DTO.PriceQuote.v3.PQCustomerDetailInput input)
        {

            Bikewale.DTO.PriceQuote.v3.PQCustomerDetailOutput output = null;

            try
            {
                Bikewale.Entities.PriceQuote.v2.PQCustomerDetailOutputEntity outEntity = null;
                Bikewale.Entities.PriceQuote.v2.PQCustomerDetailInput pqInput = null;
                if (input != null
                    && !String.IsNullOrEmpty(input.CustomerMobile)
                    && !String.IsNullOrEmpty(input.PQId)
                    && input.DealerId > 0
                    && Convert.ToInt32(input.VersionId) > 0
                    && Convert.ToInt32(input.CityId) > 0)
                {
                    pqInput = PQCustomerMapper.Convert(input);
                    NameValueCollection requestHeaders = SetRequestHeaders(Request.Headers);
                    outEntity = _objLeadProcess.ProcessPQCustomerDetailInputWithPQV2(pqInput, requestHeaders);
                    output = PQCustomerMapper.Convert(outEntity);

                    return Ok(output);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Exception : Bikewale.Service.Controllers.PriceQuote.PQCustomerDetailController.Post({0})", Newtonsoft.Json.JsonConvert.SerializeObject(input)));

                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 24 July 2018
        /// Description : Process MLA Leads
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ResponseType(typeof(bool)), Route("api/submitmlaleads/"), HttpPost]
        public IHttpActionResult Post([FromBody]Bikewale.DTO.PriceQuote.v3.MLALeadDetails input)
        {
            bool isSuccess = false;
            try
            {
                if (input != null
                    && !String.IsNullOrEmpty(input.CustomerMobile)
                    && !String.IsNullOrEmpty(input.PQId)
                    && Convert.ToUInt32(input.VersionId) > 0
                    && Convert.ToUInt32(input.CityId) > 0
                    && input.DealerIds != null
                    && input.DealerIds.Any())
                {
                    NameValueCollection requestHeaders = SetRequestHeaders(Request.Headers);
                    NameValueCollection mlaNvcObj = GetMLAConsumersObject(input, requestHeaders);
                    if (mlaNvcObj != null)
                    {
                        RabbitMqPublish objRMQPublish = new RabbitMqPublish();
                        objRMQPublish.PublishToQueue(Bikewale.Utility.BWConfiguration.Instance.MLALeadConsumerQueue, mlaNvcObj);
                    }
                    isSuccess = true;
                    return Ok(isSuccess);
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Exception : Bikewale.Service.Controllers.PriceQuote.PQCustomerDetailController.Post({0})", Newtonsoft.Json.JsonConvert.SerializeObject(input)));
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by  :   Sumit Kate on 23 May 2016
        /// Description :   Saves the Customer details if it is a new customer.
        /// generated the OTP for the non verified customer
        /// Modified By : Lucky Rathore on 11 July 2016.
        /// Description : parameter dealerArea added in NotifyDealer(); 
        /// Created by : Snehal Dange on 3rd May 2018
        /// Desc :  Restuctured the code to process all details in Bal
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ResponseType(typeof(Bikewale.DTO.PriceQuote.v2.PQCustomerDetailOutput)), Route("api/PQCustomerDetailWithOutPQ/"), HttpPost]
        public IHttpActionResult Postv2([FromBody]Bikewale.DTO.PriceQuote.v2.PQCustomerDetailInput input)
        {

            Bikewale.DTO.PriceQuote.v2.PQCustomerDetailOutput output = null;

            try
            {
                if (input != null
                    && !String.IsNullOrEmpty(input.CustomerEmail)
                    && !String.IsNullOrEmpty(input.CustomerMobile)
                    && Convert.ToInt32(input.VersionId) > 0
                    && Convert.ToInt32(input.CityId) > 0)
                {
                    Bikewale.Entities.PriceQuote.PQCustomerDetailInput pqInput = null;
                    PQCustomerDetailOutputEntity outEntity = null;
                    pqInput = PQCustomerMapper.Convert(input);
                    NameValueCollection requestHeaders = SetRequestHeaders(Request.Headers);
                    outEntity = _objLeadProcess.ProcessPQCustomerDetailInputWithoutPQ(pqInput, requestHeaders);
                    output = PQCustomerMapper.Convertv2(outEntity);//mapper

                    return Ok(output);
                }
                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.PQCustomerDetailController.Postv2");

                return InternalServerError();
            }
        }




        /// <summary>
        /// Gets the customer details for a generated price quote
        /// </summary>
        /// <param name="pqId">Price quote</param>
        /// <returns>Customer Details</returns>
        [ResponseType(typeof(PQCustomer))]
        public IHttpActionResult Get(uint pqId)
        {
            PQCustomerDetail entity = null;
            PQCustomer output = null;
            try
            {
                entity = _objDealerPriceQuote.GetCustomerDetailsByPQId(pqId);
                if (entity != null)
                {
                    output = PQCustomerMapper.Convert(entity);
                    return Ok(output);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.PQCustomerDetailController.Get");

                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 24 July 2018
        /// Description : returns nvc object for mla lead processing consumer
        /// </summary>
        /// <param name="input"></param>
        /// <param name="requestHeaders"></param>
        /// <returns></returns>
        private NameValueCollection GetMLAConsumersObject(Bikewale.DTO.PriceQuote.v3.MLALeadDetails input, NameValueCollection requestHeaders)
        {
            NameValueCollection mlaNvcObj = null;
            try
            {
                StringBuilder dealerIds = new StringBuilder();
                if(input.DealerIds != null && input.DealerIds.Any())
                {
                    foreach (uint dealerId in input.DealerIds)
                    {
                        dealerIds.Append(String.Format("{0},", dealerId));
                    }
                    if (dealerIds.Length > 1)
                    {
                        dealerIds.Remove(dealerIds.Length - 1, 1);
                    }
                }

                var request = HttpContext.Current.Request;
                string PageUrl = request.UrlReferrer.ToString();
                if (dealerIds.Length > 0)
                {
                    mlaNvcObj = new NameValueCollection();
                    mlaNvcObj.Add("pqGUId", input.PQId);
                    mlaNvcObj.Add("dealerIds", dealerIds.ToString());
                    mlaNvcObj.Add("customerName", input.CustomerName);
                    mlaNvcObj.Add("customerEmail", input.CustomerEmail);
                    mlaNvcObj.Add("customerMobile", input.CustomerMobile);
                    mlaNvcObj.Add("versionId", input.VersionId);
                    mlaNvcObj.Add("cityId", input.CityId);
                    mlaNvcObj.Add("utma", request.Cookies["_bwutma"] != null ? request.Cookies["_bwutma"].Value : requestHeaders["utma"]);
                    mlaNvcObj.Add("utmz", request.Cookies["_bwutmz"] != null ? request.Cookies["_bwutmz"].Value : requestHeaders["utmz"]);
                    mlaNvcObj.Add("platformId", Convert.ToString(input.PlatformId));
                    mlaNvcObj.Add("deviceId", input.DeviceId);
                    mlaNvcObj.Add("clientIP", CurrentUser.GetClientIP());
                    mlaNvcObj.Add("pageUrl", PageUrl);
                    mlaNvcObj.Add("leadSourceId", Convert.ToString(input.LeadSourceId));
                    mlaNvcObj.Add("cityName", input.CityName);
                    mlaNvcObj.Add("areaId", input.AreaId);
                    mlaNvcObj.Add("areaName", input.AreaName);
                    mlaNvcObj.Add("pageId", input.PageId.ToString());
                    mlaNvcObj.Add("appVersion", requestHeaders["appVersion"]);
                    mlaNvcObj.Add("queryString", PageUrl.Split('?').Length > 1 ? PageUrl.Split('?')[1].Replace('&', '|') : String.Empty);
                    mlaNvcObj.Add("userAgent", request.UserAgent);
                    mlaNvcObj.Add("cookieId", request.Cookies["BWC"] != null ? request.Cookies["BWC"].Value : request.Headers["IMEI"]);
                    mlaNvcObj.Add("bwTest", request.Cookies["_bwtest"].Value);
                    mlaNvcObj.Add("cwv", request.Cookies["_cwv"].Value);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, String.Format("Exception : Bikewale.Service.Controllers.PriceQuote.PQCustomerDetailController.GetMLAConsumersObject({0})", Newtonsoft.Json.JsonConvert.SerializeObject(mlaNvcObj)));
            }
            return mlaNvcObj;
          }

        }

    }
