using Bikewale.DAL.Feedback;
using Bikewale.Interfaces.Feedback;
using Bikewale.Notifications;
using Microsoft.Practices.Unity;
using System;
using System.Collections.Generic;
using System.Linq;
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
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Version.VersionController");
                objErr.SendMail();
                return InternalServerError();
            }

            return (IHttpActionResult)Request.CreateResponse(HttpStatusCode.NotModified, "Oops ! Something Went Wrong");
        }//post completed
    }
}
