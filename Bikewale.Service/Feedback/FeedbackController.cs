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
    /// Feedback Submission
    /// Author : Sushil Kumar
    /// Created On : 24th August 2015
    /// </summary>
    public class FeedbackController : ApiController
    {

        /// <summary>
        /// Save User FeedBack
        /// </summary>
        /// <param name="feedbackComment"></param>
        /// <param name="feedbackType"></param>
        /// <param name="platformId"></param>
        /// <param name="pageUrl"></param>
        /// <returns>True/False</returns>
        [ResponseType(typeof(Boolean))]
        public HttpResponseMessage POST(string feedbackComment, ushort feedbackType, ushort platformId, string pageUrl)
        {
            bool isSaved = false;
            try
            {
                using (IUnityContainer container = new UnityContainer())
                {
                    IFeedback userFeedback = null;

                    container.RegisterType<IFeedback, FeedbackRepository>();
                    userFeedback = container.Resolve<IFeedback>();

                    isSaved = userFeedback.SaveCustomerFeedback(feedbackComment, feedbackType, platformId, pageUrl);

                    if (isSaved)
                    {
                        return Request.CreateResponse(HttpStatusCode.OK, isSaved);
                    }
                    else
                    {
                        return Request.CreateResponse(HttpStatusCode.NotModified, "Oops ! Something Went Wrong");
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorClass objErr = new ErrorClass(ex, "Exception : Bikewale.Service.Version.VersionController");
                objErr.SendMail();
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "OOps ! Some error occured.");
            }
        }//post completed
    }
}
