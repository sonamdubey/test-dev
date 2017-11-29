using Bikewale.Utility;
using MySql.CoreDAL;
using RabbitMqPublishing;
using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Net;
using System.Text;
using System.Web;

namespace Bikewale.Common
{
    /// <summary>
    ///     Created By : Ashish G. Kamble on 23/8/2012
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

                string ctId = SaveSMSSentData(mobile, message, esms, false, string.Empty, pageUrl);



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

                string ctId = SaveSMSSentData(mobile, message, esms, false, string.Empty, pageUrl);

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
            return CommonOpn.ParseMobileNumber(input);
        }

        string SendSMS(string number, string message)
        {
            return SendSMS(number, message, false);
        }

        string SendSMS(string number, string message, bool isDND)
        {
            bool allowSMS = ConfigurationManager.AppSettings["SendSMS"] == "0" ? false : true;
            // stop sending sms if key(SendSMS) value is 0 else allow
            // Locally it will be turned off and on server it will be turned on in web.config file
            if (!allowSMS)
                return string.Empty;

            string retVal = "";
            string url = "";

            //remove special characters from the message
            message = HttpContext.Current.Server.UrlEncode(message);

            try
            {
                //url to send SMS through International Gateway 
                //url = "http://push1.maccesssmspush.com/servlet/com.aclwireless.pushconnectivity.listeners.TextListener?userId=intauto&pass=intauto&contenttype=1&to=" + number + "&from=BikeWale&text=" + message + "&selfid=true&alert=1&dlrreq=true&intflag=true";

                if (isDND == true)
                {
                    //url tp send SMS through Netcore. This will send SMS even to DND customers.
                    url = string.Format(@"http://bulkpush.mytoday.com/BulkSms/SingleMsgApi?feedid=337605&username=9967335511&password=tdjgd&To={0}&Text={1}&time=&senderid=", number, message);
                }
                else
                {
                    //url to send SMS through ACL wireless                    
                    url = string.Format(@"http://push1.maccesssmspush.com/servlet/com.aclwireless.pushconnectivity.listeners.TextListener?userId=autoex&pass=autoex&appid=autoex&subappid=autoex&contenttype=1&to={0}&from=BIKWAL&text={1}&selfid=true&alert=1&dlrreq=true", number, message);
                }

                using (WebClient webClient = new WebClient())
                {
                    var reqHTML = webClient.DownloadData(url);
                    UTF8Encoding objUTF8 = new UTF8Encoding();
                    retVal = objUTF8.GetString(reqHTML);
                }

            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Common.SMSCommon : " + err.Message);
                ErrorClass.LogError(err, "Common.SMSCommon");
                
            }

            return retVal;
        }

        public static string SendNanoSMS(string number, string message)
        {
            string retVal = "";

            if (HttpContext.Current.Request.ServerVariables["HTTP_HOST"].IndexOf("bikewale.com") >= 0)
            {
                try
                {
                    string url = string.Format(@"http://push1.maccesssmspush.com/servlet/com.aclwireless.pushconnectivity.listeners.TextListener?userId=idcarw&pass=pacarw&appid=carw&to={0}&from=BikeWale&contenttype=1&selfid=true&text={1}&dlrreq=true&alert=1", number, message);


                    using (WebClient webClient = new WebClient())
                    {
                        var reqHTML = webClient.DownloadData(url);
                        UTF8Encoding objUTF8 = new UTF8Encoding();
                        retVal = objUTF8.GetString(reqHTML);
                    }
                }
                catch (Exception err)
                {
                    HttpContext.Current.Trace.Warn("Common.SendNanoSMS : " + err.Message);
                    ErrorClass.LogError(err, "Common.SendNanoSMS");
                    
                }
            }
            return retVal;
        }

    }   // End of class
}   // End of namespace