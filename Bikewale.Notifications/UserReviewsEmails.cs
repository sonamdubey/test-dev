using Bikewale.Notifications.MailTemplates.UserReviews;

namespace Bikewale.Notifications
{
    /// <summary>
    /// Created by : Aditi Srivastava on 14 Apr 2017
    /// Summary    : Mail notification functions for user reviews
    /// </summary>
    public class UserReviewsEmails
    {
        /// <summary>
        /// Send email on submission of rating
        /// </summary>
        public static void SendRatingSubmissionEmail(string userName, string userEmail, string makeName, string modelName)
        {
            ComposeEmailBase objEmail = new RatingSubmissionEmail(userName, makeName, modelName);
            objEmail.Send(userEmail, string.Format("Thank you for rating {0} {1}", makeName, modelName));
        }

        /// <summary>
        /// Send email on submission of whole user review
        /// </summary>
        public static void SendReviewSubmissionEmail(string userName, string userEmail, string makeName, string modelName)
        {
            ComposeEmailBase objEmail = new ReviewSubmissionEmail(userName, makeName, modelName);
            objEmail.Send(userEmail, string.Format("Thank you for sharing your {0} {1}’s experience!", makeName, modelName));
        }

        /// <summary>
        /// Send follow up email if user has not written a review after rating
        /// </summary>
        public static void SendReviewReminderEmail(string userName, string userEmail, string reviewLink, string ratingDate, string makeName, string modelName)
        {
            ComposeEmailBase objEmail = new ReviewReminderEmail(userName, reviewLink,ratingDate, makeName, modelName);
            objEmail.Send(userEmail, string.Format("Share your {0} {1}’s experience! ", makeName, modelName));
        }

        /// <summary>
        /// Send mail if review is approved from opr
        /// </summary>
        public static void SendReviewApprovalEmail(string userName, string userEmail, string reviewLink, string modelName)
        {
            ComposeEmailBase objEmail = new ReviewApprovalEmail(userName, reviewLink, modelName);
            objEmail.Send(userEmail, "Congratulations! Your review has been published");
        }

        /// <summary>
        /// Send mail if review is rejected from opr
        /// </summary>
        public static void SendReviewRejectionEmail(string userName, string userEmail, string modelName)
        {
            ComposeEmailBase objEmail = new ReviewRejectionEmail(userName, modelName);
            objEmail.Send(userEmail, "Oops! We request you to verify your review again");
        }

    }
}
