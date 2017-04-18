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
            makeName,
            modelName;
        /// <summary>
        /// Initialize the member variables values
        /// </summary>
        public RatingSubmissionEmail(string userName, string makeName, string modelName)
        {
            this.userName = userName;
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
            sb.AppendFormat("<p>In case you didn't rate this bike, ", modelName);
            sb.AppendFormat("please write to us at <a href='mailto:contact@bikewale.com'>contact@bikewale.com</a> to remove this rating.</p>");
            sb.AppendFormat("<p>Thanks</p>");
            sb.AppendFormat("<p>Team BikeWale</p>");
            return sb.ToString();
        }
    }
}
