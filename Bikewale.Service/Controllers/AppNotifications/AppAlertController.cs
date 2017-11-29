using Bikewale.DTO.App.AppAlert;
using Bikewale.Entities.MobileAppAlert;
using Bikewale.Interfaces.MobileAppAlert;
using Bikewale.Notifications;
using Bikewale.Service.AutoMappers.MobileAppAlert;
using System;
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
                if (ModelState.IsValid)
                {
                    AppFCMInput requestEntity = FCMNotificationMapper.Convert(input);

                    if (!string.IsNullOrEmpty(input.SubsMasterId))
                    {

                        isSuccess = _appAlert.SubscribeFCMUser(requestEntity);
                    }
                    else
                    {
                        isSuccess = _appAlert.UnSubscribeFCMUser(requestEntity);

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
                ErrorClass.LogError(ex, string.Format("{0} - Bikewale.Service.Controllers.AppAlertController.AppNotifications.Post : IMEI : {1}, GCMId : {2} ", HttpContext.Current.Request.ServerVariables["URL"], input.Imei, input.GcmId));
               
                return InternalServerError();
            }

        }


    }

}