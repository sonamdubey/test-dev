using Bikewale.DTO.PriceQuote;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.Customer;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Bikewale.Utility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.PriceQuote
{
    /// <summary>
    /// Controller to update the PQ details
    /// </summary>
    public class UpdatePQController : ApiController
    {
        private readonly IPriceQuote _objPQ = null;
        private readonly IDealerPriceQuote _objDealerPQ = null;
        public UpdatePQController(IPriceQuote objPQ, IDealerPriceQuote objDealerPQ)
        {
            _objPQ = objPQ;
            _objDealerPQ = objDealerPQ;
        }

        /// <summary>
        /// Updates the Price Quote data for given Price Quote Id
        /// Modified By : Sadhana Upadhyay on 22 Dec 2015
        /// Summary : To update Notification template in PQ_LeadNotification Table
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
            string _abHostUrl = ConfigurationManager.AppSettings["ABApiHostUrl"];
            string _requestType = "application/json", _apiUrl = string.Empty, imagePath = string.Empty, bikeName = string.Empty;
            bool IsInsuranceFree = false, hasBumperDealerOffer = false;
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


                            // Notification Logic
                            SendEmailSMSToDealerCustomer.SaveEmailToCustomer(input.PQId, bikeName, imagePath, dealerDetailEntity.objDealer.Name, dealerDetailEntity.objDealer.EmailId, dealerDetailEntity.objDealer.MobileNo, dealerDetailEntity.objDealer.Organization, dealerDetailEntity.objDealer.Address, objCustomer.objCustomerBase.CustomerName, objCustomer.objCustomerBase.CustomerEmail, dealerDetailEntity.objQuotation.PriceList, dealerDetailEntity.objOffers, dealerDetailEntity.objDealer.objArea.PinCode, dealerDetailEntity.objDealer.objState.StateName, dealerDetailEntity.objDealer.objCity.CityName, TotalPrice, insuranceAmount);

                            hasBumperDealerOffer = OfferHelper.HasBumperDealerOffer(dealerDetailEntity.objDealer.DealerId.ToString(), "");
                            if (dealerDetailEntity.objBookingAmt.Amount > 0)
                            {
                                //SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, dealerDetailEntity, objCustomer.objCustomerBase.CustomerMobile, objCustomer.objCustomerBase.CustomerName, bikeName, dealerDetailEntity.objDealer.Name, dealerDetailEntity.objDealer.MobileNo, dealerDetailEntity.objDealer.Address, dealerDetailEntity.objBookingAmt.Amount, insuranceAmount, hasBumperDealerOffer);
                            }

                            SaveCustomerSMS(input, objCustomer, dealerDetailEntity);

                            bool isDealerNotified = _objDealerPQ.IsDealerNotified(objCustomer.DealerId, objCustomer.objCustomerBase.CustomerMobile, objCustomer.objCustomerBase.CustomerId);
                            if (!isDealerNotified)
                            {
                                SendEmailSMSToDealerCustomer.SaveEmailToDealer(input.PQId, dealerDetailEntity.objQuotation.objMake.MakeName, dealerDetailEntity.objQuotation.objModel.ModelName, dealerDetailEntity.objQuotation.objVersion.VersionName, dealerDetailEntity.objDealer.Name, dealerDetailEntity.objDealer.EmailId, objCustomer.objCustomerBase.CustomerName, objCustomer.objCustomerBase.CustomerEmail, objCustomer.objCustomerBase.CustomerMobile, objCustomer.objCustomerBase.AreaDetails.AreaName, objCustomer.objCustomerBase.cityDetails.CityName, dealerDetailEntity.objQuotation.PriceList, Convert.ToInt32(TotalPrice), dealerDetailEntity.objOffers, insuranceAmount);
                                SendEmailSMSToDealerCustomer.SaveSMSToDealer(input.PQId, dealerDetailEntity.objDealer.MobileNo, objCustomer.objCustomerBase.CustomerName, objCustomer.objCustomerBase.CustomerMobile, bikeName, objCustomer.objCustomerBase.AreaDetails.AreaName, objCustomer.objCustomerBase.cityDetails.CityName);
                            }

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

        private void SaveCustomerSMS(PQUpdateInput input, PQCustomerDetail objCustomer, PQ_DealerDetailEntity dealerDetailEntity)
        {
            DPQSmsEntity objDPQSmsEntity = new DPQSmsEntity();
            objDPQSmsEntity.CustomerMobile = objCustomer.objCustomerBase.CustomerMobile;
            objDPQSmsEntity.CustomerName = objCustomer.objCustomerBase.CustomerName;
            objDPQSmsEntity.DealerMobile = dealerDetailEntity.objDealer.MobileNo;
            objDPQSmsEntity.DealerName = dealerDetailEntity.objDealer.Name;
            objDPQSmsEntity.Locality = dealerDetailEntity.objDealer.Address;

            PriceQuoteParametersEntity pqEntity = _objPQ.FetchPriceQuoteDetailsById(input.PQId);
            String mpqQueryString = String.Format("CityId={0}&AreaId={1}&PQId={2}&VersionId={3}&DealerId={4}", pqEntity.CityId, pqEntity.AreaId, input.PQId, pqEntity.VersionId, pqEntity.DealerId);
            objDPQSmsEntity.LandingPageShortUrl = String.Format("{0}/pricequote/BikeDealerDetails.aspx?MPQ={1}", BWConfiguration.Instance.BwHostUrl, EncodingDecodingHelper.EncodeTo64(""));
            var platformId = "";
            if (Request.Headers.Contains("platformId"))
            {
                platformId = Request.Headers.GetValues("platformId").First().ToString();
            }

            if (!string.IsNullOrEmpty(platformId) && (platformId == "3" || platformId == "4"))
            {
                SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, "/api/UpdatePQ", objDPQSmsEntity, DPQTypes.AndroidAppOfferNoBooking);
            }
            else
            {
                if ((dealerDetailEntity.objOffers != null) && (dealerDetailEntity.objOffers.Count > 0))
                {
                    if (dealerDetailEntity.objBookingAmt != null && dealerDetailEntity.objBookingAmt.Amount > 0)
                    {
                        SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, "/api/UpdatePQ", objDPQSmsEntity, DPQTypes.OfferAndBooking);
                    }
                    else
                    {
                        SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, "/api/UpdatePQ", objDPQSmsEntity, DPQTypes.OfferNoBooking);
                    }
                }
                else
                {
                    if (dealerDetailEntity.objBookingAmt != null && dealerDetailEntity.objBookingAmt.Amount > 0)
                    {
                        SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, "/api/UpdatePQ", objDPQSmsEntity, DPQTypes.NoOfferOnlineBooking);
                    }
                    else
                    {
                        SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, "/api/UpdatePQ", objDPQSmsEntity, DPQTypes.NoOfferNoBooking);
                    }
                }
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On  : 11th Nov 2015
        /// Updates the Price Quote data for given Price Quote Id  colorId and versionId
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [ResponseType(typeof(PQUpdateOutput))]
        public IHttpActionResult Post([FromBody]PQUpdateInput input,uint colorId)
        {
            PQUpdateOutput output = null;
            PriceQuoteParametersEntity pqParam = null;
            try
            {
                if (input != null && ((input.PQId > 0) && (input.VersionId > 0)) && colorId > 0)
                {
                    pqParam = new PriceQuoteParametersEntity();
                    pqParam.VersionId = input.VersionId;
                    pqParam.ColorId = colorId;
                    output = new PQUpdateOutput();
                    if (_objPQ.UpdatePriceQuote(input.PQId, pqParam))
                    {
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

        private void SaveCustomerSMS(PQUpdateInput input, CustomerEntity objCust, PQ_DealerDetailEntity dealerDetailEntity, uint bookingAmount)
        {
            DPQSmsEntity objDPQSmsEntity = new DPQSmsEntity();
            objDPQSmsEntity.CustomerMobile = objCust.CustomerMobile;
            objDPQSmsEntity.CustomerName = objCust.CustomerName;
            objDPQSmsEntity.DealerMobile = dealerDetailEntity.objDealer.MobileNo;
            objDPQSmsEntity.DealerName = dealerDetailEntity.objDealer.Name;
            objDPQSmsEntity.Locality = dealerDetailEntity.objDealer.Address;

            PriceQuoteParametersEntity pqEntity = _objPQ.FetchPriceQuoteDetailsById(input.PQId);
            String mpqQueryString = String.Format("CityId={0}&AreaId={1}&PQId={2}&VersionId={3}&DealerId={4}", pqEntity.CityId, pqEntity.AreaId, input.PQId, pqEntity.VersionId, pqEntity.DealerId);
            objDPQSmsEntity.LandingPageShortUrl = String.Format("{0}/pricequote/BikeDealerDetails.aspx?MPQ={1}", BWConfiguration.Instance.BwHostUrl, EncodingDecodingHelper.EncodeTo64(""));
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
                if ((dealerDetailEntity.objOffers != null) && (dealerDetailEntity.objOffers.Count > 0))
                {
                    if (bookingAmount > 0)
                    {
                        SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, "/api/PQMobileVerification", objDPQSmsEntity, DPQTypes.OfferAndBooking);
                    }
                    else
                    {
                        SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, "/api/PQMobileVerification", objDPQSmsEntity, DPQTypes.OfferNoBooking);
                    }
                }
                else
                {
                    if (bookingAmount > 0)
                    {
                        SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, "/api/PQMobileVerification", objDPQSmsEntity, DPQTypes.NoOfferOnlineBooking);
                    }
                    else
                    {
                        SendEmailSMSToDealerCustomer.SaveSMSToCustomer(input.PQId, "/api/PQMobileVerification", objDPQSmsEntity, DPQTypes.NoOfferNoBooking);
                    }
                }
            }
        }
    }
}
