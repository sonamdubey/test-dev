using Carwale.Entity.Customers;
using Carwale.Interfaces.Users;
using Carwale.Notifications;
using System;
using System.Web;
using System.Web.Http;

namespace Carwale.Service.Controllers.Users
{
    public class FeedbackController : ApiController
    {
        private readonly IUserFeedbackBL _userFeedbackBL;

        public FeedbackController(IUserFeedbackBL userFeedbackBL)
        {
            _userFeedbackBL = userFeedbackBL;
        }

        [HttpPost]
        [Route("api/feedback/")]
        public IHttpActionResult Post([FromBody] UserFeedback feedback)
        {
            try
            {
                feedback.UserIp = HttpContext.Current.Request.ServerVariables["REMOTE_HOST"];
                feedback.FeedbackDateTime = DateTime.Now;
                _userFeedbackBL.ProcessFeedback(feedback);
                return Ok();
            }
            catch(Exception ex)
            {
                var objErr = new ExceptionHandler(ex, "ClassifiedController.GetResultsWithFiltersAndPager()");
                objErr.LogException();
                return InternalServerError(ex);
            }
        }
    }
}
