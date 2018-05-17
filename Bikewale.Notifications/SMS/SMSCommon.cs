using Bikewale.Utility;
using MySql.CoreDAL;
using RabbitMqPublishing;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;

namespace Bikewale.Notifications
{
    /// <summary>
    /// Modified by:-Subodh Jain 24 Nov 2016
    /// Summary:- Added SMSForPhotoUploadTwoDays
    /// Modified by : Snehal Dange on 18th Jan 2017
    /// Description : added DealerShowroomDetailsSMSToCustomer
    /// Modified by : Kartik Rathod on 16 may 2018, added ESCampaignLeadSMSToCustomer
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
        RSAFreeHelmetSMS = 25,
        LimitedBikeBookedOffer = 26,
        ClaimedOffer = 27,
        BookingCancellationOTP = 28,
        BookingCancellationToCustomer = 29,
        SuccessfulUsedSelllistingToSeller = 30,
        ApprovalUsedSellListingToSeller = 31,
        RejectionUsedSellListingToSeller = 32,
        ApprovalEditedUsedBikeListingToSeller = 33,
        RejectionEditedUsedBikeListingToSeller = 34,
        ServiceCenterDetailsSMSToCustomer = 35,
        BikeListingExpirySevenDaySMSToSeller = 36,
        BikeListingExpiryOneDaySMSToSeller = 37,
        SMSForPhotoUploadTwoDays = 38,
        SMSforCapitalFirstSuccess = 39,
        SMSforCapitalFirstFailure = 40,
        DealerShowroomDetailsSMSToCustomer = 41,
        ESCampaignLeadSMSToCustomer = 42
    }

    public class SMSCommon
    {
        /// <summary>
        /// Modified By : Sadhana Upadhyaya on 5 Dec 2014
        /// Summary : To send multiple sms 
        /// </summary>
        /// <param name="numbers"></param>
        /// <param name="message"></param>
        /// <param name="esms"></param>
        /// <param name="pageUrl"></param>
        public void ProcessSMS(string numbers, string message, EnumSMSServiceType esms, string pageUrl)
        {
            if (!String.IsNullOrEmpty(numbers))
            {
                string[] arrMobileNos = numbers.Split(',');

                foreach (string number in arrMobileNos)
                {
                    ProcessSMS(number, message, esms, pageUrl, true);
                }
            }
        }

        public void ProcessSMS(string number, string message, EnumSMSServiceType esms, string pageUrl, bool isDND)
        {
            //first parse the number and verify that it is a mobile number
            //if the number is a mobile number then send the message and save 
            //it to the database along with the status whether it is sent successfully or not

            string mobile = ParseMobileNumber(number);

            if (mobile != "")
            {
                if (isDND == true)
                    mobile = "91" + mobile;

                string retMsg = "";
                string ctId = "-1";
                bool status = false;
                bool isMSMQ = false;

                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["isMSMQ"]))
                {
                    isMSMQ = Convert.ToBoolean(ConfigurationManager.AppSettings["isMSMQ"].ToString());
                }

                ctId = SaveSMSSentData(mobile, message, esms, status, retMsg, pageUrl);

                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("id", ctId);
                nvc.Add("message", message);
                nvc.Add("clientno", mobile);
                nvc.Add("prefix", "BW");
                nvc.Add("provider", "");

                RabbitMqPublish publish = new RabbitMqPublish();
                publish.PublishToQueue(BWConfiguration.Instance.BWSmsQueue, nvc);
            }
        }


        /// <summary>
        /// Created By : Sadhana Upadhyay on 22 Dec 2015
        /// Summary : To push sms in sms priority queue
        /// </summary>
        /// <param name="number"></param>
        /// <param name="message"></param>
        /// <param name="esms"></param>
        /// <param name="pageUrl"></param>
        /// <param name="isDND"></param>
        public void ProcessPrioritySMS(string number, string message, EnumSMSServiceType esms, string pageUrl, bool isDND)
        {
            //first parse the number and verify that it is a mobile number
            //if the number is a mobile number then send the message and save 
            //it to the database along with the status whether it is sent successfully or not

            string mobile = ParseMobileNumber(number);

            if (!String.IsNullOrEmpty(mobile))
            {
                if (isDND == true)
                    mobile = "91" + mobile;

                string retMsg = string.Empty;
                string ctId = "-1";
                bool status = false;
                bool isMSMQ = false;

                if (!String.IsNullOrEmpty(BWConfiguration.Instance.IsMSMQ))
                {
                    isMSMQ = Convert.ToBoolean(BWConfiguration.Instance.IsMSMQ);
                }

                ctId = SaveSMSSentData(mobile, message, esms, status, retMsg, pageUrl);

                NameValueCollection nvc = new NameValueCollection();
                nvc.Add("id", ctId);
                nvc.Add("message", message);
                nvc.Add("clientno", mobile);
                nvc.Add("prefix", "BW");
                nvc.Add("provider", "");

                RabbitMqPublish publish = new RabbitMqPublish();
                publish.PublishToQueue(BWConfiguration.Instance.BWPrioritySmsQueue, nvc);
            }
        }

        string SaveSMSSentData(string number, string message, EnumSMSServiceType esms, bool status, string retMsg, string pageUrl)
        {
            string currentId = string.Empty;
            try
            {

                using (DbCommand cmd = DbFactory.GetDBCommand("insertsmssent"))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Add(DbFactory.GetDbParam("par_number", DbType.String, 50, number));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_message", DbType.String, 500, message));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_servicetype", DbType.Int32, esms));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_smssentdatetime", DbType.DateTime, DateTime.Now));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_successfull", DbType.Boolean, status));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_returnedmsg", DbType.String, 500, retMsg));
                    cmd.Parameters.Add(DbFactory.GetDbParam("par_smspageurl", DbType.String, 500, pageUrl));
                    // LogLiveSps.LogSpInGrayLog(cmd);
                    currentId = Convert.ToString(MySqlDatabase.ExecuteScalar(cmd, ConnectionType.MasterDatabase));
                }
            }
            catch (Exception err)
            {
                ErrorClass.LogError(err, "Bikewale.Notifications.SMSCommon");
            } // catch Exception
            return currentId;
        }

        string ParseMobileNumber(string input)
        {
            return Bikewale.Utility.CommonValidators.ParseMobileNumber(input);
        }
    }
}
