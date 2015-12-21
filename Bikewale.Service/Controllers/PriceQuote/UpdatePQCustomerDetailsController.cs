using Bikewale.DTO.PriceQuote;
using Bikewale.DTO.PriceQuote.BikeBooking;
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
using Bikewale.Utility;
using System;
using System.Configuration;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote
{
    /// <summary>
    /// Price Quote Customer Detail Controller with VersionId and ColorId Updation
    /// Author      :   Sushil Kumar
    /// Created On  :   12th December 2015
    /// </summary>
    public class UpdatePQCustomerDetailsController : ApiController
    {
        private readonly ICustomerAuthentication<CustomerEntity, UInt32> _objAuthCustomer = null;
        private readonly ICustomer<CustomerEntity, UInt32> _objCustomer = null;
        private readonly IDealerPriceQuote _objDealerPriceQuote = null;
        private readonly IMobileVerificationRepository _mobileVerRespo = null;
        private readonly IMobileVerification _mobileVerification = null;
        private readonly IDealer _objDealer = null;
        private readonly IPriceQuote _objPriceQuote = null;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objAuthCustomer"></param>
        /// <param name="objCustomer"></param>
        /// <param name="objDealerPriceQuote"></param>
        /// <param name="mobileVerRespo"></param>
        /// <param name="mobileVerificetion"></param>
        /// <param name="objDealer"></param>
        /// <param name="objPriceQuote"></param>
        public UpdatePQCustomerDetailsController(
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
        /// Similar to PQCustomer Controller but updates color and version
        /// Saves the Customer details if it is a new customer else updates version/color  for the same.
        /// Generates  the OTP for the non verified customer
        /// </summary>
        /// <param name="input">Customer details with price quote details</param>
        /// <returns></returns>
        [ResponseType(typeof(UpdatePQCustomerOutput))]
        public IHttpActionResult Post([FromBody]UpdatePQCustomerInput input)
        {

            UpdatePQCustomerOutput output = null;
            bool isSuccess = false;
            bool isVerified = false;
            string password = string.Empty, salt = string.Empty, hash = string.Empty;
            string bikeName = String.Empty;
            string imagePath = String.Empty;
            CustomerEntity objCust = null;
            MobileVerificationEntity mobileVer = null;
            PriceQuoteParametersEntity pqParam = null;
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

                    isSuccess = _objDealerPriceQuote.SaveCustomerDetail(input.DealerId, input.PQId, input.CustomerName, input.CustomerMobile, input.CustomerEmail,input.ColorId);

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
                        output = new UpdatePQCustomerOutput();
                        output.IsSuccess = isVerified;
                        output.NoOfAttempts = noOfAttempts;

                        return Ok(output);
                    }
                    else
                    {
                        isVerified = _objDealerPriceQuote.UpdateIsMobileVerified(input.PQId);

                        //objBookingPageDetailsEntity = _objDealerPriceQuote.FetchBookingPageDetails(Convert.ToUInt32(input.CityId), Convert.ToUInt32(input.VersionId), input.DealerId);
                        //objBookingPageDetailsDTO = BookingPageDetailsEntityMapper.Convert(objBookingPageDetailsEntity);

                        //if (objBookingPageDetailsEntity != null)
                        //{
                        //    objBookingPageDetailsEntity.BikeModelColors = null;

                        //    if (objBookingPageDetailsEntity.Disclaimers != null)
                        //    {
                        //        objBookingPageDetailsEntity.Disclaimers.Clear();
                        //        objBookingPageDetailsEntity.Disclaimers = null;
                        //    }

                        //    if (objBookingPageDetailsEntity.Offers != null)
                        //    {
                        //        objBookingPageDetailsEntity.Offers.Clear();
                        //        objBookingPageDetailsEntity.Offers = null;
                        //    }

                        //    if (objBookingPageDetailsEntity.Varients != null)
                        //    {
                        //        objBookingPageDetailsEntity.Varients.Clear();
                        //        objBookingPageDetailsEntity.Varients = null;
                        //    }
                        //}

                       // dealer = objBookingPageDetailsDTO.Dealer;
                        objCust = _objCustomer.GetByEmail(input.CustomerEmail);

                        PQ_DealerDetailEntity dealerDetailEntity = null;
                        string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
                        string _requestType = "application/json";

                        string _apiUrl = String.Format("/api/Dealers/GetDealerDetailsPQ/?versionId={0}&DealerId={1}&CityId={2}", input.VersionId, input.DealerId, input.CityId);
                        // Send HTTP GET requests 

                        using (BWHttpClient objClient = new BWHttpClient())
                        {
                           dealerDetailEntity = objClient.GetApiResponseSync<PQ_DealerDetailEntity>(APIHost.AB, _requestType, _apiUrl, dealerDetailEntity);
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
                            SendEmailSMSToDealerCustomer.SaveEmailToCustomer(input.PQId, bikeName, imagePath, dealerDetailEntity.objDealer.Name, dealerDetailEntity.objDealer.EmailId, dealerDetailEntity.objDealer.MobileNo, dealerDetailEntity.objDealer.Organization, dealerDetailEntity.objDealer.Address, objCust.CustomerName, objCust.CustomerEmail, dealerDetailEntity.objQuotation.PriceList, dealerDetailEntity.objOffers, dealerDetailEntity.objDealer.objArea.PinCode, dealerDetailEntity.objDealer.objState.StateName, dealerDetailEntity.objDealer.objCity.CityName, TotalPrice, insuranceAmount);

                            hasBumperDealerOffer = OfferHelper.HasBumperDealerOffer(dealerDetailEntity.objDealer.DealerId.ToString(), "");
                            if (bookingAmount > 0)
                            {
                                SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, dealerDetailEntity, objCust.CustomerMobile, objCust.CustomerName, bikeName, dealerDetailEntity.objDealer.Name, dealerDetailEntity.objDealer.MobileNo, dealerDetailEntity.objDealer.Address, bookingAmount, insuranceAmount, hasBumperDealerOffer);
                            }

                            bool isDealerNotified = _objDealerPriceQuote.IsDealerNotified(input.DealerId, objCust.CustomerMobile, objCust.CustomerId);
                            if (!isDealerNotified)
                            {
                                SendEmailSMSToDealerCustomer.SaveEmailToDealer(input.PQId, dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName, dealerDetailEntity.objDealer.Name, dealerDetailEntity.objDealer.EmailId, objCust.CustomerName, objCust.CustomerEmail, objCust.CustomerMobile, objCust.AreaDetails.AreaName, objCust.cityDetails.CityName, dealerDetailEntity.objQuotation.PriceList, Convert.ToInt32(TotalPrice), dealerDetailEntity.objOffers, insuranceAmount);
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
                        output = new UpdatePQCustomerOutput();
                        output.IsSuccess = isVerified;
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
    }
}
