using AppNotification.Entity;
using AppNotification.Interfaces;
using AppNotification.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Web;
//using Consumer;

namespace AppNotification.BAL
{
    public class AndroidGCMAPI<TResponse> : IAPIService<TResponse>

        where TResponse : APIResponseEntity, new()
    {

        private static readonly string _androidGlobalTopic = ConfigurationManager.AppSettings["AndroidGlobalTopic"];
        private static readonly string _FCMSendURL = ConfigurationManager.AppSettings["FCMSendURL"];
        private static readonly string _FCMApiKey = ConfigurationManager.AppSettings["FCMApiKey"];
        private static readonly string _genericQueueName = ConfigurationManager.AppSettings["GenericQueueName"];
        private static readonly int _maxRetries = 3;


        public TResponse Request(List<string> regKeyList)
        {

            string postData = GetGCMData(regKeyList);
            SubscriptionResponse subscriptionResponse = SubscribeFCMNotification(postData, 0);
            var responseEntity = new TResponse()
            {
                ResponseText = Convert.ToString(subscriptionResponse),
            };
            return responseEntity;
        }
        public string GetGCMData(List<string> regKeyList)
        {
            try
            {
                var fcmDataObj = new GCMFormat();
                fcmDataObj.to = string.Format("/topics/{0}", _androidGlobalTopic);
                fcmDataObj.registration_tokens = regKeyList;

                //Infolog.Info("GetGCMDATA1");
                return JsonConvert.SerializeObject(fcmDataObj);
            }
            catch (Exception ex)
            {
                //Infolog.Info("GetGCMDATA2" + ex.Message);
                return "";
            }
        }

        //public MobileAppNotificationBase GetGCMBaseData(T t)
        //{
        //    var gcmBaseDataObj = new MobileAppNotificationBase();
        //    gcmBaseDataObj.title = t.title;
        //    gcmBaseDataObj.detailUrl = t.detailUrl;
        //    gcmBaseDataObj.smallPicUrl = t.smallPicUrl;
        //    gcmBaseDataObj.alertTypeId = t.alertTypeId;
        //    gcmBaseDataObj.alertId = t.alertId;
        //    gcmBaseDataObj.largePicUrl = t.largePicUrl;
        //    gcmBaseDataObj.isFeatured = t.isFeatured;
        //    gcmBaseDataObj.publishDate = t.publishDate;
        //    return gcmBaseDataObj;
        //}

        //private string SendGCMNotification(string apiKey, string postData, string postDataContentType)
        //{
        //    //If SSL certification is self signed then too the following line of code will Validate the server certificate
        //    ServicePointManager.ServerCertificateValidationCallback += new RemoteCertificateValidationCallback(ValidateServerCertificate);

        //    //Logs.WriteInfoLog(String.Format("postData  : {0}", postData));
        //    //  MESSAGE CONTENT
        //    byte[] byteArray = Encoding.UTF8.GetBytes(postData);

        //    //  CREATE REQUEST
        //    HttpWebRequest Request = (HttpWebRequest)WebRequest.Create("https://android.googleapis.com/gcm/send");
        //    Request.Method = "POST";
        //    Request.KeepAlive = false;
        //    Request.ContentType = postDataContentType;
        //    Request.Headers.Add(String.Format("Authorization: key={0}", apiKey));
        //    Request.Headers.Add(string.Format("Sender: id={0}", ConfigurationManager.AppSettings["SenderId"]));
        //    Request.ContentLength = byteArray.Length;
        //    Stream dataStream = Request.GetRequestStream();
        //    dataStream.Write(byteArray, 0, byteArray.Length);
        //    dataStream.Close();
        //    string responseLine = string.Empty;

        //    //  SEND MESSAGE
        //    try
        //    {
        //        WebResponse Response = Request.GetResponse();
        //        StreamReader Reader = new StreamReader(Response.GetResponseStream());
        //        responseLine = Reader.ReadToEnd();
        //        Reader.Close();


        //        //Logs.WriteInfoLog(String.Format("api Response : {0}",responseLine));
        //        return responseLine;
        //    }
        //    catch (Exception e)
        //    {
        //        responseLine = e.Message.ToString();
        //    }
        //    return responseLine;
        //}

        //private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        //{
        //    return true;
        //}

        public SubscriptionResponse SubscribeFCMNotification(string payload, int retries)
        {
            SubscriptionResponse subsResponse = null;
            try
            {
                if (_maxRetries > retries)
                {
                    subsResponse = new SubscriptionResponse();

                    WebRequest tRequest = WebRequest.Create("https://iid.googleapis.com/iid/v1:batchAdd");
                    tRequest.Method = "POST";
                    tRequest.ContentType = "application/json";
                    tRequest.Headers.Add(string.Format("Authorization: key={0}", _FCMApiKey));

                    Byte[] byteArray = Encoding.UTF8.GetBytes(payload);

                    tRequest.ContentLength = byteArray.Length;

                    using (Stream dataStream = tRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);

                        using (HttpWebResponse tResponse = (HttpWebResponse)tRequest.GetResponse())
                        {

                            using (Stream dataStreamResponse = tResponse.GetResponseStream())
                            {
                                using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    if (tResponse.StatusCode.Equals(HttpStatusCode.OK) && subsResponse.Results == null)
                                    {
                                        String sResponseFromServer = tReader.ReadToEnd();
                                        subsResponse = JsonConvert.DeserializeObject<SubscriptionResponse>(sResponseFromServer);
                                    }
                                    else if (tResponse.StatusCode.Equals(HttpStatusCode.ServiceUnavailable))
                                    {
                                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(30)); //hard coded 30 seconds | should be exponential backoff
                                        return SubscribeFCMNotification(payload, retries + 1);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " - SendFCMNotification");
                objErr.SendMail();
            }

            return subsResponse;
        }

    }


    public class NotificationResponse
    {
        public string Error { get; set; }
    }

    public class FCMPushNotificationStatus
    {
        public bool Successful { get; set; }

        public NotificationResponse Response { get; set; }

        public Exception Error { get; set; }
    }
    [Serializable]
    public class SubscriptionResponse
    {
        [JsonProperty("results")]
        public List<SubscriptionResult> Results { get; set; }
    }
    [Serializable]
    public class SubscriptionResult
    {
        [JsonProperty("error")]
        public string Error { get; set; }
    }




}
