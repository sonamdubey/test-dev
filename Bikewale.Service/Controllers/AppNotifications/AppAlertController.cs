using Bikewale.DTO.App.AppAlert;
using Bikewale.Entities.MobileAppAlert;
using Bikewale.Interfaces.MobileAppAlert;
using Bikewale.Notifications;
using Bikewale.Utility.AndroidAppAlert;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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


        private static readonly string _batchAddEndPoint = Bikewale.Utility.BWConfiguration.Instance.SusbscribeFCMUserUrl;
        private static readonly string _batchRemoveEndPoint = Bikewale.Utility.BWConfiguration.Instance.UnSusbscribeFCMUserUrl;

        private readonly IMobileAppAlert _appAlert = null;
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="appAlert"></param>
        public AppAlertController(IMobileAppAlert appAlert)
        {
            _appAlert = appAlert;
        }
        /// <summary>
        /// Verified the resend mobile verification code
        /// </summary>
        /// <param name="input">entity</param>
        /// <returns></returns>
        [ResponseType(typeof(bool))]
        public IHttpActionResult Post([FromBody]AppIMEIDetailsInput input)
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
                        isSuccess = UnSubscribeUser(input);
                    }

                    if (isSuccess)
                    {
                        return Ok(true);
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                else
                {
                    return Unauthorized();
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("{0} - SubscribeUser : IMEI : {1}, GCMId : {2} ", HttpContext.Current.Request.ServerVariables["URL"], input.Imei, input.GcmId));
                objErr.SendMail();
                return InternalServerError();
            }

        }

        /// <summary>
        /// Created By : Sushil Kumar on 12nd Dec 2016
        /// Description : To subscribe fcm user
        /// </summary>
        /// <param name="appInput"></param>
        /// <returns></returns>
        private bool SubscribeUser(AppIMEIDetailsInput appInput)
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

                    SubscriptionResponse subscriptionResponse = _appAlert.SubscribeFCMNotification(_batchAddEndPoint, payload, 0);
                    if (subscriptionResponse != null)
                    {
                        var result = subscriptionResponse.Results[0];
                        if (!string.IsNullOrEmpty(result.Error))
                        {
                            msg = string.Format("Android : Subscribing failed for Registration id - {0} : {1} due to {2}", appInput.Imei, appInput.GcmId, result.Error);
                            ErrorClass objErr = new ErrorClass(new Exception(), string.Format("{0} - SubscribeUser : IMEI : {1}, GCMId : {2}, Message : {3} ", HttpContext.Current.Request.ServerVariables["URL"], appInput.Imei, appInput.GcmId, msg));
                            objErr.SendMail();
                        }
                        else
                        {
                            isSuccess = _appAlert.SaveIMEIFCMData(appInput.Imei, appInput.GcmId, appInput.OsType, appInput.SubsMasterId);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("{0} - SubscribeUser : IMEI : {1}, GCMId : {2} ", HttpContext.Current.Request.ServerVariables["URL"], appInput.Imei, appInput.GcmId));
                objErr.SendMail();

            }

            return isSuccess;

        }

        /// <summary>
        /// Created By : Sushil Kumar on 12nd Dec 2016
        /// Description : To unsubscribe fcm user
        /// </summary>
        /// <param name="appInput"></param>
        /// <returns></returns>
        private bool UnSubscribeUser(AppIMEIDetailsInput appInput)
        {
            string msg = string.Empty, subscriptionTopic = string.Empty;
            bool isSuccess = false;

            try
            {
                subscriptionTopic = SubscriptionTypes.GetSubscriptionType(0);

                SubscriptionRequest subscriptionRequest = new SubscriptionRequest() { To = subscriptionTopic, RegistrationTokens = new List<string>() { appInput.GcmId } };
                string payload = JsonConvert.SerializeObject(subscriptionRequest);

                SubscriptionResponse subscriptionResponse = _appAlert.SubscribeFCMNotification(_batchRemoveEndPoint, payload, 0);
                if (subscriptionResponse != null)
                {
                    var result = subscriptionResponse.Results[0];
                    if (!string.IsNullOrEmpty(result.Error))
                    {
                        msg = string.Format("Android : UnSubscribing failed for Registration id - {0} : {1} due to {2}", appInput.Imei, appInput.GcmId, result.Error);
                        ErrorClass objErr = new ErrorClass(new Exception(), string.Format("{0} - SubscribeUser : IMEI : {1}, GCMId : {2}, Message : {3} ", HttpContext.Current.Request.ServerVariables["URL"], appInput.Imei, appInput.GcmId, msg));
                        objErr.SendMail();
                    }
                    else
                    {
                        isSuccess = true;
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, string.Format("{0} - UnSubscribing : IMEI : {1}, GCMId : {2} ", HttpContext.Current.Request.ServerVariables["URL"], appInput.Imei, appInput.GcmId));
                objErr.SendMail();
            }

            return isSuccess;
        }

    }

}