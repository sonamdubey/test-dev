﻿using Bikewale.DTO.PriceQuote;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Service.Utilities;
using Bikewale.Utility;
using Microsoft.Practices.Unity;
using System;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote
{
    /// <summary>
    /// Controller to update the PQ details
    /// Modified by :   Sumit Kate on 18 May 2016
    /// Description :   Extend from CompressionApiController instead of ApiController 
    /// </summary>
    public class UpdatePQController : CompressionApiController//ApiController
    {
        private readonly IPriceQuote _objPQ = null;
        private readonly IDealerPriceQuote _objDealerPQ = null;
        private readonly ILeadNofitication _objLeadNofitication = null;
        public UpdatePQController(IPriceQuote objPQ, IDealerPriceQuote objDealerPQ, ILeadNofitication objLeadNofitication)
        {
            _objPQ = objPQ;
            _objDealerPQ = objDealerPQ;
            _objLeadNofitication = objLeadNofitication;
        }

        /// <summary>
        /// Updates the Price Quote data for given Price Quote Id
        /// Modified By : Sadhana Upadhyay on 22 Dec 2015
        /// Summary : To update Notification template in PQ_LeadNotification Table
        /// Modified By : Lucky Rathore on 20/04/2016
        /// Description : Changed making no. (mobile no.) of dealer to his phone no. for sms to customer.
        /// Modified by :   Sumit Kate on 02 May 2016
        /// Description :   Send the notification immediately
        /// Modified by :   Lucky Rathore on 13 May 2016
        /// Description :   var versionName declare, Intialized and NotifyCustomer() singature Updated.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ResponseType(typeof(PQUpdateOutput))]
        public IHttpActionResult Post([FromBody]PQUpdateInput input)
        {
            PQUpdateOutput output = null;
            PriceQuoteParametersEntity pqParam = null;
            PQCustomerDetail objCustomer = null;
            PQ_DealerDetailEntity dealerDetailEntity = null;
            PriceQuoteParametersEntity details = null;
            string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
            string _requestType = "application/json", _apiUrl = string.Empty, imagePath = string.Empty, bikeName = string.Empty, versionName = string.Empty;
            bool IsInsuranceFree = false;
            uint insuranceAmount = 0, TotalPrice = 0, exShowroomCost = 0;
            try
            {
                if (input != null && ((input.PQId > 0) && (input.VersionId > 0)))
                {
                    pqParam = new PriceQuoteParametersEntity();
                    pqParam.VersionId = input.VersionId;
                    output = new PQUpdateOutput();
                    if (_objPQ.UpdatePriceQuote(input.PQId, pqParam))
                    {
                        objCustomer = _objDealerPQ.GetCustomerDetails(Convert.ToUInt32(input.PQId));
                        _apiUrl = String.Format("/api/Dealers/GetDealerDetailsPQ/?versionId={0}&DealerId={1}&CityId={2}", input.VersionId, objCustomer.DealerId, objCustomer.objCustomerBase.cityDetails.CityId);

                        using (BWHttpClient objClient = new BWHttpClient())
                        {
                            dealerDetailEntity = objClient.GetApiResponseSync<PQ_DealerDetailEntity>(APIHost.AB, _requestType, _apiUrl, dealerDetailEntity);
                        }

                        if (dealerDetailEntity != null && dealerDetailEntity.objQuotation != null)
                        {
                            foreach (var price in dealerDetailEntity.objQuotation.PriceList)
                            {
                                IsInsuranceFree = OfferHelper.HasFreeInsurance(objCustomer.DealerId.ToString(), dealerDetailEntity.objQuotation.objModel.ModelId.ToString(), price.CategoryName, price.Price, ref insuranceAmount);
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

                            using (IUnityContainer container = new UnityContainer())
                            {
                                container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                                container.RegisterType<IPriceQuote, Bikewale.BAL.PriceQuote.PriceQuote>();
                                IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();
                                IPriceQuote objPriceQuote = container.Resolve<IPriceQuote>();
                                details = objPriceQuote.FetchPriceQuoteDetailsById(input.PQId);
                            }

                            DPQSmsEntity objDPQSmsEntity = new DPQSmsEntity();
                            objDPQSmsEntity.CustomerMobile = objCustomer.objCustomerBase.CustomerMobile;
                            objDPQSmsEntity.CustomerName = objCustomer.objCustomerBase.CustomerName;
                            objDPQSmsEntity.DealerMobile = dealerDetailEntity.objDealer.MobileNo;
                            objDPQSmsEntity.DealerName = dealerDetailEntity.objDealer.Organization;
                            objDPQSmsEntity.Locality = dealerDetailEntity.objDealer.Address;
                            objDPQSmsEntity.BookingAmount = dealerDetailEntity.objBookingAmt != null ? dealerDetailEntity.objBookingAmt.Amount : 0;
                            objDPQSmsEntity.BikeName = String.Format("{0} {1} {2}", dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName);
                            objDPQSmsEntity.DealerArea = dealerDetailEntity.objDealer.objArea.AreaName != null ? dealerDetailEntity.objDealer.objArea.AreaName : string.Empty;
                            objDPQSmsEntity.DealerAdd = dealerDetailEntity.objDealer.Address;
                            objDPQSmsEntity.DealerCity = dealerDetailEntity.objDealer.objCity != null ? dealerDetailEntity.objDealer.objCity.CityName : string.Empty;
                            objDPQSmsEntity.OrganisationName = dealerDetailEntity.objDealer.Organization;

                            _objLeadNofitication.NotifyCustomer(input.PQId, bikeName, imagePath, dealerDetailEntity.objDealer.Organization,
                                dealerDetailEntity.objDealer.EmailId, dealerDetailEntity.objDealer.MobileNo, dealerDetailEntity.objDealer.Organization,
                                dealerDetailEntity.objDealer.Address, objCustomer.objCustomerBase.CustomerName, objCustomer.objCustomerBase.CustomerEmail,
                                dealerDetailEntity.objQuotation.PriceList, dealerDetailEntity.objOffers, dealerDetailEntity.objDealer.objArea.PinCode,
                                dealerDetailEntity.objDealer.objState.StateName, dealerDetailEntity.objDealer.objCity.CityName, TotalPrice, objDPQSmsEntity,
                                "api/UpdatePQ", 16, versionName, dealerDetailEntity.objDealer.objArea.Latitude, dealerDetailEntity.objDealer.objArea.Longitude, dealerDetailEntity.objDealer.WorkingTime, platformId = "");


                            _objLeadNofitication.PushtoAB(dealerDetailEntity.objDealer.DealerId.ToString(), input.PQId, objCustomer.objCustomerBase.CustomerName, objCustomer.objCustomerBase.CustomerMobile, objCustomer.objCustomerBase.CustomerEmail, input.VersionId.ToString(), details.CityId.ToString());

                            // If customer is mobile verified push lead to autobiz
                            _objPQ.SaveBookingState(input.PQId, PriceQuoteStates.LeadSubmitted);
                        }

                        output.IsUpdated = true;
                    }
                    return Ok(output);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.UpdatePQController.Put");
                objErr.SendMail();
                return InternalServerError();
            }
        }

        /// <summary>
        /// Modified By : Lucky Rathore on 20 April 2016
        /// Description : Declare DPQSmsEntity's city and address.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="objCustomer"></param>
        /// <param name="dealerDetailEntity"></param>
        private void SaveCustomerSMS(PQUpdateInput input, PQCustomerDetail objCustomer, PQ_DealerDetailEntity dealerDetailEntity)
        {
            UrlShortner objUrlShortner = new UrlShortner();
            DPQSmsEntity objDPQSmsEntity = new DPQSmsEntity();
            try
            {
                objDPQSmsEntity.CustomerMobile = objCustomer.objCustomerBase.CustomerMobile;
                objDPQSmsEntity.CustomerName = objCustomer.objCustomerBase.CustomerName;
                objDPQSmsEntity.DealerMobile = dealerDetailEntity.objDealer.MobileNo;
                objDPQSmsEntity.DealerName = dealerDetailEntity.objDealer.Name;
                objDPQSmsEntity.Locality = dealerDetailEntity.objDealer.Address;
                objDPQSmsEntity.BookingAmount = dealerDetailEntity.objBookingAmt != null ? dealerDetailEntity.objBookingAmt.Amount : 0;
                objDPQSmsEntity.DealerArea = dealerDetailEntity.objDealer.objArea.AreaName != null ? dealerDetailEntity.objDealer.objArea.AreaName : string.Empty;
                objDPQSmsEntity.DealerAdd = dealerDetailEntity.objDealer.Address;
                objDPQSmsEntity.BikeName = String.Format("{0} {1} {2}", dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName);
                objDPQSmsEntity.DealerCity = dealerDetailEntity.objDealer.objCity != null ? dealerDetailEntity.objDealer.objCity.CityName : string.Empty;
                objDPQSmsEntity.OrganisationName = dealerDetailEntity.objDealer.Organization;
                PriceQuoteParametersEntity pqEntity = _objPQ.FetchPriceQuoteDetailsById(input.PQId);

                var platformId = "";
                if (Request.Headers.Contains("platformId"))
                {
                    platformId = Request.Headers.GetValues("platformId").First().ToString();
                }

                if (!string.IsNullOrEmpty(platformId) && (platformId == "3" || platformId == "4"))
                {
                    SendEmailSMSToDealerCustomer.SendSMSToCustomer(input.PQId, "/api/UpdatePQ", objDPQSmsEntity, DPQTypes.AndroidAppOfferNoBooking);
                }
                else
                {
                    SendEmailSMSToDealerCustomer.SendSMSToCustomer(input.PQId, "/api/UpdatePQ", objDPQSmsEntity, DPQTypes.SubscriptionModel);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.MobileVerification.UpdatePQController.SaveCustomerSMS");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On  : 11th Nov 2015
        /// Updates the Price Quote data for given Price Quote Id  colorId and versionId
        /// Modified by :   Sumit Kate on 02 May 2016
        /// Description :   Send the notification immediately
        /// Modified by :   Lucky Rathore on 12 May 2016
        /// Description :   Update ModiyDealer() Sinature.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ResponseType(typeof(PQUpdateOutput))]
        public IHttpActionResult Post([FromBody]PQUpdateInput input, uint colorId)
        {
            PQUpdateOutput output = null;
            PriceQuoteParametersEntity pqParam = null;
            PriceQuoteParametersEntity details = null;
            PQCustomerDetail objCustomer = null;
            PQ_DealerDetailEntity dealerDetailEntity = null;
            string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
            string _requestType = "application/json", _apiUrl = string.Empty, imagePath = string.Empty, bikeName = string.Empty, versionName = string.Empty;
            bool IsInsuranceFree = false;
            uint insuranceAmount = 0, TotalPrice = 0, exShowroomCost = 0;
            try
            {
                if (input != null && ((input.PQId > 0) && (input.VersionId > 0)))
                {
                    pqParam = new PriceQuoteParametersEntity();
                    pqParam.VersionId = input.VersionId;
                    pqParam.ColorId = colorId;
                    output = new PQUpdateOutput();
                    if (_objPQ.UpdatePriceQuote(input.PQId, pqParam))
                    {
                        objCustomer = _objDealerPQ.GetCustomerDetails(Convert.ToUInt32(input.PQId));
                        _apiUrl = String.Format("/api/Dealers/GetDealerDetailsPQ/?versionId={0}&DealerId={1}&CityId={2}", input.VersionId, objCustomer.DealerId, objCustomer.objCustomerBase.cityDetails.CityId);

                        using (BWHttpClient objClient = new BWHttpClient())
                        {
                            dealerDetailEntity = objClient.GetApiResponseSync<PQ_DealerDetailEntity>(APIHost.AB, _requestType, _apiUrl, dealerDetailEntity);
                        }

                        if (dealerDetailEntity != null && dealerDetailEntity.objQuotation != null)
                        {
                            foreach (var price in dealerDetailEntity.objQuotation.PriceList)
                            {
                                IsInsuranceFree = OfferHelper.HasFreeInsurance(objCustomer.DealerId.ToString(), dealerDetailEntity.objQuotation.objModel.ModelId.ToString(), price.CategoryName, price.Price, ref insuranceAmount);
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

                            using (IUnityContainer container = new UnityContainer())
                            {
                                container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                                container.RegisterType<IPriceQuote, Bikewale.BAL.PriceQuote.PriceQuote>();
                                IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();
                                IPriceQuote objPriceQuote = container.Resolve<IPriceQuote>();
                                details = objPriceQuote.FetchPriceQuoteDetailsById(input.PQId);
                            }

                            var platformId = "";
                            if (Request.Headers.Contains("platformId"))
                            {
                                platformId = Request.Headers.GetValues("platformId").First().ToString();
                            }


                            DPQSmsEntity objDPQSmsEntity = new DPQSmsEntity();
                            objDPQSmsEntity.CustomerMobile = objCustomer.objCustomerBase.CustomerMobile;
                            objDPQSmsEntity.CustomerName = objCustomer.objCustomerBase.CustomerName;
                            objDPQSmsEntity.DealerMobile = dealerDetailEntity.objDealer.MobileNo;
                            objDPQSmsEntity.DealerName = dealerDetailEntity.objDealer.Organization;
                            objDPQSmsEntity.Locality = dealerDetailEntity.objDealer.Address;
                            objDPQSmsEntity.BookingAmount = dealerDetailEntity.objBookingAmt != null ? dealerDetailEntity.objBookingAmt.Amount : 0;
                            objDPQSmsEntity.BikeName = String.Format("{0} {1} {2}", dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName);
                            objDPQSmsEntity.DealerArea = dealerDetailEntity.objDealer.objArea.AreaName != null ? dealerDetailEntity.objDealer.objArea.AreaName : string.Empty;
                            objDPQSmsEntity.DealerAdd = dealerDetailEntity.objDealer.Address;
                            objDPQSmsEntity.DealerCity = dealerDetailEntity.objDealer.objCity != null ? dealerDetailEntity.objDealer.objCity.CityName : string.Empty;
                            objDPQSmsEntity.OrganisationName = dealerDetailEntity.objDealer.Organization;

                            _objLeadNofitication.NotifyCustomer(input.PQId, bikeName, imagePath, dealerDetailEntity.objDealer.Organization,
                                dealerDetailEntity.objDealer.EmailId, dealerDetailEntity.objDealer.MobileNo, dealerDetailEntity.objDealer.Organization,
                                dealerDetailEntity.objDealer.Address, objCustomer.objCustomerBase.CustomerName, objCustomer.objCustomerBase.CustomerEmail,
                                dealerDetailEntity.objQuotation.PriceList, dealerDetailEntity.objOffers, dealerDetailEntity.objDealer.objArea.PinCode,
                                dealerDetailEntity.objDealer.objState.StateName, dealerDetailEntity.objDealer.objCity.CityName, TotalPrice, objDPQSmsEntity,
                                "api/UpdatePQ", 16, versionName, dealerDetailEntity.objDealer.objArea.Latitude, dealerDetailEntity.objDealer.objArea.Longitude, dealerDetailEntity.objDealer.WorkingTime, platformId = "");



                            _objPQ.SaveBookingState(input.PQId, PriceQuoteStates.LeadSubmitted);
                            _objLeadNofitication.PushtoAB(dealerDetailEntity.objDealer.DealerId.ToString(), input.PQId, objCustomer.objCustomerBase.CustomerName, objCustomer.objCustomerBase.CustomerMobile, objCustomer.objCustomerBase.CustomerEmail, input.VersionId.ToString(), details.CityId.ToString());
                            #region Old notification way
                            //if (platformId != "3" && platformId != "4")
                            //{

                            //    SendEmailSMSToDealerCustomer.SaveEmailToCustomer(input.PQId, bikeName, imagePath, dealerDetailEntity.objDealer.Name, dealerDetailEntity.objDealer.EmailId, dealerDetailEntity.objDealer.MobileNo, dealerDetailEntity.objDealer.Organization, dealerDetailEntity.objDealer.Address, objCustomer.objCustomerBase.CustomerName, objCustomer.objCustomerBase.CustomerEmail, dealerDetailEntity.objQuotation.PriceList, dealerDetailEntity.objOffers, dealerDetailEntity.objDealer.objArea.PinCode, dealerDetailEntity.objDealer.objState.StateName, dealerDetailEntity.objDealer.objCity.CityName, TotalPrice, insuranceAmount);
                            //}

                            //hasBumperDealerOffer = OfferHelper.HasBumperDealerOffer(dealerDetailEntity.objDealer.DealerId.ToString(), "");
                            ////if (dealerDetailEntity.objBookingAmt.Amount > 0)
                            ////{
                            ////    //SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, dealerDetailEntity, objCustomer.objCustomerBase.CustomerMobile, objCustomer.objCustomerBase.CustomerName, bikeName, dealerDetailEntity.objDealer.Name, dealerDetailEntity.objDealer.MobileNo, dealerDetailEntity.objDealer.Address, dealerDetailEntity.objBookingAmt.Amount, insuranceAmount, hasBumperDealerOffer);
                            ////}

                            //SaveCustomerSMS(input, objCustomer, dealerDetailEntity);

                            ////bool isDealerNotified = _objDealerPQ.IsDealerNotified(objCustomer.DealerId, objCustomer.objCustomerBase.CustomerMobile, objCustomer.objCustomerBase.CustomerId);
                            ////if (!isDealerNotified)
                            //{
                            //    SendEmailSMSToDealerCustomer.SaveEmailToDealer(input.PQId, dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName, dealerDetailEntity.objDealer.Name, dealerDetailEntity.objDealer.EmailId, objCustomer.objCustomerBase.CustomerName, objCustomer.objCustomerBase.CustomerEmail, objCustomer.objCustomerBase.CustomerMobile, objCustomer.objCustomerBase.AreaDetails.AreaName, objCustomer.objCustomerBase.cityDetails.CityName, dealerDetailEntity.objQuotation.PriceList, Convert.ToInt32(TotalPrice), dealerDetailEntity.objOffers, imagePath, insuranceAmount);
                            //    SendEmailSMSToDealerCustomer.SaveSMSToDealer(input.PQId, dealerDetailEntity.objDealer.MobileNo, objCustomer.objCustomerBase.CustomerName, objCustomer.objCustomerBase.CustomerMobile, bikeName, objCustomer.objCustomerBase.AreaDetails.AreaName, objCustomer.objCustomerBase.cityDetails.CityName);
                            //}

                            //// If customer is mobile verified push lead to autobiz
                            //_objPQ.SaveBookingState(input.PQId, PriceQuoteStates.LeadSubmitted); 
                            #endregion
                        }

                        output.IsUpdated = true;
                    }
                    return Ok(output);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.PriceQuote.UpdatePQController.Put");
                objErr.SendMail();
                return InternalServerError();
            }

        }

        //private void SaveCustomerSMS(PQUpdateInput input, CustomerEntity objCust, PQ_DealerDetailEntity dealerDetailEntity, uint bookingAmount)
        //{
        //    DPQSmsEntity objDPQSmsEntity = new DPQSmsEntity();
        //    objDPQSmsEntity.CustomerMobile = objCust.CustomerMobile;
        //    objDPQSmsEntity.CustomerName = objCust.CustomerName;
        //    objDPQSmsEntity.DealerMobile = dealerDetailEntity.objDealer.MobileNo;
        //    objDPQSmsEntity.DealerName = dealerDetailEntity.objDealer.Name;
        //    objDPQSmsEntity.Locality = dealerDetailEntity.objDealer.Address;

        //    PriceQuoteParametersEntity pqEntity = _objPQ.FetchPriceQuoteDetailsById(input.PQId);
        //    String mpqQueryString = String.Format("CityId={0}&AreaId={1}&PQId={2}&VersionId={3}&DealerId={4}", pqEntity.CityId, pqEntity.AreaId, input.PQId, pqEntity.VersionId, pqEntity.DealerId);
        //    objDPQSmsEntity.LandingPageShortUrl = String.Format("{0}/pricequote/BikeDealerDetails.aspx?MPQ={1}", BWConfiguration.Instance.BwHostUrl, EncodingDecodingHelper.EncodeTo64(""));
        //    var platformId = "";
        //    if (Request.Headers.Contains("platformId"))
        //    {
        //        platformId = Request.Headers.GetValues("platformId").First().ToString();
        //    }

        //    if (!string.IsNullOrEmpty(platformId) && (platformId == "3" || platformId == "4"))
        //    {
        //        SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, "/api/PQMobileVerification", objDPQSmsEntity, DPQTypes.AndroidAppOfferNoBooking);
        //    }
        //    else
        //    {
        //        if ((dealerDetailEntity.objOffers != null) && (dealerDetailEntity.objOffers.Count > 0))
        //        {
        //            if (bookingAmount > 0)
        //            {
        //                SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, "/api/PQMobileVerification", objDPQSmsEntity, DPQTypes.OfferAndBooking);
        //            }
        //            else
        //            {
        //                SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, "/api/PQMobileVerification", objDPQSmsEntity, DPQTypes.OfferNoBooking);
        //            }
        //        }
        //        else
        //        {
        //            if (bookingAmount > 0)
        //            {
        //                SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, "/api/PQMobileVerification", objDPQSmsEntity, DPQTypes.NoOfferOnlineBooking);
        //            }
        //            else
        //            {
        //                SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, "/api/PQMobileVerification", objDPQSmsEntity, DPQTypes.NoOfferNoBooking);
        //            }
        //        }
        //    }
        //}
    }
}
