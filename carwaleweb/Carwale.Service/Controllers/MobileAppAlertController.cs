using System;
using Microsoft.Practices.Unity;
using Carwale.Entity.MobileAppAlerts;
using Carwale.Interfaces;
using System.Web.Http;
using System.Collections.Specialized;
using System.Web;
using System.Configuration;
using Carwale.Service.Filters;
using RabbitMqPublishing;


namespace Carwale.Service.Controllers
{
    public class MobileAppAlertController : ApiController
    {

        private static readonly string _mobileSubscribeQueueName = ConfigurationManager.AppSettings["MobileSubscribeQueuename"] ?? string.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        [ApiAuthorization]
        public bool PostInitAlert()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);
            var alrtMsg = new MobileAppNotifications()
            {
                title = string.IsNullOrEmpty(nvc["title"]) ? "" : nvc["title"],
                detailUrl = string.IsNullOrEmpty(nvc["detailUrl"]) ? "" : nvc["detailUrl"],
                smallPicUrl = string.IsNullOrEmpty(nvc["smallPicUrl"]) ? "" : nvc["smallPicUrl"],
                alertTypeId = string.IsNullOrEmpty(nvc["alertTypeId"]) ? -1 : int.Parse(nvc["alertTypeId"]),
                alertId = string.IsNullOrEmpty(nvc["alertId"]) ? -1 : int.Parse(nvc["alertId"]),
                isFeatured = string.IsNullOrEmpty(nvc["isFeatured"]) ? false : bool.Parse(nvc["isFeatured"]),
                largePicUrl = string.IsNullOrEmpty(nvc["largePicUrl"]) ? "" : nvc["largePicUrl"],
                publishDate = DateTime.Now.ToString("yyyyMMdd")
            };
            if (alrtMsg.alertTypeId == 2)
            {
                RabbitMqPublish publish = new RabbitMqPublish();
                publish.PublishAnyObjectToQueue<MobileAppNotifications>(ConfigurationManager.AppSettings["MobileAlertQueuename"].ToString(), alrtMsg);
            }
            return true;
        }

        ///// <summary>
        ///// Api controller to process mobile app alerts 
        ///// </summary>
        ///// <returns></returns>

        /// <summary>
        /// Api controller to manage alert subscriptions from mobile Apps
        /// </summary>
        /// <returns></returns>

        public bool GetManageMobileAppAlerts()
        {
            NameValueCollection nvc = HttpUtility.ParseQueryString(Request.RequestUri.Query);

            var userInfo = new MobileAppNotificationRegistration()
            {
                IMEI = string.IsNullOrEmpty(nvc["imei"]) ? "" : nvc["imei"],
                Name = string.IsNullOrEmpty(nvc["name"]) ? "" : nvc["name"],
                EmailId = string.IsNullOrEmpty(nvc["emailid"]) ? "" : nvc["emailid"],
                ContactNo = string.IsNullOrEmpty(nvc["contactno"]) ? "" : nvc["contactno"],
                OsType = string.IsNullOrEmpty(nvc["os"]) ? -1 : int.Parse(nvc["os"]),
                GCMId = string.IsNullOrEmpty(nvc["gcmid"]) ? "" : nvc["gcmid"],
                SubsMasterId = string.IsNullOrEmpty(nvc["subsmasterid"]) ? "" : nvc["subsmasterid"],
            };
            RabbitMqPublish publish = new RabbitMqPublish();
            publish.PublishAnyObjectToQueue<MobileAppNotificationRegistration>(_mobileSubscribeQueueName, userInfo);
            return true;
        }

    }
}