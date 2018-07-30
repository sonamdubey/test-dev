using Bikewale.DAL.BikeBooking;
using Bikewale.Entities.BikeBooking;
using Bikewale.Notifications;
using Bikewale.Notifications.MailTemplates;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Bikewale.BindViewModels.Webforms
{
    public class FeedbackCancellationModel
    {
        public void ProcessFeedbackEmail(string bwId, string feedbackText)
        {
            try
            {
                FeedBackEntity feedBkEntity = new FeedBackEntity()
                {
                    BwId = bwId,
                    FeedBack = feedbackText
                };
                FeedBacksRespositry feedback = new FeedBacksRespositry();
                feedback.SaveFeedBack(feedBkEntity);
                // send emails
                if (!String.IsNullOrEmpty(feedbackText) && (!string.IsNullOrEmpty(bwId)))
                {
                    string[] emailAddress = System.Configuration.ConfigurationManager.AppSettings["CancellationFeedbackEmail"].Split(',');
                    ComposeEmailBase feedBackMail = new CancellationFeedbackTemplate(bwId, feedbackText);
                    feedBackMail.Send(Bikewale.Utility.BWConfiguration.Instance.LocalMail, "Feedback from a customer who has cancelled booking - " + bwId, "", emailAddress, null);
                }
            }
            catch (Exception ex)
            {
                Bikewale.Notifications.ErrorClass.LogError(ex, "FeedbackCancellationModel.ProcessFeedbackEmail");
                
            }
        }
    }
}