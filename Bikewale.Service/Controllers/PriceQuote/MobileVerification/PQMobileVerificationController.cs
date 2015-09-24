using Bikewale.DTO.PriceQuote;
using Bikewale.DTO.PriceQuote.BikeBooking;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Customer;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.Customer;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.Bikebooking;
using Bikewale.Service.TCAPI;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

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
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="objDealerPriceQuote"></param>
        /// <param name="mobileVerRespo"></param>
        /// <param name="objCustomer"></param>
        /// <param name="objDealer"></param>
        public PQMobileVerificationController(IDealerPriceQuote objDealerPriceQuote, IMobileVerificationRepository mobileVerRespo, ICustomer<CustomerEntity, UInt32> objCustomer, IDealer objDealer)
        {
            _objDealerPriceQuote = objDealerPriceQuote;
            _objCustomer = objCustomer;
            _mobileVerRespo = mobileVerRespo;
            _objDealer = objDealer;
        }
        /// <summary>
        /// Mobile Verification method
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
            try
            {
                if (input!=null && !String.IsNullOrEmpty(input.CustomerMobile) && !String.IsNullOrEmpty(input.CwiCode) && !String.IsNullOrEmpty(input.CustomerEmail))
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
                                dealer = objBookingPageDetailsDTO.Dealer;
                                objCust = _objCustomer.GetByEmail(input.CustomerEmail);
                                                                
                                string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                                string _requestType = "application/json";

                                string _apiUrl = String.Format("/api/Dealers/GetDealerDetailsPQ/?versionId={0}&DealerId={1}&CityId={2}", input.VersionId, input.BranchId, input.CityId);
                                // Send HTTP GET requests 

                                dealerDetailEntity = BWHttpClient.GetApiResponseSync<PQ_DealerDetailEntity>(_abHostUrl, _requestType, _apiUrl, dealerDetailEntity);

                                if (dealerDetailEntity !=null && dealerDetailEntity.objQuotation != null)
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
                                    SendEmailSMSToDealerCustomer.SendEmailToCustomer(bikeName, imagePath, dealerDetailEntity.objDealer.Name, dealerDetailEntity.objDealer.EmailId, dealerDetailEntity.objDealer.MobileNo, dealerDetailEntity.objDealer.Organization, dealerDetailEntity.objDealer.Address, objCust.CustomerName, objCust.CustomerEmail, dealerDetailEntity.objQuotation.PriceList, dealerDetailEntity.objOffers, dealerDetailEntity.objDealer.objArea.PinCode, dealerDetailEntity.objDealer.objState.StateName, dealerDetailEntity.objDealer.objCity.CityName, TotalPrice, insuranceAmount);
                                    SendEmailSMSToDealerCustomer.SMSToCustomer(objCust.CustomerMobile, objCust.CustomerName, bikeName, dealerDetailEntity.objDealer.Name, dealerDetailEntity.objDealer.MobileNo, dealerDetailEntity.objDealer.Address, bookingAmount, insuranceAmount);
                                    bool isDealerNotified = _objDealerPriceQuote.IsDealerNotified(input.BranchId, objCust.CustomerMobile, objCust.CustomerId);
                                    if (!isDealerNotified)
                                    {
                                        SendEmailSMSToDealerCustomer.SendEmailToDealer(dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName, dealerDetailEntity.objDealer.Name, dealerDetailEntity.objDealer.EmailId, objCust.CustomerName, objCust.CustomerEmail, objCust.CustomerMobile, objCust.AreaDetails.AreaName, objCust.cityDetails.CityName, dealerDetailEntity.objQuotation.PriceList, Convert.ToInt32(TotalPrice), dealerDetailEntity.objOffers, insuranceAmount);
                                        SendEmailSMSToDealerCustomer.SMSToDealer(dealerDetailEntity.objDealer.MobileNo, objCust.CustomerName, objCust.CustomerMobile, bikeName, objCust.AreaDetails.AreaName, objCust.cityDetails.CityName);
                                    }

                                    AutoBizAdaptor.PushInquiryInAB(input.BranchId.ToString(), input.PQId, input.CustomerName, input.CustomerMobile, input.CustomerEmail, input.VersionId.ToString(), input.CityId.ToString()); 
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
    }
}
