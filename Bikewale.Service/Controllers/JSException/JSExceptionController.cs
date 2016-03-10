using Bikewale.Notifications;
using System;
using System.Web;
using System.Web.Http;
using Bikewale.Entities.JSErrorLog;

namespace Bikewale.Service.Controllers.JSException
{
    /// <summary>
    /// Craeted By  : Sushil Kumar
    /// Created On  : 10th March 2016
    /// Summary     :  To log Javascript error/exception to bikewalebugs@gmail.com
    /// </summary>
    public class JSExceptionController : ApiController
    {
        public IHttpActionResult Post([FromBody] JSExceptionEntity error)
        {
            try
            {
                if (error != null)
                {
                    string emailTo = Bikewale.Utility.BWConfiguration.Instance.ErrorMailTo;
                    string subject = String.Format("Javascript Error in {0} at page: {1}", Bikewale.Utility.BWConfiguration.Instance.ApplicationName, HttpContext.Current.Request.ServerVariables["HTTP_REFERER"]);
                    ComposeEmailBase mail = new JSExceptionTemplate(error);
                    mail.Send(emailTo, subject);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.JSException.JSExceptionController.Post");
                objErr.SendMail();
                return InternalServerError();
            }
            return Ok();
        }
    }
}
