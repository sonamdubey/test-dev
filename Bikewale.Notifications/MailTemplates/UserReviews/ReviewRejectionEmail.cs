using System.Text;

namespace Bikewale.Notifications.MailTemplates.UserReviews

{
    /// <summary>
    /// Created by  :  Aditi Srivastava on 14 Apr 2017
    /// Description :  User review rejection email to user
    /// </summary>
    public class ReviewRejectionEmail : ComposeEmailBase
    {
        private string userName,
                      modelName;
        /// <summary>
        /// Initialize the member variables values
        /// </summary>
        public ReviewRejectionEmail(string userName, string modelName)
        {
            this.userName = userName;
            this.modelName = modelName;   
        }
        /// <summary>
        /// Created by  :   Aditi Srivastava on 14 Apr 2017
        /// Description :   Prepares the Email Body when user review is rejected
        /// </summary>
        public override string ComposeBody()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<p>Dear {0},</p>",userName);
            sb.AppendFormat("<p>Your reviews for {0} has been disapproved by our content verification team. ",modelName);
            sb.AppendFormat("We appreciate the effort put in writing this review.</p>");
            sb.AppendFormat("<p>Why are reviews disapproved by our content team?</p>");
            sb.AppendFormat("<ol><li>Abusive/Foul Language</li><li>Language other than English</li>");
            sb.AppendFormat("<li>Irrelevant Content</li><li>Hatred/threat for a dealership</li><li>Fake Reviews</li></ol>");
            sb.AppendFormat("<p>We request you to write a new review about your bike. ");
            sb.AppendFormat("Write back to us at <a href='mailto:contact@bikewale.com'>contact@bikewale.com</a> in case of any query.</p>");
            sb.AppendFormat("<p>Thanks</p>");
            sb.AppendFormat("<p>Team BikeWale</p>");
            return sb.ToString();
        }
    }
}
