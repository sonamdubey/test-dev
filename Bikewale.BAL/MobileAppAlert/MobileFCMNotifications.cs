using Bikewale.Entities.MobileAppAlert;
using Bikewale.Interfaces.MobileAppAlert;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Bikewale.BAL.MobileAppAlert
{

    public class MobileFCMNotifications : IMobileAppAlert
    {
        private static readonly string _FCMApiKey = Bikewale.Utility.BWConfiguration.Instance.FCMApiKey;
        private static readonly string _FCMSendURL = Bikewale.Utility.BWConfiguration.Instance.FCMSendURL;
        private static readonly string _androidGlobalTopic = Bikewale.Utility.BWConfiguration.Instance.AndroidGlobalTopic;
        private static readonly int _oneWeek = 604800;
        private static readonly int _maxRetries = 3;
        private readonly IMobileAppAlert _appAlertRepo = null;

        /// <summary>
        /// Created By : Sushil Kumar on 12nd Dec 2016
        /// Description : To esolve unity container for dal methods
        /// </summary>
        public MobileFCMNotifications()
        {
            using (IUnityContainer container = new UnityContainer())
            {
                container.RegisterType<IMobileAppAlert, Bikewale.DAL.MobileAppAlert.MobileAppAlert>();
                _appAlertRepo = container.Resolve<Bikewale.DAL.MobileAppAlert.MobileAppAlert>();
            }
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12nd Dec 2016
        /// Description : To complete notification process
        /// </summary>
        /// <param name="alertid"></param>
        /// <returns></returns>
        public bool CompleteNotificationProcess(int alertid)
        {
            return _appAlertRepo.CompleteNotificationProcess(alertid);
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12nd Dec 2016
        /// Description : To subscribe/unsubscribe for fcm notification
        /// </summary>
        /// <param name="action"></param>
        /// <param name="payload"></param>
        /// <param name="retries"></param>
        /// <returns></returns>
        public SubscriptionResponse SubscribeFCMNotification(string action, string payload, int retries)
        {
            SubscriptionResponse subsResponse = null;
            try
            {
                if (_maxRetries > retries)
                {
                    subsResponse = new SubscriptionResponse();

                    WebRequest tRequest = WebRequest.Create(action);
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
                                        return SubscribeFCMNotification(action, payload, retries + 1);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("{0} - SubscribeFCMNotification, action : {1}, payload : {2}, retries : {3}", HttpContext.Current.Request.ServerVariables["URL"], action, payload, retries));
                objErr.SendMail();
            }

            return subsResponse;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12nd Dec 2016
        /// Description : To send fcm notification
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public bool SendFCMNotification(MobilePushNotificationData payload)
        {
            bool isSuccess = false;
            try
            {

                NotificationBase androidPayload = new NotificationBase() { To = "/topics/" + _androidGlobalTopic, Data = payload, TimeToLive = _oneWeek };

                WebRequest tRequest = WebRequest.Create(_FCMSendURL);
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", _FCMApiKey));

                string json = JsonConvert.SerializeObject(androidPayload);

                Byte[] byteArray = Encoding.UTF8.GetBytes(json);

                tRequest.ContentLength = byteArray.Length;

                using (Stream dataStream = tRequest.GetRequestStream())
                {
                    dataStream.Write(byteArray, 0, byteArray.Length);

                    using (WebResponse tResponse = tRequest.GetResponse())
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
                                    _appAlertRepo.CompleteNotificationProcess(payload.AlertTypeId);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("{0} - SendFCMNotification, payload : {2}", HttpContext.Current.Request.ServerVariables["URL"], payload));
                objErr.SendMail();
            }

            return isSuccess;
        }

        /// <summary>
        /// Created By : Sushil Kumar on 12nd Dec 2016
        /// Description : To save imei and fcm data to database
        /// </summary>
        /// <param name="imei"></param>
        /// <param name="gcmId"></param>
        /// <param name="osType"></param>
        /// <param name="subsMasterId"></param>
        /// <returns></returns>
        public bool SaveIMEIFCMData(string imei, string gcmId, string osType, string subsMasterId)
        {
            return _appAlertRepo.SaveIMEIFCMData(imei, gcmId, osType, subsMasterId);
        }
    }

}
