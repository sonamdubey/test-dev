using Consumer;
using RabbitMqPublishing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PriceQuoteLeadSMS
{
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

    public class SMSCommon
    {
        protected RabbitMqPublish publish = new RabbitMqPublish();

        /// <summary>
        /// Queues the SMS to RabbitMq
        /// </summary>
        /// <param name="sms"></param>
        private void SendSMSEx(uint smsId, string message, string customerContact)
        {
            string appName = String.Empty;
            NameValueCollection nvcSMS = null;
            try
            {
                appName = ConfigurationManager.AppSettings["rabbitMqAppName"];

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
    }
}
