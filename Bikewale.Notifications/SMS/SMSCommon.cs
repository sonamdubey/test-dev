using Bikewale.Utility;
using RabbitMqPublishing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Bikewale.Notifications
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
        RSAFreeHelmetSMS = 25,
        LimitedBikeBookedOffer = 26
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
                publish.PublishToQueue("Sms", nvc);
            }
        }

        /// <summary>
        /// Method to update the database with the details of the SMS data sent.
        /// </summary>
        /// <param name="currentId">Id for which the sms was just sent</param>
        /// <param name="retMsg">The return message from the provider that is received after the SMS is sent</param>
        private void UpdateSMSSentData(string currentId, string retMsg)
        {
            if (!String.IsNullOrEmpty(currentId))
            {
                string sql = "UPDATE SMSSent SET ReturnedMsg = @RetMsg WHERE ID = @CurrentId";
                try
                {
                    using (SqlConnection con = new SqlConnection())
                    {
                        using (SqlCommand cmd = new SqlCommand())
                        {
                            cmd.CommandText = sql;
                            cmd.CommandType = CommandType.Text;
                            cmd.Parameters.Add("@CurrentId", SqlDbType.Int).Value = Convert.ToInt32(currentId);
                            cmd.Parameters.Add("@RetMsg", SqlDbType.VarChar).Value = retMsg;
                            con.ConnectionString = ConfigurationManager.AppSettings["bwconnectionstring"];
                            con.Open();
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (SqlException)
                {

                }
                catch (Exception)
                {

                }
            }
        }

        string SaveSMSSentData(string number, string message, EnumSMSServiceType esms, bool status, string retMsg, string pageUrl)
        {
            SqlConnection con;
            SqlCommand cmd;
            SqlParameter prm;
            string conStr = ConfigurationManager.AppSettings["bwconnectionstring"];
            string currentId = string.Empty;
            try
            {
                using (con = new SqlConnection(conStr))
                {
                    using (cmd = new SqlCommand("InsertSMSSent", con))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        prm = cmd.Parameters.Add("@Number", SqlDbType.VarChar, 50);
                        prm.Value = number;

                        prm = cmd.Parameters.Add("@Message", SqlDbType.VarChar, 500);
                        prm.Value = message;

                        prm = cmd.Parameters.Add("@ServiceType", SqlDbType.Int);
                        prm.Value = esms;

                        prm = cmd.Parameters.Add("@SMSSentDateTime", SqlDbType.DateTime);
                        prm.Value = DateTime.Now;

                        prm = cmd.Parameters.Add("@Successfull", SqlDbType.Bit);
                        prm.Value = status;

                        prm = cmd.Parameters.Add("@ReturnedMsg", SqlDbType.VarChar, 500);
                        prm.Value = retMsg;

                        prm = cmd.Parameters.Add("@SMSPageUrl", SqlDbType.VarChar, 500);
                        prm.Value = pageUrl;

                        con.Open();
                        //run the command
                        //cmd.ExecuteNonQuery();
                        currentId = Convert.ToString(cmd.ExecuteScalar());
                    }
                }
            }
            catch (SqlException err)
            {
                //HttpContext.Current.Trace.Warn("Common.SMSCommon : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Notifications.SMSCommon");
                objErr.SendMail();
            } // catch SqlException
            catch (Exception err)
            {
                //HttpContext.Current.Trace.Warn("Common.SMSCommon : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Bikewale.Notifications.SMSCommon");
                objErr.SendMail();
            } // catch Exception
            return currentId;
        }

        string ParseMobileNumber(string input)
        {
            return Bikewale.Utility.CommonValidators.ParseMobileNumber(input);
        }
    }
}
