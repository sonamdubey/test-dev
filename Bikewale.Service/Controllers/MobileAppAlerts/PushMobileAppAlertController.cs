using Bikewale.Entities.MobileAppAlert;
using Bikewale.Interfaces.MobileAppAlert;
using Bikewale.Notifications;
using Newtonsoft.Json;
using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.MobileAppAlerts
{
    /// <summary>
    /// Created By : Sushil Kumar 
    /// Created On : 5th Deccember 2015 
    /// To push mobile alerts for the app for various featured articles
    /// </summary>
    public class PushMobileAppAlertController : ApiController
    {

        private static readonly string _androidGlobalTopic = Bikewale.Utility.BWConfiguration.Instance.AndroidGlobalTopic;
        private static readonly string _FCMSendURL = Bikewale.Utility.BWConfiguration.Instance.FCMSendURL;
        private static readonly string _FCMApiKey = Bikewale.Utility.BWConfiguration.Instance.FCMApiKey;
        private static readonly int _oneWeek = 604800;
        private readonly IMobileAppAlert _objMobileAppAlert = null;

        public PushMobileAppAlertController(IMobileAppAlert ObjMobileAppAlert)
        {
            _objMobileAppAlert = ObjMobileAppAlert;
        }

        /// <summary>
        /// Created By : Sushil Kumar
        /// Created On : 5th December 2015
        /// Description : To push mobile notification to rabbitmq for processing queue
        /// Modified by :   Sumit Kate on 04 Feb 2016
        /// Description :   Send the alerts for all notification
        /// </summary>
        /// <returns></returns>
        [ResponseType(typeof(Boolean))]
        public IHttpActionResult POST()
        {
            try
            {

                NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

                MobilePushNotificationData data = new MobilePushNotificationData();
                data.Title = nvc["title"];
                data.DetailUrl = nvc["detailUrl"];
                data.SmallPicUrl = nvc["smallPicUrl"];
                data.LargePicUrl = nvc["largePicUrl"];
                data.AlertId = Convert.ToInt32(nvc["alertId"]);
                data.AlertTypeId = Convert.ToInt32(nvc["alertTypeId"]);
                data.IsFeatured = Convert.ToBoolean(nvc["isFeatured"]);
                data.PublishDate = DateTime.Now.ToString("yyyyMMdd");

                NotificationBase androidPayload = new NotificationBase() { To = "/topics/" + _androidGlobalTopic, Data = data, TimeToLive = _oneWeek };
                FCMPushNotificationStatus androidMessageResponse = SendFCMNotification(androidPayload);
                _objMobileAppAlert.CompleteNotificationProcess(data.AlertTypeId);
                return Ok(true);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.MobileAppAlerts.PushMobileAppAlertController");
                objErr.SendMail();
                return InternalServerError();
            }

        }

        private FCMPushNotificationStatus SendFCMNotification(NotificationBase payload)
        {
            FCMPushNotificationStatus result = new FCMPushNotificationStatus();
            try
            {
                result.Successful = false;
                result.Error = null;

                WebRequest tRequest = WebRequest.Create(_FCMSendURL);
                tRequest.Method = "POST";
                tRequest.ContentType = "application/json";
                tRequest.Headers.Add(string.Format("Authorization: key={0}", _FCMApiKey));


                var json = JsonConvert.SerializeObject(payload);

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
                                result.Response = JsonConvert.DeserializeObject<NotificationResponse>(sResponseFromServer);
                                if (result.Response.Error == null)
                                {
                                    result.Successful = true;
                                }
                                else
                                {
                                    throw new Exception(result.Response.Error);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Successful = false;
                result.Response = null;
                result.Error = ex;
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " - SendFCMNotification");
                objErr.SendMail();
            }

            return result;
        }



    }
}
