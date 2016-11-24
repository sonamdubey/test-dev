using Bikewale.Entities.UrlShortner;
using Bikewale.Utility;
using System.Text;

namespace Bikewale.Notifications.MailTemplates.UsedBikes
{
    /// <summary>
    /// Created By Sajal Gupta on 23-11-2016
    /// Description : To send email to seller for expiring time of listing.
    /// </summary>
    public class ExpiringListingReminderEmail : ComposeEmailBase
    {
        private string sellerName, makeName, modelName, repostUrl, remainingTime;
        private int remainingDays;

        public ExpiringListingReminderEmail(string sellerName, string makeName, string modelName, int remainingDays, string repostUrl)
        {
            this.sellerName = sellerName;
            this.makeName = makeName;
            this.modelName = modelName;
            this.remainingDays = remainingDays;
            this.repostUrl = repostUrl;

            if (remainingDays == 1)
                this.remainingTime = "24 hours";
            else
                this.remainingTime = "7 days";
        }

        /// <summary>
        /// Created By Sajal Gupta on 23-11-2016
        /// Desc : Prepares the Email Body for Expiring Listing Reminder.
        /// </summary>
        /// <returns></returns>
        public override string ComposeBody()
        {
            UrlShortnerResponse shortRepostUrl = null;
            shortRepostUrl = new UrlShortner().GetShortUrl(repostUrl);

            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("<p>Hi {0},</p>", sellerName);
            sb.AppendFormat("<p>Your ad posting on BikeWale of {0} {1} will expire in next  {2}. If you have already sold your bike, we request you to delete your Ad.</p>", makeName, modelName, remainingTime);
            sb.AppendFormat("<p>If not sold yet, please re-post your bike Ad with comprehensive bike details and better photos quality. Visit {0} to re-post your Ad.", shortRepostUrl.ShortUrl);

            if (remainingDays == 1)
                sb.AppendFormat("The Ad will not be visible to active buyers after 24 hours if not re-posted.</p>");
            else
                sb.AppendFormat("</p>");

            sb.AppendFormat("<p>Cheers!<br>");
            sb.AppendFormat("Team BikeWale<br>");
            return sb.ToString();
        }
    }
}
