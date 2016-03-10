using Bikewale.Notifications;
using Bikewale.UI.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
        public IHttpActionResult Post([FromBody] JSExceptionEntity error)
        {
            try
            {
                if(error != null)
                {
                    string emailTo = Bikewale.Utility.BWConfiguration.Instance.ErrorMailTo;
                    string subject = "Javascript Error in " + Bikewale.Utility.BWConfiguration.Instance.ApplicationName + " at page: " + HttpContext.Current.Request.ServerVariables["HTTP_REFERER"]; 
                    ComposeEmailBase mail = new JSExceptionTemplate(error);  
                    mail.Send(emailTo, subject);
                }
                else
                {
                    return Ok(false);
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Controllers.JSException.JSExceptionController.Post");
                objErr.SendMail();
                return InternalServerError();
            }
            return Ok(true);
        }
    }
}
