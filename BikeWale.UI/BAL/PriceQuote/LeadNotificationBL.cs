using Bikewale.Cache.Core;
using Bikewale.Cache.MobileVerification;
using Bikewale.DAL.MobileVerification;
using Bikewale.Entities.BikeBooking;
using Bikewale.Entities.PriceQuote;
using Bikewale.Interfaces.BikeBooking;
using Bikewale.Interfaces.Cache.Core;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Interfaces.PriceQuote;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using RabbitMqPublishing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Bikewale.BAL.PriceQuote
{
    /// <summary>
    /// Created by  :   Sumit Kate on 02 May 2016
    /// Description :   Lead Notification BL
    /// Modified BY : Lucky Rathore on 12 May 2016
    /// Description : Signature of Notify Dealer and SendEmailToDealer changed.
    /// Modified by :   Sumit Kate on 18 Aug 2016
    /// Description :   Push Lead To Gaadi.com external API
    /// Modified by :  Aditi Srivastava on 16 Feb 2017
    /// Summary     :  Added a function to check if mobile number is blocked or not
    /// </summary>
    public class LeadNotificationBL : ILeadNofitication
    {
        private readonly IMobileVerificationCache _objMobileVerification;
        public LeadNotificationBL()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IMobileVerificationRepository, MobileVerificationRepository>()
                          .RegisterType<IMobileVerificationCache, MobileVerificationCache>()
                          .RegisterType<ICacheManager, MemcacheManager>();
                _objMobileVerification = container.Resolve<IMobileVerificationCache>();
            }
        }
        /// <summary>
        /// Sends Email and SMS to Customer
        /// Modified By : Lucky Rathore on 13 May 2016
        /// Description : parameter versionName, dealerLat, dealerLong, workingHours added.
        /// Modified by : Aditi Srivastava on 16 Feb 2017
        /// Summary     : Added function to check if mobile number is authentic before notifying customer
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
                if (!IsFakeMobileNumber(objDPQSmsEntity.CustomerMobile))
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
                    if (leadSourceId != 16 && leadSourceId != 22 && !String.IsNullOrEmpty(customerEmail))
                    {
                        SendEmailSMSToDealerCustomer.SendEmailToCustomer(bikeName, bikeImage, dealerName, dealerEmail, dealerMobileNo, organization, address, customerName, customerEmail, priceList, offerList, pinCode, stateName, cityName, totalPrice,
                            versionName, dealerLat, dealerLong, workingHours);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "LeadNotificationBL.NotifyCustomer");
            }
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 26 JUne 2018
        /// Description : changes PQId data type
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
        /// <param name="versionName"></param>
        /// <param name="dealerLat"></param>
        /// <param name="dealerLong"></param>
        /// <param name="workingHours"></param>
        /// <param name="platformId"></param>
        public void NotifyCustomerV2(string pqId, string bikeName, string bikeImage, string dealerName, string dealerEmail, string dealerMobileNo, string organization, string address, string customerName, string customerEmail, List<PQ_Price> priceList, List<OfferEntity> offerList, string pinCode, string stateName, string cityName, uint totalPrice, DPQSmsEntity objDPQSmsEntity, string requestUrl, uint? leadSourceId, string versionName, double dealerLat, double dealerLong, string workingHours, string platformId = "")
        {
            try
            {
                if (!IsFakeMobileNumber(objDPQSmsEntity.CustomerMobile) && !String.IsNullOrEmpty(pqId))
                {
                    //Different SMS is sent if lead is submitted from BikeWale APP
                    if (platformId == "3" || platformId == "4")
                    {
                        SendEmailSMSToDealerCustomer.SendSMSToCustomerV2(pqId, requestUrl, objDPQSmsEntity, DPQTypes.AndroidAppOfferNoBooking);
                    }
                    else
                    {
                        //If lead is submitted while Booking a bike online don't sent SMS to customer
                        if (leadSourceId != 16 && leadSourceId != 22)
                            SendEmailSMSToDealerCustomer.SendSMSToCustomerV2(pqId, requestUrl, objDPQSmsEntity, DPQTypes.SubscriptionModel);
                    }
                    //If lead is submitted while Booking a bike online don't sent SMS to customer
                    if (leadSourceId != 16 && leadSourceId != 22 && !String.IsNullOrEmpty(customerEmail))
                    {
                        SendEmailSMSToDealerCustomer.SendEmailToCustomer(bikeName, bikeImage, dealerName, dealerEmail, dealerMobileNo, organization, address, customerName, customerEmail, priceList, offerList, pinCode, stateName, cityName, totalPrice,
                            versionName, dealerLat, dealerLong, workingHours);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "LeadNotificationBL.NotifyCustomerV2");
            }
        }

        /// <summary>
        /// Sends SMS and Email to Dealer
        /// Modified BY : Lucky Rathore on 12 May 2016
        /// Description : Signature of Notify Dealer and SendEmailToDealer.
        /// Modified By : Lucky Rathore on 11 July 2016.
        /// Description : parameter dealerArea added in NotifyDealer(). 
        /// Modified by : Aditi Srivastava on 16 Feb 2017
        /// Summary     : Added function to check if mobile number is authentic before notifying dealer
        /// Modified by : Pratibha Verma on 27 April 2018
        /// Description : Added additional commumication parameters
        /// Modified by : Pratibha Verma on 30 July 2018
        /// Description : Add HttpContext URL to call SMSToDealer method
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

        public void NotifyDealer(uint pqId, string makeName, string modelName, string versionName, string dealerName, string dealerEmail, string customerName, string customerEmail, string customerMobile, string areaName, string cityName, List<PQ_Price> priceList, int totalPrice, List<OfferEntity> offerList, string imagePath, string dealerMobile, string bikeName, string dealerArea, string additionalNumbers, string additionalEmails)
        {
            try
            {
                if (!IsFakeMobileNumber(customerMobile) && pqId > 0)
                {
                    SendEmailSMSToDealerCustomer.SendEmailToDealer(makeName, modelName, versionName, dealerName, dealerEmail, customerName, customerEmail, customerMobile, areaName, cityName, priceList, totalPrice, offerList, imagePath, additionalEmails);
                    SendEmailSMSToDealerCustomer.SMSToDealer(dealerMobile, customerName, customerMobile, bikeName, areaName, cityName, dealerArea, additionalNumbers, HttpContext.Current.Request.ServerVariables["URL"].ToString());
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "LeadNotificationBL.NotifyDealer");
            }
        }

        /// <summary>
        /// Created by  : Pratibha Verma on 26 June 2018
        /// Description : changes PQId data type
        /// Modified by : Pratibha Verma on 30 July 2018
        /// Description : Add HttpContext URL to call SMSToDealer method
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
        /// <param name="dealerArea"></param>
        /// <param name="additionalNumbers"></param>
        /// <param name="additionalEmails"></param>
        public void NotifyDealerV2(string pqId, string makeName, string modelName, string versionName, string dealerName, string dealerEmail, string customerName, string customerEmail, string customerMobile, string areaName, string cityName, List<PQ_Price> priceList, int totalPrice, List<OfferEntity> offerList, string imagePath, string dealerMobile, string bikeName, string dealerArea, string additionalNumbers, string additionalEmails)
        {
            try
            {
                if (!IsFakeMobileNumber(customerMobile) && !String.IsNullOrEmpty(pqId))
                {
                    SendEmailSMSToDealerCustomer.SendEmailToDealer(makeName, modelName, versionName, dealerName, dealerEmail, customerName, customerEmail, customerMobile, areaName, cityName, priceList, totalPrice, offerList, imagePath, additionalEmails);
                    SendEmailSMSToDealerCustomer.SMSToDealer(dealerMobile, customerName, customerMobile, bikeName, areaName, cityName, dealerArea, additionalNumbers, HttpContext.Current.Request.ServerVariables["URL"].ToString());
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "LeadNotificationBL.NotifyDealerV2");
            }
        }

        /// <summary>
        /// Pushes Inquiry in AutoBiz using API
        /// Modified By : Sushil Kumar on 29th Nov 2016
        /// Description : Added feature to pass autobiz leads only when dealer leads does not exceeds daily limit count
        /// Modified by : Aditi Srivastava on 14 Feb 2017
        /// Summary     : Added function to check if mobile number is authentic before pushing lead
        /// Modified by :   Sumit Kate on 24 Feb 2017
        /// Description :   If AbInquiryId is invalid Push Inquiry Json to Rabbit Mq
        /// Modified by : Pratibha Verma on 26 June 2018
        /// Description : added parameters pqGuId and leadId
        /// Modified by : Pratibha Verma on 14 November 2018
        /// Description : added parameter campaignId
        /// Modified by : Kartik Rathod on 19 nov 2019
        /// Desc        : added inquirySource , otherData and comments
        /// </summary>
        /// <param name="dealerId"></param>
        /// <param name="pqId"></param>
        /// <param name="customerName"></param>
        /// <param name="customerMobile"></param>
        /// <param name="customerEmail"></param>
        /// <param name="versionId"></param>
        /// <param name="cityId"></param>
        /// <param name="pqGuId"></param>
        /// <param name="leadId"></param>
        /// <param name="campaignId"></param>
        /// <param name="inquirySource">39 for bikewale new lead by default</param>
        /// <param name="otherData">extra datea send to autobiz api in others field</param>
        /// <param name="comments">comment sent by ab</param>
        public void PushtoAB(string dealerId, uint pqId, string customerName, string customerMobile, string customerEmail, string versionId, string cityId, string pqGuId, uint leadId, uint campaignId = 0, ushort inquirySource = 39, string otherData = "", string comments = "")
        {
            string abInquiryId = string.Empty, message = string.Empty;
            bool isNotFakeMobileNumber = false;
            try
            {

                isNotFakeMobileNumber = !IsFakeMobileNumber(customerMobile);


                //update dealer's daily lead count
                if (isNotFakeMobileNumber)
                {
                    NameValueCollection objNVC = new NameValueCollection();
                    objNVC.Add("pqId", Convert.ToString(pqId));
                    objNVC.Add("dealerId", dealerId);
                    objNVC.Add("customerName", customerName);
                    objNVC.Add("customerEmail", customerEmail);
                    objNVC.Add("customerMobile", customerMobile);
                    objNVC.Add("versionId", versionId);
                    objNVC.Add("cityId", cityId);
                    objNVC.Add("pqGUId", pqGuId);
                    objNVC.Add("manufacturerLeadId", Convert.ToString(leadId));
                    objNVC.Add("campaignId", Convert.ToString(campaignId));
                    objNVC.Add("others", otherData);
                    objNVC.Add("inquirySource", Convert.ToString(inquirySource));
                    objNVC.Add("comments", comments);

                    RabbitMqPublish objRMQPublish = new RabbitMqPublish();
                    objRMQPublish.PublishToQueue(Bikewale.Utility.BWConfiguration.Instance.LeadConsumerQueue, objNVC);
                }

            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "LeadNotificationBL.PushtoAB");
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 29th Nov 2016
        /// Description : Update dealer campaign daily lead count
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
                ErrorClass.LogError(ex, "LeadNotificationBL.UpdateDealerDailyLeadCount");
            }
            return isUpdateDealerCount;
        }

        /// <summary>
        /// Created by : Aditi Srivastava on 16 Feb 2017
        /// Summar     : Check if a number is a part of blocked numbers list
        /// </summary>
        /// <param name="mobileNumber"></param>
        /// <returns></returns>
        private bool IsFakeMobileNumber(string mobileNumber)
        {
            bool isFake = false;
            IEnumerable<string> numberList = null;
            try
            {
                numberList = _objMobileVerification.GetBlockedNumbers();
                isFake = numberList.Contains(mobileNumber);
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "LeadNotificationBL.IsFakeMobileNumber");
            }
            return isFake;
        }

    }
}
