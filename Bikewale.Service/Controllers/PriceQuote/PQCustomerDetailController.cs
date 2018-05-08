using Bikewale.BAL.ApiGateway.ApiGatewayHelper;
using Bikewale.DTO.PriceQuote;
using Bikewale.DTO.PriceQuote.BikeBooking;
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
using Bikewale.Service.AutoMappers.Bikebooking;
using Bikewale.Service.AutoMappers.PriceQuote;
using Bikewale.Service.Utilities;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http.Headers;
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
        /// </summary>
        /// <param name="input">Customer details with price quote details</param>
        /// <returns></returns>
        [ResponseType(typeof(PQCustomerDetailOutput))]
        [HttpPost,Route("api/PQCustomerDetail/")]
        public IHttpActionResult Post([FromBody]DTO.PriceQuote.PQCustomerDetailInput input)
        {

            PQCustomerDetailOutput output = null;
            bool isSuccess = false;
            bool isVerified = false;
            string password = string.Empty, salt = string.Empty, hash = string.Empty;
            string bikeName = String.Empty;
            string imagePath = String.Empty;
            string versionName = string.Empty;

            CustomerEntity objCust = null;
            PQCustomerDetail pqCustomer = null;
            BookingPageDetailsEntity objBookingPageDetailsEntity = null;
            PriceQuoteParametersEntity pqParam = null;
            BookingPageDetailsDTO objBookingPageDetailsDTO = null;
            DealerDetailsDTO dealer = null;
            uint exShowroomCost = 0;
            UInt32 TotalPrice = 0;
            uint bookingAmount = 0;
            UInt32 insuranceAmount = 0;
            bool IsInsuranceFree = false;
            sbyte noOfAttempts = 0;
            try
            {
                if (input != null && !String.IsNullOrEmpty(input.CustomerMobile) && input.PQId > 0 && input.DealerId > 0)
                {
                    if (input != null && ((input.PQId > 0) && (Convert.ToUInt32(input.VersionId) > 0)))
                    {
                        pqParam = new PriceQuoteParametersEntity();
                        pqParam.VersionId = Convert.ToUInt32(input.VersionId);

                        _objPriceQuote.UpdatePriceQuote(input.PQId, pqParam);
                    }
                    if (!_objAuthCustomer.IsRegisteredUser(input.CustomerEmail, input.CustomerMobile))
                    {
                        objCust = new CustomerEntity() { CustomerName = input.CustomerName, CustomerEmail = input.CustomerEmail, CustomerMobile = input.CustomerMobile, ClientIP = input.ClientIP };
                        UInt32 CustomerId = _objCustomer.Add(objCust);
                    }
                    else
                    {
                        var objCustomer = _objCustomer.GetByEmailMobile(input.CustomerEmail, input.CustomerMobile);
                        objCust = new CustomerEntity()
                        {
                            CustomerId = objCustomer.CustomerId,
                            CustomerName = input.CustomerName,
                            CustomerEmail = input.CustomerEmail = !String.IsNullOrEmpty(input.CustomerEmail) ? input.CustomerEmail : objCustomer.CustomerEmail,
                            CustomerMobile = input.CustomerMobile
                        };
                        _objCustomer.Update(objCust);
                    }


                    DPQ_SaveEntity entity = new DPQ_SaveEntity()
                    {
                        DealerId = input.DealerId,
                        PQId = input.PQId,
                        CustomerName = input.CustomerName,
                        CustomerEmail = input.CustomerEmail,
                        CustomerMobile = input.CustomerMobile,
                        ColorId = null,
                        UTMA = Request.Headers.Contains("utma") ? Request.Headers.GetValues("utma").FirstOrDefault() : String.Empty,
                        UTMZ = Request.Headers.Contains("utmz") ? Request.Headers.GetValues("utmz").FirstOrDefault() : String.Empty,
                        DeviceId = input.DeviceId,
                        LeadSourceId = input.LeadSourceId
                    };

                    isSuccess = _objDealerPriceQuote.SaveCustomerDetail(entity);

                    var numberList = _mobileVerCacheRepo.GetBlockedNumbers();

                    Bikewale.BAL.ApiGateway.Adapters.SpamFilter.GetScoreAdapter spamFilter = new BAL.ApiGateway.Adapters.SpamFilter.GetScoreAdapter();
                    spamFilter.AddApiGatewayCall(_apiGatewayCaller, objCust);
                    _apiGatewayCaller.Call();
                    var o = spamFilter.Output;

                    if (numberList != null && !numberList.Contains(input.CustomerMobile))
                    {
                        //Don't mark mobile verified for pq
                        //isVerified = _objDealerPriceQuote.UpdateIsMobileVerified(input.PQId);
                        isVerified = true;// Set Verified to true to push the lead into AB for un-verified leads as well

                        objBookingPageDetailsEntity = _objDealerPriceQuote.FetchBookingPageDetails(Convert.ToUInt32(input.CityId), Convert.ToUInt32(input.VersionId), input.DealerId);
                        objBookingPageDetailsDTO = BookingPageDetailsEntityMapper.Convert(objBookingPageDetailsEntity);

                        if (objBookingPageDetailsEntity != null)
                        {
                            objBookingPageDetailsEntity.VersionColors = null;

                            if (objBookingPageDetailsEntity.Disclaimers != null)
                            {
                                objBookingPageDetailsEntity.Disclaimers.Clear();
                                objBookingPageDetailsEntity.Disclaimers = null;
                            }

                            if (objBookingPageDetailsEntity.Offers != null)
                            {
                                objBookingPageDetailsEntity.Offers.Clear();
                                objBookingPageDetailsEntity.Offers = null;
                            }

                            if (objBookingPageDetailsEntity.Varients != null)
                            {
                                objBookingPageDetailsEntity.Varients.Clear();
                                objBookingPageDetailsEntity.Varients = null;
                            }
                        }

                        dealer = objBookingPageDetailsDTO.Dealer;

                        pqCustomer = _objDealerPriceQuote.GetCustomerDetails(input.PQId);
                        objCust = pqCustomer.objCustomerBase;

                        PQ_DealerDetailEntity dealerDetailEntity = null;

                        using (IUnityContainer container = new UnityContainer())
                        {
                            container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealers, Bikewale.DAL.AutoBiz.DealersRepository>();
                            Bikewale.Interfaces.AutoBiz.IDealers objDealer = container.Resolve<Bikewale.DAL.AutoBiz.DealersRepository>();
                            PQParameterEntity objParam = new PQParameterEntity();
                            objParam.CityId = Convert.ToUInt32(input.CityId);
                            objParam.DealerId = Convert.ToUInt32(input.DealerId);
                            objParam.VersionId = Convert.ToUInt32(input.VersionId);
                            dealerDetailEntity = objDealer.GetDealerDetailsPQ(objParam);
                        }

                        if (dealerDetailEntity != null && dealerDetailEntity.objQuotation != null)
                        {
                            if (dealerDetailEntity.objBookingAmt != null)
                            {
                                bookingAmount = dealerDetailEntity.objBookingAmt.Amount;
                            }
                            foreach (var price in dealerDetailEntity.objQuotation.PriceList)
                            {
                                IsInsuranceFree = OfferHelper.HasFreeInsurance(input.DealerId.ToString(), dealerDetailEntity.objQuotation.objModel.ModelId.ToString(), price.CategoryName, price.Price, ref insuranceAmount);
                            }
                            if (insuranceAmount > 0)
                            {
                                IsInsuranceFree = true;
                            }

                            bool isShowroomPriceAvail = false, isBasicAvail = false;

                            foreach (var item in dealerDetailEntity.objQuotation.PriceList)
                            {
                                //Check if Ex showroom price for a bike is available CategoryId = 3 (exshowrrom)
                                if (item.CategoryId == 3)
                                {
                                    isShowroomPriceAvail = true;
                                    exShowroomCost = item.Price;
                                }

                                //if Ex showroom price for a bike is not available  then set basic cost for bike price CategoryId = 1 (basic bike cost)
                                if (!isShowroomPriceAvail && item.CategoryId == 1)
                                {
                                    exShowroomCost += item.Price;
                                    isBasicAvail = true;
                                }

                                if (item.CategoryId == 2 && !isShowroomPriceAvail)
                                    exShowroomCost += item.Price;

                                TotalPrice += item.Price;
                            }

                            if (isBasicAvail && isShowroomPriceAvail)
                                TotalPrice = TotalPrice - exShowroomCost;

                            imagePath = Bikewale.Utility.Image.GetPathToShowImages(dealerDetailEntity.objQuotation.OriginalImagePath, dealerDetailEntity.objQuotation.HostUrl, Bikewale.Utility.ImageSize._210x118);
                            bikeName = dealerDetailEntity.objQuotation.objMake.MakeName + " " + dealerDetailEntity.objQuotation.objModel.ModelName + " " + dealerDetailEntity.objQuotation.objVersion.VersionName;
                            versionName = dealerDetailEntity.objQuotation.objVersion.VersionName;
                            var platformId = "";
                            if (Request.Headers.Contains("platformId"))
                            {
                                platformId = Request.Headers.GetValues("platformId").First().ToString();
                            }

                            DPQSmsEntity objDPQSmsEntity = new DPQSmsEntity();
                            objDPQSmsEntity.CustomerMobile = objCust.CustomerMobile;
                            objDPQSmsEntity.CustomerName = objCust.CustomerName;
                            objDPQSmsEntity.DealerMobile = dealerDetailEntity.objDealer != null ? dealerDetailEntity.objDealer.PhoneNo : string.Empty;
                            objDPQSmsEntity.DealerName = dealerDetailEntity.objDealer != null ? dealerDetailEntity.objDealer.Organization : string.Empty;
                            objDPQSmsEntity.Locality = dealerDetailEntity.objDealer != null ? dealerDetailEntity.objDealer.Address : string.Empty;
                            objDPQSmsEntity.BookingAmount = bookingAmount;
                            objDPQSmsEntity.BikeName = String.Format("{0} {1} {2}", dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName);
                            objDPQSmsEntity.DealerArea = dealerDetailEntity.objDealer != null && dealerDetailEntity.objDealer.objArea != null ? dealerDetailEntity.objDealer.objArea.AreaName : string.Empty;
                            objDPQSmsEntity.DealerAdd = dealerDetailEntity.objDealer != null ? dealerDetailEntity.objDealer.Address : string.Empty;
                            objDPQSmsEntity.DealerCity = dealerDetailEntity.objDealer != null ? dealerDetailEntity.objDealer.objCity.CityName : string.Empty;
                            objDPQSmsEntity.OrganisationName = dealerDetailEntity.objDealer != null ? dealerDetailEntity.objDealer.Organization : string.Empty;
                            if (dealerDetailEntity.objDealer != null)
                            {
                                _objLeadNofitication.NotifyCustomer(input.PQId, bikeName, imagePath, dealerDetailEntity.objDealer.Organization,
                                   dealerDetailEntity.objDealer.EmailId, dealerDetailEntity.objDealer.PhoneNo, dealerDetailEntity.objDealer.Organization,
                                   dealerDetailEntity.objDealer.Address, objCust.CustomerName, objCust.CustomerEmail,
                                   dealerDetailEntity.objQuotation.PriceList, dealerDetailEntity.objOffers, dealerDetailEntity.objDealer.objArea.PinCode,
                                   dealerDetailEntity.objDealer.objState.StateName, dealerDetailEntity.objDealer.objCity.CityName, TotalPrice, objDPQSmsEntity,
                                   "api/PQCustomerDetail", input.LeadSourceId, versionName, dealerDetailEntity.objDealer.objArea.Latitude, dealerDetailEntity.objDealer.objArea.Longitude,
                                   dealerDetailEntity.objDealer.WorkingTime, platformId);
                            }
                            if (dealerDetailEntity.objDealer != null)
                                _objLeadNofitication.NotifyDealer(input.PQId, dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName,
                                    dealerDetailEntity.objDealer.Organization, dealerDetailEntity.objDealer.EmailId, objCust.CustomerName, objCust.CustomerEmail, objCust.CustomerMobile, objCust.AreaDetails.AreaName, objCust.cityDetails.CityName, dealerDetailEntity.objQuotation.PriceList, Convert.ToInt32(TotalPrice), dealerDetailEntity.objOffers, imagePath, dealerDetailEntity.objDealer.PhoneNo, bikeName, objDPQSmsEntity.DealerArea);

                            if (isVerified)
                            {
                                _objPriceQuote.SaveBookingState(input.PQId, PriceQuoteStates.LeadSubmitted);
                                _objLeadNofitication.PushtoAB(input.DealerId.ToString(), input.PQId, objCust.CustomerName, objCust.CustomerMobile, objCust.CustomerEmail, input.VersionId, input.CityId);
                            }
                        }

                        output = new PQCustomerDetailOutput();
                        output.IsSuccess = isVerified;
                        output.Dealer = dealer;
                        output.NoOfAttempts = noOfAttempts;
                        return Ok(output);
                    }
                    else
                    {
                        return NotFound();
                    }
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
        /// Created by : Snehal Dange on 3rd May 2018
        /// Desc :  Method created to set request headers into nvc 
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
            }
            catch (Exception ex)
            {

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
        /// Modifed by : Snehal Dange on 3rd May 2018
        /// Desc :  Restuctured the code to process all details in Bal
        /// </summary>
        /// <param name="input">Customer details with price quote details</param>
        /// <returns></returns>
        [HttpPost, Route("api/PQCustomerDetail/1")]
        public IHttpActionResult PostNew([FromBody]Bikewale.DTO.PriceQuote.PQCustomerDetailInput input)
        {

            PQCustomerDetailOutput output = null;

            try
            {
                PQCustomerDetailOutputEntity outEntity = null;
                Bikewale.Entities.PriceQuote.PQCustomerDetailInput pqInput = null;
                if (input != null && !String.IsNullOrEmpty(input.CustomerMobile) && input.PQId > 0 && input.DealerId > 0)
                {
                    pqInput = PQCustomerMapper.Convert(input);
                    NameValueCollection requestHeaders = SetRequestHeaders(Request.Headers);
                    outEntity = _objLeadProcess.ProcessPQCustomerDetailInput(pqInput, requestHeaders);
                    output = PQCustomerMapper.Convert(outEntity);
                    if (output != null)
                    {
                        return Ok(output);
                    }
                    else
                    {
                        return NotFound();
                    }
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
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ResponseType(typeof(Bikewale.DTO.PriceQuote.v2.PQCustomerDetailOutput)), Route("api/PQCustomerDetailWithOutPQ"), HttpPost]
        public IHttpActionResult Postv2([FromBody]Bikewale.DTO.PriceQuote.v2.PQCustomerDetailInput input)
        {

            Bikewale.DTO.PriceQuote.v2.PQCustomerDetailOutput output = null;
            bool isSuccess = false;
            bool isVerified = false;
            string password = string.Empty, salt = string.Empty, hash = string.Empty;
            string bikeName = String.Empty;
            string imagePath = String.Empty;
            string versionName = string.Empty;

            CustomerEntity objCust = null;
            PQCustomerDetail pqCustomer = null;
            uint exShowroomCost = 0;
            UInt32 TotalPrice = 0;
            uint bookingAmount = 0;
            sbyte noOfAttempts = -1;
            UInt64 pqId = default(UInt64);
            try
            {
                if (input != null && !String.IsNullOrEmpty(input.CustomerEmail) && !String.IsNullOrEmpty(input.CustomerMobile))
                {
                    PriceQuoteParametersEntity objPQEntity = new PriceQuoteParametersEntity();
                    objPQEntity.CityId = Convert.ToUInt32(input.CityId);
                    if (Request.Headers.Contains("platformId"))
                    {
                        string platformId = Request.Headers.GetValues("platformId").First().ToString();
                        if (platformId == "3")
                        {
                            objPQEntity.SourceId = Convert.ToUInt16(Bikewale.DTO.PriceQuote.PQSources.Android);
                            objPQEntity.DeviceId = input.DeviceId;
                        }
                    }
                    objPQEntity.VersionId = Convert.ToUInt32(input.VersionId);
                    objPQEntity.DeviceId = input.DeviceId;
                    objPQEntity.PQLeadId = input.LeadSourceId;
                    objPQEntity.VersionId = Convert.ToUInt32(input.VersionId);
                    objPQEntity.DealerId = input.DealerId;
                    pqId = _objPriceQuote.RegisterPriceQuote(objPQEntity);

                    if (!_objAuthCustomer.IsRegisteredUser(input.CustomerEmail, input.CustomerMobile))
                    {
                        objCust = new CustomerEntity() { CustomerName = input.CustomerName, CustomerEmail = input.CustomerEmail, CustomerMobile = input.CustomerMobile, ClientIP = input.ClientIP };
                        UInt32 CustomerId = _objCustomer.Add(objCust);
                    }
                    else
                    {
                        var objCustomer = _objCustomer.GetByEmailMobile(input.CustomerEmail, input.CustomerMobile);
                        objCust = new CustomerEntity()
                        {
                            CustomerId = objCustomer.CustomerId,
                            CustomerName = input.CustomerName,
                            CustomerEmail = input.CustomerEmail = !String.IsNullOrEmpty(input.CustomerEmail) ? input.CustomerEmail : objCustomer.CustomerEmail,
                            CustomerMobile = input.CustomerMobile
                        };
                        _objCustomer.Update(objCust);
                    }

                    DPQ_SaveEntity entity = new DPQ_SaveEntity()
                    {
                        DealerId = input.DealerId,
                        PQId = Convert.ToUInt32(pqId),
                        CustomerName = input.CustomerName,
                        CustomerEmail = input.CustomerEmail,
                        CustomerMobile = input.CustomerMobile,
                        ColorId = null,
                        UTMA = Request.Headers.Contains("utma") ? Request.Headers.GetValues("utma").FirstOrDefault() : String.Empty,
                        UTMZ = Request.Headers.Contains("utmz") ? Request.Headers.GetValues("utmz").FirstOrDefault() : String.Empty,
                        DeviceId = input.DeviceId,
                        LeadSourceId = input.LeadSourceId
                    };

                    isSuccess = _objDealerPriceQuote.SaveCustomerDetail(entity);
                    var numberList = _mobileVerCacheRepo.GetBlockedNumbers();

                    if (numberList != null && !numberList.Contains(input.CustomerMobile))
                    {
                        //Don't mark mobile verified for pq
                        //isVerified = _objDealerPriceQuote.UpdateIsMobileVerified(input.PQId);
                        isVerified = true;// Set Verified to true to push the lead into AB for un-verified leads as well
                        pqCustomer = _objDealerPriceQuote.GetCustomerDetails(Convert.ToUInt32(pqId));
                        objCust = pqCustomer.objCustomerBase;

                        PQ_DealerDetailEntity dealerDetailEntity = null;

                        using (IUnityContainer container = new UnityContainer())
                        {
                            container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealers, Bikewale.DAL.AutoBiz.DealersRepository>();
                            Bikewale.Interfaces.AutoBiz.IDealers objDealer = container.Resolve<Bikewale.DAL.AutoBiz.DealersRepository>();
                            PQParameterEntity objParam = new PQParameterEntity();
                            objParam.CityId = Convert.ToUInt32(input.CityId);
                            objParam.DealerId = Convert.ToUInt32(input.DealerId);
                            objParam.VersionId = Convert.ToUInt32(input.VersionId);
                            dealerDetailEntity = objDealer.GetDealerDetailsPQ(objParam);
                        }

                        if (dealerDetailEntity != null && dealerDetailEntity.objQuotation != null)
                        {
                            if (dealerDetailEntity.objBookingAmt != null)
                            {
                                bookingAmount = dealerDetailEntity.objBookingAmt.Amount;
                            }

                            bool isShowroomPriceAvail = false, isBasicAvail = false;

                            foreach (var item in dealerDetailEntity.objQuotation.PriceList)
                            {
                                //Check if Ex showroom price for a bike is available CategoryId = 3 (exshowrrom)
                                if (item.CategoryId == 3)
                                {
                                    isShowroomPriceAvail = true;
                                    exShowroomCost = item.Price;
                                }

                                //if Ex showroom price for a bike is not available  then set basic cost for bike price CategoryId = 1 (basic bike cost)
                                if (!isShowroomPriceAvail && item.CategoryId == 1)
                                {
                                    exShowroomCost += item.Price;
                                    isBasicAvail = true;
                                }

                                if (item.CategoryId == 2 && !isShowroomPriceAvail)
                                    exShowroomCost += item.Price;

                                TotalPrice += item.Price;
                            }

                            if (isBasicAvail && isShowroomPriceAvail)
                                TotalPrice = TotalPrice - exShowroomCost;

                            imagePath = Bikewale.Utility.Image.GetPathToShowImages(dealerDetailEntity.objQuotation.OriginalImagePath, dealerDetailEntity.objQuotation.HostUrl, Bikewale.Utility.ImageSize._210x118);
                            bikeName = dealerDetailEntity.objQuotation.objMake.MakeName + " " + dealerDetailEntity.objQuotation.objModel.ModelName + " " + dealerDetailEntity.objQuotation.objVersion.VersionName;
                            versionName = dealerDetailEntity.objQuotation.objVersion.VersionName;
                            var platformId = "";
                            if (Request.Headers.Contains("platformId"))
                            {
                                platformId = Request.Headers.GetValues("platformId").First().ToString();
                            }

                            DPQSmsEntity objDPQSmsEntity = new DPQSmsEntity();
                            objDPQSmsEntity.CustomerMobile = objCust.CustomerMobile;
                            objDPQSmsEntity.CustomerName = objCust.CustomerName;
                            objDPQSmsEntity.DealerMobile = dealerDetailEntity.objDealer.PhoneNo;
                            objDPQSmsEntity.DealerName = dealerDetailEntity.objDealer.Organization;
                            objDPQSmsEntity.Locality = dealerDetailEntity.objDealer.Address;
                            objDPQSmsEntity.BookingAmount = bookingAmount;
                            objDPQSmsEntity.BikeName = String.Format("{0} {1} {2}", dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName);
                            objDPQSmsEntity.DealerArea = dealerDetailEntity.objDealer.objArea.AreaName != null ? dealerDetailEntity.objDealer.objArea.AreaName : string.Empty;
                            objDPQSmsEntity.DealerAdd = dealerDetailEntity.objDealer.Address;
                            objDPQSmsEntity.DealerCity = dealerDetailEntity.objDealer.objCity != null ? dealerDetailEntity.objDealer.objCity.CityName : string.Empty;
                            objDPQSmsEntity.OrganisationName = dealerDetailEntity.objDealer.Organization;

                            _objLeadNofitication.NotifyCustomer(Convert.ToUInt32(pqId), bikeName, imagePath, dealerDetailEntity.objDealer.Name,
                               dealerDetailEntity.objDealer.EmailId, dealerDetailEntity.objDealer.PhoneNo, dealerDetailEntity.objDealer.Organization,
                               dealerDetailEntity.objDealer.Address, objCust.CustomerName, objCust.CustomerEmail,
                               dealerDetailEntity.objQuotation.PriceList, dealerDetailEntity.objOffers, dealerDetailEntity.objDealer.objArea.PinCode,
                               dealerDetailEntity.objDealer.objState.StateName, dealerDetailEntity.objDealer.objCity.CityName, TotalPrice, objDPQSmsEntity,
                               "api/v2/PQCustomerDetail", input.LeadSourceId, versionName, dealerDetailEntity.objDealer.objArea.Latitude, dealerDetailEntity.objDealer.objArea.Longitude,
                               dealerDetailEntity.objDealer.WorkingTime, platformId);

                            _objLeadNofitication.NotifyDealer(Convert.ToUInt32(pqId), dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName,
                                dealerDetailEntity.objDealer.Name, dealerDetailEntity.objDealer.EmailId, objCust.CustomerName, objCust.CustomerEmail, objCust.CustomerMobile, objCust.AreaDetails.AreaName, objCust.cityDetails.CityName, dealerDetailEntity.objQuotation.PriceList, Convert.ToInt32(TotalPrice), dealerDetailEntity.objOffers, imagePath, dealerDetailEntity.objDealer.PhoneNo, bikeName, objDPQSmsEntity.DealerArea);

                            if (isVerified)
                            {
                                _objPriceQuote.SaveBookingState(Convert.ToUInt32(pqId), PriceQuoteStates.LeadSubmitted);
                                _objLeadNofitication.PushtoAB(input.DealerId.ToString(), Convert.ToUInt32(pqId), objCust.CustomerName, objCust.CustomerMobile, objCust.CustomerEmail, input.VersionId, input.CityId);
                            }
                        }

                        output = new Bikewale.DTO.PriceQuote.v2.PQCustomerDetailOutput();
                        output.IsSuccess = isVerified;
                        output.NoOfAttempts = noOfAttempts;
                        output.PQId = pqId;
                        return Ok(output);
                    }
                    else
                    {
                        return NotFound();
                    }
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
        [ResponseType(typeof(Bikewale.DTO.PriceQuote.v2.PQCustomerDetailOutput)), Route("api/PQCustomerDetailWithOutPQ"), HttpPost]
        public IHttpActionResult Postv2New([FromBody]Bikewale.DTO.PriceQuote.v2.PQCustomerDetailInput input)
        {

            Bikewale.DTO.PriceQuote.v2.PQCustomerDetailOutput output = null;

            try
            {
                if (input != null && !String.IsNullOrEmpty(input.CustomerEmail) && !String.IsNullOrEmpty(input.CustomerMobile))
                {
                    Bikewale.Entities.PriceQuote.PQCustomerDetailInput pqInput = null;
                    PQCustomerDetailOutputEntity outEntity = null;
                    pqInput = PQCustomerMapper.Convert(input);
                    NameValueCollection requestHeaders = SetRequestHeaders(Request.Headers);
                    outEntity = _objLeadProcess.ProcessPQCustomerDetailInputV1(pqInput, requestHeaders);
                    output = PQCustomerMapper.Convertv2(outEntity);//mapper
                    if (output != null)
                    {
                        return Ok(output);
                    }
                    else
                    {
                        return NotFound();
                    }

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
                entity = _objDealerPriceQuote.GetCustomerDetails(pqId);
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

    }
}
