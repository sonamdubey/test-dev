using System.Text;

namespace Bikewale.Notifications.MailTemplates.UserReviews

{
    /// <summary>
    /// Created by  :  Aditi Srivastava on 14 Apr 2017
    /// Description :  User rating submission email to user
    /// </summary>
    public class RatingSubmissionEmail : ComposeEmailBase
    {
        private string userName,
            reviewLink,
            makeName,
            modelName;
        /// <summary>
        /// Initialize the member variables values
        /// </summary>
        public RatingSubmissionEmail(string userName, string reviewLink, string makeName, string modelName)
        {
            this.userName = userName;
            this.reviewLink = reviewLink;
            this.makeName = makeName;
            this.modelName = modelName;   
        }
        /// <summary>
        /// Created by  :   Aditi Srivastava on 14 Apr 2017
        /// Description :   Prepares the Email Body when user rating is submitted
        /// </summary>
        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<p>Dear {0},</p>",userName);
            sb.AppendFormat("<p>Thanks for posting your ratings for {0} {1} on BikeWale! ", makeName, modelName);
            sb.AppendFormat("Your rating has been recorded and will be available to BikeWale users very soon.</p>");
            sb.AppendFormat("<p>Every month, more than 6 million users research about bikes on BikeWale. The prospective ");
            sb.AppendFormat("buyers look out for user reviews to make their buying decision. We request you to write ");
            sb.AppendFormat("a detailed review about your {0} {1} bike. ", makeName, modelName);
            sb.AppendFormat("Your review will be of great help to someone who is planning to buy the same motorcycle.</p>");
            sb.AppendFormat("<p>To continue writing a review for {0} {1}, click here- {2}</p>", makeName, modelName, reviewLink);
            sb.AppendFormat("<p>Looking forward to your detailed review on {0}! In case you didn't rate this bike, ", modelName);
            sb.AppendFormat("please write to us at <a href='mailto:contact@bikewale.com'>contact@bikewale.com</a> to discard this rating.</p>");
            sb.AppendFormat("<p>Thanks</p>");
            sb.AppendFormat("<p>Team BikeWale</p>");
            return sb.ToString();
        }
    }
}
