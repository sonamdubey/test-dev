using System.Text;

namespace Bikewale.Notifications.MailTemplates.UserReviews

{
    /// <summary>
    /// Created by  :  Aditi Srivastava on 14 Apr 2017
    /// Description :  User review submission email to user
    /// </summary>
    public class ReviewSubmissionEmail : ComposeEmailBase
    {
        private string userName,
            makeName,
            modelName;
        /// <summary>
        /// Initialize the member variables values
        /// </summary>
        public ReviewSubmissionEmail(string userName, string makeName, string modelName)
        {
            this.userName = userName;
            this.makeName = makeName;
            this.modelName = modelName;   
        }
        /// <summary>
        /// Created by  :   Aditi Srivastava on 14 Apr 2017
        /// Description :   Prepares the Email Body when user review is submitted
        /// </summary>
        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<p>Dear {0},</p>",userName);
            sb.AppendFormat("<p>We thank you for sharing your {0} {1}'s experience on BikeWale! ", makeName, modelName);
            sb.AppendFormat("Your review will be made available to 6 million+ users of BikeWale after verification very soon.</p>");
            sb.AppendFormat("<p>We will inform you once your review is published on BikeWale. ");
            sb.AppendFormat("Don't forget to share your review with friends and family after it is published.</p>");
            sb.AppendFormat("<p>In case you didn't write this review, please write to us at ");
            sb.AppendFormat("<a href='mailto:contact@bikewale.com'>contact@bikewale.com</a> to remove this rating/review.</p>");
            sb.AppendFormat("<p>Thanks</p>");
            sb.AppendFormat("<p>Team BikeWale</p>");
            return sb.ToString();
        }
    }
}
