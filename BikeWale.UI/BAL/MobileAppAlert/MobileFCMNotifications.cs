using Bikewale.Entities.MobileAppAlert;
using Bikewale.Interfaces.MobileAppAlert;
using Bikewale.Notifications;
using Bikewale.Utility.AndroidAppAlert;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Bikewale.BAL.MobileAppAlert
{

    public class MobileFCMNotifications : IMobileAppAlert
    {
        private const int _oneWeek = 604800;
        private const int _maxRetries = 3;
        private readonly IMobileAppAlertRepository _appAlertRepo = null;

        /// <summary>
        /// Created By : Sushil Kumar on 12nd Dec 2016
        /// Description : To resolve unity container for dal methods
        /// </summary>
        public MobileFCMNotifications()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IMobileAppAlertRepository, Bikewale.DAL.MobileAppAlert.MobileAppAlert>();
                _appAlertRepo = container.Resolve<Bikewale.DAL.MobileAppAlert.MobileAppAlert>();
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12nd Dec 2016
        /// Description : To subscribe fcm user
        /// Modified By : Sushil Kumar on 15th Dec 2016
        /// Description : Added consdition to check for news category 1,2 and remove 1 if contains
        ///                 Handle condition to register multiple subscription topics
        /// </summary>
        /// <param name="appInput"></param>
        /// <returns></returns>
        public bool SubscribeFCMUser(AppFCMInput appInput)
        {
            string msg = string.Empty, subscriptionTopic = string.Empty;
            ushort subscriptionId; bool isSuccess = false;
            try
            {

                //for supporting earlier versions of app related to news subscription
                List<string> submasterIds = new List<string>(appInput.SubsMasterId.Split(','));
                submasterIds.RemoveAll(item => item.Contains("1"));

                foreach (string _submasterId in submasterIds)
                {

                    if (ushort.TryParse(_submasterId, out subscriptionId))
                    {

                        subscriptionTopic = SubscriptionTypes.GetSubscriptionType(subscriptionId);
                        SubscriptionRequest subscriptionRequest = new SubscriptionRequest() { To = subscriptionTopic, RegistrationTokens = new List<string> { appInput.GcmId } };
                        string payload = JsonConvert.SerializeObject(subscriptionRequest);

                        SubscriptionResponse subscriptionResponse = SubscribeFCMNotification(Bikewale.Utility.BWConfiguration.Instance.FCMSusbscribeUserUrl, payload, 0);
                        if (subscriptionResponse != null)
                        {
                            var result = subscriptionResponse.Results[0];
                            if (string.IsNullOrEmpty(result.Error))
                            {
                                isSuccess = _appAlertRepo.SaveIMEIFCMData(appInput.Imei, appInput.GcmId, appInput.OsType, appInput.SubsMasterId);
                            }
                            else
                            {
                                msg = string.Format("Android : Subscribing failed for Registration id - {0} : {1} due to {2}", appInput.Imei, appInput.GcmId, result.Error);
                                ErrorClass.LogError(new Exception(), string.Format("{0} - SubscribeUser : IMEI : {1}, GCMId : {2}, Message : {3} ,subsmasterId : {4}", HttpContext.Current.Request.ServerVariables["URL"], appInput.Imei, appInput.GcmId, msg, appInput.SubsMasterId));
                                

                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("{0} - Bikewale.BAL.MobileAppAlert.SubscribeFCMUser : IMEI : {1}, GCMId : {2},subsmasterId : {3} ", HttpContext.Current.Request.ServerVariables["URL"], appInput.Imei, appInput.GcmId, appInput.SubsMasterId));

            }

            return isSuccess;
        }


        /// <summary>
        /// Created By : Sushil Kumar on 12nd Dec 2016
        /// Description : To unsubscribe fcm user
        /// </summary>
        /// <param name="appInput"></param>
        /// <returns></returns>
        public bool UnSubscribeFCMUser(AppFCMInput appInput)
        {
            string msg = string.Empty, subscriptionTopic = string.Empty;
            bool isSuccess = false;

            try
            {
                subscriptionTopic = SubscriptionTypes.GetSubscriptionType(2);  //for news we use defailt value 0 or 2

                SubscriptionRequest subscriptionRequest = new SubscriptionRequest() { To = subscriptionTopic, RegistrationTokens = new List<string> { appInput.GcmId } };
                string payload = JsonConvert.SerializeObject(subscriptionRequest);

                SubscriptionResponse subscriptionResponse = SubscribeFCMNotification(Bikewale.Utility.BWConfiguration.Instance.FCMUnSusbscribeUserUrl, payload, 0);
                if (subscriptionResponse != null)
                {
                    var result = subscriptionResponse.Results[0];
                    if (!string.IsNullOrEmpty(result.Error))
                    {
                        msg = string.Format("Android : UnSubscribing failed for Registration id - {0} : {1} due to {2}", appInput.Imei, appInput.GcmId, result.Error);
                        ErrorClass.LogError(new Exception(), string.Format("{0} - SubscribeUser : IMEI : {1}, GCMId : {2}, Message : {3} ", HttpContext.Current.Request.ServerVariables["URL"], appInput.Imei, appInput.GcmId, msg));
                        
                    }
                    else
                    {
                        isSuccess = true;
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("{0} - Bikewale.BAL.MobileAppAlert.UnSubscribeFCMUser : IMEI : {1}, GCMId : {2} ", HttpContext.Current.Request.ServerVariables["URL"], appInput.Imei, appInput.GcmId));
            }

            return isSuccess;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12nd Dec 2016
        /// Description : To subscribe/unsubscribe for fcm notification
        /// </summary>
        /// <param name="action"></param>
        /// <param name="payload"></param>
        /// <param name="retries"></param>
        /// <returns></returns>
        private SubscriptionResponse SubscribeFCMNotification(string action, string payload, int retries)
        {
            try
            {
                if (_maxRetries > retries)
                {

                    WebRequest tRequest = WebRequest.Create(action);
                    tRequest.Method = "POST";
                    tRequest.ContentType = "application/json";
                    tRequest.Headers.Add(string.Format("Authorization: key={0}", Bikewale.Utility.BWConfiguration.Instance.FCMApiKey));

                    Byte[] byteArray = Encoding.UTF8.GetBytes(payload);

                    tRequest.ContentLength = byteArray.Length;
                    String sResponseFromServer = string.Empty;
                    using (Stream dataStream = tRequest.GetRequestStream())
                    {
                        dataStream.Write(byteArray, 0, byteArray.Length);

                        using (HttpWebResponse tResponse = (HttpWebResponse)tRequest.GetResponse())
                        {

                            using (Stream dataStreamResponse = tResponse.GetResponseStream())
                            {
                                using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {

                                    if (tResponse.StatusCode.Equals(HttpStatusCode.OK))
                                    {
                                        sResponseFromServer = tReader.ReadToEnd();                                        
                                    }
                                    else if (tResponse.StatusCode.Equals(HttpStatusCode.ServiceUnavailable))
                                    {
                                        System.Threading.Thread.Sleep(TimeSpan.FromSeconds(5)); //hard coded 30 seconds | should be exponential backoff
                                        //Prasad: Changed the value to 5 seconds
                                        return SubscribeFCMNotification(action, payload, retries + 1);
                                    }
                                } 
                            }
                        }
                    }
                    //moved the task out of stream
                    if(!string.IsNullOrEmpty(sResponseFromServer))
                        return JsonConvert.DeserializeObject<SubscriptionResponse>(sResponseFromServer);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("{0} - Bikewale.BAL.MobileAppAlert.SubscribeFCMNotification, action : {1}, payload : {2}, retries : {3}", HttpContext.Current.Request.ServerVariables["URL"], action, payload, retries));
            }

            return null;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12nd Dec 2016
        /// Description : To send fcm notification
        /// Modified By : Sushil Kumar on 23rd Jan 2016
        /// Description : Added functionality to log mobile notifications and send notifications based on alertType id
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public bool SendFCMNotification(MobilePushNotificationData payload)
        {
            bool isSuccess = false;
            try
            {
                string subscriptionTopic = SubscriptionTypes.GetSubscriptionType(2); //Convert.ToUInt16(payload.AlertTypeId)
                NotificationBase androidPayload = new NotificationBase() { To = subscriptionTopic, Data = payload, TimeToLive = _oneWeek };

                WebRequest tRequest = WebRequest.Create(Bikewale.Utility.BWConfiguration.Instance.FCMSendURL);
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", Bikewale.Utility.BWConfiguration.Instance.FCMApiKey));

                string json = JsonConvert.SerializeObject(androidPayload);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);

                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (HttpWebResponse tResponse = (HttpWebResponse)tRequest.GetResponse())
                    {
                        if (tResponse.StatusCode.Equals(HttpStatusCode.OK))
                        {
                            using (Stream dataStreamResponse = tResponse.GetResponseStream())
                            {

                                using (StreamReader tReader = new StreamReader(dataStreamResponse))
                                {
                                    String sResponseFromServer = tReader.ReadToEnd();
                                    var result = JsonConvert.DeserializeObject<NotificationResponse>(sResponseFromServer);
                                    if (result != null && string.IsNullOrEmpty(result.Error))
                                    {
                                        isSuccess = true;
                                    }
                                    _appAlertRepo.CompleteNotificationProcess(payload, result);
                                } 
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, string.Format("{0} - Bikewale.BAL.MobileAppAlert.SendFCMNotification, payload : {1}", HttpContext.Current.Request.ServerVariables["URL"], payload));
            }

            return isSuccess;
        }
    }

}
