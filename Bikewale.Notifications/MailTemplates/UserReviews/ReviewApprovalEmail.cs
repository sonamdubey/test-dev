using System.Text;

namespace Bikewale.Notifications.MailTemplates.UserReviews
{
    /// <summary>
    /// Created by  :  Aditi Srivastava on 14 Apr 2017
    /// Description :  User review approval email to user
    /// </summary>
    public class ReviewApprovalEmail : ComposeEmailBase
    {
        private string userName,
            modelName,
            reviewLink;
        /// <summary>
        /// Initialize the member variables values
        /// </summary>
        public ReviewApprovalEmail(string userName, string reviewLink, string modelName)
        {
            this.userName = userName;
            this.reviewLink = reviewLink;
            this.modelName = modelName;            
        }
        /// <summary>
        /// Created by  :   Aditi Srivastava on 14 Apr 2017
        /// Description :   Prepares the Email Body when user review is approved
        /// </summary>
        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<p>Dear {0},</p>", userName);
            sb.AppendFormat("<p>Your review for {0} has been made available to over 6 million+ users on BikeWale. ",modelName);
            sb.AppendFormat("The prospective buyers have started finding your reviews useful.</p>");
            sb.AppendFormat("<p>Don't forget to share your review with your friends and family. ");
            sb.AppendFormat("Click here to see your review- {0}</p>",reviewLink);
            sb.AppendFormat("<p>Thanks</p>");
            sb.AppendFormat("<p>Team BikeWale</p>");
            return sb.ToString();
        }
    }
}

