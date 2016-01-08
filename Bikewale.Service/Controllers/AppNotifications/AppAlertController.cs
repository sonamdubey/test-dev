using Bikewale.DTO.App.AppAlert;
using Bikewale.DTO.PriceQuote.MobileVerification;
using Bikewale.Entities.MobileVerification;
using Bikewale.Interfaces.AppAlert;
using Bikewale.Interfaces.MobileVerification;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
            try
            {
                if (input != null && !String.IsNullOrEmpty(input.Imei) && !String.IsNullOrEmpty(input.GcmId) && !String.IsNullOrEmpty(input.OsType))
                {
                    isSuccess = _appAlert.SaveImeiGcmData(input.Imei, input.GcmId, input.OsType, input.SubsMasterId);
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
    }
}