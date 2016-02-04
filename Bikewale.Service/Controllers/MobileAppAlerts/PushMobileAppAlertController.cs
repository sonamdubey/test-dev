using Bikewale.Entities.MobileAppAlerts;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using RabbitMqPublishing;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
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
                nvc.Add("publishDate", DateTime.Now.ToString("yyyyMMdd"));


                RabbitMqPublish publish = new RabbitMqPublish();
                publish.PublishToQueue(ConfigurationManager.AppSettings["MobileAlertQueuename"].ToString(), nvc);

                return Ok(true);
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.MobileAppAlerts.PushMobileAppAlertController");
                objErr.SendMail();
                return InternalServerError();
            }

        }
    }
}
