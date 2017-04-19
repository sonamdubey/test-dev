using System.Text;

namespace Bikewale.Notifications.MailTemplates.UserReviews
{
    /// <summary>
    /// Created by  :  Aditi Srivastava on 14 Apr 2017
    /// Description :  User review reminder email to user
    /// </summary>
    public class ReviewReminderEmail : ComposeEmailBase
    {
        private string userName,
            makeName,
            modelName,
            reviewLink,
            ratingDate;
        /// <summary>
        /// Initialize the member variables values
        /// </summary>
        public ReviewReminderEmail(string userName, string reviewLink, string ratingDate, string makeName, string modelName)
        {
            this.userName = userName;
            this.makeName = makeName;
            this.modelName = modelName;
            this.reviewLink = reviewLink;
            this.ratingDate = ratingDate;
        }
        /// <summary>
        /// Created by  :   Aditi Srivastava on 14 Apr 2017
        /// Description :   Prepares the Email Body when user needs to be reminded to submit a review after rating
        /// </summary>
        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<p>Dear {0},</p>", userName);
            sb.AppendFormat("<p>You rated your bike {0} on {1}. We have made your ratings available to BikeWale users.</p>", modelName, ratingDate);
            sb.AppendFormat("<p>The prospective buyers look out for user reviews to make their buying decision. ");
            sb.AppendFormat("We request you to write a detailed review about your {0} {1} bike. ", makeName, modelName);
            sb.AppendFormat("Your review will be of great help to someone who is planning to buy the same motorcycle.</p>");
            sb.AppendFormat("<p>To share your {0} {1}'s experience, click here- {2}</p>", makeName, modelName, reviewLink);
            sb.AppendFormat("<p>Looking forward to your detailed review on {0}!</p>", modelName);
            sb.AppendFormat("<p>Thanks</p>");
            sb.AppendFormat("<p>Team BikeWale</p>");
            return sb.ToString();
        }
    }
}

