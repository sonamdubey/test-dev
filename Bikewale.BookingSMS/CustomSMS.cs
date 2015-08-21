using Consumer;
using RabbitMqPublishing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;

namespace Bikewale.BookingSMS
{
    /// <summary>
    /// Enumeration to specify the SMS type
    /// </summary>
    public enum EnumSMSServiceType
    {
        UsedPurchaseInquiryIndividualSeller = 1,
        UsedPurchaseInquiryDealerSeller = 2,
        UsedPurchaseInquiryIndividualBuyer = 3,
        UsedPurchaseInquiryDealerBuyer = 4,
        NewBikeSellOpr = 5,
        NewBikeBuyOpr = 6,
        AccountDetails = 7,
        CustomerRegistration = 8,
        DealerAddressRequest = 9,
        SellInquiryReminder = 10,
        PromotionalOffer = 11,
        NewBikeQuote = 12,
        CustomSMS = 13,
        CustomerPassword = 14,
        MobileVerification = 15,
        PaidRenewalAlert = 16,
        PaidRenewal = 17,
        MyGarageSMS = 18,
        MyGarageAlerts = 19,
        InsuranceRenewal = 20,
        NewBikePriceQuoteSMSToDealer = 21,
        NewBikePriceQuoteSMSToCustomer = 22,
        BikeBookedSMSToCustomer = 23,
        BikeBookedSMSToDealer = 24,
        RSAFreeHelmetSMS = 25
    }

    /// <summary>
    /// Author  : Sumit Kate
    /// Created : 16 July 2015
    /// Class handle the SMS queuing to RabbitMq
    /// </summary>
    public class CustomSMS
    {
        protected RabbitMqPublish publish = new RabbitMqPublish();
        /// <summary>
        /// Sends SMS by fectching the records from Database
        /// </summary>
        public void SendSMS()
        {
            IEnumerable<CustomSMSEntity> lstSMS = null;
            CustomSMSDAL objDal = null;
            try
            {
                objDal = new CustomSMSDAL();
                lstSMS = objDal.FetchSMSData();
                if (lstSMS != null)
                {
                    foreach (CustomSMSEntity smsEntity in lstSMS)
                    {
                        SendSMSEx(smsEntity);
                    }
                }
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in SendSMS : " + ex.Message);
            }
        }

        /// <summary>
        /// Formats the SMS body based on the CustomSMSEntity
        /// Note : Here the %26 represents the & character, because RabbitMq parses '&' as query string delimiter
        /// hence %26 is used instead of &
        /// </summary>
        /// <param name="sms">object of CustomSMSEntity</param>
        /// <returns>Formatted SMS string</returns>
        private string FormatSMS(CustomSMSEntity sms)
        {
            string smsMessage = String.Format("Avail your FREE Vega Helmet %26 1-year RSA from BikeWale on purchase {0} from {1} ({2}). If already purchased, claim your offer here - {3}.", sms.BikeName, sms.DealerName, sms.DealerContact, ConfigurationManager.AppSettings["RemindOfferPageShortUrl"]);
            return smsMessage;
        }

        /// <summary>
        /// Queues the SMS to RabbitMq
        /// </summary>
        /// <param name="sms"></param>
        private void SendSMSEx(CustomSMSEntity sms)
        {
            int smsId = 0;
            string message = String.Empty;
            string customerContact = String.Empty;
            string appName = String.Empty;
            NameValueCollection nvcSMS = null;            
            try
            {
                appName = ConfigurationManager.AppSettings["rabbitMqAppName"];
                message = FormatSMS(sms);
                customerContact = String.Format("91{0}", sms.CustomerContact);
                smsId = SaveSMSToDb(sms.CustomerContact, message, EnumSMSServiceType.RSAFreeHelmetSMS, String.Empty);
                Logs.WriteInfoLog(String.Format("Added Into DB : {0}", smsId));
                nvcSMS = new NameValueCollection();
                nvcSMS.Add("id", Convert.ToString(smsId));
                nvcSMS.Add("message", message);
                nvcSMS.Add("clientno", customerContact);
                nvcSMS.Add("prefix", "BW");
                nvcSMS.Add("provider", "");

                publish.PublishToQueue(appName, nvcSMS);
                Logs.WriteInfoLog(String.Format("Added Into Queue : {0}", smsId));
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in SendSMSEx : " + ex.Message);
            }

        }

        /// <summary>
        /// Saves the SMS details into the Database
        /// </summary>
        /// <param name="number">Mobile Number</param>
        /// <param name="message">Message</param>
        /// <param name="smsType">SMS type</param>
        /// <param name="retMsg"></param>
        /// <returns>sms id</returns>
        private int SaveSMSToDb(string number, string message, EnumSMSServiceType smsType, string retMsg)
        {
            int smsId = 0;
            try
            {
                CustomSMSDAL objDal = new CustomSMSDAL();
                smsId = Convert.ToInt32(objDal.InsertSMS(number, message, smsType, retMsg, false));
            }
            catch (Exception ex)
            {
                Logs.WriteErrorLog("Exception in SaveSMSToDb : " + ex.Message);
            }
            return smsId;
        }
    }
}
