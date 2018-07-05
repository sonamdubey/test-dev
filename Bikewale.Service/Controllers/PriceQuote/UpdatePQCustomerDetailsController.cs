using Bikewale.DTO.PriceQuote;
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
using Bikewale.Service.Utilities;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote
{
    /// <summary>
    /// Price Quote Customer Detail Controller with VersionId and ColorId Updation
    /// Author      :   Sushil Kumar
    /// Created On  :   12th December 2015
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class UpdatePQCustomerDetailsController : CompressionApiController//ApiController
    {
        private readonly ICustomerAuthentication<CustomerEntity, UInt32> _objAuthCustomer = null;
        private readonly ICustomer<CustomerEntity, UInt32> _objCustomer = null;
        private readonly IDealerPriceQuote _objDealerPriceQuote = null;
        private readonly IMobileVerificationRepository _mobileVerRespo = null;
        private readonly IMobileVerification _mobileVerification = null;
        private readonly IDealer _objDealer = null;
        private readonly IPriceQuote _objPriceQuote = null;
        private readonly ILeadNofitication _objLeadNofitication = null;
        /// <summary>
        /// Modified By : Sadhana Upadhyay on 29 Dec 2015
        /// Summary : To capture device id, utma, utmz, Pq lead id etc.
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
            IPriceQuote objPriceQuote, ILeadNofitication objLeadNofitication)
        {
            _objAuthCustomer = objAuthCustomer;
            _objCustomer = objCustomer;
            _objDealerPriceQuote = objDealerPriceQuote;
            _mobileVerRespo = mobileVerRespo;
            _mobileVerification = mobileVerificetion;
            _objDealer = objDealer;
            _objPriceQuote = objPriceQuote;
            _objLeadNofitication = objLeadNofitication;
        }
        /// <summary>
        /// Similar to PQCustomer Controller but updates color and version
        /// Saves the Customer details if it is a new customer else updates version/color  for the same.
        /// Generates  the OTP for the non verified customer
        /// Modified By : Lucky Rathore on 20/04/2016
        /// Description : Changed making no. (mobile no.) of dealer to his phone no. for sms to customer.
        /// Modified by :   Sumit Kate on 02 May 2016
        /// Description :   Send the notification immediately
        /// Modified by :   Lucky Rathore on 13 May 2016
        /// Description :   var versionName declare, Intialized and NotifyCustomer() singature Updated.
        /// Modified by :   Sumit Kate on 01 June 2016
        /// Description :   Commented Mobile verification process
        /// noOfAttempts = -1 and isVerified = true to by pass the Mobile Verification Process
        /// MOdified by : Pratibha Verma on 27 April 2018
        /// Description : added parameters additionalnumbers and additionalemails in NotifyDealer()
        /// </summary>
        /// <param name="input">Customer details with price quote details</param>
        /// <returns></returns>
        [ResponseType(typeof(UpdatePQCustomerOutput))]
        public IHttpActionResult Post([FromBody]UpdatePQCustomerInput input)
        {

            UpdatePQCustomerOutput output = null;
            bool isVerified = false;
            string password = string.Empty, salt = string.Empty, hash = string.Empty;
            string bikeName = String.Empty;
            string imagePath = String.Empty;
            string versionName = string.Empty;
            CustomerEntity objCust = null;
            PQCustomerDetail pqCustomer = null;
            MobileVerificationEntity mobileVer = null;
            PriceQuoteParametersEntity pqParam = null;
            uint exShowroomCost = 0;
            UInt32 TotalPrice = 0;
            uint bookingAmount = 0;
            UInt32 insuranceAmount = 0;
            bool IsInsuranceFree = false;
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
                    if (!_objAuthCustomer.IsRegisteredUser(input.CustomerEmail, input.CustomerMobile))
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
                        ColorId = input.ColorId,
                        UTMA = Request.Headers.Contains("utma") ? Request.Headers.GetValues("utma").FirstOrDefault() : String.Empty,
                        UTMZ = Request.Headers.Contains("utmz") ? Request.Headers.GetValues("utmz").FirstOrDefault() : String.Empty,
                        DeviceId = input.DeviceId,
                        LeadSourceId = input.LeadSourceId
                    };
                    input.LeadId = _objDealerPriceQuote.SaveCustomerDetailByPQId(entity);

                    //noOfAttempts = _mobileVerRespo.OTPAttemptsMade(input.CustomerMobile, input.CustomerEmail);
                    noOfAttempts = -1; // By-pass the mobile verification by setting noOfAttempts = -1
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
                        //Don't mark mobile verified for pq
                        //isVerified = _objDealerPriceQuote.UpdateIsMobileVerified(input.PQId);
                        isVerified = true; // Set Verified to true to push the lead into AB for un-verified leads as well

                        pqCustomer = _objDealerPriceQuote.GetCustomerDetailsByLeadId(input.LeadId);
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
                        if (dealerDetailEntity != null && dealerDetailEntity.objDealer != null)
                        {
                            objDPQSmsEntity.DealerMobile = dealerDetailEntity.objDealer.MobileNo;
                            objDPQSmsEntity.DealerName = dealerDetailEntity.objDealer.Organization;
                            objDPQSmsEntity.Locality = dealerDetailEntity.objDealer.Address;
                            objDPQSmsEntity.BookingAmount = bookingAmount;
                            objDPQSmsEntity.BikeName = String.Format("{0} {1} {2}", dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName);
                            objDPQSmsEntity.DealerArea = dealerDetailEntity.objDealer.objArea.AreaName != null ? dealerDetailEntity.objDealer.objArea.AreaName : string.Empty;
                            objDPQSmsEntity.DealerAdd = dealerDetailEntity.objDealer.Address;
                            objDPQSmsEntity.DealerCity = dealerDetailEntity.objDealer.objCity != null ? dealerDetailEntity.objDealer.objCity.CityName : string.Empty;
                            objDPQSmsEntity.OrganisationName = dealerDetailEntity.objDealer.Organization;

                            string custArea = objCust.AreaDetails == null ? string.Empty : objCust.AreaDetails.AreaName,
                                custCity = objCust.cityDetails == null ? string.Empty : objCust.cityDetails.CityName;

                            _objLeadNofitication.NotifyDealer(input.PQId, dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName,
                                dealerDetailEntity.objDealer.Organization, dealerDetailEntity.objDealer.EmailId, objCust.CustomerName, objCust.CustomerEmail, objCust.CustomerMobile, custArea, custCity, dealerDetailEntity.objQuotation.PriceList, Convert.ToInt32(TotalPrice), dealerDetailEntity.objOffers, imagePath, dealerDetailEntity.objDealer.PhoneNo, bikeName, objDPQSmsEntity.DealerArea, dealerDetailEntity.objDealer.AdditionalNumbers, dealerDetailEntity.objDealer.AdditionalEmails);
                        }

                        if (isVerified)
                        {
                            _objPriceQuote.SaveBookingState(input.PQId, PriceQuoteStates.LeadSubmitted);
                            _objLeadNofitication.PushtoAB(input.DealerId.ToString(), input.PQId, objCust.CustomerName, objCust.CustomerMobile, objCust.CustomerEmail, input.VersionId, input.CityId, String.Empty, input.LeadId);
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.PQCustomerDetailController.Post");
               
                return InternalServerError();
            }
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 26 June 2018
        /// Description : new api to pass leadId in order to remove PQId dependency
        /// Modified by : Rajan Chauhan on 27 June 2018
        /// Description : Set correct output and set LeadId in output and SaveCustomerDetailByLeadId input
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ResponseType(typeof(Bikewale.DTO.PriceQuote.v2.UpdatePQCustomerOutput)), Route("api/v1/UpdatePQCustomerDetails/")]
        public IHttpActionResult Post([FromBody]Bikewale.DTO.PriceQuote.v2.UpdatePQCustomerInput input)
        {

            Bikewale.DTO.PriceQuote.v2.UpdatePQCustomerOutput output = null;
            bool isVerified = false;
            string password = string.Empty, salt = string.Empty, hash = string.Empty;
            string bikeName = String.Empty;
            string imagePath = String.Empty;
            string versionName = string.Empty;
            CustomerEntity objCust = null;
            PQCustomerDetail pqCustomer = null;
            MobileVerificationEntity mobileVer = null;
            PriceQuoteParametersEntity pqParam = null;
            uint exShowroomCost = 0;
            UInt32 TotalPrice = 0;
            uint bookingAmount = 0;
            UInt32 insuranceAmount = 0;
            bool IsInsuranceFree = false;
            sbyte noOfAttempts = 0;
            try
            {
                if (input != null && !String.IsNullOrEmpty(input.CustomerEmail) && !String.IsNullOrEmpty(input.CustomerMobile))
                {
                    if (input != null && !String.IsNullOrEmpty(input.PQId) && Convert.ToUInt32(input.VersionId) > 0)
                    {
                        pqParam = new PriceQuoteParametersEntity();
                        pqParam.VersionId = Convert.ToUInt32(input.VersionId);
                    }
                    if (!_objAuthCustomer.IsRegisteredUser(input.CustomerEmail, input.CustomerMobile))
                    {
                        objCust = new CustomerEntity() { CustomerName = input.CustomerName, CustomerEmail = input.CustomerEmail, CustomerMobile = input.CustomerMobile, ClientIP = input.ClientIP };
                        UInt32 CustomerId = _objCustomer.Add(objCust);
                    }

                    Bikewale.Entities.BikeBooking.v2.DPQ_SaveEntity entity = new Bikewale.Entities.BikeBooking.v2.DPQ_SaveEntity()
                    {
                        DealerId = input.DealerId,
                        PQId = input.PQId,
                        CustomerName = input.CustomerName,
                        CustomerEmail = input.CustomerEmail,
                        CustomerMobile = input.CustomerMobile,
                        ColorId = input.ColorId,
                        UTMA = Request.Headers.Contains("utma") ? Request.Headers.GetValues("utma").FirstOrDefault() : String.Empty,
                        UTMZ = Request.Headers.Contains("utmz") ? Request.Headers.GetValues("utmz").FirstOrDefault() : String.Empty,
                        DeviceId = input.DeviceId,
                        LeadSourceId = input.LeadSourceId,
                        LeadId = input.LeadId,
                        VersionId = Convert.ToUInt32(input.VersionId),
                        CityId = Convert.ToUInt32(input.CityId),
                        AreaId = Convert.ToUInt32(input.AreaId)
                    };
                    input.LeadId = _objDealerPriceQuote.SaveCustomerDetailByLeadId(entity);

                    //noOfAttempts = _mobileVerRespo.OTPAttemptsMade(input.CustomerMobile, input.CustomerEmail);
                    noOfAttempts = -1; // By-pass the mobile verification by setting noOfAttempts = -1
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
                        output = new Bikewale.DTO.PriceQuote.v2.UpdatePQCustomerOutput();
                        output.IsSuccess = isVerified;
                        output.NoOfAttempts = noOfAttempts;
                        output.LeadId = input.LeadId;
                        return Ok(output);
                    }
                    else
                    {
                        //Don't mark mobile verified for pq
                        //isVerified = _objDealerPriceQuote.UpdateIsMobileVerified(input.PQId);
                        isVerified = true; // Set Verified to true to push the lead into AB for un-verified leads as well

                        pqCustomer = _objDealerPriceQuote.GetCustomerDetailsByLeadId(input.LeadId);
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
                        if (dealerDetailEntity != null && dealerDetailEntity.objDealer != null)
                        {
                            objDPQSmsEntity.DealerMobile = dealerDetailEntity.objDealer.MobileNo;
                            objDPQSmsEntity.DealerName = dealerDetailEntity.objDealer.Organization;
                            objDPQSmsEntity.Locality = dealerDetailEntity.objDealer.Address;
                            objDPQSmsEntity.BookingAmount = bookingAmount;
                            objDPQSmsEntity.BikeName = String.Format("{0} {1} {2}", dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName);
                            objDPQSmsEntity.DealerArea = dealerDetailEntity.objDealer.objArea.AreaName != null ? dealerDetailEntity.objDealer.objArea.AreaName : string.Empty;
                            objDPQSmsEntity.DealerAdd = dealerDetailEntity.objDealer.Address;
                            objDPQSmsEntity.DealerCity = dealerDetailEntity.objDealer.objCity != null ? dealerDetailEntity.objDealer.objCity.CityName : string.Empty;
                            objDPQSmsEntity.OrganisationName = dealerDetailEntity.objDealer.Organization;

                            string custArea = objCust.AreaDetails == null ? string.Empty : objCust.AreaDetails.AreaName,
                                custCity = objCust.cityDetails == null ? string.Empty : objCust.cityDetails.CityName;

                            _objLeadNofitication.NotifyDealerV2(input.PQId, dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName,
                                dealerDetailEntity.objDealer.Organization, dealerDetailEntity.objDealer.EmailId, objCust.CustomerName, objCust.CustomerEmail, objCust.CustomerMobile, custArea, custCity, dealerDetailEntity.objQuotation.PriceList, Convert.ToInt32(TotalPrice), dealerDetailEntity.objOffers, imagePath, dealerDetailEntity.objDealer.PhoneNo, bikeName, objDPQSmsEntity.DealerArea, dealerDetailEntity.objDealer.AdditionalNumbers, dealerDetailEntity.objDealer.AdditionalEmails);
                        }

                        if (isVerified)
                        {
                            //_objPriceQuote.SaveBookingState(input.PQId, PriceQuoteStates.LeadSubmitted);
                            _objLeadNofitication.PushtoAB(input.DealerId.ToString(), 0, objCust.CustomerName, objCust.CustomerMobile, objCust.CustomerEmail, input.VersionId, input.CityId, input.PQId, input.LeadId);
                        }
                    }
                    if (isVerified)
                    {
                        output = new Bikewale.DTO.PriceQuote.v2.UpdatePQCustomerOutput();
                        output.IsSuccess = isVerified;
                        output.NoOfAttempts = noOfAttempts;
						output.LeadId = input.LeadId;
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.PQCustomerDetailController.Post");

                return InternalServerError();
            }
        }

        /// <summary>
        /// Modified By : Lucky Rathore on 20 April 2016
        /// Description : Declare DPQSmsEntity's city and address and change requestURL in SaveSMSToCustomer function.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="objCust"></param>
        /// <param name="bookingAmount"></param>
        /// <param name="dealerDetailEntity"></param>
        private void SaveCustomerSMS(UpdatePQCustomerInput input, CustomerEntity objCust, uint bookingAmount, PQ_DealerDetailEntity dealerDetailEntity)
        {
            UrlShortner objUrlShortner = new UrlShortner();
            DPQSmsEntity objDPQSmsEntity = new DPQSmsEntity();
            try
            {
                objDPQSmsEntity.CustomerMobile = objCust.CustomerMobile;
                objDPQSmsEntity.CustomerName = objCust.CustomerName;
                objDPQSmsEntity.DealerMobile = dealerDetailEntity.objDealer.MobileNo;
                objDPQSmsEntity.DealerName = dealerDetailEntity.objDealer.Organization;
                objDPQSmsEntity.Locality = dealerDetailEntity.objDealer.Address;
                objDPQSmsEntity.BookingAmount = bookingAmount;
                objDPQSmsEntity.BikeName = String.Format("{0} {1} {2}", dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName);
                objDPQSmsEntity.DealerArea = dealerDetailEntity.objDealer.objArea.AreaName != null ? dealerDetailEntity.objDealer.objArea.AreaName : string.Empty;
                objDPQSmsEntity.DealerAdd = dealerDetailEntity.objDealer.Address;
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
                    SendEmailSMSToDealerCustomer.SendSMSToCustomer(input.PQId, "/api/UpdatePQCustomerDetails", objDPQSmsEntity, DPQTypes.AndroidAppOfferNoBooking);
                }
                else
                {
                    SendEmailSMSToDealerCustomer.SendSMSToCustomer(input.PQId, "/api/UpdatePQCustomerDetails", objDPQSmsEntity, DPQTypes.SubscriptionModel);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.MobileVerification.UpdatePQCustomerDetailsController.SaveCustomerSMS");
               
            }
        }
    }
}