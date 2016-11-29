using Bikewale.BAL.ABServiceRef;
using Bikewale.DAL.Dealer;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.Dealer;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Bikewale.BAL.PriceQuote
{
    /// <summary>
    /// Created by  :   Sumit Kate on 02 May 2016
    /// Description :   Lead Notification BL
    /// Modified BY : Lucky Rathore on 12 May 2016
    /// Description : Signature of Notify Dealer and SendEmailToDealer changed.
    /// Modified by :   Sumit Kate on 18 Aug 2016
    /// Description :   Push Lead To Gaadi.com external API
    /// </summary>
    public class LeadNotificationBL : ILeadNofitication
    {
        private readonly TCApi_Inquiry _objInquiry;
        public LeadNotificationBL()
        {
            _objInquiry = new TCApi_Inquiry();
        }
        /// <summary>
        /// Sends Email and SMS to Customer
        /// Modified By : Lucky Rathore on 13 May 2016
        /// Description : parameter versionName, dealerLat, dealerLong, workingHours added.
        /// </summary>
        /// <param name="pqId"></param>
        /// <param name="bikeName"></param>
        /// <param name="bikeImage"></param>
        /// <param name="dealerName"></param>
        /// <param name="dealerEmail"></param>
        /// <param name="dealerMobileNo"></param>
        /// <param name="organization"></param>
        /// <param name="address"></param>
        /// <param name="customerName"></param>
        /// <param name="customerEmail"></param>
        /// <param name="priceList"></param>
        /// <param name="offerList"></param>
        /// <param name="pinCode"></param>
        /// <param name="stateName"></param>
        /// <param name="cityName"></param>
        /// <param name="totalPrice"></param>
        /// <param name="objDPQSmsEntity"></param>
        /// <param name="requestUrl"></param>
        /// <param name="leadSourceId"></param>
        /// <param name="platformId">For Android : 3 and iOS : 4</param>
        /// <param name="isInsuranceFree"></param>
        public void NotifyCustomer(uint pqId, string bikeName, string bikeImage, string dealerName, string dealerEmail, string dealerMobileNo, string organization, string address, string customerName, string customerEmail, List<PQ_Price> priceList, List<OfferEntity> offerList, string pinCode, string stateName, string cityName, uint totalPrice, DPQSmsEntity objDPQSmsEntity, string requestUrl, uint? leadSourceId, string versionName, double dealerLat, double dealerLong, string workingHours, string platformId = "")
        {
            try
            {
                //Different SMS is sent if lead is submitted from BikeWale APP
                if (platformId == "3" || platformId == "4")
                {
                    SendEmailSMSToDealerCustomer.SendSMSToCustomer(pqId, requestUrl, objDPQSmsEntity, DPQTypes.AndroidAppOfferNoBooking);
                }
                else
                {
                    //If lead is submitted while Booking a bike online don't sent SMS to customer
                    if (leadSourceId != 16 && leadSourceId != 22)
                        SendEmailSMSToDealerCustomer.SendSMSToCustomer(pqId, requestUrl, objDPQSmsEntity, DPQTypes.SubscriptionModel);
                }
                //If lead is submitted while Booking a bike online don't sent SMS to customer
                if (leadSourceId != 16 && leadSourceId != 22)
                {
                    SendEmailSMSToDealerCustomer.SendEmailToCustomer(bikeName, bikeImage, dealerName, dealerEmail, dealerMobileNo, organization, address, customerName, customerEmail, priceList, offerList, pinCode, stateName, cityName, totalPrice,
                        versionName, dealerLat, dealerLong, workingHours);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "LeadNotificationBL.NotifyCustomer");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Sends SMS and Email to Dealer
        /// Modified BY : Lucky Rathore on 12 May 2016
        /// Description : Signature of Notify Dealer and SendEmailToDealer.
        /// Modified By : Lucky Rathore on 11 July 2016.
        /// Description : parameter dealerArea added in NotifyDealer(). 
        /// </summary>
        /// <param name="pqId"></param>
        /// <param name="makeName"></param>
        /// <param name="modelName"></param>
        /// <param name="versionName"></param>
        /// <param name="dealerName"></param>
        /// <param name="dealerEmail"></param>
        /// <param name="customerName"></param>
        /// <param name="customerEmail"></param>
        /// <param name="customerMobile"></param>
        /// <param name="areaName"></param>
        /// <param name="cityName"></param>
        /// <param name="priceList"></param>
        /// <param name="totalPrice"></param>
        /// <param name="offerList"></param>
        /// <param name="imagePath"></param>
        /// <param name="dealerMobile"></param>
        /// <param name="bikeName"></param>
        /// <param name="insuranceAmount"></param>

        public void NotifyDealer(uint pqId, string makeName, string modelName, string versionName, string dealerName, string dealerEmail, string customerName, string customerEmail, string customerMobile, string areaName, string cityName, List<PQ_Price> priceList, int totalPrice, List<OfferEntity> offerList, string imagePath, string dealerMobile, string bikeName, string dealerArea)
        {
            try
            {
                SendEmailSMSToDealerCustomer.SendEmailToDealer(makeName, modelName, versionName, dealerName, dealerEmail, customerName, customerEmail, customerMobile, areaName, cityName, priceList, totalPrice, offerList, imagePath);
                SendEmailSMSToDealerCustomer.SMSToDealer(dealerMobile, customerName, customerMobile, bikeName, areaName, cityName, dealerArea);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "LeadNotificationBL.NotifyDealer");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// Pushes Inquiry in AutoBiz using API
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="pqId"></param>
        /// <param name="customerName"></param>
        /// <param name="customerMobile"></param>
        /// <param name="customerEmail"></param>
        /// <param name="versionId"></param>
        /// <param name="cityId"></param>
        public void PushtoAB(string dealerId, uint pqId, string customerName, string customerMobile, string customerEmail, string versionId, string cityId)
        {
            string abInquiryId = string.Empty, campaignId = string.Empty;
            try
            {
                IDealerPriceQuote objDealer = null;

                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    container.RegisterType<IPriceQuote, Bikewale.BAL.PriceQuote.PriceQuote>();
                    objDealer = container.Resolve<IDealerPriceQuote>();
                    IPriceQuote objPriceQuote = container.Resolve<IPriceQuote>();
                    PriceQuoteParametersEntity details = objPriceQuote.FetchPriceQuoteDetailsById(pqId);
                    campaignId = details.CampaignId.HasValue ? details.CampaignId.Value.ToString() : "0";
                }

                //update dealer's daily lead count
                if (objDealer != null && !objDealer.IsDealerDailyLeadLimitExceeds(Convert.ToUInt32(campaignId)))
                {

                    string jsonInquiryDetails = String.Format("{{ \"CustomerName\": \"{0}\", \"CustomerMobile\":\"{1}\", \"CustomerEmail\":\"{2}\", \"VersionId\":\"{3}\", \"CityId\":\"{4}\", \"CampaignId\":\"{5}\", \"InquirySourceId\":\"39\", \"Eagerness\":\"1\",\"ApplicationId\":\"2\"}}", customerName, customerMobile, customerEmail, versionId, cityId, campaignId);

                    abInquiryId = _objInquiry.AddNewCarInquiry(dealerId, jsonInquiryDetails);
                    int abInqId = 0;
                    if (Int32.TryParse(abInquiryId, out abInqId) && abInqId > 0)
                    {
                        if (objDealer.UpdateDealerDailyLeadCount(Convert.ToUInt32(campaignId), (uint)abInqId))
                            objDealer.PushedToAB(pqId, (uint)abInqId);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "LeadNotificationBL.PushtoAB");
                objErr.SendMail();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="abInquiryId"></param>
        /// <param name="cityId"></param>
        private bool UpdateDealerDailyLeadCount(uint campaignId, uint abInquiryId)
        {
            bool isUpdateDealerCount = false;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();

                    isUpdateDealerCount = objDealer.UpdateDealerDailyLeadCount(campaignId, abInquiryId);

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "LeadNotificationBL.UpdateDealerDailyLeadCount");
                objErr.SendMail();
            }
            return isUpdateDealerCount;
        }

        /// <summary>
        /// Created By  :   Sumit Kate on 18 Aug 2016
        /// Description :   Push Lead To Gaadi.com external API
        /// </summary>
        /// <param name="leadEntity"></param>
        /// <returns></returns>
        public bool PushLeadToGaadi(Entities.Dealer.ManufacturerLeadEntity leadEntity)
        {
            string leadURL = string.Empty;
            string response = string.Empty;
            bool isSuccess = false;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IPriceQuote, Bikewale.BAL.PriceQuote.PriceQuote>().
                    RegisterType<IDealer, DealersRepository>();
                    IPriceQuote objPriceQuote = container.Resolve<IPriceQuote>();
                    BikeQuotationEntity quotation = objPriceQuote.GetPriceQuoteById(leadEntity.PQId);
                    leadURL = String.Format("http://hondalms.gaadi.com/lms/externalApi/girnarLeadHMSIApi.php?name={0}&email={1}&mobile={2}&city={3}&state={4}&make={5}&model={6}&sub_source=bikewale", leadEntity.Name, leadEntity.Email, leadEntity.Mobile, quotation.City, quotation.State, quotation.MakeName, quotation.ModelName);

                    using (HttpClient httpClient = new HttpClient())
                    {
                        using (HttpResponseMessage _response = httpClient.GetAsync(leadURL).Result)
                        {
                            if (_response.IsSuccessStatusCode)
                            {
                                if (_response.StatusCode == System.Net.HttpStatusCode.OK) //Check 200 OK Status        
                                {
                                    response = _response.Content.ReadAsStringAsync().Result;
                                    IDealer objDealer = container.Resolve<IDealer>();
                                    objDealer.UpdateManufaturerLead(leadEntity.PQId, leadEntity.Email, leadEntity.Mobile, response);
                                    _response.Content.Dispose();
                                    _response.Content = null;
                                    isSuccess = true;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "LeadNotificationBL.PushLeadToGaadi");
                objErr.SendMail();
            }
            return isSuccess;
        }
    }
}
