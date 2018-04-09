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
        
        public class JSException: Exception
        {
            public JSException(string message) : base(message) { }
        }

        public IHttpActionResult Post([FromBody] JSExceptionEntity error)
        {
            try
            {
                string errorString = string.Format("\nMessage - {0}\nErrorType - {1}\nSourceFile - {2}\nLine - {3}\nTrace - {4}", error.Message, error.ErrorType, error.SourceFile, error.LineNo, error.Trace);

                if (error != null)
                {
                    ErrorClass.LogError(new JSException(errorString), "Client Side Error");
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
