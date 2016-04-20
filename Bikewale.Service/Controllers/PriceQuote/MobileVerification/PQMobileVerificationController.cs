﻿using Bikewale.DTO.PriceQuote;
using Bikewale.DTO.PriceQuote.BikeBooking;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Customer;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Bikebooking;
using Bikewale.Service.TCAPI;
using Bikewale.Utility;
using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Description;
using System.Linq;

namespace Bikewale.Service.Controllers.PriceQuote.MobileVerification
{
    /// <summary>
    /// Mobile Verification Controller
    /// </summary>
    public class PQMobileVerificationController : ApiController
    {
        private readonly IDealerPriceQuote _objDealerPriceQuote = null;
        private readonly ICustomer<CustomerEntity, UInt32> _objCustomer = null;
        private readonly IMobileVerificationRepository _mobileVerRespo = null;
        private readonly IDealer _objDealer = null;
        private readonly IPriceQuote _objPriceQuote = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objDealerPriceQuote"></param>
        /// <param name="mobileVerRespo"></param>
        /// <param name="objCustomer"></param>
        /// <param name="objDealer"></param>
        public PQMobileVerificationController(IDealerPriceQuote objDealerPriceQuote, IMobileVerificationRepository mobileVerRespo, ICustomer<CustomerEntity, UInt32> objCustomer, IDealer objDealer, IPriceQuote objPriceQuote)
        {
            _objDealerPriceQuote = objDealerPriceQuote;
            _objCustomer = objCustomer;
            _mobileVerRespo = mobileVerRespo;
            _objDealer = objDealer;
            _objPriceQuote = objPriceQuote;
        }
        /// <summary>
        /// Mobile Verification method
        /// Modified By :   Sumit Kate on 18 Nov 2015
        /// Description :   Save the State of the Booking Journey as Described in Task# 107795062 
        /// </summary>
        /// <param name="input">Mobile Verification Input</param>
        /// <returns></returns>
        [ResponseType(typeof(PQMobileVerificationOutput))]
        public IHttpActionResult Post([FromBody]PQMobileVerificationInput input)
        {
            BookingPageDetailsEntity objBookingPageDetailsEntity = null;
            BookingPageDetailsDTO objBookingPageDetailsDTO = null;
            PQMobileVerificationOutput output = null;
            CustomerEntity objCust = null;
            PQCustomerDetail pqCustomer = null;
            DealerDetailsDTO dealer = null;
            PQ_DealerDetailEntity dealerDetailEntity = null;
            string bikeName = string.Empty;
            string imagePath = string.Empty;
            bool isSuccess = false;
            uint exShowroomCost = 0;
            UInt32 TotalPrice = 0;
            UInt32 insuranceAmount = 0;
            bool IsInsuranceFree = false;
            bool isShowroomPriceAvail = false, isBasicAvail = false;
            uint bookingAmount = 0;
            bool hasBumperDealerOffer = false;
            try
            {
                if (input != null && !String.IsNullOrEmpty(input.CustomerMobile) && !String.IsNullOrEmpty(input.CwiCode) && !String.IsNullOrEmpty(input.CustomerEmail))
                {
                    if (!_mobileVerRespo.IsMobileVerified(input.CustomerMobile, input.CustomerEmail))
                    {
                        if (_mobileVerRespo.VerifyMobileVerificationCode(input.CustomerMobile, input.CwiCode, ""))
                        {
                            isSuccess = _objDealerPriceQuote.UpdateIsMobileVerified(input.PQId);

                            // if mobile no is verified push lead to autobiz
                            if (isSuccess)
                            {
                                objBookingPageDetailsEntity = _objDealerPriceQuote.FetchBookingPageDetails(input.CityId, input.VersionId, input.BranchId);
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

                                string _apiUrl = String.Format("/api/Dealers/GetDealerDetailsPQ/?versionId={0}&DealerId={1}&CityId={2}", input.VersionId, input.BranchId, input.CityId);
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
                                        IsInsuranceFree = OfferHelper.HasFreeInsurance(input.BranchId.ToString(), dealerDetailEntity.objQuotation.objModel.ModelId.ToString(), price.CategoryName, price.Price, ref insuranceAmount);
                                    }
                                    if (insuranceAmount > 0)
                                    {
                                        IsInsuranceFree = true;
                                    }

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
                       

                                    //bool isDealerNotified = _objDealerPriceQuote.IsDealerNotified(input.BranchId, objCust.CustomerMobile, objCust.CustomerId);
                                    //if (!isDealerNotified)
                                    {
                                        SendEmailSMSToDealerCustomer.SaveEmailToDealer(input.PQId, dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName, dealerDetailEntity.objDealer.Name, dealerDetailEntity.objDealer.EmailId, objCust.CustomerName, objCust.CustomerEmail, objCust.CustomerMobile, objCust.AreaDetails.AreaName, objCust.cityDetails.CityName, dealerDetailEntity.objQuotation.PriceList, Convert.ToInt32(TotalPrice), dealerDetailEntity.objOffers, imagePath, insuranceAmount);
                                        SendEmailSMSToDealerCustomer.SaveSMSToDealer(input.PQId, dealerDetailEntity.objDealer.PhoneNo, objCust.CustomerName, objCust.CustomerMobile, bikeName, objCust.AreaDetails.AreaName, objCust.cityDetails.CityName);
                                    }

                                    if (dealerDetailEntity.objFacilities != null)
                                    {
                                        dealerDetailEntity.objFacilities.Clear();
                                        dealerDetailEntity.objFacilities = null;
                                    }

                                    if (dealerDetailEntity.objOffers != null)
                                    {
                                        dealerDetailEntity.objOffers.Clear();
                                        dealerDetailEntity.objOffers = null;
                                    }

                                    if (dealerDetailEntity.objQuotation != null)
                                    {
                                        if (dealerDetailEntity.objQuotation.PriceList != null)
                                        {
                                            dealerDetailEntity.objQuotation.PriceList.Clear();
                                            dealerDetailEntity.objQuotation.PriceList = null;
                                        }

                                        if (dealerDetailEntity.objQuotation.objOffers != null)
                                        {
                                            dealerDetailEntity.objQuotation.objOffers.Clear();
                                            dealerDetailEntity.objQuotation.objOffers = null;
                                        }

                                        dealerDetailEntity.objQuotation.Varients = null;
                                    }
                                    _objPriceQuote.SaveBookingState(input.PQId, PriceQuoteStates.LeadSubmitted);
                                    //AutoBizAdaptor.PushInquiryInAB(input.BranchId.ToString(), input.PQId, input.CustomerName, input.CustomerMobile, input.CustomerEmail, input.VersionId.ToString(), input.CityId.ToString()); 
                                }
                            }
                        }
                    }
                    if (isSuccess)
                    {
                        output = new PQMobileVerificationOutput();
                        output.IsSuccess = isSuccess;
                        output.Dealer = dealer;
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.MobileVerification.PQMobileVerificationController.Post");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        /// <summary>
        /// Modified By : Lucky Rathore
        /// Description : change sms type to subscription model for Desktop and mobile site customer. 
        /// </summary>
        /// <param name="input"></param>
        /// <param name="objCust"></param>
        /// <param name="dealerDetailEntity"></param>
        /// <param name="bookingAmount"></param>
        private void SaveCustomerSMS(PQMobileVerificationInput input, CustomerEntity objCust, PQ_DealerDetailEntity dealerDetailEntity, uint bookingAmount)
        {
            UrlShortner objUrlShortner = new UrlShortner();
            DPQSmsEntity objDPQSmsEntity = new DPQSmsEntity();
            try
            {
                objDPQSmsEntity.CustomerMobile = objCust.CustomerMobile;
                objDPQSmsEntity.CustomerName = objCust.CustomerName;
                objDPQSmsEntity.DealerMobile = dealerDetailEntity.objDealer.MobileNo;
                objDPQSmsEntity.DealerName = dealerDetailEntity.objDealer.Name;
                objDPQSmsEntity.Locality = dealerDetailEntity.objDealer.Address;
                objDPQSmsEntity.BookingAmount = bookingAmount;
                objDPQSmsEntity.DealerArea = dealerDetailEntity.objDealer.objArea.AreaName != null ? dealerDetailEntity.objDealer.objArea.AreaName : string.Empty; 
                objDPQSmsEntity.DealerAdd = dealerDetailEntity.objDealer.Address;
                objDPQSmsEntity.BikeName = String.Format("{0} {1} {2}",dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName);
                objDPQSmsEntity.DealerCity = dealerDetailEntity.objDealer.objCity != null ? dealerDetailEntity.objDealer.objCity.CityName : string.Empty;
                objDPQSmsEntity.OrganisationName = dealerDetailEntity.objDealer.Organization;
                
                PriceQuoteParametersEntity pqEntity = _objPriceQuote.FetchPriceQuoteDetailsById(input.PQId);
                
                var platformId = "";
                if (Request.Headers.Contains("platformId"))
                {
                    platformId = Request.Headers.GetValues("platformId").First().ToString();
                }

                if (!string.IsNullOrEmpty(platformId) && (platformId == "3" || platformId == "4"))
                {
                    SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, "/api/PQMobileVerification", objDPQSmsEntity, DPQTypes.AndroidAppOfferNoBooking);
                }
                else
                {
                    SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, "/api/PQMobileVerification", objDPQSmsEntity, DPQTypes.SubscriptionModel);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.MobileVerification.PQMobileVerificationController.SaveCustomerSMS");
                objErr.SendMail();
            }
        }
    }
}
