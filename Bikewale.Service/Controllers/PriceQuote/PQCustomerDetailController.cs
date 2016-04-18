using Bikewale.BAL.Customer;
using Bikewale.DTO.PriceQuote;
using Bikewale.DTO.PriceQuote.BikeBooking;
using Bikewale.DTO.PriceQuote.CustomerDetails;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Customer;
using Bikewale.Entities.MobileVerification;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Bikebooking;
using Bikewale.Service.AutoMappers.PriceQuote;
using Bikewale.Service.TCAPI;
using Bikewale.Utility;
using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using System.Collections.Generic;
using System.Linq;
namespace Bikewale.Service.Controllers.PriceQuote
{
    /// <summary>
    /// Price Quote Customer Detail Controller
    /// Author      :   Sumit Kate
    /// Created On  :   21 Aug 2015
    /// </summary>
    public class PQCustomerDetailController : ApiController
    {
        private readonly ICustomerAuthentication<CustomerEntity, UInt32> _objAuthCustomer = null;
        private readonly ICustomer<CustomerEntity, UInt32> _objCustomer = null;
        private readonly IDealerPriceQuote _objDealerPriceQuote = null;
        private readonly IMobileVerificationRepository _mobileVerRespo = null;
        private readonly IMobileVerification _mobileVerification = null;
        private readonly IDealer _objDealer = null;
        private readonly IPriceQuote _objPriceQuote = null;
        public PQCustomerDetailController(
            ICustomerAuthentication<CustomerEntity, UInt32> objAuthCustomer,
            ICustomer<CustomerEntity, UInt32> objCustomer,
            IDealerPriceQuote objDealerPriceQuote,
            IMobileVerificationRepository mobileVerRespo,
            IMobileVerification mobileVerificetion,
            IDealer objDealer,
            IPriceQuote objPriceQuote)
        {
            _objAuthCustomer = objAuthCustomer;
            _objCustomer = objCustomer;
            _objDealerPriceQuote = objDealerPriceQuote;
            _mobileVerRespo = mobileVerRespo;
            _mobileVerification = mobileVerificetion;
            _objDealer = objDealer;
            _objPriceQuote = objPriceQuote;
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
        /// </summary>
        /// <param name="input">Customer details with price quote details</param>
        /// <returns></returns>
        [ResponseType(typeof(PQCustomerDetailOutput))]
        public IHttpActionResult Post([FromBody]PQCustomerDetailInput input)
        {

            PQCustomerDetailOutput output = null;
            bool isSuccess = false;
            bool isVerified = false;
            string password = string.Empty, salt = string.Empty, hash = string.Empty;
            string bikeName = String.Empty;
            string imagePath = String.Empty;
            CustomerEntity objCust = null;
            PQCustomerDetail pqCustomer = null;
            MobileVerificationEntity mobileVer = null;
            BookingPageDetailsEntity objBookingPageDetailsEntity = null;
            PriceQuoteParametersEntity pqParam = null;
            BookingPageDetailsDTO objBookingPageDetailsDTO = null;
            DealerDetailsDTO dealer = null;
            uint exShowroomCost = 0;
            UInt32 TotalPrice = 0;
            uint bookingAmount = 0;
            UInt32 insuranceAmount = 0;
            bool IsInsuranceFree = false;
            bool hasBumperDealerOffer = false;
            sbyte noOfAttempts = 0;
            try
            {
                if (input != null && !String.IsNullOrEmpty(input.CustomerEmail) && !String.IsNullOrEmpty(input.CustomerMobile))
                {
                    if (input != null && ((input.PQId > 0) && (Convert.ToUInt32(input.VersionId) > 0)))
                    {
                        pqParam = new PriceQuoteParametersEntity();
                        pqParam.VersionId = Convert.ToUInt32(input.VersionId);

                        _objPriceQuote.UpdatePriceQuote(input.PQId, pqParam);
                    }
                    if (!_objAuthCustomer.IsRegisteredUser(input.CustomerEmail))
                    {
                        objCust = new CustomerEntity() { CustomerName = input.CustomerName, CustomerEmail = input.CustomerEmail, CustomerMobile = input.CustomerMobile, ClientIP = input.ClientIP };
                        UInt32 CustomerId = _objCustomer.Add(objCust);
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

                    //if (!_mobileVerRespo.IsMobileVerified(input.CustomerMobile, input.CustomerEmail))
                    //{
                    //    mobileVer = _mobileVerificetion.ProcessMobileVerification(input.CustomerEmail, input.CustomerMobile);
                    //    isVerified = false;

                    //    SMSTypes st = new SMSTypes();
                    //    st.SMSMobileVerification(mobileVer.CustomerMobile, input.CustomerName, mobileVer.CWICode, input.PageUrl);
                    //}

                    noOfAttempts = _mobileVerRespo.OTPAttemptsMade(input.CustomerMobile, input.CustomerEmail);

                    //here -1 implies mobile number is verified and resend OTP attempts is 2
                    if (noOfAttempts > -1)
                    {
                        if (noOfAttempts < 3)
                        {
                            mobileVer = _mobileVerification.ProcessMobileVerification(input.CustomerEmail, input.CustomerMobile);

                            SMSTypes st = new SMSTypes();
                            st.SMSMobileVerification(mobileVer.CustomerMobile, input.CustomerName, mobileVer.CWICode, input.PageUrl);
                        }

                        isVerified = false;

                        output = new PQCustomerDetailOutput();
                        output.IsSuccess = isVerified;
                        output.NoOfAttempts = noOfAttempts;
                        output.Dealer = null;

                        return Ok(output);
                    }
                    else
                    {
                        isVerified = _objDealerPriceQuote.UpdateIsMobileVerified(input.PQId);

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
                        //objCust = _objCustomer.GetByEmail(input.CustomerEmail);

                        pqCustomer = _objDealerPriceQuote.GetCustomerDetails(input.PQId);
                        objCust = pqCustomer.objCustomerBase;

                        PQ_DealerDetailEntity dealerDetailEntity = null;

                        string _apiUrl = String.Format("/api/Dealers/GetDealerDetailsPQ/?versionId={0}&DealerId={1}&CityId={2}", input.VersionId, input.DealerId, input.CityId);
                        // Send HTTP GET requests 

                        using (Utility.BWHttpClient objClient = new Utility.BWHttpClient())
                        {
                            //dealerDetailEntity = objClient.GetApiResponseSync<PQ_DealerDetailEntity>(Utility.BWConfiguration.Instance.ABApiHostUrl, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, dealerDetailEntity);
                            dealerDetailEntity = objClient.GetApiResponseSync<PQ_DealerDetailEntity>(Utility.APIHost.AB, Utility.BWConfiguration.Instance.APIRequestTypeJSON, _apiUrl, dealerDetailEntity);
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

                            var platformId = "";
                            if (Request.Headers.Contains("platformId"))
                            {
                                platformId = Request.Headers.GetValues("platformId").First().ToString();
                            }

                            if (platformId != "3" && platformId != "4")
                            {
                                SendEmailSMSToDealerCustomer.SaveEmailToCustomer(input.PQId, bikeName, imagePath, dealerDetailEntity.objDealer.Name, dealerDetailEntity.objDealer.EmailId, dealerDetailEntity.objDealer.MobileNo, dealerDetailEntity.objDealer.Organization, dealerDetailEntity.objDealer.Address, objCust.CustomerName, objCust.CustomerEmail, dealerDetailEntity.objQuotation.PriceList, dealerDetailEntity.objOffers, dealerDetailEntity.objDealer.objArea.PinCode, dealerDetailEntity.objDealer.objState.StateName, dealerDetailEntity.objDealer.objCity.CityName, TotalPrice, insuranceAmount);
                            }
                            

                            hasBumperDealerOffer = OfferHelper.HasBumperDealerOffer(dealerDetailEntity.objDealer.DealerId.ToString(), "");
                            //if (bookingAmount > 0)
                            //{
                            //    //SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, dealerDetailEntity, objCust.CustomerMobile, objCust.CustomerName, bikeName, dealerDetailEntity.objDealer.Name, dealerDetailEntity.objDealer.MobileNo, dealerDetailEntity.objDealer.Address, bookingAmount, insuranceAmount, hasBumperDealerOffer);
                            //}

                            SaveCustomerSMS(input, objCust, dealerDetailEntity, bookingAmount);

                            //bool isDealerNotified = _objDealerPriceQuote.IsDealerNotified(input.DealerId, objCust.CustomerMobile, objCust.CustomerId);
                            //if (!isDealerNotified)
                            {
                                SendEmailSMSToDealerCustomer.SaveEmailToDealer(input.PQId, dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName, dealerDetailEntity.objDealer.Name, dealerDetailEntity.objDealer.EmailId, objCust.CustomerName, objCust.CustomerEmail, objCust.CustomerMobile, objCust.AreaDetails.AreaName, objCust.cityDetails.CityName, dealerDetailEntity.objQuotation.PriceList, Convert.ToInt32(TotalPrice), dealerDetailEntity.objOffers, imagePath, insuranceAmount);
                                SendEmailSMSToDealerCustomer.SaveSMSToDealer(input.PQId, dealerDetailEntity.objDealer.MobileNo, objCust.CustomerName, objCust.CustomerMobile, bikeName, objCust.AreaDetails.AreaName, objCust.cityDetails.CityName);
                            }

                            // If customer is mobile verified push lead to autobiz
                            if (isVerified)
                            {
                                _objPriceQuote.SaveBookingState(input.PQId, PriceQuoteStates.LeadSubmitted);
                                //AutoBizAdaptor.PushInquiryInAB(input.DealerId.ToString(), input.PQId, input.CustomerName, input.CustomerMobile, input.CustomerEmail, input.VersionId, input.CityId);
                            }
                        }
                    }
                    if (isVerified)
                    {
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.PQCustomerDetailController.Post");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        /// <summary>
        /// Modified by : Lucky Rathore
        /// Created On : 17 March 2016
        /// Description : Dealer Area and address added in DPQSmsEntity.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="objCust"></param>
        /// <param name="dealerDetailEntity"></param>
        /// <param name="bookingAmount"></param>
        private void SaveCustomerSMS(PQCustomerDetailInput input, CustomerEntity objCust, PQ_DealerDetailEntity dealerDetailEntity, uint bookingAmount)
        {
            UrlShortner objUrlShortner = new UrlShortner();
            try
            {
                DPQSmsEntity objDPQSmsEntity = new DPQSmsEntity();                
                objDPQSmsEntity.CustomerMobile = objCust.CustomerMobile;
                objDPQSmsEntity.CustomerName = objCust.CustomerName;
                objDPQSmsEntity.DealerMobile = dealerDetailEntity.objDealer.MobileNo;
                objDPQSmsEntity.DealerName = dealerDetailEntity.objDealer.Organization;
                objDPQSmsEntity.Locality = dealerDetailEntity.objDealer.Address;
                objDPQSmsEntity.BookingAmount = bookingAmount;
                objDPQSmsEntity.BikeName = String.Format("{0} {1} {2}", dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName);
                objDPQSmsEntity.DealerArea = dealerDetailEntity.objDealer.objArea.AreaName;
                objDPQSmsEntity.DealerAdd = dealerDetailEntity.objDealer.Address;
                PriceQuoteParametersEntity pqEntity = _objPriceQuote.FetchPriceQuoteDetailsById(input.PQId);
                String mpqQueryString = String.Format("CityId={0}&AreaId={1}&PQId={2}&VersionId={3}&DealerId={4}", pqEntity.CityId, pqEntity.AreaId, input.PQId, pqEntity.VersionId, pqEntity.DealerId);
                objDPQSmsEntity.LandingPageShortUrl = objUrlShortner.GetShortUrl(String.Format("{0}/pricequote/BikeDealerDetails.aspx?MPQ={1}", BWConfiguration.Instance.BwHostUrlForJs, EncodingDecodingHelper.EncodeTo64(mpqQueryString))).Id;
                var platformId = "";
                if (Request.Headers.Contains("platformId"))
                {
                    platformId = Request.Headers.GetValues("platformId").First().ToString();
                }

                if (!string.IsNullOrEmpty(platformId) && (platformId == "3" || platformId == "4"))
                {
                    SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, "/api/PQCustomerDetail", objDPQSmsEntity, DPQTypes.AndroidAppOfferNoBooking);
                }
                else
                {
                    SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, "/api/PQCustomerDetail", objDPQSmsEntity, DPQTypes.SubscriptionModel);
                    
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.PQCustomerDetailController.SaveCustomerSMS");
                objErr.SendMail();
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.PQCustomerDetailController.Get");
                objErr.SendMail();
                return InternalServerError();
            }
        }

    }
}