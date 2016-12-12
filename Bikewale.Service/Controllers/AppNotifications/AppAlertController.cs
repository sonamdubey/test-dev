using Bikewale.DTO.App.AppAlert;
using Bikewale.Interfaces.AppAlert;
using Bikewale.Notifications;
using Bikewale.Utility.AndroidAppAlert;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Controllers.AppNotifications
{

    /// <summary>
    /// Author : Sangran Nandkhile
    /// Created On : 6th Jan 2016
    /// Api url to Test: http://localhost:9011/api/AppAlert
    /// Raw body: {"imei":"111111111","gcmId":"22222222","osType":"0","subsMasterId":"1"}
    /// </summary>
    public class AppAlertController : ApiController
    {

        private static readonly string _FCMApiKey = Bikewale.Utility.BWConfiguration.Instance.FCMApiKey;
        private static readonly string _batchImportEndPoint = System.Configuration.ConfigurationManager.AppSettings["IIDBatchImportEndPoint"];
        private static readonly string _batchAddEndPoint = System.Configuration.ConfigurationManager.AppSettings["IIDBatchAddEndPoint"];
        private static readonly string _batchRemoveEndPoint = System.Configuration.ConfigurationManager.AppSettings["IIDBatchRemoveEndPoint"];
        private static readonly string _androidGlobalTopic = Bikewale.Utility.BWConfiguration.Instance.AndroidGlobalTopic;
        private enum SubscriptionToggle { AndroidSubscribe, AndroidUnSubscribe };
        private static readonly int _maxRetries = 3;

        private readonly IAppAlert _appAlert = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appAlert"></param>
        public AppAlertController(IAppAlert appAlert)
        {
            _appAlert = appAlert;
        }
        /// <summary>
        /// Verified the resend mobile verification code
        /// </summary>
        /// <param name="input">entity</param>
        /// <returns></returns>
        [ResponseType(typeof(bool))]
        public IHttpActionResult Post([FromBody]AppImeiDetailsInput input)
        {
            bool isSuccess = false;
            string msg = string.Empty;
            try
            {
                if (input != null && !String.IsNullOrEmpty(input.Imei) && !String.IsNullOrEmpty(input.GcmId) && !String.IsNullOrEmpty(input.OsType))
                {

                    if (!string.IsNullOrEmpty(input.SubsMasterId))
                    {
                        isSuccess = SubscribeUser(input);
                    }
                    else
                    {
                        //isSuccess = UnSubscribeUser(input);
                    }

                    if (isSuccess)
                    {
                        return Ok(true);
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.AppNotifications.Post");
                objErr.SendMail();
                return InternalServerError();
            }

        }

        private bool SubscribeUser(AppImeiDetailsInput appInput)
        {
            string msg = string.Empty, subscriptionTopic = string.Empty;
            ushort subscriptionId; bool isSuccess = false;
            try
            {
                if (ushort.TryParse(appInput.SubsMasterId, out subscriptionId))
                {
                    subscriptionTopic = SubscriptionTypes.GetSubscriptionType(subscriptionId);
                    SubscriptionRequest subscriptionRequest = new SubscriptionRequest() { To = subscriptionTopic, RegistrationTokens = new List<string>() { appInput.GcmId } };
                    string payload = JsonConvert.SerializeObject(subscriptionRequest);

                    SubscriptionResponse subscriptionResponse = SubscribeFCMNotification(_batchAddEndPoint, payload, 0);
                    if (subscriptionResponse != null)
                    {
                        var result = subscriptionResponse.Results[0];
                        if (!string.IsNullOrEmpty(result.Error))
                        {
                            msg = string.Format("Android : Subscribing failed for Registration id - {0} : {1} due to {2}", appInput.Imei, appInput.GcmId, result.Error);
                        }
                        else
                        {
                            isSuccess = _appAlert.SaveImeiGcmData(appInput.Imei, appInput.GcmId, appInput.OsType, appInput.SubsMasterId);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " - SubscribeUser");
                objErr.SendMail();

            }

            return isSuccess;

        }

        //private bool UnSubscribeUser()
        //{
        //    string msg = string.Empty;
        //    //unsubscribeToken
        //    SubscriptionToggle toggle = SubscriptionToggle.AndroidUnSubscribe;
        //    SubscriptionResponse subscriptionResponse = SendSubscriptionRequest(input.GcmId, "/topics/" + _androidGlobalTopic, toggle);
        //    if (subscriptionResponse != null)
        //    {
        //        var result = subscriptionResponse.Results[0];
        //        if (!string.IsNullOrEmpty(result.Error))
        //        {
        //            msg = string.Format("Android : Unsubscribing failed for Registration id - {0} : {1} due to {2}", input.Imei, input.GcmId, result.Error);
        //        }
        //        else
        //        {
        //            isSuccess = true;
        //        }
        //    }
        //}


        private SubscriptionResponse SubscribeFCMNotification(string action, string payload, int retries)
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


                    var json = JsonConvert.SerializeObject(payload);

                    Byte[] byteArray = Encoding.UTF8.GetBytes(json);

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
                ErrorClass objErr = new ErrorClass(ex, HttpContext.Current.Request.ServerVariables["URL"] + " - SendFCMNotification");
                objErr.SendMail();
            }

            return subsResponse;
        }

    }

    [Serializable]
    public class MapResponse
    {
        [JsonProperty("results")]
        public List<Mapping> Results { get; set; }
    }

    [Serializable]
    public class Mapping
    {
        [JsonProperty("apns_token")]
        public string ApnsToken { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("registration_token")]
        public string RegistrationToken { get; set; }
    }

    [Serializable]
    public class SubscriptionRequest
    {
        [JsonProperty("to")]
        public string To { get; set; }

        [JsonProperty("registration_tokens")]
        public List<string> RegistrationTokens { get; set; }
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