using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using AppNotification.Service;
using AppNotification.Notifications;
using AppNotification.Entity;
using AppNotification.Interfaces;
using AppNotification.DAL;
using Microsoft.Practices.Unity;
using AppNotification.BAL;


namespace AppNotification.Service.Controllers
{
    public class MobileAppAlertController : ApiController
    {
        private IRequestManager<MobileAppNotifications> _queueProcessor;

        // POST api/<controller>
        [ResponseType(typeof(IHttpActionResult))]
        [HttpPost]
        public IHttpActionResult Post([FromBody]MobileAppNotifications appNotification)
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {

                    container.RegisterType<IRequestManager<MobileAppNotifications>, MobileAppAlertService<MobileAppNotifications>>().RegisterType<IMobileAppAlertRepository, MobileAppAlertRepository>();
                    _queueProcessor = container.Resolve<IRequestManager<MobileAppNotifications>>();
                    _queueProcessor.ProcessRequest(appNotification);
                    return Ok(true);

                }
            }
            catch (Exception err)
            {
                ErrorClass objErr = new ErrorClass(err, "Exception : AppNotification.Service.Controllers.MobileAppAlertController.Post");
                objErr.SendMail();
                return InternalServerError();
            }
        }
    }
}