using Bikewale.BAL.ABServiceRef;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;

namespace Bikewale.BAL.PriceQuote
{
    /// <summary>
    /// Created by  :   Sumit Kate on 02 May 2016
    /// Description :   Lead Notification BL
    /// Modified BY : Lucky Rathore on 12 May 2016
    /// Description : Signature of Notify Dealer and SendEmailToDealer changed.
    /// </summary>
    public class LeadNotificationBL : ILeadNofitication
    {
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

                SendEmailSMSToDealerCustomer.SendEmailToCustomer(bikeName, bikeImage, dealerName, dealerEmail, dealerMobileNo, organization, address, customerName, customerEmail, priceList, offerList, pinCode, stateName, cityName, totalPrice,
                    versionName, dealerLat, dealerLong, workingHours);
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

        public void NotifyDealer(uint pqId, string makeName, string modelName, string versionName, string dealerName, string dealerEmail, string customerName, string customerEmail, string customerMobile, string areaName, string cityName, List<PQ_Price> priceList, int totalPrice, List<OfferEntity> offerList, string imagePath, string dealerMobile, string bikeName)
        {
            try
            {
                SendEmailSMSToDealerCustomer.SendEmailToDealer(makeName, modelName, versionName, dealerName, dealerEmail, customerName, customerEmail, customerMobile, areaName, cityName, priceList, totalPrice, offerList, imagePath);
                SendEmailSMSToDealerCustomer.SMSToDealer(dealerMobile, customerName, customerMobile, bikeName, areaName, cityName);
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
                using (IUnityContainer container = new UnityContainer())
                {
                    container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                    container.RegisterType<IPriceQuote, Bikewale.BAL.PriceQuote.PriceQuote>();
                    IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();
                    IPriceQuote objPriceQuote = container.Resolve<IPriceQuote>();
                    PriceQuoteParametersEntity details = objPriceQuote.FetchPriceQuoteDetailsById(pqId);
                    campaignId = details.CampaignId.HasValue ? details.CampaignId.Value.ToString() : "0";
                }

                string jsonInquiryDetails = String.Format("{{ \"CustomerName\": \"{0}\", \"CustomerMobile\":\"{1}\", \"CustomerEmail\":\"{2}\", \"VersionId\":\"{3}\", \"CityId\":\"{4}\", \"CampaignId\":\"{5}\", \"InquirySourceId\":\"39\", \"Eagerness\":\"1\",\"ApplicationId\":\"2\"}}", customerName, customerMobile, customerEmail, versionId, cityId, campaignId);

                TCApi_Inquiry objInquiry = new TCApi_Inquiry();
                abInquiryId = objInquiry.AddNewCarInquiry(dealerId, jsonInquiryDetails);

                if (!String.IsNullOrEmpty(abInquiryId))
                {
                    if (abInquiryId != "0" && abInquiryId != "-1")
                    {
                        using (IUnityContainer container = new UnityContainer())
                        {
                            container.RegisterType<IDealerPriceQuote, Bikewale.BAL.BikeBooking.DealerPriceQuote>();
                            IDealerPriceQuote objDealer = container.Resolve<IDealerPriceQuote>();
                            objDealer.PushedToAB(pqId, Convert.ToUInt32(abInquiryId));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "LeadNotificationBL.PushtoAB");
                objErr.SendMail();
            }
        }
    }
}
