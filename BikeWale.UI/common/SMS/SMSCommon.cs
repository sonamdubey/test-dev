using System;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections.Specialized;
using RabbitMqPublishing;

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

                string retMsg = "";
                string ctId = "-1";
                bool status = false;
                bool isMSMQ = false;

                if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["isMSMQ"]))
                {
                    isMSMQ = Convert.ToBoolean(ConfigurationManager.AppSettings["isMSMQ"].ToString());
                }

                ctId = SaveSMSSentData(mobile, message, esms, status, retMsg, pageUrl);

                // Use MSMQ if enabled in Web.Config.
                //if (!isMSMQ)
                //{
                //    retMsg = SendSMS(mobile, message, isDND);

                //    if (retMsg.IndexOf("bikew") != -1)
                //        status = true;
                //    else
                //        status = false;

                //    UpdateSMSSentData(ctId, retMsg);
                //}
                //else
                //{
                //    MSMQLibrary.SendSMSMSMQ.SendSMS(ctId, message, mobile, "BW", string.Empty);
                //}


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
        /// PopulateWhere to update the database with the details of the SMS data sent.
        /// </summary>
        /// <param name="currentId">Id for which the sms was just sent</param>
        /// <param name="retMsg">The return message from the provider that is received after the SMS is sent</param>
        private void UpdateSMSSentData(string currentId, string retMsg)
        {
            if (currentId != "")
            {
                SqlCommand cmd = new SqlCommand();
                Database db = new Database();

                string sql = "UPDATE SMSSent SET ReturnedMsg = @RetMsg WHERE ID = @CurrentId";

                try
                {
                    cmd.CommandText = sql;
                    cmd.CommandType = CommandType.Text;
                    cmd.Parameters.Add("@CurrentId", SqlDbType.Int).Value = Convert.ToInt32(currentId);
                    cmd.Parameters.Add("@RetMsg", SqlDbType.VarChar).Value = retMsg;

                    db.UpdateQry(cmd);
                }
                catch (SqlException ex)
                {
                    HttpContext.Current.Trace.Warn("Error Message: " + ex);
                }
                catch (Exception e)
                {
                    HttpContext.Current.Trace.Warn("Error: " + e);
                }
                finally
                {
                    db.CloseConnection();
                }
            }
        }


        string SaveSMSSentData(string number, string message, EnumSMSServiceType esms, bool status, string retMsg, string pageUrl)
        {
            SqlConnection con;
            SqlCommand cmd;
            SqlParameter prm;
            Database db = new Database();
            CommonOpn op = new CommonOpn();

            string conStr = db.GetConString();
            string currentId = string.Empty;

            con = new SqlConnection(conStr);

            try
            {
                cmd = new SqlCommand("InsertSMSSent", con);
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
            catch (SqlException err)
            {
                HttpContext.Current.Trace.Warn("Common.SMSCommon : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Common.SMSCommon");
                objErr.SendMail();
            } // catch SqlException
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Common.SMSCommon : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Common.SMSCommon");
                objErr.SendMail();
            } // catch Exception
            finally
            {
                //close the connection	
                if (con.State == ConnectionState.Open)
                {
                    con.Close();
                }
            }

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
                    url = "http://bulkpush.mytoday.com/BulkSms/SingleMsgApi?feedid=337605&username=9967335511&password=tdjgd&To=" + number + "&Text=" + message + "&time=&senderid=";                    
                }
                else
                {
                    //url to send SMS through ACL wireless                    
                    url = "http://push1.maccesssmspush.com/servlet/com.aclwireless.pushconnectivity.listeners.TextListener?userId=autoex&pass=autoex&appid=autoex&subappid=autoex&contenttype=1&to=" + number +"&from=BIKWAL&text=" + message + "&selfid=true&alert=1&dlrreq=true";
                }

                WebClient webClient = new WebClient();
                string strUrl = url;
                byte[] reqHTML;
                reqHTML = webClient.DownloadData(strUrl);
                UTF8Encoding objUTF8 = new UTF8Encoding();

                retVal = objUTF8.GetString(reqHTML);
            }
            catch (Exception err)
            {
                HttpContext.Current.Trace.Warn("Common.SMSCommon : " + err.Message);
                ErrorClass objErr = new ErrorClass(err, "Common.SMSCommon");
                objErr.SendMail();
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
                    string url = "http://push1.maccesssmspush.com/servlet/com.aclwireless.pushconnectivity.listeners.TextListener?userId=idcarw&pass=pacarw&appid=carw&to=" + number + "&from=BikeWale&contenttype=1&selfid=true&text=" + message + "&dlrreq=true&alert=1";

                    WebClient webClient = new WebClient();
                    string strUrl = url;
                    byte[] reqHTML;
                    reqHTML = webClient.DownloadData(strUrl);
                    UTF8Encoding objUTF8 = new UTF8Encoding();

                    retVal = objUTF8.GetString(reqHTML);
                }
                catch (Exception err)
                {
                    HttpContext.Current.Trace.Warn("Common.SendNanoSMS : " + err.Message);
                    ErrorClass objErr = new ErrorClass(err, "Common.SendNanoSMS");
                    objErr.SendMail();
                }
            }
            return retVal;
        }

    }   // End of class
}   // End of namespace