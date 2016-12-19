using AppNotification.BAL;
using AppNotification.DAL;
using AppNotification.Interfaces;
using AppNotification.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Web.Http;
using System.Web.Http.Description;


namespace AppNotification.Service.Controllers
{
    public class MobileAppAlertController : ApiController
    {
        private IRequestManager _queueProcessor;

        // POST api/<controller>
        [ResponseType(typeof(IHttpActionResult))]
        [HttpPost]
        public IHttpActionResult Post()
        {
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {

                    container.RegisterType<IRequestManager, MobileAppAlertService>()
                        .RegisterType<IMobileAppAlertRepository, MobileAppAlertRepository>();
                    _queueProcessor = container.Resolve<IRequestManager>();
                    _queueProcessor.ProcessRequest();
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