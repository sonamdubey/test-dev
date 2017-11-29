using Bikewale.Entities.JSErrorLog;
using Bikewale.Notifications;
using System;
using System.Web;
using System.Web.Http;

namespace Bikewale.Service.Controllers.JSException
{
    /// <summary>
    /// Craeted By  : Sushil Kumar
    /// Created On  : 10th March 2016
    /// Summary     :  To log Javascript error/exception to bikewalebugs@gmail.com
    /// </summary>
    public class JSExceptionController : ApiController
    {

        /// <summary>
        /// Author : Sushil Kumar
        /// Created On : 10th March 2016
        /// Description : Post method used for logging javascript exception/error to bikewalebugs@gmail.com
        /// </summary>
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
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Controllers.JSException.JSExceptionController.Post");
               
                return InternalServerError();
            }
            return Ok();
        }
    }
}
