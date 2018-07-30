using Bikewale.Interfaces.Feedback;
using Bikewale.Notifications;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace Bikewale.Service.Feedback
{
    /// <summary>
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// Summary : Feedback for the website   
    /// </summary>
    public class FeedbackController : ApiController
    {

        private readonly IFeedback _feedback = null;
        public FeedbackController(IFeedback feedback)
        {
            _feedback = feedback;
        }

        /// <summary>
        /// Save User FeedBack
        /// </summary>
        /// <param name="feedbackComment"></param>
        /// <param name="feedbackType"></param>
        /// <param name="platformId"></param>
        /// <param name="pageUrl"></param>
        /// <returns>True/False</returns>
        [ResponseType(typeof(Boolean))]
        public IHttpActionResult POST(string feedbackComment, ushort feedbackType, ushort platformId, string pageUrl)
        {
            bool isSaved = false;
            try
            {

                isSaved = _feedback.SaveCustomerFeedback(feedbackComment, feedbackType, platformId, pageUrl);

                if (isSaved)
                {
                    return Ok(isSaved);
                }
            }
            catch (Exception ex)
            {
                ErrorClass.LogError(ex, "Exception : Bikewale.Service.Version.VersionController");
               
                return InternalServerError();
            }

            return (IHttpActionResult)Request.CreateResponse(HttpStatusCode.NotModified, "Oops ! Something Went Wrong");
        }//post completed
    }
}
