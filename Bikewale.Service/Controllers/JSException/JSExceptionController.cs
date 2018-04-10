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
        /// Modified by: Dhruv Joshi
        /// Dated: 10th April 2018
        /// Details: Details fetched from JSException's properties for logging
        /// </summary>
        
        public IHttpActionResult Post([FromBody] JSException error)
        {
            try
            {
                if (error != null)
                {
                    string errorString = string.Format("\nClient Side Error\nDetails - {0}\nErrorType - {1}\nSourceFile - {2}\nLine - {3}\nTrace - {4}", error.Details, error.ErrorType, error.SourceFile, error.LineNo, error.Trace);
                    ErrorClass.LogError(error, errorString);
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
