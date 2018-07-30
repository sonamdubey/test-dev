using Bikewale.Entities.MobileAppAlert;
using Bikewale.Interfaces.MobileAppAlert;
using Bikewale.Notifications;
using System;
using System.Collections.Specialized;
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
                if (nvc != null)
                {
                    MobilePushNotificationData data = new MobilePushNotificationData();
                    data.Title = nvc["title"];
                    data.DetailUrl = nvc["detailUrl"];
                    data.SmallPicUrl = nvc["smallPicUrl"];
                    data.LargePicUrl = nvc["largePicUrl"];
                    data.AlertId = Convert.ToInt32(nvc["alertId"]);
                    data.AlertTypeId = Convert.ToInt32(nvc["alertTypeId"]);
                    data.IsFeatured = Convert.ToBoolean(nvc["isFeatured"]);
                    data.PublishDate = DateTime.Now.ToString("yyyyMMdd");

                    if (_objMobileAppAlert.SendFCMNotification(data))
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
                    return BadRequest();
                }



            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.MobileAppAlerts.PushMobileAppAlertController : " + Request.RequestUri.Query);
               
                return InternalServerError();
            }

        }


    }
}
