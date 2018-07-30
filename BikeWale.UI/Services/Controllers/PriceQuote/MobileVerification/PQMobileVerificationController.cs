using Bikewale.DTO.PriceQuote;
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
using Bikewale.Service.Utilities;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote.MobileVerification
{
    /// <summary>
    /// Mobile Verification Controller
    /// Modified by :   Sumit Kate on Added Lead Notification Interface reference
    /// Modified by :   Sumit Kate on 20 May 2016
    /// Description :   Serialize the input to error message for more details
    /// </summary>
    public class PQMobileVerificationController : CompressionApiController//ApiController
    {
        private readonly IDealerPriceQuote _objDealerPriceQuote = null;
        private readonly ICustomer<CustomerEntity, UInt32> _objCustomer = null;
        private readonly IMobileVerificationRepository _mobileVerRespo = null;
        private readonly IDealer _objDealer = null;
        private readonly IPriceQuote _objPriceQuote = null;
        private readonly ILeadNofitication _objLeadNofitication = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objDealerPriceQuote"></param>
        /// <param name="mobileVerRespo"></param>
        /// <param name="objCustomer"></param>
        /// <param name="objDealer"></param>
        public PQMobileVerificationController(IDealerPriceQuote objDealerPriceQuote, IMobileVerificationRepository mobileVerRespo, ICustomer<CustomerEntity, UInt32> objCustomer, IDealer objDealer, IPriceQuote objPriceQuote, ILeadNofitication objLeadNofitication)
        {
            _objDealerPriceQuote = objDealerPriceQuote;
            _objCustomer = objCustomer;
            _mobileVerRespo = mobileVerRespo;
            _objDealer = objDealer;
            _objPriceQuote = objPriceQuote;
            _objLeadNofitication = objLeadNofitication;
        }
        /// <summary>
        /// Mobile Verification method
        /// Modified By :   Sumit Kate on 18 Nov 2015
        /// Description :   Save the State of the Booking Journey as Described in Task# 107795062 
        /// Modified By :   Lucky Rathore on 20/04/2016
        /// Description :   Changed making no. (mobile no.) of dealer to his phone no. for sms to customer.
        /// Modified by :   Sumit Kate on 02 May 2016
        /// Description :   Send the notification immediately
        /// Modified by :   Lucky Rathore on 13 May 2016
        /// Description :   New Field VersionName Intialize and NotifyCustomer() signature updated.
        /// Modified by :   Aditi Srivastava on 14 Sep 2016
        /// Description :   Changed dealer masking no to dealer phone no in DPQSmsEntity
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
            string versionName = string.Empty;

            bool isSuccess = false;
            uint exShowroomCost = 0;
            UInt32 TotalPrice = 0;
            UInt32 insuranceAmount = 0;
            bool IsInsuranceFree = false;
            bool isShowroomPriceAvail = false, isBasicAvail = false;
            uint bookingAmount = 0;
            try
            {
                if (input != null && !String.IsNullOrEmpty(input.CustomerMobile) && !String.IsNullOrEmpty(input.CwiCode) && !String.IsNullOrEmpty(input.CustomerEmail) && input.PQId > 0 && input.BranchId > 0)
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

                                pqCustomer = _objDealerPriceQuote.GetCustomerDetailsByPQId(input.PQId);
                                objCust = pqCustomer.objCustomerBase;

                                using (IUnityContainer container = new UnityContainer())
                                {
                                    container.RegisterType<Bikewale.Interfaces.AutoBiz.IDealers, Bikewale.DAL.AutoBiz.DealersRepository>();
                                    Bikewale.Interfaces.AutoBiz.IDealers objDealer = container.Resolve<Bikewale.DAL.AutoBiz.DealersRepository>();
                                    PQParameterEntity objParam = new PQParameterEntity();
                                    objParam.CityId = Convert.ToUInt32(input.CityId);
                                    objParam.DealerId = Convert.ToUInt32(input.BranchId);
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

                                    _objLeadNofitication.NotifyCustomer(input.PQId, bikeName, imagePath, dealerDetailEntity.objDealer.Organization,
                                   dealerDetailEntity.objDealer.EmailId, dealerDetailEntity.objDealer.PhoneNo, dealerDetailEntity.objDealer.Organization,
                                   dealerDetailEntity.objDealer.Address, objCust.CustomerName, objCust.CustomerEmail,
                                   dealerDetailEntity.objQuotation.PriceList, dealerDetailEntity.objOffers, dealerDetailEntity.objDealer.objArea.PinCode,
                                   dealerDetailEntity.objDealer.objState.StateName, dealerDetailEntity.objDealer.objCity.CityName, TotalPrice, objDPQSmsEntity,
                                   "api/PQMobileVerification", 0, versionName, dealerDetailEntity.objDealer.objArea.Latitude, dealerDetailEntity.objDealer.objArea.Longitude,
                                   dealerDetailEntity.objDealer.WorkingTime, platformId = "");


                                    _objPriceQuote.SaveBookingState(input.PQId, PriceQuoteStates.LeadSubmitted);
                                    _objLeadNofitication.PushtoAB(input.BranchId.ToString(), input.PQId, objCust.CustomerName, objCust.CustomerMobile, objCust.CustomerEmail, input.VersionId.ToString(), input.CityId.ToString(), string.Empty, pqCustomer.LeadId);


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
                ErrorClass.LogError(ex, String.Format("Exception : Bikewale.Service.Controllers.PriceQuote.MobileVerification.PQMobileVerificationController.Post({0})", Newtonsoft.Json.JsonConvert.SerializeObject(input)));
               
                return InternalServerError();
            }
        }

        /// <summary>
        /// Modified By : Lucky Rathore
        /// Description : change sms type to subscription model for Desktop and mobile site customer. 
        /// Modified By : Lucky Rathore on 20 April 2016
        /// Description : Declare DPQSmsEntity's city and address.
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
                objDPQSmsEntity.DealerMobile = dealerDetailEntity.objDealer.PhoneNo;
                objDPQSmsEntity.DealerName = dealerDetailEntity.objDealer.Name;
                objDPQSmsEntity.Locality = dealerDetailEntity.objDealer.Address;
                objDPQSmsEntity.BookingAmount = bookingAmount;
                objDPQSmsEntity.DealerArea = dealerDetailEntity.objDealer.objArea.AreaName != null ? dealerDetailEntity.objDealer.objArea.AreaName : string.Empty;
                objDPQSmsEntity.DealerAdd = dealerDetailEntity.objDealer.Address;
                objDPQSmsEntity.BikeName = String.Format("{0} {1} {2}", dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName);
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
                    SendEmailSMSToDealerCustomer.SendSMSToCustomer(input.PQId, "/api/PQMobileVerification", objDPQSmsEntity, DPQTypes.AndroidAppOfferNoBooking);
                }
                else
                {
                    SendEmailSMSToDealerCustomer.SendSMSToCustomer(input.PQId, "/api/PQMobileVerification", objDPQSmsEntity, DPQTypes.SubscriptionModel);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.MobileVerification.PQMobileVerificationController.SaveCustomerSMS");
               
            }
        }
    }
}
